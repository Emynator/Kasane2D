# 1.2 - Engine Configuration
During setup, there are multiple required and some optional configurations that need to be provided for the engine. Those configurations are split over 3 distinct config types.

# Graphics Configuration
The graphics configuration configures Kasane2D's graphics system except for the layers.

`IsMouseVisibible` configures if the mouse is visible in the window or not. It defaults to false.

`DefaultTileSize` configures the default tile size in pixels. This is the default value all tilemap surfaces will use if no custom override is provided in the specific layer configuration. The default value is 16x16 pixels.

`DefaultTilemapDimensions` configures the default number of tile rows and columns of a tilemap surface. This is the default value all tilemap surfaces will use if no custom override is provided in the specific layer configuration. The default value is 32x32 tiles.

`DefaultSpriteSize` configures the default sprite size in pixels. This is the default value all sprite layers will use if no custom override is provided in the specific layer configuration. The default value is 16x16 pixels.

`DefaultSurfaceSize` configures the default surface size in pixels for all surface types that don't have specific size configurations (like texture surfaces for example). It is only used if no custom override is provided in the specific layer configuration. The default value is 512x512 pixels.

`DefaultSpriteCount` configures the default number of sprite slots for a sprite layer. This is the default value all sprite layers will use if no custom override is provided in the specific layer configuration. The default value is 64 sprite slots.

`ViewportSize` configures the size of the game's viewport in pixels. This is the internal "native resolution" the game runs at so to speak. This value is independent of the actual screen resolution the game will be rendered to. Meaning, the viewport size provides a resolution independent unit of measurement for sizes in your game.

`ScreenSize` configures the actual rendering resolution of the game window in pixels. The viewport will be upscaled to this resolution for rendering in a manner that pixel art is rendered crispy instead of getting blurred.

# Render Layer Configuration
The render layer configuration is used to configure the available rendering layers of your game. The `EngineBuilder`'s `ConfigureRenderer()` method expects an `ICollection` of `RenderLayerConfig`s. Each `RenderLayerConfig` configures a single rendering layer.

## RenderLayerConfig Properties

`Name` configures the name of the layer. This property is required for all layers. The name is used to later retrieve the layer from the `IRenderer` in your `EngineMain` implementation. It can be useful to configure it with a const to prevent any exceptions from typos.

`Type` configures the type of that layer. This property is required for all layers. The currently available types are tilemap surfaces, sprite layers, and texture surfaces.

`SortingOrder` configures the sorting order of the layer. See below for an in-depth explanation. Default is -1.

`TileSize` configures the tile size in pixels. This is only used for tilemap surfaces and only needed if you want this layer to have a different tile size than the default size configured in the graphics configuration.

`SpriteSize` configures the sprite size in pixels. This is only used for sprite layers and only needed if you want this layer to have a different sprite size than the default size configured in the graphics configuration.

`Dimensions` configures the dimensions of this layer. The surface type determines what this value means. For tilemap surfaces, this is the number of tile rows and columns. For texture surfaces, this is the size of the surface in pixels. This is only needed if you want this layer to have a different dimension than the default one configured in the graphics configuration.

`SpriteCount` configures the number of sprite slots. This is only used for sprite layers and only needed if you want this layer to have a different number of sprite slots than the default number configured in the graphics configuration.

## Sorting Order of Render Layers
Kasane2D renders its layers from back to front, with higher layers drawing over the layers below them. For that reason, the ordering of your layers is important. The default value for the sorting order of every layer configuration is -1. Layers with a configured sorting order < 0 means that this layer does not have a configured set position value. The sorting order is intended to allow a fine grain control for more complex layer sorting scenarios.

To understand the behavior of this value, let's take a look at the default case. We call `ConfigureRenderer()` with the following list:
```C#
[
    new()
    {
        Name = BG0,
        Type = LayerType.Tilemap,
    },
    new()
    {
        Name = BG1,
        Type = LayerType.Tilemap,
    },
    new()
    {
        Name = BG_Sprites,
        Type = LayerType.Sprite,
    },
    new()
    {
        Name = BG2,
        Type = LayerType.Tilemap,
    },
    new()
    {
        Name = Sprites,
        Type = LayerType.Sprite,
    },
]
```

In this case, none of the layers have a sorting order configured. This means that the layers will be sorted from back to front in the order they are defined in the configuration. So "BG0" is the layer furthest back, "BG1" is on top of "BG0", "BG_Sprites" is on top of "BG0" and "BG1", etc.

If we now configure "BG_Sprites" with a sorting order of 0, then "BG_Sprites" will be the layer furthest back. "BG0" will now be on top of "BG_Sprites", "BG1" will be on top of "BG_Sprites" and "BG0", etc.

This means that the sorting order is intended to be an override for the default behavior of using the order of the configurations in the call. What the engine actually does on initialization is the following: it seperates all layers that have a sorting order >= 0 defined and treats all layers with a sorting order < 0 as filler layers.
As long as we still have layers to sort, we tick up an internal counter. If we have a layer configured for the current number, we slot that layer in. Otherwise, we take a filler layer and put that in this place.
If we have no more layers with a sorting order >= 0 available, we slot in the remaining filler layers in order.

In case that more than one layer is defined with the same sorting order >= 0, their ordering in the configuration list wins again and they are all put in in order.

# Audio Configuration
Kasane2D's sound system only gets configured when `EngineBuilder.ConfigureAudio()` is called. Otherwise, the sound system is deactivated.
The method takes an optional `AudioConfiguration` parameter. If none is provided, the default values will be used.

## AudioConfiguration Properties

`BufferSizeInMs` configures the size of a single audio buffer in milliseconds that the sound system processes in each step. Default is 15ms.

`BuffersInQueue` configures the number of buffers that the sound system preprocesses at most before the backend requests another buffer. Default is 4.

`SampleRate` configures the sample rate of the sound system in hertz. Default is 44.1 kHz.

`SfxChannelCount` configures the number of sound effects that the sound effect manager can play at the same time. Default is 32.

## Important Information
The available sample rates depend on the sample rates the backend implementation supports. If a sample rate is configured that is not supported by the backend, an exception will be thrown on startup providing information about the supported sample rates.

Usually, you only would want to configure the sample rate and sound effect channel count if the default values are insufficient for you. The configuration of the buffer size and buffers queued is an advanced topic. Both values directly influence the audio latency. Longer buffers and more buffers in the queue mean that it will take more time until the audio data reaches the output device. Shorter values mean that the audio data reaches the output device faster. But if the values are too low and buffers are consumed faster by the backend then the sound system can process them, you will encounter sound glitches.
Therefore, tweaking of these values requires careful testing and experimentation.