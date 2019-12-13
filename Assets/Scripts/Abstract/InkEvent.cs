using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InkEvent : MonoBehaviour 
{
    [SerializeField]
    public string _id;
    // Use this for initialization
    void Start () 
	{
        InkEventWatcher.Subscribe(this);
    }

	public abstract void Trigger();

    void OnDestroy()
	{
        InkEventWatcher.Unsubscribe(this);
    }
}
