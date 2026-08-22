# 2.5 - Free-Form Rendering
Kasane2D's graphics system already comes with a lot of rendering features the engine does for you, but sometimes there are scenarios that require more freedom and control over how rendering is done. The engine's `IRenderer` provides additional functionality for those advanced rendering scenarios.

Rendering requires a target to render to. The `ITextureSurface` layer type is intended to be the render target for custom rendering scenarios. It integrates seemlessly with the rest of the engine's layer system and even provides the same viewport scrolling features of the other layers.

To do any custom rendering, your implementation of `EngineMain` should override the `Draw()` method. There, your custom rendering code will be called before the engine's main drawing process.
When drawing, `IRenderer.BeginDraw(ITextureSurface target)` initializes the drawing process to the provided render target and `IRenderer.EndDraw()` then submits the draw calls to the GPU.

**Important:** Every call to `BeginDraw()` **has** to be paired with a call to `EndDraw()`!

# Available Draw Commands

`IRenderer.Draw(ITexture src, Rect? dstRect = null, Rect? srcRect = null)` draws a texture to the render target. The `dstRect` and `srcRect` parameters are optional. `dstRect` determines the target area on the render target to draw to while `srcRect` is used to clip the texture for rendering.

`IRenderer.Draw(ISurface src, Rect? dstRect = null, Rect? srcRect = null)` draws the contents of another surface to the render target. The optional `dstRect` and `srcRect` params are analogue to drawing a regular texture.

`IRenderer.Draw(Rect rect, Color color)` draws a rect filled with the given color.

`IRenderer.Draw(Line line, int thickness, Color color)` draws a line with the given thicknes and of the given color.

`IRenderer.Draw(Bezier bezier, int thickness, Color color, int precision = 5)` draws an approximation of the bézier curve with the given thickness and of the given color. The precision parameter indicates the numbers of individual lines that are used to approximate the bézier curve.