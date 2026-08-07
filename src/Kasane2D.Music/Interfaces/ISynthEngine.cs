using Kasane2D.Music.Types;
using Kasane2D.Sound.Interfaces;

namespace Kasane2D.Music.Interfaces;

public interface ISynthEngine : ISoundSubSystem
{
    public void Play(SongPattern pattern);
    
    public void Queue(SongPattern pattern);

    public void Pause();

    public void Resume();

    public void Stop();
}