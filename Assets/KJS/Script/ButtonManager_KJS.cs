using UnityEngine;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCreator : MonoBehaviour
{
    public GameObject buttonPrefab; // Button 프리팹
    public Transform parent; // 버튼을 담을 부모 오브젝트
    public SaveMgr_KJS saveMgr; // SaveMgr 스크립트 참조

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
            saveMgr.SetLoadButton(buttonComponent); // SaveMgr의 loadButton에 할당
        }
    }
}
