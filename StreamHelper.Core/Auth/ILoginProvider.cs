using Microsoft.AspNetCore.Identity;
using StreamHelper.Core.Data;

namespace StreamHelper.Core.Auth;

public interface ILoginProvider
{
    Task<UserLoginInfo?> GetLogin(User user, LoginProvider loginProvider);

    Task StoreLogin(User user, UserLoginInfo loginInfo);
}