using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SCBHarmonization.NibssClass
{
    public class HttpClientHelper
    {
        public IHost Modelbuilder()
        {
            var builder = new HostBuilder()
            .ConfigureServices((hostContext, service) =>
            {
                service.AddHttpClient("DataAggregation", configureClient: c =>
                {
                    c.BaseAddress = new Uri("http://10.205.69.213:8000/nibss/dataaggr/");
                    c.Timeout = TimeSpan.FromMinutes(15);
                    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    c.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                });
                service.AddTransient<IDataAggregationInterface, DataAggregationInplementation>();
            }).UseConsoleLifetime();

            var host = builder.Build();
            return host;
        }

    }
}