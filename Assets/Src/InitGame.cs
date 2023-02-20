using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiruLib;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InitGame : MonoBehaviour
{
    [SerializeField] GameObject[] playerClasses;
    [SerializeField] GameObject[] playerSpawnPoint;
    [SerializeField] float timeLeft;
    [SerializeField] GameObject timePanel;
    [SerializeField] TMPro.TextMeshProUGUI timeText;
    [SerializeField] CanvasGroup startMenuPanel;
    [SerializeField] CanvasGroup endMenuPanel;
    [SerializeField] bool canGetStartInput;
    [SerializeField] float fadeMenuTime;
    [SerializeField] float currentFadeMenuTime;
    [SerializeField] GameObject finishAnimObject;
    [SerializeField] Sprite[] winnerPoseSprite;
    [SerializeField] GameObject[] winnerConfeti;
    [SerializeField] EnemySpawner bossSpawner;
    [SerializeField] Animator doorAnim;
    [SerializeField] float spawnBossTime;
    [SerializeField] bool bossSpawned;
    [SerializeField] AudioSource[] sounds;


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
        startMenuPanel = GameObject.Find("StartMenu").GetComponent<CanvasGroup>();
        endMenuPanel = GameObject.Find("WinMenu").GetComponent<CanvasGroup>();
        
        //endMenuPanel.enabled = false;
        canGetStartInput = false;
        timePanel.SetActive(false);
    }

    void Start() {
        InputManager.Instance.AddPlayer(0);
        endMenuPanel.alpha = 0;
        endMenuPanel.interactable = false;
        bossSpawned = false;
        EventSystem.current.SetSelectedGameObject(GameObject.Find("PlayButton"));
        StartCoroutine(OpenStartMenu());
    }

    public void InitGameScene() {
        GameManager.Instance.SetPlayerClass(playerClasses);
        GameManager.Instance.InitGameScene(playerSpawnPoint);
        timePanel.SetActive(true);
        timeText.enabled = true;
        GameManager.Instance.SetGameStopped(false);
        sounds[0].Play();
    }

    string GetTimeText(float time) {
        string temp = "0" + (int)time / 60 + ":";
        if(time % 60 < 10) {
            temp += "0";
        }
        temp += (int)time % 60;
        return temp;
    }

    void Update() {
        if(!GameManager.Instance.GetGameStopped()) {
            if(timeLeft > 0) {
                timeLeft -= Time.deltaTime;
                timeText.SetText(GetTimeText(timeLeft));

                if(!bossSpawned && timeLeft <= spawnBossTime) {
                    StartCoroutine(SpawnBoss());
                }

                if(timeLeft < 0) {
                    timeLeft = 0;
                    GameManager.Instance.SetGameStopped(true);
                    StartCoroutine(FinishAnimCor());
                }
            }
        }
    }

    public void CallCloseStartMenu() {
        StartCoroutine(CloseStartMenu());
    }

    public void RetryGame() {
        SceneManager.Instance.ChangeScene("Game");
    }

    public void OpenKickstarter() {
        Application.OpenURL("https://www.kickstarter.com/projects/grimoriog/sword-of-the-necromancer-revenant/");
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

    IEnumerator SpawnBoss() {
        doorAnim.SetTrigger("Open");
        bossSpawned = true;
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.DespawnEnemies();
        bossSpawner.SpawnEnemy();
    }

    IEnumerator CloseStartMenu() {
        canGetStartInput = false;
        startMenuPanel.interactable = false;
        while(currentFadeMenuTime > 0) {
            currentFadeMenuTime = Mathf.Max(0, currentFadeMenuTime - Time.deltaTime);
            startMenuPanel.alpha = currentFadeMenuTime / fadeMenuTime;
            yield return null;
        }
        startMenuPanel.alpha = 0;
        InitGameScene();
    }

    IEnumerator FinishAnimCor() {
        sounds[0].Stop();
        sounds[1].PlayDelayed(1.5f);
        GameManager.Instance.DespawnEnemies();
        GameManager.Instance.SetPlayersInCombat();
        yield return new WaitForSeconds(2);
        GameObject anim = Instantiate(finishAnimObject, Vector3.up * 2, Quaternion.Euler(0,0,0));
        yield return new WaitForSeconds(4);
        StartCoroutine(OpenFinishMenu());
        yield return null;
    }

    IEnumerator OpenFinishMenu() {
        //meter info del player ganador
        timePanel.SetActive(false);
        PlayerInfo info = GameManager.Instance.GetWinnerPlayerInfo();
        sounds[1].Stop();
        sounds[2 + (int)info.character].Play();
        GameObject.Find("PlayerScore").GetComponent<Text>().text = "Score: " + ((info.score * info.multi).ToString());
        GameObject.Find("PlayerSprite").GetComponent<Image>().sprite = winnerPoseSprite[(int)info.character];
        endMenuPanel.enabled = true;
        currentFadeMenuTime = 0;

        for(int i = 0; i < 8; ++i) {
            Vector3 spawnPos = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(0.0f, 5.0f), 0);
            GameObject obj = Instantiate(winnerConfeti[(int)info.character],spawnPos, Quaternion.Euler(0,0,0));
        }

        while(currentFadeMenuTime < fadeMenuTime) {
            currentFadeMenuTime = Mathf.Min(1, currentFadeMenuTime + Time.deltaTime);
            endMenuPanel.alpha = currentFadeMenuTime / fadeMenuTime;
            yield return null;
        }
        endMenuPanel.alpha = 1;
        endMenuPanel.interactable = true;
        EventSystem.current.SetSelectedGameObject(GameObject.Find("KickstarterButton"));
    }
}
