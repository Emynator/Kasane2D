# 2.1 - Graphics System Overview
Kasane2D's graphics and rendering system is centered around the concept of composing a final image from various layers. Anyone familiar with how for example the NES's or SNES's PPUs work will feel right at home.
Unlike the hardware sprite engines of the retro consoles, Kasane2D's layer system is freely configurable to your specific needs and requirements while also exposing sane APIs instead of having to poke magic addresses.

# Interacting with the graphics system
Unless you are performaning additional free-form rendering, you usually don't have to handle the drawing of the various layers. You update the layer's state and the engine takes care of drawing the final image to the screen.

**Important notice:** The coordinate system of Kasane2D's graphics system is oriented similar to screen coordinates. This means that means that X-coordinates grow when going right, and shrink when going left, while Y-coordinates grow when going down, and shrink when going up.
**In other words: compared to the standard representation of the Cartesian coordinate system, the Y-axis is inverted!**

Most of the layers consist of a surface that can be larger than the size of the viewport. If this is the case, the viewport can be scrolled across the layer. While scrolling, the viewports wrap around on both axis and never leave the actual surface.

Take this example:
You have a viewport with a resolution of 320x240 pixels and a tilemap that consists of 32x32 tiles with each tile being 16x16 pixels in size. This means the actual surface of that layer has a size of 512x512 pixels.
If your viewport's position is at (0, 0), then the resulting image is the surface's image from (0, 0) to (319, 239).
If you now scroll the viewport to position (400, 150) then the left side of the screen is the surface's image from (400, 150) to (511, 389) while the right side of the screen is the surface's image from (0, 150) to (207, 389).

The viewport's of each surface are scrolled completely independent from each other. By scrolling multiple layers at a different speed, you achieve effortless parallax scrolling effects.

For more in-depth information of each layer type, please checkout the respective layer's documentation.

# How the final image is composed
On startup, the layer structure of the engine's renderer is configured. The [engine configuration](../gettingStarted/EngineConfiguration.md) documentation explains in depth how to configure your layer layout.

For rendering each frame, the engine goes through the following process:
1) `EngineMain.Draw()` is called to perform all the custom free-form rendering.
2) The engine will render each remaining layer and clip them to their current viewport.
3) The backbuffer is cleared with the set clear color.
4) Each layer's clipped viewport is drawn onto the engine's backbuffer in sequence from back to front to compose the final image.
5) The final image is upscaled to the actual screen size and then drawn to the actual screen buffer. The upscaling is a 2-step process to keep the intended pixel-artwork intact without introducing upscale blurr:
    1) The image is upscaled by the largest integer factor that results in resolution <= the screen resolution using point filtering.
    2) The image is upscaled and drawn to the screen buffer using linear filtering.