
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


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetAllCustomerDetails]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].usp_GetAllCustomerDetails
GO
create procedure usp_GetAllCustomerDetails (@BranchID int)  AS
begin
	select
	--RANK() over (order by ID, c.BranchID) as ID, 
	ID as CustomerID,
	CustomerSalutation + ' ' + CustomerName as Name, ISNULL(CustomerPhone, CustomerMobile) as Phone, 
	CustomerAddress as Address, p.Priority as Priority, ISNULL(Remarks, '') as Remarks, DefaultDiscountRate as Discount from CustomerMaster c inner join PriorityMaster p
	on c.CustomerPriority = p.PriorityID and c.BranchID = p.BranchID
	where c.BranchID = @BranchID
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetAllProcesses]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].usp_GetAllProcesses
GO
create procedure usp_GetAllProcesses (@BranchID int)  AS
begin
Select ProcessID, ProcessImage, ProcessName, ProcessCode, IsDiscount, ServiceTax
from ProcessMaster where Active = 1 and BranchID = @BranchID order by ProcessID 
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetAllItems]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].usp_GetAllItems
GO
create procedure usp_GetAllItems (@BranchID int)  AS
begin
	select ItemID, ItemName, ItemCode, ItemImage from ItemMaster where Active = 1 and BranchID = @BranchID order by isnull(SortOrder, 2000)
	select ItemID, CategoryID, DefaultSelected from ItemCategories where Active = 1  and BranchID = @BranchID 
	select ItemID, VariationID, DefaultSelected from ItemVariations where Active = 1 and BranchID = @BranchID 
	Select ItemID, SubItemRefID, DefaultSelected from SubItems where Active = 1 and BranchID = @BranchID 
end
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetAllCategories]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].usp_GetAllCategories
GO
create procedure usp_GetAllCategories (@BranchID int)  AS
begin
	select CategoryID, CategoryName, '' as CategoryCode, CategoryImage from CategoriesMaster where Active=1 and BranchID = @BranchID
end
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetAllPatterns]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].usp_GetAllPatterns
GO
create procedure usp_GetAllPatterns (@BranchID int)  AS
begin
	select PatternID, PatternName, '' as PatternCode, PatternImage from PatternsMaster where Active = 1 and BranchID = @BranchID
end
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetAllColors]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].usp_GetAllColors
GO
create procedure usp_GetAllColors (@BranchID int)  AS
begin
	select ColorID, ColorName, '' as ColorCode, ColorImage from ColorsMaster where Active = 1 and BranchID = @BranchID
end
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetAllBrands]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].usp_GetAllBrands
GO
create procedure usp_GetAllBrands (@BranchID int)  AS
begin
	select BrandID, BrandName from BrandsMaster where Active = 1 and BranchID = @BranchID
end
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetAllVariations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].usp_GetAllVariations
GO
create procedure usp_GetAllVariations (@BranchID int)  AS
begin
	select VariationID, VariationName from VariationsMaster where Active = 1 and BranchID = @BranchID
end
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetAllComments]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].usp_GetAllComments
GO
create procedure usp_GetAllComments (@BranchID int)  AS
begin
	select CommentID, CommentName from CommentsMaster where Active = 1 and BranchID = @BranchID
end
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetBookingDefaults]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].usp_GetBookingDefaults
GO
create procedure usp_GetBookingDefaults (@BranchID int)  AS
begin
	--Defaults
	select DefaultItemId, ProcessId as DefaultProcessID, StartBookingNumberFrom, 
	DefaultTime + ' ' + DefaultAmPm as DefaultTime, DeliveryDateOffset as DefaultDateOffset
	from MstConfigSettings d inner join ProcessMaster p
	on p.BranchId = d.BranchId and d.DefaultProcessCode = p.ProcessCode
	where d.BranchId = @BranchID
	 
	 --Urgent
	 select '1'as UrgentSeq, TodayExtendDay as UrgentOffset, TodayRate as UrgentRate from MstConfigSettings 
	 where BranchId = @BranchID
	 union
	 select '2'as UrgentSeq, NextDayExtendDay as UrgentOffset, NextDayRate as UrgentRate from MstConfigSettings
	 where BranchId = @BranchID
end
GO

