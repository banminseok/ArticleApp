using Dul.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models.Categories
{
    /// <summary>
    /// 리포지토리 인터페이스 => BREAD SHOP 패턴 사용
    /// </summary>
    public interface ICategoryBaseRepository : IBreadShop<CategoryBase>
    {
        // 로그 메서드 추가
        void Log(string message);
    }
}
