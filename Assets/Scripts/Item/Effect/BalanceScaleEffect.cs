using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceScaleEffect : IItemEffect
{
    public string Id { get; }
    private readonly HashSet<AllyUnit> applied = new();

    public BalanceScaleEffect(string id) { Id = id; }

    public void Apply(AllyUnit u)
    {
        if (u == null || applied.Contains(u)) return;
        applied.Add(u);
        u.ignoreTypeForDamage = true; // ÄÑ±â
    }
    public void Remove(AllyUnit u)
    {
        if (u == null || !applied.Contains(u)) return;
        applied.Remove(u);
        u.ignoreTypeForDamage = false; // ²ô±â
    }
}
