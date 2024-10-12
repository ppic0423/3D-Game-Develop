using UnityEngine;
using UnityEngine.EventSystems;

public class MouseInput : MonoBehaviour
{
    [SerializeField] LayerMask targetLayer;
    Selector currentSelector;
    GameObject currentObj;
    Selector cellSelector;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        cellSelector = GetComponentInChildren<CellHandler>();

        cellSelector.mouseInput = this;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // ���콺 ��ġ���� ���� �߻�
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ���̰� ������Ʈ�� �浹�ߴ��� Ȯ��
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayer))
            {
                // ���̰� UI�� �浹�� ��� ����
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                currentObj = hit.collider.gameObject;

                // �浹�� ������Ʈ�� ���̾ ���� ��� ����
                switch (hit.collider.gameObject.layer)
                {
                    case (int)Define.Layer.Cell:
                        SetSelector(cellSelector);
                        break;
                    default:
                        SetSelector(null);
                        currentObj = null;
                        break;
                }
            }
        }

        if(currentSelector != null)
        {
            currentSelector.Tick();
        }
        // TODO : ���õ� ������Ʈ, ������ ������ ���⼭
    }

    public void SetSelector(Selector selector)
    {
        if(currentSelector != null)
        {
            currentSelector.Exit();
        }

        currentSelector = selector;

        if(currentSelector != null)
        {
            currentSelector.target = currentObj;
            currentSelector.Enter();
        }
    }
}
