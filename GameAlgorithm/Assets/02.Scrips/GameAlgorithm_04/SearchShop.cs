using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchShop : MonoBehaviour
{
    public GameObject itemPrefab;      // ������ ��ư ������
    public Transform contentPanel;     // ScrollView Content
    public InputField searchInput;     // �˻� �Է�â
    public Button linearButton;        // ���� Ž�� ��ư
    public Button binaryButton;        // ���� Ž�� ��ư

    private List<GameObject> itemObjects = new List<GameObject>();
    private List<string> itemNames = new List<string>();

    void Start()
    {
        // 100�� ������ ����
        for (int i = 0; i < 100; i++)
        {
            string itemName = "Item_" + i.ToString("D2");
            itemNames.Add(itemName);

            GameObject newItem = Instantiate(itemPrefab, contentPanel);
            newItem.GetComponentInChildren<Text>().text = itemName;
            itemObjects.Add(newItem);
        }

        // ��ư ����
        linearButton.onClick.AddListener(() => LinearSearch(searchInput.text));
        binaryButton.onClick.AddListener(() => BinarySearch(searchInput.text));

        SetupContentLayout();
    }

    // ��� ������ �����
    private void HideAllItems()
    {
        foreach (var item in itemObjects)
        {
            item.SetActive(false);
        }
    }

    // ���� Ž��
    private void LinearSearch(string target)
    {
        HideAllItems();
        for (int i = 0; i < itemNames.Count; i++)
        {
            if (itemNames[i] == target)
            {
                itemObjects[i].SetActive(true);
                return;
            }
        }
        Debug.Log("������ ���� (Linear Search)");
    }

    // ���� Ž��
    private void BinarySearch(string target)
    {
        HideAllItems();

        int left = 0;
        int right = itemNames.Count - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            int compare = string.Compare(itemNames[mid], target);

            if (compare == 0)
            {
                itemObjects[mid].SetActive(true);
                return;
            }
            else if (compare < 0)
                left = mid + 1;
            else
                right = mid - 1;
        }

        Debug.Log("������ ���� (Binary Search)");
    }

    void SetupContentLayout()
    {
        if (contentPanel == null) return;

        var layout = contentPanel.GetComponent<VerticalLayoutGroup>();
        if (layout == null) layout = contentPanel.gameObject.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 10f;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;

        var fitter = contentPanel.GetComponent<ContentSizeFitter>();
        if (fitter == null) fitter = contentPanel.gameObject.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }
}