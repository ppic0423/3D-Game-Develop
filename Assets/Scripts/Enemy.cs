using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // ������
    EnemySpawner _spawner;
    
    // ���� ����
    [SerializeField] private int _hp;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _damage;
    [SerializeField] private int _dropGold;

    // �̵� ���
    WayPoint _wayPoint;
    float rotationSpeed = 5;
    int waypointIndex = 0;
    float distanceTravelled = 0;

    public void Init(EnemySpawner spawner, EnemyStats enemyStats, WayPoint waypoint)
    {
        _spawner = spawner; // �θ� ������
        _wayPoint = waypoint; // ���� ����Ʈ

        // ���� ����
        _hp = enemyStats.hp;
        _moveSpeed = enemyStats.moveSpeed;
        _damage = enemyStats.damage;
        _dropGold = enemyStats.dropGold;
    }

    private void FixedUpdate()
    {
        MoveAlongPath();
    }

    public void TakeDamage(int damage)
    {
        _hp -= damage;

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
}