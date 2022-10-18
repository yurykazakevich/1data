
    if exists (select * from dbo.sysobjects where id = object_id(N'TUsers') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table TUsers

    create table TUsers (
        UserID INT IDENTITY NOT NULL,
       PhoneNumber NVARCHAR(255) not null,
       RefreshToken NVARCHAR(255) null,
       RefreshTokenExpired DATETIME2 null,
       IsPhisical BIT not null,
       primary key (UserID)
    )

    create index PhoneNumber_IDX on TUsers (PhoneNumber)
