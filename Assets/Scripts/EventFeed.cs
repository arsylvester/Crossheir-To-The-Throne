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
    [SerializeField] float fadeInDuration;
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
        evtItem.hasMultiplier = true;
        evtItem.SetMultiplier(multi);
        activeItems.Enqueue(evt); //add to the queue of active items
        StartCoroutine(evtItem.FadeIn(fadeInDuration, fadeOutStartingPause, fadeOutDuration));
    }

    public void AddEvent(string name)
    {
        DeleteOverflow();

        GameObject evt = Instantiate(eventItemPrefab, transform);
        evt.transform.SetAsLastSibling();
        EventItem evtItem = evt.GetComponent<EventItem>();
        evtItem.SetName(name);
        evtItem.hasMultiplier = false;
        activeItems.Enqueue(evt);
        StartCoroutine(evtItem.FadeIn(fadeInDuration, fadeOutStartingPause, fadeOutDuration));
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

    public void ClearAll()
    {
        while (activeItems.Count > 0)
        {
            GameObject temp = activeItems.Peek();
            activeItems.Dequeue();

            if (temp != null)
                Destroy(temp);
        }
    }

    public void FadeOutAll() /*Used for restarting the fade out for all active items*/
    {
        //this method is needed because of a bug where items freeze animations when the game is paused.

        print("fade out all");

        DeleteOverflow();

        GameObject[] items = activeItems.ToArray();

        foreach(GameObject item in items)
        {
            if (item != null)
                StartCoroutine(item.GetComponent<EventItem>().FadeOut(fadeOutStartingPause, fadeOutDuration));
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
