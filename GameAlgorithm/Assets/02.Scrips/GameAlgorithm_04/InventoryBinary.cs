using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBinary : MonoBehaviour
{
    List<Item> items = new List<Item>();

    private void Start()
    {
        // �κ��丮 �ʱ� ������ �߰�
        items.Add(new Item("Potion",5));
        items.Add(new Item("High-Potion",2));
        items.Add(new Item("Elixir",1));
        items.Add(new Item("Sword",1));

        // Ž�� �� ����(�̸� ����)
        items.Sort((a, b) => a.itemName.CompareTo(b.itemName));

        // Ž�� �׽�Ʈ
        Item found = FindItem("Elixir");
        if (found != null)
            Debug.Log($"[���� Ž��] ã�� ������ : {found.itemName}, ���� {found.quantity}");
        else
            Debug.Log("[���� Ž��] �������� ã�� �� �����ϴ�");
    }

    public Item FindItem(string targetName)
    {
        int left = 0;
        int right = items.Count - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            int compare = items[mid].itemName.CompareTo(targetName);

            if(compare == 0)
            {
                return items[mid];  // ã��
            }
            else if ((compare < 0))
            {
                left = mid + 1;     // ������ Ž��
            }
            else
            {
                right = mid - 1;    // ���� Ž��
            }
        }
        return null;
    }
}
