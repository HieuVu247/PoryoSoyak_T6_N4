using UnityEngine;

public class ReplaceAndDestroySelf : MonoBehaviour
{
    [System.Serializable]
    public struct TagPrefabPair
    {
        public string targetTag;
        public GameObject replacementPrefab;
    }

    [SerializeField] private TagPrefabPair[] replaceableObjects;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string hitTag = collision.gameObject.tag;

        foreach (TagPrefabPair pair in replaceableObjects)
        {
            if (hitTag == pair.targetTag)
            {
                // Lấy vị trí và góc quay của object cX
                Vector3 pos = collision.transform.position;
                Quaternion rot = collision.transform.rotation;

                // Hủy object cX
                Destroy(collision.gameObject);

                // Sinh prefab thay thế
                Instantiate(pair.replacementPrefab, pos, rot);

                // Hủy chính object g1 (object gắn script này)
                Destroy(gameObject);

                Debug.Log($"Đã thay thế {hitTag} và hủy g1");
                break; // Không xử lý thêm
            }
        }
    }
}
