﻿#region

using LoESoft.Core;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

#endregion

namespace LoESoft.AppEngine
{
    public abstract class RequestHandler
    {
        protected NameValueCollection Query { get; private set; }
        protected HttpListenerContext Context { get; private set; }
        protected Database Database => Program.Database;
        protected EmbeddedData GameData => Program.GameData;

        public void HandleRequest(HttpListenerContext context)
        {
            Context = context;
            if (ParseQueryString())
            {
                Query = new NameValueCollection();
                using (var reader = new StreamReader(context.Request.InputStream))
                    Query = HttpUtility.ParseQueryString(reader.ReadToEnd());

                if (Query.AllKeys.Length == 0)
                {
                    string currurl = context.Request.RawUrl;
                    int iqs = currurl.IndexOf('?');
                    if (iqs >= 0)
                        Query = HttpUtility.ParseQueryString((iqs < currurl.Length - 1) ? currurl.Substring(iqs + 1) : string.Empty);
                }
            }

            HandleRequest();
        }

        public void WriteLine(XElement value, bool xml = true, params object[] args)
        {
            if (xml)
                using (var writer = XmlWriter.Create(Context.Response.OutputStream, settings))
                    value.Save(writer);
            else
                using (var writer = new StreamWriter(Context.Response.OutputStream))
                    if (args == null || args.Length == 0) writer.Write(value);
                    else writer.Write(value.ToString(), args);
        }

        public void WriteLine(string value, bool xml = true, params object[] args)
        {
            if (xml)
                using (var writer = XmlWriter.Create(Context.Response.OutputStream, settings))
                    XElement.Parse(value).Save(writer);
            else
                using (var writer = new StreamWriter(Context.Response.OutputStream))
                    if (args == null || args.Length == 0) writer.Write(value);
                    else writer.Write(value, args);
        }

        public void WriteErrorLine(string value, bool xml = true)
        {
            if (xml)
                using (var writer = XmlWriter.Create(Context.Response.OutputStream, settings))
                    XElement.Parse($"<Error>{value}</Error>").Save(writer);
            else
                using (var writer = new StreamWriter(Context.Response.OutputStream))
                    writer.Write($"<Error>{value}</Error>");
        }

        private XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "    ",
            OmitXmlDeclaration = true,
            Encoding = Encoding.UTF8
        };

        protected virtual bool ParseQueryString() => true;

        protected abstract void HandleRequest();
    }
}