USE [DrySoftBranch]
GO
/****** Object:  Table [dbo].[FactoryGrid]    Script Date: 07/25/2012 13:14:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FactoryGrid](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookingNumber] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BookingDeliveryDate] [datetime] NULL,
	[Customer] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ISN] [int] NULL,
	[SubItemName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ItemProcessType] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ItemExtraProcessType1] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ItemExtraProcessType2] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IsUrgent] [bit] NULL,
	[ItemTotalQuantity] [int] NULL,
	[BarCode] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BranchName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BranchCode] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[BranchId] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ExternalBranchID] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF