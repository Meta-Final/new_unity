using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    [Header("이동")]
    public float moveSpeed = 5f;

    [Header("회전")]
    public float rotSpeed = 200f;

    [Header("점프")]
    public float jumpPower = 3f;
    public int jumpMaxCount = 1;

    [Header("Terrain 보정")]
    public float groundOffset = 0.1f;

    float yPos;
    int jumpCurrentCount;

    float moveState;

    public Canvas canvasNickName;
    public TMP_Text playerNickName;

    Animator animator;
    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // 해당 캐릭터의 닉네임 설정
        playerNickName.text = photonView.Owner.NickName;
    }

    void Update()
    {
        // 내 것일 때만 컨트롤하자
        if (photonView.IsMine)
        {
            Moving();
        }

        canvasNickName.transform.rotation = Quaternion.LookRotation(canvasNickName.transform.position - Camera.main.transform.position);
    }

    public void Moving()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        moveState = dir.magnitude;

        // Player 애니메이션
        animator.SetFloat("Move", moveState);

        if (!(h == 0 & v == 0))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
        }
        
        // 중력 적용
        yPos += Physics.gravity.y * Time.deltaTime;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            float terrainHeight = hit.point.y + groundOffset;

            // 바닥에 닿았을 때
            if (cc.collisionFlags == CollisionFlags.CollidedBelow)
            {
                yPos = 0;
                jumpCurrentCount = 0;
            }

            // 점프
            if (Input.GetKeyDown(KeyCode.Space) & jumpCurrentCount < jumpMaxCount)
            {
                yPos = jumpPower;
                jumpCurrentCount++;
            }

            dir.y = yPos;

            if (transform.position.y < terrainHeight)
            {
                transform.position = new Vector3(transform.position.x, terrainHeight, transform.position.z);
            }

            cc.Move(dir * moveSpeed * Time.deltaTime);
        }

        
    }

    // 애니메이션 동기화
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(moveState);
        }
        else
        {
            moveState = (float)stream.ReceiveNext();
        }
    }
}
