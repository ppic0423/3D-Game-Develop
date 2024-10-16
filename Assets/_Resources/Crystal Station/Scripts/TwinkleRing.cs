using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private float minIntensity = 0.8f; // �ּ� Intensity
    [SerializeField]
    private float maxIntensity = 2f; // �ִ� Intensity
    [SerializeField]
    private float speed = 0.65f; // ��ȭ �ӵ�

    private Renderer objectRenderer;
    private Material material;
    private Color baseColor;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        material = objectRenderer.material;
        baseColor = material.GetColor("_EmissionColor");
    }

    private void Update()
    {
        // �ð��� ���� Intensity �� ���
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time * speed, 1));
        Color newColor = baseColor * intensity;

        // Emission ���� ������Ʈ
        material.SetColor("_EmissionColor", newColor);
    }
}
