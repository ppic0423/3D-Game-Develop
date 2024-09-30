using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("ī�޶� �̵�")]
    [SerializeField] float panSpeed = 20f;             // ī�޶� �̵� �ӵ�
    [SerializeField] float panBorderThickness = 10f;   // ȭ�� ������ ī�޶� �����̱� �����ϴ� �Ÿ�
    [SerializeField] Vector2 panLimit;                 // ī�޶� �̵� ���� (x, y ��)

    [Header("ī�޶� ��")]
    [SerializeField] float scrollSpeed = 20f;          // �� �ӵ�
    [SerializeField] float minY = 20f;                 // ī�޶� �� �ּҰ�
    [SerializeField] float maxY = 120f;                // ī�޶� �� �ִ밪

    Vector3 position;

    void Update()
    {
        position = transform.position;
    }
    private void FixedUpdate()
    {
        HandleMovement();  // ī�޶� �̵� ó��
        HandleZoom();      // ī�޶� �� ó��
    }

    // ī�޶� �̵��� ó���ϴ� �Լ�
    void HandleMovement()
    {
        // ���콺 �Ǵ� Ű����� ī�޶� �̵�
        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            position.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            position.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            position.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            position.x -= panSpeed * Time.deltaTime;
        }

        // ī�޶� �̵� ���� ����
        position.x = Mathf.Clamp(position.x, -panLimit.x, panLimit.x);
        position.z = Mathf.Clamp(position.z, -panLimit.y, panLimit.y);

        // ���ο� ��ġ�� ī�޶� ����
        transform.position = position;
    }
    // ī�޶� ���� ó���ϴ� �Լ�
    void HandleZoom()
    {
        Vector3 pos = transform.position;

        // ���콺 �ٷ� �� ����
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        // ī�޶� �� ���� ����
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        // ���ο� ��ġ�� ī�޶� ����
        transform.position = pos;
    }
}
