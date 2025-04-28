using Microsoft.AspNetCore.Components;

namespace ModernBusiness.Pages.Portfolios
{

    public partial class PortfolioThree
    {
        [Inject] 
        NavigationManager NavigationManager { get; set; } = default!;


        protected override void OnInitialized()
        {
            // Initialization logic can go here
        }

        protected void GotoDotnet()
        {
            NavigationManager.NavigateTo("/About");
        }
    }
}
