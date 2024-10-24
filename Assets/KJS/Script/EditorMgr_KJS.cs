using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class EditorMgr_KJS : MonoBehaviour
{
    public TMP_InputField inputField;         // �ؽ�Ʈ�� ������ InputField
    public TMP_Dropdown fontSizeDropdown;
    public TMP_InputField fontSizeInputField;
    public Button boldButton;
    public Button italicButton;

    // ���ο� ��ư��
    public Button underlineButton;
    public Button strikethroughButton;
    public Button alignLeftButton;
    public Button alignCenterButton;
    public Button alignRightButton;

    public Color pressedColor = new Color(0.8f, 0.8f, 0.8f);
    public Color defaultColor = Color.white;

    private int fontSize = 40;
    private bool isBold = false;
    private bool isItalic = false;
    private bool isUnderline = false;
    private bool isStrikethrough = false;
    private Button selectedButton;  // ���� ���õ� ��ư

    private const float FontSizeMultiplier = 1.5f;

    public void Start()
    {
        SetFunction_UI();

        // Dropdown �ɼ� �ʱ�ȭ
        fontSizeDropdown.ClearOptions();
        List<string> fontSizeOptions = new List<string>();
        for (int i = 10; i <= 98; i += 2)
        {
            fontSizeOptions.Add(i.ToString());
        }
        fontSizeDropdown.AddOptions(fontSizeOptions);
        fontSizeDropdown.value = fontSizeOptions.IndexOf(fontSize.ToString());

        // �̺�Ʈ ���� ���� (OnClick)
        fontSizeDropdown.onValueChanged.AddListener(OnFontSizeDropdownChanged);
        fontSizeInputField.onEndEdit.AddListener(OnFontSizeInputFieldChanged);

        boldButton.onClick.AddListener(() => ToggleStyle(ref isBold, boldButton));
        italicButton.onClick.AddListener(() => ToggleStyle(ref isItalic, italicButton));
        underlineButton.onClick.AddListener(() => ToggleStyle(ref isUnderline, underlineButton));
        strikethroughButton.onClick.AddListener(() => ToggleStyle(ref isStrikethrough, strikethroughButton));

        alignLeftButton.onClick.AddListener(() => SetAlignment(TextAlignmentOptions.Left));
        alignCenterButton.onClick.AddListener(() => SetAlignment(TextAlignmentOptions.Center));
        alignRightButton.onClick.AddListener(() => SetAlignment(TextAlignmentOptions.Right));

        inputField.onValueChanged.AddListener(OnInputFieldTextChanged);

        // �ʱ� ��Ʈ ����
        inputField.textComponent.fontSize = fontSize;
        fontSizeInputField.text = fontSize.ToString();
    }

    public void SetFunction_UI()
    {
        ResetFunction_UI();
    }

    public void ResetFunction_UI()
    {
        inputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Input..";
        inputField.contentType = TMP_InputField.ContentType.Standard;
        inputField.lineType = TMP_InputField.LineType.MultiLineNewline;
    }

    public void OnFontSizeDropdownChanged(int index)
    {
        string selectedValue = fontSizeDropdown.options[index].text;
        if (int.TryParse(selectedValue, out int newSize))
        {
            fontSize = newSize;
            inputField.textComponent.fontSize = fontSize;
            fontSizeInputField.text = fontSize.ToString();
            UpdateButtonTextStyle();
        }
    }

    public void OnFontSizeInputFieldChanged(string input)
    {
        if (int.TryParse(input, out int newSize) && newSize >= 10 && newSize <= 100)
        {
            fontSize = newSize;
            inputField.textComponent.fontSize = fontSize;

            int dropdownIndex = fontSizeDropdown.options.FindIndex(option => option.text == fontSize.ToString());
            if (dropdownIndex != -1)
            {
                fontSizeDropdown.value = dropdownIndex;
            }

            UpdateButtonTextStyle();
        }
        else
        {
            fontSizeInputField.text = fontSize.ToString();
        }
    }

    // ��Ÿ���� ����ϴ� ���� �Լ� (Bold, Italic, Underline, Strikethrough)
    private void ToggleStyle(ref bool styleFlag, Button button)
    {
        styleFlag = !styleFlag;
        UpdateTextStyle();
        button.GetComponent<Image>().color = styleFlag ? pressedColor : defaultColor;
    }

    private void SetAlignment(TextAlignmentOptions alignment)
    {
        inputField.textComponent.alignment = alignment;
    }

    private void UpdateTextStyle()
    {
        inputField.textComponent.fontStyle = FontStyles.Normal;

        if (isBold) inputField.textComponent.fontStyle |= FontStyles.Bold;
        if (isItalic) inputField.textComponent.fontStyle |= FontStyles.Italic;
        if (isUnderline) inputField.textComponent.fontStyle |= FontStyles.Underline;
        if (isStrikethrough) inputField.textComponent.fontStyle |= FontStyles.Strikethrough;

        // ���õ� ��ư�� �ؽ�Ʈ ��Ÿ�ϵ� ������Ʈ
        UpdateButtonTextStyle();
    }

    // ��ư�� �ؽ�Ʈ�� InputField�� �����ϰ� ��Ÿ�� ����ȭ
    public void SetInputFieldTextFromButton(Button button)
    {
        selectedButton = button;  // ���� ���õ� ��ư ����

        // ��ư�� �ؽ�Ʈ�� InputField�� ����
        string buttonText = button.GetComponentInChildren<TextMeshProUGUI>().text;
        inputField.text = buttonText;

        // ��ư�� �ؽ�Ʈ ��Ÿ���� InputField�� �ݿ�
        TMP_Text buttonTextComponent = button.GetComponentInChildren<TMP_Text>();
        inputField.textComponent.fontSize = (int)(buttonTextComponent.fontSize / FontSizeMultiplier);
        inputField.textComponent.fontStyle = buttonTextComponent.fontStyle;

        // ��Ʈ ũ�� InputField ������Ʈ
        fontSizeInputField.text = ((int)(buttonTextComponent.fontSize / FontSizeMultiplier)).ToString();
    }

    public void OnInputFieldTextChanged(string newText)
    {
        if (selectedButton != null)
        {
            selectedButton.GetComponentInChildren<TextMeshProUGUI>().text = newText;
        }
    }

    private void UpdateButtonTextStyle()
    {
        if (selectedButton != null)
        {
            TMP_Text buttonTextComponent = selectedButton.GetComponentInChildren<TMP_Text>();
            buttonTextComponent.fontSize = inputField.textComponent.fontSize * FontSizeMultiplier;
            buttonTextComponent.fontStyle = inputField.textComponent.fontStyle;
        }
    }
}