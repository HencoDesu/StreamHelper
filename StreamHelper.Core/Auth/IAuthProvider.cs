namespace StreamHelper.Core.Auth;

public interface IAuthProvider
{
    Task<User?> GetCurrentUser();
}