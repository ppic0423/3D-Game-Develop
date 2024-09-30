using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupPhase : Phase
{
    [SerializeField] GameObject mouseInput;

    // �غ� ������ ���� : �Ǽ� ����
    public override void Enter()
    {
        mouseInput.SetActive(true);
    }

    public override void Exit()
    {
        mouseInput.SetActive(false);
    }
}
