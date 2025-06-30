using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    /// <summary>
    /// 카테고리 모델 클래스 : Categories의 기본 클래스
    /// </summary>
    [Table("CategoriesBases")]
    public class CategoryBase
    {
        /// <summary>
        /// 카테고리 ID
        /// </summary>
        [Key]
        public int CategoryId { get; set; }

        /// <summary>
        /// 카테고리 이름
        /// </summary>
        [Required(ErrorMessage = "카테고리 이름은 필수입니다.")]
        public string CategoryName { get; set; }

    }
}
