﻿using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace SimpleBlockchain.IntegrationTests;

public class GrpcTestFixture<TStartup> : IDisposable where TStartup : class
{
    private bool _disposed;
    private TestServer? _server;
    private IHost? _host;
    private HttpMessageHandler? _handler;
    private Action<IWebHostBuilder>? _configureWebHost;
    
    public HttpMessageHandler Handler
    {
        get
        {
            EnsureServer();
            return _handler!;
        }
    }

    public IServiceProvider ServiceProvider
    {
        get
        {
            EnsureServer();
            return _host?.Services!;
        }
    }

    public void ConfigureWebHost(Action<IWebHostBuilder> configure)
    {
        _configureWebHost = configure;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        
        if (disposing)
        {
            _handler?.Dispose();
            _host?.Dispose();
            _server?.Dispose();
        }
        _disposed = true;
    }
    
    private void EnsureServer()
    {
        if (_host != null) return;
     
        var builder = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webHost =>
            {
                webHost
                    .UseTestServer()
                    .UseStartup<TStartup>();

                _configureWebHost?.Invoke(webHost);
            });
            
        _host = builder.Start();
        _server = _host.GetTestServer();
        _handler = _server.CreateHandler();
    }
}