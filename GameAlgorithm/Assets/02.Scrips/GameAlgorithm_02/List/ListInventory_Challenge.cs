using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ListInventory_Challenge : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public void AddItem(string itemName)
    {
        items.Add(new Item(itemName, 1));
        Debug.Log(itemName + "추가됨 (현재 개수: " + items.Count + ")");
    }

    public void RemoveItem(string itemName)
    {
        Item target = items.Find(x => x.itemName == itemName);

        if (target != null)
        {
            items.Remove(target);
            Debug.Log(itemName + "삭제됨 (현재 개수: " + items.Count + ")");
        }
        else
        {
            Debug.Log(itemName + "아이템이 없습니다");
        }
    }

    public void RemoveAll(string itemName)
    {

        foreach(Item it in items.ToList())
        {
            if(it.itemName == itemName)
            {
                items.Remove(it);
            }
        }

        Debug.Log($"{name} 전부 삭제 완료 (현재 개수: {items.Count})");
    }

    public void PrintInventory()
    {
        Debug.Log("=== 리스트 인벤토리 상태 ===");
        if (items.Count == 0)
        {
            Debug.Log("인벤토리가 비어 있습니다");
            return;
        }

        for (int i = 0; i < items.Count; i++)
        {
            Debug.Log(i + "번 슬롯: " + items[i].itemName + "x" + items[i].quantity);
        }
    }
}