/****** Object:  UserDefinedFunction [dbo].[MySplit]    Script Date: 05/09/2012 08:15:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MySplit]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[MySplit]
GO
CREATE FUNCTION [dbo].[MySplit]
(
@SplitString nvarchar(1000)='',
@SplitCharacter nvarchar(5)=''
)
RETURNS @RtnValue table
(
ID int Identity(1,1),
Data nvarchar(50),
Data1 nvarchar(50),
BranchId nvarchar(50)
)
AS
BEGIN
Declare @Count int
Set @Count = 1

While (Charindex(@SplitCharacter,@SplitString)>0)
Begin
Insert Into @RtnValue (Data)
Select
Data = ltrim(rtrim(Substring(@SplitString,1,Charindex(@SplitCharacter,@SplitString)-1)))

Set @SplitString = Substring(@SplitString,Charindex(@SplitCharacter,@SplitString)+1,len(@SplitString))
Set @Count = @Count + 1
End

Insert Into @RtnValue (Data)
Select Data = ltrim(rtrim(@SplitString))

Return
END
GO

/****** Object:  StoredProcedure [dbo].[sp_Item]    Script Date: 05/09/2012 08:18:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Item]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Item]
GO
CREATE PROCEDURE [dbo].[sp_Item]   
 @Flag VARCHAR(MAX)='',  
 @ItemName VARCHAR(MAX)='',  
 @OldItemName VARCHAR(MAX)='',  
 @NoOfItem VARCHAR(MAX)='',  
 @ItemCode VARCHAR(MAX)='',  
 @BranchId VARCHAR(MAX)='',  
 @ExternalBranchId VARCHAR(MAX)='',  
 @SubItemName VARCHAR(MAX)='',  
 @SubItemOrder VARCHAR(MAX)='',  
 @ItemID VARCHAR(MAX)='',  
 @ItemImage VarChar(max)='',  
 @ItemSubItemRef Varchar(max)='',  
 @ItemCategoryId Varchar(Max)='',  
 @ItemVariationId Varchar(max)=''  
   
AS  
BEGIN   
 IF(@Flag = 1)  
  BEGIN  
   INSERT INTO ItemMaster (ItemName, NumberOfSubItems,ItemCode,BranchId) VALUES (@ItemName, @NoOfItem,@ItemCode,@BranchId)  
  END   
 IF(@Flag = 2)  
  BEGIN  
   INSERT INTO EntSubItemDetails (ItemName, SubItemName, SubItemOrder,BranchId) VALUES (@ItemName, @SubItemName, @SubItemOrder,@BranchId)  
  END  
 IF(@Flag = 3)  
  BEGIN  
   SELECT * FROM ItemMaster WHERE BranchId=@BranchId AND ItemName LIKE '%'+@ItemName+'%' OR ItemCode Like '%'+@ItemName+'%' ORDER BY ItemName  
  END  
 IF(@Flag = 4)  
  BEGIN  
   UPDATE ItemMaster Set ItemName=@ItemName, NumberOfSubItems=@NoOfItem, ItemCode=@ItemCode WHERE ItemID=@ItemID AND BranchId=@BranchId  
  END  
 IF(@Flag = 5)  
  BEGIN  
   DELETE FROM EntSubItemDetails WHERE ItemName = @OldItemName AND BranchId=@BranchId  
  END  
 IF(@Flag = 6)  
  BEGIN  
   --declare @Vars varchar(1000), @Cats varchar(1000), @SubItems varchar(1000), @tempid int
	--SELECT @tempid=Itemid, @Vars = COALESCE(@Vars + ',', '') + convert(varchar(10), VariationID) + '~' + convert(varchar(10), DefaultSelected) FROM ItemVariations where ItemID=@ItemID
	--SELECT @Cats = COALESCE(@Cats + ',', '') + convert(varchar(10), CategoryID) + '~' + convert(varchar(10), DefaultSelected) FROM ItemCategories where ItemID=@ItemID
	--SELECT @SubItems = COALESCE(@SubItems + ',', '') + convert(varchar(10), SubItemRefID) + '~' + convert(varchar(10), DefaultSelected) FROM ItemCategories where ItemID=@ItemID
    SELECT * FROM ItemMaster WHERE BranchId=@BranchId ORDER BY ItemName  
  END  
 IF(@Flag = 7)  
  BEGIN  
   UPDATE EntBookingDetails Set ItemName=@ItemName Where ItemName=@OldItemName AND BranchId=@BranchId  
   UPDATE ItemWiseProcessRate Set ItemName=@ItemName Where ItemName=@OldItemName AND BranchId=@BranchId  
   UPDATE BarcodeTable Set Item=@ItemName Where Item=@OldItemName AND BranchId=@BranchId  
  END  
 IF(@Flag=8)   
  BEGIN  
   INSERT INTO ItemMaster (ItemName, ItemCode,BranchId,ItemImage,Active) VALUES (@ItemName, @ItemCode,@BranchId,@ItemImage,1)  
   Select @ItemId=Scope_Identity()  
   --Update all SubItem Records
   update SubItems set Active=0 where ItemId=@ItemId
   --Insert new records
   INSERT INTO SubItems (ItemId,SubItemRefID, DefaultSelected, BranchId)  
	    select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemSubItemRef,',')

   --Update all Category Records
   update ItemCategories set Active=0 where ItemId=@ItemId
   --Insert new records
   Insert Into ItemCategories(ItemID,CategoryID,DefaultSelected,BranchId)  
		select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemCategoryId,',')

   --Update all Variation Records
   update ItemVariations set Active=0 where ItemId=@ItemId
   --Insert new records
   Insert Into ItemVariations(ItemId,VariationID,DefaultSelected,BranchId)  
		select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemVariationId,',')
/*
   if exists(select * from SubItems where ItemId=@ItemId and SubItemRefID in (select (select data from MySplit(Data,'~') where ID = 1) from dbo.MySplit(@ItemSubItemRef,',')))  
   Begin  
    update SubItems set Active=0 where ItemId=@ItemId and SubItemRefID in (select (select data from MySplit(Data,'~') where ID = 1) from dbo.MySplit(@ItemSubItemRef,','))  
   INSERT INTO SubItems (ItemId,SubItemRefID, DefaultSelected, BranchId)  
    select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemSubItemRef,',')
    --select @ItemId,Data,@BranchId from dbo.MySplit(@ItemSubItemRef,',')  
   End  
    Else  
     Begin  
     INSERT INTO SubItems (ItemId,SubItemRefID,DefaultSelected,BranchId)  
      select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemSubItemRef,',')
      --select @ItemId,Data,@BranchId from dbo.MySplit(@ItemSubItemRef,',')  
     End  
   if exists(select * from ItemCategories where ItemId=@ItemId and CategoryId in (select (select data from MySplit(Data,'~') where ID = 1) from dbo.MySplit(@ItemCategoryId,',')))  
    Begin  
     update ItemCategories set Active=0 where ItemId=@ItemId and CategoryId in(select (select data from MySplit(Data,'~') where ID = 1) from dbo.MySplit(@ItemCategoryId,','))  
      Insert Into ItemCategories(ItemID,CategoryID,DefaultSelected,BranchId)  
        select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemCategoryId,',')
        --select @ItemId,Data,@BranchId from dbo.MySplit(@ItemCategoryId,',')  
    End  
     Else  
      Begin  
      Insert Into ItemCategories(ItemID,CategoryID,DefaultSelected,BranchId)  
        select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemCategoryId,',')
        --select @ItemId,Data,@BranchId from dbo.MySplit(@ItemCategoryId,',')  
      End  
   if exists(select * from ItemVariations where ItemId=@ItemId and VariationID in (select (select data from MySplit(Data,'~') where ID = 1) from dbo.MySplit(@ItemVariationId,',')))  
    Begin  
     update ItemVariations set Active=0 where ItemId=@ItemId and VariationID in (select (select data from MySplit(Data,'~') where ID = 1) from dbo.MySplit(@ItemVariationId,','))  
     Insert Into ItemVariations(ItemId,VariationID,DefaultSelected,BranchId)  
       select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemVariationId,',')
       --select @ItemId,Data,@BranchId from dbo.MySplit(@ItemVariationId,',')  
    End  
    else
     Begin  
      Insert Into ItemVariations(ItemId,VariationID,DefaultSelected,BranchId)  
        select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemVariationId,',')
        --select @ItemId,Data,@BranchId from dbo.MySplit(@ItemVariationId,',')  
     End  
    */
 END   
 IF(@Flag=9)  
  BEGIN  
  SELECT * FROM ItemMaster where BranchId=@BranchId and Active=1 Order By ItemName  
  END  
 IF(@Flag=10)  
 Begin  
 Select ItemName,ItemCode,ItemImage from ItemMaster where BranchId=@BranchId and ItemId=@ItemId  
 End  
  
 if(@Flag=11)  
  Begin  
   if exists(Select * from ItemMaster where BranchId=@BranchId and ItemId=@ItemId)  
    Begin  
    update ItemMaster set Active=0 where BranchId=@BranchId and ItemId=@ItemId  
    End  
  if exists(select * from SubItems where ItemId=@ItemId and SubItemRefID in (select data from dbo.MySplit(@ItemSubItemRef,',')))  
   Begin   
    update SubItems set Active=0 where ItemId=@ItemId and SubItemRefID in (select data from dbo.MySplit(@ItemSubItemRef,','))  
   End  
  if exists(select * from ItemCategories where ItemId=@ItemId and CategoryId in (select data from dbo.MySplit(@ItemCategoryId,',')))  
   Begin  
    update ItemCategories set Active=0 where ItemId=@ItemId and CategoryId in(select data from dbo.MySplit(@ItemCategoryId,','))  
   End  
  if exists(select * from ItemVariations where ItemId=@ItemId and VariationID in (Select data from dbo.MySplit(@ItemVariationId,',')))  
   Begin   
    update ItemVariations set Active=0 where ItemId=@ItemId and VariationID in (Select data from dbo.MySplit(@ItemVariationId,','))  
   End  
  End  
 if(@Flag=12)  
  Begin  

  Update ItemMaster set ItemName=@ItemName,ItemCode=@ItemCode,ItemImage=@ItemImage,Active=1 where ItemId=@ItemId and BranchId=@BranchId  
   --Update all SubItem Records
   update SubItems set Active=0 where ItemId=@ItemId
   --Insert new records
   INSERT INTO SubItems (ItemId,SubItemRefID, DefaultSelected, BranchId)  
	    select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemSubItemRef,',')

   --Update all Category Records
   update ItemCategories set Active=0 where ItemId=@ItemId
   --Insert new records
   Insert Into ItemCategories(ItemID,CategoryID,DefaultSelected,BranchId)  
		select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemCategoryId,',')

   --Update all Variation Records
   update ItemVariations set Active=0 where ItemId=@ItemId
   --Insert new records
   Insert Into ItemVariations(ItemId,VariationID,DefaultSelected,BranchId)  
		select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemVariationId,',')

