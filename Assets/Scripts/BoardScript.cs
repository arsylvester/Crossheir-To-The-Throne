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
        BoardTimer.GetComponent<TextMeshPro>().SetText(TimeMaster.timerText);
        // This is really dumb, but don't worry about it
        BoardRecords.GetComponent<TextMeshPro>().SetText(
            "__Course Records__\n" + 
            "Dev Time: 09:18.32\n" + 
            "Gold: 02:34.44\n" + 
            "Silver: 09:22.88\n" + 
            "Bronze: 10:21.32\n" + 
            "\nYour Best Time:\n" +
            TimeMaster.timeToString(PlayerPrefs.GetFloat("HighScore", 9999f)));

        BoardStats.GetComponent<TextMeshPro>().SetText(
            "__Stats__\n" + 
            "Total Shots: " + WeaponManager.shotsTaken.ToString() + "\n" + 
            "Accuracy: " + (int)(((float)WeaponManager.shotsHit / (float)WeaponManager.shotsTaken) * 100) + "%\n" + 
            "Max KillStreak: " + WeaponManager.maxKillstreak.ToString() + "\n" +
            "Max ShotStreak: " + WeaponManager.maxShotstreak.ToString());
        //((int)(time * 100)).ToString("00")
    }
}

/* BOARD RECORDS
__Course Records__
Dev Time:	09:18.32
Gold:		02:34.44
Silver:		09:22.88
Bronze:	10:21.32

Your Best Time:
00:00.00
 */

/* BOARD STATS
__Stats__
Total Shots: XX
Accuracy: XX%   
Max KillStreak: XX
Max ShotStreak: XX
 */