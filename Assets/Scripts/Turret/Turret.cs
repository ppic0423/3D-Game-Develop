using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class Turret : MonoBehaviour
{
    #region �ͷ� �ɷ�ġ
    [Header("�ͷ� �ɷ�ġ")]
    [SerializeField] protected int _damage; // ���ݷ�
    [SerializeField] protected float _findRange = 5f;  // Ž�� ����
    [SerializeField] protected float _attackInterval;
    protected float _attackIntervalDelta = 0;
    protected float _bulletSpeed = 30f;
    public int Cost
    {
        get { return _cost; }
        set { _cost = value; }
    }
    [SerializeField] protected int _cost;
    #endregion
    #region ���׷��̵�
    [Header("���׷��̵�")]
    [HideInInspector] public int currentLevel = 0; // ���� ����
    public List<TurretUpgrade> upgrades = new List<TurretUpgrade>(); // ���׷��̵� ����Ʈ
    #endregion
    [Header("�±�")]
    [SerializeField] public List<Define.Synergy> synergys = new List<Define.Synergy>();
    [HideInInspector] public List<Debuff> synergyDebuffs = new List<Debuff>();
    [HideInInspector] public float _synergyAttackInterval = 0;
    [HideInInspector] public float _synergyDamagePlus = 1;
    [SerializeField] public AudioClip _fireSound;
    protected List<Debuff> debuffs = new List<Debuff>();
    protected Transform _target;

    protected virtual void Init()
    {
    }
    protected virtual void Tick()
    {
        _attackIntervalDelta += Time.deltaTime;

        FindObjectsInRange();
        RotateTowardsTarget();
        Attack();
    }

    protected abstract void Attack();
    protected void AddDebuffOnBullet(GameObject bulletGo)
    {
        // �Ѿ˿� ����� �߰�
        Bullet bullet = bulletGo.GetComponent<Bullet>();

        foreach (Debuff debuff in debuffs)
        {
            bullet.GetComponent<Bullet>().AddDebuffs(debuff);
        }
        foreach (Debuff debuff in synergyDebuffs)
        {
            bullet.GetComponent<Bullet>().AddDebuffs(debuff);
        }
    }

    public virtual void Upgrade()
    {
        if (currentLevel >= upgrades.Count || ResourceManager.Instance.Gold < upgrades[currentLevel].cost)
            return;

        ResourceManager.Instance.UseGold(upgrades[currentLevel].cost);
        currentLevel++;
        Debug.Log(currentLevel);
    }
    protected virtual void FindObjectsInRange()
    {
        // ���� ���� �ִ� ��� �ݶ��̴��� ������
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _findRange, enemyLayer);
        float maxDistanceTravelled = float.MinValue;

        // ���� ���� ���� ���� ���
        if (hitColliders.Length == 0)
        {
            maxDistanceTravelled = 0;
            _target = null;
            return;
        }

        // �� �ݶ��̴��� ������Ʈ �̸��� ���
        foreach (Collider collider in hitColliders)
        {
            if (collider.GetComponent<Enemy>().GetDistanceTravelled() > maxDistanceTravelled)
            {
                _target = collider.transform;
                maxDistanceTravelled = collider.GetComponent<Enemy>().GetDistanceTravelled();
            }
        }
    }
    protected void RotateTowardsTarget()
    {
        if (_target == null) return;

        // Ÿ���� �ٶ󺸴� ������ ���
        Vector3 direction = (_target.transform.position - transform.position).normalized;

        // Ÿ���� ���� ȸ������ ��� (y�� ȸ���� ����)
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    public void ClearSynergy()
    {
        synergyDebuffs.Clear();

        _synergyAttackInterval = 0;
        _synergyDamagePlus = 1;
    }
    void OnDrawGizmos()
    {
        // �����⿡�� �ð������� ������ Ȯ���ϱ� ���� �׸���
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _findRange);
    }
}
