using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public TMP_Text MainNarration; //Subtitle text
    public GameObject Panel;
    
    public VoiceLine[] voiceLines; //Lines that this trigger will play

    [Header("Special Triggers")]
    public bool EndlessEnding;
    public GameObject StarfieldParticles, EyeClosePanel, FinalNarrationTrigger;
    [Space(10)] 
    public bool EndlessEndingPanic; //This one is for the trigger spawned at the end of the EndlessEnding trigger
    public AudioClip PanicMusic;
    
    
    private AudioSystem audioSystem; //Reference to main system for playing narration and sound effects
    private bool alreadyPlayed;

    public Animation PanelDisappear, TextDisappear;

    private bool isAdding;
    public int AllLineNumber; //This indicates how many lines this trigger will trigger in total

    
    void Start()
    {
       audioSystem = GameObject.FindGameObjectWithTag("AudioSystem").GetComponent<AudioSystem>();

        if (Panel == null)
        {
            Panel = GameObject.FindWithTag("SubPanel");
        }

        if (MainNarration == null)
        {
            MainNarration = GameObject.FindWithTag("SubText").GetComponent<TMP_Text>();
        }

       PanelDisappear = Panel.GetComponent<Animation>();
       TextDisappear = MainNarration.GetComponent<Animation>();

        MainNarration.text = "";
        Panel.SetActive(false);
    }

    private void Update()
    {
        if (EndlessEnding)
        {
            //FOR DEBUGGING PURPOSES ONLY, DELETE WHEN DONE
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                StartCoroutine(EndlessFloating());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                StartCoroutine(EndlessFog());
            }
        }
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
        int lineNumber = 0;
        bool EndNarration = false;
        foreach (VoiceLine line in voiceLines)
        {
            if (!alreadyPlayed)
            {
                //Plays very first voice line, interrupting any previously playing narration series
                audioSystem.PlayNarration(line.LineAudio);
                alreadyPlayed = true;
                
                //Activates subtitle text
                MainNarration.text = line.Subtitle;
                //Activates subtitle panel
                Panel.SetActive(true);
                PanelDisappear.Play("PanelAppear");
                TextDisappear.Play("TextAppear");
                

                continue;
            }
            
            //Plays subtitle panel animation, and transitions between subtitle lines
//            PanelDisappear.Play();
//            TextDisappear.Play();
            
//              MainNarration.text = line.Subtitle;
            
//            Panel.SetActive(true);
            
            //Waits until the previous line is done playing, then plays the next one
            
            yield return new WaitUntil(() => !audioSystem.NarrationAudio.isPlaying);
            audioSystem.PlayNarration(line.LineAudio);
            lineNumber++;
            
            
            
            
            Debug.Log(EndNarration);

                if (EndlessEnding)
            {
                EndlessCheck(lineNumber);
            }
            else if (EndlessEndingPanic)
            {
                EndlessPanicCheck(lineNumber);
            }
            if (!PanelDisappear.isPlaying && alreadyPlayed)
            {
                PanelDisappear.Play("PanelTransition");
                TextDisappear.Play("TextTransition");
                yield return new WaitForSeconds(0.3f); 
                TextDisappear.Play("TextAppear");
                MainNarration.text = line.Subtitle;
            }
        }
        Debug.Log(lineNumber);
        Debug.Log("alreadyPlayed =" + alreadyPlayed);
        
        
        
        yield return new WaitUntil(() => !audioSystem.NarrationAudio.isPlaying);

        if (lineNumber > AllLineNumber-2)
        {
            EndNarration = true;
        }

        if (!PanelDisappear.isPlaying && EndNarration)
        {
            PanelDisappear.Play("PanelDisappear");
            TextDisappear.Play("TextDisappear");
            alreadyPlayed = false;
        }
        
        Debug.Log("alreadyPlayed =" + alreadyPlayed);
        //When done, turn off subtitle panel
    }
    
    //------------------------------------//
    //        ENDLESS ENDING METHODS      //
    //------------------------------------//

    void EndlessCheck(float lineNumber)
    {
        if (lineNumber == 11)
        {
            StartCoroutine(EndlessFloating());
        }
        else if (lineNumber == 12)
        {
            StartCoroutine(EndlessFog());
        }
        else if (lineNumber == 24)
        {
            //Make stanley's eyes close for a bit
            StartCoroutine(EndlessEyeClose());
        }
        else if (lineNumber == voiceLines.Length - 1)
        {
            //Make stanley's eyes open and turn everything red, then start a new dialogue
            StartCoroutine(EndlessEyeOpen());
        }
    }

    void EndlessPanicCheck(float lineNumber)
    {
        if (lineNumber == 1)
        {
            audioSystem.MusicAudio.clip = PanicMusic;
            audioSystem.MusicAudio.Play();
            StartCoroutine(EndlessPanic());
        }
        else if (lineNumber == voiceLines.Length - 1)
        {
            StartCoroutine(EndlessPanicBlack());
        }
    }
    
    
    IEnumerator EndlessFloating()
    {
        PlayerController stanley = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
//        stanley.CanMove = false;
        
        float oGrav = stanley.Gravity; //Stanleys original gravity (for some reason this doesn't work/store properly???)

        stanley.Gravity = 0f;

        float t = 0;
        while (t < 3f)
        {
            t += Time.deltaTime;
            Vector3 newPos = stanley.transform.position;
            newPos.y += 0.5f;
            
            stanley.GetComponent<CharacterController>().Move(new Vector3(0, 0.01f, 0));
            
            yield return 0;
        }
                
        yield return new WaitForSeconds(2f);

        stanley.Gravity = 4;
    }

    IEnumerator EndlessFog()
    {
        RenderSettings.fog = true;
        float t = 0;

        Transform stanTran = GameObject.FindWithTag("Player").transform;
        

        while (t < 1f)
        {
            t += Time.deltaTime * 0.4f;
            float fogAmount = Mathf.Lerp(0, 0.4f, t);

            RenderSettings.fogDensity = fogAmount;

            yield return 0;
        }
        
        GameObject starParticles = Instantiate(StarfieldParticles, stanTran.position, Quaternion.identity);
        starParticles.transform.parent = stanTran;
        
        yield return new WaitForSeconds(6f);

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime;
            float fogAmount = Mathf.Lerp(0.7f, 0, t);

            RenderSettings.fogDensity = fogAmount;

            yield return 0;
        }
        
        RenderSettings.fog = false;
        Destroy(starParticles);
    }

    IEnumerator EndlessEyeClose()
    {
        EyeClosePanel.SetActive(true);
        Image panel = EyeClosePanel.GetComponent<Image>();

        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * 0.4f;
            panel.color = Color.Lerp(Color.clear, Color.black, alpha);
            yield return 0;
        }
    }

    IEnumerator EndlessEyeOpen()
    {
        yield return new WaitForSeconds(4f);
        Image panel = EyeClosePanel.GetComponent<Image>();

        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * 0.3f;
            panel.color = Color.Lerp(Color.black, Color.clear, alpha);
            yield return 0;
        }
        
        EyeClosePanel.SetActive(false);

        GameObject stanley = GameObject.FindWithTag("Player");
        GameObject newTrigger = Instantiate(FinalNarrationTrigger, stanley.transform.position, Quaternion.identity);

        NarrationTrigger newTrigScript = newTrigger.GetComponent<NarrationTrigger>();

        newTrigScript.EyeClosePanel = EyeClosePanel;
        newTrigScript.MainNarration = MainNarration;
        newTrigScript.Panel = Panel;
        newTrigScript.PanelDisappear = PanelDisappear;
        newTrigScript.TextDisappear = TextDisappear;
    }

    
    IEnumerator EndlessPanic()
    {
//        yield return new WaitForSeconds(2f);

        EyeClosePanel.SetActive(true);
        Image panel = EyeClosePanel.GetComponent<Image>();

        float alpha = 0;
        while (alpha < 0.6f)
        {
            alpha += Time.deltaTime * 0.1f;
            panel.color = Color.Lerp(Color.clear, Color.red, alpha);
            yield return 0;
        }
                
    }

    IEnumerator EndlessPanicBlack()
    {
        audioSystem.MusicAudio.Stop();
        yield return new WaitForSeconds(2f);
        Image panel = EyeClosePanel.GetComponent<Image>();
        panel.color = Color.black;
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
