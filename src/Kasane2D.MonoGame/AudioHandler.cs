using System.Buffers.Binary;
using System.Collections.Concurrent;
using Kasane2D.Sound.Interfaces;
using Microsoft.Xna.Framework.Audio;

namespace Kasane2D.MonoGame;

internal class AudioHandler : IDisposable
{
    private const string systemKey = "Backend::AudioHandler::BufferRefill";
    
    private readonly ISoundSystem soundSystem;
    private readonly DynamicSoundEffectInstance audioBackend;
    private readonly float[] leftBuffer;
    private readonly float[] rightBuffer;
    private readonly CancellationTokenSource cts = new();
    private readonly CancellationToken ct;
    private readonly EventWaitHandle threadExitedHandle = new(false, EventResetMode.AutoReset);
    private readonly EventWaitHandle bufferAvailable = new(true, EventResetMode.AutoReset);
    private readonly ConcurrentQueue<Memory<byte>> engineBufferQueue = new();
    private readonly ConcurrentQueue<Memory<byte>> backendBufferQueue = new();

    public AudioHandler(ISoundSystem soundSystem, int buffersInQueue)
    {
        this.soundSystem = soundSystem;
        audioBackend = new(soundSystem.SampleRate, AudioChannels.Stereo);
        leftBuffer = new float[soundSystem.BufferSize];
        rightBuffer = new float[soundSystem.BufferSize];
        var backendBufferSize = soundSystem.BufferSize * sizeof(short) * 2;
        var audioBuffer1 = new byte[backendBufferSize * buffersInQueue];
        for (var i = 0; i < buffersInQueue; i++)
        {
            engineBufferQueue.Enqueue(audioBuffer1.AsMemory(i * backendBufferSize, backendBufferSize));
        }

        ct = cts.Token;
        var audioThread1 = new Thread(AudioThreadMain);
        audioThread1.Start();

        audioBackend.BufferNeeded += BufferRefillHandler;
        audioBackend.Play();
    }

    public void Dispose()
    {
        cts.Cancel();
        threadExitedHandle.WaitOne();
    }

    private void BufferRefillHandler(object? sender, EventArgs e)
    {
        Engine.Monitor.StartMeasurement(systemKey);
        
        while (true)
        {
            if (!backendBufferQueue.TryDequeue(out var buffer))
            {
                continue;
            }
            
            audioBackend.SubmitBuffer(buffer.ToArray());
            engineBufferQueue.Enqueue(buffer);
            bufferAvailable.Set();
            break;
        }
        
        Engine.Monitor.FinishMeasurement(systemKey);
    }

    private void AudioThreadMain()
    {
        while (!ct.IsCancellationRequested)
        {
            bufferAvailable.WaitOne();
            if (!engineBufferQueue.TryDequeue(out var buffer))
            {
                bufferAvailable.Reset();
                continue;
            }

            UpdateAudioBuffer(buffer.Span);
            backendBufferQueue.Enqueue(buffer);
        }

        threadExitedHandle.Set();
    }

    private void UpdateAudioBuffer(Span<byte> buffer)
    {
        soundSystem.Process();

        soundSystem.AudioMixer.Master.ReadLeft(leftBuffer);
        soundSystem.AudioMixer.Master.ReadRight(rightBuffer);
        var left = leftBuffer
            .Select(s => s >= 0.0f ? MathF.Min(1.0f, s) : MathF.Max(-1.0f, s))
            .Select(s => (short)(s * (s >= 0.0f ? 32767.0f : 32768.0f)))
            .ToArray();
        var right = rightBuffer
            .Select(s => s >= 0.0f ? MathF.Min(1.0f, s) : MathF.Max(-1.0f, s))
            .Select(s => (short)(s * (s >= 0.0f ? 32767.0f : 32768.0f)))
            .ToArray();

        for (var i = 0; i < leftBuffer.Length; i++)
        {
            BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(i * 4, 2), left[i]);
            BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(i * 4 + 2, 2), right[i]);
        }
    }
}