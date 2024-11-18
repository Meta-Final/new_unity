using UnityEngine;

public class H_MergeMgr : MonoBehaviour
{
    private Vector3 mousePosition;
    private float offsetX, offsetY, offsetZ;
    public static bool mouseButtonReleased;
    private bool isMerged = false;

    public string mergeResultSpriteName; // 머지 결과로 생성될 스프라이트 이름

    // 스프라이트 이름 설정 메서드 추가
    public void SetMergeResultSprite(string spriteName)
    {
        mergeResultSpriteName = spriteName;
    }

    private void OnMouseDown()
    {
        mouseButtonReleased = false;
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offsetX = objectPosition.x - transform.position.x;
        offsetY = objectPosition.y - transform.position.y;
        offsetZ = objectPosition.z - transform.position.z;
    }

    private void OnMouseDrag()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x - offsetX, mousePosition.y - offsetY, transform.position.z);
    }
    private void OnMouseUp()
    {
        mouseButtonReleased = true;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (isMerged) return; // 이미 머지된 상태라면 실행하지 않음

        // 조건에 따라 포스트잇 합치기
        if (mouseButtonReleased && collision.gameObject.CompareTag("PostIt"))
        {
            // 기존 오브젝트 제거
            Destroy(collision.gameObject);
            Destroy(gameObject);

            // 새로운 2D 이미지 생성
            Create2DImage(mergeResultSpriteName, transform.position);
        }
    }

    private void Create2DImage(string spriteName, Vector3 position)
    {
        // 리소스에서 스프라이트 로드
        Sprite sprite = Resources.Load<Sprite>(spriteName);
        if (sprite == null)
        {
            Debug.LogError($"Sprite {spriteName} not found in Resources.");
            return;
        }

        // 2D 이미지 오브젝트 생성
        GameObject imageObject = new GameObject("Merged2DImage");
        imageObject.transform.position = position;

        SpriteRenderer renderer = imageObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = 10; // 레이어 조정
    }
}
