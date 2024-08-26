using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    public delegate void onInventoryChangedEvent();
    public event onInventoryChangedEvent onInventoryChangedEventCallback;

    private Dictionary<InventoryItemData, InventoryItem> _itemDictionary;
    public List<InventoryItem> inventory;

    private void Awake()
    {
        inventory = new List<InventoryItem>();
        _itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();

        Instance = this;
    }
    public void Add(InventoryItemData itemData)
    {
        if(_itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            Debug.Log("Sumar stack en item");
            value.AddStack();

            onInventoryChangedEventCallback.Invoke();
        }
        else
        {
            Debug.Log("Agregar un nuevo item.");
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            _itemDictionary.Add(itemData, newItem);
        }
        
    }
    public void Remove(InventoryItemData itemData)
    {
        if(_itemDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            value.RemoveFromStack();

            if(value.stackSize == 0)
            {
                inventory.Remove(value);
                _itemDictionary.Remove(itemData);
            }
        }
        onInventoryChangedEventCallback.Invoke();
    }
}
