using UnityEngine;
using UnityEngine.UI;
// �� ��ũ��Ʈ�� �巡�׸Ŵ����� �ʼ� �䱸��.
[RequireComponent(typeof(H_MergeDrag))]
public class Item : MonoBehaviour
{
    // ������ ID�� ���� �� ����
    public int id;
    // �������� ������ �� �迭
    [SerializeField] Color[] colors;

    Transform parent;
    H_MergeDrag dragMgr;

    private void Start()
    {
        dragMgr = transform.GetComponent<H_MergeDrag>();
        parent = transform.parent;
        //dragMgr.startParent = parent;
    }

    // ������ �������� ���� �Լ�
    public void SetItem(int newValue, Transform newParent)
    {
        id = newValue;
        // ������ ���� �Ҵ�
        GetComponentInChildren<Image>().color = SetColor(id);
        // �ڽ��� �ؽ�Ʈ ������Ʈ�� ���� �Ҵ�
        GetComponentInChildren<Text>().text = id.ToString();

        transform.SetParent(newParent);
    }

    //���� ���� �� �Ҵ��� ���� �Լ�
    public Color SetColor(int colorValue)
    {
        if (colorValue < 10)
            return colors[colorValue - 1];
        else
            return Color.black;
    }
}