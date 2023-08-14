namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json.Nodes;

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

    [HttpPost]
    [HttpGet]
    [Route("fetch")]
    public async Task FetchEventStream([FromBody] JsonNode? parameters)
    {
        var response = HttpContext.Response;
        response.ContentType = "text/event-stream";
        for (int i = 0; i < 10; i++)
        {
            string data = @$"id: {Guid.NewGuid()}
retry: 1000
event: NEW_LOG
data: 你好{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}


";
            data = $"data: {i},你好{parameters},{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}\n";    
            var bytes = Encoding.UTF8.GetBytes(data);

            await response.Body.WriteAsync(bytes);
            await response.Body.FlushAsync();
            await Task.Delay(1000 * 2);
        }
    }
}
