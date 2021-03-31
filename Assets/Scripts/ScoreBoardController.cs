using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardController : MonoBehaviour
{
    [SerializeField] Text scoreText;

    public void setBoard()
    {
        scoreText.text = TimeMaster.timerText;
    }
}
