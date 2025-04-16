using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models.Buffets
{
    /// <summary>
    /// 국물
    /// </summary>
    public class Broth
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(25)] // [StringLength(25)]
        public string? Name { get; set; }

        public bool IsVegan { get; set; }

        public List<Noodle> Noodles { get; set; } = new();
    }
}
