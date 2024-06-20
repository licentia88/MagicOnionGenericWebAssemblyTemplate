using MagicT.Client.Managers;
using MagicT.Shared.Models.ViewModels;
using MagicT.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace MagicT.Client.Pages.Users;

public partial class Login
{
    

    public LoginRequest LoginRequest { get; set; } = new();

    [CascadingParameter(Name =nameof(LoginManager))]
     public LoginManager LoginManager { get; set; }

    [Inject]
    IAuthenticationService Service { get; set; }

  
    public async Task LoginAsync()
    {
        await ExecuteAsync(async () =>
        {
            var result = await Service.LoginWithUsername(LoginRequest);

            await LoginManager.SignInAsync(LoginRequest);

            LoginManager.LoginData = LoginRequest;
            await LoginManager.TokenRefreshSubscriber(LoginRequest);

            NavigationManager.NavigateTo("/");
        });

    }

    protected override Task OnBeforeInitializeAsync()
    {
        NavigationManager.NavigateTo("/");
        return base.OnBeforeInitializeAsync();
    }
}
