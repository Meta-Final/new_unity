using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginSceneMgr : MonoBehaviour
{
    [Header("Button")]
    public Button btn_SignIn;   // 로그인 버튼
    public Button btn_SignOut;  // 로그아웃 버튼
    public Button btn_Join;     // 회원가입 UI 버튼
    public Button btn_Next;     // 넘기기 버튼
    public Button btn_SignUp;   // 회원가입 진행 버튼

    public GameObject canvasJoin;    // 회원가입 UI
    public GameObject canvasAccount;   // User 정보기입 창

    [Header("Input Field")]
    public TMP_InputField signInEmail;       // 로그인 이메일
    public TMP_InputField signInPassword;    // 로그인 비밀번호
    public TMP_InputField signUpEmail;      // 회원가입 이메일   
    public TMP_InputField signUpPassword;   // 회원가입 비밀번호


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // 로그인
    public void OnClickSignIn()
    {
        FireAuthManager.instance.OnSignIn(signInEmail.text, signInPassword.text);
    }

    // 로그아웃
    public void OnClickSignOut()
    {
        FireAuthManager.instance.LogOut();
    }

    public void OnClickJoin()
    {
        // 회원가입 UI 활성화
        canvasJoin.SetActive(true);
    }

    public void OnClickNext()
    {
        // User 정보기입 창 활성화
        canvasAccount.SetActive(true);
    }

    public void OnClickSignUp()
    {
        FireAuthManager.instance.OnSignUp(signUpEmail.text, signUpPassword.text);
    }

    
}
