﻿using Microsoft.EntityFrameworkCore;

namespace DotNetNote.Models
{
    public class DotNetNoteContext : DbContext
    {
        public DotNetNoteContext()
        {
            // Empty
        }

        public DotNetNoteContext(DbContextOptions<DotNetNoteContext> options)
            : base(options)
        {
            // 공식과 같은 코드 
        }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            // 닷넷 프레임워크 기반에서 호출되는 코드 영역: 
            // App.Config 또는 Web.Config의 연결 문자열 사용
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[
                    "ConnectionString"].ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        /*
        /// <summary>
        /// 아이디어 관리자
        /// 아이디어 앱에 대한 참조(Idea 모델 클래스 <=> Ideas 테이블)
        /// </summary>
        public DbSet<Idea> Ideas { get; set; }
        */
        /// <summary>
        /// 게시판
        /// </summary>
        public DbSet<Note> Notes { get; set; }

        /// <summary>
        /// 도메인 관리자 테이블 참조
        /// </summary>
        public DbSet<Url> Urls { get; set; }

        /*
        /// <summary>
        /// 기술 리스트: [실습] Teches 테이블부터 Angular 앱 또는 Blazor 앱까지
        /// </summary>
        public DbSet<Tech> Teches { get; set; }
        */

        public DbSet<Reply> Replys { get; set; }
    }
}
