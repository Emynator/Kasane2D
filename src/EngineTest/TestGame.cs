using Kasane2D;
using Kasane2D.Music;
using Kasane2D.Music.Enums;
using Kasane2D.Music.Interfaces;
using Kasane2D.Music.Types;
using Kasane2D.Music.Types.SequenceEvents;

namespace EngineTest;

public class TestGame : EngineMain
{
    private ISynthEngine? synth;

    public override void Init()
    {
        synth = SoundSystem?.CreateSynthEngine
        (
            new()
            {
                Name = "SynthTest",
                TrackConfigs =
                [
                    new()
                    {
                        Name = "Track 1",
                        Kind = GeneratorKind.BasicOscillator,
                    },
                ],
            }
        );

        var pattern = new SongPattern
        (
            "Dreamer",
            TimeSignature.FourFour,
            80,
            1,
            StepSize.Eighth,
            [
                new
                (
                    "Track 1",
                    new
                    (
                        VolumeUpdate: new(-3),
                        EnvelopeUpdate: new(50.0f, 150.0f, 0.0f, 10.0f),
                        GeneratorUpdate: new BasicOscillatorUpdate(BasicWave.Saw)
                    ),
                    [
                        new(0, 0, NoteEventKind.Begin, Note.A2),
                        new(0, 1, NoteEventKind.Begin, Note.D3),
                        new(0, 2, NoteEventKind.Begin, Note.E3),
                        new(0, 3, NoteEventKind.Begin, Note.F3),
                        new(0, 4, NoteEventKind.Begin, Note.E3),
                        new(0, 5, NoteEventKind.Begin, Note.D3),
                        new(0, 6, NoteEventKind.Hold),
                        new(1, 0, NoteEventKind.Begin, Note.F3),
                        new(1, 1, NoteEventKind.Begin, Note.E3),
                        new(1, 2, NoteEventKind.Begin, Note.D3),
                        new(1, 3, NoteEventKind.Begin, Note.A3),
                        new(1, 4, NoteEventKind.Begin, Note.G3),
                        new(1, 5, NoteEventKind.Begin, Note.F3),
                        new(1, 6, NoteEventKind.Hold),
                        new(2, 0, NoteEventKind.Begin, Note.A3),
                        new(2, 1, NoteEventKind.Begin, Note.G3),
                        new(2, 2, NoteEventKind.Begin, Note.F3),
                        new(2, 3, NoteEventKind.Begin, Note.G3),
                        new(2, 4, NoteEventKind.Begin, Note.F3),
                        new(2, 5, NoteEventKind.Begin, Note.E3),
                        new(2, 6, NoteEventKind.Hold),
                        new(3, 0, NoteEventKind.Begin, Note.G3),
                        new(3, 1, NoteEventKind.Begin, Note.F3),
                        new(3, 2, NoteEventKind.Begin, Note.E3),
                        new(3, 3, NoteEventKind.Begin, Note.F3),
                        new(3, 4, NoteEventKind.Hold),
                        new(3, 5, NoteEventKind.Begin, Note.E3),
                        new(3, 6, NoteEventKind.Begin, Note.D3),
                        new(3, 7, NoteEventKind.Hold),
                    ],
                    []
                ),
            ]
        );

        synth?.Play(pattern);
    }

    protected override void Tick(float dt)
    {
    }
}