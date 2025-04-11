using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    // Install-Package Microsoft.EntityFrameworkCore
    // Install-Package Microsoft.EntityFrameworkCore.SqlServer
    // Install-Package Microsoft.EntityFrameworkCore.Tools
    // Install-Package Microsoft.EntityFrameworkCore.InMemory
    // Install-Package System.Configuration.ConfigurationManager
    // Install-Package Microsoft.Data.SqlClient

    /// <summary>
    /// DbContext Class: Entity Framework Core
    /// </summary>
    public class ArticleAppDbContext : DbContext
    {
        public ArticleAppDbContext()
        {
            // Empty
            // ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;        
        }
        public ArticleAppDbContext(DbContextOptions<ArticleAppDbContext> options) : base(options)
        {
            // 공식과 같은 코드
            // ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 닷넷 프레임워크 기반에서 호출되는 코드 영역: 
            // App.config 또는 Web.config의 연결 문자열 사용
            // 직접 데이터베이스 연결문자열 설정 가능
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = ConfigurationManager.ConnectionStrings[
                    "ConnectionString"].ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
                */
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Articles 테이블의 Created 열은 자동으로 GetDate() 제약 조건을 부여하기 
            modelBuilder.Entity<Notice>().Property(m => m.Created).HasDefaultValueSql("GetDate()");
            modelBuilder.Entity<Upload>().Property(m => m.Created).HasDefaultValueSql("GetDate()");
            modelBuilder.Entity<Upload>().Property(m => m.Title).IsRequired().HasDefaultValue("");
            modelBuilder.Entity<Upload>().Property(m => m.Content).IsRequired().HasDefaultValue("");
        }

        //[!] ArticleApp 솔루션 관련 모든 테이블에 대한 참조 
        public DbSet<Article> Articles { get; set; }

        public DbSet<Notice> Notices { get; set; }
        public DbSet<Upload> Uploads { get; set; }

    }
}
