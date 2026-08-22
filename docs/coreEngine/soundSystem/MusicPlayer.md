# 4.6 - IMusicPlayer
The `IMusicPlayer` interface is the manager that manages playback of long-form audio files like music files. It works similar to the sound effect manager, but unlike the sound effect manager, it only has a single playback channel.
Unlike sound effects which are usually one shot playbacks, it is usually desired for music playback to loop over and over again. In addition, playback of music files can also be paused and resumed from the point where it had been paused.

The music player expects loaded `AudioFileStream`s. Please refer to the [documentation about loading audio files](LoadingAudioFiles.md) for details.

# Properties and functions

`IMusicPlayer.IsPlaying` indicates if an audio file is currently playing or not.

`IMusicPlayer.IsLooping` indicates if the current audio file will be looped when playback has finished or if the next audio file from the queue (if available) will be picked.

`IMusicPlayer.QueueLength` the number of audio files currently waiting in the queue for playback.

`IMusicPlayer.Play(AudioFileStream song, bool loop = false)` plays a song immediately, regardless if another song is currently playing or not. Optionally, this audio file can be set to loop forever.

`IMusicPlayer.Pause()` pauses playback of the current audio file.

`IMusicPlayer.Resume()` resumes playback of the current audio file where it has been paused.

`IMusicPlayer.Stop()` pauses playback of the current audio file and resets the playback position to the beginning of the file.

`IMusicPlayer.EndLoop()` cancels the loop mode of the player when it is currently set to loop mode and returns to play one file after the other from the audio queue.

`IMusicPlayer.Queue(AudioFileStream song)` puts an audio file at the end of the playback queue.

`IMusicPlayer.ClearQueue()` drops all files currently waiting for playback from the playback queue.