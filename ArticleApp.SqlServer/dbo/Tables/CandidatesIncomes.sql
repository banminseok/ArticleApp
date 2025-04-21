CREATE TABLE CandidateIncomes (
    Id INT IDENTITY(1,1) PRIMARY KEY, -- 기본 키, 자동 증가
    Source NVARCHAR(50) NOT NULL, -- 필수 입력, 최대 50자
    Amount DECIMAL(18, 2) NULL, -- 선택 입력, 소수점 포함 숫자
    UserId NVARCHAR(MAX) NULL -- 선택 입력, 길이 제한 없음
);
