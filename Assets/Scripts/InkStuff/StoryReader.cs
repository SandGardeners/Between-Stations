using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryReader : MonoBehaviour {

	[SerializeField]
	TextBox mainTextBox;

	[SerializeField]
	ChoiceManager choiceManager;

    [SerializeField]
    TextBox nameTextBox;

    [SerializeField]
    GameObject clickCatcher;

    string lastState;

    float alpha;

    public void ChangeBackground(string content)
    {
        int index = int.Parse(content);
        if(channel.backgrounds[index] != null)
        {
            channel.GetComponent<Image>().sprite = channel.backgrounds[index];
        } 
    }

    Channel channel;

    float timer;
    public float sfxDelay;

    bool mustDelete = false;

    AudioSource source;
    public bool canPlay = false;

    // [SerializeField]
    // Fader fader;

    // [SerializeField]
    // bool introDone = false;


    void Start () {
source = GetComponent<AudioSource>();
        choiceManager.Input += MadeChoice;
        mainTextBox.finishedCallback += DebugMe;
        InkOverlord.IO.Shutdown += Shutdown;

        ReadKnot("INTRO");
        Proceed();
    }

    void DebugMe()
    {
        if(InkOverlord.IO.canContinue && alpha > 0.75f)
            lastState = InkOverlord.IO.GetState();
    }

    public void FeedChannel(Channel c)
    {
        channel = c;
    }

    void ShowBox()
    {
        clickCatcher.SetActive(true);
        mainTextBox.transform.parent.gameObject.SetActive(true);
        CancelInvoke();
        Proceed(true);
    }

    public void ReadKnot(string knot)
    {
        InkOverlord.IO.RequestKnot(knot);
        ShowBox();
        //ShowBox();
    }

    public void LoadStateAndPlay(string save)
    {
        lastState = save;
        InkOverlord.IO.SetState(save);
        ShowBox();
    }

    public string SaveStateAndClose()
    {
        string state = lastState;
        CloseTextBox();
        return state;
    }

	public void MadeChoice(int i)
    {
        choiceManager.ClearChoices();
        InkOverlord.IO.MakeChoice(i);
        mainTextBox.ReadLine(0f, 1f, InkOverlord.IO.NextLine());
    }
	
    public void Proceed(bool force = false)
    {
        if (!choiceManager.IsBusy && (alpha > 0.75f || force || !canPlay))
        {
            if(mainTextBox._isReading)
            {
                mainTextBox.DisplayImmediate();
            }
            else if (InkOverlord.IO.canContinue)
            {
                lastState = InkOverlord.IO.GetState();
                mainTextBox.ReadLine(0f, 1f, InkOverlord.IO.NextLine());
            }
            else if (InkOverlord.IO.hasChoices)
            {
                choiceManager.FeedChoices(InkOverlord.IO.GetChoices());
                choiceManager.DisplayChoices();
            }
            else
            {
                CloseTextBox();
                if(mustDelete)
                {
                    DeleteChannel();
                    mustDelete = false;
                }
            }
        }

    }

    void Shutdown()
    {
        mustDelete = true;
    }

    void DeleteChannel()
    {
        if(channel != null)
        {
            channel.IsOver();
            channel = null;
        }
        else
        {
            canPlay = true;
        }
    }

    void Update()
    {
        if(mainTextBox._isReading)
        {
            timer += Time.deltaTime;
            if(timer > sfxDelay)
            {
                timer = 0f;
                if(channel != null)
                {
                    if(channel.voices.Length > 0)
                        source.PlayOneShot(channel.voices[UnityEngine.Random.Range(0, channel.voices.Length)],0.1f);
                }
            }
        }
        else
        {
            timer = 0f;
        }
    }

    internal void SetGlobalAlpha(float _alpha)
    {
        alpha = _alpha;
        mainTextBox.SetAlpha(alpha);
        choiceManager.SetAlpha(alpha);
        nameTextBox.SetAlpha(alpha);
    }

    public void SetName(string name)
    {
        nameTextBox.transform.parent.gameObject.SetActive(name != string.Empty);
        if(name != string.Empty && name != nameTextBox.CurrentString)
        {
            nameTextBox.ReadLine(name);
        }   
    }


    void CloseTextBox()
    {
        lastState = "";
        nameTextBox.ReadLine("");
        mainTextBox.ReadLine("");
        mainTextBox._isReading = false;
        choiceManager.ClearChoices();
        nameTextBox.transform.parent.gameObject.SetActive(false);
        Invoke("RemoveTrigger", 0.1f);
        mainTextBox.transform.parent.gameObject.SetActive(false);
        // if(!introDone)
        // {   
        //     fader.Init();
        //     fader.allBlackDelegate += RemoveBackground;
        // }   
    }

    void RemoveTrigger()
    {
        clickCatcher.SetActive(false);
    }

    void RemoveBackground()
    {
        // introDone = true;
        // fader.allBlackDelegate -= RemoveBackground;
        clickCatcher.SetActive(false);
    }
    /*
    public InputField inputField;

    public void OpenInputField()
    {
        inputField.gameObject.SetActive(true);
        inputField.Select();
        inputField.ActivateInputField();
    }
*/
}
