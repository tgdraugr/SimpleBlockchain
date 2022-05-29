using System.Threading.Tasks;
using SimpleBlockchain.API;
using Xunit;

namespace SimpleBlockchain.IntegrationTests;

public class GreeterTests : IntegrationTest
{
    public GreeterTests(GrpcTestFixture<Startup> fixture) : base(fixture)
    {
    }
    
    [Fact]
    public async Task Should_say_hello_with_given_name()
    {
        // this will work as a base test for guaranteeing the server is in the right state
        var client = new Greeter.GreeterClient(Channel);
        var response = await client.SayHelloAsync(new HelloRequest { Name = "Someone" });
        Assert.Equal("Hello Someone", response.Message);
    }
}