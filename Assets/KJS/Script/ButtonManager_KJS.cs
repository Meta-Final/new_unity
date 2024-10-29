using UnityEngine;
using UnityEngine.UI;

public class ButtonCreator : MonoBehaviour
{
    private SaveMgr_KJS saveMgr;  // SaveMgr ����
    private Button buttonComponent;  // ��ư ������Ʈ

    void Awake()
    {
        // ��ư�� Button ������Ʈ ��������
        buttonComponent = GetComponent<Button>();

        // SaveMgr ������Ʈ ã�� (�� ���� �ִ� ���)
        saveMgr = FindObjectOfType<SaveMgr_KJS>();

        // SaveMgr�� �� ��ư�� �Ҵ�
        if (saveMgr != null)
        {
            saveMgr.SetLoadButton(buttonComponent);  // SaveMgr�� loadButton�� �� ��ư�� ���
        }

        // ��ư Ŭ�� �̺�Ʈ �߰�
        buttonComponent.onClick.AddListener(OnButtonClick);
    }

    // ��ư Ŭ�� �� ȣ��� �޼���
    private void OnButtonClick()
    {
        Debug.Log("��ư�� Ŭ���Ǿ����ϴ�!");
        // ���⼭ ���ϴ� ������ �����մϴ�.
    }
}
