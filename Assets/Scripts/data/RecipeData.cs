using System.Collections.Generic;

[System.Serializable]
public class RecipeData
{
    public string recipeID;
    public List<string> unitIDs;  // UnitID1~5
    public string reward;
    public bool used;

    public RecipeData()
    {
        unitIDs = new List<string>();
    }
}
