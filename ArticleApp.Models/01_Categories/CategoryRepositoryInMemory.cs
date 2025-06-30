using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models.Categories
{
    public class CategoryRepositoryInMemory 
    {
        public List<Category> GetCategories()
        {
            // 컬렉션 이니셜라이저를 사용하여 Category 리스트 객체를 생성합니다.
            var categories = new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "Technology" },
                new Category { CategoryId = 2, CategoryName = "Health" },
                new Category { CategoryId = 3, CategoryName = "Lifestyle" },
                new Category { CategoryId = 4, CategoryName = "Travel" },
                new Category { CategoryId = 5, CategoryName = "Food" }
            };

            return categories;
        }
    }
}
