using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] List<GameObject> enemyClasses;
    [SerializeField] int playerID;
    [SerializeField] Vector3 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 offset = new Vector3(-2 * transform.localScale.x, 0, -0.5f);
        spawnPosition = transform.position + offset;
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerID(int id) {
        playerID = id;
    }

    public void SpawnEnemy() {
        int toSpawn = Random.Range((int)EnemySpecie.SLAAIM, (int)EnemySpecie.HOMMUNCULUS);
        GameObject tempEnemy = Instantiate(enemyClasses[toSpawn], spawnPosition, Quaternion.Euler(0,0,0));
        Enemy en = tempEnemy.GetComponent<Enemy>();
        en.InitEnemy(playerID);
        en.SetSpawner(this);
        GetComponent<Player>().AssignEnemy(en);
    }
}
