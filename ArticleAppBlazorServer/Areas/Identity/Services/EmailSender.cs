using Microsoft.AspNetCore.Identity.UI.Services;

namespace ArticleAppBlazorServer.Areas.Identity.Services
{
    public class EmailSender : IEmailSender
    {
        //18_02_IEmailSender 인터페이스를 구현하는 EmailSender 클래스 생성 및 종속성 주입 적용하기
        //https://www.youtube.com/watch?v=yfK7BNyxBTk&list=PLO56HZSjrPTAS3bC6UUNWBH9ih5yujpvS&index=72
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
