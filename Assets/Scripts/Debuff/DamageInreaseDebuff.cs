using UnityEngine;

public class DamageInreaseDebuff : Debuff
{
    float _damageIncreaseAmount;
    GameObject effect;

    public DamageInreaseDebuff(float duration, float damageIncreaseAmount) : base(duration)
    {
        _damageIncreaseAmount = damageIncreaseAmount;
    }

    public override void Apply()
    {
        // ����Ʈ
        effect = WeaknessEffectPool.Instance.pool.Get();
        effect.transform.parent = enemy.transform;
        effect.transform.localPosition = Vector3.zero;

        // ȿ�� ����
        enemy.DamageIncrease += _damageIncreaseAmount;
        base.Apply();
    }
    public override void Remove()
    {
        base.Remove();
        // ȿ�� ����
        enemy.DamageIncrease -= _damageIncreaseAmount;

        // ����Ʈ ����
        WeaknessEffectPool.Instance.pool.Release(effect);
    }
}
