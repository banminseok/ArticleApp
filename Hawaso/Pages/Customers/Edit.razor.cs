using ArticleApp.Models;
using Microsoft.AspNetCore.Components;
using UploadApp.Shared;

namespace Hawaso.Pages.Customers
{
    public partial class Edit
    {
        #region Fields
        private string[] genders = { "Male", "Female" };
        #endregion

        [Parameter]
        public int CustomerId { get; set; }

        [Inject]
        private ILogger<Edit> Logger { get; set; }


        [Inject]
        public ICustomerRepository CustomerRepositoryReference { get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        protected Customer customer = new();
        protected string oriParentId;
        protected override async Task OnInitializedAsync()
        {
            customer = await CustomerRepositoryReference.GetByIdAsync(CustomerId);
            //content = Dul.HtmlUtility.EncodeWithTabAndSpace(model.Content);
            oriParentId = customer.CustomerId.ToString();
        }

        protected async Task btnEdit_Click()
        {
            customer.Modified = DateTime.Now;
            int.TryParse(oriParentId, out int parentId);
            customer.CustomerId = parentId;
            await CustomerRepositoryReference.EditAsync(customer);
            NavigationManagerReference.NavigateTo($"/Customers/Details/{CustomerId}");
        }

        //protected async void FormSubmit()
        //{
        //    int.TryParse(ParentId, out int parentId);
        //    model.ParentId = parentId;

        //    await ReplyRepositoryReference.EditAsync(model);
        //    NavigationManagerReference.NavigateTo("/Replys");
        //}

    }
}
