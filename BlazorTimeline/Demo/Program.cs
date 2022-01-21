using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace Demo {
	public class Program {
		public static async Task Main(string[] args) {
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			builder.Services.AddMudServices();
			builder.Services.AddScoped(
				_ => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

			await builder.Build().RunAsync();
		}
	}
}