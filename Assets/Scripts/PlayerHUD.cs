using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUD : MonoBehaviour{
    [SerializeField] private UIDocument _document;
    [SerializeField] private StyleSheet _styleSheet;
    [SerializeField] private PlayerController player;
    public string hoverText = "";

    public void Start(){
        player.statusChanged.AddListener(updateHud);
        StartCoroutine(Generate());
    }

    public void OnValidate(){
        if(Application.isPlaying){
            return;
        }
        StartCoroutine(Generate());
    }

    private void updateHud(){
        StartCoroutine(Generate());
    }
    private IEnumerator Generate(){
        yield return null;
        var root = _document.rootVisualElement;
        root.Clear();
        root.styleSheets.Add(_styleSheet);
        generatePlayerStatus(root);
        generateHoverText(root);
    }

    private void generateHoverText(VisualElement root){
        var hoverTextEle = Create<Label>("hover-text");
        hoverTextEle.text = hoverText;
        root.Add(hoverTextEle);
    }
    private void generatePlayerStatus(VisualElement root){
        var maincontainer = Create("container", "player-health");
        root.Add(maincontainer);

        var hbback = Create("bar-background");
        var hbfore = Create("health-bar-foreground");

        hbfore.style.width = Length.Percent((float)player.getHealth() / player.getMaxHealth() * 100);
        
        maincontainer.Add(hbback);
        hbback.Add(hbfore);

        var container = Create("container", "player-mana-and-stamina");
        maincontainer.Add(container);

        var sbback = Create("bar-background");
        var sbfore = Create("stamina-bar-foreground");

        sbfore.style.height = Length.Percent((float)player.getStamina() / player.getMaxStamina() * 100);
        
        container.Add(sbback);
        sbback.Add(sbfore);

        var mbback = Create("bar-background");
        var mbfore = Create("mana-bar-foreground");

        mbfore.style.height = Length.Percent((float)player.getMana() / player.getMaxMana() * 100);
        
        container.Add(mbback);
        mbback.Add(mbfore);
    }

    private VisualElement Create(params string[] className){
        return Create<VisualElement>(className);
    }

    private T Create<T>(params string[] classNames) where T : VisualElement, new(){
        var ele = new T();
        foreach (string className in classNames)
        {
            ele.AddToClassList(className);
        }
        return ele;
    }
}
