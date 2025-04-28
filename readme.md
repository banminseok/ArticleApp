#    

https://www.youtube.com/playlist?list=PLO56HZSjrPTC1c1MbY72vVT0sO33G-9Od
git :	01 https://github.com/VisualAcademy/articleapp
		02 https://github.com/VisualAcademy/NoticeApp.DevLec
		03 https://github.com/VisualAcademy/UploadApp
		04 https://github.com/VisualAcademy/ReplyApp
        05 https://github.com/VisualAcademy/hawaso
        06 https://github.com/VisualAcademy/DotNetNote
        07 https://github.com/VisualAcademy/BlazorApp/blob/87af5801ff52da0a15bd7cf834565bfd3e85f64e/BlazorApp/Pages/Samples/SignaturePadDemo.razor#L4

        21 https://github.com/AdrienTorris/awesome-blazor
        22 https://github.com/SamProf/MatBlazor

@VisualAcademy
�ȳ��ϼ���. �ڿ��� �����Դϴ�.
2024�� Blazor �н� ������ ���� ������� �н��Ͻø� �˴ϴ�. 
ASP.NET Core 8.0�� �� ������Ʈ ���ø��� ��������� ����Ǿ�, �켱 ASP.NET Core 8.0 Start�� ���� ���ð�,
1.1�� 1.2 ���´� �Բ� �����ؼ� ���ø鼭, ���ķ� ��Ʈ 2�� ��Ʈ 3�� �̾����� ���¸� ���ø� �˴ϴ�. 
����� �� ������� ���ô°� ���� �����ϴ�. ���ķ��� ���´� ���� �����ּ���. �����մϴ�. 

0. ASP.NET Core 8.0 Start
https://www.youtube.com/playlist?list=PLO56HZSjrPTCffK881uMGdjT3Tc456pmg

1.1. Blazor Server 7.0 Fundamentals

https://www.youtube.com/playlist?list=PLO56HZSjrPTAS3bC6UUNWBH9ih5yujpvS

1.2. Blazor Part 1 ����
https://www.youtube.com/playlist?list=PLO56HZSjrPTCQbxR12t-67YyNSBP8L-8v

2. Blazor Part 2_Blazor �Խ��� ������Ʈ
https://www.youtube.com/playlist?list=PLO56HZSjrPTC1c1MbY72vVT0sO33G-9Od

3. Blazor Part 3: ȸ�� Ȩ������ �����
https://www.youtube.com/playlist?list=PLO56HZSjrPTA-EJxyqiN8HzItM7nDEb11


