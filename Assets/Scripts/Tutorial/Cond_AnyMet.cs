using UnityEngine;

public class Cond_AnyMet : TutorialCondition
{
    [Header("Any of these conditions met -> pass")]
    [SerializeField] private TutorialCondition[] conditions;

    public override bool IsMet
    {
        get
        {
            if (conditions == null || conditions.Length == 0) return false;

            foreach (var c in conditions)
            {
                if (c != null && c.IsMet)
                    return true; //  하나라도 true면 통과
            }
            return false;
        }
    }

    public override void Begin()
    {
        if (conditions == null || conditions.Length == 0)
        {
            Debug.LogError("[Tutorial] Cond_AnyMet: conditions가 비어있음");
            return;
        }

        foreach (var c in conditions)
            if (c != null) c.Begin();
    }

    public override void End()
    {
        if (conditions == null) return;

        foreach (var c in conditions)
            if (c != null) c.End();
    }
}