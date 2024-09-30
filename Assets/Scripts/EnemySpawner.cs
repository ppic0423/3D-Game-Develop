using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] BattlePhase battlePhase;

    [Header("���� ����")]
    [SerializeField] float spawnInterval = 1f; // ��ȯ �ð� ��
    [SerializeField] int maxSpawnCount = 10; // �ִ� ��ȯ ����
    private int spawnCountDelta = 0;

    ObjectPool<GameObject> pool; // ������Ʈ Ǯ
    int deadEnemyCount = 0;
    [SerializeField] int stage;
    
    private void Start()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: CreateEnemy,
            actionOnGet: OnGetEnemy,
            actionOnRelease: OnReleaseEnemy,
            actionOnDestroy: DestroyEnemy,
            defaultCapacity: 10
            );
    }
    
    public void Init()
    {
        // ���� �ʱ�ȭ
        spawnCountDelta = maxSpawnCount;
        deadEnemyCount = 0;

        // TODO. Change Enemy Stats
        currentEnemyStat = enemyStats[stage % enemyStats.Count];

        // �� ��ȯ ����
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        if (spawnCountDelta > 0)
        {
            pool.Get();
        }
    }

    #region ObjectPool
    [Header("���� ����")]
    [SerializeField] GameObject enemyPrefab; // ���� ������
    [SerializeField] WayPoint wayPoint; // �̵� ���
    [SerializeField] List<EnemyStats> enemyStats; // ���� ����
    EnemyStats currentEnemyStat;
    private GameObject CreateEnemy()
    {
        GameObject monster = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        monster.gameObject.transform.parent = this.transform;
        monster.SetActive(false);
        return monster;
    }
    private void OnGetEnemy(GameObject enemy)
    {
        // TODO. ���� �ɷ�ġ �� �𵨸� �����ϱ�
        enemy.GetComponent<Enemy>().Init(this, currentEnemyStat ,wayPoint);

        spawnCountDelta--; // ���� ī��Ʈ ����
        enemy.SetActive(true); // �� Ȱ��ȭ
    }
    public void OnReleaseEnemy(GameObject enemy)
    {
        enemy.SetActive(false); // �� ��Ȱ��ȭ
        deadEnemyCount++;

        // ��� ���� ����� ���
        if(deadEnemyCount == maxSpawnCount)
        {
            CancelInvoke(nameof(SpawnEnemy));
            battlePhase.ChangePhase();
        }
    }
    void DestroyEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }
    #endregion
}
