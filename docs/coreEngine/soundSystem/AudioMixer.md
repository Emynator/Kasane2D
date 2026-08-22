# 4.2 - IAudioMixer
The audio mixer is beating heart the entire sound system is build upon. It's build on top of a nested mix bus architecture that will be familiar to anyone who has gathered experience with any modern digital audio workstation.
The mixing engine is fully software based operates with 32-bit float samples for lossless processing of audio data with - not technically, but for all practical purposes speaking - infinite head room. Only at the very end, the master's output will be converted to the audio device's own bit depth.

**Important:** Digital clipping can **and will** occur when the master exceeds 0 dBFS! Infinite head room applies **only** to the sound system's internal mix buses!

`IAudioMixer.Master` gives you access to the master bus.

`IAudioMixer.CreateMixBus(string name, IMixBus? parent = null)` creates a new mix bus with the specified name. The parent parameter is optional. If it is null, the new mix bus will become a child of the master bus. Otherwise, it will become a child of the specified parent bus.

`IAudioMixer.ReleaseMixBus(IMixBus bus)` deletes a mix bus and removes it from the mixing engine.

`IAudioMixer.TryGetMixBus(string name, out IMixBus? bus)` instead of manually searching through the tree of mix buses, this helper function allows you to easily retrieve the mix bus with a specific name.