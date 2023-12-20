using Microsoft.AspNetCore.Components;
using Woof.Application.Services;
using Woof.Application.Models;


namespace Woof.Pages.Messages;

public partial class Edit
{
    [Parameter]
    public int Id { get; set; }

    [Inject]
    MessagesService messagesService { get; set; }

    [Inject]
    NavigationManager navManager { get; set; }
    private Message? message { get; set; }

    protected override async Task OnInitializedAsync()
    {
        message = await messagesService.Get(Id);
    }

    private async Task UpdateMessage()
    {
        message.UpdatedAt = DateTime.Now;
        message = await messagesService.Update(message);
        navManager.NavigateTo("");

    }
}
