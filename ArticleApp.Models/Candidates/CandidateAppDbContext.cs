using ArticleApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    public class CandidateAppDbContext : DbContext
    {
        public CandidateAppDbContext() : base()
        {
            // Empty
            // 만약, Repository 클래스에 생성자 매개 변수 주입 방식 사용시 이 생성자 제거 
        }

        public CandidateAppDbContext(DbContextOptions<CandidateAppDbContext> options)
            : base(options)
        {
            // Empty
        }

        //15__04_04_DbContext에 Table 속성 추가 및 DbContext 등록 후 데이터베이스 업데이트
        //https://www.youtube.com/watch?v=sciU57zyqe0&list=PLO56HZSjrPTAS3bC6UUNWBH9ih5yujpvS&index=15
        // DbSet of T 형태의 컬렉션 속성을 사용하여 모델(도메인)에 해당하는 테이블 생성
        public DbSet<CandidatesIncome> Candidates { get; set; } = null!;

        public DbSet<CandidateName> CandidatesNames { get; set; } = null!;
        public DbSet<CandidateIncome> CandidatesIncomes { get; set; } = null!;

        //public DbSet<CandidateIncome> CandidatesIncomes { get; set; } = null!;

        /// <summary>
        /// 모델(테이블)이 생성될때 처음 실행 되는 메서드/ 마이그레이션 할때.... 쓸일없겠다.
        /// 16___04_05_SeedData_모델 생성될 때 기본 데이터를 테이블에 입력하기
        /// https://www.youtube.com/watch?v=zzkBeE0c2-U&list=PLO56HZSjrPTAS3bC6UUNWBH9ih5yujpvS&index=16
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            /*
            base.OnModelCreating(builder);
            builder.Entity<Candidate>().HasData(
                new Candidate() { Id=1, FirstName="Ban", IsEnrollment=true},
                new Candidate() { Id=2, FirstName="Test", IsEnrollment=false},
                );
            */
        }
    }
}
