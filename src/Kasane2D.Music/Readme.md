# Kasane2D.Music

Kasane2D.Music is the optional tracker-inspired real-time synthesis and sequencing package for Kasane2D. It provides basic oscillators, DMG- and SID-inspired noise, wavetable and sample generators, programmable patterns, fine-grained control events, conductor-managed song transitions, and direct runtime control.

Install this package when your game needs synthesized, procedural, or gameplay-responsive music. Prerecorded music and sound-effect playback are already available in the core engine.

```shell
dotnet add package Kasane2D.Music
```

Create a synth engine from an initialized Kasane2D sound system:

```C#
var synth = SoundSystem?.CreateSynthEngine(new SynthConfig
{
    Name = "Game Music",
    TrackConfigs =
    [
        new()
        {
            Name = "Lead",
            Kind = GeneratorKind.BasicOscillator,
        },
    ],
});

var conductor = synth?.CreateConductor();
```

Patterns can schedule notes and microstep control events, while a conductor arranges patterns and handles queued pattern or song transitions. See the [main readme](https://github.com/Emynator/Kasane2D#readme) for the full feature overview, examples, and project documentation.