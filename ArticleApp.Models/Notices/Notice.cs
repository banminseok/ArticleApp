using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArticleApp.Models
{
    /// <summary>
    /// [2] Model Class: Notice 모델 클래스 == Notices 테이블과 일대일로 매핑
    /// Notice, NoticeModel, NoticeViewModel, NoticeBase, NoticeDto, NoticeEntity, NoticeObject, ...
    /// </summary>
    [Table("Notices")]
    public class Notice
    {
        /// <summary>
        /// 일련 번호(Serial Number)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// /// 외래키 ParentId, AppId, SiteId, ...
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 이름, 작성자
        /// </summary>
        [Required(ErrorMessage = "이름을 입력하세요.")]
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// 제목
        /// </summary>
        //[MaxLength(255)]
        //[Column(TypeName = "NVarChar(255)")]
        public string Title { get; set; }

        /// <summary>
        /// 내용
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 상단글로 고정: IsPinned, IsNotice, IsTop, IsHead, IsGlobal, IsImportant, IsFeatured, IsHot, IsSticky, ...
        /// </summary>
        public bool? IsPinned { get; set; } = false;

        /// <summary>
        /// 등록자(Creator)
        /// </summary>
        //[MaxLength(255)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 등록일(PostDate)
        /// </summary>
        /// //public DateTimeOffset Created { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;

        /// <summary>
        /// 수정자: LastModifiedBy, ModifiedBy
        /// </summary>
        [MaxLength(255)]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// 수정일: LastModified, Modified
        /// </summary>
        public DateTime? Modified { get; set; }
    }
}
