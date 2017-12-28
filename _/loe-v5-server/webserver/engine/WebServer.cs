﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;

namespace webserver.engine
{
    public class WebServer
    {
        public static HttpListener _websocket { get; set; }
        public static Queue<HttpListenerContext> _webqueue { get; set; }
        public static ManualResetEvent _webevent { get; set; }

        private static Thread[] _webthread { get; set; }
        private static object _weblock { get; set; }
        
        public static bool shutdown { get; set; }

        public static void Start()
        {
            Thread.CurrentThread.Name = "Entry";
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            
            Log.Info("Initializing WebServer... OK!");

            shutdown = false;

            _webthread = new Thread[5];
            _webqueue = new Queue<HttpListenerContext>();
            _webevent = new ManualResetEvent(false);
            _weblock = new object();

            Log.Info("Initializing WebSocket...");

            if (!Utils.PortCheck(Constants.PORT))
                ForceShutdown();

            WebSocket();

            Log.Info("Initializing WebSocket... OK!");
        }

        private static void WebSocket()
        {
            string _webaddress = $"http://localhost:{Constants.PORT}/";

            WebSocketAddAddress(_webaddress, Environment.UserDomainName, Environment.UserName);

            _websocket = new HttpListener();
            _websocket.Prefixes.Add(_webaddress);
            _websocket.Start();

            _websocket.BeginGetContext(WebSocketCallback, null);

            int i = 0;

            do
            {
                _webthread[i] = new Thread(WebSocketThread)
                {
                    Name = $"WebSocketThread_{i}"
                };
                _webthread[i].Start();
                i++;
            } while (i < _webthread.Length);
        }

        private static void WebSocketAddAddress(string address, string domain, string user)
        {
            string args = string.Format(@"http add urlacl url={0}", address) + " user=\"" + domain + "\\" + user + "\"";

            ProcessStartInfo psi = new ProcessStartInfo("netsh", args);
            psi.Verb = "runas";
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = true;

            Process.Start(psi).WaitForExit();
        }

        private static void WebSocketCallback(IAsyncResult response)
        {
            if (!_websocket.IsListening)
                return;

            HttpListenerContext _webcontext = _websocket.EndGetContext(response);

            _websocket.BeginGetContext(WebSocketCallback, null);

            lock (_weblock)
            {
                _webqueue.Enqueue(_webcontext);
                _webevent.Set();
            }
        }

        private static void WebSocketThread()
        {
            do
            {
                if (shutdown)
                    return;

                HttpListenerContext _webcontext;

                lock (_weblock)
                {
                    if (_webqueue.Count > 0)
                        _webcontext = _webqueue.Dequeue();
                    else
                    {
                        _webevent.Reset();
                        continue;
                    }
                }

                try
                {
                    WebSocketHandler(_webcontext);
                } catch (Exception e)
                {
                    Log.Error("WebSocketThread", "Unhandled exception");
                    Log.Error($"Bad data processing:\n{e}");
                    return;
                }
            } while (_webevent.WaitOne());
        }

        private static void WebSocketHandler(HttpListenerContext _webcontext)
        {
            try
            {
                string _path;

                if (_webcontext.Request.Url.LocalPath.IndexOf(".") == -1)
                    _path = "webserver.files" + _webcontext.Request.Url.LocalPath.Replace("/", ".");
                else
                    _path = "webserver.files" + _webcontext.Request.Url.LocalPath.Remove(_webcontext.Request.Url.LocalPath.IndexOf(".")).Replace("/", ".");

                Type _type = Type.GetType(_path);

                if (_type != null)
                    Log.Info($"{(_webcontext.Request.RemoteEndPoint.Address.ToString() == "::1" ? "localhost" : $"{_webcontext.Request.RemoteEndPoint.Address.ToString()}")}", _webcontext.Request.Url.LocalPath);
                else
                    Log.Warn($"{(_webcontext.Request.RemoteEndPoint.Address.ToString() == "::1" ? "localhost" : $"{_webcontext.Request.RemoteEndPoint.Address.ToString()}")}", _webcontext.Request.Url.LocalPath);
            } catch (Exception e)
            {
                _webcontext = _webqueue.Dequeue();

                using (StreamWriter stream = new StreamWriter(_webcontext.Response.OutputStream))
                    stream.Write($"<h1>Bad request!</h1>\n{_webcontext.Request.Url.LocalPath}");

                    Log.Error("WebSocketHandler", "Unhandled exception");
                Log.Error(e.ToString());
            }

            _webcontext?.Response.Close();
        }

        private static void ForceShutdown()
        {
            shutdown = true;

            int i = 3;

            do
            {
                Log.Info($"Port {Constants.PORT} is occupied, restarting in {i} second{(i <= 1 ? "" : "s")}...");
                Thread.Sleep(1000);
                i--;
            } while (i != 0);

            Log.Warn("Terminated WebServer.");

            Thread.Sleep(1000);

            Process.Start(Utils.ProcessFile("webserver"));

            Environment.Exit(0);
        }
    }
}