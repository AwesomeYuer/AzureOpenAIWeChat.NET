namespace WebApi.Controllers;

// GPTAPI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// GPTAPI.Controllers.OpenAIController
//using GPTAPI;
//using GPTAPI.Controllers;
//using GPTAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
//using Azure.AI;

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
