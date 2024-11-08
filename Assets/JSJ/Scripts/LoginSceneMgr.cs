using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 내정보 or 회원정보
[FirestoreData]
public class UserInfo
{
    [FirestoreProperty]
    public string name { get; set; }
    [FirestoreProperty]
    public string nickName { get; set; }
    [FirestoreProperty]
    public int userBirth { get; set; }
}

public class LoginSceneMgr : MonoBehaviour
{
    [Header("Button")]
    public Button btn_SignIn;   // 로그인 버튼
    public Button btn_SignOut;  // 로그아웃 버튼
    public Button btn_Join;     // 회원가입 UI 버튼
    public Button btn_SignUp;   // 회원가입 진행 버튼
    public Button btn_Next;     // 넘기기 버튼
    
    [Header("GameObject")]
    public GameObject img_SignInFail;      // 로그인 실패 UI
    public GameObject canvasJoin;          // 회원가입 UI
    public GameObject canvasAccount;       // User 정보기입 창
    public GameObject img_SignUpSuccess;   // 회원 가입 성공 UI

    [Header("로그인")]
    public TMP_InputField signInEmail;       // 로그인 이메일
    public TMP_InputField signInPassword;    // 로그인 비밀번호

    [Header("회원 가입")]
    public TMP_InputField signUpEmail;      // 회원가입 이메일   
    public TMP_InputField signUpPassword;   // 회원가입 비밀번호

    [Header("회원 정보")]
    public TMP_InputField userName;       
    public TMP_InputField userNickName;  
    public TMP_InputField userBirth;    
    



    void Start()
    {
        signInEmail.onValueChanged.AddListener(SignInValueChanged);
        signInPassword.onValueChanged.AddListener(SignInValueChanged);
        
    }

    void SignInValueChanged(string s)
    {
        btn_SignIn.interactable = s.Length > 0;
    }

    // 로그인
    public void OnClickSignIn()
    {
        FireAuthManager.instance.OnSignIn(signInEmail.text, signInPassword.text);
    }

    // 로그인 실패 UI
    public void ShowSignInFail()
    {
        StartCoroutine(SignInFail(2f));
    }

    IEnumerator SignInFail(float duration)
    {
        img_SignInFail.SetActive(true);
        yield return new WaitForSeconds(duration);
        img_SignInFail.SetActive(false);
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

    // 회원 가입
    public void OnClickSignUp()
    {
        UserInfo userInfo = new UserInfo();
        userInfo.name = userName.text;
        userInfo.nickName = userNickName.text;
        userInfo.userBirth = int.Parse(userBirth.text);

        FireAuthManager.instance.OnSignUp(signUpEmail.text, signUpPassword.text, userInfo);
    }

    // 회원 가입 성공 UI
    public void ShowSignUpSuccess()
    {
        StartCoroutine(SignUpSuccess(2f));
    }

    IEnumerator SignUpSuccess(float duration)
    {
        img_SignUpSuccess.SetActive(true);
        yield return new WaitForSeconds(duration);
        img_SignUpSuccess.SetActive(false);
    }

    // 회원정보 불러오기
    public void OnLoadUserInfo()
    {
        FireStore.instance.LoadUserInfo(onCompleteLoadUserInfo);
    }

    void onCompleteLoadUserInfo(UserInfo info)
    {
        print(info.name);
        print(info.nickName);
        print(info.userBirth);
    }

    // X 버튼
    public void OnClickX()
    {
        canvasJoin.SetActive(false);
        canvasAccount.SetActive(false);
    }



    




}
