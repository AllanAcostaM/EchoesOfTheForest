[System.Serializable]
public class InventoryItem // me permite agrupar los item para evitar varias item de la mismo genero en el inventory
{
    public InventoryItemData data;
    public int stackSize; 

    public InventoryItem(InventoryItemData itemData)
    {
        data = itemData;
        AddStack();
    }

    public void AddStack()
    {
        stackSize++;
    }
    public void RemoveFromStack()
    {
        stackSize--;
    }
}

