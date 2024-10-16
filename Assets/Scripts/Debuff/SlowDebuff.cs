using UnityEngine;

public class SlowDebuff : Debuff
{
    float _slowAmount;
    GameObject effect;

    public SlowDebuff(float duration, float slowAmount) : base(duration)
    {
        this._slowAmount = slowAmount;
    }

    public override void Apply()
    {
        // ȿ�� ����
        enemy.MoveSpeed *= _slowAmount;
        
        // ����Ʈ ����
        effect = SlowDebuffPool.Instance.pool.Get();
        effect.transform.parent = enemy.transform;
        effect.transform.localPosition = Vector3.zero;
        base.Apply();
    }
    public override void Remove()
    {
        base.Remove();
        // ȿ�� ����
        enemy.MoveSpeed /= _slowAmount;

        // ����Ʈ ����
        SlowDebuffPool.Instance.pool.Release(effect);
    }
}
