# Kasane2D

Kasane2D is a code-first 2D game engine for .NET 10. It combines retro-inspired tile and sprite rendering with input, sound-effect and music playback, and a software mixer built around nested mix buses, dBFS gain, stereo panning, and per-bus effects.

This package contains the core engine APIs. To run a game, a backend implementation is required. Kasane2D contains a [MonoGame](https://monogame.net) based reference implementation with `Kasane2D.MonoGame`.

```shell
dotnet add package Kasane2D
```

Engine setup is explicit and code-first:

```C#
var builder = new EngineBuilder()
    .ConfigureGraphics(graphicsConfig)
    .ConfigureRenderer(renderLayers)
    .ConfigureAudio()
    .WithMain<MyGame>();
```

A backend must be selected for execution. See the [main readme](https://github.com/Emynator/Kasane2D#read) for installation guidance, complete setup examples, features, and project documentation.