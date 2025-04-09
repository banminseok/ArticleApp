#    

https://www.youtube.com/playlist?list=PLO56HZSjrPTC1c1MbY72vVT0sO33G-9Od
git :	01 https://github.com/VisualAcademy/articleapp
		02 https://github.com/VisualAcademy/NoticeApp.DevLec
		03 https://github.com/VisualAcademy/UploadApp



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