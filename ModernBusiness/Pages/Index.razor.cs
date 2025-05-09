using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ModernBusiness.Pages
{
    public partial class Index
    {
        [Inject] 
        private IJSRuntime JsRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JsRuntime.InvokeAsync<object>("RunCarousel");
        }


        public DateTime SelectDate { get; set; } = DateTime.Now;
        /// <summary>
        /// This method is called when the selected date changes.
        /// </summary>
        protected void OnSelectedDateChanged(DateTime selectedDate)
        {
            SelectDate = selectedDate;
        }
    }
}
