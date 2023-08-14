namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;

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
    [Route("sse")]
    public async Task Post2()
    {
        HttpContext.Response.ContentType = "text/event-stream";
        for (int i = 0; i < 10; i++)
        {
            string data = @$"id: {Guid.NewGuid()}
retry: 1000
event: message
data: 你好{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}


";
            data = $"data: 你好{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}\n";    
            var bitys = Encoding.UTF8.GetBytes(data);

            await HttpContext.Response.Body.WriteAsync(bitys);
            await HttpContext.Response.Body.FlushAsync();
            //Task.Delay(1000 * 2);
            Thread.Sleep(1000 * 5);
        }
        //return new EmptyResult();
    }
}