# 0. ASP.NET Core 8.0 Start
- https://www.dul.me/docs/aspnet/core/start/?tabs=aspnetcore-8%2Cvisualstudio
- https://learn.microsoft.com/ko-kr/shows/lets-learn-dotnet/lets-learn-dotnet-auth-and-identity?WT.mc_id=DT-MVP-35766
- ASP.NET Core 8.0 Web API ������ : https://www.youtube.com/watch?v=_M7rUwutFOc&list=PLO56HZSjrPTCffK881uMGdjT3Tc456pmg&index=22
- GitHub: https://github.com/VisualAcademy
- C# 11.0 ���� ���ڿ� ���ͷ� : https://youtu.be/7l8S6Husiso
- https://www.hawaso.com/
- appsettings.json�� secrets.json ����==> ��Ŭ�� >����� ��ȣ���� (*����ȯ�濡�� �� ���밡��*)
- ASP.NET Core 6.0 LTS ������ 4���� �����ӿ�ũ�� �ϳ��� ������Ʈ�� �����Ͽ� �����ϱ� >>> https://www.youtube.com/watch?v=zrLYE5NJd8U&list=PLO56HZSjrPTCffK881uMGdjT3Tc456pmg&index=27
- @rendermode InteractiveServer (https://www.youtube.com/watch?v=ot5pG0P1nII&list=PLO56HZSjrPTCffK881uMGdjT3Tc456pmg&index=28)
- global using globalusing.cs
- @inject IJSRuntime js
- var variables = ViewBag.Variables as List<Variable>;
- public AppSettingsDemoController(IConfiguration configuration)

# 1.1. Blazor Server 7.0 Fundamentals
- ��Ű���������ܼ� > Add-Migration BuffetModelAdd,              update-database
- ��Ű���������ܼ� > Add-Migration BuffetModelAdd -Context Hawaso.Models.Candidates.CandidateAppDbContext,
  update-database -Context Hawaso.Models.Candidates.CandidateAppDbContext,
- https://github.com/VisualAcademy/VisualAcademy/blob/4ba3026d773a00e143f1216419defea2cd41e59e/src/VisualAcademy/VisualAcademy/Models/Candidates/01_Candidate.cs
- null!; (null ������� ����)
- Blazor Server ���� ���� DBContext ���� ���ǻ���....
  //[a] MVC, RazorPages, Web API������ DbContext ��� ����
  //builder.Services.AddDbContext<CandidateAppDbContext>(options => options.UseSqlServer(connectionString));
  //[b] Blazor Server������ DbContextFactory ��� ����
  builder.Services.AddDbContextFactory<CandidateAppDbContext>(options => options.UseSqlServer(connectionString));
  // mvc �������� Blazor ������Ʈ�� 2�� �̻� ����ϱ� ���ؼ��� DbContextFactory�� ����ؾ� �Ѵ�.
  // �ϳ��� Blazor �������� DbContext�� 2�� �̻� ����ϸ�, Blazor������Ʈ�� 2���̻� ����ϸ�  ������ �߻��Ѵ�.
- OnInValidSubmit  /// Form ��ȿ�� �˻� ���� �� ó��
- AsNoTracking (�б��������� ������� �ɼ�) await ctx.Candidates.AsNoTracking().FirstOrDefaultAsync(it => it.Id == Id);
- 17_01_Script-Migration ����� ����Ͽ� �����ͺ��̽� ��ü�� ���� SQL ��ũ��Ʈ�� �̾Ƴ���
  ��Ű���������ܼ� > Script-Migration -Context Hawaso.Models.Candidates.CandidateAppDbContext
- 17_03_���� ����ϴ� ���� ���ӽ����̽����� _Imports ���Ͽ� ��Ƽ� �����ϱ�
- 17_04_namespace ���ù��� ����Ͽ� ������Ʈ�� ���ӽ����̽��� ���������� �����ϱ�
  namespace �����ϱ� : @namespace AriticleApp.Pages.Candidates
- compoent�ױ� ����ؼ� mvc�� razor �������� �����Ҷ�, ���ǻ���
    �ϴ� ���̾ƿ� ������ blazor.server.js ���� Ȯ��
    ���<base href="~/" /> ����Ȯ��
- ASP.NET Core ���̾ƿ��� Blazor Server Layout�� �ϳ��� �����ؼ� ����ϱ�
  https://www.youtube.com/watch?v=CEv0zt_MPbo&list=PLO56HZSjrPTAS3bC6UUNWBH9ih5yujpvS&index=90

# 1.2. Blazor Part 1 ����
- Remaining.ToString("c0") : double�̳� decimal ���� .ToString("c0")�� ����ϸ�, �ش� ���� "��ȭ" ����(�Ҽ��� ����)���� ��ȯ�մϴ�. 
- Input ���ڷ����Ϳ� Output ���ڷ�����
 Input Parameter : �θ𿡼� �ڽ����� , ������Ʈ���� �Ӽ��� ����Ͽ� ������ ����
 Output Parameter : �ڽĿ��� �θ��, 
   Action : �ڱ��� ���� �θ𿡼� ����ϵ���, �ڽĿ��� �θ��� �޼��带 ȣ���ϴ� ����
   EventCallback<T>: ���׸� Ÿ���� ����Ͽ�, number, string�� ���ؼ� �ڽĿ��� �θ�� ���� �����ϴ� ����
- @ref :���ø� ��������, �ڽĿ���� @ref�� ����Ͽ� �θ𿡼� �ڽ��� �޼��带 ȣ���ϴ� ����
- MatBlazor : https://www.matblazor.com/
- 03 04 ChartJs Blaozr ������Ʈ�� ����Ͽ� ���� ������ ��Ʈ�� ������ �׸��� �� �ٽ� ��������..������ ���� �ٸ�..
- https://blazorise.com/docs
- https://startbootstrap.com/themes
- ����Ʈ html ���� : _Host.cshtml, _Layout.cshtml >> MainLayout.razor >>NavMenu.razor_
- Morder_Admin : SBAdmin2 �ٿ�ε� �� ������ ������ (https://www.youtube.com/watch?v=irRHlG1oWxo&list=PLO56HZSjrPTCQbxR12t-67YyNSBP8L-8v&index=28&t=508s)
- 
- 



# DbContextFactory VS DbContext	
Blazor Server���� Repository���� DbContext ����ϴ� ��쿡�� ū �������� ����� �����մϴ�.
���� ��ũ�� �� ���� �ҽ��� Memos ����� �ҽ��� �̹� Repository���� DbContext�� �״�� ȣ���ؼ� ����մϴ�.
https://github.com/VisualAcademy/Hawaso/blob/d5566e578dcfb22ad8f72ff9ef2a68651f35e63a/src/Hawaso.Models/Memos/04_MemoRepository.cs#L21
��, �ֱٿ��� �̿��̸� ���Ӱ� ����� Repository���� DbContextFactory�� ����Ϸ��� ������Դϴ�.
DbContext�� Repository �߰��� DbContextFactory�� �ϳ� �� �δ� �����Դϴ�. 
https://github.com/VisualAcademy/Hawaso/blob/d5566e578dcfb22ad8f72ff9ef2a68651f35e63a/src/Hawaso/Components/Pages/VendorPages/Models/07_VendorRepositoryPermanentDelete.cs#L9

# Dapper�� DbContext
-Dapper�� �淮 ORM����, DbContext�ʹ� ���������� �۵��մϴ�.
-Dapper�� �����ͺ��̽� ����(IDbConnection)�� ���� ����Ͽ� SQL ������ �����մϴ�.
-���� Dapper�� ����� ���� DbContext�� �ƴ� �����ͺ��̽� ���� ��ü�� ���� �����ؾ� �մϴ�.
2. DbContextFactory�� Dapper�� ���
DbContextFactory�� ����Ͽ� DbContext�� ������ ��, Dapper�� �Բ� ����Ϸ��� DbContext���� �����ͺ��̽� ������ �����ؾ� �մϴ�.
using Dapper;
using Microsoft.EntityFrameworkCore;

public async Task<IEnumerable<Candidate>> GetCandidatesAsync(IDbContextFactory<CandidateAppDbContext> contextFactory)
{
    // [1] DbContext ����
    using var context = contextFactory.CreateDbContext();

    // [2] DbConnection ��������
    using var connection = context.Database.GetDbConnection();

    // [3] DbConnection ����
    await connection.OpenAsync();

    // [4] Dapper�� ����Ͽ� SQL ���� ����
    var sql = "SELECT * FROM Candidates ORDER BY FirstName";
    var candidates = await connection.QueryAsync<Candidate>(sql);

    // [5] DbContext�� DbConnection�� using ����� ������ �ڵ����� Dispose() ȣ��
    return candidates;
}
--------------
using var context = _ContextFactory.CreateDbContext();
using var transaction = await context.Database.BeginTransactionAsync();
using var connection = context.Database.GetDbConnection();

await connection.ExecuteAsync("INSERT INTO Candidates (FirstName, LastName) VALUES (@FirstName, @LastName)", new { FirstName = "John", LastName = "Doe" }, transaction.GetDbTransaction());

await context.SaveChangesAsync();
await transaction.CommitAsync();
--------------------


# Nuget��Ű�� ��ġ
  Microsoft.EntityFrameworkCore
  Microsoft.EntityFrameworkCore.SqlServer
  Microsoft.EntityFrameworkCore.Tools
  System.Configuration.ConfigurationManager
  System.Data.SqlClient  (���� : https://ddochea.tistory.com/189)
  Microsoft.EntityFrameworkCore.InMemory is the EF Core
  MatBlazor 
  Charts.Blazor
  BlazorInputFile
  # EPPlus 8(����) => EPPlus 7 �������ε� �ٿ�ε� 
  Blazorise.Bootstrap
  Blazorise.Icons.FontAwesome

  Dapper
  Serilog
  Serilog.Extensions.Logging
  Serilog.Sinks.File
  -------------------------
  controller - ctor(ILogger<HomeController> logger)
  _logger.LogInformation("�α׹���.....");
  -------------------------------------------------------------------------

# UploadApp
���ε� �Խ����ε�, ��뷮ó���� ���� mvc�� ����ϰ�,
Network Services �� ���� �������..
  1. Network Services�� ���� ���� �ο�
  2. IIS���� Application Pool�� Identity�� Network Service�� ����
  3. Network Service�� ���� ���� �ο�
  4. C:\inetpub\wwwroot\UploadApp\bin\Debug\net6.0\UploadApp.dll
  5. C:\inetpub\wwwroot\UploadApp\bin\Debug\net6.0\UploadApp.pdb

  Razor Ŭ���� ���̺귯�� ����
  Razor Class Library
  �ٿ�ε� ��ũ��Ʈ : function saveAsFile(filename, bytesBase64) { js


# --------------------------------------------------------------------------------------------------------------------------------
# ArticleApp �������丮

**�Խ��� ������Ʈ** ���� �Ǵ� ���� �Ǵ� ���� �ҽ� ����

ASP.NET Core 6.0 + Bootstrap 5.0 ��� �ҽ��� ArticleApp �ַ���� VisualAcademy ������Ʈ�� �ֽ��ϴ�. 

   ���ķ� ������Ʈ�Ǵ� ���� ����� ���� ���� �ҽ����� ����˴ϴ�.
   
   * https://github.com/VisualAcademy/VisualAcademy

# Sample Text
Blazorise supports several CSS frameworks, including Bootstrap (known for responsive design elements), Bulma (valued for simplicity and Flexbox base), Material (inspired by Google's Material Design), Ant Design (geared towards enterprise-level products and adapting React components' design principles), and Tailwind CSS (famous for its utility-first approach and versatility). These frameworks provide distinct styles and philosophies, offering developers a range of options to best suit their project's requirements.
A Blazorise commercial license typically includes access to advanced components, priority support, options for dedicated consultations, frequent updates and bug fixes, a license for unrestricted commercial use, potential access to the source code, and opportunities for training and workshops. This package enhances functionality, offers better support, and provides operational security for commercial projects.


# �Խ��� ������Ʈ

## 001. �Խ��� ������Ʈ ����

�ȳ��ϼ���. �ڿ����Դϴ�. �̹� �ð����ʹ� �Խ��� ������Ʈ�� �������� �ϰڽ��ϴ�. �⺻���� ��ɺ��� ��� Ȯ���� ������ ���·� ����˴ϴ�. 

__�Խ���__, __VisualStudio__, __���__

- �ڿ��� ����(https://www.dotnetkorea.com)
- Article: ��ƼŬ, ��, ���
- ArticleApp �ַ��: ArticleApp ������Ʈ, ArticleApp.SqlServer ������Ʈ, ...
- 3Pro: Professionals(People) -> Projects(Processes) -> Products
- GitHub ����: https://github.com/VisualAcademy/ArticleApp.git
- ���� ����: Visual Studio, Visual Studio Code
- ���� ���: C#, .NET, ASP.NET Core, Blazor, SQL Server, ...
- ���� ����: ��� API�� ���ؼ� ��� �񵿱�, ��� �׽�Ʈ, ��� SPA(Single Page Application)


## 002. �ַ�� �� ������Ʈ ���� �׸��� GitHub�� ����

�Խ��� ������Ʈ�� �����ϱ� ���� Visual Studio���� �⺻ �ַ���� �����ϰ� �̸� GitHub�� ���� �������丮�� �Խ��Ͽ� ����� ��� ����� ������ ����� �����մϴ�. 

__�ַ��__, __������Ʈ__, **GitHub** 

- Visual Studio 2019 ���
- ArticleApp �ַ��
  - ArticleApp.Models
  - ArticleApp.Models.Tests
  - ArticleApp.SqlServer 

- GitHub
  - https://github.com/VisualAcademy/ArticleApp.git


## 003. �ּ� ũ���� �𵨰� ���̺� ���� �׸��� ���� �����ͺ��̽� �Խ�

�Խ����� ��ü ����� �����ϱ� ���� ��ü ���븦 �н��� ���� ���� �������� ������ �� Ŭ������ ���̺��� �����ϰ� ���� �����ͺ��̽��� �Խ��մϴ�. 

__��Ŭ����__, __�Խ������̺�__, __���õ����ͺ��̽�__

- Article.cs �� Ŭ����
- Articles.sql ���̺�
- ArticleApp �����ͺ��̽� 


## 004. �������丮 ���ϰ� �������丮 �������̽�

�������丮 ������ ����Ͽ� �Խ��� �����Ϳ� ���� �ֿ� API�� ����ϴ� �������丮 �������̽��� ����� �⺻ �޼��忡 ���� �޼��� �ñ״�ó�� �����մϴ�. 

**Repository**, **�������丮**, **�������丮�������̽�**

- Dul.dll ���� ����
- IArticleRepository.cs
- ArticleRepository.cs (ArticleRepositoryAdoNet, ArticleRepositoryDapper, ArticleRepositoryEfCore, ...)


## 005. Entity Framework Core ���� �� �����ͺ��̽� ���ؽ�Ʈ Ŭ���� ����

�̹� �ð����� ��� ���Ĵٵ� ������Ʈ�� Entity Framework Core ���� NuGet ��Ű���� �߰��ϰ� ��ü �����ͺ��̽��� ������ �� �ִ� �θ� Ŭ������ DbContext Ŭ������ �����մϴ�. 

**NuGet**, **EFCore**, **DbContext**, 

- ArticleDbContext.cs


## 006. �������丮 Ŭ������ �ֿ� �񵿱� �޼��� ��� ����

�̹� �ð����� Entity Framework Core�� ����Ͽ� �������丮 Ŭ������ �ֿ� �񵿱� �޼����� ����� �����մϴ�.

**�������丮**, **EF Core**, **CRUD**

- ArticleRepository.cs


## 007. �׽�Ʈ ������Ʈ���� �������丮 Ŭ������ ��� �񵿱� �޼��� �׽�Ʈ

MSTest ������Ʈ�� �׽�Ʈ ������Ʈ���� �������丮 Ŭ������ ��� �޼��带 �ϳ��� �׽�Ʈ�մϴ�.

__MSTest__, **�׽�Ʈ**, **�����׽�Ʈ**

- ArticleRepositoryTest.cs


## 008. Blazor ������Ʈ ���� �� �Խ��� ���� �⺻ ������ ���� 

���� ���� �������� ���� �ֽ��� �� ���� ����� �������� ����Ͽ� �� ������ ������ �����ϰڽ��ϴ�.

__Blazor__, **������**, __����__


## 009. ���� Ŭ������ DbContext�� Repository Ŭ������ ���� ������ ���� ���� �ڵ带 Startup ���Ͽ� �߰�

Models Ŭ���� ���̺귯���� DbContext�� Repository Ŭ������ ������ ������Ʈ�� ��� �����ϵ��� ������ ���� �ڵ带 �ۼ��մϴ�. 

__DI__, **����������**, __Startup__


## 010. �Խ��� ����Ʈ ������ �ۼ� 

�Խ��ǿ��� ���� ���� �����ϴ� ���� �������� ����Ʈ �������� �ٸ纸���� �ϰڽ��ϴ�. 

 __����Ʈ__, **Index**, __����������__
 
 
 ## 011. �Խ��� �۾��� ������ �ۼ�
 
 �Խ��ǿ� �����͸� �Է��ϴ� �۾��� �������� ��Ʈ��Ʈ���� ����Ͽ� ���� ����� �����͸� �Է��ϴ� �ڵ带 �ۼ��մϴ�.
 
 **�۾���**, **��Ʈ��Ʈ��**, **��**
 
 
 ## 012. �Խ��� �󼼺��� ������ �ۼ�
 
 �Խ��ǿ� �ۼ��� �����Ϳ� ���� �� ������ �� �� �ִ� �󼼺��� �������� �ۼ��մϴ�. 
 
 **��**, **�Խ���**, **Details**
 
 
 ## 013. �Խ��� ���� ������ �ۼ� �� Content �� �߰� �� ���� �ڵ� ����
  
 �Խ��ǿ� �ۼ��� �����͸� ������ �� �ִ� �������� �ۼ��մϴ�. 
 
 **����**, **�Խ���**, **Edit**


 ## 014. �Խ��� ���� ������ �ۼ� �� Ȯ�� ���� ����� IJSRuntime���� ����
  
�Խ��ǿ� �ۼ��� �����͸� ������ �� �ִ� �������� �ۼ��մϴ�. C#���� JavaScript�� �Լ��� ȣ���ϴ� ����� �ٷ�ϴ�.
 
 **����**, **�Խ���**, **Delete**
 

 ## 015. �Խ��� ����Ʈ�� ������ ������Ʈ ���� �� �⺻ ����¡ ����
  
�Խ����� ����Ʈ�� ����Ǿ� ���Ǵ� ������ ������Ʈ�� ����� ����Ʈ �������� �����մϴ�.  
 
 **Pager**, **����¡**, **������**
 

 ## 016. ������ ������Ʈ UI ����� 
  
�Խ����� ����Ʈ�� ����Ǿ� ���Ǵ� ������ ������Ʈ�� UI ����� �ۼ��մϴ�.  
 
 **Pager**, **����¡**, **������**


 ## 017. IsPinned �Ӽ� �߰� �� �����۷� �����ϴ� ����� ��� ������ �����ϱ�
  
���ο� ���� �Ӽ��� �߰��ϰ� ���� C# �ڵ带 ����Ͽ� ��� ���� ���ų� �ݴ� ����� �����մϴ�.  
 
 **Modal**, **��������**, **�����**
 
 
 ## 018. ��Ʈ��Ʈ�� ��� ���� ����Ͽ� ������ ���� ������ ���� ��� ���̾�α� ���� 
  
��Ʈ��Ʈ�� ��� ���� jQuery�� �����Ͽ� ������ ���� ������ ���� ��� ���̾�α׸� �����ϴ� ����� �����ݴϴ�. 
 
 **Modal**, **������**, **�����**
 
 Bootstrap 5 ��� �� �ҽ��� VisualAcademy ������Ʈ�� ����Ǿ� �ֽ��ϴ�. 
 
  
 ## 019. ��Ʈ��Ʈ�� ��� ���� ����Ͽ� ������ ���� ������ �Է� �� ���� ��� �� ���� 
  
��Ʈ��Ʈ�� ��� ���� jQuery�� �����Ͽ� ������ ���� ������ �Է� �� ���� ��� ���̾�α׸� �����ϴ� ����� �����ݴϴ�. 
 
 **Modal**, **������**, **�����**
 
 
 ## 020. �󼼺��� ������ �ٹٲ� ���� �߰� �� �Խ����� UI�� �����ϱ�
  
������ �귣ġ�� �ƴ� ���ο� �귣ġ���� �Խ����� UI�� �����ϴ� �۾��� �����ϰ� �׽�Ʈ�� �Ϸ�Ǹ� ������ �귣ġ�� �����ϸ鼭 ������ �����մϴ�.
 
 **������**, **�귣ġ**, **����**
 
 
 ## 021. 21_�ζ��� �ڵ� ����� �ڵ� �����ε� ������� ����
  
 �Խ��ǿ� �ۼ��� �����͸� ������ �� �ִ� �������� �ۼ��մϴ�. 
 
 **����**, **�Խ���**, **Delete**
 
 
 ## 022. ������ ������Ʈ�� Razor Ŭ���� ���̺귯���� ����� NuGet �������� ����
  
 �Խ��ǿ��� ���Ǵ� ������ ������Ʈ�� NuGet �������� �����մϴ�. 
  
 **NuGet**, **Pager**, **DulPager**


 ## 023. ������ ������Ʈ�� NuGet �������� DulPager�� ��ü
  
NuGet �������� ������ DulPager ������Ʈ�� ����մϴ�. 
  
 **NuGet**, **Pager**, **DulPager**