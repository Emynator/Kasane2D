using Kasane2D.Music.Types;

namespace Kasane2D.Music.Interfaces;

public interface IConductor
{
    public void AddSong(Song song);

    public void RemoveSong(string name);

    public void AddSongs(IReadOnlyCollection<Song> songs);

    public void RemoveSongs(IReadOnlyCollection<string> names);

    public void ClearSongs();

    public void Play(string songName, string sectionName);

    public void Pause();

    public void Resume();

    public void Stop();

    public void Queue(string songName, string sectionName);

    public void Queue(string sectionName);

    public void Transition
        (
        string transitionSection,
        string songName,
        string sectionName,
        bool switchAfterPattern = false
        );
}