using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public string GetRewardFromUnits(List<string> unitIDs)
    {
        // 정렬하여 순서 무시하고 비교
        unitIDs.Sort();

        foreach (var recipe in RecipeDataLoader.Instance.recipes)
        {
            if (recipe.used) continue;
            if (recipe.unitIDs.Count != unitIDs.Count) continue;

            List<string> rUnits = new List<string>(recipe.unitIDs);
            rUnits.Sort();

            bool same = true;
            for (int i = 0; i < unitIDs.Count; i++)
            {
                if (unitIDs[i] != rUnits[i])
                {
                    same = false;
                    break;
                }
            }

            if (same)
            {
                recipe.used = true; // 다시 사용 금지
                SaveUsed(recipe.recipeID);

                return recipe.reward; // Joker1 or Joker2
            }
        }

        return ""; // 아무 것도 없음
    }

    void SaveUsed(string recipeID)
    {
        PlayerPrefs.SetString("RECIPE_USED_" + recipeID, "TRUE");
    }
}