/*
  if exists(select * from SubItems where ItemId=@ItemId and SubItemRefID in (select (select data from MySplit(Data,'~') where ID = 1) from dbo.MySplit(@ItemSubItemRef,',')))  
   Begin  
    update SubItems set Active=0 where ItemId=@ItemId and SubItemRefID in (select (select data from MySplit(Data,'~') where ID = 1) from dbo.MySplit(@ItemSubItemRef,','))  
   INSERT INTO SubItems (ItemId,SubItemRefID, DefaultSelected, BranchId)  
    select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemSubItemRef,',')
    --select @ItemId,Data,@BranchId from dbo.MySplit(@ItemSubItemRef,',')  
   End  
    Else  
     Begin  
     INSERT INTO SubItems (ItemId,SubItemRefID,DefaultSelected,BranchId)  
      select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemSubItemRef,',')
      --select @ItemId,Data,@BranchId from dbo.MySplit(@ItemSubItemRef,',')  
     End  
   if exists(select * from ItemCategories where ItemId=@ItemId and CategoryId in (select (select data from MySplit(Data,'~') where ID = 1) from dbo.MySplit(@ItemCategoryId,',')))  
    Begin  
     update ItemCategories set Active=0 where ItemId=@ItemId and CategoryId in(select (select data from MySplit(Data,'~') where ID = 1) from dbo.MySplit(@ItemCategoryId,','))  
      Insert Into ItemCategories(ItemID,CategoryID,DefaultSelected,BranchId)  
        select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemCategoryId,',')
        --select @ItemId,Data,@BranchId from dbo.MySplit(@ItemCategoryId,',')  
    End  
     Else  
      Begin  
      Insert Into ItemCategories(ItemID,CategoryID,DefaultSelected,BranchId)  
        select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemCategoryId,',')
        --select @ItemId,Data,@BranchId from dbo.MySplit(@ItemCategoryId,',')  
      End  
   if exists(select * from ItemVariations where ItemId=@ItemId and VariationID in (select (select data from MySplit(Data,'~') where ID = 1) from dbo.MySplit(@ItemVariationId,',')))  
    Begin  
     update ItemVariations set Active=0 where ItemId=@ItemId and VariationID in (select (select data from MySplit(Data,'~') where ID = 1) from dbo.MySplit(@ItemVariationId,','))  
     Insert Into ItemVariations(ItemId,VariationID,DefaultSelected,BranchId)  
       select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemVariationId,',')
       --select @ItemId,Data,@BranchId from dbo.MySplit(@ItemVariationId,',')  
    End  
    else
     Begin  
      Insert Into ItemVariations(ItemId,VariationID,DefaultSelected,BranchId)  
        select @itemID,(select data from MySplit(Data,'~') where ID = 1), (select data from MySplit(Data,'~') where ID = 2) ,@BranchID from dbo.MySplit(@ItemVariationId,',')
        --select @ItemId,Data,@BranchId from dbo.MySplit(@ItemVariationId,',')  
     End  
*/
  end
