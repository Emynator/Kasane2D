using Kasane2D.Events;
using Kasane2D.Music.Enums;
using Kasane2D.Music.Interfaces;
using Kasane2D.Music.Types;

namespace Kasane2D.Music.Synthesis;

internal class Sequencer : ITrack
{
    private readonly SynthVoice voice;
    private readonly KasaneEventSource<Note> notePlayEvent = new();
    private readonly KasaneEventSource noteReleaseEvent = new();
    
    public Sequencer(string name, SynthVoice voice)
    {
        this.voice = voice;
        Name = name;
    }
    
    public string Name { get; }

    public KasaneEvent<Note> NotePlayEvent => notePlayEvent.Event;
    
    public KasaneEvent NoteReleaseEvent => noteReleaseEvent.Event;

    public Sequence? CurrentSequence { get; set; }
    
    public Sequence? NextSequence { get; set; }
    
    public int CurrentBpm { get; private set; }
    
    public int BeatsPerBar { get; private set; }

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
                notePlayEvent.Trigger(note.Note);
                break;
            
            case SequenceNoteEventKind.Release:
                voice.Stop();
                noteReleaseEvent.Trigger();
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
        voice.Reset();
        voice.ControlUpdate(CurrentSequence?.InitialSettings ?? default);
    }

    public void Stop()
    {
        voice.Stop();
    }
}