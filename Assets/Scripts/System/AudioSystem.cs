using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    public AudioSource NarrationAudio;
    public AudioSource SoundEffectAudio;
    
    public 

    void Update()
    {
        
    }
    
    IEnumerator PlayNarrationSeries(VoiceLine[] voiceLines)
    {
        foreach (VoiceLine line in voiceLines)
        {
            //Only executes on the first voice line, interrupts any previous playing narration
//            if (!alreadyPlayed)
//            {
//                PlayNarration(line.LineAudio);
//                alreadyPlayed = true;
//                continue;
//            }
            
            //Waits until the previous line is done playing
            yield return new WaitUntil(() => NarrationAudio.isPlaying);
            
                PlayNarration(line.LineAudio);
        }
    }

    public void PlayNarration(AudioClip clip)
    {
        NarrationAudio.Stop();
        NarrationAudio.clip = clip;
        NarrationAudio.Play();
    }

}
