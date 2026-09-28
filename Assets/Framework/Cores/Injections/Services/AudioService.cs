using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Dada.Cores;

public class AudioService : IAudioService, IDisposable
{
    private const int DefaultSFXPoolSize = 8;

    private readonly GameObject _root;
    private readonly AudioSource _bgmSource;
    private readonly List<AudioSource> _sfxSources = new();
    private readonly AudioDriver _driver;

    private int _sfxIndex;
    private float _masterVolume = 1f;
    private float _bgmVolume = 1f;
    private float _sfxVolume = 1f;
    private bool _mute;

    public float MasterVolume
    {
        get => _masterVolume;
        set { _masterVolume = Mathf.Clamp01(value); ApplyBGMVolume(); }
    }

    public float BGMVolume
    {
        get => _bgmVolume;
        set { _bgmVolume = Mathf.Clamp01(value); ApplyBGMVolume(); }
    }

    public float SFXVolume
    {
        get => _sfxVolume;
        set => _sfxVolume = Mathf.Clamp01(value);
    }

    public bool Mute
    {
        get => _mute;
        set { _mute = value; ApplyBGMVolume(); }
    }

    public AudioService(int sfxPoolSize = DefaultSFXPoolSize)
    {
        _root = new GameObject("AudioService");
        Object.DontDestroyOnLoad(_root);

        _driver = _root.AddComponent<AudioDriver>();

        _bgmSource = CreateSource("BGM_Source", _root.transform);
        _bgmSource.loop = true;

        for (var i = 0; i < sfxPoolSize; i++)
        {
            var source = CreateSource($"SFX_Source_{i}", _root.transform);
            _sfxSources.Add(source);
        }
    }

    public void PlayBGM(AudioClip clip, float fadeDuration = 1f)
    {
        if (_bgmSource.clip == clip) return;
        _driver.StopFade();
        _driver.StartFade(FadeBGM(clip, fadeDuration));
    }

    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null) return;

        var source = _sfxSources[_sfxIndex];
        _sfxIndex = (_sfxIndex + 1) % _sfxSources.Count;

        source.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        source.PlayOneShot(clip, _sfxVolume * volumeScale * (_mute ? 0f : _masterVolume));
    }

    public void StopBGM(float fadeDuration = 0.5f)
    {
        _driver.StopFade();
        _driver.StartFade(FadeOutBGM(fadeDuration));
    }

    public void Dispose()
    {
        _driver.StopFade();
        Object.Destroy(_root);
    }

    private static AudioSource CreateSource(string name, Transform parent)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent);
        var source = go.AddComponent<AudioSource>();
        source.playOnAwake = false;
        return source;
    }

    private System.Collections.IEnumerator FadeBGM(AudioClip clip, float duration)
    {
        if (_bgmSource.isPlaying)
            yield return FadeOutBGM(duration * 0.5f);

        _bgmSource.clip = clip;
        _bgmSource.Play();
        yield return FadeTo(0f, 1f, duration);
    }

    private System.Collections.IEnumerator FadeOutBGM(float duration)
    {
        yield return FadeTo(1f, 0f, duration);
        _bgmSource.Stop();
        _bgmSource.clip = null;
    }

    private System.Collections.IEnumerator FadeTo(float from, float to, float duration)
    {
        var elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var t = elapsed / duration;
            _bgmSource.volume = Mathf.Lerp(from, to, t) * _bgmVolume * (_mute ? 0f : _masterVolume);
            yield return null;
        }
    }

    private void ApplyBGMVolume()
    {
        if (_bgmSource != null)
            _bgmSource.volume = _bgmVolume * (_mute ? 0f : _masterVolume);
    }

    internal class AudioDriver : MonoBehaviour
    {
        private Coroutine _current;

        public void StartFade(System.Collections.IEnumerator routine)
        {
            _current = StartCoroutine(routine);
        }

        public void StopFade()
        {
            if (_current != null)
            {
                StopCoroutine(_current);
                _current = null;
            }
        }
    }
}
