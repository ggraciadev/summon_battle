using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyInputs {UP_INPUT, DOWN_INPUT, RIGHT_INPUT, LEFT_INPUT};

    [SerializeField] int enemyInputSize;
    [SerializeField] List<EnemyInputs> enemyInputs;

    [SerializeField] int playerID = -1;
    [SerializeField] List<int> currentInputIndex;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < enemyInputSize; ++i) {
            enemyInputs.Add((EnemyInputs)Random.Range(0, 4));
        }
    }

    void InitEnemy(int playerSize, int pID) {
        playerID = pID;
        for(int i = 0; i < playerSize; ++i) {
            currentInputIndex.Add(0);
        }
    }

    void ResetCorrectInput(int pID) {
        currentInputIndex[pID] = 0;
    }

    public bool AddPlayerInput(EnemyInputs input, int pID) {
        if(playerID != -1 && playerID != pID) {
            return false;
        }
        
        if(enemyInputs[currentInputIndex[pID]] == input) {
            currentInputIndex[pID]++;
        }
        else {
            ResetCorrectInput(pID);
        }
        return currentInputIndex[pID] == enemyInputSize;
    }
}
