﻿using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Azure.Functions.Worker.Core.Converters.Converter;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    [BindingConverter(typeof(MyProductVmCustomConverter))]
    public sealed class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public decimal Price { set; get; }
    }

    public sealed class MyProductVmCustomConverter : IConverter
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MyProductVmCustomConverter> _logger;
        public MyProductVmCustomConverter(IHttpClientFactory httpClientFactory, ILogger<MyProductVmCustomConverter> logger)
        {
            this._httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this._logger = logger;
        }

        public async ValueTask<BindingResult> ConvertAsync(ConverterContext context)
        {
            // currently gets called for all params
            if (context.Parameter.Type != typeof(ProductViewModel))
            {
                return await new ValueTask<BindingResult>(BindingResult.Failed());
            }

            int prodId = 0;
            if (context.FunctionContext.BindingContext.BindingData.TryGetValue("productId", out var productIdValObj))
            {
                prodId = Convert.ToInt32(productIdValObj);
            }
            var reqMsg = new HttpRequestMessage(HttpMethod.Get, $"https://shkr-playground.azurewebsites.net/api/products/{prodId}");
            var client = this._httpClientFactory.CreateClient();

            using var response = await client.SendAsync(reqMsg);
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                var productVm = await JsonSerializer.DeserializeAsync<ProductViewModel>(stream, SharedJsonSettings.SerializerOptions);
                this._logger.LogInformation("Received product info from REST API");

                return await new ValueTask<BindingResult>(BindingResult.Success(productVm));
            }
        }
    }
}
