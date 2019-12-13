using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverquick : MonoBehaviour {

    // Use this for initialization
    RectTransform rectTransform;

	void Start()
	{
        rectTransform = GetComponent<RectTransform>();
    }

	public void Hover(bool on)
	{
		if(on)
            rectTransform.localScale = Vector3.one * 1.15f;
		else
            rectTransform.localScale = Vector3.one;
    }
}
