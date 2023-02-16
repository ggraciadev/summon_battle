using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyInputs {UP_INPUT, DOWN_INPUT, RIGHT_INPUT, LEFT_INPUT};
public enum EnemySpecie {SLAAIM, WHISPIKE, DOLLAHAN, HOMMUNCULUS};

[System.Serializable]
public struct EnemyInfo {
    public EnemySpecie specie;
    public int enemyInputSize;

};

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyInfo enemyInfo;

    [SerializeField] List<EnemyInputs> enemyInputs;

    [SerializeField] int playerID = -1;
    [SerializeField] List<int> currentInputIndex;

    [SerializeField] EnemySpawner spawner;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] bool spawned;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        sprite = anim.gameObject.GetComponent<SpriteRenderer>();
        for(int i = 0; i < enemyInfo.enemyInputSize; ++i) {
            enemyInputs.Add((EnemyInputs)Random.Range(0, 4));
        }
        StartCoroutine(SpawnAnim());
        if(transform.position.x < 0) {
            transform.localScale = new Vector3(-1,1,1);
        }
    }

    public void InitEnemy(int pID) {
        playerID = pID;
        int size = GameManager.Instance.GetPlayersNum();
        for(int i = 0; i < size; ++i) {
            currentInputIndex.Add(0);
        }
    }
    public EnemyInfo GetEnemyInfo() {
        return enemyInfo;
    }

    public void SetSpawner(EnemySpawner s) {
        spawner = s;
    }

    void ResetCorrectInput(int pID) {
        currentInputIndex[pID] = 0;
    }

    public bool AddPlayerInput(EnemyInputs input, int pID) {
        if(!spawned || (playerID != -1 && playerID != pID)) {
            return false;
        }
        
        if(enemyInputs[currentInputIndex[pID]] == input) {
            currentInputIndex[pID]++;
        }
        else {
            ResetCorrectInput(pID);
        }
        if(currentInputIndex[pID] == enemyInfo.enemyInputSize) {
            StartCoroutine(RevivieAnim());
        }
        return currentInputIndex[pID] == enemyInfo.enemyInputSize;
    }

    public void SequenceComplete() {
        spawner.SpawnEnemy();
        Destroy(gameObject);
    }
    
    void OnSpawned() {
        spawned = true;
        //meter la UI
    }

    IEnumerator RevivieAnim() {
        //mandar trigger al animator
        anim.SetTrigger("Revive");
        yield return new WaitForSeconds(0.2f);
        float animTime = 0.7f;
        float maxAnimTime = 0.7f;
        while(animTime > 0) {
            animTime -= Time.deltaTime;
            sprite.color = new Color(1,1,1,Mathf.Max(animTime / maxAnimTime, 0));
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        SequenceComplete();
    }

    IEnumerator SpawnAnim() {
        float animTime = 0;
        float maxAnimTime = 0.7f;
        while(animTime < maxAnimTime) {
            animTime += Time.deltaTime;
            sprite.color = new Color(1,1,1,Mathf.Min(animTime / maxAnimTime, 1));
            yield return null;
        }
        OnSpawned();
    }
}
