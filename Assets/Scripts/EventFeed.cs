using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFeed : MonoBehaviour
{
    public static EventFeed instance; //create singleton for this script
    [SerializeField] GameObject eventItemPrefab;
    Queue<GameObject> activeItems = new Queue<GameObject>();

    [Header("Variables")]
    [SerializeField] int maximumEvents;
    [SerializeField] float fadeOutStartingPause;
    [SerializeField] float fadeOutDuration;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void AddEvent(string name, string multi) //overflow method with multiplier input
    {
        DeleteOverflow(); //if there isn't enough room, delete the oldest event
        
        GameObject evt = Instantiate(eventItemPrefab, transform);
        evt.transform.SetAsLastSibling();
        EventItem evtItem = evt.GetComponent<EventItem>();
        evtItem.SetName(name);
        evtItem.SetMultiplier(multi);
        activeItems.Enqueue(evt); //add to the queue of active items
        StartCoroutine(evtItem.FadeOut(fadeOutStartingPause, fadeOutDuration));
    }

    public void AddEvent(string name)
    {
        DeleteOverflow();

        GameObject evt = Instantiate(eventItemPrefab, transform);
        evt.transform.SetAsLastSibling();
        EventItem evtItem = evt.GetComponent<EventItem>();
        evtItem.SetName(name);
        evtItem.EnableMultiplier(false);
        activeItems.Enqueue(evt);
        StartCoroutine(evtItem.FadeOut(fadeOutStartingPause, fadeOutDuration));
    }

    public void DeleteOverflow()
    {
        while (activeItems.Count > maximumEvents)
        {
            GameObject temp = activeItems.Peek();
            activeItems.Dequeue();

            if (temp != null)
                Destroy(temp);
        }
    }

    public void TestEvent()
    {
        AddEvent("SHOT STREAK", "10");
        AddEvent("SHOT STREAK", "15");
        AddEvent("SHOT STREAK", "20");
        AddEvent("SHOT STREAK", "25");
        AddEvent("SHOT STREAK", "30");
        AddEvent("SHOT STREAK", "35");
    }
}
