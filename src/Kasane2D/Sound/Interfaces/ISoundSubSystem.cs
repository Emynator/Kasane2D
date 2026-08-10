namespace Kasane2D.Sound.Interfaces;

public interface ISoundSubSystem
{
    public Guid Id { get; }
    
    public void Process();
}