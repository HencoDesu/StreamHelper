using StreamHelper.Core.Commons;

namespace StreamHelper.Core.Data;

public record ClaimType(string Value) : ValueBase<string>(Value)
{
    public static ClaimType Default { get; } = new(string.Empty);
    
    public static implicit operator ClaimType(string value) => new(value);
}