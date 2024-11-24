using UnityEngine;

public class DecalController : MonoBehaviour
{
    public RenderTexture renderTexture;  // ������ ����� RenderTexture

    void Start()
    {
        // ��Į �������� ������
        Renderer decalRenderer = GetComponent<Renderer>();

        // ��Į�� ��Ƽ���� RenderTexture�� �ؽ��ķ� ����
        decalRenderer.material.mainTexture = renderTexture;
    }
}

