create table masterData (
ItemID int null,
ItemName varchar(200) null,
CategoryID int null,
CategoryName varchar(200) null,
CategorySelected tinyint null,
VariationID int null,
VariationName varchar(200) null,
VariationSelected tinyint null,
SubItemID int null,
SubItemName varchar(200) null,
SubItemSelected tinyint null )

--drop table masterData

--Insert Items
--Select * into tmpItemMaster from ItemMaster
-- truncate table itemMaster
insert into ItemMaster(ItemName, Active) 
select distinct ItemName, 1 from masterData

Update masterData set ItemID = i.ItemID from masterData m inner join ItemMaster i
on m.ItemName = i.ItemName

--select * from masterData
update masterData set CategoryName=null where CategoryName='-'
--select * from masterData where CategoryName='-'
select distinct categoryName from masterData
--update masterData set CategoryName= 'Institutional' where CategoryName = 'Insitutional'
--select * into tmpCategoryMaster from CategoriesMaster
--Truncate table CategoriesMaster
insert into CategoriesMaster (CategoryName)
select distinct CategoryName from masterData where CategoryName is NOT NULL

Update masterData set CategoryID = c.CategoryID from masterData m inner join CategoriesMaster c
on m.CategoryName = c.CategoryName

--select * from masterData
select * into tmpVariationsMaster from VariationsMaster
--truncate table VariationsMaster
update masterData set VariationName = null where VariationName= '-'
--update masterData set VariationID = null where VariationName is null
insert into VariationsMaster (VariationName)
select distinct VariationName from masterData where VariationName is not null

Update masterData set VariationID = v.VariationID from masterData m inner join VariationsMaster v
on m.VariationName = v.VariationName where m.VariationName is not null
--select * from masterData order by VariationID
-- select * from VariationsMaster

select * into tmpSubItems from subItems
--truncate table subItems
select distinct subItemName from masterData
update masterData set SubItemName = null where SubItemName = '-'

insert into ItemMaster (ItemName, Active)
select distinct subItemName, 1 from masterData where SubItemName is not null and SubItemID is null
--select i.ItemName, m.SubItemName, * from ItemMaster i inner join masterData m on i.ItemName = m.subitemName
update masterData set SubItemID = i.ItemID from ItemMaster i inner join masterData m on
m.SubItemName = i.ItemName where m.SubItemID is null
--select * from masterData

select * into tmpItemCategories from ItemCategories
--Truncate table ItemCategories
insert into ItemCategories(ItemID, CategoryID, DefaultSelected)
select ItemID, CategoryID, isnull(CategorySelected,0) from masterData where CategoryID is not null
--select * from ItemCategories

select * into tmpItemVariations from ItemVariations
--Truncate table ItemVariations
Insert into ItemVariations(ItemID, VariationID, DefaultSelected)
select ItemID, VariationID, isnull(VariationSelected,0) from masterData where VariationID is not null

select * into tmpSubItems_1 from subItems
--Truncate table subItems
Insert into SubItems(ItemID, SubItemRefID, DefaultSelected)
Select ItemID, SubItemID, isnull(SubItemSelected,0) from masterData where SubItemID is not null
select * from SUbItems


select * from ItemMaster order by ItemID

select * from CategoriesMaster order by CategoryID

select * from VariationsMaster order by VariationID

select * from ItemCategories

select * from itemVariations

select * from SubItems

select * from ColorsMaster order by ColorID

select * from CommentsMaster order by CommentID

select * from BrandsMaster order by BrandID

select * from PatternsMaster order by PatternID