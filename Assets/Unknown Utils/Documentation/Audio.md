# Simple Audio Manager

The **Audio Manager** provides a simple way to manage **SFX and Music** in your project.

## Audio Setup

Create a **ScriptableObject** for each audio entry you want to use.

Each audio entry can contain multiple audio clips and provides different playback modes:

* **First** → Plays the first clip in the list.
* **Random** → Plays a random clip from the list.
* **Shuffle** → Plays a random clip while ensuring the last played clip is not repeated.
* **PingPong** → Plays clips from `0 → 5` and then `5 → 0`.
* **Sequence** → Plays clips from `0 → 5` and then starts again from `0`.

### Sequence Time

**Sequence Time** determines how long the Audio Manager waits before resetting the sequence.

---

## Playing SFX

| Method                                         | Description                                          |
| ---------------------------------------------- | ---------------------------------------------------- |
| `PlaySFX(string id, float delay = 0)`          | Plays an SFX using the Audio Manager's audio source. |
| `PlayOneShot(string id, float delay = 0)`      | Plays an SFX as a one-shot.                          |
| `PlayOneShot(AudioClip clip, float delay = 0)` | Plays the provided `AudioClip` as a one-shot.        |
| `PauseSFX()`                                   | Pauses the currently playing SFX.                    |
| `ResumeSFX()`                                  | Resumes the paused SFX.                              |
| `StopSFX()`                                    | Stops the currently playing SFX.                     |
| `SetSFXMute(bool value)`                       | Sets the SFX mute state.                             |
| `ToggleSFX()`                                  | Toggles the SFX mute state.                          |
| `ChangePitchSFX(float value)`                  | Changes the pitch of the SFX audio source.           |

---

## Playing Music

| Method                          | Description                                  |
| ------------------------------- | -------------------------------------------- |
| `PlayMusic(string id)`          | Plays music using the specified audio ID.    |
| `PauseMusic()`                  | Pauses the currently playing music.          |
| `ResumeMusic()`                 | Resumes the paused music.                    |
| `StopMusic()`                   | Stops the currently playing music.           |
| `ToggleMusic()`                 | Toggles the music mute state.                |
| `SetMusicMute(bool value)`      | Sets the music mute state.                   |
| `ChangePitchMusic(float pitch)` | Changes the pitch of the music audio source. |

---

## Example

```csharp
AudioManager.PlaySFX("ButtonClick");
AudioManager.PlayMusic("MainTheme");

AudioManager.ToggleSFX();
AudioManager.SetMusicMute(true);
```
