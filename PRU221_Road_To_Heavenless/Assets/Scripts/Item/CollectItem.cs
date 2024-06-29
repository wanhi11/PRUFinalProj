using UnityEngine;

public class CollectItem : MonoBehaviour
{
    [SerializeField] private int value;
    private bool hasTriggered;

    private ItemManager itemManager;

    private void Start()
    {
        itemManager = ItemManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            itemManager.ChangeItems(value); // Corrected line
            Destroy(gameObject);
        }
    }
}