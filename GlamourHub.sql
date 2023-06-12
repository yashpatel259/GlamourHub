USE [master]
GO
/****** Object:  Database [GlamourHub]    Script Date: 11/06/2023 20:06:10 ******/
CREATE DATABASE [GlamourHub]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GlamourHub', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\GlamourHub.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'GlamourHub_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\GlamourHub_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [GlamourHub] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GlamourHub].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GlamourHub] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GlamourHub] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GlamourHub] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GlamourHub] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GlamourHub] SET ARITHABORT OFF 
GO
ALTER DATABASE [GlamourHub] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [GlamourHub] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GlamourHub] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GlamourHub] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GlamourHub] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GlamourHub] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GlamourHub] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GlamourHub] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GlamourHub] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GlamourHub] SET  DISABLE_BROKER 
GO
ALTER DATABASE [GlamourHub] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GlamourHub] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GlamourHub] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GlamourHub] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GlamourHub] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GlamourHub] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GlamourHub] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GlamourHub] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [GlamourHub] SET  MULTI_USER 
GO
ALTER DATABASE [GlamourHub] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GlamourHub] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GlamourHub] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GlamourHub] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [GlamourHub] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [GlamourHub] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [GlamourHub] SET QUERY_STORE = OFF
GO
USE [GlamourHub]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 11/06/2023 20:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[addresses]    Script Date: 11/06/2023 20:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[addresses](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[street] [varchar](100) NOT NULL,
	[city] [varchar](50) NOT NULL,
	[state] [varchar](50) NOT NULL,
	[postal_code] [varchar](20) NOT NULL,
	[created_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[brands]    Script Date: 11/06/2023 20:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[brands](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[created_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cart]    Script Date: 11/06/2023 20:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cart](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[product_id] [int] NULL,
	[quantity] [int] NOT NULL,
	[created_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[categories]    Script Date: 11/06/2023 20:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[created_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[order_items]    Script Date: 11/06/2023 20:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[order_items](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[order_id] [int] NULL,
	[product_id] [int] NULL,
	[quantity] [int] NOT NULL,
	[price] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[orders]    Script Date: 11/06/2023 20:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[orders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[created_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[payments]    Script Date: 11/06/2023 20:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[payments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[card_number] [varchar](16) NOT NULL,
	[expiration_date] [varchar](5) NOT NULL,
	[cvv] [varchar](4) NOT NULL,
	[created_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[products]    Script Date: 11/06/2023 20:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[products](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[description] [varchar](max) NULL,
	[price] [decimal](10, 2) NOT NULL,
	[category_id] [int] NULL,
	[brand_id] [int] NULL,
	[created_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[reviews]    Script Date: 11/06/2023 20:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[reviews](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NULL,
	[product_id] [int] NULL,
	[rating] [int] NOT NULL,
	[comment] [varchar](max) NULL,
	[created_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 11/06/2023 20:06:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NOT NULL,
	[created_at] [datetime] NULL,
	[role] [varchar](20) NOT NULL,
	[password] [varchar](100) NOT NULL,
	[firstname] [varchar](100) NOT NULL,
	[lastname] [varchar](100) NOT NULL,
	[email] [varchar](100) NOT NULL,
 CONSTRAINT [PK__users__3213E83F1447F360] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[users] ON 

INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (1, N'viral', CAST(N'2023-06-08T15:58:58.940' AS DateTime), N'Customer', N'1234', N'viral', N'Patel', N'vp.com')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (2, N'yypatel259', CAST(N'2023-06-08T16:09:11.907' AS DateTime), N'Seller', N'abcd', N'Yash', N'Patel', N'yashpatel259.yp@gmail.com')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (3, N'seller@123', CAST(N'2023-06-08T16:54:07.177' AS DateTime), N'Seller', N'abcd', N'Yash', N'Patel', N'yashpatel259.yp@gmail.com')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (4, N'Chintan', CAST(N'2023-06-08T18:17:48.367' AS DateTime), N'Customer', N'UGF0ZWxAMTIz', N'Chintan', N'Patel', N'patelchintan2510@gmail.com')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (5, N'test', CAST(N'2023-06-08T19:03:31.860' AS DateTime), N'Seller', N'dGVzdA==', N'test', N'test', N'test@1.com')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (6, N'test', CAST(N'2023-06-08T21:14:44.830' AS DateTime), N'Customer', N'YWJjZA==', N'Yash', N'Patel', N'yashpatel259.yp@gmail.com')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (7, N'Ypatel3242@conestogac.on.ca', CAST(N'2023-06-08T21:17:20.120' AS DateTime), N'Seller', N'YWJjZA==', N'Yash', N'Patel', N'yashpatel259.yp@gmail.com')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (8, N'shivani123', CAST(N'2023-06-08T22:47:37.770' AS DateTime), N'Seller', N'MTIzNDU=', N'shivani', N'Ben', N'shivani@123.bc')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (9, N'jp', CAST(N'2023-06-08T23:52:30.840' AS DateTime), N'Seller', N'anA=', N'jugal', N'patel', N'jp@123.bc')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (10, N'jp', CAST(N'2023-06-08T23:52:37.570' AS DateTime), N'Seller', N'anA=', N'jugal', N'patel', N'jp@123.bc')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (11, N'jp', CAST(N'2023-06-08T23:56:06.103' AS DateTime), N'Seller', N'anA=', N'jugal', N'patel', N'jp@123.bc')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (12, N'aa', CAST(N'2023-06-08T23:56:50.347' AS DateTime), N'Customer', N'YWE=', N'aa', N'aa', N'aa@a.a')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (13, N'rrr', CAST(N'2023-06-08T23:59:19.373' AS DateTime), N'Customer', N'cnI=', N'aa', N'cc', N'f2@s.f')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (14, N'rrr', CAST(N'2023-06-09T00:00:01.463' AS DateTime), N'Customer', N'cnI=', N'aa', N'cc', N'f2@s.f')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (15, N'rrr', CAST(N'2023-06-09T00:00:27.183' AS DateTime), N'Customer', N'cnI=', N'aa', N'cc', N'f2@s.f')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (16, N'rrr', CAST(N'2023-06-09T00:00:36.763' AS DateTime), N'Customer', N'cnI=', N'aa', N'cc', N'f2@s.f')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (17, N'rrr', CAST(N'2023-06-09T00:00:49.987' AS DateTime), N'Customer', N'cnI=', N'aa', N'cc', N'f2@s.f')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (18, N'bbb', CAST(N'2023-06-09T00:02:20.463' AS DateTime), N'Customer', N'YmJi', N'Yash', N'Patel', N'yashpatel259.yp@gmail.com')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (19, N'newuser', CAST(N'2023-06-10T16:31:57.807' AS DateTime), N'Seller', N'bmV3dXNlcg==', N'new', N'user', N'newuser@test.com')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (20, N'newuser1', CAST(N'2023-06-10T16:41:31.983' AS DateTime), N'Seller', N'bmV3dXNlcjE=', N'new', N'user', N'newuser1@test.com')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (21, N'newuser2', CAST(N'2023-06-10T16:46:12.297' AS DateTime), N'Seller', N'bmV3dXNlcjI=', N'new', N'user2', N'newuser2@test.com')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (22, N'newuser3', CAST(N'2023-06-10T16:50:44.947' AS DateTime), N'Customer', N'bmV3dXNlcjM=', N'new', N'user3', N'newuser3@test.com')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (23, N'newuser4', CAST(N'2023-06-10T16:52:01.157' AS DateTime), N'Customer', N'bmV3dXNlcjQ=', N'new', N'user4', N'newuser4@test.com')
INSERT [dbo].[users] ([id], [username], [created_at], [role], [password], [firstname], [lastname], [email]) VALUES (24, N'newuser5', CAST(N'2023-06-11T19:18:05.467' AS DateTime), N'Customer', N'bmV3dXNlcjU=', N'new', N'user5', N'newuser5@test.com')
SET IDENTITY_INSERT [dbo].[users] OFF
GO
ALTER TABLE [dbo].[addresses] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[brands] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[cart] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[categories] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[orders] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[payments] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[products] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[reviews] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[users] ADD  CONSTRAINT [DF__users__created_a__276EDEB3]  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[addresses]  WITH CHECK ADD  CONSTRAINT [FK__addresses__user___4222D4EF] FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[addresses] CHECK CONSTRAINT [FK__addresses__user___4222D4EF]
GO
ALTER TABLE [dbo].[cart]  WITH CHECK ADD FOREIGN KEY([product_id])
REFERENCES [dbo].[products] ([id])
GO
ALTER TABLE [dbo].[cart]  WITH CHECK ADD  CONSTRAINT [FK__cart__user_id__35BCFE0A] FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[cart] CHECK CONSTRAINT [FK__cart__user_id__35BCFE0A]
GO
ALTER TABLE [dbo].[order_items]  WITH CHECK ADD FOREIGN KEY([order_id])
REFERENCES [dbo].[orders] ([id])
GO
ALTER TABLE [dbo].[order_items]  WITH CHECK ADD FOREIGN KEY([product_id])
REFERENCES [dbo].[products] ([id])
GO
ALTER TABLE [dbo].[orders]  WITH CHECK ADD  CONSTRAINT [FK__orders__user_id__3A81B327] FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[orders] CHECK CONSTRAINT [FK__orders__user_id__3A81B327]
GO
ALTER TABLE [dbo].[payments]  WITH CHECK ADD  CONSTRAINT [FK__payments__user_i__45F365D3] FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[payments] CHECK CONSTRAINT [FK__payments__user_i__45F365D3]
GO
ALTER TABLE [dbo].[products]  WITH CHECK ADD FOREIGN KEY([brand_id])
REFERENCES [dbo].[brands] ([id])
GO
ALTER TABLE [dbo].[products]  WITH CHECK ADD FOREIGN KEY([category_id])
REFERENCES [dbo].[categories] ([id])
GO
ALTER TABLE [dbo].[reviews]  WITH CHECK ADD FOREIGN KEY([product_id])
REFERENCES [dbo].[products] ([id])
GO
ALTER TABLE [dbo].[reviews]  WITH CHECK ADD  CONSTRAINT [FK__reviews__user_id__49C3F6B7] FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([id])
GO
ALTER TABLE [dbo].[reviews] CHECK CONSTRAINT [FK__reviews__user_id__49C3F6B7]
GO
USE [master]
GO
ALTER DATABASE [GlamourHub] SET  READ_WRITE 
GO
