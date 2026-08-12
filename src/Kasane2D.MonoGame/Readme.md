# Kasane2D.MonoGame

Kasane2D.MonoGame is the [MonoGame](https://monogame.net) DesktopGL backend for the Kasane2D game engine. It provides the window, game loop, rendering, input, and audio integration needed to run a Kasane2D game.
```shell
dotnet add package Kasane2D.MonoGame
```

Select the backend while configuring the engine:
```C#
var engine = new EngineBuilder()
    .UseMonoGame()
    .ConfigureGraphics(graphicsConfig)
    .ConfigureRenderer(renderLayers)
    .ConfigureAudio()
    .WithMain<MyGame>()
    .Build();

engine.Run();
```

See the [main readme](https://github.com/Emynator/Kasane2D#readme) for a complete configuration example, engine features, and project documentation.