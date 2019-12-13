using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Channel : MonoBehaviour 
{
    public string knot;
    
    [HideInInspector]
	public string save;

    [HideInInspector]
    public AudioSource source;

    public Sprite[] backgrounds;

    public AudioClip[] voices;

    public delegate void doneDelegate();
    public doneDelegate IsOver;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    
}
