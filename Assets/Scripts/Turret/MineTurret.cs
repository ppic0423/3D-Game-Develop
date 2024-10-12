using System.Collections.Generic;
using UnityEngine;

public class MineTurret : Turret
{
    [Header("Ư�� �ɷ�")]
    [SerializeField] float radius;
    List<MineBullet> mines; // ������ ���� ���

    [Header("�ѱ� ��ġ")]
    [SerializeField] Vector3 muzzlePos;

    private void Start()
    {
        Init();

        mines = new List<MineBullet>();
    }
    private void Update()
    {
        _attackIntervalDelta += Time.deltaTime;

        Attack();
    }

    protected override void Attack()
    {
        if (_target == null || _attackInterval - _synergyAttackInterval > _attackIntervalDelta || mines.Count >= 20)
            return;

        GameObject mineGo = MinePool.Instance.pool.Get();
        mineGo.transform.position = transform.TransformPoint(muzzlePos);
        mineGo.GetComponent<MineBullet>().Init(null, _damage * _synergyDamagePlus, _bulletSpeed, radius);

        AddDebuffOnBullet(mineGo);

        MineBullet mine = mineGo.GetComponent<MineBullet>();
        mine.m_List = mines;
        mines.Add(mine);

        _attackIntervalDelta = 0;
    }
    public override void Upgrade()
    {
        base.Upgrade();
        TurretUpgrade upgrade = upgrades[currentLevel];
        _damage += upgrade.damage;
        radius += upgrade.range;
    }
}