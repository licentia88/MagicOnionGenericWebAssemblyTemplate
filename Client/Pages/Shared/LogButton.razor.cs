using Microsoft.AspNetCore.Components;

namespace MagicT.Client.Pages.Shared;

public partial class LogButton
{
    [Parameter,EditorRequired]
    public EventCallback OnClick { get; set; }
}