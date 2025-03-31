using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models.Tests
{
    /// <summary>
    /// [!] Test Class: (Arrange -> Act -> Assert) Pattern
    /// 필요한 NuGet 패키지: Install-Package Microsoft.EntityFrameworkCore.InMemory
    /// </summary>
    [TestClass]
    public class ArticleRepositoryTest
    {
        // Install-Package Microsoft.EntityFrameworkCore.SqlServer
        // Install-Package Microsoft.EntityFrameworkCore.InMemory

        [TestMethod]
        public async Task ArticleRepositoryAllMethodTest()
        {
            //[0] DbContextOptions<T> Object Creation
            var options = new DbContextOptionsBuilder<ArticleAppDbContext>()
                .UseInMemoryDatabase(databaseName: $"ArticleApp{Guid.NewGuid()}").Options; // 메모리 DB 사용
                                                                                           //.UseSqlServer("server=(localdb)\\mssqllocaldb;database=ArticleApp;integrated security=true;").Options;

            //[1] AddAsync() Method Test
            //[1][1] Repository 클래스를 사용하여 저장
            using (var context = new ArticleAppDbContext(options))
            {
                // Repository 클래스 생성
                var repository = new ArticleRepository(context);
                //var model = new Article { Title = "[1] 게시판 시작", Created = DateTime.Now };
                var model = new Article { Title = "[1] 게시판 시작", Content="Article 컨텐츠 내용", Created = DateTime.Now };
                await repository.AddArticleAsync(model);
                await context.SaveChangesAsync(); // 메모리 DB 사용 시 필요
            }

            //[1][2] DbContext 클래스를 통해서 개수 및 레코드 확인 
            using (var context = new ArticleAppDbContext(options))
            {
                //[!] Assert
                Assert.AreEqual(1, await context.Articles.CountAsync());

                var model = await context.Articles.Where(m => m.Title == "[1] 게시판 시작").SingleOrDefaultAsync();
                //var model = await context.Articles.Where(m => m.Id == 1).SingleOrDefaultAsync();
                //var model = await context.Articles.FirstOrDefaultAsync();
                Assert.AreEqual("[1] 게시판 시작", model?.Title); 
            }

            //[2] GetAllAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                try
                {
                    // 트랜잭션 관련 코드는 InMemoryDatabase 공급자에서는 지원 X
                    // using (var transaction = context.Database.BeginTransaction()) { transaction.Commit(); }

                    var repository = new ArticleRepository(context);
                    var model = new Article { Title = "[2] 게시판 가동", Content = "Article 컨텐츠 내용" };
                    await context.Articles.AddAsync(model);
                    await context.SaveChangesAsync(); //[1]
                    await context.Articles.AddAsync(new Article { Title = "[3] 게시판 중지", Content = "Article 컨텐츠 내용" });
                    await context.SaveChangesAsync(); //[2]
                }
                catch (Exception e)
                {
                    //Empty
                }
            }
            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new ArticleRepository(context);
                var models = await repository.GetArticlesAsync();
                Assert.AreEqual(3, models.Count);
            }

            //[3] GetByIdAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Empty
            }
            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new ArticleRepository(context);
                var model = await repository.GetArticleByIdAsync(2);
                Assert.IsTrue(model.Title.Contains("가동"));
                Assert.AreEqual("[2] 게시판 가동", model.Title);
            }

            //[4] EditAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Empty
            }
            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new ArticleRepository(context);
                var model = await repository.GetArticleByIdAsync(2);
                model.Title = "[2] 게시판 바쁨";
                await repository.EditArticleAsync(model);
                await context.SaveChangesAsync(); //생약가능 - 저장 시점을 코드로 표현하기 위함

                Assert.AreEqual("[2] 게시판 바쁨", (await repository.GetArticleByIdAsync(2)).Title);

            }

            //[5] DeleteAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Empty
            }
            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new ArticleRepository(context);
                await repository.DeleteArticleAsync(2);
                await context.SaveChangesAsync(); //생략가능 - 저장 시점을 코드로 표현하기 위함
                Assert.AreEqual(2, await context.Articles.CountAsync());
                Assert.IsNull(await repository.GetArticleByIdAsync(2));  //2번은 삭제되었으므로 null
            }

            //[6] PagingAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Empty
            }
            using (var context = new ArticleAppDbContext(options))
            {
                int pageIndex = 0;
                int pageSize = 1;
                var repository = new ArticleRepository(context);
                var models = await repository.GetAllAsync(pageIndex, pageSize);

                Assert.AreEqual("[3] 게시판 중지", models.Records.FirstOrDefault().Title); // 3번 게시물, DESC
                Assert.AreEqual(2, models.TotalRecords);
            }
        }
    }
}
