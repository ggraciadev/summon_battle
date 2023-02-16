using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiruLib;

public enum PlayerType {Controller, CPU};
public enum PlayerCharacter {Tama, Kara};

[System.Serializable]
public struct PlayerInfo {
    public PlayerCharacter character;
    public PlayerType type;
    public int playerID;
    public int controllerID;

    public PlayerInfo(PlayerCharacter c, PlayerType t, int pid = -1, int cid = -1) {
        character = c;
        type = t;
        playerID = pid;
        controllerID = cid;
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

    // Start is called before the first frame update
    void Start()
    {
        enemiesRevived = new List<EnemyInfo>();
    }

    public void SetPlayerInfo(PlayerInfo pInfo) {
        playerInfo = pInfo;
        if(playerInfo.type == PlayerType.Controller) {
            gameObject.AddComponent<PlayerInputController>();
        }
        else {
            gameObject.AddComponent<PlayerAIController>();
        }
        if(playerInfo.playerID == 0) {
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

    public void SendInput(EnemyInputs input) {
        Debug.Log("Player " + playerInfo.playerID + " input: " + input);
        if(currentEnemy == null) { return; }
        if(currentEnemy.AddPlayerInput(input, playerInfo.playerID)) {
            //anim de revivir
            enemiesRevived.Add(currentEnemy.GetEnemyInfo());
        }
    }
}
