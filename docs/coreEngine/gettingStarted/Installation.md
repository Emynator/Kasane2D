# 1.1 - Installation and Quickstart
Kasane2D is available on [NuGet](https://www.nuget.org/packages/Kasane2D/). To use the engine, you'll also need a backend to integrate with the core engine. Kasane2D comes with a [MonoGame](https://monogame.net) based backend implementation, also available on [NuGet](https://www.nuget.org/packages/Kasane2D.MonoGame/).
To get started, you need a [].NET 10](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) or newer SDK installed.

# HelloKasane step by step
Setup a .NET console application, either via your IDE or via terminal:
```
dotnet new console --framework "net10.0" -o HelloKasane
cd HelloKasane
```

Add the required NuGet packages:
```
dotnet package add Kasane2D
dotnet package add Kasane2D.MonoGame
```

Now you can start configuring Kasane2D. Delete the existing hello world code from your Program.cs until only the main remains:
```C#
namespace HelloKasane;

public static class Program
{
    public static void Main(string[] args)
    {
    }
}
```

Now it's time to configure Kasane! First, we need to create an engine builder that configures and builds the engine entry point for us.
```C#
var builder = new EngineBuilder();
```

We can now start configuring the engine. At first, let's configure the engine to use the MonoGame backend.
```C#
builder.UseMonoGame();
```

Let's configure the graphics next. We just want to setup our defaults for sprite and tilesizes and the viewport and screen sizes.
```C#
builder.ConfigureGraphics(new()
{
    DefaultTileSize = new(16, 16),
    DefaultSpriteSize = new(16, 16),
    ViewportSize = new(320, 240),
    ScreenSize = new(960, 720),
});
```

To get something rendered to the screen, we also should configure our renderer and give it a configuration for our rendering layers.
Since this is just a simple game to get something moving across the screen, we only need a single sprite layer.
```C#
builder.ConfigureRenderer
(
    [
        new()
        {
            Name = "Sprites",
            Type = LayerType.Sprite,
            SpriteCount = 64,
        },
    ]
);
```

Now we need to configure the entry point for our own code. This is where our game logic will sit.
To do that, we need to create a new class. You can put it into the Program.cs or create a new file for it.
```C#
public class HelloKasane : EngineMain
{
    public override void Init()
    {
    }
    
    protected override void Tick(float dt)
    {
    }
}
```

As you can see, our class derives from the abstract `EngineMain` class. We only need to provide the initialization code and the tick code. For an in depth explanation, you can go to the [EngineMain docs](EngineMain.md).
For now, let's just insert some basic code to get something moving across the screen. Please refer to the respective documentations if you are curious about understanding the code.
```C#
public class HelloKasane : EngineMain
{
    private const float speed = 100.0f;
    private ISpriteLayer spriteLayer = null!;
    private Vec2F position = new(160.0f, 120.0f);
    
    public override void Init()
    {
        spriteLayer = Renderer.GetSpriteLayer("Sprites");
        var atlas = Renderer.TextureManager.CreateSpriteAtlas(spriteLayer.SpriteSize, "assets/square.png");
        spriteLayer.Sprites[0].SpriteAtlas = atlas;
        spriteLayer.Sprites[0].Position = position.ToVec2I();
        spriteLayer.Sprites[0].IsActive = true;
    }
    
    protected override void Tick(float dt)
    {
        var movement = Vec2F.Zero;
        if (InputSystem.IsKeyDown(KeyKind.Up))
        {
            movement += Vec2F.Up;
        }
        if (InputSystem.IsKeyDown(KeyKind.Down))
        {
            movement += Vec2F.Down;
        }
        if (InputSystem.IsKeyDown(KeyKind.Left))
        {
            movement += Vec2F.Left;
        }
        if (InputSystem.IsKeyDown(KeyKind.Right))
        {
            movement += Vec2F.Right;
        }
        
        movement *= speed * dt;
        position += movement;

        if (position.Y < -spriteLayer.SpriteSize.Y)
        {
            position.Y = 240.0f + spriteLayer.SpriteSize.Y;
        }
        if (position.Y > 240.0f + spriteLayer.SpriteSize.Y)
        {
            position.Y = -spriteLayer.SpriteSize.Y;
        }
        if (position.X < -spriteLayer.SpriteSize.X)
        {
            position.X = 320.0f + spriteLayer.SpriteSize.X;
        }
        if (position.X > 320.0f + spriteLayer.SpriteSize.X)
        {
            position.X = -spriteLayer.SpriteSize.X;
        }
        
        spriteLayer.Sprites[0].Position = position.ToVec2I();
    }
}
```

Now we can tell the engine builder in `Main()` about our entry point.
```C#
builder.WithMain<HelloKasane>();
```

With everything configured, we can build the actual engine and run it!
```C#
var engine = builder.Build();

engine.Run();
```

If we build and run our app now, we'll get an error message, though. What happened? We forgot to add our assets! We ask the engine to render a square, but the file is missing!
We *could* create an asset folder in the output directory and put our `square.png` there, but we'd need to repeat that for a release build, too. Thankfully, .NET already provides a better way for us to handle that. Let's create a folder called `assets` in our project directory. Then we'll add the following to the `HelloKasane.csproj`:
```XML
<ItemGroup>
    <Folder Include="assets\" />
</ItemGroup>

<ItemGroup>
    <None Update="assets\square.png">
        <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
</ItemGroup>
```

The only missing thing now is to add the actual image file. You can either create your own 16 by 16 pixel square or just [download the one from the samples folder](https://github.com/Emynator/Kasane2D/blob/main/samples/MinimalSample/assets/square.png).

And that's it! You now have a white square you can move with the arrow keys across the screen!