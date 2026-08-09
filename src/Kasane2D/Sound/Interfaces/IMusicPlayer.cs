using Kasane2D.Sound.Types;

namespace Kasane2D.Sound.Interfaces;

public interface IMusicPlayer
{
    public bool IsPlaying { get; }
    
    public bool IsLooping { get; }
    
    public int QueueLength { get; }
    
    public void Play(AudioFileStream song, bool loop = false);

    public void Pause();

    public void Resume();

    public void Stop();

    public void EndLoop();
    
    public void Queue(AudioFileStream song);

    public void ClearQueue();
}