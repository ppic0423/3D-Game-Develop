using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float findRange = 5f;  // Ž�� ����
    public Transform center;  // Ž�� �߽� ��ġ

    public GameObject target;
    void Start()
    {
        if(center == null)
        {
            center = GetComponent<Transform>();
        }
    }

    private void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (target != null)
        {

        }
    }

    void FindObjectsInRange()
    {
        // ���� ���� �ִ� ��� �ݶ��̴��� ������
        Collider[] hitColliders = Physics.OverlapSphere(center.position, findRange, (int)Define.Layer.Enemy);

        // �� �ݶ��̴��� ������Ʈ �̸��� ���
        foreach (Collider collider in hitColliders)
        {
            float compareDistance = collider.gameObject.GetComponent<Enemy>().GetDistanceTravelled();

            // Ÿ���� ���ų� ���� Ÿ�ٺ��� �� �ָ����� ���
            if (target == null || compareDistance > target.GetComponent<Enemy>().GetDistanceTravelled())
            {
                target = collider.gameObject;
            }
        }
    }

    void OnDrawGizmos()
    {
        // �����⿡�� �ð������� ������ Ȯ���ϱ� ���� �׸���
        if (center != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(center.position, findRange);
        }
    }
}
