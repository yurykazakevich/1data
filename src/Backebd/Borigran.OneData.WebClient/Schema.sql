
    if exists (select * from dbo.sysobjects where id = object_id(N'TConstructorItems') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TConstructorItems

    if exists (select * from dbo.sysobjects where id = object_id(N'TImages') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TImages

    create table TConstructorItems (
        ConstructorItemID INT IDENTITY NOT NULL,
       ImageId INT null,
       primary key (ConstructorItemID)
    )

    create table TImages (
        ImageID INT IDENTITY NOT NULL,
       ImageType NVARCHAR(255) null,
       ImageData VARBINARY(MAX) null,
       primary key (ImageID)
    )
