using System.Buffers.Binary;
using Kasane2D.Config;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Input.Interfaces;
using Kasane2D.Interfaces;
using Kasane2D.MonoGame.Graphics;
using Kasane2D.MonoGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Kasane2D.MonoGame;

internal class MonoGameRunner : Game, IEngineRunner
{
    private readonly EngineMain main;
    private readonly GraphicsConfiguration config;
    private readonly Action<IRasterizer> createRenderer;
    private readonly GraphicsDeviceManager graphics;
    private readonly InputSystem inputSystem = new();
    private readonly int sampleRate;
    private readonly int bufferSize;
    private DynamicSoundEffectInstance? audioBackend = null;
    private Action? initRenderer;
    private bool doExit = false;

    public MonoGameRunner
        (
        EngineMain main,
        GraphicsConfiguration config,
        Action<IRasterizer> createRenderer,
        Action<IInputSystem> assignInputSystem,
        AudioConfiguration? audioConfig
        )
    {
        this.main = main;
        this.config = config;
        this.createRenderer = createRenderer;
        sampleRate = audioConfig?.SampleRate ?? 0;
        bufferSize = audioConfig is null ? 0 : (int)(audioConfig.SampleRate / 1000.0f * audioConfig.DefaultBufferSizeInMs);
        assignInputSystem(inputSystem);
        
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = config.ScreenSize.X;
        graphics.PreferredBackBufferHeight = config.ScreenSize.Y;
    }

    protected override void LoadContent()
    {
        var rasterizer = new Rasterizer(config, GraphicsDevice);
        createRenderer(rasterizer);
        initRenderer?.Invoke();
        if (main.SoundSystem is not null)
        {
            audioBackend = new(sampleRate, AudioChannels.Stereo);
            audioBackend.BufferNeeded += UpdateAudioBuffer;
            audioBackend.Play();
        }
        main.Init();
    }

    protected override void Update(GameTime gameTime)
    {
        if (doExit)
        {
            Exit();
        }

        inputSystem.Update();
        main.MainTick((float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f));
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        main.MainDraw();

        base.Draw(gameTime);
    }

    public void Init(Action initRenderer)
    {
        this.initRenderer = initRenderer;
    }

    public void Stop()
    {
        doExit = true;
    }

    private void UpdateAudioBuffer(object? sender, EventArgs e)
    {
        if (audioBackend is null || main.SoundSystem is null)
        {
            throw new InvalidOperationException("Sound system is not initialized.");
        }
        
        main.SoundSystem.Process(bufferSize);
        
        var leftFloat = main.SoundSystem.AudioMixer.Master.ReadLeft(bufferSize);
        var rightFloat = main.SoundSystem.AudioMixer.Master.ReadRight(bufferSize);
        var left = leftFloat
            .Select(s => s >= 0.0f ? MathF.Min(1.0f, s) : MathF.Max(-1.0f, s))
            .Select(s => (short)(s * (s >= 0.0f ? 32767.0f : 32768.0f)))
            .ToArray();
        var right = rightFloat
            .Select(s => s >= 0.0f ? MathF.Min(1.0f, s) : MathF.Max(-1.0f, s))
            .Select(s => (short)(s * (s >= 0.0f ? 32767.0f : 32768.0f)))
            .ToArray();
            
        var buffer = new byte[bufferSize * 4];
        for (var i = 0; i < bufferSize; i++)
        {
            BinaryPrimitives.WriteInt16LittleEndian(buffer.AsSpan(i * 4, 2), left[i]);
            BinaryPrimitives.WriteInt16LittleEndian(buffer.AsSpan(i * 4 + 2, 2), right[i]);
        }
            
        audioBackend.SubmitBuffer(buffer);
    }
}