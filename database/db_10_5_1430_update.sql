USE [EFP]
GO

ALTER TABLE [dbo].[DetailedProducts] DROP CONSTRAINT [FK_DetailedProducts_MasterProducts]
GO

/****** Object:  Table [dbo].[DetailedProducts]    Script Date: 10/5/2016 2:29:36 PM ******/
DROP TABLE [dbo].[DetailedProducts]
GO

/****** Object:  Table [dbo].[ProductPictures]    Script Date: 10/5/2016 2:30:01 PM ******/
DROP TABLE [dbo].[ProductPictures]
GO


/****** Object:  Table [dbo].[DetailedProducts]    Script Date: 10/5/2016 2:29:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DetailedProducts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Price] [money] NOT NULL,
	[PromotionPrice] [money] NULL,
	[Status] [bit] NOT NULL,
	[MasterId] [int] NOT NULL,
	[Attr1] [nvarchar](200) NULL,
	[Attr2] [nvarchar](200) NULL,
	[Attr3] [nvarchar](200) NULL,
	[Attr4] [nvarchar](200) NULL,
	[Attr5] [nvarchar](200) NULL,
	[Attr6] [nvarchar](200) NULL,
	[Attr7] [nvarchar](200) NULL,
 CONSTRAINT [PK_Detailed_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[DetailedProducts]  WITH CHECK ADD  CONSTRAINT [FK_DetailedProducts_MasterProducts] FOREIGN KEY([MasterId])
REFERENCES [dbo].[MasterProducts] ([Id])
GO

ALTER TABLE [dbo].[DetailedProducts] CHECK CONSTRAINT [FK_DetailedProducts_MasterProducts]
GO

USE [EFP]
GO

ALTER TABLE [dbo].[ProductPictures] DROP CONSTRAINT [FK_ProductPictures_MasterProducts]
GO

/****** Object:  Table [dbo].[ProductPictures]    Script Date: 10/5/2016 2:30:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProductPictures](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MasterId] [int] NOT NULL,
	[Urls] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ProductPictures] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[ProductPictures]  WITH CHECK ADD  CONSTRAINT [FK_ProductPictures_MasterProducts] FOREIGN KEY([MasterId])
REFERENCES [dbo].[MasterProducts] ([Id])
GO

ALTER TABLE [dbo].[ProductPictures] CHECK CONSTRAINT [FK_ProductPictures_MasterProducts]
GO



