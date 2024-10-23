using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("이동")]
    public float moveSpeed = 5f;

    [Header("회전")]
    public float rotSpeed = 200f;

    [Header("점프")]
    public float jumpPower = 3f;
    public int jumpMaxCount = 1;

    float yPos;
    int jumpCurrentCount;

    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        Moving();
    }

    public void Moving()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        if (!(h == 0 & v == 0))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
        }
        
        // 중력 적용
        yPos += Physics.gravity.y * Time.deltaTime;

        // 바닥에 닿았을 때
        if(cc.collisionFlags == CollisionFlags.CollidedBelow)
        {
            yPos = 0;
            jumpCurrentCount = 0;
        }

        // 점프
        if(Input.GetKeyDown(KeyCode.Space) & jumpCurrentCount < jumpMaxCount)
        {
            yPos = jumpPower;
            jumpCurrentCount++;
        }

        dir.y = yPos;

        cc.Move(dir * moveSpeed * Time.deltaTime);
    }
}
