using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemEffect
{
    string Id { get; }              // 아이템 고유 ID (예: "InfiniteBranch")
    void Apply(AllyUnit unit);      // 장착될 때 1회 실행
    void Remove(AllyUnit unit);     // 해제될 때 1회 실행
}
