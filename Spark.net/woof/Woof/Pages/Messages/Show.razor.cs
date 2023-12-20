using Microsoft.AspNetCore.Components;
using Woof.Application.Models;
using Woof.Application.Services;
namespace Woof.Pages.Messages;

public partial class Show
{

    [Parameter]
    public int id { get; set; }
    private Message? message { get; set; }

    [Inject]
    MessagesService messagesService { get; set; }

    protected override async Task OnInitializedAsync()
	{
        message = await messagesService.Get(id);
	}

}
