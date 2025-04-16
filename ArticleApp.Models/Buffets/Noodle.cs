using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models.Buffets
{
    /// <summary>
    /// 국수
    /// 09__03_01_Models_Broth, Noodle, Garnish 클래스 추가
    /// https://www.youtube.com/watch?v=ECO7pxJMizM&list=PLO56HZSjrPTAS3bC6UUNWBH9ih5yujpvS&index=9
    /// </summary>
    public class Noodle
    {
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string? Name { get; set; }

        [Required]
        public int? BrothId { get; set; }

        [ForeignKey("BrothId")] // Foreign Key
        public Broth? Broth { get; set; }
    }
}
