using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneInventory : MonoBehaviour
{
    public int maxStones = 10;
    public int currentStoneCount = 0;

    public bool isEquipped = false;

    // UI o texto en pantalla para mostrar la cantidad de piedras (opcional)
    public UnityEngine.UI.Text stoneCountText;

    void Update()
    {
        // Mostrar la cantidad de piedras en el inventario
        if (stoneCountText != null)
        {
            stoneCountText.text = "Stones: " + currentStoneCount;
        }

        // Equipar piedra con la tecla 1
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentStoneCount > 0)
        {
            EquipStone();
        }
    }

    public void AddStone()
    {
        if (currentStoneCount < maxStones)
        {
            currentStoneCount++;
        }
        else
        {
            Debug.Log("Inventory is full!");
        }
    }

    public void EquipStone()
    {
        isEquipped = true;
        Debug.Log("Stone equipped!");
    }

    public void RemoveStone()
    {
        if (currentStoneCount > 0)
        {
            currentStoneCount--;
        }
    }
}