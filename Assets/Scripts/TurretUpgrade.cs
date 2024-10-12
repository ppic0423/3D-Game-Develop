using UnityEngine;

[System.Serializable]
public class TurretUpgrade
{
    public int level;
    public int damage;          // ���ݷ� ������
    public float rangeIncrease;         // Ž�� ���� ������
    public float attackInterval;        // ���� �ӵ� ������
    public float slowAmount;
    public float slowDuration;
    public float damageIncrease;
    public float restTime;
    public float range;                 // ���� Ÿ��, ����
    public int cost;                    // ���׷��̵� ���
}