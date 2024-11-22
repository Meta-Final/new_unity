using UnityEngine;
using UnityEngine.UI;
// 이 스크립트는 드래그매니저를 필수 요구함.
[RequireComponent(typeof(H_MergeDrag))]
public class Item : MonoBehaviour
{
    // 아이템 ID를 저장 할 변수
    public int id;
    // 아이템의 적용할 색 배열
    [SerializeField] Color[] colors;

    Transform parent;
    H_MergeDrag dragMgr;

    private void Start()
    {
        dragMgr = transform.GetComponent<H_MergeDrag>();
        parent = transform.parent;
        //dragMgr.startParent = parent;
    }

    // 생성된 아이템의 설정 함수
    public void SetItem(int newValue, Transform newParent)
    {
        id = newValue;
        // 아이템 배경색 할당
        GetComponentInChildren<Image>().color = SetColor(id);
        // 자식의 텍스트 컴포넌트에 숫자 할당
        GetComponentInChildren<Text>().text = id.ToString();

        transform.SetParent(newParent);
    }

    //숫자 별로 색 할당을 위한 함수
    public Color SetColor(int colorValue)
    {
        if (colorValue < 10)
            return colors[colorValue - 1];
        else
            return Color.black;
    }
}