//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class MergeManager : MonoBehaviour
//{
//    public static MergeManager Instance;

//    public UnitSpawner spawner;

//    public MergeSlot[] mergeSlots;
//    private MergeSlot currentSelectedSlot;

//    private void Awake()
//    {
//        Instance = this;
//    }


//    public void SelectSlot(MergeSlot slot)
//    {
//        foreach (var s in mergeSlots)
//        {
//            s.isSelected = false;
//            s.SetHighlight(false);
//        }

//        currentSelectedSlot = slot;
//        slot.isSelected = true;
//        slot.SetHighlight(true);
//    }


//    public void TryAssignUnit(UnitStatus unit)
//    {
//        if (!unit.canMerge)
//        {
//            return;
//        }

//        if (currentSelectedSlot != null && currentSelectedSlot.isSelected)
//        {
//            currentSelectedSlot.SetUnit(unit);   // UnitStatus 통째로 전달
//            //currentSelectedSlot.SetUnitPrefab(unit.prefabPath);
//            unit.canMerge = false;

//            currentSelectedSlot.SetHighlight(false);
//            currentSelectedSlot.isSelected = false;
//            currentSelectedSlot = null;
//        }
//    }

//    public JokerType CheckJokerResult()
//    {
//        // 슬롯에서 들어있는 유닛 모으기
//        List<UnitStatus> units = new List<UnitStatus>();
//        foreach (var slot in mergeSlots)
//            if (slot.assignedUnit != null)
//                units.Add(slot.assignedUnit);

//        int count = units.Count;


//        if (count == 4)
//        {
//            // 모두 같은 등급인지?
//            UnitGrade g0 = units[0].unitGrade;
//            bool sameGrade = true;
//            foreach (var u in units)
//                if (u.unitGrade != g0)
//                    sameGrade = false;

//            // 종족 4종인지
//            HashSet<UnitClass> raceSet = new HashSet<UnitClass>();
//            foreach (var u in units)
//                raceSet.Add(u.unitClass);

//            bool fourDifferentRaces = raceSet.Count == 4;

//            //  조건 만족 시 조커 생성
//            if (sameGrade && fourDifferentRaces)
//            {
//                // Basic~Advanced  흑백
//                if (g0 == UnitGrade.Basic ||
//                    g0 == UnitGrade.Intermediate ||
//                    g0 == UnitGrade.Advanced)
//                    return JokerType.BlackWhite;

//                // Epic~Supreme  컬러
//                return JokerType.Color;
//            }

//            return JokerType.None;
//        }


//        if (count == 5)
//        {
//            //  같은 종족인지?
//            bool sameRace = true;
//            UnitClass r0 = units[0].unitClass;

//            foreach (var u in units)
//                if (u.unitClass != r0)
//                    sameRace = false;

//            if (sameRace)
//            {
//                // 등급 리스트 정렬
//                List<int> grades = new List<int>();
//                foreach (var u in units)
//                    grades.Add((int)u.unitGrade);
//                grades.Sort();

//                // 등급이 0,1,2,3,4 식으로 연속인지
//                bool sequential = true;
//                for (int i = 1; i < grades.Count; i++)
//                    if (grades[i] != grades[i - 1] + 1)
//                        sequential = false;

//                if (sequential)
//                    return JokerType.Color;
//            }

//            // B) 같은 등급 + 같은 종족 5개 → 흑백
//            bool sameGrade5 = true;
//            UnitGrade g05 = units[0].unitGrade;

//            foreach (var u in units)
//                if (u.unitGrade != g05)
//                    sameGrade5 = false;

//            bool sameRace5 = true;
//            foreach (var u in units)
//                if (u.unitClass != units[0].unitClass)
//                    sameRace5 = false;

//            if (sameGrade5 && sameRace5)
//                return JokerType.BlackWhite;

//            return JokerType.None;
//        }


//        return JokerType.None;
//    }


//    public void OnClickMergeButton()
//    {
//        JokerType result = CheckJokerResult();

//        if (result == JokerType.BlackWhite)
//        {
//            Debug.Log("흑백 조커 생성");
//            spawner.SpawnUnit("Joker1");
//        }
//        else if (result == JokerType.Color)
//        {
//            Debug.Log("컬러 조커 생성");
//            spawner.SpawnUnit("Joker2");
//        }
//        else
//        {
//            Debug.Log("조커 없음!");
//        }

//        ResetAllSlots();
//    }

//    public void ResetAllSlots()
//    {
//        foreach (var slot in mergeSlots)
//        {
//            if (slot.assignedUnit != null)
//            {
//                slot.assignedUnit.canMerge = true;
//            }

//            slot.Clear();
//            slot.SetHighlight(false);
//        }
//        currentSelectedSlot = null;
//    }
//}

