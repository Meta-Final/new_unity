using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(H_MergeDrag))]
public class H_MergeItem : MonoBehaviour
{
    // ������ ID�� ������ ����
    public int id;
    // �����ۿ� ������ �� �迭
    [SerializeField] private Color[] colors;

    // ������ �������� ���� �Լ�
    public void SetItem(int newValue, Transform newParent)
    {
        id = newValue;
        // ������ ���� �Ҵ�
        GetComponentInChildren<Image>().color = SetColor(id);
        // �ڽ��� �ؽ�Ʈ ������Ʈ�� ���� �Ҵ�
        //GetComponentInChildren<Text>().text = id.ToString();

        transform.SetParent(newParent);
    }

    // ���ں��� ���� �Ҵ��ϴ� �Լ�
    public Color SetColor(int colorValue)
    {
        if (colorValue < colors.Length + 1)
            return colors[colorValue - 1];
        else
            return Color.black;
    }
}
