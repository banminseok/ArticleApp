using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    public class CandidateIncome
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50)]
        public string? Source { get; set; }

        public decimal? Amount { get; set; }

        public string? UserId { get; set; } = null;
    }

}
