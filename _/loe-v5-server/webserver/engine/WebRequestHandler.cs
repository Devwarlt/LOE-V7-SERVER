using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace webserver.engine
{
    public abstract class WebRequestHandler
    {
        protected NameValueCollection _webquery { get; private set; }
        protected HttpListenerContext _webcontext { get; private set; }

        public class WebSocketObject : IDisposable
        {
            public string _weburl { get; set; }
            public HttpListenerContext _webcontext { get; set; }
            public NameValueCollection _webquery { get; set; }

            public NameValueCollection WebSocketParseParameters(int parameters, string url) => HttpUtility.ParseQueryString((parameters < url.Length - 1) ? url.Substring(parameters + 1) : string.Empty);

            public const char WebSocketCharParameter = '?';

            public void Dispose()
            {
                _weburl = null;
                _webcontext = null;
                _webquery = null;
            }
        }

        public class WebSocketData : IDisposable
        {
            public XElement _webelement { get; set; }
            public string _webvalue { get; set; }
            public bool _webxml { get; set; }
            public object[] _webargs { get; set; }
            public StreamWriter _webstream { get; set; }

            public static XmlWriterSettings WebSocketXMLSettings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "    ",
                OmitXmlDeclaration = true,
                Encoding = Encoding.UTF8
            };

            public void WriteElement() => _webstream.Write(_webelement);

            public void WriteString(bool isString = false) => _webstream.Write(isString ? _webvalue : _webelement.ToString(), _webargs);

            public void Dispose()
            {
                _webelement = null;
                _webvalue = null;
                _webxml = false;
                _webargs = null;
                _webstream = null;
            }
        }

        public void WebSocketRequestHandler(HttpListenerContext _webcontext)
        {
            WebSocketObject _webobject = new WebSocketObject();
            _webobject._webcontext = _webcontext;
            _webobject._webquery = new NameValueCollection();
            _webobject._weburl = string.Empty;

            if (WebSocketVirtualValidation())
            {
                using (StreamReader stream = new StreamReader(this._webcontext.Request.InputStream))
                    _webobject._webquery = HttpUtility.ParseQueryString(stream.ReadToEnd());

                if (_webobject._webquery.AllKeys.Length == 0)
                {
                    _webobject._weburl = _webobject._webcontext.Request.RawUrl;

                    if (_webobject._weburl.IndexOf(WebSocketObject.WebSocketCharParameter) >= 0)
                        _webobject._webquery = _webobject.WebSocketParseParameters(_webobject._weburl.IndexOf(WebSocketObject.WebSocketCharParameter), _webobject._webcontext.Request.RawUrl);
                }   

                _webcontext = _webobject._webcontext;
                _webquery = _webobject._webquery;
            }

            WebSocketRequestHandler();

            _webobject.Dispose();
        }

        public void Write(XElement element, bool isXML = true, params object[] args)
        {
            WebSocketData _webdata = new WebSocketData();
            _webdata._webelement = element;
            _webdata._webxml = isXML;
            _webdata._webargs = args;

            if (_webdata._webxml)
                using (XmlWriter writer = XmlWriter.Create(_webcontext.Response.OutputStream, WebSocketData.WebSocketXMLSettings))
                    _webdata._webelement.Save(writer);
            else
                using (StreamWriter writer = new StreamWriter(_webcontext.Response.OutputStream))
                {
                    _webdata._webstream = writer;
                    if (_webdata._webargs == null || _webdata._webargs.Length == 0)
                        _webdata.WriteElement();
                    else
                        _webdata.WriteString();
                }

            _webdata.Dispose();
        }

        public void Write(string value, bool isXML = true, params object[] args)
        {
            WebSocketData _webdata = new WebSocketData();
            _webdata._webvalue = value;
            _webdata._webxml = isXML;
            _webdata._webargs = args;

            if (_webdata._webxml)
                using (XmlWriter writer = XmlWriter.Create(_webcontext.Response.OutputStream, WebSocketData.WebSocketXMLSettings))
                    XElement.Parse(_webdata._webvalue).Save(writer);
            else
                using (StreamWriter writer = new StreamWriter(_webcontext.Response.OutputStream))
                {
                    _webdata._webstream = writer;
                    if (_webdata._webargs == null || _webdata._webargs.Length == 0)
                        _webdata.WriteElement();
                    else
                        _webdata.WriteString(true);
                }

            _webdata.Dispose();
        }

        public void WriteError(string value, bool isXML = true)
        {
            WebSocketData _webdata = new WebSocketData();
            _webdata._webvalue = value;
            _webdata._webxml = isXML;

            if (_webdata._webxml)
                using (XmlWriter writer = XmlWriter.Create(_webcontext.Response.OutputStream, WebSocketData.WebSocketXMLSettings))
                    XElement.Parse($"<Error>{_webdata._webvalue}</Error>").Save(writer);
            else
                using (StreamWriter writer = new StreamWriter(_webcontext.Response.OutputStream))
                {
                    _webdata._webstream = writer;
                    _webdata._webstream.Write($"<Error>{_webdata._webvalue}</Error>");
                }

            _webdata.Dispose();
        }
        
        protected virtual bool WebSocketVirtualValidation() => true;
        protected abstract void WebSocketRequestHandler();
    }
}