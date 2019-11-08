using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public AudioSource NarrationAudio;
    public AudioSource SoundEffectAudio;

    void Update()
    {
        
    }

    public void PlayNarration(AudioClip clip)
    {
        NarrationAudio.Stop();
        NarrationAudio.clip = clip;
        NarrationAudio.Play();
    }

}
