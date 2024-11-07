using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginSceneMgr : MonoBehaviour
{
    [Header("Button")]
    public Button btn_SignIn;   // �α��� ��ư
    public Button btn_SignOut;  // �α׾ƿ� ��ư
    public Button btn_Join;     // ȸ������ UI ��ư
    public Button btn_Next;     // �ѱ�� ��ư
    public Button btn_SignUp;   // ȸ������ ���� ��ư

    public GameObject canvasJoin;    // ȸ������ UI
    public GameObject canvasAccount;   // User �������� â

    [Header("Input Field")]
    public TMP_InputField signInEmail;       // �α��� �̸���
    public TMP_InputField signInPassword;    // �α��� ��й�ȣ
    public TMP_InputField signUpEmail;      // ȸ������ �̸���   
    public TMP_InputField signUpPassword;   // ȸ������ ��й�ȣ


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // �α���
    public void OnClickSignIn()
    {
        FireAuthManager.instance.OnSignIn(signInEmail.text, signInPassword.text);
    }

    // �α׾ƿ�
    public void OnClickSignOut()
    {
        FireAuthManager.instance.LogOut();
    }

    public void OnClickJoin()
    {
        // ȸ������ UI Ȱ��ȭ
        canvasJoin.SetActive(true);
    }

    public void OnClickNext()
    {
        // User �������� â Ȱ��ȭ
        canvasAccount.SetActive(true);
    }

    public void OnClickSignUp()
    {
        FireAuthManager.instance.OnSignUp(signUpEmail.text, signUpPassword.text);
    }

    
}
