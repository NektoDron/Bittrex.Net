﻿#if NETSTANDARD
using Bittrex.Net.Interfaces;
using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication;
using WebSocket4Net;

namespace Bittrex.Net.Sockets
{
    public class Websocket4Net : IWebsocket
    {
        List<Action<Exception>> errorhandlers = new List<Action<Exception>>();
        List<Action> openhandlers = new List<Action>();
        List<Action> closehandlers = new List<Action>();
        List<Action<string>> messagehandlers = new List<Action<string>>();
        WebSocket socket;

        public Websocket4Net(string url, IDictionary<string, string> cookies, IDictionary<string, string> headers)
        {
            Debug.WriteLine($"Creating socket. Url: {url}");

            socket = new WebSocket(url, cookies: cookies.ToList(), customHeaderItems: headers.ToList(), receiveBufferSize: 2048, sslProtocols: SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls);
            socket.NoDelay = true;

            socket.Error += HandleError;
            socket.Opened += HandleOpen;
            socket.Closed += HandleClose;
            socket.MessageReceived += HandleMessage;
        }

        private void HandleError(object sender, ErrorEventArgs e)
        {
            Debug.WriteLine($"Error: {e.Exception}");
            foreach (var handler in errorhandlers)
                handler(e.Exception);
        }

        private void HandleOpen(object sender, EventArgs e)
        {
            Debug.WriteLine($"Open");
            foreach (var handler in openhandlers)
                handler();
        }

        private void HandleClose(object sender, EventArgs e)
        {
            Debug.WriteLine($"Close");
            foreach (var handler in closehandlers)
                handler();
        }

        private void HandleMessage(object sender, MessageReceivedEventArgs e)
        {
            Debug.WriteLine($"Message");
            foreach (var handler in messagehandlers)
                handler(e.Message);
        }

        public event Action<Exception> OnError
        {
            add { errorhandlers.Add(value); }
            remove { errorhandlers.Remove(value); }
        }
        public event Action OnOpen
        {
            add { openhandlers.Add(value); }
            remove { openhandlers.Remove(value); }
        }
        public event Action OnClose
        {
            add { closehandlers.Add(value); }
            remove { closehandlers.Remove(value); }
        }
        public event Action<string> OnMessage
        {
            add { messagehandlers.Add(value); }
            remove { messagehandlers.Remove(value); }
        }

        public void Close()
        {
            Debug.WriteLine($"Close call");
            socket.Close();
        }

        public bool IsClosed()
        {
            Debug.WriteLine($"IsClosed = {socket.State == WebSocketState.Closed}");
            return socket.State == WebSocketState.Closed;
        }

        public bool IsOpen()
        {
            Debug.WriteLine($"IsOpen = {socket.State == WebSocketState.Open}");
            return socket.State == WebSocketState.Open;
        }

        public void Open()
        {
            Debug.WriteLine($"Opening");
            socket.Open();
        }

        public void Send(string data)
        {
            Debug.WriteLine($"Sending {data}");
            socket.Send(data);
        }
    }
}
#endif