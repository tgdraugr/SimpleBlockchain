using SimpleBlockchain.API.Services;

namespace SimpleBlockchain.API;

public class Startup
{
    private const int ProofOfWorkDifficulty = 4;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc();
        services.AddSingleton<IProduceHash,Sha256HashProducer>();
        services.AddSingleton<IBrewNonce, SimpleProofOfWork>(serviceProvider =>
        {
            var hashProducer = serviceProvider.GetRequiredService<IProduceHash>();
            return new SimpleProofOfWork(hashProducer, ProofOfWorkDifficulty);
        });
        services.AddSingleton<Blockchain>();
        // services.AddSingleton<Neighbors>();
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
            endpoints.MapGrpcService<BlockchainNodeService>();
        });
    }
}