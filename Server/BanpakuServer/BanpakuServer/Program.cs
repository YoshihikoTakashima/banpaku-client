﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;

static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseKestrel(options =>
        {
            options.ConfigureEndpointDefaults(endpointOptions =>
            {
                endpointOptions.Protocols = HttpProtocols.Http2;
            });
            
            options.ListenAnyIP(8282, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
            });
        });
		
        builder.Services.AddGrpc();
        builder.Services.AddMagicOnion();
        var app = builder.Build();
		
        app.MapMagicOnionService();
		
        app.Run();
    }
}