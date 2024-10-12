using UnityEngine;

public class SlowBullet : Bullet
{
    Vector3 _boxSize;
    float _slowDuration;
    float _slowAmount;
    public override void Init(Transform target, float damage, float speed)
    {
        _target = target;
        _damage = damage;
        _speed = speed;
    }
    public void Init(Transform target, float damage, float speed, Vector3 range)
    {
        Init(target, damage, speed);
        _boxSize = range;
    }
    private void FixedUpdate()
    {
        MoveBullet();
    }
    protected override void HitTarget()
    {
        CommonBulletPool.Instance.pool.Release(this.gameObject); // ������Ʈ Ǯ�� ��ȯ

        // �ֺ� �� Ž��
        LayerMask targetLayer = LayerMask.GetMask("Enemy");
        Collider[] enemyColliders = Physics.OverlapBox(transform.position, _boxSize / 2, Quaternion.identity, targetLayer);

        // ������ ����� �ο�
        foreach (Collider collider in enemyColliders)
        {
            ApplyDebuffs(collider.GetComponent<Enemy>());
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, _boxSize);
    }
}
