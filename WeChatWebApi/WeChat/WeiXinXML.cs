namespace Microshaoft
{
    using System;
    using System.Xml;

    public class WeiXinXML
    {
        public static string CreateTextMsg(XmlDocument xmlDoc, string content)
        {
            var toUserName = GetFromXML(xmlDoc, "FromUserName");
            var fromUserName = GetFromXML(xmlDoc, "ToUserName");

            return string.Format("<xml>\r\n                <ToUserName><![CDATA[{0}]]></ToUserName>\r\n                <FromUserName><![CDATA[{1}]]></FromUserName>\r\n                <CreateTime>{2}</CreateTime>\r\n                <MsgType><![CDATA[text]]></MsgType>\r\n                <Content><![CDATA[{3}]]></Content>\r\n                </xml>", toUserName, fromUserName, DateTime2Int(DateTime.Now), content);
        }

        public static string CreatePicMsg(XmlDocument xmlDoc, string content)
        {
            var toUserName = GetFromXML(xmlDoc, "FromUserName");
            var fromUserName = GetFromXML(xmlDoc, "ToUserName");


            return string.Format("<xml>\r\n                <ToUserName><![CDATA[{0}]]></ToUserName>\r\n                <FromUserName><![CDATA[{1}]]></FromUserName>\r\n                <CreateTime>{2}</CreateTime>\r\n                <MsgType><![CDATA[image]]></MsgType>\r\n                <Image>\r\n                <MediaId><![CDATA[{3}]]></MediaId>\r\n                </Image>\r\n                </xml>", toUserName, fromUserName, DateTime2Int(DateTime.Now), content);
        }

        public static string CreateMediaMsg(XmlDocument xmlDoc, string mediaid, string title, string desc)
        {
            var toUserName = GetFromXML(xmlDoc, "FromUserName");
            var fromUserName = GetFromXML(xmlDoc, "ToUserName");

            return string.Format("<xml>\r\n                <ToUserName><![CDATA[{0}]]></ToUserName>\r\n                <FromUserName><![CDATA[{1}]]></FromUserName>\r\n                <CreateTime>{2}</CreateTime>\r\n                <MsgType><![CDATA[video]]></MsgType>\r\n                <Video>\r\n                    <MediaId><![CDATA[{3}]]></MediaId>\r\n                    <Title><![CDATA[{4}]]></Title>\r\n                    <Description><![CDATA[{5}]]></Description>\r\n                </Video>\r\n                </xml>", toUserName, fromUserName, DateTime2Int(DateTime.Now), mediaid, title, desc);
        }

        public static string CreateArticleMsg(XmlDocument xmlDoc, string Title, string Description, string PicUrl, string Url)
        {
            var toUserName = GetFromXML(xmlDoc, "FromUserName");
            var fromUserName = GetFromXML(xmlDoc, "ToUserName");

            return string.Format("<xml>\r\n                <ToUserName><![CDATA[{0}]]></ToUserName>\r\n                <FromUserName><![CDATA[{1}]]></FromUserName>\r\n                <CreateTime>{2}</CreateTime>\r\n                <MsgType><![CDATA[news]]></MsgType>\r\n                <ArticleCount>1</ArticleCount>\r\n                <Articles>\r\n                    <item>\r\n                        <Title><![CDATA[{3}]]></Title>\r\n                        <Description><![CDATA[{4}]]></Description>\r\n                        <PicUrl><![CDATA[{5}]]></PicUrl>\r\n                        <Url><![CDATA[{6}]]></Url>\r\n                    </item>\r\n                </Articles>\r\n                </xml>", toUserName, fromUserName, DateTime2Int(DateTime.Now), Title, Description, PicUrl, Url);
        }

        public static int DateTime2Int(DateTime dt)
        {
            DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(dt - dateTime).TotalSeconds;
        }

        public static string? GetFromXML(XmlDocument xmlDoc, string name)
        {
            var @return = string.Empty;
            XmlNode xmlNode = xmlDoc.SelectSingleNode("xml/" + name);
            if (xmlNode != null && xmlNode.ChildNodes.Count > 0)
            {
                @return = xmlNode.ChildNodes[0].Value;
            }
            return @return;
        }
    }
}
