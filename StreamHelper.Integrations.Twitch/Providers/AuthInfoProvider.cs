using JetBrains.Annotations;
using StreamHelper.Core.Auth;
using StreamHelper.Core.Extensions;
using StreamHelper.Core.Providers;
using StreamHelper.Integrations.Twitch.Data;
using StreamHelper.Integrations.Twitch.Extensions;
using TwitchLib.Api.Interfaces;

namespace StreamHelper.Integrations.Twitch.Providers;

[UsedImplicitly]
public class AuthInfoProvider
    : ProviderBase<AuthInfo>
{
    private readonly ITwitchAPI _tokenRefresher;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILoginProvider _loginProvider;
    private readonly IClaimsProvider _claimsProvider;

    public AuthInfoProvider(
        ITwitchAPI tokenRefresher,
        ITokenProvider tokenProvider,
        ILoginProvider loginProvider, 
        IClaimsProvider claimsProvider) 
    {
        _tokenRefresher = tokenRefresher;
        _tokenProvider = tokenProvider;
        _loginProvider = loginProvider;
        _claimsProvider = claimsProvider;
    }

    public override async Task<AuthInfo> Get(User user)
    {
        var authInfo = await base.Get(user);
        if (authInfo.AccessToken == AccessToken.Default || authInfo.AccessTokenExpired)
        {
            authInfo = await UpdateAuthInfo(authInfo);
        }

        return authInfo;
    }

    protected override async Task<AuthInfo> CreateNew(User user)
    {
        var accessToken = await _tokenProvider.GetToken(user, Constants.LoginProvider, Constants.AccessToken);
        var refreshToken = await _tokenProvider.GetToken(user, Constants.LoginProvider, Constants.RefreshToken);
        var expiresAtToken = await _tokenProvider.GetToken(user, Constants.LoginProvider, Constants.ExpiresAt);
        var userLoginInfo = await _loginProvider.GetLogin(user, Constants.LoginProvider);
        var displayName = await _claimsProvider.GetClaim(user, Constants.DisplayName);
        var profileImage = await _claimsProvider.GetClaim(user, Constants.ProfileImageUrl);

        return new AuthInfo
        {
            AccessToken = accessToken ?? AccessToken.Default,
            RefreshToken = refreshToken ?? RefreshToken.Default,
            ExpiresAt = expiresAtToken is not null ? DateTime.Parse(expiresAtToken) : null,
            UserId = userLoginInfo?.ProviderKey ?? UserId.Default,
            DisplayName = displayName?.Value ?? string.Empty,
            ProfileImage = profileImage?.Value ?? string.Empty
        };
    }

    private async Task<AuthInfo> UpdateAuthInfo(AuthInfo oldAuthInfo)
    {
        var twitchResponse = await _tokenRefresher.RefreshAccessToken(oldAuthInfo.RefreshToken);

        return oldAuthInfo with
        {
            AccessToken = twitchResponse.AccessToken,
            RefreshToken = twitchResponse.RefreshToken,
            ExpiresAt = DateTime.UtcNow + twitchResponse.ExpiresIn.Seconds()
        };
    }
}