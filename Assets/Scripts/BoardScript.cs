using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class BoardScript : MonoBehaviour
{
    [SerializeField] GameObject BoardTimer;
    [SerializeField] GameObject BoardRoomTimes;
    [SerializeField] GameObject BoardRecords;
    [SerializeField] GameObject BoardStats;

    [SerializeField] float devTime = 34f;
    [SerializeField] float goldTime = 45f;
    [SerializeField] float silverTime = 56f;
    [SerializeField] float bronzeTime = 70f;

    [SerializeField] GameObject S_Medal, A_Medal, B_Medal, C_Medal;

    void Start()
    {
        SetAllMedalsInactive();
    }

    public void drawStats()
    {
        BoardTimer.GetComponent<TextMeshPro>().SetText(TimeMaster.timerText);

        // Individual Rooms
        BoardRoomTimes.GetComponent<TextMeshPro>().SetText(
            "__Individual Rooms__\n" +
            "Room 1\t" + TimeMaster.getCheckpoint(1) + "\n" +
            "Room 2\t" + TimeMaster.getCheckpoint(2) + "\n" +
            "Room 3\t" + TimeMaster.getCheckpoint(3) + "\n" +
            "Room 4\t" + TimeMaster.getCheckpoint(4) + "\n" +
            "Room 5\t" + TimeMaster.getCheckpoint(5));

        // This is really dumb, but don't worry about it
        BoardRecords.GetComponent<TextMeshPro>().SetText(
            "__Course Records__\n" +
            "S Rank:\t" + TimeMaster.timeToString(devTime) + "\n" +
            "A Rank:\t" + TimeMaster.timeToString(goldTime) + "\n" +
            "B Rank:\t" + TimeMaster.timeToString(silverTime) + "\n" +
            "C Rank:\t" + TimeMaster.timeToString(bronzeTime) + "\n" +
            "\n" + getLeaderBoard());

        BoardStats.GetComponent<TextMeshPro>().SetText(
            "__Stats__\n" + 
            "Total Shots: " + WeaponManager.shotsTaken.ToString() + "\n" + 
            "Accuracy: " + (int)(((float)WeaponManager.shotsHit / (float)WeaponManager.shotsTaken) * 100) + "%\n" + 
            "Max KillStreak: " + WeaponManager.maxKillstreak.ToString() + "\n" +
            "Max ShotStreak: " + WeaponManager.maxShotstreak.ToString() + "\n" +
            "Full Reloads: " + WeaponManager.timesReloaded.ToString() + "\n");
        //((int)(time * 100)).ToString("00")

        float completionTime = TimeMaster.currentTime;

        

        if (completionTime <= devTime)
        {
            SetAllMedalsInactive();
            S_Medal.SetActive(true);
            return;
        }
        else if (completionTime <= goldTime)
        {
            SetAllMedalsInactive();
            A_Medal.SetActive(true);
            return;
        }
        else if (completionTime <= silverTime)
        {
            SetAllMedalsInactive();
            B_Medal.SetActive(true);
            return;
        }
        else if (completionTime <= bronzeTime)
        {
            SetAllMedalsInactive();
            C_Medal.SetActive(true);
            return;
        }

    }

    void SetAllMedalsInactive()
    {
        S_Medal.SetActive(false);
        A_Medal.SetActive(false);
        B_Medal.SetActive(false);
        C_Medal.SetActive(false);
    }

    string getLeaderBoard() // This is where I'll put the online leaderboard code
    {
        Dictionary<string, float> leaderboard = new Dictionary<string, float>();
        string playerUsername = "You\t";
        //playerUsername = "<color=\"yellow\">" + playerUsername + "</color>";

        leaderboard.Add("quaintt", 29.31f);
        leaderboard.Add("Plomp", 31.59f);
        leaderboard.Add("Sparkfire", 34.39f);
        leaderboard.Add("Bowling", 36.66f); //there's a bug in the rounding script that lowers the decimal by one, so it's .67 to compensate
        leaderboard.Add("GiantRat", 39.85f);
        leaderboard.Add(playerUsername, PlayerPrefs.GetFloat("HighScore", 9999f));

        string str = "__Leader Board__\n";

        foreach (KeyValuePair<string, float> item in leaderboard.OrderBy(key=> key.Value))
        {
            if (item.Key == playerUsername)
            {
                str += "<color=\"yellow\">" + item.Key + "\t" + TimeMaster.timeToString(item.Value) + "</color>\n";
            }
            else
                str += item.Key + "\t" + TimeMaster.timeToString(item.Value) + "\n";
        }

/*        string str = "__Leader Board__\n" +
            "quaintt\t" + 29.31f + "\n" +
            "Plomp\t" + 31.59f + "\n" +
            "Sparkfire\t" + 34.39f;*/

        return str;
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