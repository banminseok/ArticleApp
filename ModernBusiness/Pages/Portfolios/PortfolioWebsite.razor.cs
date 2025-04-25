using Microsoft.AspNetCore.Components;

namespace ModernBusiness.Pages.Portfolios
{

    public partial class PortfolioWebsite
    {
        [Inject] 
        NavigationManager NavigationManager { get; set; } = default!;


        protected override void OnInitialized()
        {
            // Initialization logic can go here
        }

        protected void GotoDotnet()
        {
            NavigationManager.NavigateTo("https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor");
        }
    }
}
