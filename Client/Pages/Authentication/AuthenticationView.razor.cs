using System;
using DocumentFormat.OpenXml.Wordprocessing;
using MagicT.Client.Managers;
using MagicT.Shared.Models.ViewModels;
using Microsoft.AspNetCore.Components;

namespace MagicT.Client.Pages.Authentication;

public partial class AuthenticationView
{
    [Inject]
    public NavigationManager NavigationManager { get; set; }


    [CascadingParameter(Name = nameof(LoginManager))]
    public LoginManager LoginManager { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public RenderFragment Authenticated { get; set; }

    [Parameter]
    public RenderFragment NotAuthenticated { get; set; }


}
