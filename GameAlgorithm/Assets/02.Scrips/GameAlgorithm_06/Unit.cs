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
        Debug.Log($"{turn}턴 / {name} 의 턴입니다!");
        nextTurnTime = Time.time + coolTime;
    }
}
