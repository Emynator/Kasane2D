# 1.3 - The EngineMain
The `EngineMain` class is the primary entry point and interaction layer where the engine's execution lifecycle interacts with your game code. This is also where you have access to the various subsystems of the engine.

To provide your own main class, you need to create a new class and derive it from `EngineMain`. It's important that you don't provide a constructor. The engine will initialize all subsystems and hook them up to your `EngineMain` before your code is called. For that reason, all required pre running initialization should be handled by overriding the `EngineMain.Init()` method.

# Overridable Methods

## Init()
The `Init()` function is the first function the engine calls after all engine systems are initialized and before the engine enters the main game loop. It is guaranteed that all engine components available in `EngineMain` are initialized and ready to use when `Init()` is called.

## Tick(float dt)
This is the main execution function of your game. It will be called for each iteration of the main loop. The `dt` parameter is (roughly) the amount of seconds that have passed since the last iteration of the game loop, also known as delta time. Instead of relying on a specific framerate, you should use the delta time for all timing sensitive updates like for example movement.

## Draw()
This is the main drawing function of your game. This is the place where you put all custom drawing code that runs in addition to Kasane2D's baseline rendering of layers. This function is called before the engine proceeds with its own drawing process.

**Important:** You should not rely on calls to draw being called in sync with `Tick()`. It is not guaranteed that each call of `Tick()` will result in a successive call of `Draw()`. Calls to `Draw()` are always synced to the game's framerate.

If you don't require custom drawing code, you can completely ignore this function and leave it empty. Otherwise, refer to the [free-form rendering](../graphicsSystem/FreeFormRendering.md) documentation.

# Available Properties

## SoundSystem
This is your access to the engine's sound system. For further information, please refer to the [sound system documentation](../soundSystem/Overview.md).

**Important** The `SoundSystem` property might be null if no sound system is configured by the engine builder!

## Renderer
This is your access to the engine's graphics system. For further information, please refer to the [graphics system documentation](../graphicsSystem/Overview.md).

## InputSystem
This is your access to the engine's input system. For further information, please refer to the [input system documentation](../inputSystem/Overview.md).