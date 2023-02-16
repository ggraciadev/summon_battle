using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiruLib;
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] Player playerInstance;
    [SerializeField] int controllerID;

    

    // Start is called before the first frame update
    void Start()
    {
        playerInstance = GetComponent<Player>();
        controllerID = playerInstance.GetControllerID();
        InputManager.Instance.AddPlayer(controllerID);
    }

    // Update is called once per frame
    void Update()
    {
        if(InputManager.Instance.GetButtonDown("Up", controllerID)) {
            playerInstance.SendInput(EnemyInputs.UP_INPUT);
        }
        if(InputManager.Instance.GetButtonDown("Down", controllerID)) {
            playerInstance.SendInput(EnemyInputs.DOWN_INPUT);
        }
        if(InputManager.Instance.GetButtonDown("Left", controllerID)) {
            playerInstance.SendInput(EnemyInputs.LEFT_INPUT);
        }
        if(InputManager.Instance.GetButtonDown("Right", controllerID)) {
            playerInstance.SendInput(EnemyInputs.RIGHT_INPUT);
        }
    }
}
