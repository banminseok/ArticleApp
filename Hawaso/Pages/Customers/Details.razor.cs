using ArticleApp.Models;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using UploadApp.Shared;

namespace Hawaso.Pages.Customers
{
    public partial class Details
    {
        #region Parameters
        [Parameter]
        public int CustomerId { get; set; }
        #endregion

        #region Injectors
        [Inject]
        public ICustomerRepository CustomerRepositoryReference { get; set; }

        [Inject]
        private ILogger<Details> Logger { get; set; }
        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        #endregion

        #region Fields
        private Customer customer = new();
        #endregion

        #region Lifecycle Methods   
        protected override async Task OnInitializedAsync()
        {
            customer = await CustomerRepositoryReference.GetByIdAsync(CustomerId);
            Logger.LogInformation($"※※※ id: ${CustomerId}, model 불러오기 , {JsonSerializer.Serialize(customer)}");
            //content = Dul.HtmlUtility.EncodeWithTabAndSpace(model.Content);
        }
        #endregion
    }
}
