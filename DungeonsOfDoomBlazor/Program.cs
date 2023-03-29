using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using DungeonsOfDoomBlazor;
using DungeonsOfDoomBlazor.GameEngine.Services;
using DungeonsOfDoomBlazor.GameEngine.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddBlazorise(o => { o.Immediate = false; }).AddBootstrapProviders();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<IGameSession, GameSession>();
builder.Services.AddSingleton<IDiceService, DiceService>();
builder.Services.AddTransient<MerchantVM>();

await builder.Build().RunAsync();
