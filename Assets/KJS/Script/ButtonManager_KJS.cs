using UnityEngine;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCreator : MonoBehaviour
{
    public GameObject buttonPrefab; // Button ������
    public Transform parent; // ��ư�� ���� �θ� ������Ʈ
    public SaveMgr_KJS saveMgr; // SaveMgr ��ũ��Ʈ ����

    void Start()
    {
        CreateButton();
    }

    void CreateButton()
    {
        GameObject newButton = Instantiate(buttonPrefab, parent);
        Button buttonComponent = newButton.GetComponent<Button>();

        if (saveMgr != null)
        {
            saveMgr.SetLoadButton(buttonComponent); // SaveMgr�� loadButton�� �Ҵ�
        }
    }
}
