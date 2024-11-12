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
        if (!photonView.IsMine) return; // �ڽ��� �ƴ� �ٸ� �÷��̾�� ���� �Է��� ����

        bool isWalking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        // RPC ȣ���� ���� ��� Ŭ���̾�Ʈ�� �ִϸ��̼� ���¸� ������Ʈ
        photonView.RPC("SetAnimationState", RpcTarget.All, isWalking);
    }

    [PunRPC]
    void SetAnimationState(bool isWalking)
    {
        animator.SetBool("Walking", isWalking); // �ִϸ����� �Ķ���� "Walking"�� Ű �Է� ���� �ݿ�
    }
}