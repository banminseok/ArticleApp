using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ArticleApp.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArticleApp.Models.Categories;

namespace ArticleApp.Models.Tests
{
    [TestClass]
    public class CategoryRepositoryAsyncTest
    {
        [TestMethod]
        public async Task CategoryRepositoryAsyncAllMethodTest()
        {
            // DbContextOptions<T> Object Creation
            var options = new DbContextOptionsBuilder<ArticleAppDbContext>()
                .UseInMemoryDatabase(databaseName: $"DotNetSaleCore{Guid.NewGuid()}").Options;
            //.UseSqlServer("server=(localdb)\\mssqllocaldb;database=DotNetSaleCore;integrated security=true;").Options;

            // ILoggerFactory Object Creation
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();

            //[1] AddAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Repository Object Creation
                //[!] Arrange
                var repository = new CategoryRepository(context, factory);
                var model = new Category { CategoryName = "[1] 카테고리이름" };

                //[!] Act
                await repository.AddAsync(model);
                await context.SaveChangesAsync();
            }
            using (var context = new ArticleAppDbContext(options))
            {
                //[!] Assert
                Assert.AreEqual(1, await context.Categories.CountAsync());
                var model = await context.Categories.Where(m => m.CategoryId == 1).SingleOrDefaultAsync();
                Assert.AreEqual("[1] 카테고리이름", model?.CategoryName);
            }

            //[2] GetAllAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                //// 트랜잭션 관련 코드는 InMemoryDatabase 공급자에서는 지원 X
                ////using (var transaction = context.Database.BeginTransaction()) { transaction.Commit(); }
                var repository = new CategoryRepository(context, factory);
                var model = new Category { CategoryName = "[2] 책" };
                await context.Categories.AddAsync(model);
                await context.SaveChangesAsync(); //[1]
                await context.Categories.AddAsync(new Category { CategoryName = "[3] 강의" });
                await context.SaveChangesAsync(); //[2]
            }
            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new CategoryRepository(context, factory);
                var models = await repository.GetAllAsync();
                Assert.AreEqual(3, models.Count);
            }

            //[3] GetByIdAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Empty
            }
            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new CategoryRepository(context, factory);
                var model = await repository.GetByIdAsync(2);
                Assert.IsTrue(model.CategoryName.Contains("책"));
                Assert.AreEqual("[2] 책", model.CategoryName);
            }

            //[4] GetEditAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Empty
            }
            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new CategoryRepository(context, factory);
                var model = await repository.GetByIdAsync(2);
                model.CategoryName = "[2] 컴퓨터";
                await repository.EditAsync(model);
                await context.SaveChangesAsync();

                Assert.AreEqual("[2] 컴퓨터",
                    (await context.Categories.Where(m => m.CategoryId == 2).SingleOrDefaultAsync()).CategoryName);
            }

            //[5] GetDeleteAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Empty
            }
            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new CategoryRepository(context, factory);
                await repository.DeleteAsync(2);
                await context.SaveChangesAsync();

                Assert.AreEqual(2, await context.Categories.CountAsync());
                Assert.IsNull(await repository.GetByIdAsync(2));
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

                var repository = new CategoryRepository(context, factory);
                var models = await repository.GetAllAsync(pageIndex, pageSize);

                Assert.AreEqual("[3] 강의", models.Records.FirstOrDefault().CategoryName);
                Assert.AreEqual(2, models.TotalRecords);
            }
        }
    }
}
