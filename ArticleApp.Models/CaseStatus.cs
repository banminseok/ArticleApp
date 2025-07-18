using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    [Table("CaseStatus")]
    public class CaseStatus
    {
        [Key]
        public int Id { get; set; }

        //Class 명을 프로퍼티명으로 사용못해서 ColumnAttribute를 사용
        [Display(Name = "Status Name")]
        [Column("CaseStatus")]
        public string CaseStatusName { get; set; }

        public bool Active { get; set; }
    }
}
