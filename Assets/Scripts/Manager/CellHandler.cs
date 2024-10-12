using System.Collections.Generic;
using UnityEngine;

public class CellHandler : Selector
{
    [Header("Grid")]
    public Grid grid;

    [Header("UI")]
    RectTransform current_UI;
    [SerializeField] RectTransform onTower_UI;
    [SerializeField] RectTransform empty_UI;
    [SerializeField] GameObject TurretShop_1;
    [SerializeField] GameObject TurretShop_2;

    public override void Enter()
    {
        if (target.GetComponent<Tile>().turret != null)
        {
            onTower_UI.gameObject.SetActive(true);
            current_UI = onTower_UI;
        }
        else
        {
            empty_UI.gameObject.SetActive(true);
            TurretShop_1.SetActive(true);
            TurretShop_2.SetActive(false);
            current_UI = empty_UI;
        }
    }
    public override void Tick()
    {
        current_UI.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
    }
    public override void Exit()
    {
        onTower_UI.gameObject.SetActive(false);
        empty_UI.gameObject.SetActive(false);
    }

    // �ͷ� ����
    public void BuildTurret(GameObject prefab)
    {
        // ��� Ȯ��
        int cost = prefab.GetComponent<Turret>().Cost;
        if (ResourceManager.Instance.Gold < cost)
            return;
        // ��� ���
        ResourceManager.Instance.UseGold(cost);

        // �ͷ� ����
        GameObject go = Instantiate(prefab, target.transform);
        Turret newTurret = go.GetComponent<Turret>();
        target.GetComponent<Tile>().turret = newTurret;

        /* �ó��� ���� */

        // �ֺ� Ÿ�� �� �ͷ� Ȯ��
        List<Tile> neighbourTiles = grid.Neighbours(target.GetComponent<Tile>());
        // ��� ���� �ͷ� �ó��� ������ ��� ����
        List<Turret> nearbyTurretsForNewTurret = new List<Turret>();

        // ��� ���� �ͷ��� �ó��� ����
        SynergyManager.Instance.CheckAndApplySynergy(newTurret, nearbyTurretsForNewTurret);
        
        // �ֺ� �ͷ� �ó��� ����
        foreach (Tile tile in neighbourTiles)
        {
            if (tile.turret != null)
            {
                nearbyTurretsForNewTurret.Add(tile.turret);

                // �̿� Ÿ���� �ͷ� �ó����� ������ ��� ����
                List<Turret> neighbourTurretsForExistingTurret = new List<Turret>();
                foreach (Tile neighbourTile in grid.Neighbours(tile))
                {
                    if (neighbourTile.turret != null)
                    {
                        neighbourTurretsForExistingTurret.Add(neighbourTile.turret);
                    }
                }

                // �̿� �ͷ� �ó��� ����
                SynergyManager.Instance.CheckAndApplySynergy(tile.turret, neighbourTurretsForExistingTurret);
            }
        }



        // ���콺 �Է� ����
        mouseInput.SetSelector(null);
    }
    // �ͷ� ����
    public void RemoveTurret()
    {
        if (target.GetComponent<Tile>().turret == null)
            return;

        Destroy(target.GetComponent<Tile>().turret.gameObject);
        target.GetComponent<Tile>().turret = null;

        mouseInput.SetSelector(null);
    }
    // �ͷ� ���׷��̵�
    public void UpgradeTurret()
    {
        Turret targetTurret = target.GetComponent<Tile>().turret;

        targetTurret.Upgrade();
    }
    // �ͷ� �Ǹ�
    public void SellTurret()
    {
        Turret targetTurret = target.GetComponent<Tile>().turret;

        ResourceManager.Instance.AddGold((int)(targetTurret.Cost * 2 / 3));
    }
}