using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum EnemyInputs {UP_INPUT, DOWN_INPUT, RIGHT_INPUT, LEFT_INPUT};
public enum EnemySpecie {SLAAIM, RESTOREBBIT, WHISPIKE, SCHOLOAK, DOLLAHAN, MERYON, HOMMUNCULUS};

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
    [SerializeField] List<SpriteRenderer> buttonRenderer;
    [SerializeField] GameObject buttonTemplate;
    [SerializeField] Sprite[] buttonSprites;
    [SerializeField] bool spawned;
    [SerializeField] VisualEffectAsset[] impacts;
    [SerializeField] VisualEffectAsset[] lightning;

    [SerializeField] VisualEffect impactFX;
    [SerializeField] VisualEffect lightningFX;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        buttonRenderer = new List<SpriteRenderer>();
        sprite = anim.gameObject.GetComponent<SpriteRenderer>();
        impactFX = transform.GetChild(3).GetComponent<VisualEffect>();
        impactFX.Stop();
        lightningFX = transform.GetChild(4).GetComponent<VisualEffect>();
        lightningFX.Stop();
        for(int i = 0; i < enemyInfo.enemyInputSize; ++i) {
            enemyInputs.Add((EnemyInputs)Random.Range(0, 4));
            buttonRenderer.Add(transform.GetChild(2).GetChild(i).GetComponent<SpriteRenderer>());
            buttonRenderer[i].sprite = buttonSprites[(int)enemyInputs[i] + 4*playerID];
            buttonRenderer[i].color = new Color(1,1,1,0.2f);
        }
        StartCoroutine(SpawnAnim());
        if(transform.position.x < 0) {
            sprite.transform.localScale = new Vector3(-1,1,1);
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

    public bool HasSummon(PlayerInfo pInfo) {
        return currentInputIndex[pInfo.playerID] == enemyInfo.enemyInputSize;
    }

    void ResetCorrectInput(int pID) {
        currentInputIndex[pID] = 0;
        for(int i = 0; i < enemyInfo.enemyInputSize; ++i) {
            buttonRenderer[i].color = new Color(1,1,1,0.2f);
        }
    }

    public bool AddPlayerInput(EnemyInputs input, PlayerInfo info) {
        if(!spawned || (playerID != -1 && playerID != info.playerID)) {
            return false;
        }
        
        if(enemyInputs[currentInputIndex[info.playerID]] == input) {
            buttonRenderer[currentInputIndex[info.playerID]].color = new Color(1,1,1,1);
            Vector3 position = buttonRenderer[currentInputIndex[info.playerID]].transform.position;
            position.z = transform.position.z;
            impactFX.transform.position = position;
            impactFX.visualEffectAsset = impacts[(int)info.character];
            impactFX.Play();
            currentInputIndex[info.playerID]++;
        }
        else {
            ResetCorrectInput(info.playerID);
            return false;
        }
        if(currentInputIndex[info.playerID] == enemyInfo.enemyInputSize) {
            lightningFX.visualEffectAsset = lightning[(int)info.character];
            lightningFX.Stop();
            StartCoroutine(RevivieAnim());
        }
        return true;
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
        yield return new WaitForSeconds(0.4f);
        lightningFX.Play();
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
