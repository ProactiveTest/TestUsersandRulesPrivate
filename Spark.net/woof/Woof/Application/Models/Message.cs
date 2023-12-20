using Spark.Library.Database;

namespace Woof.Application.Models;

public class Message : BaseModel
{
    public int Id { get; set; }
    public int UserId { get; set; }

    public virtual User User { get; set; }

    public string Content { get; set; }
}