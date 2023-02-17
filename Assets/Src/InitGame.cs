using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiruLib;
using UnityEngine.UI;

public class InitGame : MonoBehaviour
{
    [SerializeField] GameObject[] playerClasses;
    [SerializeField] GameObject[] playerSpawnPoint;
    [SerializeField] float timeLeft;
    [SerializeField] TMPro.TextMeshProUGUI timeText;
    [SerializeField] Image startMenuPanel;
    [SerializeField] bool canGetStartInput;
    [SerializeField] float fadeMenuTime;
    [SerializeField] float currentFadeMenuTime;


    // Start is called before the first frame update
    void Awake()
    {
        List<PlayerInfo> pInfo = new List<PlayerInfo>();
        pInfo.Add(new PlayerInfo(PlayerCharacter.Kara, PlayerType.Controller));
        pInfo.Add(new PlayerInfo(PlayerCharacter.Tama, PlayerType.Controller));
        GameManager.Instance.SetGamePlayers(pInfo);
        GameManager.Instance.SetGameTime(120);
        GameManager.Instance.SetGameStopped(true);

        timeText = GameObject.Find("TimeText").GetComponent<TMPro.TextMeshProUGUI>();
        timeLeft = GameManager.Instance.GetGameTime();
        timeText.SetText(GetTimeText(timeLeft));

        timeText.enabled = false;
        startMenuPanel = GameObject.Find("StartMenu").GetComponent<Image>();
        canGetStartInput = false;
    }

    void Start() {
        InputManager.Instance.AddPlayer(0);
        StartCoroutine(OpenStartMenu());
    }

    void InitGameScene() {
        //TESTING
        

        //REAL
        GameManager.Instance.SetPlayerClass(playerClasses);
        GameManager.Instance.InitGameScene(playerSpawnPoint);
        
        timeText.enabled = true;
        GameManager.Instance.SetGameStopped(false);
    }

    string GetTimeText(float time) {
        return "" + (int)time / 60 + ":" + (int)time % 60;
    }

    void Update() {
        if(!GameManager.Instance.GetGameStopped()) {
            if(timeLeft > 0) {
                timeLeft -= Time.deltaTime;
                timeText.SetText(GetTimeText(timeLeft));
                if(timeLeft < 0) {
                    timeLeft = 0;
                    GameManager.Instance.SetGameStopped(true);
                }
            }
        }
        else {
            if(canGetStartInput && InputManager.Instance.GetAnyButtonDown()) {
                StartCoroutine(CloseStartMenu());
            }
        }
    }


    IEnumerator OpenStartMenu() {
        startMenuPanel.enabled = true;
        while(currentFadeMenuTime < fadeMenuTime) {
            currentFadeMenuTime = Mathf.Min(1, currentFadeMenuTime + Time.deltaTime);
            startMenuPanel.color = new Color(1,1,1,currentFadeMenuTime / fadeMenuTime);
            yield return null;
        }
        canGetStartInput = true;
    }

    IEnumerator CloseStartMenu() {
        Debug.Log("Closing menu");
        while(currentFadeMenuTime > 0) {
            currentFadeMenuTime = Mathf.Max(0, currentFadeMenuTime - Time.deltaTime);
            startMenuPanel.color = new Color(1,1,1, currentFadeMenuTime / fadeMenuTime);
            yield return null;
        }
        startMenuPanel.enabled = false;
        InitGameScene();
    }
}
