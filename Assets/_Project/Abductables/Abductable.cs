using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Abducatable : MonoBehaviour
{
    [SerializeField] private UnityEvent<AbductionState> OnAbductionStateChanged;
    public Abductor Abductor { get; private set; }
    private void OnEnable()
    {
        AbductionController.Add(this);
    }

    private void OnDisable()
    {
        AbductionController.Remove(this);
    }

    public void StartAbduction(Abductor abductor)
    {
        Abductor = abductor;
        OnAbductionStateChanged?.Invoke(AbductionState.Start);
    }

    [Button("Abduct")]
    public void CompleteAbduction()
    {
        OnAbductionStateChanged?.Invoke(AbductionState.Complete);
        Abductor = null;
    }
    public void FailAbduction()
    {
        OnAbductionStateChanged?.Invoke(AbductionState.Fail);
        AbductionController.Return(this);
        Abductor = null;
    }
}

public enum AbductionState
{
    None,
    Start,
    Complete,
    Fail
}

public static class AbductionController
{
    private static List<AbductionContainer> contents = new();

    public static void Add(Abducatable abducatable)
    {
        contents.Add(new AbductionContainer(abducatable));
    }

    public static void Remove(Abducatable abducatable)
    {
        contents.RemoveAt(GetIndexOf(abducatable));
    }

    private static int GetIndexOf(Abducatable abducatable)
    {
        return contents.FindIndex((x) => x.Matches(abducatable));
    }

    public static bool TryGetFreeAbductable(out Abducatable abducatable)
    {
        var newAbductable = GetFreeRandomAbductable();
        if (newAbductable == null)
        {
            abducatable = null;
            return false;
        }
        
        abducatable = newAbductable;
        return true;
    }

    private static Abducatable GetFreeRandomAbductable()
    {
        AbductionContainer container = contents.Where((x) => x.CanBeAbducted()).OrderBy(x => UnityEngine.Random.value).FirstOrDefault();
        if (container == null) return null;
        container.BecomeTargeted();
        return container.Abducatable;
    }

    public static void Return(Abducatable abducatable)
    {
        contents[GetIndexOf(abducatable)].Free();
    }
    
}

public class AbductionContainer
{
    public Abducatable Abducatable {get; private set;}
    private bool isBeingAbducted;
    public AbductionContainer(Abducatable _abducatable)
    {
        Abducatable = _abducatable;
        isBeingAbducted = false;
    }

    public bool Matches(Abducatable _abducatable)
    {
        return Abducatable == _abducatable;
    }
    public bool CanBeAbducted() { return !isBeingAbducted; }

    public void Free()
    {
        isBeingAbducted = false;
    }

    public void BecomeTargeted()
    {
        isBeingAbducted = true;
    }
}