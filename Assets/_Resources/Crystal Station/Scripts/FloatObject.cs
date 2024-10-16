using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatObject : MonoBehaviour
{
    [SerializeField]
    private float amplitude = 0.5f; // �������� ����
    [SerializeField]
    private float speed = 2.0f; // �������� �ӵ�
    private Vector3 startPosition;

    private void Awake()
    {
        // ���� ��ġ ����
        startPosition = transform.position;
    }

    private void Update()
    {
        // �ð��� ���� y�� ��ġ ����
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
