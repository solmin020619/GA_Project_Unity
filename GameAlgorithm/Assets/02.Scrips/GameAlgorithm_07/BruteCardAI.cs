using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BruteCardAI : MonoBehaviour
{
    [Header("UI")]
    public Text resultText;         
    // public TMP_Text resultText;  

    private int maxCost = 15;

    // 카드 정보 고정값
    // 퀵샷 x2 (6딜, 2코스트)
    // 헤비샷 x2 (8딜, 3코스트)
    // 멀티샷 x1 (16딜, 5코스트)
    // 트리플샷 x1 (24딜, 7코스트)

    public void OnClick_FindBestCombo()
    {
        int bestDamage = 0;
        string bestComboText = "";

        // 각 카드별로 “최대 몇 장까지 쓸 수 있나”를 기준으로 for문
        for (int quick = 0; quick <= 2; quick++)       // 퀵샷 0~2장
        {
            for (int heavy = 0; heavy <= 2; heavy++)   // 헤비샷 0~2장
            {
                for (int multi = 0; multi <= 1; multi++)   // 멀티샷 0~1장
                {
                    for (int triple = 0; triple <= 1; triple++) // 트리플샷 0~1장
                    {
                        // 코스트/데미지 계산
                        int totalCost =
                            quick * 2 +     // 퀵샷 코스트 2
                            heavy * 3 +     // 헤비샷 코스트 3
                            multi * 5 +     // 멀티샷 코스트 5
                            triple * 7;     // 트리플샷 코스트 7

                        // 코스트 넘으면 이 조합은 버림
                        if (totalCost > maxCost)
                            continue;

                        int totalDamage =
                            quick * 6 +     // 퀵샷 데미지 6
                            heavy * 8 +     // 헤비샷 데미지 8
                            multi * 16 +    // 멀티샷 데미지 16
                            triple * 24;    // 트리플샷 데미지 24

                        // 지금까지 중에 제일 쎄면 갱신
                        if (totalDamage > bestDamage)
                        {
                            bestDamage = totalDamage;
                            bestComboText =
                                $"퀵샷 x {quick}, 헤비샷 x {heavy}, 멀티샷 x {multi}, 트리플샷 x {triple}\n" +
                                $"코스트: {totalCost}";
                        }
                    }
                }
            }
        }

        // 결과 출력
        if (resultText != null)
        {
            resultText.text =
                $"[AI 최적 콤보]\n{bestComboText}\n총 데미지: {bestDamage}";
        }
        else
        {
            Debug.Log($"[AI 최적 콤보]\n{bestComboText}\n총 데미지: {bestDamage}");
        }
    }
}
