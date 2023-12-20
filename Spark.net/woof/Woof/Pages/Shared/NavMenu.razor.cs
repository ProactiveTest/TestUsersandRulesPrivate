using Microsoft.AspNetCore.Components;
using Woof.Application.Models;

namespace Woof.Pages.Shared;

public partial class NavMenu
{
    [CascadingParameter]
    public MainLayout? Layout { get; set; }
    private User? user => Layout?.User;
}
