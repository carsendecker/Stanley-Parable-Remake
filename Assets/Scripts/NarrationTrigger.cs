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
    [Space(10)]
    public bool CowardEnding;
    
    
    private AudioSystem audioSystem; //Reference to main system for playing narration and sound effects
    private bool alreadyPlayed;

    [Space(10)]
    public Animation PanelDisappear, TextDisappear;

    private bool isAdding;
    public int AllLineNumber; //This indicates how many lines this trigger will trigger in total

    public AudioSource StartMusicSource;
    public bool TurnOffMusic;

    
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
            //FOR DEBUGGING PURPOSES
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

        if (TurnOffMusic)
            StartCoroutine(FadeOutMusic());
        
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

                if (EndlessEndingPanic)
                {
                    //Plays music and makes the screen red
                    StartCoroutine(EndlessPanic());
                }

                //Activates subtitle text
                MainNarration.text = line.Subtitle;
                //Activates subtitle panel
                Panel.SetActive(true);
                PanelDisappear.Play("PanelAppear");
                TextDisappear.Play("TextAppear");
                

                continue;
            }
            
            
            //Waits until the previous line is done playing, then plays the next one
            yield return new WaitUntil(() => !audioSystem.NarrationAudio.isPlaying);
            audioSystem.PlayNarration(line.LineAudio);
            lineNumber++;
            
            

            //Checks for multiple special cases, typically for scripted events in each ending
            if (EndlessEnding)
            {
                EndlessCheck(lineNumber);
            }
            else if (EndlessEndingPanic)
            {
                EndlessPanicCheck(lineNumber);
            }
            else if (CowardEnding)
            {
                 CowardEndingCheck(lineNumber);
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

    IEnumerator FadeOutMusic()
    {
        float t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime;
            StartMusicSource.volume = t;
            yield return 0;
        }
        StartMusicSource.Stop();
    }
    
    
    
    //------------------------------------//
    //        ENDLESS ENDING METHODS      //
    //------------------------------------//

    //For the endless ending, where certain scripted effects happen during specific voice lines
    void EndlessCheck(float lineNumber)
    {
        if (lineNumber == 11)
        {
            //Makes Stanley float
            StartCoroutine(EndlessFloating());
        }
        else if (lineNumber == 12)
        {
            //Makes fog and star particles float around
            StartCoroutine(EndlessFog());
        }
        else if (lineNumber == 24)
        {
            //Make stanley's eyes close for a bit
            StartCoroutine(EndlessEyeClose());
        }
        else if (lineNumber == voiceLines.Length - 1)
        {
            //Make stanley's eyes open, then start a new "panic" dialogue
            StartCoroutine(EndlessEyeOpen());
        }
    }

    //For endless ending after eye close, as these voice lines are contained in a separate instantiated trigger
    void EndlessPanicCheck(float lineNumber)
    {

        if (lineNumber == voiceLines.Length - 1)
        {
            //Blacks out the screen and reloads the scene
            StartCoroutine(EndlessPanicBlack());
        }
    }

    //For when Stanley closes the door to his office
    void CowardEndingCheck(float lineNumber)
    {
        if (lineNumber == voiceLines.Length - 1)
        {
            //Reloads the scene again
            StartCoroutine(CowardEndingCutoff());
        }
    }
    
    
    IEnumerator EndlessFloating()
    {
        PlayerController stanley = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        
        float oGrav = stanley.Gravity; //Stanleys original gravity (for some reason this doesn't work/store properly???)

        stanley.Gravity = 0f;

        //Moves stanley up for a bit
        float t = 0;
        while (t < 4f)
        {
            t += Time.deltaTime;
            Vector3 newPos = stanley.transform.position;
            newPos.y += 0.5f;
            
            stanley.GetComponent<CharacterController>().Move(new Vector3(0, 0.012f, 0));
            
            yield return 0;
        }
                
        yield return new WaitForSeconds(2.5f);

        stanley.Gravity = 4;
    }

    
    IEnumerator EndlessFog()
    {
        //Turns on scene fog
        RenderSettings.fog = true;
        float t = 0;

        Transform stanTran = GameObject.FindWithTag("Player").transform;
        
        //Slowly increases fog density
        while (t < 1f)
        {
            t += Time.deltaTime * 0.4f;
            float fogAmount = Mathf.Lerp(0, 0.4f, t);

            RenderSettings.fogDensity = fogAmount;

            yield return 0;
        }
        
        //Creates particles for stars
        GameObject starParticles = Instantiate(StarfieldParticles, stanTran.position, Quaternion.identity);
        starParticles.transform.parent = stanTran;
        
        yield return new WaitForSeconds(6f);

        //Slowly lowers fog density
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
        //Makes a black UI panel fade in to black out screen
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
        //Fades out the black panel
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

        //Creates a new narration trigger at stanley's location, which begins the panic sequence
        GameObject stanley = GameObject.FindWithTag("Player");
        GameObject newTrigger = Instantiate(FinalNarrationTrigger, stanley.transform.position, Quaternion.identity);

        NarrationTrigger newTrigScript = newTrigger.GetComponent<NarrationTrigger>();

        //Assign the new trigger's variables for it
        newTrigScript.EyeClosePanel = EyeClosePanel;
        newTrigScript.MainNarration = MainNarration;
        newTrigScript.Panel = Panel;
        newTrigScript.PanelDisappear = PanelDisappear;
        newTrigScript.TextDisappear = TextDisappear;
    }

    
    IEnumerator EndlessPanic()
    {
        yield return new WaitForSeconds(2f);
        
        audioSystem.MusicAudio.clip = PanicMusic;
        audioSystem.MusicAudio.Play();

        EyeClosePanel.SetActive(true);
        Image panel = EyeClosePanel.GetComponent<Image>();

        //Slowly turns screen tint red
        float alpha = 0;
        while (alpha < 0.6f)
        {
            alpha += Time.deltaTime * 0.13f;
            panel.color = Color.Lerp(Color.clear, Color.red, alpha);
            yield return 0;
        }
                
    }

    IEnumerator EndlessPanicBlack()
    {
        //Stops music, waits, then blacks out screen
        audioSystem.MusicAudio.Stop();
        yield return new WaitForSeconds(2f);
        Image panel = EyeClosePanel.GetComponent<Image>();
        panel.color = Color.black;
        
        //reloads the scene to start again
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator CowardEndingCutoff()
    {
        yield return new WaitForSeconds(audioSystem.NarrationAudio.clip.length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
  
    
}
