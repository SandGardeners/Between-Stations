using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour {
	

	  public delegate void VoidDelegate();
    public VoidDelegate allBlackDelegate;
    [SerializeField]
    float timeToBlack;

    bool started;
    bool reached;

[SerializeField]
    int whitenessValue;

    float timer;

    Image _image;

    // Use this for initialization
    void Start () {
        allBlackDelegate += Reverse;
        _image = GetComponent<Image>();
        _image.raycastTarget = false;
    }

	void Reverse()
	{
        reached = true;
    }
	
	// Update is called once per frame
	void Update () 
	{
		if(started)
		{
			if(!reached)
			{
            	timer += Time.deltaTime;
			}
			else
			{
                timer -= Time.deltaTime;
            }
			if(timer >= timeToBlack)
			{
				if(allBlackDelegate != null)
                    allBlackDelegate();
            }
			else if(timer <= 0)
			{
                started = false;
                _image.raycastTarget = false;
            }

            _image.color = new Color(whitenessValue, whitenessValue, whitenessValue, Mathf.Lerp(0f, 1f, timer / timeToBlack));
        }	
	}

    public void Init()
    {
		if(!started)
		{
            started = true;
            reached = false;
            timer = 0f;
            _image.raycastTarget = true;
        }
    }
}
