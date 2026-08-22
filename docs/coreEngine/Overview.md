# Kasane2D Core Engine
The core engine provides all of the engine's core functionalities. It contains Kasane's graphics system, input system, and sound system. In addition, the core engine is responsible for the application lifecycle mangement and integrating the backend into a single, unified API surface for all user code targeting the engine. Finally, the core engine is also the common denominator where all optional modules hook up with the respective APIs.

# Brief overview of Kasane's core systems

## Graphics system
Kasane's graphics system is heavily inspired by the graphics hardware of retro consoles from the 8-bit and 16-bit era like the PPU of various Nintendo consoles. The underlying concept of Kasane's rendering system is that of composing the final image from a set of independent layers. Unlike the old hardware sprite and tile engines, Kasane's layer system is configurable and exposes a set of well defined APIs for manipulating and updating the layers.

## Input system
Kasane's input system is relative simple and straight forward. It automatically polls and updates the state of the various input devices and provides a simple API for game's to check the current state of the various input devices.

## Sound system
Kasane's sound system features a full fledged software mixer architecture familiar to everyone who has worked with any modern digital audio workstation before. It is based on the concept of a freely configurable, hierarchical set of mix buses. Each mix bus has its own input that can feed into it and can have an arbitrary number of child busses that feed into it as well. The signals get summed, effects are applied, gain and pan are applied and the output is fed to the parent bus. At the root of the hierarchy is the master bus that feeds its output to the sound device.

In addition to the mixer system, the core engine also provides a sound effect manager for playing sound effects, a music player for playing music files, and the architecture for loading audio files, handling audio streams, and resampling audio streams to a common sample rate.

# Table of Content
- 1 Getting Started
  - [1.1 Installation and Quickstart](gettingStarted/Installation.md)
  - [1.2 Engine Configuration](gettingStarted/EngineConfiguration.md)
  - [1.3 The EngineMain](gettingStarted/EngineMain.md)
- 2 Graphics system
  - [2.1 Overview](graphicsSystem/Overview.md)
  - [2.2 Layer Types](graphicsSystem/LayerTypes.md)
  - [2.3 Accessing the Graphics System](graphicsSystem/Interfaces.md)
  - [2.4 Managing sprite slots with ISlotManager](graphicsSystem/SlotManager.md)
  - [2.5 Free-Form Rendering](graphicsSystem/FreeFormRendering.md)
- [3 Input System](inputSystem/Overview.md)
- 4 Sound System
  - [4.1 Overview and ISoundSystem](soundSystem/Overview.md)
  - [4.2 IAudioMixer](soundSystem/AudioMixer.md)
  - [4.3 What is a Mix Bus?](soundSystem/MixBus.md)
  - [4.4 Loading Audio Files](soundSystem/LoadingAudioFiles.md)
  - [4.5 Sfx Manager](soundSystem/SfxManager.md)
  - [4.6 Music Player](soundSystem/MusicPlayer.md)
  - [4.7 Audio Effects](soundSystem/AudioEffects.md)
  - 4.8 Advanced Topics
    - [4.8.1 Low-Level Audio Types](soundSystem/AudioTypes.md)
    - [4.8.2 Sound Sub-Systems](soundSystem/SoundSubSystem.md)
    - [4.8.3 Implementing custom Audio File Streams](soundSystem/CustomAudioFileStream.md)