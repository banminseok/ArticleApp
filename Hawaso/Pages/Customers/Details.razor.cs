using ArticleApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
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
        private ILoggerFactory loggerFactory { get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        #endregion

        #region Fields
        private Customer customer = new();
        private ILogger _logger;
        #endregion

        #region Lifecycle Methods   
        protected override async Task OnInitializedAsync()
        {
            _logger = loggerFactory.CreateLogger(nameof(Details));
            customer = await CustomerRepositoryReference.GetByIdAsync(CustomerId);
            _logger.LogInformation($"※※※ id: ${CustomerId}, model 불러오기 , {JsonSerializer.Serialize(customer)}");
            //content = Dul.HtmlUtility.EncodeWithTabAndSpace(model.Content);
        }
        #endregion
    }
}
