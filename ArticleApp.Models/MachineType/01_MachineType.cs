using System.ComponentModel.DataAnnotations;

namespace ArticleApp.Models
{
    public class MachineType
    {
        public int Id { get; set; }

        // PM> Install-Package System.ComponentModel.Annotations
        [Required]
        public string Name { get; set; }
    }
}
