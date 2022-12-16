namespace StreamHelper.Integrations.Twitch.Data.Exceptions;

[Serializable]
public class TokenTypeException : Exception
{
    public TokenTypeException(string expectedType, string actualType) 
        : base($"Wrong token type. Expected: {expectedType}. Actual: {actualType}")
    {
    }
}