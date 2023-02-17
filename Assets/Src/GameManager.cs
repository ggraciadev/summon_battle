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

    public void InitGameScene(GameObject[] playersSpawnPoint) {
        if(playerInstances != null) {
            playerInstances.Clear();
        }
        else {
            playerInstances = new List<Player>();
        }
        for(int i = 0; i < playersNum; ++i) {
            GameObject temp = Instantiate(playerClass[(int)playersInfo[i].character], playersSpawnPoint[i].transform.localPosition, Quaternion.Euler(0,0,0));
            playerInstances.Add(temp.GetComponent<Player>());
            playerInstances[i].SetPlayerInfo(playersInfo[i]);
        }
    }

    public void SetPlayerClass(GameObject[] pc) {
        playerClass = pc;
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

    public void SetGamePlayers(List<PlayerInfo> pInfo) {
        playersInfo = pInfo;
        playersNum = playersInfo.Count;
        CalcIDs();
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
