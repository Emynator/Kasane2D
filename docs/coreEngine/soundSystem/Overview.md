# 4.1 - Sound System Overview and ISoundSystem
Kasane2D's sound system is build on top of a sophisticated audio mixing engine. Unlike many other engines, there is not just a simple `PlaySound("bounce.wav")` function. The audio mixing engine is much closer to mixing engines usually found in digital audio workstations. This allows for a much finer grained control of all things related to audio, including realtime audio effects and full dynamic control of the mixing engine.

The sound system itself runs on a dedicated thread to keep the audio device's buffer fed. With that in mind, the entire sound system is designed to be thread safe.

# ISoundSystem
The `ISoundSystem` interface is your entry point to the engine's sound system and available with `EngineMain.SoundSystem`. Unlike the graphics system and the input system, the sound system is an optional component. It will only be injected, if the sound system is configured. (Refer to the [configuration documentation](../gettingStarted/EngineConfiguration.md) for details.)

`ISoundSystem.SampleRate` and `ISoundSystem.BufferSize` are the sound system's sample rate and the size of the audio buffer in samples that is processed in each processing step. Both values are only relevant if you are creating [custom sound sub-systems](SoundSubSystem.md).

`ISoundSystem.AudioMixer` is your access to the software mixing engine.

`ISoundSystem.SfxManager` is, as the name suggests, a sound effect manager that handles simple playback of sound effects. This is where the functionality for your `PlaySound("bounce.wav")` usecase is located.

`ISoundSystem.MusicPlayer` is analogue to the sound effect manager, but intended for playback of long form audio files like your game's soundtrack.

`ISoundSystem.AddSubSystem(ISoundSubSystem system)` adds a custom sound sub-system that will be processed in sync with all other sound systems. Refer to the [sound sub-system documentation](SoundSubSystem.md) for details.

`ISoundSystem.RemoveSubSystem(Guid id)` removes a custom sound sub-system.

`ISoundSystem.CreateBuffer(int bufferSize)` creates an audio buffer to use. This is mostly for advanced use cases and custom sub-systems. The relevant documentation is [here](AudioTypes.md).