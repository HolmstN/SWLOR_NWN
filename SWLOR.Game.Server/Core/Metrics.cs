﻿using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;

namespace SWLOR.Game.Server.Core
{
    public static class Metrics
    {
        public static readonly ActivitySource ActivitySource = new ActivitySource("nwn.swlor.logging", "1.0.0");
        public static readonly string SourceName = "nwn.swlor.logging";

        public static TracerProvider Initialize()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            return Sdk.CreateTracerProviderBuilder()
                .AddGrpcClientInstrumentation()
                .AddSource(SourceName)
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("swlor"))
                .AddOtlpExporter(config =>
                {
                    config.Endpoint = new Uri("http://otel-collector:4317");
                    config.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                })
                .Build();
        }
    }
}