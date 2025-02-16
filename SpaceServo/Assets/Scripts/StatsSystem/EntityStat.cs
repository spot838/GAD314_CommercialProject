using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
[Serializable]
public class EntityStat
{
    [Tooltip("The current value of this stat")] public virtual float ValueCurrent
    {
        get
        {
            if (isDirty || ValueBase != lastBaseValue)
            {
                lastBaseValue = ValueBase;
                _value = CalculateFinalValue();
                isDirty = false;
            }
            ValueCurrentVisual = _value;
            return _value;
        }
    }
    [Tooltip("The base value of this stat")] public float ValueBase;
    // DO NOT GET OR SET THIS VALUE
    [ShowOnly] public float ValueCurrentVisual; //THIS IS A VISUAL FOR THE INSPECTOR ONLY

    protected bool isDirty = true;
    protected float _value;
    protected float lastBaseValue = float.MinValue;

    protected readonly List<StatModifier> statModifiers;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;

    //  Constructors
    public EntityStat()
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }
    public EntityStat(float baseValue) : this()
    {
        ValueBase = baseValue;
    }

    public virtual void AddModifier(StatModifier modifier)
    {
        isDirty = true;
        statModifiers.Add(modifier);
        statModifiers.Sort(CompareModifierOrder);
    }
    public virtual bool RemoveModifier(StatModifier modifier)
    {
        if (statModifiers.Remove(modifier))
        {
            isDirty = true;
            return true;
        }
        return false;
    }
    public virtual bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;
        for (int i = statModifiers.Count - 1; i >= 0; i--)
        {
            if (statModifiers[i].Source == source)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }
    protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order)
        {
            return -1;
        }
        else if (a.Order > b.Order)
        {
            return 1;
        }
        // if both are equal return 0
        return 0;
    }
    protected virtual float CalculateFinalValue()
    {
        float finalValue = ValueBase;
        float sumPercentAdd = 0;

        for (int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier modifier = statModifiers[i];
            if (modifier.Type == StatModifierType.Flat)
            {
                finalValue += modifier.Value;
            }
            else if (modifier.Type == StatModifierType.PercentAdd)
            {
                sumPercentAdd += modifier.Value;
                if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModifierType.PercentAdd)
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if (modifier.Type == StatModifierType.PercentMultiply)
            {
                finalValue *= 1 + modifier.Value;
            }
        }

        return (float)MathF.Round(finalValue, 4);
    }
}