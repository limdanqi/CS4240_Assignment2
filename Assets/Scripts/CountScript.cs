using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CountScript : MonoBehaviour
{
    public TextMeshProUGUI scoreCountText;
  
    void Update()
    {
        int hitCount = PotHitScript.getScore();
        scoreCountText.text = "Score: " + hitCount.ToString();

    }
}
