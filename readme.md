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
안녕하세요. 박용준 강사입니다.
2024년 Blazor 학습 순서는 다음 순서대로 학습하시면 됩니다. 
ASP.NET Core 8.0에 들어서 프로젝트 템플릿이 대대적으로 변경되어, 우선 ASP.NET Core 8.0 Start를 먼저 보시고,
1.1과 1.2 강좌는 함께 병행해서 보시면서, 이후로 파트 2와 파트 3로 이어지는 강좌를 보시면 됩니다. 
현재는 이 순서대로 보시는게 제일 좋습니다. 이후로의 강좌는 따로 문의주세요. 감사합니다. 

0. ASP.NET Core 8.0 Start
https://www.youtube.com/playlist?list=PLO56HZSjrPTCffK881uMGdjT3Tc456pmg

1.1. Blazor Server 7.0 Fundamentals

https://www.youtube.com/playlist?list=PLO56HZSjrPTAS3bC6UUNWBH9ih5yujpvS

1.2. Blazor Part 1 기초
https://www.youtube.com/playlist?list=PLO56HZSjrPTCQbxR12t-67YyNSBP8L-8v

2. Blazor Part 2_Blazor 게시판 프로젝트
https://www.youtube.com/playlist?list=PLO56HZSjrPTC1c1MbY72vVT0sO33G-9Od

3. Blazor Part 3: 회사 홈페이지 만들기
https://www.youtube.com/playlist?list=PLO56HZSjrPTA-EJxyqiN8HzItM7nDEb11