END  
GO

--drop table PriceList
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[PriceList]') AND type in (N'U'))
Create table PriceList(
	PriceID int Identity(1,1) primary key,
	ItemID int NOT NULL,
	SubItemID int,
	VariationID int,
	CategoryID int,
	ProcessID int NOT NULL,
	Price money not null,
	BranchID int NOT NULL,
	Active tinyint NOT NULL  default(1),
	DateCreated datetime NOT NULL default(getdate()),
	DateModified datetime NOT NULL default(getdate()))
	
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetPriceList]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].usp_GetPriceList
GO
CREATE PROCEDURE [dbo].usp_GetPriceList (@BranchID int)  AS
   BEGIN
		Select ItemID, isnull(SubItemID, 0) as SubItemID, isnull(VariationID, 0) as VariationID, 
			isnull(CategoryID, 0) as CategoryID, ProcessID, Price from PriceList where BranchID = @BranchID and Active = 1
   END
GO



IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[Booking_Items_Patterns]') AND type in (N'U'))
Create table Booking_Items_Patterns(
	Booking_ItemPatternID int IDENTITY(1,1) NOT NULL,
	PatternID int NOT NULL,
	BranchId varchar(10) not null,
	ExternalBranchID varchar(10) not null,
	Active tinyint NOT NULL default(1),
	DateCreated datetime default getdate(),
	DateModified datetime default getdate(),
	CreatedBy int NOT NULL,
	ModifiedBy int NOT NULL
)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='Booking_ItemID' and o.name='Booking_Items_Patterns'  )
	alter table Booking_Items_Patterns add Booking_ItemID int NOT NULL
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Proc_BookingItemPatterns]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Proc_BookingItemPatterns]
GO
CREATE Procedure [dbo].[sp_Proc_BookingItemPatterns]
(
@Booking_ItemPatternID INT='',
@Booking_ItemID int,
@PatternID INT='',
@Active TINYINT='',
@DateCreated DATETIME='',
@DateModified DATETIME='',
@CreatedBy INT='',
@ModifiedBy INT='',
@BranchID INT='',
@Flag VARCHAR(MAX)=''
)
AS
BEGIN
	IF(@Flag=1)	
