using System.Net;
using System.Text;

using BatteryMonitorApp.Contracts.Models.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;

namespace BatteryMonitorApp.IntegrationTests
{
    public class DataApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public DataApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PutDataCheckUri()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();
            // Act
            HttpResponseMessage response = await client.PutAsync("api/data", new StringContent(""));
            var code = response.StatusCode;
            // Assert
            Assert.True(code != HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task PutDataCheckCheckIsSuccessStatusCode()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();
            var formdata = new BatteryDataShortFormat()
            {
                Di = PhysicalDeviceEmulator.PhysicalDeviceEmulator.Id,
                V = 33.5f
            };
            // Act
            HttpResponseMessage response = await client.PutAsync("api/data", new StringContent(
            JsonConvert.SerializeObject(formdata), Encoding.UTF8, "application/json"));
            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }
        [Fact]
        public async Task PutDataSendEmptyData()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();
            // Act
            HttpResponseMessage response = await client.PutAsync("api/data", new StringContent(""));
            var code = response.StatusCode;
            // Assert
            Assert.True(code == HttpStatusCode.UnsupportedMediaType);
        }
        [Fact]
        public async Task PutDataChectOkStatus()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();
            // Act
            HttpResponseMessage response = await client.PutAsync("api/data", new StringContent(
                JsonConvert.SerializeObject(PhysicalDeviceEmulator.PhysicalDeviceEmulator.TestBatteryData), Encoding.UTF8, "application/json"));

            var code = response.StatusCode;
            // Assert
            Assert.True(code == HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetDataCheckUri()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();
            // Act
            HttpResponseMessage response = await client.GetAsync("api/data");
            var code = response.StatusCode;
            // Assert
            Assert.True(code != HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task GetDataCheckEmptyRequest()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();
            // Act
            HttpResponseMessage response = await client.GetAsync("api/data");
            var code = response.StatusCode;
            string content = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.True(code == HttpStatusCode.UnsupportedMediaType);
        }

        [Fact]
        public async Task GetDataChectOkStatus()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();
            // Act
            HttpResponseMessage response = await client.GetAsync(
                $"api/data?di={PhysicalDeviceEmulator.PhysicalDeviceEmulator.
                TestBatteryData.DeviceId}");
            var code = response.StatusCode;
            string content = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.True(code == HttpStatusCode.OK);
        }
    }
}
