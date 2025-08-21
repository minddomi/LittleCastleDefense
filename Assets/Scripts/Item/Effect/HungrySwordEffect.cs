using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungrySwordEffect : IItemEffect
{
    public string Id { get; }
    public HungrySwordEffect(string id) { Id = id; }
    public void Apply(AllyUnit unit) { /* TODO */ }
    public void Remove(AllyUnit unit) { /* TODO */ }
}

