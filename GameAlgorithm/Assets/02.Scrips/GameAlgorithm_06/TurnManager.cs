using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private List<Unit> units = new List<Unit>();                   // 전체 유닛 목록
    private SimplePriorityQueue<Unit> turnQueue = new SimplePriorityQueue<Unit>(); // 턴 우선순위 큐
    private List<Unit> firstRoundUnits = new List<Unit>();         // 첫 4턴 추적용

    private int turnCount = 0; // 현재 턴 수

    private void Start()
    {
        //  유닛 등록 (이름, 쿨타임)
        units.Add(new Unit("전사", 5f));
        units.Add(new Unit("마법사", 7f));
        units.Add(new Unit("궁수", 10f));
        units.Add(new Unit("도적", 12f));

        //  초기 우선순위 큐 등록
        foreach (var u in units)
            turnQueue.Enqueue(u, u.nextTurnTime);

        Debug.Log("초기 등록 완료!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ProcessTurn();
    }

    private void ProcessTurn()
    {

        //  1~4턴: 랜덤 순서로 실행 (각 유닛 1회씩)
        if (turnCount <= 4)
        {
            Unit randomUnit = GetRandomUnit();
            turnCount++;
            randomUnit.DoTurn(turnCount);

            if (!firstRoundUnits.Contains(randomUnit))
                firstRoundUnits.Add(randomUnit);
            return;
        }

        //  5턴 이후: 쿨타임(속도)에 비례한 순서
        if (turnQueue.Count > 0)
        {
            var nextUnit = turnQueue.Dequeue();
            float now = Time.time;

            if (nextUnit.nextTurnTime <= now)
            {
                turnCount++;
                nextUnit.DoTurn(turnCount);
                nextUnit.nextTurnTime = now + nextUnit.coolTime; // 쿨타임 재설정
                turnQueue.Enqueue(nextUnit, nextUnit.nextTurnTime);
            }
            else
            {
                // 아직 쿨타임이 안 돌았을 때
                turnQueue.Enqueue(nextUnit, nextUnit.nextTurnTime);
            }
        }
    }

    // 첫 라운드 랜덤 유닛 선택 함수
    private Unit GetRandomUnit()
    {
        List<Unit> available = new List<Unit>();

        // 아직 턴 안 한 유닛만 필터링
        foreach (var u in units)
            if (!firstRoundUnits.Contains(u))
                available.Add(u);

        // 이미 모두 돌았으면 전체 중 랜덤
        if (available.Count == 0)
            available = new List<Unit>(units);

        return available[Random.Range(0, available.Count)];
    }
}
