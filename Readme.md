# Kasane2D
Kasane2D is a 2D game engine inspired by the programming model of retro graphics hardware. It provides the same kinds of tools as the sprite and tile engines of the 8- and 16-bit eras, without reproducing their hardware limitations, quirks, or compromise-driven constraints.
An optional real-time synthesis engine adds tracker-inspired music sequencing and procedural music generation.
All of this is exposed through a low-boilerplate, code-first programming model built around modern, idiomatic C# and .NET 10.

# TODO: small code sample

# Why Kasane2D?
- **Retro-inspired rendering without retro hardware limitations.**
  Kasane2D borrows the programming model of classic sprite and tile-based hardware: configurable render layers, tilemaps, sprite layers, independently scrollable viewports, and explicit control over how the screen is composed. You get the simplicity and predictability of that model without palette restrictions, scanline sprite limits, bitplanes, or fixed hardware layouts. No magic control-register addresses, cryptic bit flags, DMA routines, or HBlank/VBlank timing gymnastics required.
- **Code-first and explicit by design.**
  Kasane2D avoids scene editors, opaque object hierarchies, and framework-heavy boilerplate. Engine systems are configured directly in C#, and game code interacts with them through small, focused APIs. The engine provides the machinery while leaving structure and architecture in the hands of the developer.
- **Audio is a first-class engine system.**
  Instead of treating audio as little more than `PlaySound()`, Kasane2D includes a hierarchical software mixer with nested buses, dBFS gain, panning, runtime routing, and per-bus effect chains. Games can manipulate the audio graph dynamically just like any other engine system.
- **Start simple, opt into complexity when you want it.**
  The core engine stays deliberately lightweight. Straightforward games can use ordinary sprite rendering, sound effects, and prerecorded music without touching anything more advanced. Optional packages such as `Kasane2D.Music` add tracker-inspired sequencing, synthesis, procedural music, and deep runtime control without making those features mandatory for everyone else.

# TODO: installation/quick start

# Core features
- Graphics and rendering system inspired by hardware sprite engines of retro consoles.
  - Screens are composed from a set of configurable layers:
    - Tilemap layers with dynamically adjustable tile sheets and freely configurable tile dimensions.
    - Sprite layers with configurable sprite counts. No hardware-imposed scanline limits.
    - Texture layers for full customizable rendering.
  - Each surface has an independently scrollable viewport, making parallax effects trivial to implement.
  - Sprites can smoothly scroll in and out of the viewport.
  - Modern 32-bit RGBA textures with full alpha transparency and true color. No palette management or bitplanes required.
  - Horizontal and Vertical flipping of tiles and sprites.
  - Wraparound scrolling on both axis and in all four directions.
- Input system supporting keyboard, mouse, and controller input.
- Sound system designed around familiar audio-engineering concepts:
  - Hierarchical software mixer with an arbitrary number of nested mix buses.
  - Gain in dBFS rather than arbitrary normalized volume values.
  - Stereo panning with proper panning-law compensation.
  - 32-bit floating-point processing throughout the mixer.
  - Runtime control over mixer gain, pan, routing, and effects.
  - Per-bus effect chains with real-time parameter control.
  - Sound effect manager for playback and channel management.
  - Music player supporting looping tracks and dynamically managed playlists.
  - Extension interface for integrating custom audio systems directly into the existing mixer graph.
- The core engine stays lightweight and explicit: little boilerplate, no opaque scene machinery, and direct access to the systems you configure. Additional complexity lives in optional modules when you need it.

# Kasane2D.Music features
`Kasane2D.Music` is an optional tracker-inspired real-time synthesis and sequencing system. Musical structure and synthesis parameters remain fully programmable at runtime, allowing music to react directly to gameplay.
- Configurable synth engines with any number of tracks and freely selectable generator types.
- Built-in generators include:
  - Basic oscillator with sine, triangle, saw, and pulse waveforms, including waveform changes during playback.
  - DMG- and SID-inspired LFSR noise generators.
  - DMG-inspired wavetable oscillator.
  - Sample player for classic 16-bit tracker-style instruments.
- Custom generators can be implemented and plugged directly into the synthesis engine.
- Full MIDI note range.
- Per-pattern BPM, time signature, length, and note resolution.
- Note sequencing from quarter notes down to 128th notes.
- 64 control-event positions per quarter note, or 256 per bar in 4/4.
- Control events can modify generator, envelope, mixer, and effect parameters during playback.
- Seamless integration with the core mix-bus architecture.
- Song conductor for arranging and transitioning between patterns:
  - Pattern linking.
  - Queued pattern changes.
  - Song-to-song transitions.
  - Dedicated transition patterns.
- Direct runtime control makes procedural and gameplay-driven music possible.

# TODO: links and stuff

# Planned features