---INSERT INTO BOOKINGITEMPatternS TABLE
	BEGIN
		INSERT INTO Booking_Items_Patterns(PatternID,Booking_ItemID, Active,DateCreated, DateModified,CreatedBy,ModifiedBy,BranchId) VALUES (@PatternID,@Booking_ItemID, @Active,@DateCreated, @DateModified,@CreatedBy,@ModifiedBy,@BranchId)	
	END
IF(@Flag=2)	
---UPDATE THE RECORDS IN BOOKINGITEMPatternS TABLE
	BEGIN
		UPDATE Booking_Items_Patterns SET PatternID=@PatternID,Active=@Active,DateModified=@DateModified,ModifiedBy=@ModifiedBy WHERE BranchId=@BranchId AND Booking_ItemPatternID=@Booking_ItemPatternID	
	END
IF(@Flag=3)	
---DELETE THE RECORDS FROM BOOKINGITEMPatternS
	BEGIN
		UPDATE Booking_Items_Patterns SET Active=0 WHERE BranchId=@BranchId AND Booking_ItemPatternID=@Booking_ItemPatternID
	END
IF(@Flag=4)	
	BEGIN
		SELECT Booking_ItemPatternID,PatternID,CreatedBy,ModifiedBy,BranchId,ExternalBranchId FROM dbo.Booking_Items_Patterns WHERE BranchId=@BranchId AND Active=1 		
	END
END
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='Booking_ItemID' and o.name='Booking_Items_SubItems'  )
	alter table Booking_Items_SubItems add Booking_ItemID int NOT NULL
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Proc_BookingItem_SubItems]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Proc_BookingItem_SubItems]
GO
CREATE Procedure [dbo].[sp_Proc_BookingItem_SubItems]
(
@Booking_ItemSubItemID INT='',
@Booking_ItemID int,
@SubItemID INT='',
@Active TINYINT='',
@DateCreated DATETIME='',
@DateModified DATETIME='',
@CreatedBy INT='',
@ModifiedBy INT='',
@BranchID INT='',
@Flag VARCHAR(MAX)=''
)
AS
BEGIN
	IF(@Flag=1)	
---INSERT INTO BOOKINGITEMCOLORS TABLE
	BEGIN
	INSERT INTO Booking_Items_SubItems(SubItemID,Booking_ItemID, Active, DateCreated, DateModified,CreatedBy,ModifiedBy,BranchId) VALUES (@SubItemID,@Booking_ItemID, @Active,@DateCreated, @DateModified,@CreatedBy,@ModifiedBy,@BranchId)	
	END
IF(@Flag=2)	
---UPDATE THE RECORDS IN BOOKINGITEMCOLORS TABLE
	BEGIN
	UPDATE Booking_Items_SubItems SET SubItemID=@SubItemID,Active=@Active,DateModified=@DateModified,ModifiedBy=@ModifiedBy WHERE BranchId=@BranchId AND Booking_ItemSubItemID=@Booking_ItemSubItemID	
	END
IF(@Flag=3)	
---DELETE THE RECORDS FROM BOOKINGITEMCOLORS
	BEGIN
	UPDATE Booking_Items_SubItems SET Active=0 WHERE BranchId=@BranchId AND Booking_ItemSubItemID=@Booking_ItemSubItemID
	END
IF(@Flag=4)	
	BEGIN
	SELECT Booking_ItemSubItemID,Booking_ItemID, SubItemID,BranchId,ExternalBranchId FROM dbo.Booking_Items_SubItems WHERE BranchId=@BranchId AND Active=1 		
	END

END

GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='Booking_ItemID' and o.name='Booking_Items_Brands'  )
	alter table Booking_Items_Brands add Booking_ItemID int NOT NULL
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Proc_BookingItemBrands]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Proc_BookingItemBrands]
GO
CREATE Procedure [dbo].[sp_Proc_BookingItemBrands]
(
@Booking_ItemBrandID INT='',
@Booking_ItemID int,
@BrandID INT='',
@Active TINYINT='',
@DateCreated DATETIME='',
@DateModified DATETIME='',
@CreatedBy INT='',
@ModifiedBy INT='',
@BranchID INT='',
@Flag VARCHAR(MAX)=''
)
AS
BEGIN
	IF(@Flag=1)	
