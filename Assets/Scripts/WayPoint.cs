using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] public Vector3[] points; // �� �̵� ����Ʈ
    [SerializeField] Color gizmoColor = Color.green; // ���� ����

    void OnDrawGizmos()
    {
        if (points != null && points.Length > 1)
        {
            // ���� ������ ����
            Gizmos.color = gizmoColor;

            for (int i = 0; i < points.Length - 1; i++)
            {
                // ���� ����Ʈ�� ���� ����Ʈ ���̿� �� �׸���
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
        }
    }
}