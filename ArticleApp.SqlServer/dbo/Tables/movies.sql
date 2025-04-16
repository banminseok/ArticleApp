CREATE TABLE Movies (
    Id INT IDENTITY(1,1) PRIMARY KEY, -- 기본 키, 자동 증가
    Title NVARCHAR(255) NOT NULL DEFAULT '', -- 제목, 기본값은 빈 문자열
    ReleaseDate DATE NULL, -- 출시일, NULL 허용
    Genre NVARCHAR(255) NOT NULL DEFAULT '', -- 장르, 기본값은 빈 문자열
    Price DECIMAL(18, 2) NOT NULL -- 가격, 소수점 2자리까지
);