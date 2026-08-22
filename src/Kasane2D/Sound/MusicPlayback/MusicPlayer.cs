using System.Collections.Concurrent;
using Kasane2D.Sound.Extensions;
using Kasane2D.Sound.Interfaces;
using Kasane2D.Sound.Types;

namespace Kasane2D.Sound.MusicPlayback;

internal class MusicPlayer : IMusicPlayer
{
    private readonly SemaphoreSlim tlock = new(1, 1);
    private readonly int bufferSize;
    private readonly IMixBus bus;
    private readonly float[] scratchBuffer;
    private readonly ConcurrentQueue<AudioFileStream> songQueue = new();
    private AudioFileStream? currentSong = null;
    private int currentPosition = 0;

    public MusicPlayer(int bufferSize, IAudioMixer mixer)
    {
        this.bufferSize = bufferSize;
        bus = mixer.CreateMixBus("Music Player");
        bus.Gain = -3;
        
        scratchBuffer = new float[bufferSize];
    }

    public bool IsPlaying { get; private set; }

    public bool IsLooping { get; private set; }

    public int QueueLength => songQueue.Count;

    public void Play(AudioFileStream song, bool loop = false)
    {
        tlock.Wait();

        currentPosition = 0;
        IsPlaying = true;
        IsLooping = loop;
        currentSong = song;

        tlock.Release();
    }

    public void Pause()
    {
        tlock.Wait();
        IsPlaying = false;
        tlock.Release();
    }

    public void Resume()
    {
        tlock.Wait();
        IsPlaying = true;
        tlock.Release();
    }

    public void Stop()
    {
        tlock.Wait();

        IsPlaying = false;
        currentPosition = 0;

        tlock.Release();
    }

    public void EndLoop()
    {
        tlock.Wait();
        IsLooping = false;
        tlock.Release();
    }

    public void Queue(AudioFileStream song)
    {
        songQueue.Enqueue(song);
    }

    public void ClearQueue()
    {
        songQueue.Clear();
    }

    public void Update()
    {
        tlock.Wait();

        var zeroBuffer = scratchBuffer.AsSpan();
        zeroBuffer.Clear();

        if (currentSong is null)
        {
            if (!songQueue.TryDequeue(out var song))
            {
                IsPlaying = false;
                bus.WriteLeft(zeroBuffer);
                bus.WriteRight(zeroBuffer);

                tlock.Release();

                return;
            }

            currentPosition = 0;
            currentSong = song;
        }

        if (!IsPlaying)
        {
            bus.WriteLeft(zeroBuffer);
            bus.WriteRight(zeroBuffer);

            tlock.Release();

            return;
        }

        var count = bufferSize;
        if (currentPosition + bufferSize >= currentSong.Length)
        {
            var remaining = currentSong.Length - currentPosition;
            var remainingStream = currentSong.Read(currentPosition, remaining).AsStereoStream();
            
            bus.WriteLeft(remainingStream.GetLeft());
            bus.WriteRight(remainingStream.GetRight());
            count -= remaining;

            currentPosition = 0;
            if (!IsLooping)
            {
                currentSong = null;
                if (songQueue.TryDequeue(out var song))
                {
                    currentSong = song;
                }
            }
        }

        if (currentSong is null)
        {
            var rest = zeroBuffer.Slice(0, count);
            bus.WriteLeft(rest);
            bus.WriteRight(rest);
            
            tlock.Release();
            return;
        }

        var stream = currentSong.Read(currentPosition, count).AsStereoStream();
        bus.WriteLeft(stream.GetLeft());
        bus.WriteRight(stream.GetRight());
        currentPosition += count;

        tlock.Release();
    }
}