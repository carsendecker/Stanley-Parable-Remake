﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//USAGE: put this on the NarrationTriggerSystem and it generates the main narration
public class NarrationTrigger : MonoBehaviour
{
    [Serializable]
    public struct VoiceLine
    {
        public AudioClip LineAudio;
        [TextArea] public string Subtitle;
    }
    
    public TMPro.TMP_Text MainNarration;

    public GameObject Panel;

    public bool GameStart;

    public int TextNumber,OfficeTextNumber;

    public Animation PanelDisappear, TextDisappear;

    private bool isAdding;
    private bool EnterOffice;

    public AudioClip[] sounds;
    
    public VoiceLine[] voiceLines;
    
    private AudioSource NarrationAudio;
    private AudioSystem audioSystem;
    private bool alreadyPlayed;

    
    // Start is called before the first frame update
    void Start()
    {
        NarrationAudio = GameObject.FindGameObjectWithTag("AudioSystem").GetComponent<AudioSource>();
        MainNarration.text = "";
        Panel.SetActive(false);
        NarrationAudio.Stop();
        TextNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            GameStart = true;
//            
//        }

        if (!EnterOffice)
        {
            if (GameStart && TextNumber == 0)
            {
                MainNarration.text = "Something was very clearly wrong. Shocked, frozen solid, Stanley found himself" +
                                     "unable to move for\n the longest time.";
            
                Panel.SetActive(true);
                
                if (!NarrationAudio.isPlaying)
                {
                    NarrationAudio.clip = sounds[TextNumber];
                    NarrationAudio.Play();
                    StartCoroutine(timer(sounds[TextNumber].length));
                }
            }

            if (TextNumber == 1)
            {
                PanelDisappear.Play();
                TextDisappear.Play();
                MainNarration.text = "But as he came to his wits and regained his senses, he got up from his desk and" +
                                     "stepped out of his\noffice.";
                Panel.SetActive(true);
                if (!NarrationAudio.isPlaying)
                {
                    NarrationAudio.clip = sounds[TextNumber];
                    NarrationAudio.Play();
                    StartCoroutine(timer(sounds[TextNumber].length));
                }
            }
            if (TextNumber == 2)
            {
                PanelDisappear.Play();
                TextDisappear.Play();
                MainNarration.text = "But as he came to his wits and regained his senses, he got up from his desk and" +
                                     "stepped out of his\noffice.";
                Panel.SetActive(true);
                if (!NarrationAudio.isPlaying)
                {
                    NarrationAudio.clip = sounds[TextNumber];
                    NarrationAudio.Play();
                    StartCoroutine(timer(sounds[TextNumber].length));
                }
            }
            if (TextNumber == 3)
            {
                MainNarration.text = "But as he came to his wits and regained his senses, he got up from his desk and" +
                                     "stepped out of his\noffice.";
                Panel.SetActive(true);
                if (!NarrationAudio.isPlaying)
                {
                    NarrationAudio.clip = sounds[TextNumber];
                    NarrationAudio.Play();
                    StartCoroutine(timer(sounds[TextNumber].length));
                }
            }
        }

        if (OfficeTextNumber == 1)
        {
            MainNarration.text = "Stanley decided to go to the meeting room; perhaps he had simply missed a memo.";
            Panel.SetActive(true);
            if (!NarrationAudio.isPlaying)
            {
                NarrationAudio.PlayOneShot(sounds[3]);
            }
        }
    }
    
    IEnumerator timer(float seconds)
    {
        isAdding = true;
        yield return new WaitForSeconds(seconds);
        TextNumber++;
        isAdding = false;
        
    }
    /*
    IEnumerator Officetimer(float seconds)
    {
        isAdding = true;
        yield return new WaitForSeconds(seconds);
        OfficeTextNumber++;
        isAdding = false;
    } */

    public void OnTriggerEnter(Collider other)
    {
        if (alreadyPlayed)
        {
            return;
        }
        
        if (other.gameObject.name == "Office" & !EnterOffice)
        {
            NarrationAudio.Stop();
            EnterOffice = true;
            MainNarration.text = "All of his co-workers were gone. What could it mean?";
            Panel.SetActive(true);
            if (!NarrationAudio.isPlaying)
            {
                NarrationAudio.PlayOneShot(sounds[2]);
            }

            if (!NarrationAudio.isPlaying)
                OfficeTextNumber++;
        }

        StartCoroutine(PlayNarrationSeries());

    }

    IEnumerator PlayNarrationSeries()
    {
        foreach (VoiceLine line in voiceLines)
        {
            //Only executes on the first voice line, interrupts any previous playing narration
            if (!alreadyPlayed)
            {
                audioSystem.PlayNarration(line.LineAudio);
                alreadyPlayed = true;
                continue;
            }
            
            //Waits until the previous line is done playing
            yield return new WaitUntil(() => !audioSystem.NarrationAudio.isPlaying);
            
            audioSystem.PlayNarration(line.LineAudio);
        }
    }
    
}
