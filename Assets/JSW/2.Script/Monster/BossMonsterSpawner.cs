using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossMonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    private List<GameObject> spawnedMonsters = new List<GameObject>();
    private Coroutine spawnCoroutine = null;
    [SerializeField]private Transform[] spawnPoints = null;
    [SerializeField]private BossMonster bossMonster = null;

    void Start()
    {
        bossMonster = GetComponent<BossMonster>();
        
    }
    public void StartSpawning()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnRoutine());
        }
    }

    private IEnumerator SpawnRoutine()
    {
            Debug.Log(bossMonster.IsDead());
        while (!bossMonster.IsDead())
        {
            if (spawnedMonsters.Count < 10)
            {
                SpawnMonsters();
            }
            yield return new WaitForSeconds(10f);
        }
    }

    private void SpawnMonsters()
    {
        for (int i = 0; i < 5; i++)
        {
            if (spawnedMonsters.Count >= 10) break;

            int randomIndex = Random.Range(1, spawnPoints.Length); // 0은 부모 Transform이므로 제외
            Vector3 spawnPosition = spawnPoints[randomIndex].position;
            
            GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            spawnedMonsters.Add(monster);
        }
    }

    private void OnDestroy()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }
}
