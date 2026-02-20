using UnityEngine;

public abstract class TutorialCondition : MonoBehaviour
{
    public abstract void Begin();
    public abstract void End();
    public abstract bool IsMet { get; }
}
