using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(H_MergeDrag))]
public class H_MergeItem : MonoBehaviour
{
    // 아이템 ID를 저장할 변수
    public int id;
    // 아이템에 적용할 색 배열
    [SerializeField] private Color[] colors;

    // 생성된 아이템의 설정 함수
    public void SetItem(int newValue, Transform newParent)
    {
        id = newValue;
        // 아이템 배경색 할당
        GetComponentInChildren<Image>().color = SetColor(id);
        // 자식의 텍스트 컴포넌트에 숫자 할당
        //GetComponentInChildren<Text>().text = id.ToString();

        transform.SetParent(newParent);
    }

    // 숫자별로 색을 할당하는 함수
    public Color SetColor(int colorValue)
    {
        if (colorValue < colors.Length + 1)
            return colors[colorValue - 1];
        else
            return Color.black;
    }
}
