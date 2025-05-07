-- [1] Table: Software Lists
CREATE TABLE [dbo].[Medias]
(
	[Id] Int Not Null Identity(1, 1) Primary Key,	-- Serial Number
	[Name] NVarChar(255) Not Null,					-- S/W Name
	-- TODO: Columns Add Region


	-- AuditableBase.cs 참조
	[CreatedBy] NVarChar(255) Null,			-- 등록자(Creator)
	[Created] DateTime Default(GetDate()),	-- 생성일
	[ModifiedBy] NVarChar(255) Null,		-- 수정자(LastModifiedBy)
	[Modified] DateTime Null,				-- 수정일(LastModified)
)
