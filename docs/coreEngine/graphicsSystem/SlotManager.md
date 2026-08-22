# 2.4 - Managing sprite slots with ISlotManager
A sprite layer provides direct access to the array of all the sprites contained in the layer. Directly manipulating the entries of this array works, but it quickly becomes tedious to keep track of what sprites in the layer are used for what when dealing with a lot of sprites.

The `ISlotManager` is a useful helper for taking this load away from you. A slot manager is always associated with one specific sprite layer. There is also only one slot manager that exists for each sprite layer.
Instead of manipulating the sprites in the layer's sprite array directly, you can request `SpriteSlot` objects from the slot manager. A sprite slot is just a reference to the slot's sprite in the sprite array. Changing the slot's properties directly changes the properties of the slot's underlying sprite.

Since there is only a limited number of sprites in a sprite layer, the slot might not be able to return a valid sprite slot when no slot is available. If that is the case, `ISlotManager.GetSlot()` returns false.

To not run out of available sprite slots, you should return no longer needed slots to the slot manager with `ISlotManager.FreeSlot()`.