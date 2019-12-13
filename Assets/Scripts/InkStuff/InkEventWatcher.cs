using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InkEventWatcher
{
    static Dictionary<string, InkEvent> subscribedEvents;

	public static void Subscribe(InkEvent _event)
	{
		if(subscribedEvents == null)
		{
            subscribedEvents = new Dictionary<string, InkEvent>();
        }
        subscribedEvents[_event._id] = _event;
    } 

	public static void Unsubscribe(InkEvent _event)
	{
        subscribedEvents.Remove(_event._id);
    }
    
    public static void Trigger(string id)
    {
        if(subscribedEvents.ContainsKey(id))
        {
            subscribedEvents[id].Trigger();
        }
        else
            Debug.Log("Can't find key " + id);
    }
}
