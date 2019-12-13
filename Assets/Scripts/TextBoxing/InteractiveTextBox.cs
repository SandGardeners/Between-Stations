using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTextBox : TextBox 
{
    public bool _isAvailable;

    public void Start()
    {
        RealStart();
        finishedCallback += MakeAvailable;
    }

    public void MakeAvailable()
    {
        _isAvailable = true;
    }
	

    public void DefaultStyle()
    {
        _textComponent.color = _colorDefault;
    }

    public void HoverStyle()
    {
		
        _textComponent.color = _hoverColor;
    }

    public void PressedStyle()
    {
		
        _textComponent.color = _pressedColor;
    }

    [SerializeField]
    Color _hoverColor;
    [SerializeField]
    Color _pressedColor;
}
