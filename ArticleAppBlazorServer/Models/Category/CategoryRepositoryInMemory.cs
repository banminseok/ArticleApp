using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleAppBlazorServer.Models
{
    public class CategoryRepositoryInMemory : ICategoryRepository
    {
        public List<Category> GetCategories()
        {
            // 컬렉션 이니셜라이저를 사용하여 Category 리스트 객체를 생성합니다.
            var categories = new List<Category>
            {
                new Category { Id = 1, CategoryName = "Technology" },
                new Category { Id = 2, CategoryName = "Health" },
                new Category { Id = 3, CategoryName = "Lifestyle" },
                new Category { Id = 4, CategoryName = "Travel" },
                new Category { Id = 5, CategoryName = "Food" }
            };

            return categories;
        }
    }
}
