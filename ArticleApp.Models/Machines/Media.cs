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
    /// [2] Model Class: Media 모델 클래스 == Medias 테이블과 일대일로 매핑
    /// Media, MediaModel, MediaViewModel, MediaBase, MediaDto, MediaEntity, ...
    /// </summary>
    public class Media 
    {
        /// <summary>
        /// Serial Number
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Hardware Name
        /// </summary>
        [Required]
        public string Name { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public string? ModifiedBy { get; set; }

        public DateTime? Modified { get; set; }
    }
}
