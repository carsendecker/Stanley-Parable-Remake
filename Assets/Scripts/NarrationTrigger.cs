using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Struct that groups together an audio voice clip with its subtitle
/// </summary>
[Serializable]
public struct VoiceLine
{
    public AudioClip LineAudio;
    [TextArea] public string Subtitle;
}

//USAGE: put this on the NarrationTriggerSystem and it generates the main narration
public class NarrationTrigger : MonoBehaviour
{
    public TMPro.TMP_Text MainNarration; //Subtitle text
    public GameObject Panel;
    public bool GameStart;
    
    public VoiceLine[] voiceLines; //Lines that this trigger will play
    
    private AudioSystem audioSystem; //Reference to main system for playing narration and sound effects
    private bool alreadyPlayed;
    
    private Animation PanelDisappear, TextDisappear;
    private bool isAdding;

    
    void Start()
    {
        audioSystem = GameObject.FindGameObjectWithTag("AudioSystem").GetComponent<AudioSystem>();

        PanelDisappear = Panel.GetComponent<Animation>();
        TextDisappear = MainNarration.GetComponent<Animation>();

        MainNarration.text = "";
        Panel.SetActive(false);
    }
    
    //Basic timer, waiting for certain amount of seconds
    IEnumerator timer(float seconds)
    {
        isAdding = true;
        yield return new WaitForSeconds(seconds);
        isAdding = false;
        
    }

    //Starts the narration when player enters the trigger
    public void OnTriggerEnter(Collider other)
    {
        if (alreadyPlayed)
        {
            return;
        }
        
        StartCoroutine(PlayNarrationSeries());

    }

    /// <summary>
    /// Plays the audio files stored in voiceLines[], along with its subsequent subtitle, in sequence
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayNarrationSeries()
    {
        foreach (VoiceLine line in voiceLines)
        {
            if (!alreadyPlayed)
            {
                //Plays very first voice line, interrupting any previously playing narration series
                audioSystem.PlayNarration(line.LineAudio);
                alreadyPlayed = true;
                
                //Activates subtitle panel
                Panel.SetActive(true);

                continue;
            }
            
            //Plays subtitle panel animation, and transitions between subtitle lines
            PanelDisappear.Play();
            TextDisappear.Play();
            
            MainNarration.text = line.Subtitle;
            
            Panel.SetActive(true);
            
            //Waits until the previous line is done playing, then plays the next one
            yield return new WaitUntil(() => !audioSystem.NarrationAudio.isPlaying);
            
            audioSystem.PlayNarration(line.LineAudio);
        }
        
        //When done, turn off subtitle panel
        Panel.SetActive(false);
    }
    
}
