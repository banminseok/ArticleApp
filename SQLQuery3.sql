    insert into Notices ( [ParentId]    ,
    [Name]    ,
    [Title]   ,
    [Content] ,
    [IsPinned]  ,
    [CreatedBy], Created)
    select top 310
    [ParentId]    ,
    [Name]    ,
    [Title]   ,
    [Content] ,
    [IsPinned]  ,
    [CreatedBy],
    '2025-10-01 17:17:56.770'
    from Notices
    where Created between '2025-03-01' and '2025-03-31'