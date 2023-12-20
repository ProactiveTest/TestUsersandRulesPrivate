using Microsoft.AspNetCore.Components;
using HealthyTips.Application.Models;

namespace HealthyTips.Pages.Shared;

public partial class NavMenu
{
    [CascadingParameter]
    public MainLayout? Layout { get; set; }
    private User? user => Layout?.User;
}
