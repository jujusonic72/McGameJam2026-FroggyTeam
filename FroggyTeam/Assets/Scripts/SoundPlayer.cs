using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    private float checkFrequency = 2f;

    [SerializeField]
    private AudioSource sourceTemplate;
    List<AudioSource> audioSources;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void init()
    {
        if(audioSources == null)
        {
            audioSources = new List<AudioSource>();
            Invoke("CheckAllAudioSources", checkFrequency);   
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSources.Count != 0) 
        {
            int i = 0;
            while (i < audioSources.Count)
            {
                if (!audioSources[i].isPlaying)
                {
                    var source = audioSources[i];
                    audioSources.Remove(source);
                    Destroy(source);
                }
                else ++i;
            }
        }
    }
    void CheckAllAudioSources()
    {
        
    }
    public void PlaySound(AudioClip clip, bool looping, bool soundFatigue=false, float volume=1f)
    {
        AudioSource source = Instantiate(sourceTemplate, transform);
        if (looping)
        {
            source.loop = true;
        }
        if (soundFatigue)
        {
            source.pitch += Random.Range(-0.5f, 0.5f);
        }
        source.clip = clip;
        source.volume = volume;
        source.Play();
        if (audioSources == null) audioSources = new List<AudioSource>();
        if (source != null && audioSources != null) audioSources.Add(source);
    }
    public void ClearAllSounds()
    {
        while(audioSources.Count > 0)
        {
            var source = audioSources[0];
                    audioSources.Remove(source);
                    Destroy(source);
        }
    }
    void OnApplicationQuit()
    {
        audioSources.Clear();
    }
}
