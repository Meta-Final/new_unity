using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class H_PlayerMove : MonoBehaviour
{
    public float MoveSpeed = 30f;
    CharacterController H_cc;

    void Start()
    {
        H_cc = GetComponent<CharacterController>();      
    }
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        //transform.position += dir * MoveSpeed * Time.deltaTime;

        H_cc.Move(dir * MoveSpeed * Time.deltaTime);

        // 이동 방향에 따라 플레이어 회전
        if (dir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);
        }
    }
}
