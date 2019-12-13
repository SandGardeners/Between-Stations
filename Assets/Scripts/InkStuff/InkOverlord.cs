using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;

public class InkOverlord : MonoBehaviour 
{
    static InkOverlord _instance;

    public static InkOverlord IO
	{
		get 
		{
            return _instance;
        }
	}

    [SerializeField] TextAsset storyScript;
    Story inkStory;

    TagsParser tagsParser;
    public void ChangeVariable(string _key, object _v)
    {
        inkStory.variablesState[_key] = _v;
    }

    public bool canContinue
	{
		get 
		{
            return inkStory.canContinue;
        }
	}

    public bool hasChoices
    {
        get
        {
            return inkStory.currentChoices.Count > 0;
        }
    }
	public string NextLine()
	{
        string line = inkStory.Continue();
        if(tagsParser != null)
            GetTags();
        return line;
    }

	public List<Choice> GetChoices()
	{
        return inkStory.currentChoices;
    }

    public void SetState(string save)
    {
        inkStory.state.LoadJson(save);
    }

    public string GetState()
    {
        return inkStory.state.ToJson();
    }

    public bool MakeChoice(int index)
	{
		if(index < inkStory.currentChoices.Count)
		{
            inkStory.ChooseChoiceIndex(index);
            return true;
        }
        Debug.LogError("INVALID CHOICE INDEX");
        return false;
    }

    public delegate void shutdownDelegate();
    public shutdownDelegate Shutdown;

    // Use this for initialization
    void Awake()
    { 
	
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        inkStory = new Story(storyScript.text);
        tagsParser = GetComponent<TagsParser>();
        
        inkStory.BindExternalFunction("Shutdown", () =>
        {
            if(Shutdown != null)
                Shutdown();
        });

    }

	public void RequestKnot(string knotPath)
	{
        inkStory.ChoosePathString(knotPath);
    }

    public void GetTags()
    {
        List<string> tags = inkStory.currentTags;
        if(tags.Count > 0)
        {
            foreach(string s in tags)
            {
                string[] tag = s.Split(':');
                if(tag.Length != 2)
                {
                    Debug.LogError("Invalid tag syntax");
                }
                tagsParser.ParseTag(tag[0], tag[1]);
            }
        }
    }
}
