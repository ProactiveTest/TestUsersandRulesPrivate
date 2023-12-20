using Microsoft.AspNetCore.Components;
using Spark.Library.Logging;
using Woof.Application.Models;
using ILogger = Spark.Library.Logging.ILogger;

namespace Woof.Pages;

public partial class Index
{
    [Inject]
    public ILogger Logger { get; set; } = default!;

    private List<Message> Messages = new List<Message>();

    protected override async Task OnInitializedAsync()
    {
        Messages = await messageService.GetAll();
    }
    
        
   
}
