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
        if (!photonView.IsMine) return; // 자신이 아닌 다른 플레이어는 로컬 입력을 무시

        bool isWalking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        // RPC 호출을 통해 모든 클라이언트의 애니메이션 상태를 업데이트
        photonView.RPC("SetAnimationState", RpcTarget.All, isWalking);
    }

    [PunRPC]
    void SetAnimationState(bool isWalking)
    {
        animator.SetBool("Walking", isWalking); // 애니메이터 파라미터 "Walking"에 키 입력 상태 반영
    }
}