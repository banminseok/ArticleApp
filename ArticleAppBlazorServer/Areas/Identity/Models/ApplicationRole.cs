using Microsoft.AspNetCore.Identity;

namespace ArticleAppBlazorServer.Areas.Identity.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
