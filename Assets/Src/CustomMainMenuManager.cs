using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using WiruLib;

public class CustomMainMenuManager : MainMenuManager
{

    [SerializeField] protected CanvasGroup mainMenuPanel;
    [SerializeField] protected CanvasGroup creditsPanel;
    [SerializeField] protected GameObject backCreditsButton;
 
    public void OpenCredits() {
        mainMenuPanel.alpha = 0;
        mainMenuPanel.interactable = false;
        creditsPanel.alpha = 1;
        creditsPanel.interactable = true;
        EventSystem.current.SetSelectedGameObject(backCreditsButton);
        eventSystemCurrentSelected = backCreditsButton;
    }

    public void CloseCredits() {
        creditsPanel.alpha = 0;
        creditsPanel.interactable = false;
        mainMenuPanel.alpha = 1;
        mainMenuPanel.interactable = true;
        EventSystem.current.SetSelectedGameObject(buttons[1]);
        eventSystemCurrentSelected = buttons[1];
    }
}
