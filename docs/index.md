---
_layout: landing
---

# Kasane2D
Welcome to the documentation hub for Kasane2D!

Kasane2D is a 2D game engine inspired by the programming model of retro graphics hardware. It provides the same kinds of tools as the sprite and tile engines of the 8- and 16-bit eras, without reproducing their hardware limitations, quirks, or compromise-driven constraints.
An optional real-time synthesis engine adds tracker-inspired music sequencing and procedural music generation.
All of this is exposed through a low-boilerplate, code-first programming model built around modern, idiomatic C# and .NET 10.

Kasane2D is available under the MIT license. The source code is available [on GitHub](https://github.com/Emynator/Kasane2D).

# Why Kasane2D?
- **Retro-inspired rendering without retro hardware limitations.**
  Kasane2D borrows the programming model of classic sprite and tile-based hardware: configurable render layers, tilemaps, sprite layers, independently scrollable viewports, and explicit control over how the screen is composed. You get the simplicity and predictability of that model without palette restrictions, scanline sprite limits, bitplanes, or fixed hardware layouts. No magic control-register addresses, cryptic bit flags, DMA routines, or HBlank/VBlank timing gymnastics required.
- **Code-first and explicit by design.**
  Kasane2D avoids scene editors, opaque object hierarchies, and framework-heavy boilerplate. Engine systems are configured directly in C#, and game code interacts with them through small, focused APIs. The engine provides the machinery while leaving structure and architecture in the hands of the developer.
- **Audio is a first-class engine system.**
  Instead of treating audio as little more than `PlaySound()`, Kasane2D includes a hierarchical software mixer with nested buses, dBFS gain, panning, runtime routing, and per-bus effect chains. Games can manipulate the audio graph dynamically just like any other engine system.
- **Start simple, opt into complexity when you want it.**
  The core engine stays deliberately lightweight. Straightforward games can use ordinary sprite rendering, sound effects, and prerecorded music without touching anything more advanced. Optional packages such as `Kasane2D.Music` add tracker-inspired sequencing, synthesis, procedural music, and deep runtime control without making those features mandatory for everyone else.