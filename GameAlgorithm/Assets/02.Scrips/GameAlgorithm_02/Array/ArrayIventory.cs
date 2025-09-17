using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayIventory : MonoBehaviour
{
    public int iventorySize = 10; // �κ��丮 ĭ ����
    public Item[] items;

    private void Start()
    {
        items = new Item[iventorySize];
    }

    public void AddItem(string itemName)
    {
        // ��ĭ ã�� 
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = new Item(itemName,1);
                Debug.Log(itemName + "�߰��� (����" + i + ")");
                return;
            }
        }
        Debug.Log("�κ��丮�� ���� á���ϴ�");
    }

    public void RemoveItem(string itemName)
    {
        for (int i = 0;i < items.Length; i++)
        {
            if(items[i] != null && items[i].itemName == itemName)
            {
                Debug.Log(itemName + "������(����" + i + ")");
                items[i] = null;
                return;
            }
        }
        Debug.Log(itemName + "������ �����ϴ�");
    }

    public void PrintInventory()
    {
        Debug.Log("=== �迭 �κ��丮 ���� ====");
        for(int i = 0; i<items.Length; i++)
        {
            if (items[i] != null)
            {
                Debug.Log(i + "�� ����: " + items[i].itemName + "x" + items[i].quantity);
            }
            else
            {
                Debug.Log(i + "�� ����: �������");
            }
        }
    }
}
