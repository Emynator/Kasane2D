# 4.8.2 - Sound Sub-Systems
Feeding audio data into a mix bus for processing needs to be synchronized with the rest of the audio system. Otherwise, the input buffers of the mix buses are at risk of overrunning. Kasane2D's sound system runs on a dedicated thread and is not synced with the main update function for game code. This makes it difficult to feed custom audio data into mix buses in sync with the rest of the sound system.

The `ISoundSubSystem` interface solves this problem. A sound sub-system is a custom sound processing system that is able to seemlessly integrate with the rest of the sound system and make use of Kasane2D's mixing engine for all kinds of scenarios.
One example of a sound sub-systems is the optional `Kasane2D.Music` package that adds a real-time synthesizer engine.

# Implementing a custom Sound Sub-System
Implementing a custom sound sub-system is relatively straight forward. The sub-system needs to implement the `ISoundSubSystem` interface. It exposed a `Guid` used to identify the sub-system for removal and a `Process()` function that runs in sync with the rest of the engine's sound system.

The `ISoundSystem` interface exposes two important properties for custom sub-systems:
- `ISoundSystem.SampleRate` is the configured sample rate the sound system runs at.
- `ISoundSystem.BufferSize` is the number of samples that should be processed during each call of `Process()`.

You can create mix buses and audio buffers as needed for your custom sub-system.

**Important:** If you expose functionality meant to be called from user code, you should design all access in with thread safety in mind to prevent race conditions!