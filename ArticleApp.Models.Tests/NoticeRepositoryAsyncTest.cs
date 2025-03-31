using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ArticleApp.Models.Tests
{
    [TestClass]
    public class NoticeRepositoryAsyncTest
    {
        [TestMethod]
        public async Task NoticeRepositoryAsyncAllMethodTest()
        {
            #region [0] DbContextOptions<T> Object Creation and ILoggerFactory Object Creation
            //[0] DbContextOptions<T> Object Creation and ILoggerFactory Object Creation
            var options = new DbContextOptionsBuilder<ArticleAppDbContext>()
                .UseInMemoryDatabase(databaseName: $"NoticeApp{Guid.NewGuid()}").Options;
            //.UseSqlServer("server=(localdb)\\mssqllocaldb;database=NoticeApp;integrated security=true;").Options;

            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            #endregion

            #region [1] AddAsync() Method Test
            //[1] AddAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                context.Database.EnsureCreated(); // 데이터베이스가 만들어져 있는지 확인

                //[A] Arrange
                var repository = new NoticeRepositoryAsync(context, factory);
                var model = new Notice { Name = "[1] 관리자", Title = "공지사항입니다.", Content = "내용입니다." };

                //[B] Act
                await repository.AddAsync(model); // Id: 1
                await context.SaveChangesAsync(); // 메모리 DB 사용 시 필요
            }
            using (var context = new ArticleAppDbContext(options))
            {
                //[C] Assert
                Assert.AreEqual(1, await context.Notices.CountAsync());
                var model = await context.Notices.Where(n => n.Id == 1).SingleOrDefaultAsync();
                Assert.AreEqual("[1] 관리자", model.Name);
                Console.WriteLine($"[1] model: {JsonSerializer.Serialize(model)}");
            }
            #endregion

            #region [2] GetAllAsync() Method Test
            //[2] GetAllAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // 트랜잭션 관련 코드는 InMemoryDatabase 공급자에서는 지원 X
                //using (var transaction = context.Database.BeginTransaction()) { transaction.Commit(); }
                //[A] Arrange
                var repository = new NoticeRepositoryAsync(context, factory);
                var model = new Notice { Name = "[2] 홍길동", Title = "it is a notice.", Content = "내용입니다." };

                //[B] Act
                await repository.AddAsync(model); // Id: 2
                await context.SaveChangesAsync(); // 메모리 DB 사용 시 필요
                await repository.AddAsync(new Notice { Name = "[3] 백두산", Title = "공지사항입니다.", Content = "내용입니다." }); // Id: 3
                await context.SaveChangesAsync(); // 메모리 DB 사용 시 필요
                await repository.AddAsync(new Notice { Name = "[4] turtle", Title = "바다거북.", Content = "a turtle 명사." }); // Id: 4
                await context.SaveChangesAsync(); // 메모리 DB 사용 시 필요
                await repository.AddAsync(new Notice { Name = "[5] crawle", Title = "(업드려서)기어가다.", Content = "동사, 명사 a crawle." }); // Id: 5
                await context.SaveChangesAsync(); // 메모리 DB 사용 시 필요
            }
            using (var context = new ArticleAppDbContext(options))
            {
                //[C] Assert
                var repository = new NoticeRepositoryAsync(context, factory);
                var models = await repository.GetAllAsync();
                Assert.AreEqual(5, models.Count()); // TotalRecords: 5
                Console.WriteLine($"[2] models.Count: {models.Count()}");
            }
            #endregion

 
            #region [3] GetByIdAsync() Method Test
            //[3] GetByIdAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Empty
            }
            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new NoticeRepositoryAsync(context, factory);
                var model = await repository.GetByIdAsync(2);
                Assert.IsTrue(model.Name.Contains("길동"));
                Assert.AreEqual("[2] 홍길동", model.Name);
            }
            #endregion
            
            #region [4] EditAsync() Method Test
            //[4] EditAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Empty
            }
            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new NoticeRepositoryAsync(context, factory);
                var model = await repository.GetByIdAsync(2);

                model.Name = "[2] 임꺽정"; // Modified
                await repository.EditAsync(model);
                await context.SaveChangesAsync(); // 메모리 DB 사용 시 필요

                var updateModel = await repository.GetByIdAsync(2);

                Assert.IsTrue(updateModel.Name.Contains("꺽정"));
                Assert.AreEqual("[2] 임꺽정", updateModel.Name);
                Assert.AreEqual("[2] 임꺽정",
                    (await context.Notices.Where(m => m.Id == 2).SingleOrDefaultAsync())?.Name);
            }
            #endregion

            #region [5] DeleteAsync() Method Test
            //[5] DeleteAsync() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Empty
            }
            using (var context = new ArticleAppDbContext(options))
            {
                var repository = new NoticeRepositoryAsync(context, factory);
                await repository.DeleteAsync(2);
                await context.SaveChangesAsync(); // 메모리 DB 사용 시 필요

                Console.WriteLine($"[5] context.Notices.Count: {await context.Notices.CountAsync()}");
                Assert.AreEqual(4, (await context.Notices.CountAsync()));
                Assert.IsNull(await repository.GetByIdAsync(2));
            }
            #endregion

            #region [6] GetAllAsync(PagingAsync)() Method Test
            //[6] GetAllAsync(PagingAsync)() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                // Empty
            }
            using (var context = new ArticleAppDbContext(options))
            {
                int pageIndex = 0;
                int pageSize = 1;

                var repository = new NoticeRepositoryAsync(context, factory);
                var noticesSet = await repository.GetAllAsync(pageIndex, pageSize);

                var firstName = noticesSet.Records.FirstOrDefault()?.Name;
                var recordCount = noticesSet.TotalRecords;

                Console.WriteLine($"[5] firstName: {firstName}");
                Assert.AreEqual("[5] crawle", firstName);
                Assert.AreEqual(4, recordCount);
            }
            #endregion

            #region [7] GetStatus() Method Test
            //[7] GetStatus() Method Test
            using (var context = new ArticleAppDbContext(options))
            {
                int parentId = 1;

                var no1 = await context.Notices.Where(m => m.Id == 1).SingleOrDefaultAsync();
                no1.ParentId = parentId;
                no1.IsPinned = true; // Pinned

                context.Entry(no1).State = EntityState.Modified;
                context.SaveChanges();

                var repository = new NoticeRepositoryAsync(context, factory);
                var r = await repository.GetStatus(parentId);

                Assert.AreEqual(1, r.Item1); // Pinned Count == 1
            }
            #endregion
        }
    }
}
