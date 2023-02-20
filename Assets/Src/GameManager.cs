using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiruLib;

public class GameManager : BaseGameManager<GameManager>
{ 
    [SerializeField] GameObject[] playerClass;
    [SerializeField] int playersNum;
    [SerializeField] List<Player> playerInstances;
    [SerializeField] List<PlayerInfo> playersInfo;
    [SerializeField] float gameTime;
    [SerializeField] bool gameStopped;

    void Awake() {
        playerInstances = new List<Player>();
    }

    public void InitGameScene(GameObject[] playersSpawnPoint) {
        if(playerInstances == null) {
            playerInstances = new List<Player>();
        }
        for(int i = playerInstances.Count - 1; i >= 0; --i) {
            if(playerInstances[i] != null) {
                Destroy(playerInstances[i].gameObject);
            }
        }
        playerInstances.Clear();
        
        for(int i = 0; i < playersNum; ++i) {
            GameObject temp = Instantiate(playerClass[(int)playersInfo[i].character], playersSpawnPoint[i].transform.localPosition, Quaternion.Euler(0,0,0));
            playerInstances.Add(temp.GetComponent<Player>());
            playerInstances[i].SetPlayerInfo(playersInfo[i]);
        }
    }

    public void SetPlayerClass(GameObject[] pc) {
        playerClass = pc;
    }

    public void SetGameStopped(bool s) {
        gameStopped = s;
        for(int i = 0; i < playerInstances.Count; ++i) {
            if(playerInstances[i] != null) {
                playerInstances[i].enabled = !s;
            }
        }
    }

    public void SetGameTime(float time) {
        gameTime = time;
    }

    public float GetGameTime() {
        return gameTime;
    }

    public bool GetGameStopped() {
        return gameStopped;
    }

    public void AssignEnemyToPlayer(Enemy e, int player) {
        playerInstances[player].AssignEnemy(e);
    }

    public int GetPlayersNum() { 
        return playersNum;
    }

    public Player GetPlayerInstance(int id) {
        if(id < 0 || id >= playersNum) {
            Debug.LogError("Mirate eso bien que me pides una ID de player que no toca: [ID=" + id + " | size: " + playersNum + "]");
            return null;
        }
        return playerInstances[id];
    }

    public void UpdatePlayer(PlayerInfo info) {
        for(int i = 0; i < playersNum; ++i) {
            if(info.playerID == playersInfo[i].playerID) {
                playersInfo[i] = info;
                return;
            }
        }
    }

    public void SpawnEnemies() {
        foreach(Player p in playerInstances) {
            p.SpawnEnemy();
        }
    }

    public void DespawnEnemies() {
        foreach(Player p in playerInstances) {
            p.DespawnEnemy();
        }
    }

    public void SetGamePlayers(List<PlayerInfo> pInfo) {
        playersInfo = pInfo;
        playersNum = playersInfo.Count;
        CalcIDs();
    }

    public void SetPlayersInCombat() {
        foreach(Player p in playerInstances) {
            p.SetAttackPose();
        }
    }

    public void SetWinnerPlayerPose() {

        playerInstances[GetWinnerPlayerInfo().playerID].SetSpawnPose();
    }

    public void SetLosersPlayerPose() {
        int winnerID = GetWinnerPlayerInfo().playerID;
        foreach(Player p in playerInstances) {
            if(p.GetPlayerID() != winnerID) {
                p.SetDamagePose();
            }
        }
    }

    public void SetFinishCombatPoses() {
        SetWinnerPlayerPose();
        SetLosersPlayerPose();
    }


    public PlayerInfo GetWinnerPlayerInfo() {
        PlayerInfo info = playersInfo[0];
        for(int i = 1; i < playersNum; ++i) {
            if(playersInfo[i].score * playersInfo[i].multi > info.score * info.multi) {
                info = playersInfo[i];
            }
        }
        return info;
    }

    public void CalcIDs() {
        int contr = 0;
        for(int i = 0; i < playersNum; ++i) {
            PlayerInfo temp = playersInfo[i];
            temp.playerID = i;
            if(temp.type == PlayerType.Controller) {
                temp.SetControllerID(contr++);
            }
            playersInfo[i] = temp;
        }
    }
}
