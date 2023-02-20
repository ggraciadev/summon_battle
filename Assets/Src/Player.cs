using System.Collections.Generic;
using UnityEngine;

public enum PlayerType {Controller, CPU};
public enum PlayerCharacter {Tama, Kara};

[System.Serializable]
public struct PlayerInfo {
    public PlayerCharacter character;
    public PlayerType type;
    public int playerID;
    public int controllerID;
    public int score;
    public int multi;

    public PlayerInfo(PlayerCharacter c, PlayerType t, int pid = -1, int cid = -1) {
        character = c;
        type = t;
        playerID = pid;
        controllerID = cid;
        score = 0;
        multi = 1;
    }

    public void SetPlayerCharacter(PlayerCharacter pc) {
        character = pc;
    }

    public void SetPlayerType(PlayerType pt) {
        type = pt;
    }

    public void SetControllerID(int cid) {
        controllerID = cid;
    }

    public void SetPlayerID(int pid) {
        playerID = pid;
    }
};

public class Player : MonoBehaviour
{
    [SerializeField] protected Enemy currentEnemy;
    [SerializeField] protected PlayerInfo playerInfo;
    [SerializeField] List<EnemyInfo> enemiesRevived;
    [SerializeField] Animator anim;

    float stunTime;

    // Start is called before the first frame update
    void Start()
    {
        enemiesRevived = new List<EnemyInfo>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update() {
        if(stunTime > 0) {
            stunTime -= Time.deltaTime;
        }
    }

    public void SetPlayerInfo(PlayerInfo pInfo) {
        playerInfo = pInfo;
        if(playerInfo.type == PlayerType.Controller) {
            gameObject.AddComponent<PlayerInputController>();
        }
        else {
            gameObject.AddComponent<PlayerAIController>();
        }
        if(transform.position.x < 0) {
            transform.localScale = new Vector3(-1,1,1);
        }
        GetComponent<EnemySpawner>().SetPlayerID(playerInfo.playerID);
    }

    public void AssignEnemy(Enemy e) {
        currentEnemy = e;
    }

    // Update is called once per frame
    public int GetPlayerID()
    {
        return playerInfo.playerID;
    }

    public int GetControllerID() {
        return playerInfo.controllerID;
    }

    public void DespawnEnemy() {
        if(currentEnemy != null) {
            Destroy(currentEnemy.gameObject);
        }
    }

    public void SpawnEnemy() {
        GetComponent<EnemySpawner>().SpawnEnemy();
    }

    public void SetAttackPose() {
        anim.SetBool("AttackPose", true);
    }

    public void SetDamagePose() {
        anim.SetBool("AttackPose", false);
        anim.SetBool("DamagePose", true);
    }

    public void SetSpawnPose() {
        anim.SetBool("AttackPose", false);
        anim.SetBool("SpawnPose", true);
    }

    public void SendInput(EnemyInputs input) {
        if(currentEnemy == null || stunTime > 0 || GameManager.Instance.GetGameStopped()) { return; }
        if(currentEnemy.AddPlayerInput(input, playerInfo)) {
            //anim de revivir
            if(currentEnemy.HasSummon(playerInfo)) {
                anim.SetTrigger("Summon");
                enemiesRevived.Add(currentEnemy.GetEnemyInfo());
                if(currentEnemy.GetEnemyInfo().specie == EnemySpecie.HOMMUNCULUS) {
                    playerInfo.multi = 2;
                }
                playerInfo.score++;
                GameManager.Instance.UpdatePlayer(playerInfo);
            }
        }
        else {
            anim.SetTrigger("Fail");
            stunTime = 1;
        }
    }
}