---INSERT INTO BOOKINGITEMBRANDS TABLE
	BEGIN
	INSERT INTO Booking_Items_Brands(BrandID,Booking_ItemID, Active,DateCreated, DateModified,CreatedBy,ModifiedBy,BranchId) VALUES (@BrandID, @Booking_ItemID, @Active, @DateCreated, @DateModified, @CreatedBy, @ModifiedBy, @BranchId)	
	END
IF(@Flag=2)	
---UPDATE THE RECORDS IN BOOKINGITEMBRANDS TABLE
	BEGIN
	UPDATE Booking_Items_Brands SET BrandID=@BrandID,Active=@Active,DateModified=@DateModified,ModifiedBy=@ModifiedBy WHERE BranchId=@BranchId AND Booking_ItemBrandID=@Booking_ItemBrandID	
	END
IF(@Flag=3)	
---DELETE THE RECORDS FROM BOOKINGITEMBRANDS
	BEGIN
	UPDATE Booking_Items_Brands SET Active=0 WHERE BranchId=@BranchId AND Booking_ItemBrandID=@Booking_ItemBrandID
	END
IF(@Flag=4)	
	BEGIN
	SELECT Booking_ItemBrandID, Booking_ItemID, BrandID,BranchId,ExternalBranchId FROM dbo.Booking_Items_Brands WHERE BranchId=@BranchId AND Active=1 		
	END

END
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='Booking_ItemID' and o.name='Booking_Items_Colors'  )
	alter table Booking_Items_Colors add Booking_ItemID int NOT NULL
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Proc_BookingItemColors]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Proc_BookingItemColors]
GO
CREATE Procedure [dbo].[sp_Proc_BookingItemColors]
(
@Booking_ItemColorID INT='',
@Booking_ItemID int,
@ColorID INT='',
@Active TINYINT='',
@DateCreated DATETIME='',
@DateModified DATETIME='',
@CreatedBy INT='',
@ModifiedBy INT='',
@BranchID INT='',
@Flag VARCHAR(MAX)=''
)
AS
BEGIN
	IF(@Flag=1)	
---INSERT INTO BOOKINGITEMCOLORS TABLE
	BEGIN
	INSERT INTO Booking_Items_Colors(ColorID,Booking_ItemID, Active,DateCreated, DateModified,CreatedBy,ModifiedBy,BranchId) VALUES (@ColorID,@Booking_ItemID, @Active,@DateCreated, @DateModified,@CreatedBy,@ModifiedBy,@BranchId)	
	END
IF(@Flag=2)	
---UPDATE THE RECORDS IN BOOKINGITEMCOLORS TABLE
	BEGIN
	UPDATE Booking_Items_Colors SET ColorID=@ColorID,Active=@Active,DateModified=@DateModified,ModifiedBy=@ModifiedBy WHERE BranchId=@BranchId AND Booking_ItemColorID=@Booking_ItemColorID	
	END
IF(@Flag=3)	
---DELETE THE RECORDS FROM BOOKINGITEMCOLORS
	BEGIN
	UPDATE Booking_Items_Colors SET Active=0 WHERE BranchId=@BranchId AND Booking_ItemColorID=@Booking_ItemColorID
	END
IF(@Flag=4)	
	BEGIN
	SELECT Booking_ItemColorID,Booking_ItemID, ColorID,DateCreated,DateModiFied,CreatedBy,ModifiedBy,BranchId,ExternalBranchId FROM dbo.Booking_Items_Colors WHERE BranchId=@BranchId AND Active=1 		
	END

END

GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='Booking_ItemID' and o.name='Booking_Items_Comments'  )
	alter table Booking_Items_Comments add Booking_ItemID int NOT NULL
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Proc_BookingItemComments]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Proc_BookingItemComments]
GO
CREATE Procedure [dbo].[sp_Proc_BookingItemComments]
(
@Booking_ItemCommentID INT='',
@Booking_ItemID int,
@CommentID INT='',
@Active TINYINT='',
@DateCreated DATETIME='',
@DateModified DATETIME='',
@CreatedBy INT='',
@ModifiedBy INT='',
@BranchID INT='',
@Flag VARCHAR(MAX)=''
)
AS
BEGIN
	IF(@Flag=1)	
---INSERT INTO BOOKINGITEMSCOMMENTS TABLE
	BEGIN
	INSERT INTO Booking_Items_Comments(CommentID,Booking_ItemID,Active,DateCreated,DateModified,CreatedBy,ModifiedBy,BranchId) VALUES (@CommentID,@Booking_ItemID, @Active,@DateCreated,@DateModified,@CreatedBy,@ModifiedBy,@BranchId)	
	END
IF(@Flag=2)	
---UPDATE THE RECORDS IN BOOKINGITEMSCOMMENTS TABLE
	BEGIN
	UPDATE Booking_Items_Comments SET CommentID=@CommentID,Active=@Active,DateModified=@DateModified,ModifiedBy=@ModifiedBy WHERE BranchId=@BranchId AND Booking_ItemCommentID=@Booking_ItemCommentID	
	END
