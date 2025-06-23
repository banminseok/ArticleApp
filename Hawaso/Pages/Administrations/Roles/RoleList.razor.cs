using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace Hawaso.Pages.Administrations.Roles
{
    public partial class RoleList
    {
        [Inject]
        public RoleManager<IdentityRole> RoleManager { get; set; }

        private List<IdentityRole> models;

        protected override async Task OnInitializedAsync()
        {
            models = RoleManager.Roles.OrderBy(m => m.Name).ToList();
            await Task.CompletedTask;
        }
    }
}
