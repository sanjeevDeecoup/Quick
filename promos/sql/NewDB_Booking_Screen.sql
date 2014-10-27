
if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='ProcessID' and o.name='ProcessMaster' )
	alter table ProcessMaster add ProcessID int identity(1, 1) NOT NULL Unique
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='ProcessImage' and o.name='ProcessMaster'  )
	alter table ProcessMaster add ProcessImage varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='Active' and o.name='ProcessMaster' )
BEGIN
	alter table ProcessMaster add Active tinyint Default(1)
END
GO


if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='Active' and o.name='ItemMaster' )
BEGIN
	alter table ItemMaster add Active tinyint default(1)
END
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='SortOrder' and o.name='ItemMaster' )
BEGIN
	alter table ItemMaster add SortOrder smallint
END
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='ItemImage' and o.name='ItemMaster' )
BEGIN
	alter table ItemMaster add ItemImage varchar(50)
END
GO


IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[ColorsMaster]') AND type in (N'U'))
Create table ColorsMaster(
	ColorID int Identity(1,1) primary key,
	ColorName nvarchar(30) NOT NULL,
	ColorImage varchar(800) ,
	Active tinyint NOT NULL  default(1),
	DateCreated datetime NOT NULL default(getdate()),
	DateModified datetime NOT NULL default(getdate()))
	

GO
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[PatternsMaster]') AND type in (N'U'))
Create table PatternsMaster(
	PatternID int Identity(1,1) primary key,
	PatternName nvarchar(30) NOT NULL,
	PatternImage varchar(800) ,
	Active tinyint NOT NULL default(1),
	DateCreated datetime NOT NULL default(getdate()),
	DateModified datetime NOT NULL default(getdate()))
	

GO


IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[BrandsMaster]') AND type in (N'U'))
Create table BrandsMaster(
	BrandID int Identity(1,1) primary key,
	BrandName nvarchar(30) NOT NULL,
	Active tinyint NOT NULL default(1),
	DateCreated datetime NOT NULL default(getdate()),
	DateModified datetime NOT NULL default(getdate()))
	

GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[CommentsMaster]') AND type in (N'U'))
Create table CommentsMaster(
	CommentID int Identity(1,1) primary key,
	CommentName nvarchar(30) NOT NULL,
	Active tinyint NOT NULL default(1),
	DateCreated datetime NOT NULL default(getdate()),
	DateModified datetime NOT NULL default(getdate()))
	

GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[CategoriesMaster]') AND type in (N'U'))
Create table CategoriesMaster(
	CategoryID int Identity(1,1) primary key,
	CategoryName nvarchar(30) NOT NULL,
	CategoryImage varchar(200) ,
	Active tinyint NOT NULL default(1),
	DateCreated datetime NOT NULL default(getdate()),
	DateModified datetime NOT NULL default(getdate()))
	

GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[VariationsMaster]') AND type in (N'U'))
Create table VariationsMaster(
	VariationID int Identity(1,1) primary key,
	VariationName nvarchar(30) NOT NULL,
	Active tinyint NOT NULL default(1),
	DateCreated datetime NOT NULL default(getdate()),
	DateModified datetime NOT NULL default(getdate()))
	

GO

--drop table ItemCategories
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[ItemCategories]') AND type in (N'U'))
Create table ItemCategories(
	ItemCategoryID int Identity(1,1) primary key,
	ItemID int NOT NULL,
	CategoryID int NOT NULL,
	DefaultSelected tinyint NOT NULL default(0),
	Active tinyint NOT NULL default(1),
	DateCreated datetime NOT NULL default(getdate()),
	DateModified datetime NOT NULL default(getdate()))
	

GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[ItemVariations]') AND type in (N'U'))
Create table ItemVariations(
	ItemVariationID int Identity(1,1) primary key,
	ItemID int NOT NULL,
	VariationID int NOT NULL,
	DefaultSelected tinyint NOT NULL default(0),
	Active tinyint NOT NULL default(1),
	DateCreated datetime NOT NULL default(getdate()),
	DateModified datetime NOT NULL default(getdate()))
	

GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[SubItems]') AND type in (N'U'))
Create table SubItems(
	SubItemID int Identity(1,1) primary key,
	ItemID int NOT NULL,
	SubItemRefID int NOT NULL,
	DefaultSelected tinyint NOT NULL default(0),
	Active tinyint NOT NULL default(1),
	DateCreated datetime NOT NULL default(getdate()),
	DateModified datetime NOT NULL default(getdate()))

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetAllItemsDetails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_GetAllItemsDetails]
GO
create procedure usp_GetAllItemsDetails AS
begin
	select ItemID, ItemName, ItemCode, ItemImage from ItemMaster where Active = 1 order by isnull(SortOrder, 2000)
	select ItemID, CategoryID, DefaultSelected from ItemCategories where Active = 1
	select ItemID, VariationID, DefaultSelected from ItemVariations where Active = 1
	Select ItemID, SubItemRefID, DefaultSelected from SubItems where Active = 1
end
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[Booking]') AND type in (N'U'))
Create table Booking(
	BookingID int Identity(1,1) NOT NULL,
	BookingNumber varchar(25) NOT NULL,
	IsHomeReceipt tinyint NOT NULL default(0),
	HomeReceiptNumber varchar(25),
	CustomerID int NOT NULL,
	DueDate datetime,
	DueTime datetime,
	IsUrgent tinyint NOT NULL default(0),
	IsSMS tinyint NOT NULL default(0),
	IsEmail tinyint NOT NULL Default(0),
	ReceiptRemarks varchar(4000),
	SalesManUserID int NOT NULL,
	CheckedByUserID int NOT NULL,
	Quantity smallint NOT NULL,
	TotalGrossAmount float,
	TotalDiscount float,
	TotalTax float,
	TotalAdvance float,
	ReceiptStatus smallint NOT NULL, -- OPen/Draft, Submitted/Saved/Booked, Delivered (with Pay options), Closed, Cancelled
	Active tinyint NOT NULL default(1),
	DateCreated datetime default getdate(),
	DateModified datetime default getdate(),
	CreatedBy int NOT NULL,
	ModifiedBy int NOT NULL
)
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[Booking_Items]') AND type in (N'U'))
Create table Booking_Items(
	Booking_ItemID int IDENTITY(1,1) NOT NULL,
	BookingID int NOT NULL,
	ItemID int NOT NULL,
	SubItemCount smallint NOT NULL,
	ProcessCount smallint NOT NULL,
	Sequence smallint not null,
	CategoryID int NOT NULL,
	VariationID int NOT NULL,
	Active tinyint NOT NULL default(1),
	DateCreated datetime default getdate(),
	DateModified datetime default getdate(),
	CreatedBy int NOT NULL,
	ModifiedBy int NOT NULL
)
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[Booking_Items_Processes]') AND type in (N'U'))
Create table Booking_Items_Processes(
	Booking_ItemProcessID int IDENTITY(1,1) NOT NULL,
	Booking_ItemID int NOT NULL,
	ProcessID int not null,
	ProcessRate float not null,
	Active tinyint NOT NULL default(1),
	DateCreated datetime default getdate(),
	DateModified datetime default getdate(),
	CreatedBy int NOT NULL,
	ModifiedBy int NOT NULL
)
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[Booking_Items_SubItems]') AND type in (N'U'))
Create table Booking_Items_SubItems(
	Booking_ItemSubItemID int IDENTITY(1,1) NOT NULL,
	SubItemID int NOT NULL,
	Active tinyint NOT NULL default(1),
	DateCreated datetime default getdate(),
	DateModified datetime default getdate(),
	CreatedBy int NOT NULL,
	ModifiedBy int NOT NULL
)
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[Booking_Items_Colors]') AND type in (N'U'))
Create table Booking_Items_Colors(
	Booking_ItemColorID int IDENTITY(1,1) NOT NULL,
	ColorID int NOT NULL,
	Active tinyint NOT NULL default(1),
	DateCreated datetime default getdate(),
	DateModified datetime default getdate(),
	CreatedBy int NOT NULL,
	ModifiedBy int NOT NULL
)
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[Booking_Items_Brands]') AND type in (N'U'))
Create table Booking_Items_Brands(
	Booking_ItemBrandID int IDENTITY(1,1) NOT NULL,
	BrandID int NOT NULL,
	Active tinyint NOT NULL default(1),
	DateCreated datetime default getdate(),
	DateModified datetime default getdate(),
	CreatedBy int NOT NULL,
	ModifiedBy int NOT NULL
)
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[Booking_Items_Comments]') AND type in (N'U'))
Create table Booking_Items_Comments(
	Booking_ItemCommentID int IDENTITY(1,1) NOT NULL,
	CommentID int NOT NULL,
	Active tinyint NOT NULL default(1),
	DateCreated datetime default getdate(),
	DateModified datetime default getdate(),
	CreatedBy int NOT NULL,
	ModifiedBy int NOT NULL
)
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[ItemTracking]') AND type in (N'U'))
Create table ItemTracking(
	TrackingID int IDENTITY(1,1) NOT NULL,
	BookingID int NOT NULL,
	ItemID int NOT NULL,
	IsSubItem tinyint NOT NULL,
	SubItemID int,
	SubItemItemID int,
	TrackingStatus tinyint NOT NULL, --Booked, BarCode generated, Processing Status, Ready
	Barcode varchar(200),
	Active tinyint NOT NULL default(1),
	DateCreated datetime default getdate(),
	DateModified datetime default getdate(),
	CreatedBy int NOT NULL,
	ModifiedBy int NOT NULL
)
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[BranchChallanHeader]') AND type in (N'U'))
Create table BranchChallanHeader(
	BranchChallanHeaderID int IDENTITY(1,1) NOT NULL,
	BookingChallanNumber varchar(100) NOT NULL,
	TotalItemCount int,
	OutDatetime datetime,
	InDatetime datetime,
	BranchChallanStatus tinyint NOT NULL, --Prepared, Sent, Received, Cancelled
	Active tinyint NOT NULL default(1),
	DateCreated datetime default getdate(),
	DateModified datetime default getdate(),
	CreatedBy int NOT NULL,
	ModifiedBy int NOT NULL
)
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[BranchChallanDetail]') AND type in (N'U'))
Create table BranchChallanDetail(
	BranchChallanDetailID int IDENTITY(1,1) NOT NULL,
	BranchChallanHeaderID int NOT NULL,
	TrackingID int NOT NULL,
	Active tinyint NOT NULL default(1),
	DateCreated datetime default getdate(),
	DateModified datetime default getdate(),
	CreatedBy int NOT NULL,
	ModifiedBy int NOT NULL
)
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[FactoryChallanHeader]') AND type in (N'U'))
Create table FactoryChallanHeader(
	FactoryChallanHeaderID int IDENTITY(1,1) NOT NULL,
	FactoryChallanNumber varchar(100) NOT NULL,
	TotalItemCount int,
	OutDatetime datetime,
	InDatetime datetime,
	FactoryChallanStatus tinyint NOT NULL, --Prepared, Sent, Received, Cancelled
	Active tinyint NOT NULL default(1),
	DateCreated datetime default getdate(),
	DateModified datetime default getdate(),
	CreatedBy int NOT NULL,
	ModifiedBy int NOT NULL
)
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[FactoryChallanDetail]') AND type in (N'U'))
Create table FactoryChallanDetail(
	FactoryChallanDetailID int IDENTITY(1,1) NOT NULL,
	FactoryChallanHeaderID int NOT NULL,
	TrackingID int NOT NULL,
	Active tinyint NOT NULL default(1),
	DateCreated datetime default getdate(),
	DateModified datetime default getdate(),
	CreatedBy int NOT NULL,
	ModifiedBy int NOT NULL
)
GO
