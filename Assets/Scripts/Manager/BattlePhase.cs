using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePhase : Phase
{
    [SerializeField] EnemySpawner enemySpawner;

    // ��Ʋ ������ ���� : �������� ����, �� ������ ����, �Ǽ� �Ұ���
    public override void Enter()
    {
        phaseText.text = "BattlePhase";
        enemySpawner.Init();
    }
    
    public override void Exit()
    {
    }
}
