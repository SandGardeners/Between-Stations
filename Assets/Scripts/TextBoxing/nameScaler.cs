using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class nameScaler : MonoBehaviour {
     TMP_Text t;

    
	// Use this for initialization
	void Start ()
    {
        t = GetComponentInChildren<TMP_Text>();	
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, t.preferredWidth+20);
        t.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, t.preferredWidth+50);
	}
}
