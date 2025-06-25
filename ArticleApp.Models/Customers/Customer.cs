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
    /// 고객(Customer) 모델 클래스: Customers 테이블과 일대일로 매핑 
    /// </summary>
    [Table("Customers")]
    public class Customer
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        [Key]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "고객명을 입력하세요.")]
        [Column(TypeName = "NVarChar(50)")]
        public string CustomerName { get; set; }

        //[Required(ErrorMessage = "이메일을 입력하세요.")]
        public string EmailAddress { get; set; }

        public string? Address { get; set; }= string.Empty;
        public string? AddressDetail { get; set; } = string.Empty;
        public string? Phone1 { get; set; } = string.Empty;
        public string? Phone2 { get; set; } = string.Empty;
        public string? Phone3 { get; set; } = string.Empty;
        public string? Mobile1 { get; set; } = string.Empty;
        public string? Mobile2 { get; set; } = string.Empty;
        public string? Mobile3 { get; set; } = string.Empty;
        public string? Zip { get; set; } = string.Empty;
        public string? Ssn1 { get; set; } = string.Empty;
        public string? Ssn2 { get; set; } = string.Empty;
        public int? MemberDivision { get; set; } = 0;

        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Gender { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;

        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? Created { get; set; }
        public string? ModifiedBy { get; set; } = string.Empty;
        public DateTime? Modified { get; set; }
    }

    public class CustomerViewModel
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        [Required(ErrorMessage = "고객명을 입력하세요.")]
        public string CustomerName { get; set; }

        public string EmailAddress { get; set; }
    }
}
