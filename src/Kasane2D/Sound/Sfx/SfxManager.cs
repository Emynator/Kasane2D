using Kasane2D.Config;
using Kasane2D.Sound.Extensions;
using Kasane2D.Sound.Interfaces;
using Kasane2D.Sound.Types;

namespace Kasane2D.Sound.Sfx;

internal class SfxManager : ISfxManager
{
    private readonly SemaphoreSlim tlock = new(1, 1);
    private readonly int bufferSize;
    private readonly List<SfxChannel> channels = [];
    private readonly Queue<AudioStream> soundQueue = new();
    
    public SfxManager(AudioConfiguration config, int bufferSize, IAudioMixer mixer)
    {
        this.bufferSize = bufferSize;
        ChannelCount = config.SfxChannelCount;
        var sfxBus = mixer.CreateMixBus("SFX");
        sfxBus.Level = -3;
        for (var i = 0; i < ChannelCount; i++)
        {
            channels.Add(new(mixer.CreateMixBus($"SFX Channel {i}", sfxBus), bufferSize));
        }
    }

    public int ChannelCount { get; }

    public int BusyChannels => channels.Count(c => c.CurrentFile is not null);
    
    public bool AllChannelsBusy => BusyChannels == ChannelCount;

    public int QueueLength => soundQueue.Count;
    
    public void Play(AudioFileStream sound)
    {
        tlock.Wait();

        var stream = sound.Read(sound.Length);
        var availableChannel = channels.FirstOrDefault(c => c.CurrentFile is null);
        if (availableChannel is not null)
        {
            availableChannel.CurrentFile = stream;

            tlock.Release();
            return;
        }
        
        soundQueue.Enqueue(stream);
        
        tlock.Release();
    }

    public void StopAll()
    {
        tlock.Wait();
        
        foreach (var channel in channels)
        {
            channel.CurrentFile = null;
        }
        
        tlock.Release();
    }

    public void DropQueue()
    {
        tlock.Wait();
        soundQueue.Clear();
        tlock.Release();
    }

    public void Update()
    {
        tlock.Wait();

        Parallel.ForEach(channels, c => c.Update());
        while (soundQueue.Count > 0)
        {
            var availableChannel = channels.FirstOrDefault(c => c.CurrentFile is null);
            if (availableChannel is null)
            {
                break;
            }
            
            availableChannel.CurrentFile = soundQueue.Dequeue();
        }
        
        tlock.Release();
    }
}