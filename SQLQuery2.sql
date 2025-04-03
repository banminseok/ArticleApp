    insert into Notices ( [ParentId]    ,
    [Name]    ,
    [Title]   ,
    [Content] ,
    [IsPinned]  ,
    [CreatedBy], Created)
    select
    [ParentId]    ,
    [Name]    ,
    [Title]   ,
    [Content] ,
    [IsPinned]  ,
    [CreatedBy],
    DATEADD(month, (convert(int , (id%12))-5), [Created]) 
    from Notices
