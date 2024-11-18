using UnityEngine;

public class H_MergeMgr : MonoBehaviour
{
    private Vector3 mousePosition;
    private float offsetX, offsetY, offsetZ;
    public static bool mouseButtonReleased;
    private bool isMerged = false;

    public string mergeResultSpriteName; // ���� ����� ������ ��������Ʈ �̸�

    // ��������Ʈ �̸� ���� �޼��� �߰�
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
        if (isMerged) return; // �̹� ������ ���¶�� �������� ����

        // ���ǿ� ���� ����Ʈ�� ��ġ��
        if (mouseButtonReleased && collision.gameObject.CompareTag("PostIt"))
        {
            // ���� ������Ʈ ����
            Destroy(collision.gameObject);
            Destroy(gameObject);

            // ���ο� 2D �̹��� ����
            Create2DImage(mergeResultSpriteName, transform.position);
        }
    }

    private void Create2DImage(string spriteName, Vector3 position)
    {
        // ���ҽ����� ��������Ʈ �ε�
        Sprite sprite = Resources.Load<Sprite>(spriteName);
        if (sprite == null)
        {
            Debug.LogError($"Sprite {spriteName} not found in Resources.");
            return;
        }

        // 2D �̹��� ������Ʈ ����
        GameObject imageObject = new GameObject("Merged2DImage");
        imageObject.transform.position = position;

        SpriteRenderer renderer = imageObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = 10; // ���̾� ����
    }
}
