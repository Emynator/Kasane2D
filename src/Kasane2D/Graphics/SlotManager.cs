using System.Diagnostics.CodeAnalysis;
using Kasane2D.Graphics.Interfaces;
using Kasane2D.Graphics.Types;

namespace Kasane2D.Graphics;

internal class SlotManager : ISlotManager
{
    private readonly SemaphoreSlim tlock = new(1, 1);
    private readonly SpriteSlot?[] slots;
    
    public SlotManager(ISpriteLayer layer)
    {
        SpriteLayer = layer;
        slots = new SpriteSlot[layer.Sprites.Length];
        for (var i = 0; i < layer.Sprites.Length; i++)
        {
            slots[i] = null;
        }
    }
    
    public ISpriteLayer SpriteLayer { get; }
    
    public bool GetSlot([NotNullWhen(true)] out SpriteSlot? result)
    {
        tlock.Wait();
        
        for (var i = 0; i < slots.Length; i++)
        {
            if (slots[i] is not null)
            {
                continue;
            }

            result = new(SpriteLayer, i);
            slots[i] = result;
            
            tlock.Release();
            
            return true;
        }
        
        tlock.Release();
        result = null;

        return false;
    }

    public void FreeSlot(SpriteSlot slot)
    {
        tlock.Wait();
        
        slots[slot.Index] = null;
        slot.Freed = true;
        
        tlock.Release();
    }
}