USE [onedata]
GO

/****** Object:  Table [dbo].[TAreaAddonImages]    Script Date: 23.09.2022 16:25:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TAreaAddonImages]') AND type in (N'U'))
DROP TABLE [dbo].[TAreaAddonImages]
GO

/****** Object:  Table [dbo].[TAreaAddons]    Script Date: 23.09.2022 16:26:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TAreaAddons]') AND type in (N'U'))
DROP TABLE [dbo].[TAreaAddons]
GO

/****** Object:  Table [dbo].[TUsers]    Script Date: 23.09.2022 16:26:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TUsers]') AND type in (N'U'))
DROP TABLE [dbo].[TUsers]
GO

/****** Object:  Table [dbo].[TConstructorItems]    Script Date: 23.09.2022 17:31:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TConstructorItems]') AND type in (N'U'))
DROP TABLE [dbo].[TConstructorItems]
GO

