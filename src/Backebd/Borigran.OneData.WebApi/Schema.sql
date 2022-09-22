
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_ConstructorItem_Image]') and parent_object_id = OBJECT_ID(N'TConstructorItemImages'))
alter table TConstructorItemImages  drop constraint FK_ConstructorItem_Image


    if exists (select * from dbo.sysobjects where id = object_id(N'TUsers') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TUsers

    if exists (select * from dbo.sysobjects where id = object_id(N'TConstructorItemImages') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TConstructorItemImages

    if exists (select * from dbo.sysobjects where id = object_id(N'TConstructorItems') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TConstructorItems

    if exists (select * from dbo.sysobjects where id = object_id(N'TImages') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TImages

    create table TUsers (
        UserID INT IDENTITY NOT NULL,
       PhoneNumber NVARCHAR(255) not null,
       RefreshToken NVARCHAR(255) null,
       RefreshTokenExpired DATETIME2 null,
       primary key (UserID),
      unique (PhoneNumber)
    )

    create table TConstructorItemImages (
        ConstructorItemImageID INT IDENTITY NOT NULL,
       ImagePath NVARCHAR(255) not null,
       ConstructorItemId INT not null,
       primary key (ConstructorItemImageID)
    )

    create table TConstructorItems (
        ConstructorItemID INT IDENTITY NOT NULL,
       Name NVARCHAR(255) not null,
       Price DECIMAL(19,5) not null,
       ArticleNumber NVARCHAR(255) not null,
       Material NVARCHAR(255) not null,
       Length DECIMAL(19,5) not null,
       Width DECIMAL(19,5) not null,
       Height DECIMAL(19,5) not null,
       Weight DECIMAL(19,5) not null,
       Varranty INT not null,
       primary key (ConstructorItemID)
    )

    create table TImages (
        ImageID INT IDENTITY NOT NULL,
       ImageType INT null,
       ImageData VARBINARY(MAX) null,
       primary key (ImageID)
    )

    create index PhoneNumber_IDX on TUsers (PhoneNumber)

    alter table TConstructorItemImages 
        add constraint FK_ConstructorItem_Image 
        foreign key (ConstructorItemId) 
        references TConstructorItems
