namespace OpenAIWebApi.Controllers;

using Microshaoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Xml;
using Tencent;
using AzureOpenAI.Utilities;


[Route("api/[controller]")]
[ApiController]
public class CallbackController : ControllerBase
{
    private readonly ILogger _logger;

    public Settings _settings { get; }

    private WXBizMsgCrypt _wxBizMsgCrypt;

    public CallbackController(ILogger<CallbackController> logger, IOptionsSnapshot<Settings> options)
    {
        _settings = options.Value;
        _logger = logger;

        _wxBizMsgCrypt = new WXBizMsgCrypt(_settings.WxToken!, _settings.WxEncodingAESKey!, _settings.WxAppId!);
    }

    [HttpGet]
    public string Get()
    {
        Console.WriteLine("===Get====");
        var query = HttpContext.Request.Query;
        string echostr = query["echostr"];
        return echostr;

    }

    [HttpPost]
    public async Task<string> PostAsync()
    {
        var @return = string.Empty;
        var query = HttpContext.Request.Query;
        string msg_signature = string.Empty;
        string nonce = string.Empty;
        string timestamp = string.Empty;
        string encrypt_type = string.Empty;
        if (query is not null)
        {
            msg_signature = query["msg_signature"];
            nonce = query["nonce"];
            timestamp = query["timestamp"];
            encrypt_type = query["encrypt_type"];
            _ = query["signature"];
            _ = query["echostr"];
            if (!string.IsNullOrEmpty(encrypt_type))
            {
                if (!string.Equals(encrypt_type, "aes", StringComparison.OrdinalIgnoreCase))
                {
                    return string.Empty;
                }
            }
        }

        using StreamReader reader = new StreamReader(HttpContext.Request.Body);
        var data = await reader.ReadToEndAsync();
        XmlDocument? xmlDocument = null;
        string decryptedMsg = string.Empty;
        if (!string.IsNullOrEmpty(encrypt_type))
        {
            xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(data);
            var r = _wxBizMsgCrypt.DecryptMsg(xmlDocument, msg_signature, timestamp, nonce, ref decryptedMsg);
            if (r != 0)
            {
                decryptedMsg = "wx error";
            }
        }

        if (xmlDocument is not null)
        {
            if (!string.IsNullOrEmpty(decryptedMsg))
            {
                xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(decryptedMsg);
                string message = xmlDocument.SelectSingleNode("xml/Content")?.ChildNodes[0]?.Value!;

                var originalWeChatFromUserId = WeiXinXML.GetFromXML(xmlDocument, "FromUserName");
                var originalWeChatToUserId = WeiXinXML.GetFromXML(xmlDocument, "ToUserName");
                var weChatMsgId = WeiXinXML.GetFromXML(xmlDocument, "MsgId");

                message = await OpenAI.GetOpenAIResultAsync(message, _settings);

                var wxXmlMessage = WeiXinXML.CreateTextMsg(message, originalWeChatFromUserId!, originalWeChatToUserId!);

                timestamp = WeiXinXML.DateTime2Int(DateTime.Now).ToString();

                @return = wxXmlMessage;

                _wxBizMsgCrypt.EncryptMsg(wxXmlMessage, timestamp,nonce, ref @return);

            }
        }
        return @return;
    }
}
