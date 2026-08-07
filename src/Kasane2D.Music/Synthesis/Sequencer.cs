using Kasane2D.Music.Enums;
using Kasane2D.Music.Types;

namespace Kasane2D.Music.Synthesis;

internal class Sequencer
{
    private readonly SynthVoice voice;
    
    public Sequencer(SynthVoice voice)
    {
        this.voice = voice;
    }

    public Sequence? CurrentSequence { get; set; }
    
    public Sequence? NextSequence { get; set; }

    public void Process(int sampleCount)
    {
        voice.Process(sampleCount);
    }

    public void Step(int step)
    {
        if (CurrentSequence is null)
        {
            return;
        }

        voice.ControlUpdate(CurrentSequence.ControlEvents[step]);

        var note = CurrentSequence.Notes[step];
        switch (note.Kind)
        {
            case SequenceNoteEventKind.Begin:
                voice.Play(note.Note);
                break;
            
            case SequenceNoteEventKind.Release:
                voice.Stop();
                break;
        }
    }

    public void Next()
    {
        if (NextSequence is null)
        {
            Reset();
            return;
        }
        
        CurrentSequence = NextSequence;
        NextSequence = null;
        Reset();
    }

    public void Reset()
    {
        voice.Stop();
        voice.ControlUpdate(CurrentSequence?.InitialSettings ?? default);
    }

    public void Stop()
    {
        voice.Stop();
    }
}