IF(@Flag=3)	
---DELETE THE RECORDS FROM BOOKINGITEMSCOMMENTS
	BEGIN
	UPDATE Booking_Items_Comments SET Active=0 WHERE BranchId=@BranchId AND Booking_ItemCommentID=@Booking_ItemCommentID
	END
IF(@Flag=4)	
	BEGIN
	SELECT Booking_ItemCommentID,Booking_ItemID, CommentID,DateCreated,DateModiFied,CreatedBy,ModifiedBy,BranchId,ExternalBranchId FROM dbo.Booking_Items_Comments WHERE BranchId=@BranchId AND Active=1	
	END

END



GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='Booking_ItemID' and o.name='Booking_Items_Processes'  )
	alter table Booking_Items_Processes add Booking_ItemID int NOT NULL
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Booking_Items_Process]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Booking_Items_Process]
GO
CREATE procedure [dbo].[sp_Booking_Items_Process]
	@Flag VARCHAR(MAX)='',
	@ProcessID int ='',
	@ProcessRate float ='',
	@Active tinyint ='',	
	@DateModified datetime ='',
	@DateCreated datetime,
	@CreatedBy int ='',
	@ModifiedBy int ='',
	@BranchId varchar(10) ='',
	@Booking_ItemID int,
	@Booking_ItemProcessID int =''
AS
BEGIN	
	IF(@Flag = 1)
		BEGIN		
		-- INSERT INTO Booking_Items_Processes TABLE				
				INSERT INTO Booking_Items_Processes (Booking_ItemID,ProcessID,ProcessRate,Active,CreatedBy,ModifiedBy,BranchId, DateCreated)
				 VALUES     (@Booking_ItemID, @ProcessID,@ProcessRate,@Active,@CreatedBy,@ModifiedBy,@BranchId, @DateCreated)		
		END	
	IF(@Flag = 2)
		BEGIN
		--- UPDATE THE RECORDS IN THE Booking_Items_Processes TABLE
				UPDATE Booking_Items_Processes SET ProcessID=@ProcessID,ProcessRate=@ProcessRate, ModifiedBy=@ModifiedBy,DateModified=@DateModified WHERE BranchId=@BranchId AND Booking_ItemProcessID=@Booking_ItemProcessID
		END	
	IF(@Flag = 3)
		BEGIN
		--- DELETE RECORD FROM Booking_Items_Processes TABLE
				DELETE FROM Booking_Items_Processes WHERE BranchId=@BranchId AND Booking_ItemProcessID=@Booking_ItemProcessID
		END
	IF(@Flag = 4)
		BEGIN
		--- SELECT RECORD FROM Booking_Items_Processes TABLE
				SELECT * FROM Booking_Items_Processes WHERE BranchId=@BranchId AND Booking_ItemProcessID=@Booking_ItemProcessID and Active = 1
		END
END
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='Booking_Xml' and o.name='Booking'  )
	alter table Booking add Booking_Xml varchar(MAX)
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Booking]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Booking]
GO
CREATE procedure [dbo].[sp_Booking]
	@Flag VARCHAR(MAX)='',
	@BookingNumber varchar(25) ='',
	@IsHomeReceipt tinyint ='',
	@HomeReceiptNumber varchar(25) ='',
	@CustomerID int ='',
	@DueDate datetime ='',
	@DueTime varchar(100) ='',
	@IsUrgent tinyint ='',
	@IsSMS tinyint ='',
	@IsEmail tinyint ='',
	@ReceiptRemarks varchar(4000) ='',
	@SalesManUserID int ='',
	@CheckedByUserID int ='',
	@TotalGrossAmount float ='',
	@TotalDiscount float='',
	@TotalTax float='',
	@TotalAdvance float='',
	@ReceiptStatus smallint ='',
	@Active tinyint ='',
	@DateCreated datetime ='',
	@DateModified datetime ='',
	@CreatedBy int ='',
	@ModifiedBy int ='',
	@BranchId varchar(10)='',
	@XMLName VARCHAR(50)='',
	@Booking_Xml varchar(MAX)
	
