namespace StreamHelper.Core.Commons;

public abstract record ValueBase<TValue>(TValue Value)
{
    public static implicit operator TValue(ValueBase<TValue> value) => value.Value;
}