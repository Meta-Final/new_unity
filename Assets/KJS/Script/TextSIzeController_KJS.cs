using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class TextSIzeController_KJS : MonoBehaviour
{
    public TextMeshProUGUI targetText; // ��� TMP �ؽ�Ʈ
    private RectTransform rectTransform;
    private float previousHeight;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (targetText == null)
        {
            Debug.LogError("Target TextMeshProUGUI�� �Ҵ����ּ���.");
            return;
        }

        previousHeight = targetText.rectTransform.rect.height;
    }

    private void Update()
    {
        float currentHeight = targetText.rectTransform.rect.height;

        if (Mathf.Abs(currentHeight - previousHeight) > Mathf.Epsilon)
        {
            AdjustHeight(currentHeight);
            previousHeight = currentHeight;
        }
    }

    private void AdjustHeight(float newHeight)
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = newHeight;
        rectTransform.sizeDelta = sizeDelta;
    }
}
