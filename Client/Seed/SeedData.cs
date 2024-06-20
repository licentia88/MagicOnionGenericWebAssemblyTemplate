using System;
using System.Collections.Generic;
using MagicT.Shared.Models;
using MagicT.Shared.Models.Base;
using MagicT.Shared.Models.ViewModels;
using MagicT.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MagicT.Client.Seed;

[RegisterScoped]
public class SeedData
{
    // Lazy-loaded collections
    private Lazy<List<AUTHORIZATIONS_BASE>> AUTHORIZATIONS_BASE { get; set; }
    private Lazy<List<USERS>> USERS { get; set; }
    private Lazy<List<Operations>> Operations { get; set; }

    // Service for initializing data
    private IInitializerService InitializerService { get; set; }

    // Constructor to inject dependencies
    public SeedData(IServiceProvider provider)
    {
        AUTHORIZATIONS_BASE = provider.GetService<Lazy<List<AUTHORIZATIONS_BASE>>>();
        USERS = provider.GetService<Lazy<List<USERS>>>();
        Operations = provider.GetService<Lazy<List<Operations>>>();
        InitializerService = provider.GetService<IInitializerService>();
    }

    // Method to initialize data
    public async Task InitializeAsync()
    {
        // Get data from the initialization service
        var users = await InitializerService.GetUsers();
        var roles = await InitializerService.GetRoles();
        var permissions = await InitializerService.GetPermissions();
        var operations = await InitializerService.GetOperations();

        // Add data to the lazy-loaded collections
        USERS.Value.AddRange(users);
        AUTHORIZATIONS_BASE.Value.AddRange(roles);
        AUTHORIZATIONS_BASE.Value.AddRange(permissions);
        Operations.Value.AddRange(operations);
    }
}