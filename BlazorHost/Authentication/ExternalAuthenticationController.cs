using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreamHelper.Core.Auth;

namespace BlazorHost.Authentication;

[Route("ExternalAuth")]
public class ExternalAuthenticationController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<ExternalAuthenticationController> _logger;

    public ExternalAuthenticationController(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        ILogger<ExternalAuthenticationController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }
    
    [HttpGet("{providerName}")]
    public IActionResult Index(string providerName)
    {
        var redirectUrl = Url.Action("OnGetCallback", "ExternalAuthentication");
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(providerName, redirectUrl);
        return new ChallengeResult(providerName, properties);
    }

    [HttpGet("LogOut")]
    public async Task<IActionResult> LogOut(string? returnUrl = null)
    {
        await _signInManager.SignOutAsync();
        return Redirect("~/");
    }

    public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
    {
        returnUrl ??= Url.Content("~/");
        if (remoteError is not null)
        {
            return LocalRedirect(returnUrl);
        }

        var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
        if (externalLoginInfo?.Principal.Identity is null)
        {
            return LocalRedirect(returnUrl);
        }

        var user = await _userManager.FindByLoginAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey);

        if (user is not null)
        {
            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                externalLoginInfo.LoginProvider, 
                externalLoginInfo.ProviderKey, 
                isPersistent: false, 
                bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider", 
                    externalLoginInfo.Principal.Identity.Name, 
                    externalLoginInfo.LoginProvider);

                await UpdateTokens(user, externalLoginInfo);
            }
        }
        else
        {
            user = new User();
            
            await _userManager.SetUserNameAsync(user, externalLoginInfo.Principal.Identity!.Name);
            await _userManager.CreateAsync(user);
            await _userManager.AddLoginAsync(user, externalLoginInfo);
            await _userManager.AddClaimsAsync(user, externalLoginInfo.Principal.Claims);
            await _userManager.GetUserIdAsync(user);
            await UpdateTokens(user, externalLoginInfo);

            await _signInManager.SignInAsync(user, isPersistent: false, externalLoginInfo.LoginProvider);
        }
        
        return LocalRedirect(returnUrl);

    }

    private async Task UpdateTokens(User user, ExternalLoginInfo externalLoginInfo)
    {
        if (externalLoginInfo.AuthenticationTokens is null)
        {
            return;
        }
        
        foreach (var token in externalLoginInfo.AuthenticationTokens)
        {
            await _userManager.SetAuthenticationTokenAsync(
                user, 
                externalLoginInfo.LoginProvider, 
                token.Name,
                token.Value);
        }
    }
}