using Kasane2D.Music.Interfaces;
using Kasane2D.Music.Types;

namespace Kasane2D.Music.Synthesis;

internal class Conductor : IConductor
{
    private readonly SynthEngine synthEngine;
    private readonly Dictionary<string, Song> songs = new();
    private Song? currentSong = null;
    private Song? nextSong = null;
    private SongElement? nextElement = null;
    private SongElement? nextSongElement = null;
    private SongElement? transitionElement = null;
    private SongPattern? currentPattern = null;
    private int repeats = 0;
    private int maxRepeats = 0;
    private string? sectionNameToSet = null;

    public Conductor(SynthEngine synthEngine)
    {
        this.synthEngine = synthEngine;
        synthEngine.InternalConductor = this;
    }

    public ISynthEngine SynthEngine => synthEngine;
    
    public bool IsPlaying { get; private set; } = false;
    
    public string CurrentSong => currentSong?.Name ?? "";

    public string CurrentSection { get; private set; } = "";
    
    public string CurrentPattern => currentPattern?.Name ?? "";

    public string NextPattern => nextElement?.PatternName ?? "";

    public string NextSong => nextSong?.Name ?? "";

    public string NextSongSection => nextSongElement?.PatternName ?? "";

    public string TransitionSection => transitionElement?.PatternName ?? "";

    public void AddSong(Song song)
    {
        songs[song.Name] = song;
    }

    public void RemoveSong(string name)
    {
        songs.Remove(name);
    }

    public void AddSongs(IReadOnlyCollection<Song> songs)
    {
        foreach (var song in songs)
        {
            this.songs[song.Name] = song;
        }
    }

    public void RemoveSongs(IReadOnlyCollection<string> names)
    {
        foreach (var name in names)
        {
            songs.Remove(name);
        }
    }

    public void ClearSongs()
    {
        songs.Clear();
    }

    public void Play(string songName, string sectionName)
    {
        if (!songs.TryGetValue(songName, out var song))
        {
            return;
        }

        if (!song.Sections.TryGetValue(sectionName, out var section))
        {
            return;
        }

        if (!song.Patterns.TryGetValue(section.PatternName, out var pattern))
        {
            return;
        }

        currentSong = song;
        nextElement = section.Next;
        synthEngine.Play(pattern);
        IsPlaying = true;
        CurrentSection = sectionName;

        if (!currentSong.Patterns.TryGetValue(nextElement?.PatternName ?? "", out var nextPattern))
        {
            return;
        }

        synthEngine.Queue(nextPattern);
        nextElement = nextElement?.Next;
    }

    public void Pause()
    {
        synthEngine.Pause();
        IsPlaying = false;
    }

    public void Resume()
    {
        synthEngine.Resume();
        IsPlaying = true;
    }

    public void Stop()
    {
        synthEngine.Stop();
        IsPlaying = false;
    }

    public void Queue(string songName, string sectionName)
    {
        if (!songs.TryGetValue(songName, out var song))
        {
            return;
        }

        if (!song.Sections.TryGetValue(sectionName, out var section))
        {
            return;
        }

        nextSong = song;
        nextSongElement = section;
    }

    public void Queue(string sectionName)
    {
        if (currentSong is null)
        {
            return;
        }

        if (!currentSong.Sections.TryGetValue(sectionName, out var section))
        {
            return;
        }
        
        nextElement = section;
        sectionNameToSet = sectionName;
    }

    public void Transition
        (
        string transitionSection,
        string songName,
        string sectionName,
        bool switchAfterPattern = false
        )
    {
        if (!songs.TryGetValue(songName, out var song))
        {
            return;
        }

        if (!song.Sections.TryGetValue(sectionName, out var section))
        {
            return;
        }

        if (currentSong is null)
        {
            currentSong = song;
            nextElement = section;

            return;
        }

        if (!currentSong.Sections.TryGetValue(transitionSection, out var transition))
        {
            return;
        }

        nextSong = song;
        nextSongElement = section;

        if (switchAfterPattern)
        {
            nextElement = transition;

            return;
        }

        transitionElement = transition;
    }

    public void UpdateSynthEngine()
    {
        repeats++;
        
        if (currentPattern is not null)
        {
            if (repeats < maxRepeats)
            {
                synthEngine.Queue(currentPattern);

                return;
            }
        }

        currentPattern = null;
        repeats = 0;
        maxRepeats = 0;
        
        if (currentSong is null)
        {
            if (nextSong is null || nextSongElement is null)
            {
                return;
            }

            currentSong = nextSong;
            nextSong = null;
            if (currentSong.Patterns.TryGetValue(nextSongElement.PatternName, out var pattern))
            {
                currentPattern = pattern;
                maxRepeats = nextSongElement.RepeatCount;
                synthEngine.Queue(pattern);
            }

            nextElement = nextSongElement.Next;
            nextSongElement = null;

            return;
        }

        if (nextElement is null)
        {
            if (transitionElement is not null)
            {
                nextElement = transitionElement.Next;
                if (currentSong.Patterns.TryGetValue(transitionElement.PatternName, out var pattern))
                {
                    currentPattern = pattern;
                    maxRepeats = transitionElement.RepeatCount;
                    synthEngine.Queue(pattern);
                }

                transitionElement = null;

                return;
            }

            if (nextSong is null || nextSongElement is null)
            {
                return;
            }

            currentSong = nextSong;
            nextSong = null;
            if (currentSong.Patterns.TryGetValue(nextSongElement.PatternName, out var pat))
            {
                currentPattern = pat;
                maxRepeats = nextSongElement.RepeatCount;
                synthEngine.Queue(pat);
            }

            nextElement = nextSongElement.Next;
            nextSongElement = null;

            return;
        }

        if (currentSong.Patterns.TryGetValue(nextElement.PatternName, out var nextPattern))
        {
            if (sectionNameToSet is not null)
            {
                CurrentSection = sectionNameToSet;
                sectionNameToSet = null;
            }
            
            currentPattern = nextPattern;
            maxRepeats = nextElement.RepeatCount;
            synthEngine.Queue(nextPattern);
        }

        nextElement = nextElement.Next;
    }
}