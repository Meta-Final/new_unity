
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

    void Start()
    {
        Joinbox.SetActive(false);
    }

    void Update()
    {
        
    }
    public void OnClickJoinbox()
    {
        
        //print("ȸ������â ���");
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

}
