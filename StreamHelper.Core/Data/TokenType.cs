using StreamHelper.Core.Commons;

namespace StreamHelper.Core.Data;

public record TokenType(string Value) : ValueBase<string>(Value)
{
    public static TokenType Default { get; } = new(string.Empty);
    
    public static implicit operator TokenType(string value) => new(value);
}