using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private List<Unit> units = new List<Unit>();                   // ��ü ���� ���
    private SimplePriorityQueue<Unit> turnQueue = new SimplePriorityQueue<Unit>(); // �� �켱���� ť
    private List<Unit> firstRoundUnits = new List<Unit>();         // ù 4�� ������

    private int turnCount = 0; // ���� �� ��

    private void Start()
    {
        //  ���� ��� (�̸�, ��Ÿ��)
        units.Add(new Unit("����", 5f));
        units.Add(new Unit("������", 7f));
        units.Add(new Unit("�ü�", 10f));
        units.Add(new Unit("����", 12f));

        //  �ʱ� �켱���� ť ���
        foreach (var u in units)
            turnQueue.Enqueue(u, u.nextTurnTime);

        Debug.Log("�ʱ� ��� �Ϸ�!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ProcessTurn();
    }

    private void ProcessTurn()
    {

        //  1~4��: ���� ������ ���� (�� ���� 1ȸ��)
        if (turnCount <= 4)
        {
            Unit randomUnit = GetRandomUnit();
            turnCount++;
            randomUnit.DoTurn(turnCount);

            if (!firstRoundUnits.Contains(randomUnit))
                firstRoundUnits.Add(randomUnit);
            return;
        }

        //  5�� ����: ��Ÿ��(�ӵ�)�� ����� ����
        if (turnQueue.Count > 0)
        {
            var nextUnit = turnQueue.Dequeue();
            float now = Time.time;

            if (nextUnit.nextTurnTime <= now)
            {
                turnCount++;
                nextUnit.DoTurn(turnCount);
                nextUnit.nextTurnTime = now + nextUnit.coolTime; // ��Ÿ�� �缳��
                turnQueue.Enqueue(nextUnit, nextUnit.nextTurnTime);
            }
            else
            {
                // ���� ��Ÿ���� �� ������ ��
                turnQueue.Enqueue(nextUnit, nextUnit.nextTurnTime);
            }
        }
    }

    // ù ���� ���� ���� ���� �Լ�
    private Unit GetRandomUnit()
    {
        List<Unit> available = new List<Unit>();

        // ���� �� �� �� ���ָ� ���͸�
        foreach (var u in units)
            if (!firstRoundUnits.Contains(u))
                available.Add(u);

        // �̹� ��� �������� ��ü �� ����
        if (available.Count == 0)
            available = new List<Unit>(units);

        return available[Random.Range(0, available.Count)];
    }
}
