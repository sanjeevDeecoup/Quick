
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[SchemeTemplates]') AND type in (N'U'))
Create table SchemeTemplates(
	TemplateID int Identity(1,1) primary key,
	SchemeName nvarchar(30) NOT NULL,
	SchemeXML varchar(8000) ,
	Active tinyint NOT NULL,
	DateCreated datetime NOT NULL default(getdate()),
	DateModified datetime NOT NULL default(getdate()))
	

GO


--truncate table SchemeTemplates
--truncate table PromoSchemes

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
	alter table ProcessMaster add Active tinyint
	update ProcessMaster set Active = 1
END
GO


if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='Active' and o.name='ItemMaster' )
BEGIN
	alter table ItemMaster add Active tinyint
	update ItemMaster set Active = 1
END
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='ItemImage' and o.name='ItemMaster' )
BEGIN
	alter table ItemMaster add ItemImage varchar(50)
END
GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[PromoSchemes]') AND type in (N'U'))
Create table PromoSchemes(
	PromoID int Identity(1,1) primary key,
	SchemeTemplateID int ,
	PromoName varchar(30) NOT NULL,
	PromoDesc varchar(2000) NOT NULL,
	PromoXML varchar(8000) NOT NULL,
	Active tinyint NOT NULL,
	DateCreated datetime NOT NULL default(getdate()),
	DateModified datetime NOT NULL default(getdate()))
GO

/****** Object:  StoredProcedure [dbo].[usp_PromoSchemes]    Script Date: 02/14/2012 19:15:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_PromoSchemes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_PromoSchemes]
GO
create procedure usp_PromoSchemes (
	@Flag tinyint,
	@PromoID int output,
	@SchemeTemplateID int ,
	@PromoName varchar(30),
	@PromoDesc varchar(2000),
	@PromoXML varchar(8000) )
AS
BEGIN
	if (@Flag = 1) --Insert
		begin
			INSERT INTO [PromoSchemes] ([SchemeTemplateID],[PromoName],[PromoDesc],[PromoXML],Active)
			VALUES(@SchemeTemplateID,@PromoName,@PromoDesc ,@PromoXML, 1)
			select @PromoID = SCOPE_IDENTITY()
		end
	else if (@Flag = 2) --Update
		begin
			update [PromoSchemes] SET [SchemeTemplateID] = @SchemeTemplateID
			,[PromoName] = @PromoName ,[PromoDesc] = @PromoDesc,[PromoXML] = @PromoXML where PromoID = @PromoID
		end
	else if (@Flag = 3) --Delete
		begin
			Update [PromoSchemes] SET Active = 1 where  PromoID = @PromoID
		end
	else if (@Flag = 4) --Select
		begin
			SELECT [PromoID],[SchemeTemplateID],[PromoName],[PromoDesc],[PromoXML] from [PromoSchemes] where Active = 1
		end
END	

GO

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = 
	OBJECT_ID(N'[dbo].[Feedback]') AND type in (N'U'))
Create table Feedback(
	FeedbackID int Identity(1,1) primary key,
	Name nvarchar(200) NOT NULL,
	comments varchar(8000) ,
	Active tinyint NOT NULL,
	DateCreated datetime NOT NULL default(getdate()),
	DateModified datetime NOT NULL default(getdate()))
	

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
--drop table ColorsMaster
--drop table PatternsMaster
--drop table CategoriesMaster
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
