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
        if (!photonView.IsMine) return; // ���� �÷��̾ �Է��� ó��

        // WASD Ű �Է��� üũ�Ͽ� �̵� ���¸� Ȯ��
        bool isWalking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        // ��� Ŭ���̾�Ʈ���� isWalking ���¸� ����ȭ
        photonView.RPC("SetWalkingState", RpcTarget.All, isWalking);
    }

    [PunRPC]
    void SetWalkingState(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }
}