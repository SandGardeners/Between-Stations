using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TextBoxScalerSimple : MonoBehaviour {

    private TMP_Text textReader;
    private RectTransform tr;
    float curHeight;
    float startHeight;

    TextMeshBox tBox;
    
	void Start () 
    {
        textReader = GetComponent<TMP_Text>();
        tr = GetComponent<RectTransform>();
        curHeight = tr.sizeDelta.y;
        startHeight = curHeight;
        
        GetComponent<TextMeshBox>().newLineCallback += ResetHeight;
    }

    void ResetHeight()
    {
        curHeight = startHeight;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if(textReader.isTextOverflowing)
        {
            curHeight += 10f;
        }
        if(curHeight != textReader.rectTransform.sizeDelta.y)
            textReader.rectTransform.sizeDelta = new Vector2(tr.sizeDelta.x, curHeight);
    }
}
