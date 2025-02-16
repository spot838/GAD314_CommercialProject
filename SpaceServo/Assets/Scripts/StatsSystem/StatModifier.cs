using System;

public enum StatModifierType
{
    Flat = 100,
    PercentAdd = 200,
    PercentMultiply = 300,
}

[Serializable]
public class StatModifier
{
    public readonly float Value;
    public readonly StatModifierType Type;
    public readonly int Order;
    public readonly object Source;

    public StatModifier(float value, StatModifierType type, int order, object source)
    {
        Value = value;
        Type = type;
        Order = order;
        Source = source;
    }
    
    /// <summary>
    /// StatModifier Value and Type are REQUIRED, Order OR Source are OPTIONAL
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    public StatModifier(float value, StatModifierType type) : this (value, type, (int)type, null) { }
    /// <summary>
    /// StatModifier Order indicates the sorting order at which the modifier is calculated with the value
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    /// <param name="order"></param>
    public StatModifier(float value, StatModifierType type, int order) : this (value, type, order, null) { }
    /// <summary>
    /// StatModifier Source is the StatModifier's source object, using the object type
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    /// <param name="source"></param>
    public StatModifier(float value, StatModifierType type, object source) : this (value, type, (int)type, source) { }
}