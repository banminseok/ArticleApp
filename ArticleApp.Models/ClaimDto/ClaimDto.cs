using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models.ClaimDto
{
    #region DTO

    /// <summary>
    /// 사용자 정보 DTO 클래스 Data Transfer Object Class
    /// </summary>
    public class ClaimDto
    {
        [Required]
        public string? Type { get; set; }  //prop tab tab
        [Required]
        public string? Value { get; set; }
    }
    #endregion
}
