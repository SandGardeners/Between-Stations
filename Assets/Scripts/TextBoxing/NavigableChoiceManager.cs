using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigableChoiceManager : ChoiceManager 
{
    int index = 0;
    private bool _released;
    private float _timer;
    public float _waitTimer;
    bool _releasedInteract;

    // Use this for initialization
    void Start () 
	{

	}

    void UpdateIndex(int offset)
    {
        InteractiveTextBox[] children = GetComponentsInChildren<InteractiveTextBox>();
        if (children.Length > 0)
        {
            if (index + offset < 0)
                offset = children.Length - 1;
            else if (index + offset >= children.Length)
                offset = -index;
            Debug.Log(index);

            children[index].DefaultStyle();
            index += offset;
            children[index].HoverStyle();

            Debug.Log(index);
        }
    }
    //    transform.GetChild(i).GetComponent<TextBox>().Hover();

    // Update is called once per frame
    void Update()
    {
        if (IsBusy)
        {

            float verticalAxis = UnityEngine.Input.GetAxis("Menu Y");

            if (Mathf.Abs(verticalAxis) > 0)
            {
                if (_released || _timer <= 0f)
                {
                    UpdateIndex(1 * (int)Mathf.Sign(verticalAxis));
                    _released = false;
                    _timer = _waitTimer;
                }
                _timer -= Time.deltaTime;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
            {
                UpdateIndex(-1);
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
            {
                UpdateIndex(1);
            }
            else
            {
                UpdateIndex(0);
                _released = true;
            }

            if (UnityEngine.Input.GetButtonDown("Interact") || UnityEngine.Input.GetMouseButtonDown(0))
            {
                if (_releasedInteract)
                {
                    _releasedInteract = false;
					if(GetComponentsInChildren<InteractiveTextBox>()[index]._isAvailable)
	                    Input(index);
                }
            }
            else
            {
                _releasedInteract = true;
            }
        }

    }
}
