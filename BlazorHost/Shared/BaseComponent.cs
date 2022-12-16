using Microsoft.AspNetCore.Components;
using StreamHelper.Core.Auth;

namespace BlazorHost.Shared;

public abstract class BaseComponent : LayoutComponentBase
{
    [Inject] protected IAuthProvider AuthProvider { get; set; } = null!;

    protected User? CurrentUser { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        CurrentUser = await AuthProvider.GetCurrentUser();
    }

    protected Task Redraw()
        => InvokeAsync(StateHasChanged);
}