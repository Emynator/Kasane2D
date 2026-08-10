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
    private readonly float[] scratchBuffer0;
    private readonly float[] scratchBuffer1;
    private readonly ConcurrentQueue<StereoAudioStream> musicQueue = new();
    private StereoAudioStream? currentStream = null;
    private int currentPosition = 0;

    public MusicPlayer(int bufferSize, IAudioMixer mixer)
    {
        this.bufferSize = bufferSize;
        bus = mixer.CreateMixBus("Music Player");
        bus.Level = -3;
        
        scratchBuffer0 = new float[bufferSize];
        scratchBuffer1 = new float[bufferSize];
    }

    public bool IsPlaying { get; private set; }

    public bool IsLooping { get; private set; }

    public int QueueLength => musicQueue.Count;

    public void Play(AudioFileStream song, bool loop = false)
    {
        tlock.Wait();

        currentPosition = 0;
        IsPlaying = true;
        currentStream = song.ReadIn();

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
        musicQueue.Enqueue(song.ReadIn());
    }

    public void ClearQueue()
    {
        musicQueue.Clear();
    }

    public void Update()
    {
        tlock.Wait();

        var left = scratchBuffer0.AsSpan();
        left.Clear();

        if (currentStream is null)
        {
            if (!musicQueue.TryDequeue(out var stream))
            {
                IsPlaying = false;
                bus.WriteLeft(left);
                bus.WriteRight(left);

                tlock.Release();

                return;
            }

            currentPosition = 0;
            currentStream = stream;
        }

        if (!IsPlaying)
        {
            bus.WriteLeft(left);
            bus.WriteRight(left);

            tlock.Release();

            return;
        }

        var right = scratchBuffer1.AsSpan();
        right.Clear();
        var count = bufferSize;
        if (currentPosition + bufferSize >= currentStream.Length)
        {
            var remaining = currentStream.Length - currentPosition;
            currentStream.GetLeft().Slice(currentPosition, remaining).CopyTo(left);
            currentStream.GetRight().Slice(currentPosition, remaining).CopyTo(right);

            count = currentStream.Length - currentPosition;
            left = left.Slice(remaining, count);
            right = right.Slice(remaining, count);
            currentPosition = 0;

            if (!IsLooping)
            {
                currentStream = null;
                if (musicQueue.TryDequeue(out var stream))
                {
                    currentStream = stream;
                }
            }
        }

        if (currentStream is not null)
        {
            currentStream.GetLeft().Slice(currentPosition, count).CopyTo(left);
            currentStream.GetRight().Slice(currentPosition, count).CopyTo(right);
        }

        currentPosition += count;
        bus.WriteLeft(left);
        bus.WriteRight(right);

        tlock.Release();
    }
}