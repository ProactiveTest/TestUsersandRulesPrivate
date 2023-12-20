using Spark.Library.Database;

namespace HealthyTips.Application.Models;

public class Article : BaseModel
{
    public string Slug { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string UserId { get; set; }

    public string User { get; set; }
}