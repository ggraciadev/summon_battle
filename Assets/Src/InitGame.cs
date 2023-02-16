using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiruLib;

public class InitGame : MonoBehaviour
{
    [SerializeField] GameObject playerClass;
    // Start is called before the first frame update
    void Awake()
    {
        //TESTING
        List<PlayerInfo> pInfo = new List<PlayerInfo>();
        pInfo.Add(new PlayerInfo(PlayerCharacter.Kara, PlayerType.Controller));
        pInfo.Add(new PlayerInfo(PlayerCharacter.Tama, PlayerType.Controller));
        GameManager.Instance.SetGamePlayers(pInfo);

        //REAL
        GameManager.Instance.SetPlayerClass(playerClass);
        GameManager.Instance.InitGameScene();
    }
}
