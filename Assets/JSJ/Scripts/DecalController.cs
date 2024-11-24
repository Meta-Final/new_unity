using UnityEngine;

public class DecalController : MonoBehaviour
{
    public RenderTexture renderTexture;  // 비디오가 재생될 RenderTexture

    void Start()
    {
        // 데칼 렌더러를 가져옴
        Renderer decalRenderer = GetComponent<Renderer>();

        // 데칼의 머티리얼에 RenderTexture를 텍스쳐로 설정
        decalRenderer.material.mainTexture = renderTexture;
    }
}

