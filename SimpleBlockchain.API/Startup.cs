using SimpleBlockchain.API.Services;

namespace SimpleBlockchain.API;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc();
        services.AddSingleton<IProduceHash,Sha256HashProducer>();
        services.AddSingleton<IBrewNonce, SimpleProofOfWork>();
        services.AddSingleton<Blockchain>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<GreeterService>();
            endpoints.MapGrpcService<BlockchainNodeService>();
        });
    }
}