using System;
using Grpc.Net.Client;
using SimpleBlockchain.API;
using Xunit;

namespace SimpleBlockchain.IntegrationTests;

public class IntegrationTest : IClassFixture<GrpcTestFixture<Startup>>, IDisposable
{
    private GrpcChannel? _channel;

    protected IntegrationTest(GrpcTestFixture<Startup> fixture)
    {
        Fixture = fixture;
    }

    private GrpcTestFixture<Startup> Fixture { get; }
    
    protected GrpcChannel Channel => _channel ??= CreateChannel();

    private GrpcChannel CreateChannel()
    {
        return GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
        {
            HttpHandler = Fixture.Handler
        });
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _channel = null;
    }
}