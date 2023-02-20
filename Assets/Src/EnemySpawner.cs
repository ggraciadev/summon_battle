using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] List<GameObject> enemyClasses;
    [SerializeField] int playerID;
    [SerializeField] Vector3 spawnPosition;
    [SerializeField] bool isBoss;
    [SerializeField] bool bossSpawned;

    // Start is called before the first frame update
    void Start()
    {
        if(!isBoss) {
            Vector3 offset = new Vector3(-2 * transform.localScale.x, 0, -0.5f);
            spawnPosition = transform.position + offset;
            SpawnEnemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerID(int id) {
        playerID = id;
    }

    public void SpawnEnemy() {
        if(isBoss) {
            if(bossSpawned) {
                GameManager.Instance.SpawnEnemies();
                Destroy(gameObject);
            }
            else {
                GameObject tempEnemy = Instantiate(enemyClasses[(int)EnemySpecie.HOMMUNCULUS], spawnPosition, Quaternion.Euler(0,0,0));
                Enemy[] en = tempEnemy.GetComponentsInChildren<Enemy>();
                int index = 0;
                foreach (Enemy e in en) {
                    e.InitEnemy(index);
                    e.SetSpawner(this);
                    GameManager.Instance.GetPlayerInstance(index).AssignEnemy(e);
                    index++;
                }
                bossSpawned = true;
            }
            return;
        }
        else {
            int toSpawn = Random.Range((int)EnemySpecie.SLAAIM, (int)EnemySpecie.HOMMUNCULUS);
            GameObject tempEnemy = Instantiate(enemyClasses[toSpawn], spawnPosition, Quaternion.Euler(0,0,0));
            Enemy en = tempEnemy.GetComponent<Enemy>();
            en.InitEnemy(playerID);
            en.SetSpawner(this);
            GameManager.Instance.GetPlayerInstance(playerID).AssignEnemy(en);
            GetComponent<Player>().AssignEnemy(en);
        }
    }
}
