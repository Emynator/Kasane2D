# 2.3 - Accessing the Graphics System
Access to Kasane2D's graphics system is available through the `IRenderer` interface available at `EngineMain.Renderer`. The renderer provides access to the different rendering layers. In addition, it contains an `ITextureManager` to load and manage various graphics resources.
The renderer also provides access to the methods required for free-form rendering. Those will be explained in [the respective documentation](FreeFormRendering.md).

# IRenderer
You can use `IRenderer.ClearColor` to change the clear color of the background. This is the color used to clear the backbuffer with before the layers are drawn on top of it.

You can use `IRenderer.TextureManager` to access the texture manager.

`IRenderer.GetSurface<T>(string name)` is used to return the render surface of the respective name.
`T` is the respective type of the surface (for example an `ITilemapSurface`). This is used to retrieve all surfaces with the exception of sprite layers.
The `name` parameter is the configured name of the layer. See [the configuration documentation](../gettingStarted/EngineConfiguration.md) for details.

`IRenderer.GetSpriteLayer(string name)` is used to return the sprite layer of the respective name. The `name` parameter is the confogured name of the sprite layer. See [the configuration documentation](../gettingStarted/EngineConfiguration.md) for details.

`IRenderer.GetSlotManager(string layerName)` is used to return the `ISlotManager` that is used to manage sprite slots for a sprite layer. The `layerName` parameter refers to the configured name of the sprite layer. See [the configuration documentation](../gettingStarted/EngineConfiguration.md) for details.

# ITextureManager
The texture manager is used to load and manage graphics resources, primarily textures and sprite atlases. You can either load a texture by providing the (relative) path to the image file, or you can create an empty texture of the given size in pixels.

A sprite atlas is, in the end, also just a normal texture. You can load one from an image file or create an empty one with the given size in pixels.
Unlike a normal texture, a sprite atlas is used to access many individual sprites packed into one larger texture. The engine automatically handles the slicing and addressing the respective subsections of the texture during rendering. For that reason, you also need to provide the size of an individual sprite in the atlas in pixels. Once loaded, the number of rows and columns in the sprite atlas is available in `ISpriteAtlas.Dimensions`.
There is also an option available to load a sprite atlas from an image and set the number of rows and columns yourself. This is useful, if you want to load a sprite atlas, but want to clamp the number of available sprite rows and columns to a value lower than what is contained in the image.

**Important:** Textures and sprite atlases allocate graphics resources. When they are no longer needed, they need to be freed by calling `ITextureManager.FreeTexture()` or `ITextureManager.FreeSpriteAtlas()` respectively. Otherwise, this leads to memory leaks!

# ITexture
A texture provides its size in pixels. In addition, you can get a copy of the two dimensional array of the texture's color data as well as set the texture's color data manually.