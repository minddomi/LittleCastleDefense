using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager Instance;

    public UnitSpawner spawner;

    public MergeSlot[] mergeSlots;
    private MergeSlot currentSelectedSlot;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectSlot(MergeSlot slot)
    {
        foreach (var s in mergeSlots)
        {
            s.isSelected = false;
            s.SetHighlight(false);
        }

        currentSelectedSlot = slot;
        slot.isSelected = true;
        slot.SetHighlight(true);
    }

    public void TryAssignUnit(UnitStatus unit)
    {
        if (!unit.canMerge)
            return;

        if (currentSelectedSlot != null && currentSelectedSlot.isSelected)
        {
            currentSelectedSlot.SetUnit(unit);
            unit.canMerge = false;

            currentSelectedSlot.SetHighlight(false);
            currentSelectedSlot.isSelected = false;
            currentSelectedSlot = null;
        }
    }

    // CSV 레시피 체크
    public RecipeData CheckRecipeMatch()
    {
        // 슬롯에 들어있는 실제 유닛 ID 모으기
        List<string> inputUnitIDs = new List<string>();
        foreach (var slot in mergeSlots)
            if (slot.assignedUnit != null)
                inputUnitIDs.Add(slot.assignedUnit.unitID);

        inputUnitIDs.Sort(); // 정렬하여 순서 상관없이 매칭

        // 모든 레시피 탐색
        foreach (var recipe in RecipeDataLoader.Instance.recipes)
        {
            if (recipe.used)
                continue; // 이미 사용한 레시피는 제외

            // 레시피 유닛 목록
            List<string> recipeList = new List<string>(recipe.unitIDs);
            recipeList.Sort();

            // 개수 다르면 스킵
            if (recipeList.Count != inputUnitIDs.Count)
                continue;

            // 완전 동일 매칭
            bool match = true;
            for (int i = 0; i < recipeList.Count; i++)
            {
                if (recipeList[i] != inputUnitIDs[i])
                {
                    match = false;
                    break;
                }
            }

            if (match)
                return recipe; // 매칭된 레시피 반환
        }

        return null; // 매칭 없음
    }

    public void OnClickMergeButton()
    {
        RecipeData result = CheckRecipeMatch();

        if (result == null)
        {
            Debug.Log("레시피 없음!");
            ResetAllSlots();
            return;
        }

        // 보상 적용
        Debug.Log("레시피 성공! → Reward: " + result.reward);

        if (result.reward == "BlackWhite")
            spawner.SpawnUnit("Joker1");
        else if (result.reward == "Color")
            spawner.SpawnUnit("Joker2");
        else
            spawner.SpawnUnit(result.reward);

        // 사용한 레시피는 다시 못 쓰게
        result.used = true;

        ResetAllSlots();
    }

    public void ResetAllSlots()
    {
        foreach (var slot in mergeSlots)
        {
            if (slot.assignedUnit != null)
                slot.assignedUnit.canMerge = true;

            slot.Clear();
            slot.SetHighlight(false);
        }

        currentSelectedSlot = null;
    }
}
