using StreamHelper.Core.Commons;

namespace StreamHelper.Core.Data;

public record LoginProvider(string Value) : ValueBase<string>(Value)
{
    public static LoginProvider Default { get; } = new(string.Empty);
    
    public static implicit operator LoginProvider(string value) => new(value);
}