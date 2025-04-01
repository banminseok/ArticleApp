seLECT [Id], [Content], [Created], [CreatedBy], [IsPinned], [Modified], [ModifiedBy], [Name], [ParentId], [Title]
      FROM [Notices] 
      ORDER BY [Id] DESC
      OFFSET 3 ROWS FETCH NEXT 3 ROWS ONLY