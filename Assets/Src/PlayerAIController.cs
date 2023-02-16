using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiruLib;

public class PlayerAIController : MonoBehaviour
{
    [SerializeField] Player playerInstance;
    

    // Start is called before the first frame update
    void Start()
    {
        playerInstance = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //cosas de IA
    }
}
