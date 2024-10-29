using UnityEngine;
using UnityEngine.UI;

public class ButtonCreator : MonoBehaviour
{
    private SaveMgr_KJS saveMgr;  // SaveMgr 참조
    private Button buttonComponent;  // 버튼 컴포넌트

    void Awake()
    {
        // 버튼의 Button 컴포넌트 가져오기
        buttonComponent = GetComponent<Button>();

        // SaveMgr 오브젝트 찾기 (씬 내에 있는 경우)
        saveMgr = FindObjectOfType<SaveMgr_KJS>();

        // SaveMgr에 이 버튼을 할당
        if (saveMgr != null)
        {
            saveMgr.SetLoadButton(buttonComponent);  // SaveMgr의 loadButton에 이 버튼을 등록
        }

        // 버튼 클릭 이벤트 추가
        buttonComponent.onClick.AddListener(OnButtonClick);
    }

    // 버튼 클릭 시 호출될 메서드
    private void OnButtonClick()
    {
        Debug.Log("버튼이 클릭되었습니다!");
        // 여기서 원하는 동작을 수행합니다.
    }
}
