using UnityEngine;
using Photon.Pun;

public class PlayerAnimationController : MonoBehaviourPun
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!photonView.IsMine) return; // 로컬 플레이어만 입력을 처리

        // WASD 키 입력을 체크하여 이동 상태를 확인
        bool isWalking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        // 모든 클라이언트에서 isWalking 상태를 동기화
        photonView.RPC("SetWalkingState", RpcTarget.All, isWalking);
    }

    [PunRPC]
    void SetWalkingState(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }
}