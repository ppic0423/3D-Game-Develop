using System.Collections.Generic;
using UnityEngine;

public class CellHandler : Selector
{
    [Header("UI Elements")]
    private RectTransform currentUI;
    [SerializeField] private RectTransform onTowerUI;
    [SerializeField] private RectTransform emptyUI;
    [SerializeField] private GameObject turretShop1;
    [SerializeField] private GameObject turretShop2;

    [SerializeField] GameObject turretInfo_1;
    [SerializeField] private GameObject turretInfo_2;

    public override void Enter()
    {
        Tile tile = target.GetComponent<Tile>();

        if (tile.turret != null)
        {
            // Ÿ�Ͽ� �ͷ��� ���� ���
            onTowerUI.gameObject.SetActive(true);
            currentUI = onTowerUI;
            turretInfo_1.SetActive(true);
            turretInfo_2.SetActive(false);
        }
        else
        {
            // Ÿ���� ������� ���
            emptyUI.gameObject.SetActive(true);
            turretShop1.SetActive(true);
            turretShop2.SetActive(false);
            currentUI = emptyUI;
        }
    }

    public override void Tick()
    {
        // UI�� ��ġ�� Ÿ���� ��ġ�� ����
        currentUI.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
    }

    public override void Exit()
    {
        // ��� UI ��Ȱ��ȭ
        onTowerUI.gameObject.SetActive(false);
        emptyUI.gameObject.SetActive(false);
    }

    // �ͷ� ����
    public void BuildTurret(GameObject prefab)
    {
        Tile tile = target.GetComponent<Tile>();
        Turret newTurret = prefab.GetComponent<Turret>();

        // ��� ���� �� ��ȯ
        if (!CanAffordTurret(newTurret.Cost))
            return;

        // �ͷ� ���� �� ��� ����
        ResourceManager.Instance.UseGold(newTurret.Cost);
        Turret turretInstance = Instantiate(prefab, tile.transform).GetComponent<Turret>();
        tile.turret = turretInstance;

        // �ֺ� Ÿ���� �ͷ� �ó��� ����
        List<Turret> nearbyTurrets = GetNearbyTurrets(tile);
        ApplySynergies(turretInstance, nearbyTurrets);

        // ���콺 �Է� ����
        mouseInput.SetSelector(null);
    }

    // �ͷ� ����
    public void RemoveTurret()
    {
        Tile tile = target.GetComponent<Tile>();
        if (tile.turret == null)
            return;

        Destroy(tile.turret.gameObject);
        tile.turret = null;

        // ���콺 �Է� ����
        mouseInput.SetSelector(null);
    }

    // �ͷ� ���׷��̵�
    public void UpgradeTurret()
    {
        Turret turret = target.GetComponent<Tile>().turret;
        turret?.Upgrade();
    }

    // �ͷ� �Ǹ�
    public void SellTurret()
    {
        Turret turret = target.GetComponent<Tile>().turret;
        if (turret == null)
            return;

        ResourceManager.Instance.AddGold((int)(turret.Cost * 2 / 3));
        RemoveTurret();
    }

    // �ͷ� ���� ���� ���� Ȯ��
    private bool CanAffordTurret(int cost)
    {
        return ResourceManager.Instance.Gold >= cost;
    }

    // �ó��� ���� �Լ�
    private void ApplySynergies(Turret turret, List<Turret> nearbyTurrets)
    {
        // ���� ������ �ͷ��� �ó��� ����
        SynergyManager.Instance.CheckAndApplySynergy(turret, nearbyTurrets);

        // �ֺ� �ͷ����� �ó����� ����
        foreach (Turret nearbyTurret in nearbyTurrets)
        {
            List<Turret> neighbourTurrets = GetNearbyTurrets(nearbyTurret.GetComponentInParent<Tile>());
            SynergyManager.Instance.CheckAndApplySynergy(nearbyTurret, neighbourTurrets);
        }
    }

    // �ֺ� Ÿ�Ͽ��� �ͷ��� ã�� �Լ�
    private List<Turret> GetNearbyTurrets(Tile tile)
    {
        List<Turret> nearbyTurrets = new List<Turret>();

        foreach (Tile neighbor in tile.parentGrid.Neighbours(tile))
        {
            if (neighbor.turret != null)
            {
                nearbyTurrets.Add(neighbor.turret);
            }
        }

        return nearbyTurrets;
    }
}
