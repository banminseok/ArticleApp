using ArticleApp.Models;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Text.Json.Nodes;
using UploadApp.Shared;

namespace Hawaso.Pages.Customers
{
    public partial class Create
    {
        [Inject]
        private ILogger<Create> Logger { get; set; }

        [Inject]
        public ICustomerRepository CustomerRepositoryReference { get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        private Customer customer = new Customer();
        private string[] genders = { "Male", "Female" };


        protected async void btnSubmit_Click()
        {
            customer.Created = DateTime.Now;
            await CustomerRepositoryReference.AddAsync(customer);
            
            Logger.LogInformation($"※※※ Customer model 저장 , {JsonSerializer.Serialize(customer)}");
            NavigationManagerReference.NavigateTo("/Customers");
        }

    }
}
