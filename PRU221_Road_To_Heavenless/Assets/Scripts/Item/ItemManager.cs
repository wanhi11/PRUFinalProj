using UnityEngine;
using TMPro;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    private int items;
    [SerializeField] private TMP_Text itemsDisplay;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    private void OnGUI()
    {
        itemsDisplay.text = SceneController.Point.ToString();
    }

    public void ChangeItems(int amount)
    {
        SceneController.Point += amount;
        Debug.Log($"Item count: {SceneController.Point}");
    }

    public int GetTotalItems()
    {
        return SceneController.Point;
    }

    public void ResetItems()
    {
        SceneController.Point = 0;
        UpdateItemsDisplay();
    }

    private void UpdateItemsDisplay()
    {
        if (itemsDisplay != null)
        {
            itemsDisplay.text = SceneController.Point.ToString();
        }
    }
}

