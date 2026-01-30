using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeDataLoader : MonoBehaviour
{
    public static RecipeDataLoader Instance;

    public List<RecipeData> recipes = new List<RecipeData>();

    private void Awake()
    {
        Instance = this;
        LoadCSV();
    }

    void LoadCSV()
    {
        TextAsset csv = Resources.Load<TextAsset>("CSV/Recipe");
        string[] lines = csv.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)  // 0줄은 헤더
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] cols = line.Split(',');

            RecipeData r = new RecipeData();
            r.recipeID = cols[0];

            for (int u = 1; u <= 5; u++)
            {
                if (!string.IsNullOrEmpty(cols[u]))
                    r.unitIDs.Add(cols[u]);
            }

            r.reward = cols[6];
            r.used = cols[7] == "TRUE";

            recipes.Add(r);
        }
    }
}
