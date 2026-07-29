using Kasane2D.Graphics.Types;

namespace Kasane2D.Graphics.Interfaces;

public interface ISlotManager
{
    public ISpriteLayer SpriteLayer { get; }

    public bool GetSlot(out SpriteSlot? result);

    public void FreeSlot(SpriteSlot slot);
}