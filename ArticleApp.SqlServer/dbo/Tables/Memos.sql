﻿-- [0] Table: Memos(완성형 게시판) 테이블 설계(멀티 게시판은 Acts로 이동됨) 
-- [!] 게시판 테이블 설계: Articles, Posts, Entries, Notes, Memos, (Basic+Upload+Reply) => DotNetNote/DotNetMemo

CREATE TABLE [dbo].[Memos]
(
	[Id]            BIGINT NOT NULL PRIMARY KEY Identity(1, 1), -- [1][일련번호], Serial Number
	[ParentId]      BigInt Null,								-- ParentId, AppId, SiteId, ProductId, CategoryId, ...
	[ParentKey]     NVarChar(255) Null,						    -- ParentKey == 부모의 GUID, UserId, RoleId, ...

    [Active]        BIT NOT NULL DEFAULT(1),                    -- 활성 상태
    [IsDeleted]     BIT NOT NULL DEFAULT(0),                    -- 소프트 삭제

    --[!] Auditable
	[CreatedBy]     NVarChar(255) Null,						    -- 등록자(Creator)
	[Created]       DATETIMEOFFSET Default(GetDate()) Null,  	-- [5][생성일](PostDate), DatePublished, CreatedAt
	[ModifiedBy]    NVarChar(255) Null,					        -- 수정자(LastModifiedBy)
	[Modified]      DATETIMEOFFSET Null,						-- 수정일(LastModified)

    --[0] 5W1H: 누가, 언제, 어디서, 무엇을, 어떻게, 왜
    [Name]          NVarChar(255) Not Null,                     -- [2][이름](작성자)
    [PostDate]      DateTime Default GetDate() Not Null,        -- 작성일 
    [PostIp]        NVarChar(15) Null,                          -- 작성IP
    [Title]         NVarChar(512) Not Null,                     -- [3][제목]
    [Content]       NText Not Null,                             -- [4][내용]__NVarChar(Max) => NText__
    [Category]      NVarChar(20) Default('Free') Null,          -- 카테고리(확장...) => '공지', '자유', '자료', '사진', ...

	--[1] 기본형(Article, Basic, Notice) 게시판 관련 주요 컬럼
    Email           NVarChar(100) Null,                         -- 이메일 
    Password        NVarChar(255) Null,                         -- 비밀번호
    ReadCount       Int Default 0,                              -- 조회수
    Encoding        NVarChar(20) Not Null,                      -- 인코딩(Text/HTML/Mixed/MarkDown)
    Homepage        NVarChar(100) Null,                         -- 홈페이지
    ModifyDate      DateTime Null,                              -- 수정일 
    ModifyIp        NVarChar(15) Null,                          -- 수정IP
    CommentCount    Int Default 0,                              -- 댓글수
	IsPinned        Bit Default 0 Null,                         -- 공지글로 올리기, IsActive 

	--[2] 자료실(Upload, BBS) 게시판 관련 주요 컬럼
    FileName        NVarChar(255) Null,                         -- 파일명
    FileSize        Int Default 0,                              -- 파일크기
    DownCount       Int Default 0,                              -- 다운수 

	--[3] 답변형(Reply, Qna) 게시판 관련 주요 컬럼
    Ref             Int Not Null,                               -- 참조(부모글), Group
    Step            Int Not Null Default 0,                     -- 답변깊이(레벨), Depth, Level
    RefOrder        Int Not Null Default 0,                     -- 답변순서
    AnswerNum       Int Not Null Default 0,                     -- 답변수
    ParentNum       Int Not Null Default 0,                     -- 부모글번호

    --[!] 추가 항목 들어오는 곳...
    Num             Int Null,                                   -- 번호(확장...)
    UserId          Int Null,                                   -- (확장...) 사용자 테이블 Id
    CategoryId      Int Null Default 0,                         -- (확장...) 카테고리 테이블 Id
    BoardId         Int Null Default 0,                         -- (확장...) 게시판(Boards) 테이블 Id
    ApplicationId   Int Null Default 0                          -- (확장용) 응용 프로그램 Id
)
Go