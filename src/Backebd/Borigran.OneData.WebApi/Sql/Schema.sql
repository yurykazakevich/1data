
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK_Area_AreaAddon]') and parent_object_id = OBJECT_ID(N'TAreaAddons'))
alter table TAreaAddons  drop constraint FK_Area_AreaAddon


    if exists (select * from dbo.sysobjects where id = object_id(N'TUsers') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TUsers

    if exists (select * from dbo.sysobjects where id = object_id(N'TAreaAddons') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TAreaAddons

    if exists (select * from dbo.sysobjects where id = object_id(N'TAreas') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TAreas

    if exists (select * from dbo.sysobjects where id = object_id(N'TConstructorItems') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TConstructorItems

    create table TUsers (
        UserID INT IDENTITY NOT NULL,
       PhoneNumber NVARCHAR(255) not null,
       RefreshToken NVARCHAR(255) null,
       RefreshTokenExpired DATETIME2 null,
       IsPhisical BIT not null,
       primary key (UserID),
      unique (PhoneNumber)
    )

    create table TAreaAddons (
        AreaAddonID INT IDENTITY NOT NULL,
       Name NVARCHAR(255) not null,
       AddonType INT not null,
       AreaId INT null,
       primary key (AreaAddonID)
    )

    create table TAreas (
        AreaID INT IDENTITY NOT NULL,
       Name NVARCHAR(255) not null,
       Length INT not null,
       Width INT not null,
       BurialType INT not null,
       primary key (AreaID)
    )

    create table TConstructorItems (
        ConstructorItemID INT IDENTITY NOT NULL,
       Name NVARCHAR(255) null,
       ImageName NVARCHAR(255) null,
       Price DECIMAL(19,5) not null,
       ArticleNumber NVARCHAR(255) not null,
       Material NVARCHAR(255) not null,
       Length INT not null,
       Width INT not null,
       Height INT not null,
       Weight DECIMAL(19,5) not null,
       Varranty INT not null,
       ItemType INT not null,
       primary key (ConstructorItemID)
    )

    create index PhoneNumber_IDX on TUsers (PhoneNumber)

    create index IDX_AreaAddon_AreaId on TAreaAddons (AreaId)

    alter table TAreaAddons 
        add constraint FK_Area_AreaAddon 
        foreign key (AreaId) 
        references TAreas
