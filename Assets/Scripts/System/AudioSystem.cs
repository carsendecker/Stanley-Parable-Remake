using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Carsen Decker
//USAGE: Put this on an object tagged "AudioSystem" with 2 AudioSources
//FUNCTION: Acts as a centralized place to handle playing audio. Especially useful for managing narration lines overlapping each other
public class AudioSystem : MonoBehaviour
{
    public AudioSource NarrationAudio;
    public AudioSource SoundEffectAudio;

    
    /// <summary>
    /// Originally used to play a list of VoiceLines in sequence, playing next one after the last one is finished
    /// </summary>
    /// <param name="voiceLines">An array of VoiceLines, a struct that contains both an audio file and a subtitle string</param>
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

    /// <summary>
    /// Plays a line of narration from its assigned AudioSource. Will cut off the currently playing voice line if one is playing
    /// </summary>
    /// <param name="clip">The audio clip to play</param>
    public void PlayNarration(AudioClip clip)
    {
        NarrationAudio.Stop();
        NarrationAudio.clip = clip;
        NarrationAudio.Play();
    }

}
