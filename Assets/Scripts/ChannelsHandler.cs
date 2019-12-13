using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChannelsHandler : MonoBehaviour 
{
    [SerializeField] Image staticEffect;
    Material staticMaterial;
    int nbChannels;
    int seenChannels;
	int currentChannel;
    Channel[] channels;
    Channel[] availableChannels;

    List<Channel> allChannels;
    StoryReader reader;

    bool init = false;
    private bool reading;

    public UnityEngine.Rendering.PostProcessing.PostProcessVolume ppVolume;
    public UnityEngine.Rendering.PostProcessing.PostProcessVolume ppGrainVideo;
    public Image bg;

    [HideInInspector]
    public AudioSource[] sources;

    float currentValue = 1f;
    float nextValue = 1f;

    public Slider slider;

    public AudioClip turnOn;
    public AudioClip turnOff;

    public AudioSource roomSource;

    public Channel endingChannel;
    bool turnedOn = false;

    // Use this for initialization
    void Start () 
	{
        staticMaterial = new Material(staticEffect.material);
        staticEffect.material = staticMaterial;
        sources = GetComponents<AudioSource>();
        reader = GetComponentInParent<StoryReader>();
        channels = GetComponentsInChildren<Channel>();
        availableChannels = new Channel[14];

        nbChannels = channels.Length;
        currentChannel = nbChannels-1;

        allChannels = new List<Channel>(channels);
        Debug.Log(allChannels.Count);
        for(int i = 0; i < 6; i++)
		{
            Channel c = allChannels[Random.Range(0, allChannels.Count)];
            PlaceChannel(c);
            allChannels.Remove(c);
            Debug.Log(c.gameObject.name);
        }
        Debug.Log(nbChannels);
        foreach(Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
        staticEffect.gameObject.SetActive(false);
    }

    void Update()
    {
        if(currentValue != nextValue)
        {
            sources[1].volume = 1f;
        }
        else
        {
            sources[1].volume = 0f;
        }
        currentValue = nextValue;
        
    }

    void PlaceChannel(Channel c)
    {
        List<int> availableSpots = new List<int>();
        for (int i = 0; i < availableChannels.Length;i++)
        {

            if(i != currentChannel &&  availableChannels[i] == null)
                availableSpots.Add(i);
        }

        availableChannels[availableSpots[Random.Range(0, availableSpots.Count)]] = c;
    }

    float fadeTimer = 0;
    bool fading = false;

    // every 2 seconds perform the print()
    private IEnumerator Fade(bool on, float duration)
    {
        while(fadeTimer < duration)
        {
            fading = true;
            fadeTimer += Time.deltaTime;
            float lerp = Utilities.Ease.quadOut(fadeTimer / duration);
            float lerp0to1 = Mathf.Lerp(0f, 1f, lerp);
            float lerp1to0 = Mathf.Lerp(1f, 0f, lerp);
            ppVolume.weight = on ? lerp0to1 : lerp1to0;
            ppGrainVideo.weight = on ? lerp1to0 : lerp0to1;
            Color c = bg.color;
            c.a = on ?  lerp1to0 : lerp0to1;
            roomSource.volume = on? lerp1to0 : lerp0to1;
            bg.color = c;
            sources[0].volume = on ? lerp0to1 : lerp1to0;
            staticMaterial.SetFloat("_Alpha", on ? lerp0to1 : lerp1to0);
            yield return new WaitForEndOfFrame();
        }
        fadeTimer = 0f;
        slider.gameObject.SetActive(on);
        if(on)
        {
            ChangeChannel(currentValue);
            SwitchChannel(currentChannel);
        }
        foreach(Transform t in transform)
        {
            t.gameObject.SetActive(on);
        }
        staticEffect.gameObject.SetActive(on);
        turnedOn = on;
        fading = false;

        Debug.Log("WaitAndPrint " + fadeTimer);   
    }
	
    public void PowerSwitch()
    {
        if(!fading && reader.canPlay)
        {
            bool on = !turnedOn;
            foreach(Transform t in transform)
            {
                t.gameObject.SetActive(false);
            }

            staticEffect.gameObject.SetActive(true);
            IEnumerator coroutine = Fade(on, on?5.5f:1f);
            StartCoroutine(coroutine);
            reader.GetComponent<AudioSource>().PlayOneShot(on ? turnOn : turnOff);
            if(!on)
            {
                SaveAndClose();
                reading = false;
            }
        }


        // ppVolume.weight = on ? 1f : 0f;
        // Color c = bg.color;
        // c.a = on ? 0f : 1f;
        // bg.color = c;
        // slider.gameObject.SetActive(on);
        
        // sources[0].volume = on ? 1f : 0f;
    }
    
    public void ChangeChannel(float value)
    {
        nextValue = value;
        int channel = Mathf.FloorToInt((((value)*(availableChannels.Length))-1) / 100);
        float delta = Mathf.Abs(((((value) * (availableChannels.Length)) - 1) % 100) - 50.0f);
     //   Debug.Log(channel +" - '"+ delta);
        if(availableChannels[channel] == null)
        {
            staticMaterial.SetFloat("_Alpha", 1f);
            sources[0].volume = 1f;
               
            if(channel != currentChannel)
                SaveAndClose();
        }
        else
        {
            float lerp = Mathf.Lerp(0f, 1f, Mathf.InverseLerp(10, 40, delta));
        //    Debug.Log(availableChannels[channel].knot);
            staticMaterial.SetFloat("_Alpha",lerp);
            reader.SetGlobalAlpha(1f-lerp);
           
              sources[0].volume = lerp;
               
            Channel c = availableChannels[currentChannel];
            if(c != null && c.source != null)
            {
                c.source.volume = (1f - lerp);
            }
            if(channel != currentChannel)
                    SwitchChannel(channel);
                else
                {
                    if(1f-lerp > 0.95)
                    {
                        if(!reading)
                        {
                        Channel ch = availableChannels[currentChannel];
                        reading = true;
                            if (ch.save != string.Empty)
                            {
                            reader.FeedChannel(ch);
                            reader.LoadStateAndPlay(ch.save);
                            }
                            else
                            {
                            reader.FeedChannel(ch);
                            reader.ReadKnot(ch.knot);
                            }
                        }
                    }
                    else if(reading)
                    {
                        reading = false;
                        SaveAndClose();
                    }
                }
        }
    }

    void SaveAndClose()
    {
        string save = reader.SaveStateAndClose();
        Channel c = availableChannels[currentChannel];
        if(c!=null)
        {
            if(save != string.Empty)
                availableChannels[currentChannel].save = save;

            if(c != null && c.source != null)
                c.source.volume = 0f;

        }
    }

    void RemoveChannel()
    {
        availableChannels[currentChannel].gameObject.SetActive(false);
        if(availableChannels[currentChannel].source != null)
            availableChannels[currentChannel].source.volume = 0f;
        availableChannels[currentChannel] = null;
        ChangeChannel(currentValue);

        seenChannels++;
        if(allChannels.Count > 0)
        {
            Channel c = allChannels[Random.Range(0, allChannels.Count)];
            PlaceChannel(c);
            allChannels.Remove(c);
        }
        
        if(seenChannels == nbChannels)
        {
            endingChannel.transform.SetParent(transform);
            endingChannel.gameObject.SetActive(true);
            PlaceChannel(endingChannel);
        }
    }
    public void SwitchChannel(int newChannel)
    {
        if(newChannel != currentChannel)
        {
            reading = false;
                SaveAndClose();

            Channel c = availableChannels[currentChannel];
            
            if (c != null)
            {
                if (c.source != null)
                    c.source.volume = 0f;
                c.IsOver -= RemoveChannel;

                
                availableChannels[currentChannel].GetComponent<Image>().enabled = false;
                
            }
            currentChannel = newChannel;
            
            availableChannels[currentChannel].transform.SetAsLastSibling();
            availableChannels[currentChannel].GetComponent<Image>().enabled = true;
            availableChannels[currentChannel].IsOver += RemoveChannel;
        }
    }
}
