using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser : TagsParser {

    [SerializeField] StoryReader reader;
    public override void ParseTag(string tagHeader, string content)
    {
        switch(tagHeader)
		{
			case "name":
                reader.SetName(content);
                break;
            case "background":
                reader.ChangeBackground(content);
                break;
			default:
                Debug.LogError("Not implemented tag");
                break;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
