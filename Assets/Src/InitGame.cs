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
    [SerializeField] CanvasGroup startMenuPanel;
    [SerializeField] CanvasGroup endMenuPanel;
    [SerializeField] bool canGetStartInput;
    [SerializeField] float fadeMenuTime;
    [SerializeField] float currentFadeMenuTime;
    [SerializeField] GameObject finishAnimObject;
    [SerializeField] Sprite[] winnerPoseSprite;


    // Start is called before the first frame update
    void Awake()
    {
        List<PlayerInfo> pInfo = new List<PlayerInfo>();
        pInfo.Add(new PlayerInfo(PlayerCharacter.Kara, PlayerType.Controller));
        pInfo.Add(new PlayerInfo(PlayerCharacter.Tama, PlayerType.Controller));
        GameManager.Instance.SetGamePlayers(pInfo);
        GameManager.Instance.SetGameTime(10);
        GameManager.Instance.SetGameStopped(true);

        timeText = GameObject.Find("TimeText").GetComponent<TMPro.TextMeshProUGUI>();
        timeLeft = GameManager.Instance.GetGameTime();
        timeText.SetText(GetTimeText(timeLeft));

        timeText.enabled = false;
        startMenuPanel = GameObject.Find("StartMenu").GetComponent<CanvasGroup>();
        endMenuPanel = GameObject.Find("WinMenu").GetComponent<CanvasGroup>();
        
        //endMenuPanel.enabled = false;
        canGetStartInput = false;
    }

    void Start() {
        InputManager.Instance.AddPlayer(0);
        endMenuPanel.alpha = 0;
        endMenuPanel.interactable = false;
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
                    StartCoroutine(FinishAnimCor());
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
            startMenuPanel.alpha = currentFadeMenuTime / fadeMenuTime;
            yield return null;
        }
        canGetStartInput = true;
    }

    IEnumerator CloseStartMenu() {
        canGetStartInput = false;
        while(currentFadeMenuTime > 0) {
            currentFadeMenuTime = Mathf.Max(0, currentFadeMenuTime - Time.deltaTime);
            startMenuPanel.alpha = currentFadeMenuTime / fadeMenuTime;
            yield return null;
        }
        startMenuPanel.interactable = false;
        InitGameScene();
    }

    IEnumerator FinishAnimCor() {
        GameObject anim = Instantiate(finishAnimObject, Vector3.zero, Quaternion.Euler(0,0,0));
        yield return new WaitForSeconds(4);
        StartCoroutine(OpenFinishMenu());
        yield return null;
    }

    IEnumerator OpenFinishMenu() {
        //meter info del player ganador
        PlayerInfo info = GameManager.Instance.GetWinnerPlayerInfo();
        GameObject.Find("PlayerName").GetComponent<TMPro.TextMeshProUGUI>().SetText(info.character.ToString());
        GameObject.Find("PlayerSprite").GetComponent<Image>().sprite = winnerPoseSprite[(int)info.character];
        endMenuPanel.enabled = true;
        currentFadeMenuTime = 0;
        while(currentFadeMenuTime < fadeMenuTime) {
            currentFadeMenuTime = Mathf.Min(1, currentFadeMenuTime + Time.deltaTime);
            endMenuPanel.alpha = currentFadeMenuTime / fadeMenuTime;
            yield return null;
        }
    }
}
