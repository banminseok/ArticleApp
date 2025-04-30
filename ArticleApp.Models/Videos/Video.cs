using Dul.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    /// <summary>
    /// [1] 모델 클래스: Video 모델 클래스 == Videos 테이블과 일대일로 매핑
    /// Video, VideoModel, VideoViewModel, VideoBase, VideoDto, VideoEntity, ..
    /// </summary>
    public class Video 
    {
        /// <summary>
        /// 일련번호
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 동영상 제목
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 동영상 제공 URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 동영상 작성자
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 회사
        /// </summary>
        public string Company { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public string? ModifiedBy { get; set; }

        public DateTime? Modified { get; set; }
    }
}
