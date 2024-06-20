using System;
using MudBlazor;

namespace MagicT.Client.Models;

public class NotificationVM
{
    public NotificationVM(string message, Severity severity)
    {
        Message = message;
        Severity = severity;
    }

    public string Message { get; }

    public Severity Severity { get; }
}

 