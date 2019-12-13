using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : TextMeshBox
{
    SubstringInfo[] textLengthFor;
    protected RectTransform _rt;

    Image bg;
    

    struct SubstringInfo
    {
        public int StartsAt;
        public string TagSuffix;

        public SubstringInfo(int startsAt, IEnumerable<string> tagSuffixChain)
        {
            StartsAt = startsAt;
            TagSuffix = string.Empty;
            foreach (var tagSuffix in tagSuffixChain)
                TagSuffix += tagSuffix;
        }
    }

    public void SetAlpha(float alpha)
    {
        
        if(bg!= null)
        {
        Color c = bg.color;
        c.a = alpha;
        bg.color = c;
        }

        if(_textComponent != null)
        {
            _textComponent.alpha = alpha;
        }
    }
    public override void FeedLine(string s)
    {
        base.FeedLine(s);
        // get text lengths
        var textLengths = new List<SubstringInfo>();
        var fullText = s;
        var tagStack = new Stack<string>();
        int tentativeTagStartAt = -1;
        for (int i = 0; i < fullText.Length; i++)
        {
            char at = fullText[i];

            if (at == '<')
            {
                if (tentativeTagStartAt != -1)
                    // wasn't a tag, add characters from there to here
                    for (int j = tentativeTagStartAt; j < i; j++)
                        textLengths.Add(new SubstringInfo(j, tagStack.ToArray()));

                tentativeTagStartAt = i;
            }
            else if (at == '>')
            {
                if (tentativeTagStartAt != -1)
                {
                    var tag = fullText.Substring(tentativeTagStartAt, i - tentativeTagStartAt);
                    if (tag.StartsWith("<color"))
                        tagStack.Push("</color>");
                    else if (tag.StartsWith("<size"))
                        tagStack.Push("</size>");
                    else if(tag.StartsWith("<align"))
                        tagStack.Push("</align>");
                    else if (tag.StartsWith("<alpha"))
                        tagStack.Push("</alpha>");
                    else if (tag.StartsWith("<b"))
                        tagStack.Push("</b>");
                    else if (tag.StartsWith("<i"))
                        tagStack.Push("</i>");
                    else if (tag.StartsWith("<cspace"))
                        tagStack.Push("</cspace>");
                    else if (tag.StartsWith("<font"))
                        tagStack.Push("</font>");
                    else if (tag.StartsWith("<indent"))
                        tagStack.Push("</indent>");
                    else if (tag.StartsWith("<line-height"))
                        tagStack.Push("</line-height>");
                    else if (tag.StartsWith("<line-indent"))
                        tagStack.Push("</line-indent>");
                    else if (tag.StartsWith("<link"))
                        tagStack.Push("</link>");
                    else if (tag.StartsWith("<lowercase"))
                        tagStack.Push("</lowercase>");
                    else if (tag.StartsWith("<uppercase"))
                        tagStack.Push("</uppercase>");
                    else if (tag.StartsWith("<smallcaps"))
                        tagStack.Push("</smallcaps>");
                    else if (tag.StartsWith("<margin"))
                        tagStack.Push("</margin>");
                    else if (tag.StartsWith("<mark"))
                        tagStack.Push("</mark>");
                    else if (tag.StartsWith("<mspace"))
                        tagStack.Push("</mspace>");
                    else if (tag.StartsWith("<noparse"))
                        tagStack.Push("</noparse>");
                    else if (tag.StartsWith("<nobr"))
                        tagStack.Push("</nobr>");
                    else if (tag.StartsWith("<page"))
                        tagStack.Push("</page>");
                    else if (tag.StartsWith("<pos"))
                        tagStack.Push("</pos>");
                    else if (tag.StartsWith("<size"))
                        tagStack.Push("</size>");
                    else if (tag.StartsWith("<space"))
                        tagStack.Push("</space>");
                    else if (tag.StartsWith("<s"))
                        tagStack.Push("</s>");
                    else if (tag.StartsWith("<u"))
                        tagStack.Push("</u>");
                    else if (tag.StartsWith("<sub"))
                        tagStack.Push("</sub>");
                    else if (tag.StartsWith("<sup"))
                        tagStack.Push("</sup>");
                    else if (tag.StartsWith("<voffset"))
                        tagStack.Push("</voffset>");
                    else if (tag.StartsWith("<width"))
                        tagStack.Push("</width>");
                    else if (tag.StartsWith("<material"))
                        tagStack.Push("</material>");
                    else if (tag.StartsWith("</"))
                        tagStack.Pop(); // assume they'll match...
                    else
                    {
                        // unrecognized tag, parse as text
                        for (int j = tentativeTagStartAt; j < fullText.Length; j++)
                            textLengths.Add(new SubstringInfo(j, tagStack.ToArray()));
                    }

                    tentativeTagStartAt = -1;
                }
                else
                    textLengths.Add(new SubstringInfo(i, tagStack.ToArray()));
            }
            else
            {
                if (tentativeTagStartAt == -1)
                    textLengths.Add(new SubstringInfo(i, tagStack.ToArray()));
            }
        }
        if (tentativeTagStartAt != -1)
            // wasn't a tag, add characters from there to here
            for (int j = tentativeTagStartAt; j < fullText.Length; j++)
                textLengths.Add(new SubstringInfo(j, tagStack.ToArray()));

        // --------

        textLengthFor = textLengths.ToArray();
    }

    void Start()
    {
        _rt = GetComponent<RectTransform>();
        if(_currentString != string.Empty)
        {
            FeedLine(_currentString);
        }
        bg = GetComponentInParent<Image>();
        RealStart();
    }

    protected override void Update()
    {
        base.Update();
    }
    protected override void Progress(int index)
    {
       if (index < textLengthFor.Length)
        {
            if (textLengthFor[index].TagSuffix == "</font>")
            {
                Debug.Log("FOOOOONT YOOOOOO");
                _textComponent.ForceMeshUpdate();
            }
            
//            Debug.Log(_textComponent);
//            Debug.Log(textLengthFor.Length);
            _textComponent.text = CurrentString.Substring(0, textLengthFor[index].StartsAt) + textLengthFor[index].TagSuffix;
        }
        else
        {
            DisplayImmediate();
        }
    }

}
