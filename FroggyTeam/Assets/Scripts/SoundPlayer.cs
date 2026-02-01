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
        audioSources = new List<AudioSource>();
        Invoke("CheckAllAudioSources", checkFrequency);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CheckAllAudioSources()
    {
        if (audioSources.Count != 0) 
        {
            foreach (var source in audioSources)
            {
                if (!source.isPlaying)
                {
                    audioSources.Remove(source);
                    Destroy(source);
                }
            }
        }
        Invoke("CheckAllAudioSources", checkFrequency);
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
        audioSources.Add(source);
    }
}
