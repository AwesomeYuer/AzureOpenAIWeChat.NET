namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

[Route("api/[controller]")]
[ApiController]
public class OpenAIController : ControllerBase
{
    private readonly ILogger _logger;

    public Settings _settings { get; }

    public OpenAIController(ILogger<OpenAIController> logger, IOptions<Settings> options)
    {
        _settings = options.Value;
        _logger = logger;
    }

    [HttpPost]
    public Topic Post([FromBody] Topic Topic)
    {
        string ret = OpenAI.GetOpenAIResultAsync(Topic.User!, _settings).Result;
        return new Topic
        {
            Bot = ret,
            User = Topic.User
        };
    }
}
