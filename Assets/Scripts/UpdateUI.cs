using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerUi;
    [SerializeField] private Entity player;
    void Start(){
        playerUi.text = player.ToString("Player Stats:\n");
    }

    void FixedUpdate(){
        playerUi.text = player.ToString("Player Stats:\n");
    }
}
