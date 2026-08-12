using Kasane2D.Music.Types;

namespace Kasane2D.Music.Interfaces;

/// <summary>
/// Represents a conductor managing a <see cref="ISynthEngine"/>.
/// </summary>
public interface IConductor
{
    /// <summary>
    /// Gets the synth engine this conductor is managing.
    /// </summary>
    public ISynthEngine SynthEngine { get; }
    
    /// <summary>
    /// Gets if a pattern is currently playing.
    /// </summary>
    public bool IsPlaying { get; }
    
    /// <summary>
    /// Gets the song name that is currently playing.
    /// </summary>
    public string CurrentSong { get; }
    
    /// <summary>
    /// Gets the section name that is currently playing.
    /// </summary>
    public string CurrentSection { get; }
    
    /// <summary>
    /// Gets the pattern name that is currently playing.
    /// </summary>
    public string CurrentPattern { get; }
    
    /// <summary>
    /// Gets the name of the next pattern that will play after the current one.
    /// </summary>
    public string NextPattern { get; }
    
    /// <summary>
    /// Gets the name of the next song queued for playback.
    /// </summary>
    public string NextSong { get; }
    
    /// <summary>
    /// Gets the section name of the next song queued for playback.
    /// </summary>
    public string NextSongSection { get; }
    
    /// <summary>
    /// Gets the name of the transition section currently set.
    /// </summary>
    public string TransitionSection { get; }
    
    /// <summary>
    /// Adds a song to the song list.
    /// </summary>
    /// <param name="song">The song to add.</param>
    public void AddSong(Song song);

    /// <summary>
    /// Removes a song from the song list.
    /// </summary>
    /// <param name="name">The song to remove.</param>
    public void RemoveSong(string name);

    /// <summary>
    /// Adds a collection of songs to the song list.
    /// </summary>
    /// <param name="songs">The songs to add.</param>
    public void AddSongs(IReadOnlyCollection<Song> songs);

    /// <summary>
    /// Removes a collection of songs from the song list.
    /// </summary>
    /// <param name="names">The songs to remove.</param>
    public void RemoveSongs(IReadOnlyCollection<string> names);

    /// <summary>
    /// Clears the song list.
    /// </summary>
    public void ClearSongs();

    /// <summary>
    /// Plays a song from the song list.
    /// </summary>
    /// <param name="songName">The song to play.</param>
    /// <param name="sectionName">The section of the song to play.</param>
    public void Play(string songName, string sectionName);

    /// <summary>
    /// Pauses playback.
    /// </summary>
    public void Pause();

    /// <summary>
    /// Resumes playback.
    /// </summary>
    public void Resume();

    /// <summary>
    /// Stops playback and drops song from the synth engine.
    /// </summary>
    public void Stop();

    /// <summary>
    /// Queues a song to play after the current one is finished.
    /// </summary>
    /// <param name="songName">The song to play next.</param>
    /// <param name="sectionName">The song's section to play next.</param>
    /// <remarks>If a section is an infinite loop because it references itself, a transition will never occur.</remarks>
    public void Queue(string songName, string sectionName);

    /// <summary>
    /// Queues a section from the current song to play next.
    /// </summary>
    /// <param name="sectionName">The song's section to play next.</param>
    /// <remarks>If a section is an infinite loop because it references itself, a transition will never occur.</remarks>
    public void Queue(string sectionName);

    /// <summary>
    /// Sets up a transition from the current song to the next song.
    /// </summary>
    /// <param name="transitionSection">The transition section of the current song to play before the next song.</param>
    /// <param name="songName">The song to transition to.</param>
    /// <param name="sectionName">The song's section to transition to.</param>
    /// <param name="switchAfterPattern">Optional: Switches immediately after the current pattern finishes when true.
    /// Default is false.</param>
    /// <remarks>If a section is an infinite loop because it references itself, a transition will never occur unless
    /// switchAfterPattern is set to true.</remarks>
    public void Transition
        (
        string transitionSection,
        string songName,
        string sectionName,
        bool switchAfterPattern = false
        );
}