using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TagsParser : MonoBehaviour 
{
    public abstract void ParseTag(string tagHeader, string content);
	
}
