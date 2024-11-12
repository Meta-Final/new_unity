using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFreeAnim : MonoBehaviour
{

    Vector3 rot = Vector3.zero;
    float rotSpeed = 40f;
    Animator anim;

    // Use this for initialization
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        gameObject.transform.eulerAngles = rot;
    }

    // Update is called once per frame
    void Update()
    {
        CheckKey();
        gameObject.transform.eulerAngles = rot;
    }

    void CheckKey()
    {
        // Walk
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("Walk_Anim", true);
            anim.SetBool("Roll_Anim", true);
        }
        else
        {
            anim.SetBool("Walk_Anim", false);
            anim.SetBool("Roll_Anim", false);
        }

        // Rotate Left
        if (Input.GetKey(KeyCode.A))
        {
            rot[1] -= rotSpeed * Time.fixedDeltaTime;
        }

        // Rotate Right
        if (Input.GetKey(KeyCode.D))
        {
            rot[1] += rotSpeed * Time.fixedDeltaTime;
        }

        // Open/Close animation toggle
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            anim.SetBool("Open_Anim", !anim.GetBool("Open_Anim"));
        }
    }
}