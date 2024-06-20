using Generator.Components.Args;
using Generator.Components.Components;
using Generator.Components.Enums;
using Generator.Components.Interfaces;
using MagicT.Client.Pages.Base;
using MagicT.Shared.Models;
using MagicT.Shared.Services;

namespace MagicT.Client.Pages.Users;

public partial class RolesPage : ServicePage<ROLES, IRolesService>
{
    private GenTextField AB_NAME;
 
    protected override async Task OnBeforeInitializeAsync()
    {
        await ReadAsync(default);
        await base.OnBeforeInitializeAsync();
    }
    protected override Task LoadAsync(IGenView<ROLES> view)
    {
        if (view.ViewState != ViewState.Create)
            AB_NAME.Disabled = true;

        //Service.WithCancellationToken

        return base.LoadAsync(view);
 
    }
}