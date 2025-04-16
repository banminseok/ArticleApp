using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models.Buffets
{
    /// <summary>
    /// 고명 
    /// </summary>
    public class Garnish
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string? Name { get; set; }

        public int? NoodleId { get; set; }

        public Noodle? Noodle { get; set; }
    }
}
