using Kasane2D.Music.Enums;
using Kasane2D.Music.Types;

namespace Kasane2D.Music.Synthesis;

internal class Sequencer
{
    public Sequencer(SynthVoice voice)
    {
        Voice = voice;
    }
    
    public SynthVoice Voice { get; }

    public Sequence? CurrentSequence { get; set; }
    
    public Sequence? NextSequence { get; set; }

    public void Process(int sampleCount)
    {
        Voice.Process(sampleCount);
    }

    public void Step(int step)
    {
        if (CurrentSequence is null)
        {
            return;
        }

        var note = CurrentSequence.Notes[step];
        switch (note.Kind)
        {
            case SequenceNoteEventKind.Begin:
                Voice.Play(note.Note);
                break;
            
            case SequenceNoteEventKind.Release:
                Voice.Stop();
                break;
        }
    }

    public void Next()
    {
        Reset();
        
        if (NextSequence is null)
        {
            return;
        }
        
        CurrentSequence = NextSequence;
        NextSequence = null;
    }

    public void Reset()
    {
        Voice.Stop();
    }
}