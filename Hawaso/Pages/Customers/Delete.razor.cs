using ArticleApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using UploadApp.Shared;

namespace Hawaso.Pages.Customers
{
    public partial class Delete
    {
        [Parameter]
        public int CustomerId { get; set; }

        [Inject]
        private ILoggerFactory loggerFactory { get; set; }

        //ILogger<Create> Logger { get; set; }


        [Inject]
        public ICustomerRepository CustomerRepositoryAsync { get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        private Customer customer = new Customer();

        protected string content = "";

        protected override async Task OnInitializedAsync()
        {
            loggerFactory.CreateLogger(nameof(Delete));
            customer = await CustomerRepositoryAsync.GetByIdAsync(CustomerId);
            //content = Dul.HtmlUtility.EncodeWithTabAndSpace(model.Content);
        }

        protected async void btnDelete_Click()
        {
            bool isDelete = await JSRuntime.InvokeAsync<bool>("confirm", $"{CustomerId}번 고객 정보를 정말로 삭제하시겠습니까?");
            if (isDelete)
            {
                await CustomerRepositoryAsync.DeleteAsync(CustomerId);
                NavigationManagerReference.NavigateTo("/Customers");
            }
            else
            {
                await JSRuntime.InvokeAsync<object>("alert", "취소되었습니다.");
            }
        }
    }
}
