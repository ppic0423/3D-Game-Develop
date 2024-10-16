using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // ������
    EnemySpawner _spawner;

    // �����
    public List<Debuff> Debuffs
    {
        get { return _debuffs; }
        set { _debuffs = value; }
    }
    List<Debuff> _debuffs;

    [Header("���� ����")]
    [SerializeField] private float _maxHp;
    public float MaxHp
    {
        get { return _maxHp; }
        set { _maxHp = value; }
    }
    [SerializeField] private float _hp;
    public float Hp
    {
        get { return _hp; }
        set { _hp = value; }
    }
    [SerializeField] private float _moveSpeed;
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }
    [SerializeField] private int _damage;
    [SerializeField] private int _dropGold;
    float _damageInrease = 1;
    public float DamageIncrease
    {
        get { return _damageInrease; }
        set { _damageInrease = value; }
    }

    [Header("�̵� ����")]
    [SerializeField] float rotationSpeed = 5;
    int waypointIndex = 0;
    float distanceTravelled = 0;
    WayPoint _wayPoint;

    public void Init(EnemySpawner spawner, EnemyStats enemyStats, float enemyStatsWeight, WayPoint waypoint)
    {
        _spawner = spawner; // �θ� ������
        _wayPoint = waypoint; // ���� ����Ʈ

        // ���� ����
        _maxHp = enemyStats.hp * enemyStatsWeight;
        _hp = _maxHp;
        _moveSpeed = enemyStats.moveSpeed * enemyStatsWeight;
        _damage = enemyStats.damage * (int)enemyStatsWeight;
        _dropGold = enemyStats.dropGold * (int)enemyStatsWeight;

        Debuffs = new List<Debuff>();
    }

    private void FixedUpdate()
    {
        MoveAlongPath();
    }

    public void ApplyDebuff(Debuff debuff)
    {
        // �̹� ���� ������ ������� �ִ��� Ȯ��
        Debuff existingDebuff = Debuffs.Find(d => d.GetType() == debuff.GetType());

        // ���� ���� ������� ���ٸ� ���� �߰�
        if (existingDebuff == null)
        {
            StartCoroutine(debuff.StartDebuff());
        }
        else
        {
            // �̹� ���� ������� ���� ���, ���� ���� �ð� ����
            existingDebuff.duration = Mathf.Max(existingDebuff.duration, debuff.duration);
        }
    }

    public void TakeDamage(float damage)
    {
        _hp -= damage * _damageInrease;

        if( _hp <= 0 ) 
        {
            Dead();
        }
    }
    void Dead()
    {
        // ��� ���
        ResourceManager.Instance.AddGold(_dropGold);
        // ���� Ǯ�� �ٽ� ����
        _spawner.OnReleaseEnemy(this.gameObject);
    }
    #region �̵� ����
    // ���� ���� �̵�
    void MoveAlongPath()
    {
        if(waypointIndex < _wayPoint.points.Length)
        {
            Vector3 targetWayPoint = _wayPoint.points[waypointIndex];
            Vector3 direction = targetWayPoint - transform.position;

            if(direction.magnitude < 0.1f)
            {
                waypointIndex++;
                ArriveNexus();   
            }
            else
            {
                transform.position += direction.normalized * _moveSpeed * Time.deltaTime;
                RotateTowards(direction);
                distanceTravelled += _moveSpeed * Time.deltaTime;
            }
        }
    }
    void ArriveNexus()
    {
        // �ؼ����� �������� ���
        if (waypointIndex >= _wayPoint.points.Length)
        {
            ResourceManager.Instance.TakeDamage(_damage); // ü�� ����
            _spawner.OnReleaseEnemy(this.gameObject); // ������Ʈ Ǯ�� �ٽ� �ֱ�
        }
    }

    // �̵� �������� ȸ��
    void RotateTowards(Vector3 direction)
    {
        if(direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // ������� �̵��� �Ÿ� ��ȯ
    public float GetDistanceTravelled()
    {
        return distanceTravelled;
    }
    #endregion
}