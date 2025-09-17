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
        Debug.Log(itemName + "�߰��� (���� ����: " + items.Count + ")");
    }

    public void RemoveItem(string itemName)
    {
        Item target = items.Find(x => x.itemName == itemName);

        if (target != null)
        {
            items.Remove(target);
            Debug.Log(itemName + "������ (���� ����: " + items.Count + ")");
        }
        else
        {
            Debug.Log(itemName + "�������� �����ϴ�");
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

        Debug.Log($"{name} ���� ���� �Ϸ� (���� ����: {items.Count})");
    }

    public void PrintInventory()
    {
        Debug.Log("=== ����Ʈ �κ��丮 ���� ===");
        if (items.Count == 0)
        {
            Debug.Log("�κ��丮�� ��� �ֽ��ϴ�");
            return;
        }

        for (int i = 0; i < items.Count; i++)
        {
            Debug.Log(i + "�� ����: " + items[i].itemName + "x" + items[i].quantity);
        }
    }
}
