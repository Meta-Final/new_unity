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

    private bool canMove = true; // `Moving` 활성화 여부를 제어하는 플래그
    private bool isRotatingToHelper = false; // Helper 방향 회전 활성화 플래그
    private Transform helperTarget; // Helper 오브젝트의 Transform

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
            // Helper 방향으로 회전 중이라면
            if (isRotatingToHelper && helperTarget != null)
            {
                RotateToHelper(); // Helper 방향으로 회전
            }
            else if (canMove)
            {
                Moving(); // 일반 이동 처리
            }
        }

        canvasNickName.transform.rotation = Quaternion.LookRotation(canvasNickName.transform.position - Camera.main.transform.position);
    }

    // 플레이어 이동 관련 함수
    public void Moving()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        moveState = dir.magnitude;

        // Player 애니메이션
        animator.SetFloat("Moving", moveState);

        if (!(h == 0 & v == 0))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
        }

        // 중력 적용
        yPos += Physics.gravity.y * Time.deltaTime;

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

        cc.Move(dir * moveSpeed * Time.deltaTime);
    }


    // `Moving` 기능 활성화/비활성화
    public void EnableMoving(bool enable)
    {
        canMove = enable;

        // 이동 중단 시 애니메이션도 초기화
        if (!enable)
        {
            animator.SetFloat("Move", 0f);
        }
    }


    // Helper 방향으로 회전 시작
    public void StartRotateToHelper()
    {
        // "Helper" 태그를 가진 오브젝트 찾기
        GameObject helperObject = GameObject.FindGameObjectWithTag("Helper");

        if (helperObject != null)
        {
            helperTarget = helperObject.transform; // Helper의 Transform 저장
            isRotatingToHelper = true; // 회전 활성화
            EnableMoving(false); // 이동 중단
        }
        else
        {
            Debug.LogWarning("Helper 오브젝트를 찾을 수 없습니다.");
        }
    }


    // Helper 방향으로 회전
    private void RotateToHelper()
    {
        if (helperTarget == null) return;

        // Helper의 방향 계산
        Vector3 directionToHelper = helperTarget.position - transform.position;
        directionToHelper.y = 0; // Y축 고정 (수평 방향 회전만 적용)

        // 현재 방향에서 Helper 방향으로 점진적으로 회전
        Quaternion targetRotation = Quaternion.LookRotation(directionToHelper);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);

        // Helper 방향으로 거의 회전 완료 시
        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            isRotatingToHelper = false; // 회전 중단
            Debug.Log("Helper 방향으로 회전 완료!");
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

            animator.SetFloat("Moving", moveState);
        }
    }
}