using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;

namespace Hawaso.Pages.Administrations.Roles
{
    public partial class RoleDetails
    {
        [Parameter]
        public string? Id { get; set; }
        [Inject]
        public RoleManager<IdentityRole> RoleManager { get; set; } = default!;
        [Inject]
        public NavigationManager NavigationManagerRef { get; set; } = default!;
        [Inject]
        public IJSRuntime JSRuntime { get; set; } = default!;

        private IdentityRole? model = new IdentityRole();

        protected override async Task OnInitializedAsync()
        {
            model = await RoleManager.FindByIdAsync(Id ?? string.Empty);
            if (model == null)
            {
                //await JSRuntime.InvokeVoidAsync("alert", "Role not found.");
                NavigationManagerRef.NavigateTo("/administrations/roles");
            }
            else
            {
                model.ConcurrencyStamp = Guid.NewGuid().ToString();
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && model == null)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Role not found.");
                NavigationManagerRef.NavigateTo("/administrations/roles");
            }
        }
    }
}
