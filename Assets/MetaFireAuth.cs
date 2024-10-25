using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MetaFireAuth : MonoBehaviour
{

    public static MetaFireAuth instance;

    public FirebaseAuth auth;

    private void Awake()
    {
        instance = this;

     
    }
    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        //로그인 상태 체크 이벤트 등록
        auth.StateChanged += OnChangedAuthState;
    } 

    void OnChangedAuthState(object sender, EventArgs e)
    {
        //만약에 현재 유저정보(내정보)가 있다면 로그인
        if(auth.CurrentUser != null)
        {
            print("로그인 되었습니다.");
        }
        //그렇지 않으면 로그아웃
        else
        {
            print("로그 아웃 되었습니다.");
            SignIn("a@gmail.com", "123456");
        }
    }

    public void SignUp(string email, string password)
    {
        StartCoroutine(CoSignUp(email, password));
    }

    IEnumerator CoSignUp(string email, string password)
    {
        // 회원가입 시도
        Task<AuthResult> task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        // 통신이 완료될때까지 기다리자.
        yield return new WaitUntil(() => { return task.IsCompleted; });

        // 만약에 예외가 없다면
        if(task.Exception == null)
        {
            print("회원가입 성공");
        }
        else
        {
            print("화원가입 실패 : " + task.Exception);
        }
     }

    public void SignIn(string email, string password)
    {
        StartCoroutine(CoSignIn(email, password));
    }

    IEnumerator CoSignIn(string email, string password)
    {
        // 로그인 시도
        Task<AuthResult> task = auth.SignInWithEmailAndPasswordAsync(email, password);
        // 통신이 완료될때까지 기다리자.
        yield return new WaitUntil(() => { return task.IsCompleted; });

        // 만약에 예외가 없다면
        if (task.Exception == null)
        {
            print("로그인 성공");
        }
        else
        {
            print("로그인 실패 : " + task.Exception);
        }
    }

    public void SignOut()
    {
        auth.SignOut();
    }
}
