using UnityEngine;

public class Unit
{
    public string name;
    public float coolTime;
    public float nextTurnTime;

    public Unit(string name, float coolTime)
    {
        this.name = name;
        this.coolTime = coolTime;
        this.nextTurnTime = Time.time + coolTime;
    }

    public void DoTurn(int turn)
    {
        Debug.Log($"{turn}�� / {name} �� ���Դϴ�!");
        nextTurnTime = Time.time + coolTime;
    }
}
