using Kasane2D.Sound.Types;

namespace Kasane2D.Sound.Interfaces;

public interface ISfxManager
{
    public int ChannelCount { get; }
    
    public int BusyChannels { get; }
    
    public bool AllChannelsBusy { get; }
    
    public int QueueLength { get; }
    
    public void Play(AudioFileStream sound);

    public void StopAll();

    public void DropQueue();
}