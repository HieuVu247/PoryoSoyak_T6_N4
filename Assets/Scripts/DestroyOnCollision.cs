using UnityEngine;

public class MultiTagDestroyer : MonoBehaviour
{
    [SerializeField] private string[] attackerTags;     // p1, p2, p3
    [SerializeField] private string[] destroyableTags;  // c1, c2, c3

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu THIS object là một trong các attacker
        if (TagInList(this.tag, attackerTags))
        {
            // Và đối tượng va chạm có tag thuộc destroyable
            if (TagInList(collision.gameObject.tag, destroyableTags))
            {
                Debug.Log($"{this.tag} phá huỷ {collision.gameObject.tag}");
                Destroy(collision.gameObject);
            }
        }
    }

    // Hàm hỗ trợ kiểm tra tag có nằm trong mảng không
    private bool TagInList(string tagToCheck, string[] tagList)
    {
        foreach (string t in tagList)
        {
            if (tagToCheck == t)
                return true;
        }
        return false;
    }
}
