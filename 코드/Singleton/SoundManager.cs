using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : Singleton<SoundManager> {

    AudioSource _mainAudio;
    List<AudioSource> _audioSource;
    Dictionary<AudioSource, float> _volume;
    UnityEngine.UI.Slider _slider;

	void Awake () {
        init();
	}
	
    internal void playSound(string name, float volum = 1f)
    {
        internalPlaySound(name, volum);
    }

    internal void playSoundEffect(string name, float volum = 1f)
    {
        internalPlaySoundEffect(name, volum);
    }

    internal void playSoundEffectLoop(string name, float volum = 1f)
    {
        internalPlaySoundEffectLoop(name, volum);
    }

    internal void stopSoundEffect(string name)
    {
        internalStopSoundEffet(name);
    }

    internal void setVolume()
    {
        var loopSoundEffects = _volume.Keys;
        foreach(var key in loopSoundEffects)
        {
            float volumeId;
            _volume.TryGetValue(key, out volumeId);
            key.volume = volumeId * _slider.value;
        }
    }

    void init()
    {
        _audioSource = new List<AudioSource>();
        _volume = new Dictionary<AudioSource, float>();
        _mainAudio = createAudio();
        _slider = Utility.Instance.findChild("UI", "Option/SoundSlider").GetComponent<UnityEngine.UI.Slider>();

        for (int i = 0; i < 5; i++)
        {
            var audioSource = createAudio();
            _audioSource.Add(audioSource);
        }
    }    

    void internalPlaySound(string name, float volume = 1f)
    {
        if(_mainAudio.isPlaying) _mainAudio.Stop();
        _mainAudio.clip = ResourcesManager.Instance.audioClips[name];

        if (_mainAudio.clip == null) return;

        _mainAudio.volume = volume * _slider.value;
        _mainAudio.loop = true;
        _mainAudio.Play();

        if (!_volume.ContainsKey(_mainAudio))
            _volume.Add(_mainAudio, volume);
    }

    void internalPlaySoundEffect(string name, float volum = 1f)
    {
        var audio = findEmptyAudio();

        if(!audio)
        {
            audio = createAudio();
            _audioSource.Add(audio);
        }

        audio.clip = ResourcesManager.Instance.audioClips[name];
        audio.volume = volum * _slider.value;

        if (audio.clip != null) audio.PlayOneShot(audio.clip, volum);        
    }

    void internalPlaySoundEffectLoop(string name, float volum = 1f)
    {
        var audio = findAudio(name);

        if (audio)
        {
            audio.volume = volum * _slider.value;
            if (!_volume.ContainsKey(audio))
                _volume.Add(audio, volum);
            return;
        }

        else
            audio = findEmptyAudio();

        if (!audio)
        {
            audio = createAudio();
            _audioSource.Add(audio);
        }

        audio.clip = ResourcesManager.Instance.audioClips[name];
        audio.volume = volum * _slider.value;
        audio.loop = true;        
        audio.Play();
        if(!_volume.ContainsKey(audio))
            _volume.Add(audio, volum);

    }

    void internalStopSoundEffet(string name)
    {
        var audio = findAudio(name);

        if (!audio) return;

        audio.loop = false;
        audio.Stop();
        audio.clip = null;
    }

    AudioSource findEmptyAudio()
    {
        for (int i = 0; i < _audioSource.Count; i++)
        {
            if (!_audioSource[i].isPlaying) return _audioSource[i];
        }

        return null;
    }

    AudioSource findAudio(string name)
    {
        for (int i = 0; i < _audioSource.Count; i++)
        {

            if (_audioSource[i].clip && _audioSource[i].clip.name.CompareTo(name) == 0)            
                return _audioSource[i];                
            
        }

        return null;
    }

    AudioSource createAudio()
    {
        var gameObj = new GameObject();
        var audioSource = gameObj.AddComponent<AudioSource>();        
        gameObj.transform.SetParent(transform);        

        return audioSource;
    }
}
