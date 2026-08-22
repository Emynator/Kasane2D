# 4.5 - ISfxManager
The `ISfxManager` interface is the sound effect manager for the audio system. It automatically manages playing sound effects. There is a fixed number of available sound effect channels that are able to play at the same time. If playback of sound effects is requested when all channels are busy, the effect gets queued for playback as soon as a channel is available.

The sound effect manager expects loaded `AudioFileStream`s. Please refer to the [documentation about loading audio files](LoadingAudioFiles.md) for details.

# Properties and functions

`ISfxManager.ChannelCount` is the configured number of available audio channels for sound effects. This is the maximum number of sound effects that can play at the same time.

`ISfxManager.BusyChannels` is the number of effect channels currently busy with playback.

`ISfxManager.AllChannelsBusy` indicates if all channels are currently busy with playback or if channels are available to receive new sound effects for immediate playback.

`ISfxManager.QueueLength` how many sound effects are currently waiting to be played back.

`ISfxManager.Play(AudioFileStream sound)` plays the provided sound effect immediately when a channel is available or puts it to the end of the playback queue if all channels are currently busy.

`ISfxManager.StopAll()` stops playback of all sound effects and removes them from the respective channels.

`ISfxManager.DropQueue()` drops all sound effects still waiting in the queue.

# Useful information
The sound effect manager uses a dedicated mix bus feeding into the master bus with the name "SFX". In addition, each sound effect channel has its own mix bus that feeds into the shared sfx bus. The channel buses are named following the scheme "SFX_Channel_{n}" where n is the channel number ranging from 0 to `ISfxManager.ChannelCount` - 1.
Those names can be used to quickly retrieve the belonging mix buses from the mixer and adjust their parameters.