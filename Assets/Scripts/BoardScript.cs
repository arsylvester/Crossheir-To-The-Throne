using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardScript : MonoBehaviour
{
    [SerializeField] GameObject BoardTimer;
    [SerializeField] GameObject BoardRecords;
    [SerializeField] GameObject BoardStats;

    public void drawStats()
    {
        BoardTimer.GetComponent<TextMeshPro>().SetText(TimeMaster.timeToString(TimeMaster.currentTime));
    }
}
