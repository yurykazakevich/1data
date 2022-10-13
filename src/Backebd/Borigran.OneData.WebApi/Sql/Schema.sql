
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_ConstructorItemCategory_ParentCategory]') and parent_object_id = OBJECT_ID(N'TConstructorItemCategorys'))
alter table TConstructorItemCategorys  drop constraint FK_ConstructorItemCategory_ParentCategory


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_ConstructorItem_Category]') and parent_object_id = OBJECT_ID(N'TConstructorItems'))
alter table TConstructorItems  drop constraint FK_ConstructorItem_Category


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_ConstructorItem_Position]') and parent_object_id = OBJECT_ID(N'TConstructorItemPositions'))
alter table TConstructorItemPositions  drop constraint FK_ConstructorItem_Position


    if exists (select * from dbo.sysobjects where id = object_id(N'TUsers') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TUsers

    if exists (select * from dbo.sysobjects where id = object_id(N'TAreas') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TAreas

    if exists (select * from dbo.sysobjects where id = object_id(N'TConstructorItemCategorys') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TConstructorItemCategorys

    if exists (select * from dbo.sysobjects where id = object_id(N'TConstructorItems') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TConstructorItems

    if exists (select * from dbo.sysobjects where id = object_id(N'TConstructorItemPositions') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TConstructorItemPositions

    create table TUsers (
        UserID INT IDENTITY NOT NULL,
       PhoneNumber NVARCHAR(255) not null,
       RefreshToken NVARCHAR(255) null,
       RefreshTokenExpired DATETIME2 null,
       IsPhisical BIT not null,
       primary key (UserID),
      unique (PhoneNumber)
    )

    create table TAreas (
        AreaID INT IDENTITY NOT NULL,
       Name NVARCHAR(255) not null,
       Length INT not null,
       Width INT not null,
       BurialType INT not null,
       primary key (AreaID)
    )

    create table TConstructorItemCategorys (
        ConstructorItemCategoryID INT IDENTITY NOT NULL,
       Name NVARCHAR(255) not null,
       ItemType INT not null,
       SortOrder INT null,
       ParentCategoryId INT null,
       primary key (ConstructorItemCategoryID)
    )

    create table TConstructorItems (
        ConstructorItemID INT IDENTITY NOT NULL,
       Name NVARCHAR(255) not null,
       Price DECIMAL(19,5) not null,
       ArticleNumber NVARCHAR(255) not null,
       Material NVARCHAR(255) not null,
       Length INT not null,
       Width INT not null,
       Height INT null,
       Weight DECIMAL(19,5) not null,
       Varranty INT not null,
       ItemType INT not null,
       CategoryId INT null,
       AllowedBurialTypes NVARCHAR(255) null,
       primary key (ConstructorItemID)
    )

    create table TConstructorItemPositions (
        ConstructorItemPositionID INT IDENTITY NOT NULL,
       ImageName NVARCHAR(255) not null,
       Position INT not null,
       ConstructorItemId INT not null,
       primary key (ConstructorItemPositionID)
    )

    create index PhoneNumber_IDX on TUsers (PhoneNumber)

    create index IDX_ConstructorItemCategory_ParentCategoryId on TConstructorItemCategorys (ParentCategoryId)

    alter table TConstructorItemCategorys 
        add constraint FK_ConstructorItemCategory_ParentCategory 
        foreign key (ParentCategoryId) 
        references TConstructorItemCategorys

    create index IDX_ConstructorItem_CategoryId on TConstructorItems (CategoryId)

    alter table TConstructorItems 
        add constraint FK_ConstructorItem_Category 
        foreign key (CategoryId) 
        references TConstructorItemCategorys

    create index IDX_ConstructorItemPosition_ConstructorItemId on TConstructorItemPositions (ConstructorItemId)

    alter table TConstructorItemPositions 
        add constraint FK_ConstructorItem_Position 
        foreign key (ConstructorItemId) 
        references TConstructorItems
