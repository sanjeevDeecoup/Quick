/*
   Tuesday, August 28, 201211:13:40 AM
   User: sa
   Server: 192.168.1.101
   Database: DrySoftBranch
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.mstReceiptConfig ADD
	BarCodeFontName varchar(50) NULL
	
ALTER TABLE dbo.mstReceiptConfig ADD
BarCodeFontSize varchar(50) NULL

ALTER TABLE dbo.mstReceiptConfig ADD CONSTRAINT
	DF_mstReceiptConfig_LogoOption DEFAULT 0 FOR LogoOption

ALTER TABLE dbo.mstReceiptConfig ADD CONSTRAINT
	DF_mstReceiptConfig_ShopOption DEFAULT 0 FOR ShopOption

ALTER TABLE dbo.mstReceiptConfig ADD
LogoAlign varchar(50) NULL

ALTER TABLE dbo.mstReceiptConfig ADD
ShopName varchar(50) NULL

ALTER TABLE dbo.mstReceiptConfig ADD
PrinterName varchar(50) NULL

ALTER TABLE dbo.mstReceiptConfig ADD
LogoPosition varchar(50) NULL


GO
COMMIT
