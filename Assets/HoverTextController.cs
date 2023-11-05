using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverTextController : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI hoverUi;
    private string textToDisplay;

    public void Awake(){
        hoverUi = GetComponent<TextMeshProUGUI>();
    }

    public void Update(){
        hoverUi.text = textToDisplay;
        textToDisplay = "";
    }

    public void UpdateText(string str){
        textToDisplay = str;
    }
}
