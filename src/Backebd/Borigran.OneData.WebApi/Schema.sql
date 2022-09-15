
    if exists (select * from dbo.sysobjects where id = object_id(N'TUsers') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TUsers

    if exists (select * from dbo.sysobjects where id = object_id(N'TConstructorItems') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TConstructorItems

    if exists (select * from dbo.sysobjects where id = object_id(N'TImages') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TImages

    create table TUsers (
        UserID INT IDENTITY NOT NULL,
       PhoneNumber NVARCHAR(255) null,
       RefreshToken NVARCHAR(255) null,
       RefreshTokenExpired DATETIME2 null,
       primary key (UserID),
      unique (PhoneNumber)
    )

    create table TConstructorItems (
        ConstructorItemID INT IDENTITY NOT NULL,
       ImageId INT null,
       primary key (ConstructorItemID)
    )

    create table TImages (
        ImageID INT IDENTITY NOT NULL,
       ImageType INT null,
       ImageData VARBINARY(MAX) null,
       primary key (ImageID)
    )

    create index PhoneNumber_IDX on TUsers (PhoneNumber)
