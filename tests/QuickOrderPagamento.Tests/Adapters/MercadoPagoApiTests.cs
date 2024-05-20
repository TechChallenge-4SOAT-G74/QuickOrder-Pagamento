using Flurl.Http.Testing;
using Microsoft.Extensions.Options;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Requests;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Responses;
using QuickOrderPagamento.Adapters.Driven.MercadoPago;
using QuickOrderPagamento.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace QuickOrderPagamento.Tests.Adapters
{
    public class MercadoPagoApiTests
    {
        [Fact]
        public async Task GeraQrCodePagamento_Sucesso()
        {
            // Arrange
            var options = Options.Create(new MercadoPagoSettings
            {
                AccessToken = "tokenTeste",
                User_id = 1,
                External_pos_id = "pos_id",
                NotificationUrl = "http://notification.url"
            });

            var request = new PaymentQrCodeRequest();
            var expectedResponse = new PaymentQrCodeResponse();

            using var httpTest = new HttpTest();
            httpTest.RespondWithJson(expectedResponse);

            var mercadoPagoApi = new MercadoPagoApi(options);

            // Act
            var response = await mercadoPagoApi.GeraQrCodePagamento(request);

            // Assert
            Assert.NotNull(response);
        }

        [Fact]
        public async Task GeraQrCodePagamento_Erro()
        {
            // Arrange
            var options = Options.Create(new MercadoPagoSettings
            {
                AccessToken = "tokenTeste",
                User_id = 1,
                External_pos_id = "pos_id",
                NotificationUrl = "http://notification.url"
            });

            var request = new PaymentQrCodeRequest();

            using var httpTest = new HttpTest();
            httpTest.SimulateTimeout();

            var mercadoPagoApi = new MercadoPagoApi(options);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await mercadoPagoApi.GeraQrCodePagamento(request));
        }

        [Fact]
        public async Task ObterPagamento_Sucesso()
        {
            // Arrange
            var options = Options.Create(new MercadoPagoSettings
            {
                AccessToken = "tokenTeste",
                User_id = 1
            });

            var expectedPayment = new Payment(); 

            using var httpTest = new HttpTest();
            httpTest.RespondWithJson(expectedPayment);

            var mercadoPagoApi = new MercadoPagoApi(options);

            // Act
            var response = await mercadoPagoApi.ObterPagamento("payment_id");

            // Assert
            Assert.NotNull(response);
        }

        [Fact]
        public async Task ObterPagamento_Erro()
        {
            // Arrange
            var options = Options.Create(new MercadoPagoSettings
            {
                AccessToken = "tokenTeste",
                User_id = 1
            });

            using var httpTest = new HttpTest();
            httpTest.SimulateTimeout();

            var mercadoPagoApi = new MercadoPagoApi(options);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await mercadoPagoApi.ObterPagamento("payment_id"));
        }
    }
}
