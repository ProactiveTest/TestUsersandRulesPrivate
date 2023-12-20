using Microsoft.AspNetCore.Components;
using Woof.Application.Models;
using Woof.Pages.Shared;

namespace Woof.Pages;

public partial class Dashboard
{
    [CascadingParameter]
    public MainLayout? Layout { get; set; }
    private User? user => Layout.User;
}
