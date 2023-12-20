using Microsoft.AspNetCore.Components;
using HealthyTips.Application.Models;
using HealthyTips.Pages.Shared;

namespace HealthyTips.Pages;

public partial class Dashboard
{
    [CascadingParameter]
    public MainLayout? Layout { get; set; }
    private User? user => Layout.User;
}
