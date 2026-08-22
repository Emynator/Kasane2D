# 4.3 - What is a Mix Bus?
A mix bus is a relative simple concept, but for anyone not familiar with digital audio workstations, the term mix bus might be confusing at first.

A mix bus is essentially a pipeline for audio data. It has an input where samples get put into the bus. Then, if a mix bus has child buses, the input audio gets summed with the output of all child buses. The resulting audio is then fed in sequence through a chain of audio effects (if present). At the end, the buse's pan and gain settings are applied to the final audio data before it is fed into the buse's output.

# IMixBus
The `IMixBus` interface exposes all relevant functionality to interact with the mix buses of Kasane2D's sound system.

## Important properties and functions
These properties and functions are ones you will likely want to use most of the time.

`IMixBus.Name` is the name of mix bus.

`IMixBus.Gain` is the current gain that will be applied to resulting audio data before output. Positive values increase the volume, negative values decrease the volume.

`IMixBus.Pan` is the current pan of the mix bus. It can reach from -100 (fully panned to the left) to 100 (fully panned to the right). The [pan law](https://en.wikipedia.org/wiki/Panning_law) is applied to this property.

`IMixBus.Effects` is the list of `IAudioEffect`s currently assigned to this mix bus. Effects are applied in the order of this list in sequence.

`IMixBus.AddEffect(IAudioEffect effect)` adds a new audio effect to the end of the buse's effect chain.

`IMixBus.RemoveEffect(string name)` removes the audio effect of the given name from the buse's effect chain.

`IMixBusSetEffects(IReadOnlyCollection<IAudioEffect> effects).` replaces the buse's current effect chain with a new one.

`IMixBus.ClearEffects()` drops all audio effects from the bus.

## Situationally useful
Usually not needed properties. They are mostly useful for traversing the tree of mix buses. Usually you can just retrieve specific mix buses via `IAudioMixer.TryGetMixBus()`, but situationally it might turn out to be useful to manually traverse the tree of mix buses.

`IMixBus.Parent` is the parent bus of the current mix bus. All mix buses except the master bus have a parent.

`IMixBus.Children` is the list of children this mix bus has. Might be empty. This is mostly useful for traversing the tree of mix buses.

## Only use when you know what you are doing
Danger zone. Writing audio data into a mix bus is something that usually should only be done from a sound sub-system, since processing of audio data is synchronized across all sound systems. Since the mix buses use circular audio buffers, writing into them out of order can override existing sample data which can lead to popping or other undesired audio glitches.

`IMixBus.WriteLeft(ReadOnlySpan<float> samples)` writes audio data into the left channel of the mix bus.

`IMixBus.WriteRight(ReadOnlySpan<float> samples)` writes audio data into the right channel of the mix bus.