using UnityEngine;

namespace Dada.Cores;

public interface IAudioService
{
    void PlayBGM(AudioClip clip, float fadeDuration = 1f);
    void PlaySFX(AudioClip clip, float volumeScale = 1f);
    void StopBGM(float fadeDuration = 0.5f);

    float MasterVolume { get; set; }
    float BGMVolume { get; set; }
    float SFXVolume { get; set; }
    bool Mute { get; set; }
}
