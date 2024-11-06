
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ButtonMgr : MonoBehaviour
{
    public InputField inputemail;
    public InputField inputpassword;
    //public GameObject Loginbox;
    public GameObject Joinbox;
    public GameObject Accountbox;

    void Start()
    {
        Joinbox.SetActive(false);
        Accountbox.SetActive(false);
    }

    void Update()
    {
        
    }
    public void OnClickJoinbox()
    {
        
        //print("회원가입창 까꿍");
        Joinbox.SetActive(true);
     
    }

    public void OnClickSignIn()
    {
        MetazipAuth.instance.SignIn(inputemail.text, inputpassword.text);
    }

    public void OnClickSignOut()
    {
        MetazipAuth.instance.SignOut();
    }

    public void OnClickNextSignUp()
    {
        Joinbox.SetActive(false);
        Accountbox.SetActive(true);
    }

}