# 0. ASP.NET Core 8.0 Start
- https://www.dul.me/docs/aspnet/core/start/?tabs=aspnetcore-8%2Cvisualstudio
- https://learn.microsoft.com/ko-kr/shows/lets-learn-dotnet/lets-learn-dotnet-auth-and-identity?WT.mc_id=DT-MVP-35766
- ASP.NET Core 8.0 Web API 맛보기 : https://www.youtube.com/watch?v=_M7rUwutFOc&list=PLO56HZSjrPTCffK881uMGdjT3Tc456pmg&index=22
- GitHub: https://github.com/VisualAcademy
- C# 11.0 원시 문자열 리터럴 : https://youtu.be/7l8S6Husiso
- https://www.hawaso.com/
- appsettings.json과 secrets.json 설명==> 우클릭 >사용자 암호관리 (*개발환경에서 만 적용가능*)
- ASP.NET Core 6.0 LTS 버전의 4가지 프레임워크를 하나의 프로젝트에 포함하여 실행하기 >>> https://www.youtube.com/watch?v=zrLYE5NJd8U&list=PLO56HZSjrPTCffK881uMGdjT3Tc456pmg&index=27
- @rendermode InteractiveServer (https://www.youtube.com/watch?v=ot5pG0P1nII&list=PLO56HZSjrPTCffK881uMGdjT3Tc456pmg&index=28)
- global using globalusing.cs
- @inject IJSRuntime js
- var variables = ViewBag.Variables as List<Variable>;
- public AppSettingsDemoController(IConfiguration configuration)

# 1.1. Blazor Server 7.0 Fundamentals
- 패키지관리자콘솔 > Add-Migration BuffetModelAdd,              update-database
- 패키지관리자콘솔 > Add-Migration BuffetModelAdd -Context Hawaso.Models.Candidates.CandidateAppDbContext,
  update-database -Context Hawaso.Models.Candidates.CandidateAppDbContext,
- https://github.com/VisualAcademy/VisualAcademy/blob/4ba3026d773a00e143f1216419defea2cd41e59e/src/VisualAcademy/VisualAcademy/Models/Candidates/01_Candidate.cs
- null!; (null 허용하지 않음)
- Blazor Server 에서 직접 DBContext 사용시 주의사항....
  //[a] MVC, RazorPages, Web API에서는 DbContext 사용 가능
  //builder.Services.AddDbContext<CandidateAppDbContext>(options => options.UseSqlServer(connectionString));
  //[b] Blazor Server에서는 DbContextFactory 사용 권장
  builder.Services.AddDbContextFactory<CandidateAppDbContext>(options => options.UseSqlServer(connectionString));
  // mvc 페이지에 Blazor 컴포넌트를 2개 이상 사용하기 위해서는 DbContextFactory를 사용해야 한다.
  // 하나의 Blazor 페이지에 DbContext를 2개 이상 사용하면, Blazor컴포넌트를 2개이상 사용하면  오류가 발생한다.
- OnInValidSubmit  /// Form 유효성 검사 실패 시 처리
- AsNoTracking (읽기전용으로 성능향상 옵션) await ctx.Candidates.AsNoTracking().FirstOrDefaultAsync(it => it.Id == Id);
- 17_01_Script-Migration 명령을 사용하여 데이터베이스 개체를 순수 SQL 스크립트로 뽑아내기
  패키지관리자콘솔 > Script-Migration -Context Hawaso.Models.Candidates.CandidateAppDbContext
- 17_03_자주 사용하는 공통 네임스페이스들을 _Imports 파일에 모아서 관리하기
- 17_04_namespace 지시문을 사용하여 컴포넌트의 네임스페이스를 고정값으로 설정하기
  namespace 설정하기 : @namespace AriticleApp.Pages.Candidates
- compoent테그 사용해서 mvc나 razor 페이지에 삽입할때, 주의사항
    하단 레이아웃 페이지 blazor.server.js 포함 확인
    상단<base href="~/" /> 포함확인
- ASP.NET Core 레이아웃과 Blazor Server Layout을 하나로 통합해서 사용하기
  https://www.youtube.com/watch?v=CEv0zt_MPbo&list=PLO56HZSjrPTAS3bC6UUNWBH9ih5yujpvS&index=90

# 1.2. Blazor Part 1 기초
- Remaining.ToString("c0") : double이나 decimal 값에 .ToString("c0")을 사용하면, 해당 값을 "통화" 형식(소수점 없이)으로 변환합니다. 
- Input 데코레이터와 Output 데코레이터
 Input Parameter : 부모에서 자식으로 , 컴포넌트에게 속성을 사용하여 데이터 전달
 Output Parameter : 자식에서 부모로, 
   Action : 자긱의 값을 부모에서 사용하도록, 자식에서 부모의 메서드를 호출하는 형태
   EventCallback<T>: 제네릭 타입을 사용하여, number, string형 정해서 자식에서 부모로 값을 전달하는 형태
- @ref :템플릿 참조변수, 자식요소의 @ref를 사용하여 부모에서 자식의 메서드를 호출하는 형태
- MatBlazor : https://www.matblazor.com/
- 03 04 ChartJs Blaozr 컴포넌트를 사용하여 여러 종류의 차트를 빠르게 그리기 는 다시 공부하자..버전이 많이 다름..
- https://blazorise.com/docs
- https://startbootstrap.com/themes
- 사이트 html 구성 : _Host.cshtml, _Layout.cshtml >> MainLayout.razor >>NavMenu.razor_
- Morder_Admin : SBAdmin2 다운로드 한 관리자 페이지 (https://www.youtube.com/watch?v=irRHlG1oWxo&list=PLO56HZSjrPTCQbxR12t-67YyNSBP8L-8v&index=28&t=508s)
- 
- 



# DbContextFactory VS DbContext	
Blazor Server에서 Repository에서 DbContext 사용하는 경우에는 큰 문제없이 사용이 가능합니다.
다음 링크의 제 강의 소스의 Memos 모듈의 소스가 이미 Repository에서 DbContext를 그대로 호출해서 사용합니다.
https://github.com/VisualAcademy/Hawaso/blob/d5566e578dcfb22ad8f72ff9ef2a68651f35e63a/src/Hawaso.Models/Memos/04_MemoRepository.cs#L21
단, 최근에는 이왕이면 새롭게 만드는 Repository들은 DbContextFactory를 사용하려고 노력중입니다.
DbContext와 Repository 중간에 DbContextFactory를 하나 더 두는 형태입니다. 
https://github.com/VisualAcademy/Hawaso/blob/d5566e578dcfb22ad8f72ff9ef2a68651f35e63a/src/Hawaso/Components/Pages/VendorPages/Models/07_VendorRepositoryPermanentDelete.cs#L9

# Dapper와 DbContext
-Dapper는 경량 ORM으로, DbContext와는 독립적으로 작동합니다.
-Dapper는 데이터베이스 연결(IDbConnection)을 직접 사용하여 SQL 쿼리를 실행합니다.
-따라서 Dapper를 사용할 때는 DbContext가 아닌 데이터베이스 연결 객체를 직접 관리해야 합니다.
2. DbContextFactory와 Dapper의 사용
DbContextFactory를 사용하여 DbContext를 생성한 후, Dapper와 함께 사용하려면 DbContext에서 데이터베이스 연결을 추출해야 합니다.
using Dapper;
using Microsoft.EntityFrameworkCore;

public async Task<IEnumerable<Candidate>> GetCandidatesAsync(IDbContextFactory<CandidateAppDbContext> contextFactory)
{
    // [1] DbContext 생성
    using var context = contextFactory.CreateDbContext();

    // [2] DbConnection 가져오기
    using var connection = context.Database.GetDbConnection();

    // [3] DbConnection 열기
    await connection.OpenAsync();

    // [4] Dapper를 사용하여 SQL 쿼리 실행
    var sql = "SELECT * FROM Candidates ORDER BY FirstName";
    var candidates = await connection.QueryAsync<Candidate>(sql);

    // [5] DbContext와 DbConnection은 using 블록이 끝나면 자동으로 Dispose() 호출
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


# Nuget패키지 설치
  Microsoft.EntityFrameworkCore
  Microsoft.EntityFrameworkCore.SqlServer
  Microsoft.EntityFrameworkCore.Tools
  System.Configuration.ConfigurationManager
  System.Data.SqlClient  (참조 : https://ddochea.tistory.com/189)
  Microsoft.EntityFrameworkCore.InMemory is the EF Core
  MatBlazor 
  Charts.Blazor
  BlazorInputFile
  # EPPlus 8(유료) => EPPlus 7 엑셀업로드 다운로드 
  Blazorise.Bootstrap
  Blazorise.Icons.FontAwesome

  Dapper
  Serilog
  Serilog.Extensions.Logging
  Serilog.Sinks.File
  -------------------------
  controller - ctor(ILogger<HomeController> logger)
  _logger.LogInformation("로그문구.....");
  -------------------------------------------------------------------------

# UploadApp
업로드 게시판인데, 대용량처리를 위해 mvc를 사용하고,
Network Services 에 대해 쓰기권한..
  1. Network Services에 쓰기 권한 부여
  2. IIS에서 Application Pool의 Identity를 Network Service로 변경
  3. Network Service에 쓰기 권한 부여
  4. C:\inetpub\wwwroot\UploadApp\bin\Debug\net6.0\UploadApp.dll
  5. C:\inetpub\wwwroot\UploadApp\bin\Debug\net6.0\UploadApp.pdb

  Razor 클래스 라이브러리 생성
  Razor Class Library
  다운로드 스크립트 : function saveAsFile(filename, bytesBase64) { js


# --------------------------------------------------------------------------------------------------------------------------------
# ArticleApp 리포지토리

**게시판 프로젝트** 개발 또는 강의 또는 집필 소스 모음

ASP.NET Core 6.0 + Bootstrap 5.0 기반 소스는 ArticleApp 솔루션의 VisualAcademy 프로젝트에 있습니다. 

   이후로 업데이트되는 다음 경로의 강의 메인 소스에서 진행됩니다.
   
   * https://github.com/VisualAcademy/VisualAcademy

# Sample Text
Blazorise supports several CSS frameworks, including Bootstrap (known for responsive design elements), Bulma (valued for simplicity and Flexbox base), Material (inspired by Google's Material Design), Ant Design (geared towards enterprise-level products and adapting React components' design principles), and Tailwind CSS (famous for its utility-first approach and versatility). These frameworks provide distinct styles and philosophies, offering developers a range of options to best suit their project's requirements.
A Blazorise commercial license typically includes access to advanced components, priority support, options for dedicated consultations, frequent updates and bug fixes, a license for unrestricted commercial use, potential access to the source code, and opportunities for training and workshops. This package enhances functionality, offers better support, and provides operational security for commercial projects.


# 게시판 프로젝트

## 001. 게시판 프로젝트 시작

안녕하세요. 박용준입니다. 이번 시간부터는 게시판 프로젝트를 만들어보도록 하겠습니다. 기본적인 기능부터 계속 확장해 나가는 형태로 진행됩니다. 

__게시판__, __VisualStudio__, __닷넷__

- 박용준 강사(https://www.dotnetkorea.com)
- Article: 아티클, 글, 기사
- ArticleApp 솔루션: ArticleApp 프로젝트, ArticleApp.SqlServer 프로젝트, ...
- 3Pro: Professionals(People) -> Projects(Processes) -> Products
- GitHub 공개: https://github.com/VisualAcademy/ArticleApp.git
- 개발 도구: Visual Studio, Visual Studio Code
- 개발 기술: C#, .NET, ASP.NET Core, Blazor, SQL Server, ...
- 개발 제약: 모든 API에 대해서 모두 비동기, 모두 테스트, 모두 SPA(Single Page Application)


## 002. 솔루션 및 프로젝트 생성 그리고 GitHub에 공개

게시판 프로젝트를 진행하기 위해 Visual Studio에서 기본 솔루션을 생성하고 이를 GitHub의 공개 리포지토리에 게시하여 기능을 계속 만들어 나가는 기반을 마련합니다. 

__솔루션__, __프로젝트__, **GitHub** 

- Visual Studio 2019 사용
- ArticleApp 솔루션
  - ArticleApp.Models
  - ArticleApp.Models.Tests
  - ArticleApp.SqlServer 

- GitHub
  - https://github.com/VisualAcademy/ArticleApp.git


## 003. 최소 크기의 모델과 테이블 생성 그리고 로컬 데이터베이스 게시

게시판의 전체 기능을 구현하기 전에 전체 뼈대를 학습을 위한 가장 기초적인 형태의 모델 클래스와 테이블을 생성하고 로컬 데이터베이스에 게시합니다. 

__모델클래스__, __게시판테이블__, __로컬데이터베이스__

- Article.cs 모델 클래스
- Articles.sql 테이블
- ArticleApp 데이터베이스 


## 004. 리포지토리 패턴과 리포지토리 인터페이스

리포지토리 패턴을 사용하여 게시판 데이터에 대한 주요 API를 담당하는 리포지토리 인터페이스를 만들고 기본 메서드에 대한 메서드 시그니처를 생성합니다. 

**Repository**, **리포지토리**, **리포지토리인터페이스**

- Dul.dll 파일 참조
- IArticleRepository.cs
- ArticleRepository.cs (ArticleRepositoryAdoNet, ArticleRepositoryDapper, ArticleRepositoryEfCore, ...)


## 005. Entity Framework Core 참조 및 데이터베이스 컨텍스트 클래스 생성

이번 시간에는 닷넷 스탠다드 프로젝트에 Entity Framework Core 관련 NuGet 패키지를 추가하고 전체 데이터베이스에 접근할 수 있는 부모 클래스인 DbContext 클래스를 생성합니다. 

**NuGet**, **EFCore**, **DbContext**, 

- ArticleDbContext.cs


## 006. 리포지토리 클래스의 주요 비동기 메서드 기능 구현

이번 시간에는 Entity Framework Core를 사용하여 리포지토리 클래스의 주요 비동기 메서드의 기능을 구현합니다.

**리포지토리**, **EF Core**, **CRUD**

- ArticleRepository.cs


## 007. 테스트 프로젝트에서 리포지토리 클래스의 모든 비동기 메서드 테스트

MSTest 프로젝트인 테스트 프로젝트에서 리포지토리 클래스의 모든 메서드를 하나씩 테스트합니다.

__MSTest__, **테스트**, **단위테스트**

- ArticleRepositoryTest.cs


## 008. Blazor 프로젝트 생성 및 게시판 관련 기본 페이지 생성 

현재 강의 시점에서 가장 최신의 웹 개발 기술인 블레이저를 사용하여 웹 페이지 제작을 시작하겠습니다.

__Blazor__, **블레이저**, __웹앱__


## 009. 공통 클래스인 DbContext와 Repository 클래스에 대한 의존성 주입 관련 코드를 Startup 파일에 추가

Models 클래스 라이브러리의 DbContext와 Repository 클래스를 블레이저 프로젝트에 사용 가능하도록 의존성 주입 코드를 작성합니다. 

__DI__, **의존성주입**, __Startup__


## 010. 게시판 리스트 페이지 작성 

게시판에서 제일 먼저 접근하는 메인 페이지인 리스트 페이지를 꾸며보도록 하겠습니다. 

 __리스트__, **Index**, __메인페이지__
 
 
 ## 011. 게시판 글쓰기 페이지 작성
 
 게시판에 데이터를 입력하는 글쓰기 페이지를 부트스트랩을 사용하여 폼을 만들고 데이터를 입력하는 코드를 작성합니다.
 
 **글쓰기**, **부트스트랩**, **폼**
 
 
 ## 012. 게시판 상세보기 페이지 작성
 
 게시판에 작성된 데이터에 대한 상세 내용을 볼 수 있는 상세보기 페이지를 작성합니다. 
 
 **상세**, **게시판**, **Details**
 
 
 ## 013. 게시판 수정 페이지 작성 및 Content 열 추가 후 관련 코드 수정
  
 게시판에 작성된 데이터를 수정할 수 있는 페이지를 작성합니다. 
 
 **수정**, **게시판**, **Edit**


 ## 014. 게시판 삭제 페이지 작성 및 확인 관련 기능을 IJSRuntime으로 구현
  
게시판에 작성된 데이터를 삭제할 수 있는 페이지를 작성합니다. C#에서 JavaScript의 함수를 호출하는 방법을 다룹니다.
 
 **삭제**, **게시판**, **Delete**
 

 ## 015. 게시판 리스트에 페이저 컴포넌트 적용 및 기본 페이징 구현
  
게시판의 리스트에 적용되어 사용되는 페이저 컴포넌트의 기능을 리스트 페이지에 적용합니다.  
 
 **Pager**, **페이징**, **페이저**
 

 ## 016. 페이저 컴포넌트 UI 만들기 
  
게시판의 리스트에 적용되어 사용되는 페이저 컴포넌트의 UI 기능을 작성합니다.  
 
 **Pager**, **페이징**, **페이저**


 ## 017. IsPinned 속성 추가 및 공지글로 설정하는 기능을 모달 폼으로 구현하기
  
새로운 열과 속성을 추가하고 순수 C# 코드를 사용하여 모달 폼을 띄우거나 닫는 기능을 구현합니다.  
 
 **Modal**, **공지설정**, **모달폼**
 
 
 ## 018. 부트스트랩 모달 폼을 사용하여 관리자 전용 데이터 삭제 모달 다이얼로그 구현 
  
부트스트랩 모달 폼과 jQuery를 연동하여 관리자 전용 데이터 삭제 모달 다이얼로그를 구현하는 방법을 보여줍니다. 
 
 **Modal**, **삭제폼**, **모달폼**
 
 Bootstrap 5 모달 폼 소스는 VisualAcademy 프로젝트에 적용되어 있습니다. 
 
  
 ## 019. 부트스트랩 모달 폼을 사용하여 관리자 전용 데이터 입력 및 수정 모달 폼 구현 
  
부트스트랩 모달 폼과 jQuery를 연동하여 관리자 전용 데이터 입력 및 수정 모달 다이얼로그를 구현하는 방법을 보여줍니다. 
 
 **Modal**, **수정폼**, **모달폼**
 
 
 ## 020. 상세보기 내용의 줄바꿈 등을 추가 및 게시판의 UI를 개선하기
  
마스터 브랜치가 아닌 새로운 브랜치에서 게시판의 UI를 개선하는 작업을 진행하고 테스트가 완료되면 마스터 브랜치로 병합하면서 개발을 진행합니다.
 
 **마스터**, **브랜치**, **병합**
 
 
 ## 021. 21_인라인 코드 방식을 코드 비하인드 방식으로 변경
  
 게시판에 작성된 데이터를 삭제할 수 있는 페이지를 작성합니다. 
 
 **삭제**, **게시판**, **Delete**
 
 
 ## 022. 페이저 컴포넌트를 Razor 클래스 라이브러리로 만들어 NuGet 갤러리에 공개
  
 게시판에서 사용되는 페이저 컴포넌트를 NuGet 갤러리에 공개합니다. 
  
 **NuGet**, **Pager**, **DulPager**


 ## 023. 페이저 컴포넌트를 NuGet 갤러리의 DulPager로 대체
  
NuGet 갤러리에 공개된 DulPager 컴포넌트를 사용합니다. 
  
 **NuGet**, **Pager**, **DulPager**