AS
BEGIN	
	IF(@Flag = 1)
		BEGIN	
		---- INSERT INTO BOOKING TABLE	
				INSERT INTO Booking (Booking_Xml, BookingNumber,IsHomeReceipt,HomeReceiptNumber,CustomerID,DueDate,DueTime,IsUrgent,IsSMS,IsEmail,ReceiptRemarks,SalesManUserID,CheckedByUserID,TotalGrossAmount,TotalDiscount,TotalTax,TotalAdvance,ReceiptStatus,Active,CreatedBy,ModifiedBy,BranchId,XMLName)
				 VALUES     (@Booking_Xml, @BookingNumber,@IsHomeReceipt,@HomeReceiptNumber,@CustomerID,@DueDate,@DueTime,@IsUrgent,@IsSMS,@IsEmail,@ReceiptRemarks,@SalesManUserID,@CheckedByUserID,@TotalGrossAmount,@TotalDiscount,@TotalTax,@TotalAdvance,@ReceiptStatus,@Active,@CreatedBy,@ModifiedBy,@BranchId,@XMLName)		
				select SCOPE_IDENTITY()
		END	
	IF(@Flag = 2)
		BEGIN
		--- UPDATE THE RECORDS IN THE BOOKING TABLE
				UPDATE Booking SET Booking_Xml= @Booking_Xml, IsHomeReceipt=@IsHomeReceipt,HomeReceiptNumber=@HomeReceiptNumber,CustomerID=@CustomerID,DueDate=@DueDate,DueTime=@DueTime,IsUrgent=@IsUrgent,IsSMS=@IsSMS,IsEmail=@IsEmail,ReceiptRemarks=@ReceiptRemarks,SalesManUserID=@SalesManUserID,CheckedByUserID=@CheckedByUserID,TotalGrossAmount=@TotalGrossAmount,TotalDiscount=@TotalDiscount,TotalTax=@TotalTax,TotalAdvance=@TotalAdvance,ReceiptStatus=@ReceiptStatus,Active=@Active,ModifiedBy=@ModifiedBy,XMLName=@XMLName, DateModified = @DateModified  WHERE BranchId=@BranchId AND BookingNumber=@BookingNumber
		END	
	IF(@Flag = 3)
		BEGIN
		--- DELETE RECORD FROM BOOKING TABLE
				DELETE FROM Booking WHERE BranchId=@BranchId AND BookingNumber=@BookingNumber
		END
	IF(@Flag = 4)
		BEGIN
		--- SELECT RECORD FROM BOOKING TABLE
				SELECT * FROM Booking WHERE BranchId=@BranchId AND BookingNumber=@BookingNumber and Active = 1
		END
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Booking_Items]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Booking_Items]
GO
CREATE procedure [dbo].[sp_Booking_Items]
	@Flag VARCHAR(MAX)='',
	@BookingID int ='',
	@ItemID int ='',
	@Quantity smallint ='',
	@SubItemCount smallint ='',
	@ProcessCount smallint ='',
	@Sequence smallint ='',
	@CategoryID int ='',
	@VariationID int ='',
	@Active tinyint ='',
	@DateCreated datetime ='',
	@DateModified datetime ='',
	@CreatedBy int ='',
	@ModifiedBy int ='',
	@BranchId varchar(10)='',
	@Booking_ItemID int=''	
	
AS
BEGIN	
	IF(@Flag = 1)
		BEGIN	
		-- INSERT INTO Booking_Items TABLE				
				INSERT INTO Booking_Items (Quantity,BookingID,ItemID,SubItemCount,ProcessCount,Sequence,CategoryID,VariationID,Active,CreatedBy,ModifiedBy,BranchId)
				 VALUES     (@Quantity,@BookingID,@ItemID,@SubItemCount,@ProcessCount,@Sequence,@CategoryID,@VariationID,@Active,@CreatedBy,@ModifiedBy,@BranchId)		
				 select SCOPE_IDENTITY()
		END	
	IF(@Flag = 2)
		BEGIN
		--- UPDATE THE RECORDS IN THE Booking_Items TABLE
				UPDATE Booking_Items SET ItemID=@ItemID,Quantity=@Quantity,SubItemCount=@SubItemCount, ProcessCount=@ProcessCount,Sequence=@Sequence,CategoryID=@CategoryID,VariationID=@VariationID,Active=@Active,CreatedBy=@CreatedBy,ModifiedBy=@ModifiedBy,DateModified=@DateModified WHERE BranchId=@BranchId AND Booking_ItemID=@Booking_ItemID
		END	
	IF(@Flag = 3)
		BEGIN
		--- DELETE RECORD FROM BOOKING TABLE
				DELETE FROM Booking_Items WHERE BranchId=@BranchId AND BookingID=@BookingID
		END
	IF(@Flag = 4)
		BEGIN
		--- SELECT RECORD FROM Booking_Items TABLE
				SELECT * FROM Booking_Items WHERE BranchId=@BranchId AND BookingID=@BookingID
		END
END

GO

if EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='Quantity' and o.name='Booking'  )
	alter table Booking drop column quantity
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='Quantity' and o.name='Booking_Items'  )
	alter table Booking_Items add quantity smallint NOT NULL
GO

if EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='DueTime' and o.name='Booking'  )
	alter table Booking alter column DueTime varchar(100)
GO

if EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='ExternalBranchID' and o.name='Booking_Items_Patterns'  )
	alter table Booking_Items_Patterns drop column ExternalBranchID
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='ExternalBranchID' and o.name='Booking_Items_Patterns'  )
	alter table Booking_Items_Patterns add ExternalBranchID varchar(10) 
GO

