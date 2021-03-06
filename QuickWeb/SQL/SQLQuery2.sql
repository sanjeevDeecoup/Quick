USE [DRYSOFT]
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_smsTimeChecking]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <6 Aug 2010>
-- Description:	<To select Challan Details for challan return>
-- =============================================
EXEC Sp_Sel_ChallanReturnDetails '1431','1440',''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_smsTimeChecking]
	(
		@BookNumberFrom varchar(15) = '',
		@BookNumberUpto varchar(10) = '',
		@ChallanShift varchar(10) = '',
		@SerialNo varchar(10) = ''
	)
AS
BEGIN
	DECLARE @SQL varchar(max)	
	SET @SQL = 'SELECT dbo.BarcodeTable.BookingNo AS BookingNumber, dbo.BarcodeTable.Item AS SubItemName, dbo.EntChallan.Urgent, dbo.BarcodeTable.Process AS ItemProcessType, CASE WHEN BarcodeTable.ItemExtraprocessType = ''0'' THEN '''' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType1, CASE WHEN BarcodeTable.ItemExtraprocessType = ''0'' THEN '''' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType2, dbo.EntChallan.ItemTotalQuantitySent, dbo.EntChallan.ItemsReceivedFromVendor, dbo.EntChallan.ItemTotalQuantitySent - dbo.EntChallan.ItemsReceivedFromVendor AS ItemsPending, dbo.BarcodeTable.RowIndex AS ISN FROM  dbo.EntChallan INNER JOIN  dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex'
	SET @SQL = @SQL + ' WHERE (EntChallan.ItemsReceivedFromVendor < EntChallan.ItemTotalQuantitySent)'
	print(@BookNumberFrom)
	IF(@BookNumberFrom <> '')
		BEGIN
			IF(@BookNumberUpto = '')
				BEGIN
					SET @BookNumberUpto = @BookNumberFrom
				END
			SET @SQL = @SQL + ' AND (EntChallan.BookingNumber = ' + @BookNumberFrom + ')'
		END		
	SET @SQL = @SQL + ' ORDER BY BarcodeTable.BookingNo, BarcodeTable.RowIndex'
	PRINT @SQL
	EXEC (@SQL)
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_SecondTimeChallanReturnDetails]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <6 Aug 2010>
-- Description:	<To select Challan Details for challan return>
-- =============================================
EXEC Sp_Sel_ChallanReturnDetails '1431','1440',''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_SecondTimeChallanReturnDetails]
	(
		@BookNumberFrom varchar(15) = '',
		@BookNumberUpto varchar(10) = '',
		@ChallanShift varchar(10) = '',
		@SerialNo varchar(10) = ''
	)
AS 
BEGIN
	DECLARE @SQL varchar(max)
	SET @SQL = 'SELECT dbo.BarcodeTable.BookingNo AS BookingNumber, dbo.BarcodeTable.Item AS SubItemName, dbo.EntChallan.Urgent, dbo.BarcodeTable.Process AS ItemProcessType, CASE WHEN BarcodeTable.ItemExtraprocessType = ''0'' THEN '''' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType1, CASE WHEN BarcodeTable.ItemExtraprocessType = ''0'' THEN '''' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType2, dbo.EntChallan.ItemTotalQuantitySent, dbo.EntChallan.ItemsReceivedFromVendor, dbo.EntChallan.ItemTotalQuantitySent - dbo.EntChallan.ItemsReceivedFromVendor AS ItemsPending, dbo.BarcodeTable.RowIndex AS ISN FROM  dbo.EntChallan INNER JOIN  dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex'
	SET @SQL = @SQL + ' WHERE (dbo.BarcodeTable.StatusId=''2'')'
	print(@BookNumberFrom)
	IF(@BookNumberFrom <> '')
		BEGIN
			IF(@BookNumberUpto = '')
				BEGIN
					SET @BookNumberUpto = @BookNumberFrom
				END
			SET @SQL = @SQL + ' AND (EntChallan.BookingNumber = ' + @BookNumberFrom + ') AND (BarcodeTable.RowIndex = ' + @BookNumberUpto + ')'
		END		
	SET @SQL = @SQL + ' ORDER BY BarcodeTable.BookingNo, BarcodeTable.RowIndex'
	PRINT @SQL
	EXEC (@SQL)
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_VendorChallanReturnDetails]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <6 Aug 2010>
-- Description:	<To select Challan Details for challan return>
-- =============================================
EXEC Sp_Sel_ChallanReturnDetails '80794','80795',''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_VendorChallanReturnDetails]
	(
		@BookNumberFrom varchar(15) = '',
		@BookNumberUpto varchar(10) = '',
		@ChallanShift varchar(10) = '',
		@VendorId varchar(15) = ''
	)
AS
BEGIN
	DECLARE @SQL varchar(max)
	SET @SQL = 'SELECT EntVendorChallan.ChallanNumber, EntVendorChallan.ChallanBranchCode, EntVendorChallan.ChallanDate, EntBookingDetails.BookingNumber, EntBookingDetails.ISN, SubItemName, CASE WHEN EntBookingDetails.ItemProcessType = ''None'' THEN '''' ELSE EntBookingDetails.ItemProcessType END As ItemProcessType, CASE WHEN EntBookingDetails.ItemExtraProcessType1= ''None'' THEN '''' WHEN EntBookingDetails.ItemProcessType = ''None'' THEN ''O'' + EntBookingDetails.ItemExtraProcessType1 ELSE EntBookingDetails.ItemExtraProcessType1 END AS ItemExtraProcessType1, CASE WHEN EntBookingDetails.ItemExtraProcessType2= ''None'' THEN '''' WHEN EntBookingDetails.ItemProcessType = ''None'' THEN ''O'' + EntBookingDetails.ItemExtraProcessType2 ELSE EntBookingDetails.ItemExtraProcessType2 END AS ItemExtraProcessType2, EntBookingDetails.ItemQuantityAndRate, EntVendorChallan.ItemTotalQuantitySent, ItemsReceivedFromVendor, (ItemTotalQuantitySent - ItemsReceivedFromVendor) AS ItemsPending, EntVendorChallan.ItemReceivedFromVendorOnDate, EntVendorChallan.Urgent FROM EntVendorChallan INNER JOIN EntBookingDetails ON EntVendorChallan.BookingNumber = EntBookingDetails.BookingNumber AND EntVendorChallan.ItemSNo = EntBookingDetails.ISN INNER JOIN ItemMaster ON EntBookingDetails.ItemName = ItemMaster.ItemName'
	SET @SQL = @SQL + ' WHERE (EntVendorChallan.ItemsReceivedFromVendor < EntVendorChallan.ItemTotalQuantitySent)'
	SET @SQL = @SQL + ' AND EntVendorChallan.VendorId = ''' + @VendorId + ''''
	print(@BookNumberFrom)
	IF(@BookNumberFrom <> '')
		BEGIN
			IF(@BookNumberUpto = '')
				BEGIN
					SET @BookNumberUpto = @BookNumberFrom
				END
			SET @SQL = @SQL + ' AND (EntVendorChallan.BookingNumber BETWEEN ' + @BookNumberFrom + ' AND ' + @BookNumberUpto + ')'
		END
	IF (@ChallanShift <> '')
		BEGIN
			SET @SQL = @SQL + ' AND ChallanSendingShift = ''' + @ChallanShift + ''''
		END
	SET @SQL = @SQL + ' ORDER BY EntBookingDetails.BookingNumber, EntBookingDetails.ISN'
	PRINT @SQL
	EXEC (@SQL)
END
GO
/****** Object:  Table [dbo].[UserTypeMaster]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserTypeMaster](
	[UserTypeID] [int] NOT NULL,
	[UserType] [varchar](30) NOT NULL,
	[UserTypeDetails] [varchar](50) NULL,
 CONSTRAINT [PK_UserTypeMaster] PRIMARY KEY CLUSTERED 
(
	[UserTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserMaster]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserMaster](
	[UserId] [varchar](20) NOT NULL,
	[UserPassword] [varchar](200) NOT NULL,
	[UserTypeCode] [tinyint] NOT NULL,
	[UserBranchCode] [varchar](10) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[UserAddress] [varchar](100) NULL,
	[UserPhoneNumber] [varchar](30) NULL,
	[UserMobileNumber] [varchar](30) NULL,
	[UserEmailId] [varchar](100) NULL,
	[UserActive] [bit] NOT NULL,
 CONSTRAINT [PK_UserMaster] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TmpChallan]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TmpChallan](
	[ChallanNumber] [int] NOT NULL,
	[ChallanBranchCode] [varchar](5) NOT NULL,
	[ChallanDate] [datetime] NOT NULL,
	[ChallanSendingShift] [varchar](10) NOT NULL,
	[BookingNumber] [varchar](15) NOT NULL,
	[ItemSNo] [int] NOT NULL,
	[ItemTotalQuantitySent] [int] NOT NULL,
	[ItemsReceivedFromVendor] [int] NOT NULL,
	[ItemReceivedFromVendorOnDate] [datetime] NULL,
	[Urgent] [bit] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 12/26/2011 11:55:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Split]
(
	@RowData nvarchar(2000),
	@SplitOn nvarchar(5)
)  
RETURNS @RtnValue table 
(
	Id int identity(1,1),
	Data nvarchar(100)
) 
AS  
BEGIN 
	Declare @Cnt int
	Set @Cnt = 1

	While (Charindex(@SplitOn,@RowData)>0)
	Begin
		Insert Into @RtnValue (data)
		Select 
			Data = ltrim(rtrim(Substring(@RowData,1,Charindex(@SplitOn,@RowData)-1)))

		Set @RowData = Substring(@RowData,Charindex(@SplitOn,@RowData)+1,len(@RowData))
		Set @Cnt = @Cnt + 1
	End
	
	Insert Into @RtnValue (data)
	Select Data = ltrim(rtrim(@RowData))

	Return
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_VendorReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <7 August 2010>
-- Description:	<To select data for vendor report>
-- =============================================
EXEC Sp_Report_VendorReport '1/1/2011','1/30/2011','',''
*/
CREATE PROCEDURE [dbo].[Sp_VendorReport]
	(
		@BookDate1 datetime,
		@BookDate2	datetime,
		@ProcessCode varchar(max)= '',
		@ItemName varchar(max) = ''
	)
AS
BEGIN
	DECLARE @SQL varchar(max)
	SET @SQL ='SELECT BookingDate, BookingNumber, SUM(ProcessCost) AS ProcessCost, Pieces
	FROM
	(
SELECT     CONVERT(varchar, dbo.EntBookings.BookingDate, 106) AS BookingDate, dbo.EntBookings.BookingNumber,dbo.EntBookingDetails.ItemQuantityAndRate, 
			dbo.EntBookingDetails.ItemTotalQuantity * dbo.ItemMaster.NumberOfSubItems AS Pieces,dbo.EntBookingDetails.ItemName,         
            dbo.EntBookingDetails.ItemSubTotal as ProcessCost,dbo.EntBookingDetails.ItemProcessType                        
FROM         dbo.EntBookings INNER JOIN
             dbo.EntBookingDetails ON dbo.EntBookings.BookingNumber = dbo.EntBookingDetails.BookingNumber INNER JOIN
             dbo.ItemMaster ON dbo.EntBookingDetails.ItemName = dbo.ItemMaster.ItemName INNER JOIN
             dbo.ProcessMaster ON dbo.EntBookingDetails.ItemProcessType = dbo.ProcessMaster.ProcessCode
	WHERE (dbo.ProcessMaster.ProcessUsedForVendorReport=''1'') and (BookingDate BETWEEN ''' + Convert(varchar,@BookDate1,106) + ''' AND ''' + Convert(varchar,@BookDate2,106) + ''')'
	IF(@ProcessCode <> '')
		BEGIN			
			SET @ProcessCode = REPLACE(@ProcessCode,',',''',''')
			SET @SQL = @SQL + ' AND (ItemProcessType IN (''' + @ProcessCode + '''))'
		END
	IF(@ItemName <>'')
		BEGIN
			SET @ItemName = REPLACE(@ItemName,',',''',''')
			SET @SQL = @SQL + ' AND (EntBookingDetails.ItemName IN ('''+ @ItemName +'''))'
		END
	SET @SQL = @SQL + ') AS T1'
	SET @SQL = @SQL + '	Group By BookingDate, BookingNumber, Pieces '
	
	PRINT @SQL
	EXEC (@SQL)
	
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_ReportVendorChallanReturnDetails]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <6 Aug 2010>
-- Description:	<To select Challan Details for challan return>
-- =============================================
EXEC Sp_Sel_ReportVendorChallanReturnDetails '1/1/2011','1/30/2011','','',''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_ReportVendorChallanReturnDetails]
	(
		@BookDate1 datetime,
		@BookDate2	datetime,
		@BookNumberFrom varchar(15) = '',
		@BookNumberUpto varchar(10) = '',
		@ChallanShift varchar(10) = '',
		@VendorId varchar(15) = ''
	)
AS
BEGIN
	DECLARE @SQL varchar(max)
	SET @SQL = 'SELECT EntVendorChallan.ChallanNumber, EntVendorChallan.ChallanBranchCode, convert(varchar,EntVendorChallan.ChallanDate,106) as ChallanDate, EntBookingDetails.BookingNumber, EntBookingDetails.ISN, SubItemName, CASE WHEN EntBookingDetails.ItemProcessType = ''None'' THEN '''' ELSE EntBookingDetails.ItemProcessType END As ItemProcessType, CASE WHEN EntBookingDetails.ItemExtraProcessType1= ''None'' THEN '''' WHEN EntBookingDetails.ItemProcessType = ''None'' THEN ''O'' + EntBookingDetails.ItemExtraProcessType1 ELSE EntBookingDetails.ItemExtraProcessType1 END AS ItemExtraProcessType1, CASE WHEN EntBookingDetails.ItemExtraProcessType2= ''None'' THEN '''' WHEN EntBookingDetails.ItemProcessType = ''None'' THEN ''O'' + EntBookingDetails.ItemExtraProcessType2 ELSE EntBookingDetails.ItemExtraProcessType2 END AS ItemExtraProcessType2, EntBookingDetails.ItemQuantityAndRate, EntVendorChallan.ItemTotalQuantitySent, ItemsReceivedFromVendor, (ItemTotalQuantitySent - ItemsReceivedFromVendor) AS ItemsPending, EntVendorChallan.ItemReceivedFromVendorOnDate, EntVendorChallan.Urgent FROM EntVendorChallan INNER JOIN EntBookingDetails ON EntVendorChallan.BookingNumber = EntBookingDetails.BookingNumber AND EntVendorChallan.ItemSNo = EntBookingDetails.ISN INNER JOIN ItemMaster ON EntBookingDetails.ItemName = ItemMaster.ItemName'
	SET @SQL = @SQL + ' WHERE (EntVendorChallan.ChallanDate BETWEEN ''' + Convert(varchar,@BookDate1,106) + ''' AND ''' + Convert(varchar,@BookDate2,106) + ''')'
	SET @SQL = @SQL + ' AND EntVendorChallan.VendorId = ''' + @VendorId + ''''
	print(@BookNumberFrom)
	IF(@BookNumberFrom <> '')
		BEGIN
			IF(@BookNumberUpto = '')
				BEGIN
					SET @BookNumberUpto = @BookNumberFrom
				END
			SET @SQL = @SQL + ' AND (EntVendorChallan.BookingNumber BETWEEN ' + @BookNumberFrom + ' AND ' + @BookNumberUpto + ')'
		END
	IF (@ChallanShift <> '')
		BEGIN
			SET @SQL = @SQL + ' AND ChallanSendingShift = ''' + @ChallanShift + ''''
		END
	SET @SQL = @SQL + ' ORDER BY EntBookingDetails.BookingNumber, EntBookingDetails.ISN'
	PRINT @SQL
	EXEC (@SQL)
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_ChallanReturnDetailsForPrint]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <6 Aug 2010>
-- Description:	<To select Challan Details for challan return>
-- =============================================
EXEC Sp_Sel_ChallanReturnDetailsForPrint '1431','1450',''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_ChallanReturnDetailsForPrint]
	(
		@BookNumberFrom varchar(15) = '',
		@BookNumberUpto varchar(10) = '',
		@ChallanShift varchar(10) = ''
	)
AS
BEGIN
	CREATE TABLE #TmpChallanForPrint(BookingNumber int, SubItemName varchar(30), ItemProcessType varchar(50), ItemToalQunatitySent int)
	DECLARE @SQL varchar(max)
	SET @SQL = 'INSERT INTO #TmpChallanForPrint (BookingNumber, SubItemName, ItemProcessType, ItemToalQunatitySent) SELECT EC.BookingNumber, SubItemName, ItemProcessType, SUM(ItemTotalQuantitySent) FROM EntChallan EC INNER JOIN EntBookingDetails EBD ON EC.BookingNumber = EBD.BookingNumber AND EC.ItemSNo = EBD.ISN'
	SET @SQL = @SQL + ' WHERE (EC.ItemsReceivedFromVendor < EC.ItemTotalQuantitySent)'
	IF(@BookNumberFrom <> '')
		BEGIN
			IF(@BookNumberUpto = '')
				BEGIN
					SET @BookNumberUpto = @BookNumberFrom
				END
			SET @SQL = @SQL + ' AND (EC.BookingNumber BETWEEN ' + @BookNumberFrom + ' AND ' + @BookNumberUpto + ')'
		END
	IF (@ChallanShift <> '')
		BEGIN
			SET @SQL = @SQL + ' AND ChallanSendingShift = ''' + @ChallanShift + ''''
		END
	SET @SQL = @SQL + ' Group By EC.BookingNumber, SubItemName, ItemProcessType'
	SET @SQL = @SQL + ' ORDER BY EC.BookingNumber'
	PRINT @SQL
	EXEC (@SQL)
	SELECT DISTINCT BookingNumber FROM #TmpChallanForPrint Order by BookingNumber
	SELECT BookingNumber, SUM(ItemToalQunatitySent) AS ItemTotalQuantitySent, SubItemName FROM #TmpChallanForPrint 
	WHERE ItemProcessType <>'None'
	GROUP BY BookingNumber, SubItemName	
	ORDER BY BookingNumber, SubItemName
	
	SELECT BookingNumber, SUM(ItemToalQunatitySent) AS ItemTotalQuantitySent, SubItemName FROM #TmpChallanForPrint 
	WHERE ItemProcessType ='None'
	GROUP BY BookingNumber, SubItemName	
	ORDER BY BookingNumber, SubItemName
	DROP TABLE  #TmpChallanForPrint
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_ChallanReturnDetails]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <6 Aug 2010>
-- Description:	<To select Challan Details for challan return>
-- =============================================
EXEC Sp_Sel_ChallanReturnDetails '1431','1440',''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_ChallanReturnDetails]
	(
		@BookNumberFrom varchar(15) = '',
		@BookNumberUpto varchar(10) = '',
		@ChallanShift varchar(10) = '',
		@SerialNo varchar(10) = ''
	)
AS
BEGIN
	DECLARE @SQL varchar(max)	
	SET @SQL = 'SELECT dbo.BarcodeTable.BookingNo AS BookingNumber, dbo.BarcodeTable.Item AS SubItemName, dbo.EntChallan.Urgent, dbo.BarcodeTable.Process AS ItemProcessType, CASE WHEN BarcodeTable.ItemExtraprocessType = ''0'' THEN '''' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType1, CASE WHEN BarcodeTable.ItemExtraprocessType = ''0'' THEN '''' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType2, dbo.EntChallan.ItemTotalQuantitySent, dbo.EntChallan.ItemsReceivedFromVendor, dbo.EntChallan.ItemTotalQuantitySent - dbo.EntChallan.ItemsReceivedFromVendor AS ItemsPending, dbo.BarcodeTable.RowIndex AS ISN FROM  dbo.EntChallan INNER JOIN  dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex'
	SET @SQL = @SQL + ' WHERE (dbo.BarcodeTable.StatusId=''2'')'
	print(@BookNumberFrom)
	IF(@BookNumberFrom <> '')
		BEGIN
			IF(@BookNumberUpto = '')
				BEGIN
					SET @BookNumberUpto = @BookNumberFrom
				END
			SET @SQL = @SQL + ' AND (EntChallan.BookingNumber = ' + @BookNumberFrom + ')'
		END		
	SET @SQL = @SQL + ' ORDER BY BarcodeTable.BookingNo, BarcodeTable.RowIndex'
	PRINT @SQL
	EXEC (@SQL)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Restore]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sanjeev Arora>
-- Create date: <25-Aug-2011>
-- Description:	<Restore DataBase>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Restore]	
AS
BEGIN	
	ALTER DATABASE DRYSOFT SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	RESTORE DATABASE DRYSOFT
	FROM DISK = N'c:\INETPUB\WWWROOT\DRYSOFT_backup.bak' WITH Replace
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Report_VendorReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <7 August 2010>
-- Description:	<To select data for vendor report>
-- =============================================
EXEC Sp_Report_VendorReport '1/Sep/2011','30/Sep/2011','',''
*/
CREATE PROCEDURE [dbo].[Sp_Report_VendorReport]
	(
		@BookDate1 datetime,
		@BookDate2	datetime,
		@ProcessCode varchar(max)= '',
		@ItemName varchar(max) = ''
	)
AS
BEGIN
	DECLARE @SQL varchar(max)
	SET @SQL ='SELECT BookingDate, BookingNumber, SUM(ProcessCost) AS ProcessCost, Pieces
	FROM
	(
	SELECT Convert(varchar, BookingDate,106) AS BookingDate, EntBookings.BookingNumber, EntBookingDetails.ItemName, EntBookingDetails.ItemTotalQuantity, EntBookingDetails.ItemTotalQuantity * ItemMaster.NumberOfSubItems As Pieces, ItemProcessType, ItemExtraProcessType1, ItemExtraProcessRate1, CASE WHEN ItemProcessType = ''None'' THEN ItemExtraProcessRate1 * EntBookingDetails.ItemTotalQuantity ELSE ItemExtraProcessRate1 END AS ProcessCost,dbo.ProcessMaster.ProcessUsedForVendorReport
	FROM EntBookings INNER JOIN EntBookingDetails ON EntBookings.BookingNumber = EntBookingDetails.BookingNumber INNER JOIN ItemMaster ON EntBookingDetails.ItemName = ItemMaster.ItemName INNER JOIN dbo.ProcessMaster ON dbo.EntBookingDetails.ItemExtraProcessType1 = dbo.ProcessMaster.ProcessCode
	WHERE (dbo.ProcessMaster.ProcessUsedForVendorReport = ''1'')'
	IF(@ProcessCode <> '')
		BEGIN			
			SET @ProcessCode = REPLACE(@ProcessCode,',',''',''')
			SET @SQL = @SQL + ' AND (ItemExtraProcessType1 IN (''' + @ProcessCode + '''))'
		END
--	IF(@ItemName <>'')
--		BEGIN
--			SET @ItemName = REPLACE(@ItemName,',',''',''')
--			SET @SQL = @SQL + ' AND (EntBookingDetails.ItemName IN ('''+ @ItemName +'''))'
--		END
	SET @SQL = @SQL + ') AS T1'
	SET @SQL = @SQL + '	Group By BookingDate, BookingNumber, Pieces '
	PRINT @SQL
	--EXEC (@SQL)
	
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Report_ServiceTaxReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Gupta>
-- Create date: <10 Oct 2011>
-- Description:	<To select data for serviceTax report>
-- =============================================
EXEC Sp_Report_VendorReport '1/Sep/2011','30/Sep/2011','',''
*/
CREATE PROCEDURE [dbo].[Sp_Report_ServiceTaxReport]
	(
		@BookDate1 datetime,
		@BookDate2	datetime,
		@SearchText varchar(max)= ''		
	)
AS
BEGIN
	DECLARE @SQL varchar(max)
	DECLARE @SQL1 varchar(max)
	DECLARE @SQL2 varchar(max)
--	declare @SearchText varchar(max)
--	set @SearchText= 'CL'''+','+'''C'''+','+'''DC'''+','+'''DY'''+','+'''RF'''+','+'''RFO'''+','+'''RE'''+','+'''ST'''+','+'''SP'''+','+'''WC'
	SET @SearchText = REPLACE(@SearchText,',',''',''')
	CREATE TABLE #TmpTable (BookingNumber varchar(20), Amount float, ProcessType varchar(200),BookingDate datetime,BookingAmount float,Status varchar(20))
	SET @SQL ='INSERT INTO #TmpTable( BookingNumber, Amount, ProcessType,BookingDate,BookingAmount,Status) SELECT BookingNumber, Amount, ProcessType,convert(varchar,BookingDate,106),BookingAmount,Status
		FROM(
	SELECT     dbo.EntBookingDetails.BookingNumber, SUM(dbo.EntBookingDetails.STPAmt) AS Amount, dbo.EntBookingDetails.ItemProcessType AS ProcessType, 
                      CONVERT(varchar, dbo.EntBookings.BookingDate, 106) AS BookingDate, 
                     abs(dbo.EntBookingDetails.ItemSubTotal - dbo.EntBookingDetails.ItemExtraProcessRate1 - dbo.EntBookingDetails.ItemExtraProcessRate2) AS BookingAmount,CASE WHEN EntBookings.ReceiptDeliverd=''True'' THEN ''Deliverd'' ELSE ''NonDeliverd'' END AS Status
FROM         dbo.EntBookingDetails INNER JOIN
                      dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber
	WHERE  (dbo.EntBookings.BookingDate BETWEEN ''' + Convert(varchar,@BookDate1,106) + ''' AND ''' + Convert(varchar,@BookDate2,106) + ''') AND ((dbo.EntBookingDetails.STPAmt <> 0))AND (dbo.EntBookingDetails.ItemProcessType IN (''' + @SearchText + '''))
GROUP BY dbo.EntBookingDetails.BookingNumber, dbo.EntBookingDetails.STPAmt, dbo.EntBookingDetails.ItemProcessType, dbo.EntBookings.BookingDate, 
                      dbo.EntBookings.NetAmount, dbo.EntBookingDetails.ItemSubTotal, dbo.EntBookingDetails.ItemExtraProcessRate1, 
                      dbo.EntBookingDetails.ItemExtraProcessRate2, dbo.EntBookings.ReceiptDeliverd) AS T1'


exec(@SQL)



SET @SQL1 ='INSERT INTO #TmpTable( BookingNumber, Amount, ProcessType,BookingDate,BookingAmount,Status) SELECT BookingNumber, Amount, ProcessType,convert(varchar,BookingDate,106),BookingAmount,Status
		FROM(
	SELECT     dbo.EntBookingDetails.BookingNumber, SUM(dbo.EntBookingDetails.STEP1Amt) AS Amount, 
                      dbo.EntBookingDetails.ItemExtraProcessType1 AS ProcessType, CONVERT(varchar, dbo.EntBookings.BookingDate, 106) AS BookingDate, 
                     abs((dbo.EntBookingDetails.ItemSubTotal - dbo.EntBookingDetails.ItemExtraProcessRate2) 
                      - (dbo.EntBookingDetails.ItemSubTotal - dbo.EntBookingDetails.ItemExtraProcessRate1 + dbo.EntBookingDetails.ItemExtraProcessRate2)) 
                      AS BookingAmount,CASE WHEN EntBookings.ReceiptDeliverd=''True'' THEN ''Deliverd'' ELSE ''NonDeliverd'' END AS Status
FROM         dbo.EntBookingDetails INNER JOIN
                      dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber
WHERE   (dbo.EntBookings.BookingDate BETWEEN ''' + Convert(varchar,@BookDate1,106) + ''' AND ''' + Convert(varchar,@BookDate2,106) + ''') AND  ((dbo.EntBookingDetails.STEP1Amt <> 0))AND (dbo.EntBookingDetails.ItemExtraProcessType1 IN (''' + @SearchText + '''))
GROUP BY dbo.EntBookingDetails.BookingNumber, dbo.EntBookingDetails.STEP1Amt, dbo.EntBookingDetails.ItemExtraProcessType1, 
                      dbo.EntBookings.BookingDate, dbo.EntBookings.NetAmount, dbo.EntBookingDetails.ItemExtraProcessRate2, dbo.EntBookingDetails.ItemSubTotal, 
                      dbo.EntBookingDetails.ItemExtraProcessRate1,dbo.EntBookings.ReceiptDeliverd) AS T1'


exec(@SQL1)

SET @SQL2 ='INSERT INTO #TmpTable( BookingNumber, Amount, ProcessType,BookingDate,BookingAmount,Status) SELECT BookingNumber, Amount, ProcessType,convert(varchar,BookingDate,106),BookingAmount,Status
		FROM(
	SELECT     dbo.EntBookingDetails.BookingNumber, SUM(dbo.EntBookingDetails.STEP2Amt) AS Amount, 
                      dbo.EntBookingDetails.ItemExtraProcessType2 AS ProcessType, CONVERT(varchar, dbo.EntBookings.BookingDate, 106) AS BookingDate, 
                     abs((dbo.EntBookingDetails.ItemSubTotal - dbo.EntBookingDetails.ItemExtraProcessRate1) 
                      - (dbo.EntBookingDetails.ItemSubTotal - dbo.EntBookingDetails.ItemExtraProcessRate1 + dbo.EntBookingDetails.ItemExtraProcessRate2)) 
                      AS BookingAmount,CASE WHEN EntBookings.ReceiptDeliverd=''True'' THEN ''Deliverd'' ELSE ''NonDeliverd'' END AS Status
FROM         dbo.EntBookingDetails INNER JOIN
                      dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber
WHERE   (dbo.EntBookings.BookingDate BETWEEN ''' + Convert(varchar,@BookDate1,106) + ''' AND ''' + Convert(varchar,@BookDate2,106) + ''') AND  ((dbo.EntBookingDetails.STEP2Amt <> 0))AND (dbo.EntBookingDetails.ItemExtraProcessType2 IN (''' + @SearchText + '''))
GROUP BY dbo.EntBookingDetails.BookingNumber, dbo.EntBookingDetails.STEP2Amt, dbo.EntBookingDetails.ItemExtraProcessType2, 
                      dbo.EntBookings.BookingDate, dbo.EntBookings.NetAmount, dbo.EntBookingDetails.ItemExtraProcessRate2, dbo.EntBookingDetails.ItemSubTotal, 
                      dbo.EntBookingDetails.ItemExtraProcessRate1,dbo.EntBookings.ReceiptDeliverd) AS T1'


exec(@SQL2)
--) as T1
select * from #TmpTable
group by ProcessType,BookingNumber,Amount,BookingDate,BookingAmount,Status
DROP TABLE #TmpTable
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Report_PaymentTypeReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Gupta>
-- Create date: <10 Oct 2011>
-- Description:	<To select data for Payment Type report>
-- =============================================
EXEC Sp_Report_VendorReport '1/Sep/2011','30/Sep/2011','',''
*/
CREATE PROCEDURE [dbo].[Sp_Report_PaymentTypeReport]
	(
		@BookDate1 datetime,
		@BookDate2	datetime,
		@SearchText varchar(MAX)= ''		
	)
AS
BEGIN
	DECLARE @SQL varchar(max)		
	SET @SearchText = REPLACE(@SearchText,',',''',''')
	CREATE TABLE #TmpTable (BookingNumber VARCHAR(MAX), Amount FLOAT,PaymentType VARCHAR(MAX))
	SET @SQL ='INSERT INTO #TmpTable(BookingNumber, Amount, PaymentType) SELECT BookingNumber, Amount,PaymentType
		FROM(
		SELECT BookingNumber, PaymentMade AS Amount,PaymentType FROM EntPayment
		WHERE (PaymentDate BETWEEN ''' + Convert(varchar,@BookDate1,106) + ''' AND ''' + Convert(varchar,@BookDate2,106) + ''') AND (PaymentType IN (''' + @SearchText + ''')) 
		) AS T1'


exec(@SQL)
SELECT * FROM #TmpTable
GROUP BY PaymentType,BookingNumber,Amount
DROP TABLE #TmpTable
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Report_NewVendorReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar>
-- Create date: <27 Jan 2011>
-- Description:	<To select data for out sourced vendor report>
-- =============================================
EXEC Sp_Report_NewVendorReport '1/1/2011','1/30/2011','','','1'
*/
CREATE PROCEDURE [dbo].[Sp_Report_NewVendorReport]
	(
		@BookDate1 datetime,
		@BookDate2	datetime,
		@ProcessCode varchar(max)= '',
		@ItemName varchar(max) = '',
		@VendorId varchar(max)=''
	)
AS
BEGIN
	DECLARE @SQL varchar(max)
	SET @SQL ='SELECT BookingDate, BookingNumber, SUM(ProcessCost) AS ProcessCost, Pieces
	FROM
	(
	SELECT CONVERT(varchar, dbo.EntBookings.BookingDate, 106) AS BookingDate, dbo.EntBookings.BookingNumber, dbo.EntBookingDetails.ItemName, dbo.EntBookingDetails.ItemTotalQuantity, dbo.EntBookingDetails.ItemTotalQuantity * dbo.ItemMaster.NumberOfSubItems AS Pieces, dbo.EntBookingDetails.ItemProcessType, dbo.EntBookingDetails.ItemExtraProcessType1, dbo.EntBookingDetails.ItemExtraProcessRate1, CASE WHEN ItemProcessType = '' None '' THEN ItemExtraProcessRate1 * EntBookingDetails.ItemTotalQuantity ELSE ItemExtraProcessRate1 END AS ProcessCost, dbo.EntVendorChallan.VendorId FROM dbo.EntBookings INNER JOIN dbo.EntBookingDetails ON dbo.EntBookings.BookingNumber = dbo.EntBookingDetails.BookingNumber INNER JOIN dbo.ItemMaster ON dbo.EntBookingDetails.ItemName = dbo.ItemMaster.ItemName INNER JOIN dbo.EntVendorChallan ON dbo.EntBookings.BookingNumber = dbo.EntVendorChallan.BookingNumber
	WHERE (BookingDate BETWEEN ''' + Convert(varchar,@BookDate1,106) + ''' AND ''' + Convert(varchar,@BookDate2,106) + ''')'
	IF(@VendorId <> '')
		BEGIN			
			SET @VendorId = REPLACE(@VendorId,',',''',''')
			SET @SQL = @SQL + ' AND (dbo.EntVendorChallan.VendorId IN (''' + @VendorId + '''))'
		END
	
	SET @SQL = @SQL + ') AS T1'
	SET @SQL = @SQL + '	Group By BookingDate, BookingNumber, Pieces '
	PRINT @SQL
	EXEC (@SQL)
	
END
GO
/****** Object:  StoredProcedure [dbo].[sp_MakeAllProcedure]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Manoj Kumar Gupta>
-- Create date: <12-Dec-2011>
-- Description:	<This procedure make all procedure>
-- =============================================
CREATE PROCEDURE [dbo].[sp_MakeAllProcedure]	
AS
BEGIN	
	if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='CleanDataBase'
)
exec(
'
')
else
exec(
'create procedure CleanDataBase
(
	@j int
)
as
	select @j as number
')
--------
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='prcTask'
)
exec(
'
')
else
exec(
'create procedure prcTask
(
	@j int
)
as
	select @j as number 
')
-----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='prcTodayDate'
)
exec(
'
')
else
exec(
'create procedure prcTodayDate
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='sp_Backup'
)
exec(
'
')
else
exec(
'create procedure sp_Backup
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='sp_BindGrid'
)
exec(
'
')
else
exec(
'create procedure sp_BindGrid
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='sp_CustomerWiseDueReport'
)
exec(
'
')
else
exec(
'create procedure sp_CustomerWiseDueReport
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='sp_Dry_BarcodeMaster'
)
exec(
'
')
else
exec(
'create procedure sp_Dry_BarcodeMaster
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='sp_Dry_DefaultDataInMasters'
)
exec(
'
')
else
exec(
'create procedure sp_Dry_DefaultDataInMasters
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='sp_Dry_DrawlMaster'
)
exec(
'
')
else
exec(
'create procedure sp_Dry_DrawlMaster
(
	@j int
)
as
	select @j as number 
')
-----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='sp_Dry_EmployeeMaster'
)
exec(
'
')
else
exec(
'create procedure sp_Dry_EmployeeMaster
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='sp_dry_NewChallan'
)
exec(
'
')
else
exec(
'create procedure sp_dry_NewChallan
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='sp_EditBooking_SaveProc'
)
exec(
'
')
else
exec(
'create procedure sp_EditBooking_SaveProc
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='sp_EditRecord'
)
exec(
'
')
else
exec(
'create procedure sp_EditRecord
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='sp_InsertIntoBarcodeTable'
)
exec(
'
')
else
exec(
'create procedure sp_InsertIntoBarcodeTable
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_InsUpd_ConfigSettings'
)
exec(
'
')
else
exec(
'create procedure Sp_InsUpd_ConfigSettings
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_InsUpd_FirstTimeConfigSettings'
)
exec(
'
')
else
exec(
'create procedure Sp_InsUpd_FirstTimeConfigSettings
(
	@j int
)
as
	select @j as number 
')
-----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='SP_Mearge_DataWithExistigBooking'
)
exec(
'
')
else
exec(
'create procedure SP_Mearge_DataWithExistigBooking
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='sp_NewBooking'
)
exec(
'
')
else
exec(
'create procedure sp_NewBooking
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='sp_NewBooking_SaveProc'
)
exec(
'
')
else
exec(
'create procedure sp_NewBooking_SaveProc
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='sp_ReceiptConfigSetting'
)
exec(
'
')
else
exec(
'create procedure sp_ReceiptConfigSetting
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Report_ChallanReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Report_ChallanReport
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Report_NewVendorReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Report_NewVendorReport
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Report_PaymentTypeReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Report_PaymentTypeReport
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Report_ServiceTaxReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Report_ServiceTaxReport
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Report_VendorReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Report_VendorReport
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='sp_Restore'
)
exec(
'
')
else
exec(
'create procedure sp_Restore
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='sp_rpt_barcodprint'
)
exec(
'
')
else
exec(
'create procedure sp_rpt_barcodprint
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_AllDeliveryReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_AllDeliveryReport
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_AreaWiseBookingReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_AreaWiseBookingReport
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_AreaWiseClothBookingReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_AreaWiseClothBookingReport
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_BookingDetailsForDelivery'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_BookingDetailsForDelivery
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_BookingDetailsForEditing'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_BookingDetailsForEditing
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_BookingDetailsForReceipt'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_BookingDetailsForReceipt
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_BookingDetailsReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_BookingDetailsReport
(
	@j int
)
as
	select @j as number 
')
-----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_BookingDetailsReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_BookingDetailsReport
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_BookingReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_BookingReport
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_ChallanReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_ChallanReport
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_ChallanReturnDetails'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_ChallanReturnDetails
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_ChallanReturnDetailsForPrint'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_ChallanReturnDetailsForPrint
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_CustomerAllPrevoiusDue'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_CustomerAllPrevoiusDue
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_CustomerStatus'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_CustomerStatus
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_CustomerWiseBookingReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_CustomerWiseBookingReport
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_DayBookReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_DayBookReport
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_DefaultAnniveraryCustomer'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_DefaultAnniveraryCustomer
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_DefaultBirthDayCustomer'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_DefaultBirthDayCustomer
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_DefaultHomeDeliveryShow'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_DefaultHomeDeliveryShow
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_DefaultUrgentShow'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_DefaultUrgentShow
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_DeliveryQuantityandBooking'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_DeliveryQuantityandBooking
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_DeliveryReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_DeliveryReport
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_EmployeeCheckedByReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_EmployeeCheckedByReport
(
	@j int
)
as
	select @j as number 
')
-----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_ItemWiseReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_ItemWiseReport
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_NewDeliveryReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_NewDeliveryReport
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_PaymentReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_PaymentReport
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_QuantityandBooking'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_QuantityandBooking
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='SP_Sel_RecForItemIdUpdate'
)
exec(
'
')
else
exec(
'create procedure SP_Sel_RecForItemIdUpdate
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_RecleanReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_RecleanReport
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_RecordsForBookingCancellation'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_RecordsForBookingCancellation
(
	@j int
)
as
	select @j as number 
')
-----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_ReportVendorChallanReturnDetails'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_ReportVendorChallanReturnDetails
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_SalesLedgerReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_SalesLedgerReport
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_SecondTimeChallanReturnDetails'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_SecondTimeChallanReturnDetails
(
	@j int
)
as
	select @j as number 
')
--
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_smsTimeChecking'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_smsTimeChecking
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_StockReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_StockReport
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_TimeWiseClothBookingReport'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_TimeWiseClothBookingReport
(
	@j int
)
as
	select @j as number 
')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_Sel_VendorChallanReturnDetails'
)
exec(
'
')
else
exec(
'create procedure Sp_Sel_VendorChallanReturnDetails
(
	@j int
)
as
	select @j as number 
')
-----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='SP_SelectBookingRecordsForEdition'
)
exec(
'
')
else
exec(
'create procedure SP_SelectBookingRecordsForEdition
(
	@j int
)
as
	select @j as number 
')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME='Sp_VendorReport'
)
exec(
'
')
else
exec(
'create procedure Sp_VendorReport
(
	@j int
)
as
	select @j as number 
')
END
GO
/****** Object:  Table [dbo].[mstTask]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mstTask](
	[Key1] [nvarchar](100) NULL,
	[Key2] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mstRemark]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[mstRemark](
	[ID] [int] NOT NULL,
	[Remarks] [varchar](50) NOT NULL,
 CONSTRAINT [IX_mstRemark] UNIQUE NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[mstRecordCheck]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[mstRecordCheck](
	[Status] [varchar](50) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[mstReceiptConfig]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[mstReceiptConfig](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HeaderText] [varchar](40) NULL,
	[HeaderFontName] [varchar](max) NULL,
	[HeaderFontSize] [varchar](max) NOT NULL,
	[HeaderFontStyle] [varchar](max) NULL,
	[AddressText] [varchar](70) NULL,
	[AddressFontName] [varchar](max) NULL,
	[AddressFontSize] [varchar](max) NULL,
	[AddressFontStyle] [varchar](max) NULL,
	[LogoLeftRight] [varchar](max) NULL,
	[LogoHeight] [varchar](max) NULL,
	[LogoWeight] [varchar](max) NULL,
	[SloganText] [varchar](100) NULL,
	[SloganFontName] [varchar](max) NULL,
	[SloganFontSize] [varchar](max) NULL,
	[SloganFontStyle] [varchar](max) NULL,
	[Barcode] [bit] NULL,
	[PreviewDue] [bit] NULL,
	[Term1] [varchar](100) NULL,
	[Term2] [varchar](100) NULL,
	[Term3] [varchar](100) NULL,
	[Term4] [varchar](100) NULL,
	[FooterSloganText] [varchar](100) NULL,
	[FooterSloganFontName] [varchar](max) NULL,
	[FooterSloganFontSize] [varchar](max) NULL,
	[FooterSloganFontStyle] [varchar](max) NULL,
	[ST] [bit] NULL,
	[PrinterLaserOrDotMatrix] [bit] NULL,
	[PrintLogoonReceipt] [bit] NULL,
	[PrePrinted] [bit] NULL,
	[TextFontStyleUL] [varchar](50) NULL,
	[TextFontItalic] [varchar](50) NULL,
	[TextAddressUL] [varchar](50) NULL,
	[TextAddressItalic] [varchar](50) NULL,
	[HeaderSloganUL] [varchar](50) NULL,
	[HeaderSloganItalic] [varchar](50) NULL,
	[FooterSloganUL] [varchar](50) NULL,
	[FooterSloganItalic] [varchar](50) NULL,
	[ShowHeaderSlogan] [bit] NULL,
	[ShowFooterSlogan] [bit] NULL,
	[TermsAndConditionTrue] [bit] NULL,
	[DotMatrixPrinter40Cloumn] [bit] NULL,
	[TableBorder] [bit] NULL,
	[NDB] [varchar](50) NULL,
	[NDI] [varchar](50) NULL,
	[NDU] [varchar](50) NULL,
	[NDFName] [varchar](max) NULL,
	[NDFSize] [varchar](max) NULL,
	[NAB] [varchar](50) NULL,
	[NAI] [varchar](50) NULL,
	[NAU] [varchar](50) NULL,
	[NAFName] [varchar](max) NULL,
	[NAFSize] [varchar](max) NULL,
	[CurrencyType] [varchar](50) NULL,
 CONSTRAINT [PK_mstReceiptConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[mstReceiptConfig] ON
INSERT [dbo].[mstReceiptConfig] ([Id], [HeaderText], [HeaderFontName], [HeaderFontSize], [HeaderFontStyle], [AddressText], [AddressFontName], [AddressFontSize], [AddressFontStyle], [LogoLeftRight], [LogoHeight], [LogoWeight], [SloganText], [SloganFontName], [SloganFontSize], [SloganFontStyle], [Barcode], [PreviewDue], [Term1], [Term2], [Term3], [Term4], [FooterSloganText], [FooterSloganFontName], [FooterSloganFontSize], [FooterSloganFontStyle], [ST], [PrinterLaserOrDotMatrix], [PrintLogoonReceipt], [PrePrinted], [TextFontStyleUL], [TextFontItalic], [TextAddressUL], [TextAddressItalic], [HeaderSloganUL], [HeaderSloganItalic], [FooterSloganUL], [FooterSloganItalic], [ShowHeaderSlogan], [ShowFooterSlogan], [TermsAndConditionTrue], [DotMatrixPrinter40Cloumn], [TableBorder], [NDB], [NDI], [NDU], [NDFName], [NDFSize], [NAB], [NAI], [NAU], [NAFName], [NAFSize], [CurrencyType]) VALUES (1, N'Quick Dry Cleaning Software', N'Arial', N'30', N'Bold', N'E-166, Kamla Nagar, Delhi-110007', N'Arial', N'15', N'Bold', N'1', N'1', N'1', N'Booking Receipt', N'Arial Black', N'15', N'Bold', 1, 1, N'All garments are accepted at Owner''s Risk. Make sure to check the pockets before giving your cloths.', N'The Firm assures its best efforts to remove all spots and stains, still there is no responsibility.', N'It is not being Practically Possible to note all the Cuts, Holes, Scratches Stain etc.', N'', N'Delivery Receipt ', N'Arial Black', N'16', N'Bold', 1, 1, 1, 1, N'', N'', N'', N'', N'', N'', N'', N'', 1, 1, 1, 0, 1, N'Bold', N'', N'', N'Arial', N'12', N'Bold', N'', N'', N'Arial', N'12', N'Rs')
SET IDENTITY_INSERT [dbo].[mstReceiptConfig] OFF
/****** Object:  Table [dbo].[mstJobType]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[mstJobType](
	[ID] [int] NOT NULL,
	[JobType] [varchar](50) NOT NULL,
 CONSTRAINT [PK_mstJobType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_mstJobType] UNIQUE NONCLUSTERED 
(
	[JobType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[mstExpKey]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mstExpKey](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExpKey] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_mst_ExpKey] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[mstDrwal]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[mstDrwal](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DrawlName] [varchar](50) NOT NULL,
	[ParentDrawl] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[mstDrawl]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[mstDrawl](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Drawl] [varchar](50) NOT NULL,
 CONSTRAINT [IX_mstDrawl] UNIQUE NONCLUSTERED 
(
	[Drawl] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[mstDate]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[mstDate](
	[DateId] [int] IDENTITY(1,1) NOT NULL,
	[TodayDate] [datetime] NULL,
	[Key1] [nvarchar](100) NULL,
 CONSTRAINT [PK_mstDate] PRIMARY KEY CLUSTERED 
(
	[DateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MstConfigSettings]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MstConfigSettings](
	[DefaultItemId] [int] NULL,
	[DefaultProcessCode] [varchar](5) NULL,
	[DefaultExtraProcessCode] [varchar](5) NULL,
	[StartBookingNumberFrom] [int] NULL,
	[CustomerMessage] [varchar](500) NULL,
	[DefaultColorName] [varchar](50) NULL,
	[DefaultTime] [varchar](50) NULL,
	[DefaultAmPm] [varchar](10) NULL,
	[DeliveryDateOffset] [int] NULL,
	[DefaultSearchCriteria] [varchar](50) NULL,
	[AmountType] [varchar](50) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[MstConfigSettings] ([DefaultItemId], [DefaultProcessCode], [DefaultExtraProcessCode], [StartBookingNumberFrom], [CustomerMessage], [DefaultColorName], [DefaultTime], [DefaultAmPm], [DeliveryDateOffset], [DefaultSearchCriteria], [AmountType]) VALUES (1, N'WC', N'', 0, N'', N'2', N'6', N'PM', 2, N'Name', N'False')
/****** Object:  Table [dbo].[mstColor]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[mstColor](
	[ID] [int] NOT NULL,
	[ColorName] [varchar](50) NULL,
	[ColorCode] [varchar](50) NULL,
 CONSTRAINT [IX_mstColor] UNIQUE NONCLUSTERED 
(
	[ColorName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_mstColor_1] UNIQUE NONCLUSTERED 
(
	[ColorCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LedgerMaster]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LedgerMaster](
	[LedgerName] [varchar](100) NOT NULL,
	[OpenningBalance] [float] NOT NULL,
	[CurrentBalance] [float] NOT NULL,
 CONSTRAINT [PK_LedgerMaster] PRIMARY KEY CLUSTERED 
(
	[LedgerName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ItemwiseProcessRate]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ItemwiseProcessRate](
	[ItemName] [varchar](50) NOT NULL,
	[ProcessCode] [varchar](5) NOT NULL,
	[ProcessPrice] [float] NOT NULL,
 CONSTRAINT [PK_ItemwiseProcessRate] PRIMARY KEY CLUSTERED 
(
	[ItemName] ASC,
	[ProcessCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ItemMaster]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ItemMaster](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[ItemName] [varchar](50) NOT NULL,
	[NumberOfSubItems] [int] NOT NULL,
	[ItemCode] [varchar](100) NULL,
 CONSTRAINT [PK_ItemMaster_1] PRIMARY KEY CLUSTERED 
(
	[ItemName] ASC,
	[NumberOfSubItems] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ItemMaster] ON
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (1, N'Achkan Embrodiary', 1, N'ACHE')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (2, N'Achkan Plain', 1, N'ACH')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (3, N'Baby Achkaan Embrodry', 1, N'BAE')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (4, N'Baby Achkaan Plain', 1, N'BAP')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (5, N'Baby Coat', 1, N'BCT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (6, N'Baby Jacket', 1, N'BJKT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (7, N'baby Jeans', 1, N'BJN')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (8, N'Baby Kurta-Pyjama', 2, N'BKP')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (9, N'Baby Lehnga', 2, N'BLHN')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (10, N'Baby Shirt', 1, N'BSHR')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (11, N'Baby Shorts', 1, N'BSRT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (12, N'Baby Skirt', 1, N'BSK')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (13, N'Baby Skirt Top', 2, N'BSKT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (14, N'Baby Slacks', 1, N'BSLK')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (15, N'Baby Stoking', 1, N'BSTK')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (16, N'Baby Suit 2PC', 2, N'BS2')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (17, N'Baby Suit 3PC', 3, N'BS3')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (18, N'Baby Suit Female 2PC', 2, N'BSF2')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (19, N'Baby Suit Female 3PC', 3, N'BSF3')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (20, N'Baby Sweat Shirt', 1, N'BSS')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (21, N'Baby Sweater', 1, N'BSWT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (22, N'Baby T Shirt', 1, N'BTS')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (23, N'Baby Top', 1, N'BTOP')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (24, N'Baby Trouser', 1, N'BTSR')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (25, N'Bag', 1, N'BAG')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (26, N'Bath Mat', 1, N'MAT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (27, N'Bath Robes', 1, N'ROBE')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (28, N'Bed Throw', 1, N'THR')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (29, N'Bedsheet (Double)', 1, N'DBS')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (30, N'Bedsheet (Single)', 1, N'SBS')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (31, N'Bell Boy Coat', 1, N'HBBC')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (32, N'Bell Boy Pant', 1, N'HBBP')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (33, N'Belt', 1, N'BLT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (34, N'Blanket (Double)', 1, N'DBL')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (35, N'Blanket (Single)', 1, N'SBL')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (36, N'Blouse', 1, N'BLZ')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (37, N'Bo-Tie', 1, N'')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (38, N'Cap', 1, N'CAP')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (39, N'Capri', 1, N'CPR')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (40, N'Cardigans', 1, N'CRD')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (41, N'Carpet', 1, N'CPT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (42, N'Chair Covers', 1, N'CCR')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (43, N'Chef Coat', 1, N'HCC')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (44, N'Chef Pant', 1, N'HCP')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (45, N'Coat', 1, N'COAT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (46, N'Cook Apron', 1, N'HCA')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (47, N'Curtain', 1, N'CUR')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (48, N'Curtain (With Lining)', 1, N'CURL')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (49, N'Curtain (Woolen)', 1, N'CURW')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (50, N'Cushion Covers (BIG)', 1, N'BCC')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (51, N'Cushions Cover (SMALL)', 1, N'SCC')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (52, N'Dangree', 1, N'DAN')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (53, N'ddLADIES SUIT (2 PCS)', 1, N'dd')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (54, N'Dhoti', 1, N'')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (55, N'Dhoti-KURTA', 1, N'DHK')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (56, N'Dressing Gown', 1, N'DRG')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (57, N'Dupatta', 1, N'DUP')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (58, N'Dupatta (Heavy)', 1, N'DUH')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (59, N'Duvet (BIG)', 1, N'HDB')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (60, N'Duvet (SMALL)', 1, N'HDS')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (61, N'Duvet Covers (BIG)', 1, N'HDCB')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (62, N'Duvet Covers (SMALL)', 1, N'HDCS')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (63, N'Face Towel', 1, N'FTWL')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (64, N'Foot Mats', 1, N'FMAT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (65, N'Frill', 1, N'FRL')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (66, N'Frock', 1, N'FRK')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (67, N'Gents Suit 2Pcs', 2, N'GS2')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (68, N'Gents Suit 3Pcs', 3, N'GS3')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (69, N'Gloves', 1, N'GLV')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (70, N'Gown', 1, N'GWN')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (71, N'Handkerchief', 1, N'HANK')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (72, N'Hat', 1, N'HAT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (73, N'Jacket', 1, N'JKT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (74, N'Jacket (Leather)', 1, N'JKTL')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (75, N'Jacket Half', 1, N'JKTH')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (76, N'Jeans', 1, N'JEN')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (77, N'Kameez', 1, N'KAM')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (78, N'Kurta', 1, N'')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (79, N'Kurta-Pyjama', 1, N'KUP')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (80, N'Ladies Suit (2 Pcs)', 2, N'LS2')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (81, N'Ladies Suit (3 Pcs)', 3, N'LS3')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (82, N'lehenga', 1, N'LHG')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (83, N'Lehenga 2pcs', 2, N'LHG2')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (84, N'Lehenga 3pcs', 3, N'LHG3')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (85, N'Long Coat', 1, N'LCT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (86, N'Lungi', 1, N'')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (87, N'Mattress Protector (BIG)', 1, N'BMP')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (88, N'Mattress Protector (SMALL)', 1, N'SMP')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (89, N'Muflaur', 1, N'MUF')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (90, N'Napkin', 1, N'NAP')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (91, N'Napkin Cocktail', 1, N'NAPC')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (92, N'Nighty', 1, N'NGT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (93, N'OverCoat', 1, N'OCT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (94, N'Pagri', 1, N'')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (95, N'Peticot', 1, N'PTC')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (96, N'Pillow Covers (BIG)', 1, N'PCB')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (97, N'Pillow Covers (SMALL)', 1, N'PCS')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (98, N'Place Matt', 1, N'')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (99, N'Pool Towel', 1, N'HPT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (100, N'Pullover', 1, N'PUL')
GO
print 'Processed 100 total records'
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (101, N'Pyjama', 1, N'PYJ')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (102, N'Quilt', 1, N'QLT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (103, N'Quilt ( Cover )', 1, N'QLTC')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (104, N'Quilt ( Double )', 1, N'QLTD')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (105, N'Rain-Coat', 1, N'RAC')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (106, N'Robe', 1, N'ROB')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (107, N'Safari Suit', 1, N'SAF')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (108, N'Salwar', 1, N'SAL')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (109, N'Salwar-Kameez', 2, N'SALK')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (110, N'Saree', 1, N'SAR')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (111, N'Saree (Border)', 1, N'SARB')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (112, N'Saree (Heavy work)', 1, N'SARH')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (113, N'Saree (Light Work)', 1, N'SARL')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (114, N'Saree (Medium Work)', 1, N'SARM')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (115, N'Saree (Printed)', 1, N'SARP')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (116, N'Saree Zari', 1, N'SARZ')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (117, N'Scarf', 1, N'SCF')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (118, N'Seat Cover', 1, N'STC')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (119, N'Shawl', 1, N'SHL')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (120, N'Shawl (Pashmina)', 1, N'SHLP')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (121, N'Sherwani', 1, N'SHR')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (122, N'Shirt', 1, N'SHI')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (123, N'Shoes', 1, N'SHO')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (124, N'Short', 1, N'SHRT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (125, N'Skirt', 1, N'SKR')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (126, N'Skirt Top', 1, N'SKRT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (127, N'Slacks', 1, N'SLK')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (128, N'Socks', 1, N'SOC')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (129, N'Stoking', 1, N'STK')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (130, N'Stroll', 1, N'')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (131, N'Stuff toy', 1, N'STOY')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (132, N'Sweat-Shirt', 1, N'SWT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (133, N'Sweater (FS)', 1, N'SWF')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (134, N'Sweater (HS)', 1, N'SWH')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (135, N'T-Shirt', 1, N'TSH')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (136, N'Table Cloth', 1, N'TBC')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (137, N'Tie', 1, N'')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (138, N'Top', 1, N'TOP')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (139, N'Towel (L)', 1, N'TWL')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (140, N'Towel (M)', 1, N'TWM')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (141, N'Towel Hand', 1, N'TWH')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (142, N'Trouser', 1, N'TRO')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (143, N'Trousers Half', 1, N'TROH')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (144, N'Under Wear', 1, N'')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (145, N'Vest', 1, N'')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (146, N'Waist Coat', 1, N'WCT')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (147, N'Waiter Cloth', 1, N'HWC')
INSERT [dbo].[ItemMaster] ([ItemID], [ItemName], [NumberOfSubItems], [ItemCode]) VALUES (148, N'Wiping Cloth', 1, N'')
SET IDENTITY_INSERT [dbo].[ItemMaster] OFF
/****** Object:  UserDefinedFunction [dbo].[fnSplitString]    Script Date: 12/26/2011 11:55:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fnSplitString](@str nvarchar(max),@sep nvarchar(max))
RETURNS @tbl TABLE(value nvarchar(max))
AS
BEGIN
	DECLARE @idx1 INT;
	DECLARE @idx2 INT;
	SET @idx1=0;
	WHILE @idx1 >-1
	BEGIN;
		SELECT @idx2 =  CHARINDEX(@sep,@str,@idx1);
		IF @idx2 > 0
		BEGIN;
			INSERT INTO @tbl(value)
			SELECT SUBSTRING(@str,@idx1,@idx2-@idx1)
			SET @idx1 = @idx2+1;
		END;
		ELSE
		BEGIN;
			INSERT INTO @tbl(value)
			SELECT SUBSTRING(@str,@idx1,LEN(@str)+1-@idx1)
			SET @idx1 = -1;
		END;
	END;
	RETURN;
END;
GO
/****** Object:  Table [dbo].[BranchMaster]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BranchMaster](
	[BranchCode] [varchar](10) NOT NULL,
	[BranchName] [varchar](30) NOT NULL,
	[BranchAddress] [varchar](100) NOT NULL,
	[BranchPhone] [varchar](50) NULL,
	[BranchSlogan] [varchar](100) NULL,
 CONSTRAINT [PK_BranchMaster] PRIMARY KEY CLUSTERED 
(
	[BranchCode] ASC,
	[BranchName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[sp_Backup]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sanjeev Arora>
-- Create date: <25-Aug-2011>
-- Description:	<Back Up DataBase>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Backup]	
AS
BEGIN
	BACKUP DATABASE [DRYSOFT] TO  
    DISK = N'c:\iNETPUB\WWWROOT\DRYSOFT_backup.bak'
    WITH NOFORMAT, NOINIT,  NAME = N'DRYSOFT',
	SKIP, NOREWIND, NOUNLOAD,  STATS = 10
END
GO
/****** Object:  Table [dbo].[ShiftMaster]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ShiftMaster](
	[ShiftID] [int] IDENTITY(0,1) NOT NULL,
	[ShiftName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_ShiftMaster_1] PRIMARY KEY CLUSTERED 
(
	[ShiftName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProcessMaster]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProcessMaster](
	[ProcessCode] [varchar](5) NOT NULL,
	[ProcessName] [varchar](30) NOT NULL,
	[ProcessUsedForVendorReport] [bit] NOT NULL,
	[Discount] [bit] NULL,
	[ServiceTax] [float] NULL,
	[IsActiveServiceTax] [bit] NULL,
	[IsDiscount] [bit] NULL,
 CONSTRAINT [PK_ProcessMaster] PRIMARY KEY CLUSTERED 
(
	[ProcessCode] ASC,
	[ProcessName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PriorityMaster]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PriorityMaster](
	[PriorityID] [int] NOT NULL,
	[Priority] [varchar](100) NOT NULL,
 CONSTRAINT [PK_PriorityMaster_1] PRIMARY KEY CLUSTERED 
(
	[Priority] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EntMenuRights]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EntMenuRights](
	[UserTypeId] [int] NOT NULL,
	[PageTitle] [varchar](500) NOT NULL,
	[FileName] [varchar](500) NULL,
	[RightToView] [bit] NOT NULL,
	[MenuItemLevel] [int] NULL,
	[MenuPosition] [int] NULL,
	[ParentMenu] [varchar](500) NOT NULL,
 CONSTRAINT [PK_EntMenuRights] PRIMARY KEY CLUSTERED 
(
	[UserTypeId] ASC,
	[PageTitle] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Accounts', N'~/Accounts/ExpenseEntry.aspx', 1, 1, 4, N'None')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Admin', N'~/Admin/MenuRights.aspx', 1, 1, 6, N'None')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'All Delivery', N'~/Reports/MainDeliveryReport.aspx', 1, 2, 13, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Allotted Stock Location Rack', N'~/Bookings/AllottedDrawl.aspx', 1, 2, 12, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Area Wise', N'~/Reports/AreaLocationReport.aspx', 1, 2, 16, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Backup Database', N'~/Backup/Backup.aspx', 1, 2, 7, N'Admin')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Booking', N'~/Reports/BookingReport.aspx', 1, 2, 1, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Booking Cancellation', N'~/Admin/BookingCancellation.aspx', 1, 2, 2, N'Admin')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Bookings', N'~/New_Booking/frm_New_Booking.aspx', 1, 1, 3, N'None')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Branch Master', N'~/Masters/BranchMaster.aspx', 1, 2, 1, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Cash book', N'~/Accounts/Cashbook.aspx', 1, 2, 3, N'Accounts')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Challan/Workshop Note', N'~/Reports/ChallanReport.aspx', 1, 2, 3, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Challan/Workshop Note Inward', N'~/Bookings/ChallanReturn.aspx', 1, 2, 4, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Challan/Workshop Note Outward', N'~/Bookings/NewChallan.aspx', 1, 2, 3, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Change Password', N'~/Masters/ChangePassword.aspx', 1, 2, 1, N'Home')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Checked By Employee', N'~/Reports/EmployeeCheckedByReport.aspx', 1, 2, 15, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Color Master', N'~/Masters/ColorMaster.aspx', 1, 2, 11, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Configuration Setting', N'~/Config Setting/frmReceipt.aspx', 1, 2, 6, N'Admin')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Customer Master', N'~/Masters/CustomerMaster.aspx', 1, 2, 2, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Customer Receipt Status', N'~/Reports/StatusBookingByCustomer.aspx', 1, 2, 12, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Customer wise booking', N'~/Reports/BookingByCustomerReport.aspx', 1, 2, 2, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Customer Wise Delivery', N'~/Reports/DeliveryReport.aspx', 1, 2, 4, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Daybook', N'~/Accounts/Daybook.aspx', 1, 2, 4, N'Accounts')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Delivery', N'~/Bookings/Delivery.aspx', 1, 2, 5, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Employee Master', N'~/Masters/EmployeeMaster.aspx', 1, 2, 14, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Expense', N'~/Admin/ExpenseReport.aspx', 1, 2, 21, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Expenses', N'~/Accounts/ExpenseEntry.aspx', 1, 2, 2, N'Accounts')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Home', N'~/Masters/Default.aspx', 1, 1, 1, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Item Master', N'~/Masters/ItemMaster.aspx', 1, 2, 3, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Item Wise', N'~/Reports/itemwiseReport.aspx', 1, 2, 17, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Job Type', N'~/Masters/JobType.aspx', 1, 2, 12, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Masters', N'~/Masters/BranchMaster.aspx', 1, 1, 2, N'None')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Monthly Statement', N'~/Reports/MonthlyStatement.aspx', 1, 2, 5, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'New Booking', N'~/New_Booking/frm_New_Booking.aspx', 1, 2, 1, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Out Sourced Vendor', N'~/Reports/VendorChallanReturn.aspx', 1, 2, 11, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Out Sourced Vendor\Delivery Note', N'~/Bookings/VendorNewChallan.aspx', 1, 2, 10, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Payment', N'~/Reports/PaymentReport.aspx', 1, 2, 6, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Payment Type', N'~/Reports/PaymentTypeReport.aspx', 1, 2, 20, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Print Barcode', N'~/Reports/InvoiceNoPrint.aspx', 1, 2, 13, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Priority Master', N'~/Masters/PriorityMaster.aspx', 1, 2, 5, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Process Master', N'~/Masters/ProcessMaster.aspx', 1, 2, 6, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Rack Master', N'~/Masters/Drawl.aspx', 1, 2, 17, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Rate List', N'~/Masters/ItemProcessPriceMaster.aspx', 1, 2, 8, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Recleaning', N'~/Reports/RecleaningReport.aspx', 1, 2, 7, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Recleaning Form', N'~/Bookings/SendForRecleaning.aspx', 1, 2, 9, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Remarks Master', N'~/Masters/RemarkMaster.aspx', 1, 2, 15, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Reports', N'~/Reports/BookingReport.aspx', 1, 1, 5, N'None')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Return OutSourced Vendor\Delivery Note', N'~/Bookings/VendorChallanReturn.aspx', 1, 2, 11, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Sales Ledger', N'~/Accounts/SalesLedger.aspx', 1, 2, 4, N'Accounts')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Service Tax', N'~/Reports/ServiceTaxReport.aspx', 1, 2, 19, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Set menu rights', N'~/Admin/MenuRights.aspx', 1, 2, 1, N'Admin')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Shift Master', N'~/Masters/ShiftMaster.aspx', 1, 2, 10, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Stock', N'~/Reports/StockReport.aspx', 1, 2, 8, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Stock Location Master', N'~/Masters/DrawlMaster.aspx', 1, 2, 16, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Time Wise', N'~/Reports/TimeandColthReport.aspx', 1, 2, 18, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Today Booking', N'~/Reports/QuantityandPriceReport.aspx', 1, 2, 10, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'User Master', N'~/Masters/UserMaster.aspx', 1, 2, 7, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Vendor', N'~/Reports/VendorReport.aspx', 1, 2, 9, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Vendor Master', N'~/Masters/VendorMaster.aspx', 1, 2, 13, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (1, N'Workshop\Delivery Note By Numbers', N'~/Reports/GetChallanByNumbers.aspx', 1, 2, 8, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Accounts', N'~/Accounts/ExpenseEntry.aspx', 1, 1, 4, N'None')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Admin', N'~/Admin/MenuRights.aspx', 1, 1, 6, N'None')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'All Delivery', N'~/Reports/MainDeliveryReport.aspx', 1, 2, 13, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Allotted Stock Location Rack', N'~/Bookings/AllottedDrawl.aspx', 1, 2, 12, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Area Wise', N'~/Reports/AreaLocationReport.aspx', 1, 2, 16, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Backup Database', N'~/Backup/Backup.aspx', 1, 2, 7, N'Admin')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Booking', N'~/Reports/BookingReport.aspx', 1, 2, 1, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Booking Cancellation', N'~/Admin/BookingCancellation.aspx', 1, 2, 2, N'Admin')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Bookings', N'~/New_Booking/frm_New_Booking.aspx', 1, 1, 3, N'None')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Branch Master', N'~/Masters/BranchMaster.aspx', 1, 2, 1, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Cash book', N'~/Accounts/Cashbook.aspx', 1, 2, 3, N'Accounts')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Challan/Workshop Note', N'~/Reports/ChallanReport.aspx', 1, 2, 3, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Challan/Workshop Note Inward', N'~/Bookings/ChallanReturn.aspx', 1, 2, 4, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Challan/Workshop Note Outward', N'~/Bookings/NewChallan.aspx', 1, 2, 3, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Change Password', N'~/Masters/ChangePassword.aspx', 1, 2, 1, N'Home')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Checked By Employee', N'~/Reports/EmployeeCheckedByReport.aspx', 1, 2, 15, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Color Master', N'~/Masters/ColorMaster.aspx', 1, 2, 11, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Configuration Setting', N'~/Config Setting/frmReceipt.aspx', 1, 2, 6, N'Admin')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Customer Master', N'~/Masters/CustomerMaster.aspx', 1, 2, 2, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Customer Receipt Status', N'~/Reports/StatusBookingByCustomer.aspx', 1, 2, 12, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Customer wise booking', N'~/Reports/BookingByCustomerReport.aspx', 1, 2, 2, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Customer Wise Delivery', N'~/Reports/DeliveryReport.aspx', 1, 2, 4, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Daybook', N'~/Accounts/Daybook.aspx', 1, 2, 4, N'Accounts')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Delivery', N'~/Bookings/Delivery.aspx', 1, 2, 5, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Employee Master', N'~/Masters/EmployeeMaster.aspx', 1, 2, 14, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Expense', N'~/Admin/ExpenseReport.aspx', 1, 2, 21, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Expenses', N'~/Accounts/ExpenseEntry.aspx', 1, 2, 2, N'Accounts')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Home', N'~/Masters/Default.aspx', 1, 1, 1, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Item Master', N'~/Masters/ItemMaster.aspx', 1, 2, 3, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Item Wise', N'~/Reports/itemwiseReport.aspx', 1, 2, 17, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Job Type', N'~/Masters/JobType.aspx', 1, 2, 12, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Masters', N'~/Masters/BranchMaster.aspx', 1, 1, 2, N'None')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Monthly Statement', N'~/Reports/MonthlyStatement.aspx', 1, 2, 5, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'New Booking', N'~/New_Booking/frm_New_Booking.aspx', 1, 2, 1, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Out Sourced Vendor', N'~/Reports/VendorChallanReturn.aspx', 1, 2, 11, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Out Sourced Vendor\Delivery Note', N'~/Bookings/VendorNewChallan.aspx', 1, 2, 10, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Payment', N'~/Reports/PaymentReport.aspx', 1, 2, 6, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Payment Type', N'~/Reports/PaymentTypeReport.aspx', 1, 2, 20, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Print Barcode', N'~/Reports/InvoiceNoPrint.aspx', 1, 2, 13, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Priority Master', N'~/Masters/PriorityMaster.aspx', 1, 2, 5, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Process Master', N'~/Masters/ProcessMaster.aspx', 1, 2, 6, N'Masters')
GO
print 'Processed 100 total records'
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Rack Master', N'~/Masters/Drawl.aspx', 1, 2, 17, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Rate List', N'~/Masters/ItemProcessPriceMaster.aspx', 1, 2, 8, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Recleaning', N'~/Reports/RecleaningReport.aspx', 1, 2, 7, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Recleaning Form', N'~/Bookings/SendForRecleaning.aspx', 1, 2, 9, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Remarks Master', N'~/Masters/RemarkMaster.aspx', 1, 2, 15, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Reports', N'~/Reports/BookingReport.aspx', 1, 1, 5, N'None')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Return OutSourced Vendor\Delivery Note', N'~/Bookings/VendorChallanReturn.aspx', 1, 2, 11, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Sales Ledger', N'~/Accounts/SalesLedger.aspx', 1, 2, 4, N'Accounts')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Service Tax', N'~/Reports/ServiceTaxReport.aspx', 1, 2, 19, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Set menu rights', N'~/Admin/MenuRights.aspx', 1, 2, 1, N'Admin')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Shift Master', N'~/Masters/ShiftMaster.aspx', 1, 2, 10, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Stock', N'~/Reports/StockReport.aspx', 1, 2, 8, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Stock Location Master', N'~/Masters/DrawlMaster.aspx', 1, 2, 16, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Time Wise', N'~/Reports/TimeandColthReport.aspx', 1, 2, 18, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Today Booking', N'~/Reports/QuantityandPriceReport.aspx', 1, 2, 10, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'User Master', N'~/Masters/UserMaster.aspx', 1, 2, 7, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Vendor', N'~/Reports/VendorReport.aspx', 1, 2, 9, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Vendor Master', N'~/Masters/VendorMaster.aspx', 1, 2, 13, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (2, N'Workshop\Delivery Note By Numbers', N'~/Reports/GetChallanByNumbers.aspx', 1, 2, 8, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Accounts', N'~/Accounts/ExpenseEntry.aspx', 1, 1, 4, N'None')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Admin', N'~/Admin/MenuRights.aspx', 1, 1, 6, N'None')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'All Delivery', N'~/Reports/MainDeliveryReport.aspx', 1, 2, 13, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Allotted Stock Location Rack', N'~/Bookings/AllottedDrawl.aspx', 1, 2, 12, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Area Wise', N'~/Reports/AreaLocationReport.aspx', 1, 2, 16, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Backup Database', N'~/Backup/Backup.aspx', 0, 2, 7, N'Admin')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Booking', N'~/Reports/BookingReport.aspx', 1, 2, 1, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Booking Cancellation', N'~/Admin/BookingCancellation.aspx', 0, 2, 2, N'Admin')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Bookings', N'~/New_Booking/frm_New_Booking.aspx', 1, 1, 3, N'None')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Branch Master', N'~/Masters/BranchMaster.aspx', 1, 2, 1, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Cash book', N'~/Accounts/Cashbook.aspx', 0, 2, 3, N'Accounts')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Challan/Workshop Note', N'~/Reports/ChallanReport.aspx', 1, 2, 3, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Challan/Workshop Note Inward', N'~/Bookings/ChallanReturn.aspx', 1, 2, 4, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Challan/Workshop Note Outward', N'~/Bookings/NewChallan.aspx', 1, 2, 3, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Change Password', N'~/Masters/ChangePassword.aspx', 1, 2, 1, N'Home')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Checked By Employee', N'~/Reports/EmployeeCheckedByReport.aspx', 1, 2, 15, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Color Master', N'~/Masters/ColorMaster.aspx', 1, 2, 11, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Configuration Setting', N'~/Config Setting/frmReceipt.aspx', 1, 2, 6, N'Admin')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Customer Master', N'~/Masters/CustomerMaster.aspx', 1, 2, 2, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Customer Receipt Status', N'~/Reports/StatusBookingByCustomer.aspx', 1, 2, 12, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Customer wise booking', N'~/Reports/BookingByCustomerReport.aspx', 1, 2, 2, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Customer Wise Delivery', N'~/Reports/DeliveryReport.aspx', 1, 2, 4, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Daybook', N'~/Accounts/Daybook.aspx', 1, 2, 4, N'Accounts')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Delivery', N'~/Bookings/Delivery.aspx', 0, 2, 5, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Employee Master', N'~/Masters/EmployeeMaster.aspx', 1, 2, 14, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Expense', N'~/Admin/ExpenseReport.aspx', 1, 2, 21, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Expenses', N'~/Accounts/ExpenseEntry.aspx', 0, 2, 2, N'Accounts')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Home', N'~/Masters/Default.aspx', 1, 1, 1, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Item Master', N'~/Masters/ItemMaster.aspx', 1, 2, 3, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Item Wise', N'~/Reports/itemwiseReport.aspx', 1, 2, 17, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Job Type', N'~/Masters/JobType.aspx', 1, 2, 12, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Masters', N'~/Masters/BranchMaster.aspx', 1, 1, 2, N'None')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Monthly Statement', N'~/Reports/MonthlyStatement.aspx', 0, 2, 5, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'New Booking', N'~/New_Booking/frm_New_Booking.aspx', 1, 2, 1, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Out Sourced Vendor', N'~/Reports/VendorChallanReturn.aspx', 1, 2, 11, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Out Sourced Vendor\Delivery Note', N'~/Bookings/VendorNewChallan.aspx', 1, 2, 10, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Payment', N'~/Reports/PaymentReport.aspx', 1, 2, 6, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Payment Type', N'~/Reports/PaymentTypeReport.aspx', 1, 2, 20, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Print Barcode', N'~/Reports/InvoiceNoPrint.aspx', 1, 2, 13, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Priority Master', N'~/Masters/PriorityMaster.aspx', 1, 2, 5, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Process Master', N'~/Masters/ProcessMaster.aspx', 1, 2, 6, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Rack Master', N'~/Masters/Drawl.aspx', 1, 2, 17, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Rate List', N'~/Masters/ItemProcessPriceMaster.aspx', 1, 2, 8, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Recleaning', N'~/Reports/RecleaningReport.aspx', 1, 2, 7, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Recleaning Form', N'~/Bookings/SendForRecleaning.aspx', 1, 2, 9, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Remarks Master', N'~/Masters/RemarkMaster.aspx', 1, 2, 15, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Reports', N'~/Reports/BookingReport.aspx', 1, 1, 5, N'None')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Return OutSourced Vendor\Delivery Note', N'~/Bookings/VendorChallanReturn.aspx', 1, 2, 11, N'Bookings')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Sales Ledger', N'~/Accounts/SalesLedger.aspx', 0, 2, 4, N'Accounts')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Service Tax', N'~/Reports/ServiceTaxReport.aspx', 1, 2, 19, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Set menu rights', N'~/Admin/MenuRights.aspx', 1, 2, 1, N'Admin')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Shift Master', N'~/Masters/ShiftMaster.aspx', 1, 2, 10, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Stock', N'~/Reports/StockReport.aspx', 1, 2, 8, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Stock Location Master', N'~/Masters/DrawlMaster.aspx', 1, 2, 16, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Time Wise', N'~/Reports/TimeandColthReport.aspx', 1, 2, 18, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Today Booking', N'~/Reports/QuantityandPriceReport.aspx', 1, 2, 10, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'User Master', N'~/Masters/UserMaster.aspx', 1, 2, 7, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Vendor', N'~/Reports/VendorReport.aspx', 1, 2, 9, N'Reports')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Vendor Master', N'~/Masters/VendorMaster.aspx', 1, 2, 13, N'Masters')
INSERT [dbo].[EntMenuRights] ([UserTypeId], [PageTitle], [FileName], [RightToView], [MenuItemLevel], [MenuPosition], [ParentMenu]) VALUES (3, N'Workshop\Delivery Note By Numbers', N'~/Reports/GetChallanByNumbers.aspx', 1, 2, 8, N'Bookings')
/****** Object:  Table [dbo].[EntLedgerEntries]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EntLedgerEntries](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TransDate] [datetime] NOT NULL,
	[LedgerName] [varchar](50) NOT NULL,
	[Particulars] [varchar](50) NOT NULL,
	[OpeningBalance] [float] NOT NULL,
	[Debit] [float] NOT NULL,
	[Credit] [float] NOT NULL,
	[ClosingBalance] [float] NOT NULL,
	[Narration] [varchar](500) NULL,
 CONSTRAINT [PK_EntLedgerEntries] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EntChallan]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EntChallan](
	[ChallanNumber] [int] NOT NULL,
	[ChallanBranchCode] [varchar](5) NOT NULL,
	[ChallanDate] [datetime] NOT NULL,
	[ChallanSendingShift] [varchar](10) NOT NULL,
	[BookingNumber] [varchar](15) NOT NULL,
	[ItemSNo] [int] NOT NULL,
	[SubItemName] [varchar](50) NOT NULL,
	[ItemTotalQuantitySent] [int] NOT NULL,
	[ItemsReceivedFromVendor] [int] NOT NULL,
	[ItemReceivedFromVendorOnDate] [datetime] NULL,
	[Urgent] [bit] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EntBookings]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EntBookings](
	[BookingID] [bigint] NOT NULL,
	[BookingNumber] [varchar](15) NOT NULL,
	[BookingByCustomer] [varchar](50) NOT NULL,
	[BookingAcceptedByUserId] [varchar](20) NOT NULL,
	[IsUrgent] [bit] NOT NULL,
	[BookingDate] [datetime] NOT NULL,
	[BookingDeliveryDate] [datetime] NOT NULL,
	[BookingDeliveryTime] [varchar](50) NULL,
	[TotalCost] [float] NOT NULL,
	[Discount] [float] NOT NULL,
	[NetAmount] [float] NOT NULL,
	[BookingStatus] [int] NOT NULL,
	[BookingCancelDate] [datetime] NULL,
	[BookingCancelReason] [varchar](100) NOT NULL,
	[BookingRemarks] [varchar](200) NULL,
	[ItemTotalQuantity] [int] NOT NULL,
	[Qty] [int] NULL,
	[VendorOrderStatus] [int] NULL,
	[HomeDelivery] [bit] NULL,
	[CheckedByEmployee] [varchar](50) NULL,
	[BarCode] [varchar](200) NULL,
	[BookingTime] [nvarchar](50) NULL,
	[Format] [varchar](10) NULL,
	[ReceiptDeliverd] [bit] NULL,
	[DiscountAmt] [float] NULL,
	[DiscountOption] [varchar](1) NULL,
 CONSTRAINT [PK_EntBookings] PRIMARY KEY CLUSTERED 
(
	[BookingNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_EntBookings] UNIQUE NONCLUSTERED 
(
	[BookingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0=No Order by customer;
1=Received from customer and PendingAtShop;
2=Sent To Vendor;
3=Received fromVendor and PendingAtShop;
4=Delivered to customer;
5=Cancelled By Customer and Pending at shop;' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntBookings', @level2type=N'COLUMN',@level2name=N'BookingStatus'
GO
/****** Object:  Table [dbo].[EmployeeMaster]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmployeeMaster](
	[ID] [int] NULL,
	[EmployeeCode] [varchar](20) NULL,
	[EmployeeSalutation] [varchar](10) NULL,
	[EmployeeName] [varchar](50) NULL,
	[EmployeeAddress] [varchar](100) NULL,
	[EmployeePhone] [varchar](50) NULL,
	[EmployeeMobile] [varchar](50) NULL,
	[EmployeeEmailId] [varchar](100) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CustomerMaster]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CustomerMaster](
	[ID] [int] NOT NULL,
	[CustomerCode] [varchar](20) NOT NULL,
	[CustomerSalutation] [varchar](10) NOT NULL,
	[CustomerName] [varchar](50) NOT NULL,
	[CustomerAddress] [varchar](100) NOT NULL,
	[CustomerPhone] [varchar](50) NULL,
	[CustomerMobile] [varchar](50) NULL,
	[CustomerEmailId] [varchar](100) NULL,
	[CustomerPriority] [int] NULL,
	[CustomerRefferredBy] [varchar](50) NULL,
	[CustomerRegisterDate] [datetime] NULL,
	[CustomerIsActive] [bit] NULL,
	[CustomerCancelDate] [smalldatetime] NULL,
	[DefaultDiscountRate] [int] NULL,
	[Remarks] [varchar](100) NULL,
	[BirthDate] [smalldatetime] NULL,
	[AnniversaryDate] [smalldatetime] NULL,
	[AreaLocation] [varchar](100) NULL,
 CONSTRAINT [PK_CustomerMaster_1] PRIMARY KEY CLUSTERED 
(
	[CustomerCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ConfigurationSetting]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConfigurationSetting](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WebsiteName] [varchar](100) NULL,
	[StoreName] [varchar](100) NULL,
	[Address] [varchar](100) NULL,
	[Timing] [varchar](100) NULL,
	[FooterName] [varchar](100) NULL,
	[Printing] [bit] NULL,
	[Configuration] [bit] NULL,
	[SetSlipInch] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ConfigurationSetting] ON
INSERT [dbo].[ConfigurationSetting] ([ID], [WebsiteName], [StoreName], [Address], [Timing], [FooterName], [Printing], [Configuration], [SetSlipInch]) VALUES (1, N'http://www.quickdrycleaning.com', N'Quick Dry Cleaning Software', N'E-166, Kamla Nagar, New Delhi -110007', N'Mon - Fri 9 to 6 , Sat - Sun 10 to 5 .', N'We deals in all type of Drycleaning tasks', 1, 1, 3)
SET IDENTITY_INSERT [dbo].[ConfigurationSetting] OFF
/****** Object:  Table [dbo].[EntSubItemDetails]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EntSubItemDetails](
	[ItemName] [varchar](50) NOT NULL,
	[SubItemName] [varchar](50) NOT NULL,
	[SubItemOrder] [tinyint] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Gents Suit 2Pcs', N'Coat', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Gents Suit 2Pcs', N'Pant', 2)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Bell Boy Coat', N'Bell Boy Coat', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Bell Boy Pant', N'Bell Boy Pant', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Chef Coat', N'Chef Coat', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Chef Pant', N'Chef Pant', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Cook Apron', N'Cook Apron', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Duvet (BIG)', N'Duvet (BIG)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Duvet (SMALL)', N'Duvet (SMALL)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Duvet Covers (BIG)', N'Duvet Covers (BIG)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Duvet Covers (SMALL)', N'Duvet Covers (SMALL)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Pool Towel', N'Pool Towel', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Waiter Cloth', N'Waiter Cloth', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Bag', N'Bag', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Bath Mat', N'Bath Mat', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Bath Robes', N'Bath Robes', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Bed Throw', N'Bed Throw', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Bedsheet (Double)', N'Bedsheet (Double)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Bedsheet (Single)', N'Bedsheet (Single)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Blanket (Double)', N'Blanket (Double)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Blanket (Single)', N'Blanket (Single)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Carpet', N'Carpet', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Chair Covers', N'Chair Covers', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Curtain', N'Curtain', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Curtain (With Lining)', N'Curtain (With Lining)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Curtain (Woolen)', N'Curtain (Woolen)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Cushion Covers (BIG)', N'Cushion Covers (BIG)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Cushions Cover (SMALL)', N'Cushions Cover (SMALL)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Face Towel', N'Face Towel', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Foot Mats', N'Foot Mats', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Frill', N'Frill', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Mattress Protector (BIG)', N'Mattress Protector (BIG)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Mattress Protector (SMALL)', N'Mattress Protector (SMALL)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Napkin', N'Napkin', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Napkin Cocktail', N'Napkin Cocktail', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Pillow Covers (BIG)', N'Pillow Covers (BIG)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Pillow Covers (SMALL)', N'Pillow Covers (SMALL)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Place Matt', N'Place Matt', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Quilt', N'Quilt', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Quilt ( Cover )', N'Quilt ( Cover )', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Quilt ( Double )', N'Quilt ( Double )', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Seat Cover', N'Seat Cover', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Stroll', N'Stroll', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Table Cloth', N'Table Cloth', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Towel (L)', N'Towel (L)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Towel (M)', N'Towel (M)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Towel Hand', N'Towel Hand', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Wiping Cloth', N'Wiping Cloth', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Achkan Embrodiary', N'Achkan Embrodiary', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Achkan Plain', N'Achkan Plain', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Bo-Tie', N'Bo-Tie', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Dhoti', N'Dhoti', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Dhoti-KURTA', N'Dhoti-KURTA', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Kurta', N'Kurta', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Kurta-Pyjama', N'Kurta-Pyjama', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Lungi', N'Lungi', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'OverCoat', N'OverCoat', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Pagri', N'Pagri', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Pyjama', N'Pyjama', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Rain-Coat', N'Rain-Coat', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Safari Suit', N'Safari Suit', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Sherwani', N'Sherwani', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Tie', N'Tie', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Under Wear', N'Under Wear', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Vest', N'Vest', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Waist Coat', N'Waist Coat', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Blouse', N'Blouse', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Capri', N'Capri', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Cardigans', N'Cardigans', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Dangree', N'Dangree', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Dressing Gown', N'Dressing Gown', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Dupatta', N'Dupatta', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Dupatta (Heavy)', N'Dupatta (Heavy)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Frock', N'Frock', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Gown', N'Gown', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Kameez', N'Kameez', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'lehenga', N'lehenga', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Nighty', N'Nighty', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Peticot', N'Peticot', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Salwar', N'Salwar', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Saree', N'Saree', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Saree (Border)', N'Saree (Border)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Saree (Heavy work)', N'Saree (Heavy work)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Saree (Light Work)', N'Saree (Light Work)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Saree (Medium Work)', N'Saree (Medium Work)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Saree (Printed)', N'Saree (Printed)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Saree Zari', N'Saree Zari', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Scarf', N'Scarf', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Shawl', N'Shawl', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Shawl (Pashmina)', N'Shawl (Pashmina)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Skirt', N'Skirt', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Skirt Top', N'Skirt Top', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Slacks', N'Slacks', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Stoking', N'Stoking', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Top', N'Top', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Stuff toy', N'Stuff toy', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Achkaan Plain', N'Baby Achkaan Plain', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Achkaan Embrodry', N'Baby Achkaan Embrodry', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Shirt', N'Baby Shirt', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Trouser', N'Baby Trouser', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'baby Jeans', N'baby Jeans', 1)
GO
print 'Processed 100 total records'
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Coat', N'Baby Coat', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Skirt', N'Baby Skirt', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Slacks', N'Baby Slacks', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Stoking', N'Baby Stoking', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Top', N'Baby Top', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Jacket', N'Baby Jacket', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Shorts', N'Baby Shorts', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Sweater', N'Baby Sweater', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Sweat Shirt', N'Baby Sweat Shirt', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby T Shirt', N'Baby T Shirt', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Muflaur', N'Muflaur', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Belt', N'Belt', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Cap', N'Cap', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Coat', N'Coat', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Long Coat', N'Long Coat', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Handkerchief', N'Handkerchief', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Hat', N'Hat', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Jacket', N'Jacket', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Jacket (Leather)', N'Jacket (Leather)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Jacket Half', N'Jacket Half', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Jeans', N'Jeans', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Pullover', N'Pullover', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Robe', N'Robe', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Shirt', N'Shirt', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Short', N'Short', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Socks', N'Socks', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Sweater (FS)', N'Sweater (FS)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Sweater (HS)', N'Sweater (HS)', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Sweat-Shirt', N'Sweat-Shirt', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Tie', N'Tie', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Trouser', N'Trouser', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Trousers Half', N'Trousers Half', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'T-Shirt', N'T-Shirt', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Ladies Suit (2 Pcs)', N'Suit', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Ladies Suit (2 Pcs)', N'Salwar', 2)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Lehenga 2pcs', N'Lehnga', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Lehenga 2pcs', N'Salwar', 2)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Salwar-Kameez', N'Salwar', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Salwar-Kameez', N'Kameez', 2)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Kurta-Pyjama', N'Baby Kurta', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Kurta-Pyjama', N'Baby Pyjama', 2)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Suit 2PC', N'Baby Coat', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Suit 2PC', N'Baby Trouser', 2)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Suit Female 2PC', N'Baby Salwar', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Suit Female 2PC', N'Baby Kameez', 2)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Lehnga', N'Baby Salwar', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Lehnga', N'Baby Kameez', 2)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Skirt Top', N'Baby Skirt', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Skirt Top', N'Baby Top', 2)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Gents Suit 3Pcs', N'Gents Shirt', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Gents Suit 3Pcs', N'Gents Trouser', 2)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Gents Suit 3Pcs', N'Gents Coat', 3)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Ladies Suit (3 Pcs)', N'Ladies Suit', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Ladies Suit (3 Pcs)', N'Ladies Salwar', 2)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Ladies Suit (3 Pcs)', N'Ladies Dupatta', 3)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Lehenga 3pcs', N'Ladies Salwar', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Lehenga 3pcs', N'Ladies Dupatta', 2)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Lehenga 3pcs', N'Ladies Blowse', 3)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Suit 3PC', N'Baby Shirt', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Suit 3PC', N'Baby Coat', 2)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Suit 3PC', N'Baby Trouser', 3)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Suit Female 3PC', N'Baby Suit', 1)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Suit Female 3PC', N'Baby Salwar', 2)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'Baby Suit Female 3PC', N'Baby Dupatta', 3)
INSERT [dbo].[EntSubItemDetails] ([ItemName], [SubItemName], [SubItemOrder]) VALUES (N'ddLADIES SUIT (2 PCS)', N'ddLADIES SUIT (2 PCS)', 1)
/****** Object:  Table [dbo].[EntRecleaning]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EntRecleaning](
	[BookingNumber] [int] NOT NULL,
	[SubItemName] [varchar](30) NOT NULL,
	[ItemSentForReclean] [int] NOT NULL,
	[ItemReceivedFromReclean] [int] NOT NULL,
	[SendingDate] [datetime] NULL,
	[ReceivingDate] [datetime] NULL,
 CONSTRAINT [PK_EntRecleaning] PRIMARY KEY CLUSTERED 
(
	[BookingNumber] ASC,
	[SubItemName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EntPayment]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EntPayment](
	[BookingNumber] [varchar](15) NOT NULL,
	[PaymentDate] [smalldatetime] NOT NULL,
	[PaymentMade] [float] NOT NULL,
	[DiscountOnPayment] [float] NOT NULL,
	[DeliveryStatus] [bit] NULL,
	[MsgStaus] [bit] NULL,
	[DeliveryMsg] [varchar](100) NULL,
	[PaymentType] [varchar](50) NULL,
	[PaymentRemarks] [varchar](100) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[CleanDataBase]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CleanDataBase]	
AS
BEGIN	
	Select CustomerName,CustomerAddress From CustomerMaster
END
GO
/****** Object:  Table [dbo].[EntBookingDetails]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EntBookingDetails](
	[BookingNumber] [varchar](15) NOT NULL,
	[ISN] [int] NOT NULL,
	[ItemName] [varchar](50) NOT NULL,
	[ItemTotalQuantity] [int] NOT NULL,
	[ItemProcessType] [varchar](50) NOT NULL,
	[ItemQuantityAndRate] [varchar](2000) NOT NULL,
	[ItemExtraProcessType1] [varchar](50) NOT NULL,
	[ItemExtraProcessRate1] [float] NOT NULL,
	[ItemExtraProcessType2] [varchar](50) NOT NULL,
	[ItemExtraProcessRate2] [float] NOT NULL,
	[ItemExtraProcessType3] [varchar](50) NOT NULL,
	[ItemExtraProcessRate3] [float] NOT NULL,
	[ItemSubTotal] [float] NOT NULL,
	[ItemStatus] [int] NOT NULL,
	[ItemRemark] [varchar](100) NULL,
	[DeliveredQty] [int] NOT NULL,
	[ItemColor] [varchar](50) NULL,
	[VendorItemStatus] [int] NULL,
	[STPAmt] [float] NULL,
	[STEP1Amt] [float] NULL,
	[STEP2Amt] [float] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[prcTodayDate]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[prcTodayDate]
(
@TodayDate datetime=null,
@Flag int=null,
@ExpKey VARCHAR(MAX)='',
@Key1 nvarchar(max)=''
)
as
begin

if(@Flag=1)
begin
insert into mstDate(TodayDate,Key1) values(@TodayDate,@Key1)
declare @day int
select @day=Key2 from mstTask WHERE Key1=@Key1
update mstTask set Key2=@day+1 WHERE Key1=@Key1
end

if(@Flag=2)
begin
select top 1 CONVERT(VARCHAR, TodayDate,106) as TodayDate  from mstDate Where Key1=@Key1 order by DateId desc
end
if(@Flag=3)
begin
insert into mstExpKey(ExpKey) values(@ExpKey)
end
if(@Flag=4)
begin
select * from mstExpKey where ExpKey=@ExpKey
end
if(@Flag=5)
begin
select * from msttask order by key1
end
end
GO
/****** Object:  StoredProcedure [dbo].[prcTask]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[prcTask]
(
@Key1 nvarchar(100)=null,
@Key2 int=null,
@Flag int=null
)
as
begin

if(@Flag=1)
begin
insert into mstTask(Key1,Key2) values(@Key1, @Key2)
end

if(@Flag=2)
begin
update mstTask
set
Key2=@Key2
where 
Key1=@Key1
end

if(@Flag=3)
begin
select * from mstTask order by key1 desc
end

if(@Flag=4)
begin
select * from mstTask where key1=@key1
end

end
GO
/****** Object:  Table [dbo].[mstVendor]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[mstVendor](
	[ID] [int] NOT NULL,
	[VendorSalutation] [varchar](10) NOT NULL,
	[VendorName] [varchar](50) NOT NULL,
	[VendorAddress] [varchar](100) NOT NULL,
	[VendorPhone] [varchar](50) NULL,
	[VendorMobile] [varchar](50) NULL,
	[VendorEmailId] [varchar](100) NULL,
	[JobType] [int] NULL,
 CONSTRAINT [PK_mstVendor] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BarcodeTable]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BarcodeTable](
	[Id] [int] NOT NULL,
	[BookingDate] [smalldatetime] NULL,
	[CurrentTime] [varchar](50) NULL,
	[DueDate] [smalldatetime] NULL,
	[Item] [varchar](100) NULL,
	[BarCode] [varchar](200) NULL,
	[Process] [varchar](100) NULL,
	[StatusId] [int] NULL,
	[BookingNo] [varchar](15) NULL,
	[SNo] [int] NULL,
	[RowIndex] [int] NULL,
	[BookingByCustomer] [varchar](50) NULL,
	[Colour] [varchar](50) NULL,
	[ItemExtraprocessType] [varchar](100) NULL,
	[DeliveredQty] [bit] NOT NULL,
	[DrawlStatus] [bit] NULL,
	[DrawlAlloted] [bit] NULL,
	[AllottedDrawl] [varchar](50) NULL,
	[DeliverItemStaus] [varchar](50) NULL,
	[ClothDeliveryDate] [smalldatetime] NULL,
	[ItemRemarks] [varchar](max) NULL,
	[ItemTotalandSubTotal] [varchar](max) NULL,
	[ItemExtraprocessType2] [varchar](max) NULL,
	[BookingItemName] [varchar](max) NULL,
	[BarcodeISN] [varchar](max) NULL,
	[DelQty] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[Sp_InsUpd_FirstTimeConfigSettings]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Manoj Gupta>
-- Create date: <20 Aug 2011>
-- Description:	<To save config settings>
-- =============================================
CREATE PROCEDURE [dbo].[Sp_InsUpd_FirstTimeConfigSettings]
	(		
		@WebsiteName varchar(max)='',
		@StoreName varchar(max)='',
		@Address varchar(max)='',
		@Timing varchar(max)='',
		@FooterName varchar(max)='',
		@SetSlipInch int='',
		@Printing bit ='',
		@Configuration bit =''
	)
AS
BEGIN
	IF EXISTS(SELECT * FROM ConfigurationSetting) 
		BEGIN
			UPDATE ConfigurationSetting SET WebsiteName = @WebsiteName, StoreName = @StoreName, Address = @Address, Timing = @Timing , FooterName = @FooterName	, Printing = @Printing ,SetSlipInch=@SetSlipInch,Configuration = @Configuration
		END
	ELSE
		BEGIN
			INSERT INTO ConfigurationSetting (WebsiteName,StoreName,Address,Timing,FooterName,Printing,SetSlipInch,Configuration)
				VALUES (@WebsiteName,@StoreName,@Address,@Timing,@FooterName,@Printing,@SetSlipInch,@Configuration)
		END	
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_InsUpd_ConfigSettings]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <16 July 2010>
-- Description:	<To save config settings>
-- =============================================
CREATE PROCEDURE [dbo].[Sp_InsUpd_ConfigSettings]
	(
		@DefItemId int = '',
		@DefMainProcessCode varchar(5) = '',
		@DefExtraProcessCode varchar(5) = '',
		@StartBookingNumberFrom int = '',
		@CustomerMessage varchar(max)='',
		@DefaultColorName varchar(max)='',
		@DefaultTime varchar(max)='',
		@DefaultAmPm varchar(10) = '',
		@DeliveryDateOffset int='',
		@DefaultSearchCriteria varchar(50)='',
		@AmountType varchar(50)=''
	)
AS
BEGIN
	IF EXISTS(SELECT * FROM MstConfigSettings)
		BEGIN
			UPDATE MstConfigSettings SET DefaultItemId = @DefItemId, DefaultProcessCode = @DefMainProcessCode, DefaultExtraProcessCode = @DefExtraProcessCode, StartBookingNumberFrom = @StartBookingNumberFrom	, CustomerMessage = @CustomerMessage , DefaultColorName = @DefaultColorName , DefaultTime = @DefaultTime , DefaultAmPm = @DefaultAmPm , DeliveryDateOffset = @DeliveryDateOffset , DefaultSearchCriteria = @DefaultSearchCriteria,AmountType=@AmountType
		END
	ELSE
		BEGIN
			INSERT INTO MstConfigSettings (StartBookingNumberFrom, DefaultItemId, DefaultProcessCode, DefaultExtraProcessCode, CustomerMessage,DefaultColorName,DefaultTime,DefaultAmPm,DeliveryDateOffset,DefaultSearchCriteria,AmountType)
				VALUES (@StartBookingNumberFrom, @DefItemId, @DefMainProcessCode, @DefExtraProcessCode, @CustomerMessage,@DefaultColorName,@DefaultTime,@DefaultAmPm,@DeliveryDateOffset,@DefaultSearchCriteria,@AmountType)
		END	
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ReceiptConfigSetting]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_ReceiptConfigSetting]
(
	@HeaderText VARCHAR(40)='',
	@HeaderFontName VARCHAR(MAX)='',
	@HeaderFontSize VARCHAR(MAX)='',
	@HeaderFontStyle VARCHAR(MAX)='',
	@AddressText VARCHAR(70)='',
	@AddressFontName VARCHAR(MAX)='',
	@AddressFontSize VARCHAR(MAX)='',
	@AddressFontStyle VARCHAR(MAX)='',
	@LogoLeftRight BIT='',
	@LogoHeight VARCHAR(MAX)='',
	@LogoWeight VARCHAR(MAX)='',
	@SloganText VARCHAR(100)='',
	@SloganFontName VARCHAR(MAX)='',
	@SloganFontSize VARCHAR(MAX)='',
	@SloganFontStyle VARCHAR(MAX)='',
	@Barcode BIT='',
	@PreviewDue BIT='',
	@Term1 VARCHAR(100)='',
	@Term2 VARCHAR(100)='',
	@Term3 VARCHAR(100)='',
	@Term4 VARCHAR(100)='',
	@FooterSloganText VARCHAR(100)='',
	@FooterSloganFontName VARCHAR(MAX)='',
	@FooterSloganFontSize VARCHAR(MAX)='',
	@FooterSloganFontStyle VARCHAR(MAX)='',
	@ST BIT='',
	@Flag VARCHAR(MAX)='',
	@PrinterLaserOrDotMatrix bit='',
	@PrintLogoonReceipt bit='',
	@PrePrinted bit='',
	@TextFontStyleUL VARCHAR(MAX)='',
	@TextFontItalic VARCHAR(MAX)='',
	@TextAddressUL VARCHAR(MAX)='',
	@TextAddressItalic VARCHAR(MAX)='',
	@HeaderSloganUL VARCHAR(MAX)='',
	@HeaderSloganItalic VARCHAR(MAX)='',
	@FooterSloganUL VARCHAR(MAX)='',
	@FooterSloganItalic VARCHAR(MAX)='',
	@ShowHeaderSlogan bit='',
	@ShowFooterSlogan bit='',
	@TermsAndConditionTrue bit='',
	@DotMatrixPrinter40Cloumn bit='',
	@TableBorder bit='',
	@NDB VARCHAR(MAX)='',
	@NDI VARCHAR(MAX)='',
	@NDU VARCHAR(MAX)='',
	@NDFName VARCHAR(MAX)='',
	@NDFSize VARCHAR(MAX)='',
	@NAB VARCHAR(MAX)='',
	@NAI VARCHAR(MAX)='',
	@NAU VARCHAR(MAX)='',
	@NAFName VARCHAR(MAX)='',
	@NAFSize VARCHAR(MAX)='',
	@CurrencyType VARCHAR(MAX)=''
	
)
as
BEGIN
	IF(@Flag=1)
		IF EXISTS(SELECT * FROM mstReceiptConfig)
			BEGIN
				UPDATE  mstReceiptConfig  SET HeaderText=@HeaderText,HeaderFontName=@HeaderFontName,HeaderFontSize=@HeaderFontSize,HeaderFontStyle=@HeaderFontStyle,AddressText=@AddressText,AddressFontName=@AddressFontName,AddressFontSize=@AddressFontSize,AddressFontStyle=@AddressFontStyle,LogoLeftRight=@LogoLeftRight,LogoHeight=@LogoHeight,LogoWeight=@LogoWeight,SloganText=@SloganText,SloganFontName=@SloganFontName,SloganFontSize=@SloganFontSize,SloganFontStyle=@SloganFontStyle,Barcode=@Barcode,PreviewDue=@PreviewDue,Term1=@Term1,Term2=@Term2,Term3=@Term3,Term4=@Term4,FooterSloganText=@FooterSloganText,FooterSloganFontName=@FooterSloganFontName,FooterSloganFontSize=@FooterSloganFontSize,FooterSloganFontStyle=@FooterSloganFontStyle,ST=@ST,PrinterLaserOrDotMatrix=@PrinterLaserOrDotMatrix,PrintLogoonReceipt=@PrintLogoonReceipt,PrePrinted=@PrePrinted,TextFontStyleUL=@TextFontStyleUL,TextFontItalic=@TextFontItalic,TextAddressUL=@TextAddressUL,TextAddressItalic=@TextAddressItalic,HeaderSloganUL=@HeaderSloganUL,HeaderSloganItalic=@HeaderSloganItalic,FooterSloganUL=@FooterSloganUL,FooterSloganItalic=@FooterSloganItalic,ShowHeaderSlogan=@ShowHeaderSlogan,ShowFooterSlogan=@ShowFooterSlogan,TermsAndConditionTrue=@TermsAndConditionTrue,DotMatrixPrinter40Cloumn=@DotMatrixPrinter40Cloumn,TableBorder=@TableBorder,NDB=@NDB,NDI=@NDI,NDU=@NDU,NDFName=@NDFName,NDFSize=@NDFSize,NAB=@NAB,NAI=@NAI,NAU=@NAU,NAFName=@NAFName,NAFSize=@NAFSize,CurrencyType=@CurrencyType
			END
		ELSE
			BEGIN
				INSERT INTO mstReceiptConfig (HeaderText,HeaderFontName,HeaderFontSize,HeaderFontStyle,AddressText,AddressFontName,AddressFontSize,AddressFontStyle,LogoLeftRight,LogoHeight,LogoWeight,SloganText,SloganFontName,SloganFontSize,SloganFontStyle,Barcode,PreviewDue,Term1,Term2,Term3,Term4,FooterSloganText,FooterSloganFontName,FooterSloganFontSize,FooterSloganFontStyle,ST,PrinterLaserOrDotMatrix,PrintLogoonReceipt,PrePrinted,TextFontStyleUL,TextFontItalic,TextAddressUL,TextAddressItalic,HeaderSloganUL,HeaderSloganItalic,FooterSloganUL,FooterSloganItalic,ShowHeaderSlogan,ShowFooterSlogan,TermsAndConditionTrue,DotMatrixPrinter40Cloumn,TableBorder,NDB,NDI,NDU,NDFName,NDFSize,NAB,NAI,NAU,NAFName,NAFSize,CurrencyType) 
					VALUES(@HeaderText,@HeaderFontName,@HeaderFontSize,@HeaderFontStyle,@AddressText,@AddressFontName,@AddressFontSize,@AddressFontStyle,@LogoLeftRight,@LogoHeight,@LogoWeight,@SloganText,@SloganFontName,@SloganFontSize,@SloganFontStyle,@Barcode,@PreviewDue,@Term1,@Term2,@Term3,@Term4,@FooterSloganText,@FooterSloganFontName,@FooterSloganFontSize,@FooterSloganFontStyle,@ST,@PrinterLaserOrDotMatrix,@PrintLogoonReceipt,@PrePrinted,@TextFontStyleUL,@TextFontItalic,@TextAddressUL,@TextAddressItalic,@HeaderSloganUL,@HeaderSloganItalic,@FooterSloganUL,@FooterSloganItalic,@ShowHeaderSlogan,@ShowFooterSlogan,@TermsAndConditionTrue,@DotMatrixPrinter40Cloumn,@TableBorder,@NDB,@NDI,@NDU,@NDFName,@NDFSize,@NAB,@NAI,@NAU,@NAFName,@NAFSize,@CurrencyType)		
			END	
	IF(@Flag=2)
		BEGIN
			SELECT * FROM mstReceiptConfig 
		END	
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_RecleanReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_RecleanReport '1 jul 2010','1 aug 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_RecleanReport]
	(
		@BookDate1 datetime,
		@BookDate2 datetime
	)
AS
BEGIN
	SELECT BookingNumber, SubItemName As Item, ItemSentForReclean as SentQty, ItemReceivedFromReclean as ReceivedQty
	FROM EntRecleaning
	WHERE SendingDate BETWEEN @BookDate1 AND @BookDate2
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Sel_RecForItemIdUpdate]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
EXEC SP_Sel_RecForItemIdUpdate '1'
*/
CREATE PROCEDURE [dbo].[SP_Sel_RecForItemIdUpdate]
	(
		@ItemId int = 0
	)
AS
BEGIN	
	SELECT ItemId, ItemName, NumberOfSubItems, ItemCode From ItemMaster WHERE ItemId = @ItemId
	SELECT ItemName, SubItemName From EntSubItemDetails WHERE ItemName IN (SELECT DISTINCT ItemName  From ItemMaster WHERE ItemId = @ItemId) ORDER BY SubItemOrder
	
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_DefaultBirthDayCustomer]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Gupta>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport '1 SEP 2010','2 sep 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_DefaultBirthDayCustomer]
	(
		@BookDate1 datetime,		
		@MainDate datetime
	)
AS
BEGIN
	--DECLARE @Date datetime,@MainDate datetime	
	set @MainDate=''
	set @MainDate= (SELECT convert(varchar, DATEADD(DAY, DATEDIFF(DAY, 0, @BookDate1), 0) + 5,106))
	SELECT  CustomerName, CustomerAddress, CustomerPhone, CustomerMobile, convert(varchar,BirthDate,106) as BirthDate	FROM dbo.CustomerMaster	
	where BirthDate BETWEEN @BookDate1 AND @MainDate
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_DefaultAnniveraryCustomer]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Gupta>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport '1 SEP 2010','2 sep 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_DefaultAnniveraryCustomer]
	(
		@BookDate1 datetime,		
		@MainDate datetime
	)
AS
BEGIN
	--DECLARE @Date datetime,@MainDate datetime	
	set @MainDate=''
	set @MainDate= (SELECT convert(varchar, DATEADD(DAY, DATEDIFF(DAY, 0, @BookDate1), 0) + 5,106))
	SELECT  CustomerName, CustomerAddress, CustomerPhone, CustomerMobile, convert(varchar,AnniversaryDate,106) as AnniversaryDate FROM dbo.CustomerMaster	
	where AnniversaryDate BETWEEN @BookDate1 AND @MainDate
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Dry_DefaultDataInMasters]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Manoj Gupta>
-- Create date: <20 Aug 2011>
-- Description:	<To save Default Data In Software In First Time when data was clean This procedure was execute>
-- =============================================

CREATE PROCEDURE [dbo].[sp_Dry_DefaultDataInMasters]		
	
	
AS
BEGIN 
	
			--select * from UserTypeMaster
			-- Insert Data In The First Time UserTypeMaster
			INSERT INTO UserTypeMaster (UserTypeID,UserType,UserTypeDetails) VALUES ('1','Administrator','BuiltInAdministrator')
			INSERT INTO UserTypeMaster (UserTypeID,UserType,UserTypeDetails) VALUES ('2','SuperVisor','SuperVisor')
			INSERT INTO UserTypeMaster (UserTypeID,UserType,UserTypeDetails) VALUES ('3','User','General User')		
			
			--select * from usermaster
			-- Insert Data In The First Time UserTypeMaster
			INSERT INTO UserMaster (UserId,UserPassword,UserTypeCode,UserBranchCode,UserName,UserAddress,UserPhoneNumber,UserMobileNumber,UserEmailId,UserActive) VALUES ('admin','admin','1','HO','Admin','','','','','1')		

			--select * from BranchMaster
			-- Insert Data In The First Time BranchMaster
			INSERT INTO BranchMaster (BranchCode,BranchName,BranchAddress,BranchPhone,BranchSlogan) values ('HO','Quick Dry Cleaning','E-1166, Kamla Nagar New Delhi - 110007','1123847402','Modernize Your Dry Cleaning Business')	

			--select * from mstcolor
			-- Insert Data In The First Time ColorMaster 
			
			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES ('1','Red','RD')
			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES ('2','Blue','BL')
			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES ('3','Green','GR')
			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES ('4','Black','BK')
			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES ('5','Gray','GY')			
			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES ('6','MUSTAD','MU')
			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES ('7','ORANGE','OR')
			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES ('8','GOLDEN','GO')
			
			-- Insert Data In The First Time ProcessMaster 
			
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES ('ALT','Alteration','FALSE','FALSE','0','FALSE','FALSE')
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES ('CL','Calendering','FALSE','FALSE','0','FALSE','FALSE')
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES ('DC','Dry Cleaning','FALSE','FALSE','0','FALSE','FALSE')
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES ('DY','DYE','FALSE','FALSE','0','FALSE','FALSE')
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES ('RE','Repairing','FALSE','FALSE','0','FALSE','FALSE')
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES ('RF','Raffu','FALSE','FALSE','0','FALSE','FALSE')
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES ('SP','Steam Press','FALSE','FALSE','0','FALSE','FALSE')
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES ('ST','Starch','FALSE','FALSE','0','FALSE','FALSE')
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES ('WC','Wet Cleaning','FALSE','FALSE','0','FALSE','FALSE')
			

			

			-- select * from mstRemark			
			INSERT INTO mstRemark (ID,Remarks) VALUES ('1','Stain')
			INSERT INTO mstRemark (ID,Remarks) VALUES ('2','Press Mark')
			INSERT INTO mstRemark (ID,Remarks) VALUES ('3','Faded')
			INSERT INTO mstRemark (ID,Remarks) VALUES ('4','Loose Buttons')
			INSERT INTO mstRemark (ID,Remarks) VALUES ('5','Button Missing')
			INSERT INTO mstRemark (ID,Remarks) VALUES ('6','Cuts')
			INSERT INTO mstRemark (ID,Remarks) VALUES ('7','Rust Stain')

			-- select * from PriorityMaster
			INSERT INTO PriorityMaster (PriorityID,Priority) VALUES ('0','Hard Starch')
			INSERT INTO PriorityMaster (PriorityID,Priority) VALUES ('1','No Starch')
			INSERT INTO PriorityMaster (PriorityID,Priority) VALUES ('2','Medium Starch')
			INSERT INTO PriorityMaster (PriorityID,Priority) VALUES ('3','Suits with hanger')
			INSERT INTO PriorityMaster (PriorityID,Priority) VALUES ('4','Flat press on pleetless trousers')
			INSERT INTO PriorityMaster (PriorityID,Priority) VALUES ('5','Tie in separate packing')
			
			-- select * from CustomerMaster
			DECLARE @CurentDate DATETIME
			SET @CurentDate=(GETDATE())
			INSERT INTO CustomerMaster (ID,CustomerCode,CustomerSalutation,CustomerName,CustomerAddress,CustomerPhone,CustomerMobile,CustomerEmailId,CustomerPriority,CustomerRefferredBy,CustomerRegisterDate,CustomerIsActive,CustomerCancelDate,DefaultDiscountRate,Remarks,BirthDate,AnniversaryDate,AreaLocation) VALUES ('1','Cust1','Mr','Dee Coup','E-166, Kamla Nagar, New Delhi-110007','9810755331','9212515278','info@deecoup.com','1','',@CurentDate,'1',@CurentDate,'10','','','','Kamla Nagar')

			--- SELECT * FROM LEDGERMASTER
			INSERT INTO LedgerMaster (LedgerName,OpenningBalance,CurrentBalance) VALUES ('CASH','0','0')
			INSERT INTO LedgerMaster (LedgerName,OpenningBalance,CurrentBalance) VALUES ('Cust1','0','0')			
			INSERT INTO LedgerMaster (LedgerName,OpenningBalance,CurrentBalance) VALUES ('Sales','0','0')

			
			
			-- select * from EmployeeMaster			
			INSERT INTO dbo.EmployeeMaster (ID,EmployeeCode,EmployeeSalutation,EmployeeName,EmployeeAddress,EmployeePhone,EmployeeMobile,EmployeeEmailId) VALUES ('1','Emp1','Mr','Vivek Saini','DC Web Services Pvt Ltd.  E-166, Kamla Nagar New Delhi-110007','9810755331','9212515278','Vivek.Saini@deecoup.com')

			--- Insert Data In the mstRecordCheck

			INSERT INTO mstRecordCheck (Status) values ('Save')		

			INSERT INTO mstjobtype (ID,JobType) values ('1','Manager')		

			INSERT INTO ShiftMaster (ShiftName) values ('5 PM')	
			
	END
GO
/****** Object:  StoredProcedure [dbo].[sp_BindGrid]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<>
-- Create date: <>
-- Description:	<>
-- =============================================
CREATE PROCEDURE [dbo].[sp_BindGrid]	
AS
BEGIN	
	Select CustomerName,CustomerAddress From CustomerMaster
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_TimeWiseClothBookingReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport '1 SEP 2010','2 sep 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_TimeWiseClothBookingReport]
	(
		@BookDate1 datetime,
		@BookDate2 datetime,
		@BookingTime varchar(max),		
		@BookingTime1 nvarchar(50)='',
		@Format varchar(10)=''
	)
AS
BEGIN
	SELECT dbo.BarcodeTable.Item, SUM(dbo.BarcodeTable.SNo) AS Qty FROM dbo.BarcodeTable INNER JOIN
           dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN
           dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
	WHERE (dbo.EntBookings.BookingTime BETWEEN @BookingTime AND @BookingTime1) AND (dbo.EntBookings.Format = @Format) AND (dbo.BarcodeTable.BookingDate BETWEEN @BookDate1 AND @BookDate2)
	GROUP BY dbo.BarcodeTable.SNo, dbo.BarcodeTable.Item
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_StockReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_StockReport 1,'2 Piece'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_StockReport]
	(
		@Flag int =0,
		@ItemName varchar(Max)=''
	)
AS
BEGIN
	IF(@Flag=0)
		BEGIN
			SELECT Item, SUM(SNO-DelQty) As StockQty 
			FROM barcodeTable
			WHERE DelQty < SNO
			Group By Item
			ORDER BY Item
		END
	ELSE IF(@Flag=1)
		BEGIN
			SELECT bookingno as BookingNumber , SUM(SNO) AS ItemsReceived, SUM(DelQty) As Delivered
			FROM barcodeTable
			WHERE Item = @ItemName
			Group By bookingno, Item
			ORDER BY Item
		END
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_SalesLedgerReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Challan details>
-- =============================================
EXEC Sp_Sel_DayBookReport '14 SEP 2010', '14 SEP 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_SalesLedgerReport]
	(
		@PaymentDate1 datetime,
		@PaymentDate2 datetime
	)
AS
BEGIN
	CREATE TABLE #TmpTable (PaymentDate varchar(20), BookingNumber varchar(20), PaymentMade float)
	INSERT INTO #TmpTable(PaymentDate, PaymentMade)
		SELECT Convert(varchar,PaymentDate,106), SUM(PaymentMade) As Received FROM EntPayment INNER JOIN EntBookings ON EntPayment.BookingNumber = EntBookings.BookingNumber
		WHERE PaymentDate BETWEEN @PaymentDate1 AND DateADD(Day,1,@PaymentDate2) AND PaymentMade<>0
		GROUP BY Convert(varchar,PaymentDate,106)
	SELECT PaymentDate, PaymentMade FROM #TmpTable

	DELETE FROM #TmpTable

	INSERT INTO #TmpTable(PaymentDate, BookingNumber, PaymentMade)
		SELECT Convert(varchar,PaymentDate,106), EntBookings.BookingNumber, SUM(PaymentMade) As Received FROM EntPayment INNER JOIN EntBookings ON EntPayment.BookingNumber = EntBookings.BookingNumber
		WHERE PaymentDate BETWEEN @PaymentDate1 AND DateADD(Day,1,@PaymentDate2) AND PaymentMade<>0
		GROUP BY Convert(varchar,PaymentDate,106), EntBookings.BookingNumber
	SELECT PaymentDate, BookingNumber, PaymentMade FROM #TmpTable
	
	DROP TABLE #TmpTable
END
GO
/****** Object:  StoredProcedure [dbo].[SP_SelectBookingRecordsForEdition]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
EXEC SP_SelectBookingRecordsForEdition '8/1/2010','8/30/2010'
*/
CREATE PROCEDURE [dbo].[SP_SelectBookingRecordsForEdition]
(
	@BookingDate1 datetime='',
	@BookingDate2 datetime=''
)
AS
BEGIN
	SELECT Convert(varchar,EntBookings.BookingDate,106) As BookingDate, EntBookings.BookingNumber, EntBookingDetails.ItemName, 
                      EntBookingDetails.ItemTotalQuantity As 'TotalQty', EntBookingDetails.ItemProcessType As 'Process', EntBookingDetails.ItemQuantityAndRate As 'ItemQtyAndRate', 
                      EntBookingDetails.ItemExtraProcessType1 As 'ExtraProcess1', EntBookingDetails.ItemExtraProcessRate1 As 'ExtraProcessRate1', EntBookingDetails.ItemExtraProcessType2 As 'ExtraProcess2', 
                      EntBookingDetails.ItemExtraProcessRate2 As 'ExtraProcessRate2', EntBookingDetails.ItemSubTotal
	FROM   EntBookings INNER JOIN EntBookingDetails ON EntBookings.BookingNumber = EntBookingDetails.BookingNumber
	WHERE (BookingDate BETWEEN @BookingDate1 AND @BookingDate2)
	ORDER BY BookingDate, EntBookings.BookingNumber

END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_RecordsForBookingCancellation]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To Select Records for Booking Cancellation>
-- =============================================
EXEC Sp_Sel_RecordsForBookingCancellation '1'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_RecordsForBookingCancellation]
	(
		@BookingNumber varchar(20)
	)
AS
BEGIN
	SELECT EntBookings.BookingNumber ,EntBookings.BookingDate, CustomerCode, CustomerSalutation + ' '  + CustomerName As CustomerName, CustomerAddress, NetAmount, BookingStatus, SUM(PaymentMade) As PaymentMade FROM EntBookings INNER JOIN CustomerMaster ON EntBookings.BookingByCustomer = CustomerMaster.CustomerCode LEFT JOIN EntPayment ON EntBookings.BookingNumber = EntPayment.BookingNumber
	WHERE EntBookings.BookingNumber = @BookingNumber
	GROUP BY EntBookings.BookingNumber ,EntBookings.BookingDate, CustomerCode, CustomerSalutation, CustomerName, CustomerAddress, NetAmount, BookingStatus
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Dry_BarcodeMaster]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Dry_BarcodeMaster]		
	@Flag VARCHAR(MAX)='',
	@ID int='',
	@BookingDate smalldatetime='',
	@CurrentTime VARCHAR(MAX)='',
	@DueDate smalldatetime='',
	@Item  VARCHAR(MAX)='',
	@BarCode VARCHAR(MAX)='',
	@Process VARCHAR(MAX)='',
	@StatusId int='',
	@BookingNo VARCHAR(MAX)='',
	@SNo int='',
	@RowIndex int='',
	@BookingByCustomer varchar(max)='',
	@Colour varchar(max)='',
	@ItemExtraprocessType varchar(max)='',
	@DrawlStatus bit=false,
	@DrawlAlloted bit=false,
	@DrawlName varchar(100)=null,
	@AllottedDrawl varchar(max)='',
	@DeliveredQty int='',
	@BookingNumber varchar(max)='',
	@ItemName varchar(max)='',
	@ItemsReceivedFromVendor int='',
	@BookingUser varchar(max)='',
	@BookDate1 datetime='',
	@BookDate2 datetime='',
	@ReceiptDeliverd bit='',	
	@Qty int='',
	@ItemRemarks varchar(max)='',
	@CustName varchar(max)='',
	@ItemTotalandSubTotal varchar(max)=''
	
AS
BEGIN
	IF(@Flag=1)
		BEGIN
			INSERT INTO dbo.BarcodeTable
				(Id,BookingDate,CurrentTime,DueDate,Item,BarCode,Process,StatusId,BookingNo,SNo,RowIndex,BookingByCustomer,Colour,ItemExtraprocessType,DrawlStatus,DrawlAlloted,DeliveredQty,ItemRemarks,ItemTotalandSubTotal)
			VALUES
				(@ID,@BookingDate,@CurrentTime,@DueDate,@Item,@BarCode,@Process,@StatusId,@BookingNo,@SNo,@RowIndex,@BookingByCustomer,@Colour,@ItemExtraprocessType,@DrawlStatus,@DrawlAlloted,@DeliveredQty,@ItemRemarks,@ItemTotalandSubTotal)
		END	
	ELSE IF(@Flag=2)
		BEGIN
			UPDATE dbo.BarcodeTable SET DrawlStatus=@DrawlStatus,DrawlAlloted=@DrawlAlloted WHERE BookingNo=@BookingNo AND RowIndex=@RowIndex
		END	
	ELSE IF(@Flag=3)
		BEGIN
			SELECT dbo.BarcodeTable.BookingNo AS BookingNumber, dbo.BarcodeTable.Item, dbo.BarcodeTable.RowIndex FROM dbo.EntChallan INNER JOIN  dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex WHERE (dbo.BarcodeTable.DrawlStatus = 'true') AND (dbo.BarcodeTable.DrawlAlloted = 'false') order by dbo.BarcodeTable.RowIndex asc
		END
	ELSE IF(@Flag=4)
		BEGIN
			SELECT distinct(DrawlName),Id FROM mstDrwal 
		END
	ELSE IF(@Flag=5)
		BEGIN
			SELECT ParentDrawl,Id FROM mstDrwal WHERE DrawlName=@DrawlName 
		END
	ELSE IF(@Flag=6)
		BEGIN
			UPDATE BarcodeTable SET DrawlAlloted=@DrawlAlloted,  AllottedDrawl=@AllottedDrawl where BookingNo=@BookingNo and RowIndex=@RowIndex 
		END
	ELSE IF(@Flag=7)
		BEGIN	
			SELECT dbo.EntBookings.BookingByCustomer, dbo.EntBookings.Qty FROM dbo.EntBookings INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
			WHERE  (dbo.EntBookings.BookingNumber =@BookingNumber)
		END
	ELSE IF(@Flag=8)
		BEGIN	
			Update EntChallan Set ItemsReceivedFromVendor='1', ItemReceivedFromVendorOnDate=getdate() Where BookingNumber=@BookingNo And ItemSno=@RowIndex AND SubItemName=@ItemName
		END
	ELSE IF(@Flag=9)
		BEGIN	
			UPDATE BarcodeTable SET DrawlStatus='True',DrawlAlloted='False',  StatusId = '3'  where BookingNo=@BookingNo and RowIndex=@RowIndex 
		END
	ELSE IF(@Flag=10)
		BEGIN	
			UPDATE EntBookings SET BarCode=@BarCode where BookingNumber=@BookingNo
		END
	ELSE IF(@Flag=11)
		BEGIN	
			SELECT sum(ItemTotalQuantity) as ItemTotalQuantity FROM EntBookingDetails Where BookingNumber=@BookingNo
		END
	ELSE IF(@Flag=12)
		BEGIN	
			SELECT * FROM EntBookings WHERE BookingNumber=@BookingNo and BookingAcceptedByUserId=@BookingUser
		END
	ELSE IF(@Flag=13)
		BEGIN	
			SELECT  dbo.CustomerMaster.CustomerEmailId FROM  dbo.CustomerMaster INNER JOIN  dbo.EntBookings ON dbo.CustomerMaster.CustomerCode = dbo.EntBookings.BookingByCustomer WHERE (dbo.EntBookings.BookingNumber = @BookingNo)
		END
	ELSE IF(@Flag=14)
		BEGIN	
			CREATE TABLE #TmpTable (PaymentDate varchar(20))
			INSERT INTO #TmpTable(PaymentDate)
				SELECT Convert(varchar,PaymentDate,106) FROM EntPayment 
				WHERE BookingNumber=@BookingNo
				GROUP BY Convert(varchar,PaymentDate,106)
			SELECT PaymentDate FROM #TmpTable
			DELETE FROM #TmpTable
		END
	ELSE IF (@Flag=15)
		BEGIN
			SELECT IsActiveServiceTax FROM ProcessMaster WHERE ProcessCode=@Process
		END
	ELSE IF (@Flag=16)
		BEGIN
			SELECT ServiceTax FROM ProcessMaster WHERE ProcessCode=@Process
		END
--	ELSE IF (@Flag=17)
--		BEGIN
----			SELECT     dbo.EntBookingDetails.BookingNumber, dbo.EntBookingDetails.ServiceProcess, 
----                   CASE WHEN dbo.EntBookingDetails.ServiceExtraProcess<>'' THEN  sum(dbo.EntBookingDetails.ServiceProcessAmount) + sum(dbo.EntBookingDetails.ServiceExtraAmount) ELSE sum(dbo.EntBookingDetails.ServiceProcessAmount) END as Amount, convert(varchar,dbo.EntBookings.BookingDate,106) as BookingDate,dbo.ProcessMaster.ServiceTax 
----			FROM         dbo.EntBookings INNER JOIN
----                      dbo.EntBookingDetails ON dbo.EntBookings.BookingNumber = dbo.EntBookingDetails.BookingNumber INNER JOIN
----                      dbo.ProcessMaster ON dbo.EntBookingDetails.ItemProcessType = dbo.ProcessMaster.ProcessCode
----			WHERE     (dbo.EntBookingDetails.ServiceProcess <> '') AND (dbo.EntBookingDetails.ServiceProcessAmount <> 0) AND dbo.EntBookings.BookingDate BETWEEN @BookDate1 AND @BookDate2                      
----			group by dbo.EntBookingDetails.BookingNumber,dbo.EntBookingDetails.ServiceProcess,dbo.EntBookings.BookingDate,dbo.ProcessMaster.ServiceTax ,dbo.EntBookingDetails.ServiceExtraProcess
--		END
--	ELSE IF (@Flag=18)
--		BEGIN
--			SELECT     dbo.EntBookingDetails.BookingNumber, dbo.EntBookingDetails.ServiceProcess, 
--                   CASE WHEN dbo.EntBookingDetails.ServiceExtraProcess<>'' THEN  sum(dbo.EntBookingDetails.ServiceProcessAmount) + sum(dbo.EntBookingDetails.ServiceExtraAmount) ELSE sum(dbo.EntBookingDetails.ServiceProcessAmount) END as Amount, convert(varchar,dbo.EntBookings.BookingDate,106) as BookingDate,dbo.ProcessMaster.ServiceTax 
--			FROM         dbo.EntBookings INNER JOIN
--                      dbo.EntBookingDetails ON dbo.EntBookings.BookingNumber = dbo.EntBookingDetails.BookingNumber INNER JOIN
--                      dbo.ProcessMaster ON dbo.EntBookingDetails.ItemProcessType = dbo.ProcessMaster.ProcessCode
--			WHERE     (dbo.EntBookingDetails.ServiceProcess <> '') AND (dbo.EntBookingDetails.ServiceProcessAmount <> 0) AND 
--                      (dbo.EntBookingDetails.ServiceProcess = @Process) AND dbo.EntBookings.BookingDate BETWEEN @BookDate1 AND @BookDate2
--			group by dbo.EntBookingDetails.BookingNumber,dbo.EntBookingDetails.ServiceProcess,dbo.EntBookings.BookingDate,dbo.ProcessMaster.ServiceTax ,dbo.EntBookingDetails.ServiceExtraProcess
--		END
	ELSE IF (@Flag=19)
		BEGIN
			select * from mstRecordCheck
		END
	ELSE IF (@Flag=20)
		BEGIN
			insert into mstRecordCheck (Status) values ('Save')			
		END
	ELSE IF (@Flag=21)
		BEGIN
			SELECT DeliverItemStaus from BarcodeTable where bookingno=@BookingNumber
		END
	ELSE IF (@Flag=22)
		BEGIN
			SELECT NetAmount FROM EntBookings where BookingNumber=@BookingNumber
			SELECT sum(PaymentMade) as PaymentMade FROM EntPayment where BookingNumber=@BookingNumber
		END
	ELSE IF (@Flag=23)
		BEGIN
			UPDATE EntBookings SET ReceiptDeliverd=@ReceiptDeliverd where BookingNumber=@BookingNumber
		END
	ELSE IF (@Flag=24)
		BEGIN
			SELECT     dbo.EntSubItemDetails.SubItemName as ItemName,dbo.ItemMaster.NumberOfsubItems * @Qty as Qty
			FROM         dbo.ItemMaster INNER JOIN
                      dbo.EntSubItemDetails ON dbo.ItemMaster.ItemName = dbo.EntSubItemDetails.ItemName
			WHERE     (dbo.ItemMaster.ItemName = @ItemName)
		END
	ELSE IF(@Flag=25)
		BEGIN
			select * from customermaster where CustomerName like @Process
		END
	ELSE IF(@Flag=26)
		BEGIN			
			--set @CustName=(select CustomerCode from customermaster where CustomerName like @CustName)
			declare @LedgerName varchar(max),@CustomerAddress varchar(max),@TotalDue float
			CREATE TABLE #TmpAcountTable (CustomerName varchar(max),CustomerAddress varchar(max),DuePayment float)
			set @LedgerName=(select '('+customercode+')'+ +' '+ CustomerSalutation+' '+ CustomerName as name1 from customermaster where customercode=@CustName)
			set @CustomerAddress=(select CustomerAddress from customermaster where customercode=@CustName)
			set @TotalDue=(select top(1) ClosingBalance from entledgerentries where ledgername=@CustName order by id desc)
			INSERT INTO #TmpAcountTable(CustomerName,CustomerAddress,DuePayment) values (@LedgerName,@CustomerAddress,@TotalDue)
				
			SELECT * FROM #TmpAcountTable
			DROP TABLE #TmpAcountTable 
		END
	ELSE IF (@Flag=27)
		BEGIN
			SELECT (CustomerCode + '  ' + '-' + '  ' + CustomerName + ' ' + CustomerAddress + ' ' + CustomerMobile) AS CustName FROM   dbo.CustomerMaster  WHERE customername like '%'+@CustName+'%' or CustomerAddress like '%'+@CustName+'%' or CustomerMobile like '%'+@CustName+'%'
		END
	ELSE IF(@Flag=28)
		BEGIN			
			declare @TotalQty int,@CustomerName varchar(max),@CustMobile varchar(max),@TotalAmount float
			--set @BookingNumber='65'
			CREATE TABLE #TmpSMSTable (TotalQty int,CustomerName varchar(max),CustMobile varchar(max),TotalAmount float)
			set @TotalQty=(select sum(sno) as Qty from barcodetable where bookingno=@BookingNumber)
			set @CustomerName=(SELECT dbo.CustomerMaster.CustomerName FROM dbo.CustomerMaster INNER JOIN dbo.EntBookings ON dbo.CustomerMaster.CustomerCode = dbo.EntBookings.BookingByCustomer where entbookings.Bookingnumber=@BookingNumber)
			set @CustMobile=(SELECT dbo.CustomerMaster.CustomerMobile FROM dbo.CustomerMaster INNER JOIN dbo.EntBookings ON dbo.CustomerMaster.CustomerCode = dbo.EntBookings.BookingByCustomer where entbookings.Bookingnumber=@BookingNumber)
			set @TotalAmount=(SELECT dbo.EntBookings.NetAmount FROM dbo.CustomerMaster INNER JOIN dbo.EntBookings ON dbo.CustomerMaster.CustomerCode = dbo.EntBookings.BookingByCustomer where entbookings.Bookingnumber=@BookingNumber)
			INSERT INTO #TmpSMSTable(TotalQty,CustomerName,CustMobile,TotalAmount) values (@TotalQty,@CustomerName,@CustMobile,@TotalAmount)
				
			SELECT * FROM #TmpSMSTable
			DROP TABLE #TmpSMSTable 
		END	 
	END
GO
/****** Object:  StoredProcedure [dbo].[sp_CustomerWiseDueReport]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<>
-- Create date: <>
-- Description:	<>
-- =============================================
CREATE PROCEDURE [dbo].[sp_CustomerWiseDueReport]	
@CustomerName varchar(max)='',
@Flag varchar(max)=''
AS
BEGIN	
	IF(@Flag=1)
	BEGIN
	SELECT     TOP (100) PERCENT CustomerName, CustomerAddress, SUM(DuePayment) AS DuePayment
		FROM         (SELECT     '(' + dbo.CustomerMaster.CustomerCode + ') ' + dbo.CustomerMaster.CustomerSalutation + ' ' + dbo.CustomerMaster.CustomerName AS CustomerName,
                                               dbo.CustomerMaster.CustomerAddress, COALESCE (SUM(dbo.EntBookings.NetAmount), 0) 
                                              - COALESCE (SUM(dbo.EntPayment.PaymentMade), 0) AS DuePayment
                       FROM          dbo.EntBookings LEFT OUTER JOIN
                                              dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber INNER JOIN
                                              dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
                       WHERE      (dbo.EntPayment.PaymentMade <> 0) AND (dbo.CustomerMaster.CustomerName LIKE @CustomerName) OR
                                              (dbo.CustomerMaster.CustomerAddress LIKE @CustomerName) OR
                                              (dbo.CustomerMaster.CustomerPhone LIKE @CustomerName) OR
                                              (dbo.CustomerMaster.CustomerMobile LIKE @CustomerName)
                       GROUP BY dbo.CustomerMaster.CustomerCode, dbo.CustomerMaster.CustomerSalutation, dbo.CustomerMaster.CustomerName, 
                                              dbo.CustomerMaster.CustomerAddress, dbo.EntBookings.NetAmount) AS T1
		GROUP BY CustomerName, CustomerAddress
		ORDER BY CustomerName
	END
ELSE IF(@Flag=2)
	BEGIN
		SELECT     TOP (100) PERCENT CustomerName, CustomerAddress, SUM(DuePayment) AS DuePayment
		FROM         (SELECT     '(' + dbo.CustomerMaster.CustomerCode + ') ' + dbo.CustomerMaster.CustomerSalutation + ' ' + dbo.CustomerMaster.CustomerName AS CustomerName,
                                               dbo.CustomerMaster.CustomerAddress, COALESCE (SUM(dbo.EntBookings.NetAmount), 0) 
                                              - COALESCE (SUM(dbo.EntPayment.PaymentMade), 0) AS DuePayment
                       FROM          dbo.EntBookings LEFT OUTER JOIN
                                              dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber INNER JOIN
                                              dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
                       WHERE      (dbo.EntPayment.PaymentMade <> 0) 
                       GROUP BY dbo.CustomerMaster.CustomerCode, dbo.CustomerMaster.CustomerSalutation, dbo.CustomerMaster.CustomerName, 
                                              dbo.CustomerMaster.CustomerAddress, dbo.EntBookings.NetAmount) AS T1
		GROUP BY CustomerName, CustomerAddress
		ORDER BY CustomerName
	END
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_DayBookReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Challan details>
-- =============================================
EXEC Sp_Sel_DayBookReport '1 Oct 2010', '30 Oct 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_DayBookReport]
	(
		@BookingDate1 datetime,
		@BookingDate2 datetime
	)
AS
BEGIN
	CREATE TABLE #TmpTable (ID int identity (1,1),BookingDate DateTime, BookingNumber varchar(20), DC varchar(200), WC varchar(200))
	INSERT INTO #TmpTable( BookingDate, BookingNumber, DC, WC)
		SELECT BookingDate, BookingNumber, DC, WC
		FROM (SELECT DISTINCT dbo.EntPayment.PaymentDate AS BookingDate, dbo.EntPayment.BookingNumber, dbo.EntPayment.PaymentMade AS DC, '' AS WC
FROM         dbo.EntBookings INNER JOIN
                      dbo.EntBookingDetails ON dbo.EntBookings.BookingNumber = dbo.EntBookingDetails.BookingNumber INNER JOIN
                      dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber
				WHERE     (dbo.EntBookingDetails.ItemProcessType = 'DC') AND dbo.EntPayment.PaymentMade<>0
				UNION
				SELECT dbo.EntPayment.PaymentDate as BookingDate, EntPayment.BookingNumber, '' AS DC,
					CASE WHEN ItemProcessType = 'None' THEN Convert(varchar,(EntPayment.PaymentMade)) ELSE EntPayment.PaymentMade END FROM EntBookings INNER JOIN EntBookingDetails ON EntBookings.BookingNumber = EntBookingDetails.BookingNumber
					INNER JOIN  dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber
					
				WHERE EntBookingDetails.ItemProcessType <> 'DC' AND dbo.EntPayment.PaymentMade<>0 ) AS T1
				
		WHERE BookingDate BETWEEN @BookingDate1 AND @BookingDate2

	UPDATE #TmpTable SET DC = '0' WHERE DC ='' OR DC IS NULL
	UPDATE #TmpTable SET DC = DC + ';'-- WHERE CHARINDEX('@',DC)>0
	UPDATE #TmpTable SET WC = '0' WHERE WC ='' OR WC IS NULL
	UPDATE #TmpTable SET WC = WC + ';'-- WHERE CHARINDEX('@',WC)>0
	--SELECT * FROM #TmpTable
	DECLARE @ID int, @BN varchar(10), @DC varchar(200), @WC varchar(200), @ItemExtra1 float, @ItemExtra2 float, @Qty FLOAT, @Rate FLOAT, @Total float, @Tmp varchar(200)
	SELECT @ID = 0, @BN='', @DC='', @WC = '', @ItemExtra1 = '', @ItemExtra2 = '', @Qty = 0, @Rate = 0, @Total = 0, @Tmp = ''
	DECLARE CURBN CURSOR
	FOR
		SELECT ID, BookingNumber, DC, WC FROM #TmpTable
	OPEN CURBN
		FETCH FROM CURBN INTO @ID, @BN, @DC, @WC
		WHILE @@FETCH_STATUS = 0
			BEGIN
				IF (LEN(COALESCE(@DC,''))>0)
					BEGIN
						SET @Total = 0
						PRINT CHARINDEX(';',@DC)
						WHILE (CHARINDEX(';',@DC)>0)
							BEGIN
								SET @TMP = '' + SUBSTRING (@DC,1,CHARINDEX(';',@DC)-1)
								IF(CHARINDEX('@',@TMP)>0)
									BEGIN
										SET @Qty = CONVERT(FLOAT,SUBSTRING(@TMP,1, CHARINDEX('@', @TMP)-1))
										SET @Rate = CONVERT(FLOAT,SUBSTRING(@TMP,CHARINDEX('@', @TMP)+1, LEN(@Tmp)))
									END
								ELSE
									BEGIN
										SET @QTY = 1
										SET @Rate = @Tmp
									END
								SET @TOTAL = @Total + (@Qty * @Rate)
								SET @DC = SUBSTRING(@DC, CHARINDEX(';', @DC)+1, LEN(@DC))
							END
						UPDATE #TmpTable SET DC = @Total WHERE ID = @ID
						UPDATE #TmpTable SET WC = COALESCE(@ItemExtra1,0) + COALESCE(@ItemExtra2,0)  WHERE ID = @ID
					END
				IF (@WC <> '')
					BEGIN
						SET @Total = 0
						PRINT CHARINDEX(';',@WC)
						WHILE (CHARINDEX(';',@WC)>0)
							BEGIN
								SET @TMP = '' + SUBSTRING (@WC,1,CHARINDEX(';',@WC)-1) print @Tmp 
								IF(CHARINDEX('@',@TMP)>0)
									BEGIN
										SET @Qty = CONVERT(FLOAT,SUBSTRING(@TMP,1, CHARINDEX('@', @TMP)-1))
										SET @Rate = CONVERT(FLOAT,SUBSTRING(@TMP,CHARINDEX('@', @TMP)+1, LEN(@Tmp)-CHARINDEX('@', @TMP)))
									END
								ELSE
									BEGIN
										SET @QTY = 1
										SET @Rate = @Tmp
									END
								PRINT @QTY PRINT @Rate
								SET @TOTAL = @Total + (@Qty * @Rate) 
								SET @WC = SUBSTRING(@WC, CHARINDEX(';', @WC)+1, LEN(@WC)- CHARINDEX(';', @WC)) PRINT @WC
							END
						UPDATE #TmpTable SET WC = @Total WHERE ID = @ID
					END
				FETCH NEXT FROM CURBN INTO @ID, @BN, @DC, @WC
			END
	CLOSE CURBN
	DEALLOCATE CURBN
ALTER TABLE #TmpTable ALTER COLUMN DC FLOAT
ALTER TABLE #TmpTable ALTER COLUMN WC FLOAT
--update #TmpTable set dc=round(dc,2), wc = Convert(varchar,round(wc,2))
--select * from #tMPtABLE
SELECT BookingDate, BookingNumber, DC, WC, DC+WC AS TotalCharges, 0 AS ServiceTax, DC+WC+0 As Total
FROM
	(
	
	SELECT Convert(varchar,BookingDate,106) As BookingDate, BookingNumber, SUM(ROUND(COALESCE(DC,0),2)) AS DC, SUM(ROUND(COALESCE(WC,0),2)) AS WC
	FROM #TmpTable
	Group By BookingNumber,BookingDate
	) As T1
	DROP TABLE #TmpTable
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_CustomerWiseBookingReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
EXEC Sp_Sel_CustomerWiseBookingReport '9/1/2011 00:00:00','10/1/2011 00:00:00','CUST1'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_CustomerWiseBookingReport]
	(
		@BookDate1 datetime,
		@BookDate2 datetime,
		@CustCode varchar(10) = ''
	)
AS
BEGIN
	CREATE TABLE #TmpBookReport (BookingNumber varchar(20), BookingDate varchar(max), NetAmount float, PaymentMade float, DuePayment float)
	INSERT INTO #TmpBookReport (BookingNumber, BookingDate, NetAmount) SELECT BookingNumber, convert(varchar,BookingDate,106), NetAmount FROM EntBookings WHERE BookingByCustomer = @CustCode AND (BookingDate BETWEEN @BookDate1 AND @BookDate2)

	DECLARE @BookingNumber varchar(20), @PaymentMade float, @DuePayment float
	SET @BookingNumber = ''
	DECLARE CURPay  CURSOR
		FOR
			SELECT BookingNumber FROM #TmpBookReport
	FOR READ ONLY
	OPEN CURPay
		FETCH NEXT FROM CURPay INTO @BookingNumber
		WHILE @@FETCH_STATUS = 0
			BEGIN
				SELECT @PaymentMade = COALESCE(SUM(PaymentMade),0) FROM EntPayment WHERE BookingNumber = @BookingNumber
				UPDATE #TmpBookReport SET PaymentMade = @PaymentMade WHERE BookingNumber = @BookingNumber
				FETCH NEXT FROM CURPay INTO @BookingNumber
			END
	CLOSE CURPay
	DEALLOCATE CURPay
	UPDATE #TmpBookReport SET DuePayment = NetAmount - PaymentMade
	SELECT * FROM #TmpBookReport
	DROP TABLE #TmpBookReport
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_CustomerStatus]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <6 Aug 2010>
-- Description:	<To select Challan Details for challan return>
-- =============================================
EXEC Sp_Sel_ReportVendorChallanReturnDetails '1/1/2011','1/30/2011','','',''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_CustomerStatus]
	(
		@BookDate1 datetime,
		@BookDate2	datetime,
		@BookNumberFrom varchar(15) = '',
		@BookNumberUpto varchar(10) = '',
		@ChallanShift varchar(10) = '',
		@CustomerId varchar(15) = ''
	)
AS
BEGIN
	SELECT dbo.EntBookingDetails.BookingNumber, dbo.EntBookingDetails.ISN, CASE WHEN EntBookingDetails.ItemExtraProcessType1 <> 'None' THEN EntBookingDetails.ItemProcessType + ',' + EntBookingDetails.ItemExtraProcessType1 + '' ELSE EntBookingDetails.ItemProcessType END AS ItemProcessType, dbo.EntBookingDetails.ItemQuantityAndRate, dbo.EntBookingDetails.ItemName, dbo.EntBookingDetails.ItemTotalQuantity, dbo.EntBookingDetails.DeliveredQty, dbo.EntBookingDetails.ItemTotalQuantity - dbo.EntBookingDetails.DeliveredQty AS ItemsPending, dbo.EntBookings.BookingByCustomer, convert(varchar,dbo.EntBookings.BookingDate,106) as BookingDate,dbo.EntBookings.IsUrgent FROM dbo.EntBookingDetails INNER JOIN dbo.ItemMaster ON dbo.EntBookingDetails.ItemName = dbo.ItemMaster.ItemName INNER JOIN dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber
	WHERE dbo.EntBookings.BookingDate BETWEEN Convert(varchar,@BookDate1,106)  AND  Convert(varchar,@BookDate2,106)  AND dbo.EntBookings.BookingByCustomer =@CustomerId 
	ORDER BY EntBookingDetails.BookingNumber, EntBookingDetails.ISN
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_CustomerAllPrevoiusDue]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Gupta>
-- Create date: <27-Sep-2011>
-- Description:	<Customer>
-- =============================================
EXEC Sp_Sel_CustomerAllPrevoiusDue 'CUST4'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_CustomerAllPrevoiusDue]
	(		
		@CustCode varchar(10) = ''
	)
AS
BEGIN
--	declare @CustCode varchar(10)
--	set @CustCode='CUST4'
	CREATE TABLE #TmpBookReport (BookingNumber varchar(20), BookingDate smalldatetime, NetAmount float, PaymentMade float, DuePayment float)
	INSERT INTO #TmpBookReport (BookingNumber, BookingDate, NetAmount) SELECT BookingNumber, BookingDate, NetAmount FROM EntBookings WHERE BookingByCustomer = @CustCode AND (BookingDate<getdate()-1)

	DECLARE @BookingNumber varchar(20), @PaymentMade float, @DuePayment float,@TotalAmt float
	SET @BookingNumber = ''
	DECLARE CURPay  CURSOR
		FOR
			SELECT BookingNumber FROM #TmpBookReport
	FOR READ ONLY
	OPEN CURPay
		FETCH NEXT FROM CURPay INTO @BookingNumber
		WHILE @@FETCH_STATUS = 0
			BEGIN
				SELECT @PaymentMade = COALESCE(SUM(PaymentMade),0) FROM EntPayment WHERE BookingNumber = @BookingNumber
				UPDATE #TmpBookReport SET PaymentMade = @PaymentMade WHERE BookingNumber = @BookingNumber
				FETCH NEXT FROM CURPay INTO @BookingNumber
			END
	CLOSE CURPay
	DEALLOCATE CURPay
	UPDATE #TmpBookReport SET DuePayment = NetAmount - PaymentMade
	SET @TotalAmt =(SELECT SUM(DuePayment) FROM #TmpBookReport)
	if(@TotalAmt is null)
	begin
	insert into #TmpBookReport (DuePayment) values (0)
	end
	else
	begin
	UPDATE #TmpBookReport SET DuePayment=@TotalAmt
	end	
	SELECT top(1) * FROM #TmpBookReport
	DROP TABLE #TmpBookReport
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_QuantityandBooking]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport '10/19/2010','10/22/2010'
EXEC Sp_Sel_QuantityandBooking '1 Dec 2010','31 Dec 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_QuantityandBooking]
	(
		@BookDate1 datetime,
		@BookDate2 datetime
	)
AS
BEGIN
	SELECT EB.BookingId, EB.BookingNumber, convert(varchar,BookingDate,106) as BookingDate,Qty, NetAmount, SUM(COALESCE(PaymentMade,0)) as PaymentMade, SUM(COALESCE(DiscountOnPayment,0)) As DiscountOnPayment, NETAMOUNT-SUM(COALESCE(PaymentMade,0)+COALESCE(DiscountOnPayment,0))AS DuePayment ,EB.BookingStatus
	FROM EntBookings EB LEFT JOIN EntPayment EP ON EB.BookingNumber = EP.BookingNumber
	WHERE EB.BookingDate BETWEEN @BookDate1 AND DateAdd(DAY,1,@BookDate2)
	GROUP BY EB.BookingNumber, BookingDate, NetAmount,Qty,EB.BookingStatus,EB.BookingId
	order by EB.BookingId
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_PaymentReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Challan details>
-- =============================================
EXEC Sp_Sel_PaymentReport '1 JUL 2010', '1 NOV 2010' 
*/
CREATE PROCEDURE [dbo].[Sp_Sel_PaymentReport]
	(
		@PaymentDate1 datetime,
		@PaymentDate2 datetime
	)
AS
BEGIN
	CREATE TABLE #TmpTable (PaymentDate varchar(20), BookingNumber varchar(20), PaymentMade float , BalanceAmount float)
	INSERT INTO #TmpTable(PaymentDate, BookingNumber, PaymentMade, BalanceAmount)
		SELECT Convert(varchar,PaymentDate,106), EntBookings.BookingNumber, SUM(PaymentMade) As Received , Entbookings.NetAmount - SUM(PaymentMade) As Balance
		FROM EntBookings LEFT JOIN EntPayment ON EntPayment.BookingNumber = EntBookings.BookingNumber
		WHERE PaymentDate BETWEEN @PaymentDate1 AND @PaymentDate2 and PaymentMade<>0
		GROUP BY Convert(varchar,PaymentDate,106), EntBookings.BookingNumber,Entbookings.NetAmount
	SELECT * FROM #TmpTable
	DROP TABLE #TmpTable
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_NewDeliveryReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Gupta>
-- Create date: <5 Sep 2011>
-- Description:	<To select Records for delivery report>
-- =============================================
EXEC Sp_Sel_BookingReport '1 SEP 2010','2 sep 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_NewDeliveryReport]
	(
		@BookingDate1 datetime='',
		@BookingDate2 datetime='',
		@Flag varchar(50)='',
		@BookingNo VARCHAR(MAX)=''
	)
AS
BEGIN 
	IF(@Flag=1)
		BEGIN
			---- Select query 
			SELECT CONVERT(VARCHAR,BookingDate,106) AS BookingDate, BookingNo, SUM(SNo) AS QTY , SUM(DelQty) AS DeliveredQty , SUM(SNO) - SUM(DelQty) AS BalanceQty  FROM BarcodeTable WHERE BookingDate BETWEEN @BookingDate1 AND @BookingDate2 GROUP BY BookingDate, BookingNo

		END	
	ELSE IF(@Flag=2)
		BEGIN
			---- Hyperlink query 
			SELECT BookingNo,CONVERT(VARCHAR,ClothDeliveryDate,106) AS DeliveryDate, Item AS ItemName , SUM(DelQty) AS DeliveredQty FROM BarcodeTable where BookingNo=@BookingNo AND DeliveredQty<>0 GROUP BY item,DeliveredQty,ClothDeliveryDate,BookingNo
		END	
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_ItemWiseReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport '1 SEP 2010','2 sep 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_ItemWiseReport]
	(
		@BookDate1 datetime,
		@BookDate2 datetime,
		@ItemName varchar(max)
	)
AS
BEGIN
	SELECT convert(int,dbo.EntBookingDetails.BookingNumber) as BookingNumber, dbo.EntBookingDetails.ItemName, dbo.EntBookingDetails.ItemQuantityAndRate, convert(varchar, dbo.EntBookings.BookingDate,106) as BookingDate
	FROM   dbo.EntBookingDetails INNER JOIN dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber
	WHERE  (dbo.EntBookingDetails.ItemName = @ItemName) And EntBookings.BookingDate BETWEEN @BookDate1 AND @BookDate2
	GROUP BY dbo.EntBookingDetails.BookingNumber, dbo.EntBookingDetails.ItemName, dbo.EntBookingDetails.ItemQuantityAndRate,dbo.EntBookings.BookingDate
	order by dbo.EntBookingDetails.BookingNumber
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_EmployeeCheckedByReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport '10/19/2010','10/22/2010'
EXEC Sp_Sel_QuantityandBooking '1 Dec 2010','31 Dec 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_EmployeeCheckedByReport]
	(
		@BookingNo varchar(max)=''
	)
AS
BEGIN
	SELECT     dbo.BarcodeTable.BookingNo, dbo.EmployeeMaster.EmployeeName, dbo.BarcodeTable.Item
	FROM         dbo.BarcodeTable INNER JOIN
                      dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN
                      dbo.EmployeeMaster ON dbo.EntBookings.CheckedByEmployee = dbo.EmployeeMaster.EmployeeCode
	WHERE     (dbo.BarcodeTable.BookingNo = @BookingNo)
	order by dbo.BarcodeTable.RowIndex
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_DeliveryReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Challan details>
-- =============================================
EXEC Sp_Sel_DeliveryReport '1 JAN 2011', '1 SEP 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_DeliveryReport]
	(
		@BookingDate1 datetime,
		@BookingDate2 datetime,
		@Customerid varchar(max)='',
		@Flag varchar(max)='',
		@BookingNumber varchar(max)=''
	)
AS
BEGIN
	IF(@Flag=1)
	BEGIN
	CREATE TABLE #TmpTable (BookingDate varchar(20), BookingNumber varchar(20), DeliveryDate varchar(20),PaymentMade float,NetAmount float,Balance float)
	INSERT INTO #TmpTable(BookingDate, BookingNumber, DeliveryDate,PaymentMade,NetAmount,Balance)
		SELECT    CONVERT(VARCHAR,dbo.EntBookings.BookingDate,106) AS BookingDate, dbo.EntBookings.BookingNumber, CONVERT(varchar, dbo.EntPayment.PaymentDate, 106) AS PaymentDate, 
                      SUM(dbo.EntPayment.PaymentMade) AS Received, dbo.EntBookings.NetAmount AS NetAmount,round(dbo.EntBookings.NetAmount - SUM(dbo.EntPayment.PaymentMade),2)
                      AS Balance
		FROM         dbo.EntBookings INNER JOIN
                      dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode LEFT OUTER JOIN
                      dbo.EntPayment ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber
		WHERE (PaymentDate BETWEEN @BookingDate1 AND @BookingDate2 ) And EntBookings.BookingNumber=@BookingNumber AND EntPayment.PaymentMade<>0
		GROUP BY CONVERT(varchar, dbo.EntPayment.PaymentDate, 106), dbo.EntBookings.BookingNumber, dbo.EntBookings.NetAmount, dbo.EntBookings.BookingDate
	SELECT * FROM #TmpTable
	DROP TABLE #TmpTable
	END
	ELSE IF(@Flag=2)
	BEGIN
	CREATE TABLE #TmpTable1 (BookingDate varchar(20), BookingNumber varchar(20), DeliveryDate varchar(20),PaymentMade float,NetAmount float,Balance float)
	INSERT INTO #TmpTable1(BookingDate, BookingNumber, DeliveryDate,PaymentMade,NetAmount,Balance)
		SELECT      CONVERT(VARCHAR,dbo.EntBookings.BookingDate,106) AS BookingDate, dbo.EntBookings.BookingNumber, CONVERT(varchar, dbo.EntPayment.PaymentDate, 106) AS PaymentDate, 
                      SUM(dbo.EntPayment.PaymentMade) AS Received, dbo.EntBookings.NetAmount AS NetAmount,round(dbo.EntBookings.NetAmount - SUM(dbo.EntPayment.PaymentMade),2)
                      AS Balance
		FROM         dbo.EntBookings INNER JOIN
                      dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode LEFT OUTER JOIN
                      dbo.EntPayment ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber

		WHERE (PaymentDate BETWEEN @BookingDate1 AND @BookingDate2 ) AND EntPayment.PaymentMade<>0
		GROUP BY CONVERT(varchar, dbo.EntPayment.PaymentDate, 106), dbo.EntBookings.BookingNumber, dbo.EntBookings.NetAmount, dbo.EntBookings.BookingDate
	SELECT * FROM #TmpTable1
	DROP TABLE #TmpTable1
	END
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_DeliveryQuantityandBooking]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport '10/19/2010','10/22/2010'
EXEC Sp_Sel_QuantityandBooking '1 Dec 2010','31 Dec 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_DeliveryQuantityandBooking]
	(
		@BookDate1 datetime,
		@BookDate2 datetime
	)
AS
BEGIN
	SELECT EB.BookingNumber, convert(varchar,BookingDeliveryDate,106) as BookingDeliveryDate,Qty, NetAmount, SUM(COALESCE(PaymentMade,0)) as PaymentMade, SUM(COALESCE(DiscountOnPayment,0)) As DiscountOnPayment, NETAMOUNT-SUM(COALESCE(PaymentMade,0)+COALESCE(DiscountOnPayment,0))AS DuePayment ,EB.BookingStatus
	FROM EntBookings EB LEFT JOIN EntPayment EP ON EB.BookingNumber = EP.BookingNumber
	WHERE EB.BookingDeliveryDate BETWEEN @BookDate1 AND @BookDate2
	GROUP BY EB.BookingNumber, BookingDeliveryDate, NetAmount,Qty,EB.BookingStatus
	order by EB.BookingNumber
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_DefaultUrgentShow]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
*/
CREATE PROCEDURE [dbo].[Sp_Sel_DefaultUrgentShow]
	(
		@BookDate1 datetime,
		@BookDate2 datetime	
	)
AS
BEGIN
	SELECT EB.BookingNumber, convert(varchar,BookingDate,106) as BookingDate,EB.BookingDeliveryTime ,Qty, NetAmount, SUM(COALESCE(PaymentMade,0)) as PaymentMade, SUM(COALESCE(DiscountOnPayment,0)) As DiscountOnPayment, NETAMOUNT-SUM(COALESCE(PaymentMade,0)+COALESCE(DiscountOnPayment,0))AS DuePayment ,EB.BookingStatus,convert(varchar,EB.BookingDeliveryDate,106) as BookingDeliveryDate,convert(varchar,EP.PaymentDate,106) as PaymentDate
	FROM EntBookings EB LEFT JOIN EntPayment EP ON EB.BookingNumber = EP.BookingNumber
	WHERE EB.BookingDeliveryDate BETWEEN @BookDate1 AND @BookDate2 And (EB.IsUrgent = 'True')
	GROUP BY EB.BookingNumber, BookingDate, NetAmount,Qty,EB.BookingStatus,BookingDeliveryDate,EP.PaymentDate,EB.BookingDeliveryTime
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_DefaultHomeDeliveryShow]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_DefaultUrgentShow '1/31/2011',''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_DefaultHomeDeliveryShow]
	(
		@BookDate1 datetime,
		@BookDate2 datetime	
	)
AS
BEGIN
	SELECT EB.BookingNumber, convert(varchar,BookingDate,106) as BookingDate ,Qty, NetAmount, SUM(COALESCE(PaymentMade,0)) as PaymentMade, SUM(COALESCE(DiscountOnPayment,0)) As DiscountOnPayment, NETAMOUNT-SUM(COALESCE(PaymentMade,0)+COALESCE(DiscountOnPayment,0))AS DuePayment ,EB.BookingStatus,convert(varchar,EB.BookingDeliveryDate,106) as BookingDeliveryDate,convert(varchar,EP.PaymentDate,106) as PaymentDate
	FROM EntBookings EB LEFT JOIN EntPayment EP ON EB.BookingNumber = EP.BookingNumber
	WHERE EB.BookingDeliveryDate BETWEEN @BookDate1 AND @BookDate2 And (EB.HomeDelivery = 'True')
	GROUP BY EB.BookingNumber, BookingDate, NetAmount,Qty,EB.BookingStatus,BookingDeliveryDate,EP.PaymentDate
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_ChallanReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Challan details>
-- =============================================
EXEC Sp_Sel_ChallanReport '1 SEP 2010', '30 SEP 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_ChallanReport]
	(
		@ChallanDate1 datetime,
		@ChallanDate2 datetime
	)
AS
BEGIN
	CREATE TABLE #TmpChallanReport(BookingDate varchar(30), ChallanNumber int, BookingNumber int, ItemName varchar(50), ItemProcessType varchar(30), ItemExtraProcessType1 varchar(30), ItemExtraProcessType2 varchar(30), ItemsSent int, ItemsReceived int)
	INSERT INTO #TmpChallanReport(BookingDate, ChallanNumber, BookingNumber, ItemName, ItemProcessType, ItemExtraProcessType1, ItemExtraProcessType2, ItemsSent, ItemsReceived)
	SELECT BookingDate, ChallanNumber, EntChallan.BookingNumber, EntBookingDetails.ItemName, CASE WHEN ItemProcessType = 'NONE' THEN '' ELSE ItemProcessType END As ItemProcessType, CASE WHEN ItemExtraProcessType1 = 'NONE' THEN '' WHEN ItemProcessType = 'NONE' THEN 'O' + ItemExtraProcessType1 ELSE ItemExtraProcessType1 END As ItemExtraProcessType1, CASE WHEN ItemExtraProcessType2 = 'NONE' THEN '' WHEN ItemProcessType = 'NONE' THEN 'O' + ItemExtraProcessType2 ELSE ItemExtraProcessType2 END As ItemExtraProcessType2, SUM(ItemTotalQuantitySent) AS ItemTotalQuantitySent, SUM(ItemsReceivedFromVendor) AS ItemsReceivedFromVendor
	FROM EntBookings EB INNER JOIN EntChallan ON EB.BookingNumber = EntChallan.BookingNumber INNER JOIN EntBookingDetails ON EntChallan.BookingNumber = EntBookingDetails.BookingNumber AND EntChallan.ItemSno= EntBookingDetails.ISN
	WHERE ChallanDate BETWEEN @ChallanDate1 AND @ChallanDate2
	Group By BookingDate, ChallanNumber, EntChallan.BookingNumber, EntBookingDetails.ItemName, ItemProcessType, ItemExtraProcessType1, ItemExtraProcessType2

	INSERT INTO #TmpChallanReport(BookingDate) SELECT ''
	INSERT INTO #TmpChallanReport(BookingDate, BookingNumber) SELECT ItemProcessType, SUM(ItemsSent) FROM #TmpChallanReport WHERE ItemProcessType<>'' GROUP BY ItemProcessType
	INSERT INTO #TmpChallanReport(BookingDate, BookingNumber) SELECT ItemExtraProcessType1, SUM(ItemsSent) FROM #TmpChallanReport WHERE ItemExtraProcessType1<>'' GROUP BY ItemExtraProcessType1
	INSERT INTO #TmpChallanReport(BookingDate, BookingNumber) SELECT ItemExtraProcessType2, SUM(ItemsSent) FROM #TmpChallanReport WHERE ItemExtraProcessType2<>'' GROUP BY ItemExtraProcessType2
	INSERT INTO #TmpChallanReport(BookingDate, BookingNumber) SELECT 'Total', SUM(ItemsSent) FROM #TmpChallanReport
	SELECT * FROM #TmpChallanReport

	DROP TABLE #TmpChallanReport

END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_BookingReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport '1 SEP 2010','2 sep 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_BookingReport]
	(
		@BookDate1 datetime,
		@BookDate2 datetime
	)
AS
BEGIN
	SELECT EB.BookingId, EB.BookingNumber, convert(varchar,BookingDate,106) as BookingDate, NetAmount, SUM(COALESCE(PaymentMade,0)) as PaymentMade, SUM(COALESCE(DiscountOnPayment,0)) As DiscountOnPayment, NETAMOUNT-SUM(COALESCE(PaymentMade,0)+COALESCE(DiscountOnPayment,0))AS DuePayment, BookingAcceptedByUserId, BookingStatus
	FROM EntBookings EB LEFT JOIN EntPayment EP ON EB.BookingNumber = EP.BookingNumber
	WHERE EB.BookingDate BETWEEN @BookDate1 AND @BookDate2
	GROUP BY EB.BookingNumber, BookingDate, NetAmount,BookingAcceptedByUserId, BookingStatus,EB.BookingId
	order by  EB.BookingId asc
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Sel_BookingRecordsForBulkModification]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <25 Sep 2010>
-- Description:	<To select records for booking Bulk modification>
-- =============================================
EXEC SP_Sel_BookingRecordsForBulkModification '1 sep 2010', '1 Oct 2010'
*/
CREATE PROCEDURE [dbo].[SP_Sel_BookingRecordsForBulkModification]
	(
		@BookingDate1 datetime='',
		@BookingDate2 datetime=''
	)
AS
BEGIN
--	SELECT * From EntBookings EB INNER JOIN EntBookingDetails EBD ON EB.BookingNumber = EBD.BookingNumber
--	WHERE BookingDate BETWEEN @BookingDate1 AND DATEADD(DAY,1,@BookingDate2)
--	
	SELECT EB.BookingNumber, ISN, ItemName, ItemRemark, EBD.ItemTotalQuantity, ItemProcessType, ItemQuantityAndRate, ItemExtraProcessType1, ItemExtraProcessRate1, ItemExtraProcessType2, ItemExtraProcessRate2
	From EntBookings EB INNER JOIN EntBookingDetails EBD ON EB.BookingNumber = EBD.BookingNumber
	WHERE BookingDate BETWEEN @BookingDate1 AND DATEADD(DAY,1,@BookingDate2) AND EB.BookingStatus<>5

	SELECT EB.BookingNumber, 0 As TotalCost 
	From EntBookings EB 
	WHERE BookingDate BETWEEN @BookingDate1 AND DATEADD(DAY,1,@BookingDate2) AND EB.BookingStatus<>5

END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_BookingDetailsReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Booking Details for>
-- =============================================
EXEC Sp_Sel_BookingDetailsForReceipt '2'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_BookingDetailsReport]
	(
		@BookingNumber varchar(10)=''
	)
AS
BEGIN
SELECT     dbo.EntBookings.BookingNumber,dbo.EntBookings.BarCode,dbo.EntBookings.Discount,dbo.EntBookings.BookingRemarks, dbo.EntBookings.IsUrgent, dbo.EntBookings.BookingAcceptedByUserId, dbo.EntBookingDetails.ItemTotalQuantity, 
                      dbo.EntBookingDetails.ItemSubTotal, dbo.EntBookingDetails.ItemExtraProcessType1, dbo.EntBookingDetails.ItemExtraProcessRate1, 
                      dbo.EntBookingDetails.ItemColor, dbo.EntBookingDetails.ItemQuantityAndRate,dbo.EntBookingDetails.ItemRemark, Convert(varchar(20),dbo.EntBookings.BookingDate,106) as BookingDate, Convert(varchar(20),dbo.EntBookings.BookingDeliveryDate,106) as BookingDeliveryDate, 
                      dbo.EntBookingDetails.ItemProcessType, dbo.EntBookings.BookingDeliveryTime, dbo.EntBookings.TotalCost, dbo.EntBookings.BookingByCustomer, 
                      dbo.CustomerMaster.CustomerCode, dbo.CustomerMaster.CustomerName, dbo.CustomerMaster.CustomerAddress, 
                      dbo.CustomerMaster.CustomerPhone, dbo.CustomerMaster.CustomerMobile, dbo.EntPayment.PaymentMade, dbo.EntBookings.HomeDelivery, 
                      dbo.EntBookingDetails.ItemName, dbo.ProcessMaster.ProcessName, dbo.PriorityMaster.Priority
FROM         dbo.EntBookings LEFT OUTER JOIN
                      dbo.EntBookingDetails ON dbo.EntBookings.BookingNumber = dbo.EntBookingDetails.BookingNumber Left outer JOIN
                      dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber Left outer JOIN
                      dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode Left outer JOIN
                      dbo.ProcessMaster ON dbo.EntBookingDetails.ItemProcessType = dbo.ProcessMaster.ProcessCode Left outer JOIN
                      dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID
where dbo.EntBookings.BookingNumber=@BookingNumber and  dbo.EntPayment.PaymentMade>=0
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_BookingDetailsForReceipt1]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Booking Details for>
-- =============================================
EXEC Sp_Sel_BookingDetailsForReceipt '9'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_BookingDetailsForReceipt1]
	(
		@BookingNumber varchar(10)=''
	)
AS
BEGIN
	DECLARE @SQL varchar(max), @PaymentMade float, @DiscountGiven float, @CustCode varchar(10)
	DECLARE @TotNetAmount float, @TotPaymentMade float
	SET @CustCode = ''
	SELECT @CustCode= BookingByCustomer FROM EntBookings WHERE BookingNumber = @BookingNumber
	CREATE TABLE #TmpDeliveryInfo (BookingNumber varchar(10), BookingDate varchar(20), DeliveryDate varchar(20), DeliveryTime varchar(20), CustomerCode varchar(10), CustomerName varchar(100), CustomerAddress varchar(100), CustomerPhone varchar(50), CustomerPriority varchar(50), BookingRemarks varchar(50), BookingAmount float, Discount float, NetAmount float, PaymentMade float, DuePayment float, DiscountOnPayment float, HomeDelivery VARCHAR(10),Barcode VARCHAR(100),Urgent VARCHAR (15), PrevDue float,DiscountAmt float)
	INSERT INTO #TmpDeliveryInfo (BookingNumber , CustomerCode , CustomerName, CustomerAddress, CustomerPhone, CustomerPriority, BookingDate, DeliveryDate, DeliveryTime, BookingAmount, Discount, NetAmount, HomeDelivery,Barcode,Urgent, BookingRemarks,DiscountAmt)
	  SELECT BookingNumber, BookingByCustomer, CustomerSalutation + ' '  + CustomerName As CustomerName, CustomerAddress, CustomerMobile, Priority, Convert(varchar,BookingDate,106) As BookingDate, Convert(varchar, BookingDeliveryDate, 106) As BookingDeliveryDate, BookingDeliveryTime, TotalCost, Discount, NetAmount,CASE WHEN HomeDelivery = '0' THEN '' ELSE 'H' END,Barcode ,CASE WHEN Isurgent = '0' THEN '' ELSE 'U' END  , Convert(varchar(50),BookingRemarks), DiscountAmt
		FROM EntBookings LEFT JOIN CustomerMaster ON EntBookings.BookingByCustomer = CustomerMaster.CustomerCode LEFT JOIN PriorityMaster ON CustomerMaster.CustomerPriority = PriorityMaster.PriorityID
		WHERE BookingNumber = @BookingNumber
	
	SELECT @PaymentMade = COALESCE(SUM(PaymentMade),0), @DiscountGiven = COALESCE(SUM(DiscountOnPayment),0) FROM EntPayment WHERE BookingNumber = @BookingNumber
	UPDATE #TmpDeliveryInfo SET PaymentMade = @PaymentMade
	UPDATE #TmpDeliveryInfo SET DuePayment= COALESCE(NetAmount - PaymentMade,0), DiscountOnPayment = @DiscountGiven
	
	SELECT @TotNetAmount = SUM(NetAmount) FROM EntBookings WHERE BookingByCustomer = @CustCode AND Convert(int, BookingNumber)<Convert(int, @BookingNumber)
	SELECT @TotPaymentMade = SUM(PaymentMade) FROM EntPayment WHERE BookingNumber IN (SELECT BookingNumber FROM EntBookings WHERE BookingByCustomer = @CustCode  AND Convert(int, BookingNumber)<Convert(int, @BookingNumber))
	UPDATE #TmpDeliveryInfo SET PrevDue = COALESCE(@TotNetAmount- @TotPaymentMade,0)
	--UPDATE #TmpDeliveryInfo SET PrevDue = (PrevDue - DuePayment)
	
	SELECT * FROM #TmpDeliveryInfo
	SELECT BookingNumber, ISN, EntBookingDetails.ItemName, ItemTotalQuantity, COALESCE(ItemTotalQuantity,1)* COALESCE(NumberOfSubItems,1) As SubPieces, ItemProcessType, ItemQuantityAndRate, ItemExtraProcessType1, ItemExtraProcessRate1, ItemExtraProcessType2, ItemExtraProcessRate2, ItemSubTotal, ItemStatus, ItemRemark, DeliveredQty,ItemColor,STPAmt,STEP1Amt,STEP2Amt  FROM EntBookingDetails LEFT JOIN ItemMaster ON EntBookingDetails.ItemName = ItemMaster.ItemName WHERE BookingNumber = @BookingNumber
	SELECT Convert(varchar,PaymentDate,100) As 'Paid On', PaymentMade As 'Payment' FROM EntPayment WHERE BookingNumber = @BookingNumber and PaymentMade<>0 AND PaymentDate <getdate() -1
	SELECT SUM(PaymentMade) As 'Payment' FROM EntPayment WHERE BookingNumber = @BookingNumber and PaymentMade<>0 AND convert(varchar,PaymentDate,6)= convert(varchar, getdate(),6)

	
	DROP TABLE #TmpDeliveryInfo
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_BookingDetailsForReceipt]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Booking Details for>
-- =============================================
EXEC Sp_Sel_BookingDetailsForReceipt '9'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_BookingDetailsForReceipt]
	(
		@BookingNumber varchar(10)=''
	)
AS
BEGIN
	DECLARE @SQL varchar(max), @PaymentMade float, @DiscountGiven float, @CustCode varchar(10)
	DECLARE @TotNetAmount float, @TotPaymentMade float
	SET @CustCode = ''
	SELECT @CustCode= BookingByCustomer FROM EntBookings WHERE BookingNumber = @BookingNumber
	CREATE TABLE #TmpDeliveryInfo (BookingNumber varchar(10), BookingDate varchar(20), DeliveryDate varchar(20), DeliveryTime varchar(20), CustomerCode varchar(10), CustomerName varchar(100), CustomerAddress varchar(100), CustomerPhone varchar(50), CustomerPriority varchar(50), BookingRemarks varchar(50), BookingAmount float, Discount float, NetAmount float, PaymentMade float, DuePayment float, DiscountOnPayment float, HomeDelivery VARCHAR(10),Barcode VARCHAR(100),Urgent VARCHAR (15), PrevDue float,DiscountAmt float)
	INSERT INTO #TmpDeliveryInfo (BookingNumber , CustomerCode , CustomerName, CustomerAddress, CustomerPhone, CustomerPriority, BookingDate, DeliveryDate, DeliveryTime, BookingAmount, Discount, NetAmount, HomeDelivery,Barcode,Urgent, BookingRemarks,DiscountAmt)
	  SELECT BookingNumber, BookingByCustomer, CustomerSalutation + ' '  + CustomerName As CustomerName, CustomerAddress, CustomerMobile, Priority, Convert(varchar,BookingDate,106) As BookingDate, Convert(varchar, BookingDeliveryDate, 106) As BookingDeliveryDate, BookingDeliveryTime, TotalCost, Discount, NetAmount,CASE WHEN HomeDelivery = '0' THEN '' ELSE 'H' END,Barcode ,CASE WHEN Isurgent = '0' THEN '' ELSE 'U' END  , Convert(varchar(50),BookingRemarks), DiscountAmt
		FROM EntBookings LEFT JOIN CustomerMaster ON EntBookings.BookingByCustomer = CustomerMaster.CustomerCode LEFT JOIN PriorityMaster ON CustomerMaster.CustomerPriority = PriorityMaster.PriorityID
		WHERE BookingNumber = @BookingNumber
	
	SELECT @PaymentMade = COALESCE(SUM(PaymentMade),0), @DiscountGiven = COALESCE(SUM(DiscountOnPayment),0) FROM EntPayment WHERE BookingNumber = @BookingNumber
	UPDATE #TmpDeliveryInfo SET PaymentMade = @PaymentMade
	UPDATE #TmpDeliveryInfo SET DuePayment= COALESCE(NetAmount - PaymentMade,0), DiscountOnPayment = @DiscountGiven
	
	SELECT @TotNetAmount = SUM(NetAmount) FROM EntBookings WHERE BookingByCustomer = @CustCode AND Convert(int, BookingNumber)<Convert(int, @BookingNumber)
	SELECT @TotPaymentMade = SUM(PaymentMade) FROM EntPayment WHERE BookingNumber IN (SELECT BookingNumber FROM EntBookings WHERE BookingByCustomer = @CustCode  AND Convert(int, BookingNumber)<Convert(int, @BookingNumber))
	UPDATE #TmpDeliveryInfo SET PrevDue = COALESCE(@TotNetAmount- @TotPaymentMade,0)
	--UPDATE #TmpDeliveryInfo SET PrevDue = (PrevDue - DuePayment)
	
	SELECT * FROM #TmpDeliveryInfo
	SELECT BookingNumber, ISN, EntBookingDetails.ItemName, ItemTotalQuantity, COALESCE(ItemTotalQuantity,1)* COALESCE(NumberOfSubItems,1) As SubPieces, ItemProcessType, ItemQuantityAndRate, ItemExtraProcessType1, ItemExtraProcessRate1, ItemExtraProcessType2, ItemExtraProcessRate2, ItemSubTotal, ItemStatus, ItemRemark, DeliveredQty,ItemColor,STPAmt,STEP1Amt,STEP2Amt  FROM EntBookingDetails LEFT JOIN ItemMaster ON EntBookingDetails.ItemName = ItemMaster.ItemName WHERE BookingNumber = @BookingNumber
	SELECT Convert(varchar,PaymentDate,100) As 'Paid On', PaymentMade As 'Payment' FROM EntPayment WHERE BookingNumber = @BookingNumber and PaymentMade<>0
	
	DROP TABLE #TmpDeliveryInfo
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_BookingDetailsForEditing]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Booking Details for>
-- =============================================
EXEC Sp_Sel_BookingDetailsForEditing '21452'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_BookingDetailsForEditing]
	(
		@BookingNumber varchar(10)=''
	)
AS
BEGIN
	DECLARE @SQL varchar(max), @PaymentMade float, @DiscountGiven float, @CustCode varchar(10)
	DECLARE @TotNetAmount float, @TotPaymentMade float
	SET @CustCode = ''
	SELECT @CustCode= BookingByCustomer FROM EntBookings WHERE BookingNumber = @BookingNumber
	CREATE TABLE #TmpDeliveryInfo (BookingNumber varchar(10), BookingDate varchar(20), DeliveryDate varchar(20), CustomerCode varchar(10), CustomerName varchar(100), CustomerAddress varchar(100), CustomerPhone varchar(50), CustomerPriority varchar(50), BookingAmount float, Discount float, NetAmount float, BookingRemarks varchar(200), PaymentMade float, DuePayment float, DiscountOnPayment float, ISUrgent bit, PrevDue float,HomeDelivery bit,CheckedByEmployee varchar(50))
	INSERT INTO #TmpDeliveryInfo (BookingNumber , CustomerCode , CustomerName, CustomerAddress, CustomerPhone, CustomerPriority, BookingDate, DeliveryDate, BookingAmount, Discount, NetAmount, BookingRemarks, ISUrgent,HomeDelivery,CheckedByEmployee)
	 SELECT BookingNumber, BookingByCustomer, CustomerSalutation + ' '  + CustomerName As CustomerName, CustomerAddress, CustomerPhone, Priority, Convert(varchar,BookingDate,103) As BookingDate, Convert(varchar, BookingDeliveryDate, 103) As BookingDeliveryDate, TotalCost, Discount, NetAmount, BookingRemarks, ISUrgent ,HomeDelivery,CheckedByEmployee
		FROM EntBookings LEFT JOIN CustomerMaster ON EntBookings.BookingByCustomer = CustomerMaster.CustomerCode LEFT JOIN PriorityMaster ON CustomerMaster.CustomerPriority = PriorityMaster.PriorityID
		WHERE BookingNumber = @BookingNumber
	
	SELECT @PaymentMade = COALESCE(SUM(PaymentMade),0), @DiscountGiven = COALESCE(SUM(DiscountOnPayment),0) FROM EntPayment WHERE BookingNumber = @BookingNumber
	UPDATE #TmpDeliveryInfo SET PaymentMade = @PaymentMade
	UPDATE #TmpDeliveryInfo SET DuePayment= COALESCE(NetAmount - PaymentMade,0), DiscountOnPayment = @DiscountGiven
	
	SELECT @TotNetAmount = SUM(NetAmount) FROM EntBookings WHERE BookingByCustomer = @CustCode AND Convert(int, BookingNumber)<Convert(int, @BookingNumber)
	SELECT @TotPaymentMade = SUM(PaymentMade) FROM EntPayment WHERE BookingNumber IN (SELECT BookingNumber FROM EntBookings WHERE BookingByCustomer = @CustCode  AND Convert(int, BookingNumber)<Convert(int, @BookingNumber))
	UPDATE #TmpDeliveryInfo SET PrevDue = COALESCE(@TotNetAmount- @TotPaymentMade,0)
	--UPDATE #TmpDeliveryInfo SET PrevDue = (PrevDue - DuePayment)
	
	SELECT * FROM #TmpDeliveryInfo
	SELECT * FROM EntBookingDetails WHERE BookingNumber = @BookingNumber
	SELECT Convert(varchar,PaymentDate,100) As 'Paid On', PaymentMade As 'Payment' FROM EntPayment WHERE BookingNumber = @BookingNumber
	
	DROP TABLE #TmpDeliveryInfo
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_BookingDetailsForDelivery]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Booking Details for>
-- =============================================
EXEC Sp_Sel_BookingDetailsForDelivery '33'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_BookingDetailsForDelivery]
	(
		@BookingNumber varchar(15)='',
		@RowIndex int=''
	)
AS
BEGIN
	DECLARE @SQL varchar(max), @PaymentMade float, @DiscountGiven float,@ServiceTax float,@DeliveryRemarks varchar(max)
	CREATE TABLE #TmpDeliveryInfo (BookingNumber varchar(10), BookingDate varchar(20), DeliveryDate varchar(20), CustomerCode varchar(10), CustomerName varchar(100), BookingAmount float, Discount float, NetAmount float, PaymentMade float, DuePayment float, DiscountOnPayment float,ServiceTax float,DeliveryRemarks varchar(max),DiscountAmt float)
	INSERT INTO #TmpDeliveryInfo (BookingNumber , CustomerCode , CustomerName , BookingDate, DeliveryDate, BookingAmount, Discount, NetAmount,DiscountAmt)
	 SELECT BookingNumber, BookingByCustomer, CustomerSalutation + ' '  + CustomerName As CustomerName, Convert(varchar,BookingDate,106) As BookingDate, Convert(varchar, BookingDeliveryDate, 106) + ' , ' + BookingDeliveryTime As BookingDeliveryDate, TotalCost, Discount, NetAmount ,ROUND(DiscountAmt,2)
		FROM EntBookings LEFT JOIN CustomerMaster ON EntBookings.BookingByCustomer = CustomerMaster.CustomerCode
		WHERE BookingNumber = @BookingNumber 
	
	SELECT @PaymentMade = COALESCE(SUM(PaymentMade),0), @DiscountGiven = COALESCE(SUM(DiscountOnPayment),0) FROM EntPayment WHERE BookingNumber = @BookingNumber
	set @ServiceTax=round((SELECT SUM(STPAmt) + SUM(Step1Amt) + SUM(Step2Amt) AS Servicetax FROM EntBookingDetails WHERE BookingNumber=@BookingNumber),2)
	set @DeliveryRemarks=(SELECT top(1) DeliveryMsg FROM dbo.EntPayment WHERE BookingNumber=@BookingNumber)
	UPDATE #TmpDeliveryInfo SET PaymentMade = @PaymentMade + @DiscountGiven
	UPDATE #TmpDeliveryInfo SET DuePayment= COALESCE(NetAmount - PaymentMade,0), DiscountOnPayment = @DiscountGiven
	UPDATE #TmpDeliveryInfo SET ServiceTax=@ServiceTax,DeliveryRemarks=@DeliveryRemarks	
	--Table(0)
	SELECT * FROM #TmpDeliveryInfo
	--Table(1)
	SELECT     dbo.BarcodeTable.BookingNo, dbo.BarcodeTable.RowIndex AS ISN, dbo.BarcodeTable.Item AS ItemName, 
                      CASE WHEN BarcodeTable.ItemExtraprocessType = '0' THEN 'None' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraProcessType1, dbo.BarcodeTable.SNo AS ItemTotalQuantity, 
                      dbo.BarcodeTable.Process AS ItemProcessType, dbo.BarcodeTable.DeliveredQty,dbo.BarcodeTable.AllottedDrawl,dbo.BarcodeTable.DeliverItemStaus,convert(varchar,dbo.BarcodeTable.ClothDeliveryDate,106) as Date
	FROM         dbo.EntChallan INNER JOIN
                      dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex WHERE BookingNumber = @BookingNumber AND dbo.BarcodeTable.StatusId<>'1' AND dbo.BarcodeTable.StatusId<>'2'
	--Table(2)
	SELECT Convert(varchar,PaymentDate,106) As 'Paid On', PaymentMade As 'Payment',PaymentType AS 'Payment Type',PaymentRemarks AS 'Payment Details' FROM EntPayment WHERE BookingNumber = @BookingNumber AND PaymentMade<>0

	-- Table(3)

	SELECT     dbo.BarcodeTable.BookingNo, dbo.BarcodeTable.RowIndex AS ISN, dbo.BarcodeTable.Item AS ItemName, 
                      CASE WHEN BarcodeTable.ItemExtraprocessType = '0' THEN 'None' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraProcessType1, dbo.BarcodeTable.SNo AS ItemTotalQuantity, 
                      dbo.BarcodeTable.Process AS ItemProcessType, dbo.BarcodeTable.DeliveredQty,dbo.BarcodeTable.AllottedDrawl,dbo.BarcodeTable.DeliverItemStaus,convert(varchar,dbo.BarcodeTable.ClothDeliveryDate,106) as Date 
	FROM         dbo.EntChallan INNER JOIN
                      dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex WHERE BookingNumber = @BookingNumber AND dbo.BarcodeTable.RowIndex=@RowIndex AND dbo.BarcodeTable.StatusId<>'1' AND dbo.BarcodeTable.StatusId<>'2'

	DROP TABLE #TmpDeliveryInfo
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_AreaWiseClothBookingReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport '1 SEP 2010','2 sep 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_AreaWiseClothBookingReport]
	(
		@BookDate1 datetime,
		@BookDate2 datetime,
		@AreaType varchar(max)
	)
AS
BEGIN
	SELECT dbo.BarcodeTable.Item, SUM(dbo.BarcodeTable.SNo) AS Qty
    FROM   dbo.BarcodeTable INNER JOIN dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
	WHERE (SOUNDEX(dbo.CustomerMaster.AreaLocation) = SOUNDEX(@AreaType)) AND (dbo.BarcodeTable.BookingDate BETWEEN @BookDate1 AND @BookDate2)
	GROUP BY dbo.BarcodeTable.SNo, dbo.BarcodeTable.Item	
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_AreaWiseBookingReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport '1 SEP 2010','2 sep 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_AreaWiseBookingReport]
	(
		@BookDate1 datetime,
		@BookDate2 datetime,
		@AreaType varchar(max)
	)
AS
BEGIN
	SELECT EB.BookingNumber, convert(varchar, EB.BookingDate,106) as BookingDate, EB.NetAmount, SUM(COALESCE (EP.PaymentMade, 0)) AS PaymentMade,
    SUM(COALESCE (EP.DiscountOnPayment, 0)) AS DiscountOnPayment, EB.NetAmount - SUM(COALESCE (EP.PaymentMade, 0) 
    + COALESCE (EP.DiscountOnPayment, 0)) AS DuePayment, EB.BookingAcceptedByUserId, EB.BookingStatus, dbo.CustomerMaster.AreaLocation
	FROM dbo.EntBookings AS EB INNER JOIN
    dbo.CustomerMaster ON EB.BookingByCustomer = dbo.CustomerMaster.CustomerCode LEFT OUTER JOIN
    dbo.EntPayment AS EP ON EB.BookingNumber = EP.BookingNumber
	WHERE (SOUNDEX(dbo.CustomerMaster.AreaLocation) = SOUNDEX(@AreaType)) AND (EB.BookingDate BETWEEN @BookDate1 AND @BookDate2)
	GROUP BY EB.BookingNumber, EB.BookingDate, EB.NetAmount, EB.BookingAcceptedByUserId, EB.BookingStatus, dbo.CustomerMaster.AreaLocation
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Sel_AllDeliveryReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Challan details>
-- =============================================
EXEC Sp_Sel_DeliveryReport '1 JUL 2010', '1 SEP 2010'
*/
CREATE PROCEDURE [dbo].[Sp_Sel_AllDeliveryReport]
	(
		@BookingDate1 datetime,
		@BookingDate2 datetime
	)
AS
BEGIN
	CREATE TABLE #TmpTable (BookingDate varchar(20), BookingNumber varchar(20), DeliveryDate varchar(20))
	INSERT INTO #TmpTable(BookingDate, BookingNumber, DeliveryDate)
		SELECT Convert(varchar,BookingDate,106), EntBookings.BookingNumber, Convert(varchar,Max(PaymentDate),106)
		 FROM EntBookings LEFT JOIN EntPayment ON EntPayment.BookingNumber = EntBookings.BookingNumber
			WHERE BookingDate BETWEEN @BookingDate1 AND @BookingDate2
		GROUP BY Convert(varchar,BookingDate,106), EntBookings.BookingNumber
	SELECT * FROM #TmpTable
	DROP TABLE #TmpTable
END
GO
/****** Object:  StoredProcedure [dbo].[sp_rpt_barcodprint]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_rpt_barcodprint]
	@Flag int=0,
	@BookingNo VARCHAR(MAX)='',
	@RowIndex int=null
	
AS
BEGIN
	IF(@Flag=1)
	 BEGIN
		SELECT BookingNumber as BookingNo FROM EntBookings ORDER BY BookingID asc
	 END
	IF(@Flag=2)
	 BEGIN
		SELECT distinct RowIndex from BarcodeTable WHERE BookingNo=@BookingNo
	 END
	IF(@Flag=3)
	 BEGIN
			SELECT colour, convert(varchar, BookingDate,106) as BookingDate,CurrentTime,convert(varchar, DueDate,106) as DueDate ,Item,Barcode,Process,BookingNo,RowIndex,CustomerName from BarcodeTable bt left outer join dbo.CustomerMaster cm on bt.BookingByCustomer=cm.CustomerCode


			where BookingNo=@BookingNo and ( @RowIndex is null or RowIndex=@RowIndex)

	 END
IF(@Flag=4)
	 BEGIN			
		SELECT dbo.BarcodeTable.Colour, CONVERT(varchar, dbo.BarcodeTable.BookingDate, 106) AS BookingDate, dbo.BarcodeTable.CurrentTime, 
			   CONVERT(varchar, dbo.BarcodeTable.DueDate, 106) AS DueDate, dbo.BarcodeTable.Item, dbo.BarcodeTable.BarCode, dbo.BarcodeTable.Process, 
			   dbo.BarcodeTable.BookingNo, dbo.BarcodeTable.RowIndex, dbo.CustomerMaster.CustomerName,dbo.BarcodeTable.ItemRemarks as ItemRemark,dbo.BarcodeTable.ItemExtraprocessType,dbo.BarcodeTable.ItemTotalandSubTotal,dbo.BarcodeTable.ItemExtraprocessType2
		FROM   dbo.BarcodeTable INNER JOIN dbo.CustomerMaster ON dbo.BarcodeTable.BookingByCustomer = dbo.CustomerMaster.CustomerCode
		WHERE  (dbo.BarcodeTable.BookingNo = @BookingNo) and ( @RowIndex is null or dbo.BarcodeTable.RowIndex=@RowIndex)
	 END
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_Report_ChallanReport]    Script Date: 12/26/2011 11:55:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Manoj Gupta>
-- Create date: <10 Oct 2011>
-- Description:	<To select data for serviceTax report>
-- =============================================
EXEC Sp_Report_VendorReport '1/Sep/2011','30/Sep/2011','',''
*/
CREATE PROCEDURE [dbo].[Sp_Report_ChallanReport]
	(
		@ChallanDate1 datetime='',
		@ChallanDate2	datetime='',
		@ChallanNumber varchar(max)= '',
		@Flag varchar(max)=''
	)
AS
BEGIN
IF(@Flag=1)
BEGIN
	CREATE TABLE #TmpTable (BookingDate datetime, ChallanNumber varchar(max) ,BookingNumber varchar(20), ItemName varchar(max), ProcessType varchar(max),ExtraProcessType varchar(max),TotalQty int)
	INSERT INTO #TmpTable( BookingDate, ChallanNumber, BookingNumber,ItemName,ProcessType,ExtraProcessType,TotalQty)
		SELECT BookingDate, ChallanNumber, BookingNumber,ItemName,ProcessType,ExtraProcessType,TotalQty
		FROM(
SELECT     CONVERT(varchar, dbo.BarcodeTable.BookingDate, 106) AS BookingDate, dbo.EntChallan.ChallanNumber, dbo.EntChallan.BookingNumber, 
                      dbo.BarcodeTable.Item as ItemName, CASE WHEN barcodetable.Process = 'NONE' THEN '' ELSE barcodetable.Process END AS ProcessType, 
                      barcodetable.ItemExtraProcessType AS ExtraProcessType, SUM(dbo.BarcodeTable.SNo) AS TotalQty
FROM         dbo.EntChallan INNER JOIN
                      dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND 
                      dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex
WHERE EntChallan.ChallanDate BETWEEN @ChallanDate1 AND @ChallanDate2
GROUP BY dbo.EntChallan.ChallanNumber, dbo.EntChallan.BookingNumber, dbo.BarcodeTable.Item, dbo.BarcodeTable.Process, 
                      dbo.BarcodeTable.ItemExtraprocessType, dbo.BarcodeTable.SNo, dbo.BarcodeTable.BookingDate

) as T1
	SELECT * FROM #TmpTable
	DROP TABLE #TmpTable 
END	
ELSE IF(@Flag=2)
		BEGIN
			CREATE TABLE #TmpTable1 (BookingDate varchar(20), ChallanNumber varchar(max) ,BookingNumber varchar(20), ItemName varchar(max), ProcessType varchar(max),ExtraProcessType varchar(max),TotalQty int,ChallanDate varchar(20),ChallanSendingShift varchar(max),ItemRemarks varchar(max),DueDate varchar(20))
			INSERT INTO #TmpTable1( BookingDate, ChallanNumber, BookingNumber,ItemName,ProcessType,ExtraProcessType,TotalQty,ChallanDate,ChallanSendingShift,ItemRemarks,DueDate)
			SELECT BookingDate, ChallanNumber, BookingNumber,ItemName,ProcessType,ExtraProcessType,TotalQty,ChallanDate,ChallanSendingShift,ItemRemarks,DueDate
						FROM(
			SELECT     CONVERT(varchar, dbo.BarcodeTable.BookingDate, 106) AS BookingDate, dbo.EntChallan.ChallanNumber, dbo.EntChallan.BookingNumber, 
									  dbo.BarcodeTable.Item as ItemName, CASE WHEN barcodetable.Process = 'NONE' THEN '' ELSE barcodetable.Process END AS ProcessType, 
									  barcodetable.ItemExtraProcessType AS ExtraProcessType, dbo.BarcodeTable.SNo AS TotalQty, convert(varchar,dbo.EntChallan.ChallanDate,106) as ChallanDate,dbo.EntChallan.ChallanSendingShift,dbo.BarcodeTable.ItemRemarks, convert(varchar,dbo.BarcodeTable.DueDate,106) as DueDate
				FROM         dbo.EntChallan INNER JOIN
									  dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND 
									  dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex
				WHERE dbo.EntChallan.ChallanNumber=@ChallanNumber
							
			) as T1			
			SELECT * FROM #TmpTable1
			order by BookingNumber
			DROP TABLE #TmpTable1
		END
ELSE IF(@Flag=3)
		BEGIN
				SELECT DISTINCT(ChallanNumber),CONVERT(VARCHAR,ChallanDate,106) AS ChallanDate,'Print' AS Prints FROM EntChallan
				WHERE ChallanDate BETWEEN @ChallanDate1 AND @ChallanDate2
		END
ELSE IF(@Flag=4)
		BEGIN
				SELECT DISTINCT(ChallanNumber),CONVERT(VARCHAR,ChallanDate,106) AS ChallanDate,'Print' AS Prints FROM EntChallan
				WHERE ChallanNumber=@ChallanNumber
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[sp_NewBooking_SaveProc]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<MANOJ KUMAR GUPTA>
-- Create date: <15-Nov-2011>
-- Description:	<SAVE DATA IN THE NEW BOOKING>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NewBooking_SaveProc]	
	@Flag VARCHAR(MAX)='',
	@BookingAcceptedByUserId VARCHAR(MAX)='',
	@BookingByCustomer VARCHAR(MAX)='',
	@IsUrgent bit='',
	@BookingDate datetime='',
	@BookingDeliveryDate datetime='',
	@BookingDeliveryTime VARCHAR(MAX)='',
	@TotalCost float='',
	@Discount float='',
	@NetAmount float='',
	@BookingStatus int='',
	@BookingCancelDate datetime='',
	@BookingCancelReason VARCHAR(MAX)='',
	@BookingRemarks VARCHAR(MAX)='',
	@ItemTotalQuantity int='',
	@Qty int='',
	@VendorOrderStatus int='',
	@HomeDelivery bit='',
	@CheckedByEmployee VARCHAR(MAX)='',
	@BarCode VARCHAR(MAX)='',
	@BookingTime NVARCHAR(MAX)='',
	@Format VARCHAR(10)='',
	@ReceiptDeliverd bit='',
	@BOOKINGNUMBER VARCHAR(MAX)='',
	@ISN INT='',
	@ItemName VARCHAR(MAX)='',	
	@ItemProcessType VARCHAR(MAX)='',
	@ItemQuantityAndRate VARCHAR(MAX)='',
	@ItemExtraProcessType1 VARCHAR(MAX)='',
	@ItemExtraProcessRate1 FLOAT='',
	@ItemExtraProcessType2 VARCHAR(MAX)='',
	@ItemExtraProcessRate2 FLOAT='',
	@ItemExtraProcessType3 VARCHAR(MAX)='',
	@ItemExtraProcessRate3 FLOAT='',
	@ItemSubTotal FLOAT='',
	@ItemStatus INT='',
	@ItemRemark VARCHAR(MAX)='',
	@DeliveredQty INT='',
	@ItemColor VARCHAR(MAX)='',
	@VendorItemStatus INT='',
	@STPAmt FLOAT='',
	@STEP1Amt FLOAT='',
	@STEP2Amt FLOAT='',
	@CustomerCode VARCHAR(MAX)='',
	@TransDate SMALLDATETIME='',
	@AdvanceAmt float='',
	@ID int='',	
	@CurrentTime VARCHAR(MAX)='',
	@DueDate smalldatetime='',
	@Item  VARCHAR(MAX)='',	
	@Process VARCHAR(MAX)='',
	@StatusId int='',
	@BookingNo VARCHAR(MAX)='',
	@SNo int='',
	@RowIndex int='',	
	@Colour varchar(max)='',
	@ItemExtraprocessType varchar(max)='',
	@DrawlStatus bit=false,
	@DrawlAlloted bit=false,
	@DrawlName varchar(100)=null,
	@AllottedDrawl varchar(max)='',			
	@ItemsReceivedFromVendor int='',
	@BookingUser varchar(max)='',
	@BookDate1 datetime='',
	@BookDate2 datetime='',			
	@ItemRemarks varchar(max)='',
	@CustomerName varchar(max)='',
	@CustomerAddress varchar(max)='',
	@ColorName varchar(max)='',
	@ColorCode varchar(max)='',
	@ProcessCode varchar(max)='',
	@DiscountAmt float='',
	@DiscountOption varchar(1)=''
	
AS
BEGIN	
	IF(@Flag = 1)
	BEGIN
		DECLARE @BOOKINGID INT,@StartBookingNumberFrom VARCHAR(MAX),@CheckValue int,@CheckRecord int
		SET @BOOKINGID =(SELECT COALESCE(MAX(Convert(int, BookingId)),0) From EntBookings)
		SET @BOOKINGID=@BOOKINGID+1
		SET @CheckRecord=(SELECT COUNT(*) FROM ENTBOOKINGS)
		--print @CheckRecord 
		IF(@CheckRecord=0)
		BEGIN
			SET @BOOKINGNUMBER = (SELECT COALESCE(MAX(Convert(int, BookingNumber)),0) FROM EntBookings)
			SET @StartBookingNumberFrom =(SELECT COALESCE(max(Convert(int, StartBookingNumberFrom)),0) FROM MstConfigSettings)
			IF(convert(int,@StartBookingNumberFrom)>convert(int,@BOOKINGNUMBER))
			BEGIN
				UPDATE MstConfigSettings SET StartBookingNumberFrom=@StartBookingNumberFrom+1
				SET @BOOKINGNUMBER=@StartBookingNumberFrom	
			END
			ELSE
			BEGIN
				SET @BOOKINGNUMBER=@BOOKINGNUMBER+1	
			END					
		END
		ELSE
		BEGIN		
		SET @BOOKINGNUMBER = (SELECT COALESCE(MAX(Convert(int, BookingNumber)),0) FROM EntBookings)
		SET @StartBookingNumberFrom =(SELECT COALESCE(max(Convert(int, StartBookingNumberFrom)),0) FROM MstConfigSettings)
		--SET @StartBookingNumberFrom=@StartBookingNumberFrom+1		
		SET @CheckValue= (SELECT BookingNumber FROM EntBookings WHERE BookingNumber=@StartBookingNumberFrom)
		IF(@CheckValue IS NULL)
		BEGIN
		------ PICK THE BOOKING NUMBER FROM CONFIG TABLE delete from entbookings
		SET @BOOKINGNUMBER=@StartBookingNumberFrom+1	
		UPDATE MstConfigSettings SET StartBookingNumberFrom=@BOOKINGNUMBER
		SET @BOOKINGNUMBER=@BOOKINGNUMBER-1			
		END
		ELSE
		BEGIN
		-----  PICK THE BOOKING NUMBER FROM ENTBOOKING TABLE
		SET @BOOKINGNUMBER=@BOOKINGNUMBER+1		
		END
		END
		
		SET @BookingTime=(select convert(varchar(2),getdate(),8))
		SET @Format=(select RIGHT(convert(varchar,getdate(),100),2))
	
		--SET @BookingByCustomer=(Select CustomerCode From CustomerMaster Where CustomerName=@CustomerName AND CustomerAddress=@CustomerAddress)

		----- INSERT INTO ENTBOOKING TABLE THIS IS FIRST TABLE
		
		INSERT INTO EntBookings (BookingID,BookingNumber,BookingByCustomer,BookingAcceptedByUserId,IsUrgent,BookingDate,BookingDeliveryDate,BookingDeliveryTime,TotalCost,Discount,NetAmount,BookingStatus,BookingCancelDate,BookingCancelReason,BookingRemarks, ItemTotalQuantity,VendorOrderStatus,HomeDelivery,CheckedByEmployee,BookingTime,Format,DiscountAmt,DiscountOption)
			VALUES
								(@BOOKINGID,@BOOKINGNUMBER,@BookingByCustomer,@BookingAcceptedByUserId,@IsUrgent,@BookingDate,@BookingDeliveryDate,@BookingDeliveryTime,@TotalCost,@Discount,@NetAmount,'1','1/1/1900','',@BookingRemarks,@ItemTotalQuantity,'1',@HomeDelivery,@CheckedByEmployee,@BookingTime,@Format,@DiscountAmt,@DiscountOption)
		SELECT @BOOKINGNUMBER as BookingNumber 
--	select * from EntBookingDetails where bookingnumber='25042'
	END	
	ELSE IF(@Flag = 2)
	BEGIN
		
		----- INSERT INTO ENTBOOKINGDETAILS TABLE THIS IS SECOND TABLE
		--SET @BOOKINGNUMBER = (SELECT COALESCE(MAX(Convert(int, BookingNumber)),0) FROM EntBookings)		
		INSERT INTO EntBookingDetails (BookingNumber, ISN, ItemName, ItemTotalQuantity, ItemProcessType, ItemQuantityAndRate, ItemExtraProcessType1, ItemExtraProcessRate1, ItemExtraProcessType2, ItemExtraProcessRate2, ItemSubTotal, ItemStatus, ItemRemark,ItemColor,VendorItemStatus,STPAmt,STEP1Amt,STEP2Amt)
			VALUES
										(@BOOKINGNUMBER,@ISN,@ItemName,@ItemTotalQuantity,@ItemProcessType,@ItemQuantityAndRate,@ItemExtraProcessType1,@ItemExtraProcessRate1,@ItemExtraProcessType2,@ItemExtraProcessRate2,@ItemSubTotal,'1',@ItemRemark,@ItemColor,'1',@STPAmt,@STEP1Amt,@STEP2Amt)

	END
	ELSE IF(@Flag = 3)
	BEGIN
		------ START ACCOUNT PORTION
		DECLARE @CASH FLOAT,@fltCustPostBalance FLOAT,@SALES FLOAT,@CustLedgerName VARCHAR(MAX),@CustPreBalance FLOAT
		SET @CustLedgerName=(Select CustomerCode As CustLedgerName From CustomerMaster Where CustomerCode=@CustomerCode)
		--SET @BOOKINGNUMBER = (SELECT COALESCE(MAX(Convert(int, BookingNumber)),0) FROM EntBookings)				
		SET @CASH=(Select CurrentBalance From LedgerMaster Where LedgerName='Cash')		
		SET @SALES=(Select CurrentBalance From LedgerMaster Where LedgerName='Sales')		
		SET @CustPreBalance=(Select CurrentBalance From LedgerMaster Where LedgerName=@CustomerCode)
		SET @TransDate=getdate()
		--PRINT @CASH
		IF(@CASH IS NULL)
		BEGIN
			INSERT INTO LedgerMaster (LedgerName, OpenningBalance, CurrentBalance) Values('CASH','0','0')
			SET @CASH=(Select CurrentBalance From LedgerMaster Where LedgerName='Cash')					
		END				
		IF(@SALES IS NULL)
		BEGIN
			INSERT INTO LedgerMaster (LedgerName, OpenningBalance, CurrentBalance) Values('Sales','0','0')
			SET @SALES=(Select CurrentBalance From LedgerMaster Where LedgerName='Sales')
		END			
		------ SAVE INTO ENTLEDGERENTRIES
		INSERT INTO EntLedgerEntries 
			(TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration)VALUES
			(@TransDate,@CustLedgerName,'By Sales',@CustPreBalance,@TotalCost,'0',@CustPreBalance+@TotalCost,'On Credit New Booking Number '+@BOOKINGNUMBER)
		INSERT INTO EntLedgerEntries 
			(TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration)VALUES
			(@TransDate,'Sales','To'+' '+@CustLedgerName,@SALES,'0',@TotalCost,@CustPreBalance-@TotalCost,'On Credit New Booking Number '+@BOOKINGNUMBER)
		SET @fltCustPostBalance=@CustPreBalance+@TotalCost
		IF(@AdvanceAmt>0)
			BEGIN
				SET @CustPreBalance=(Select CurrentBalance From LedgerMaster Where LedgerName=@CustomerCode)
				INSERT INTO EntPayment (BookingNumber, PaymentDate, PaymentMade, DiscountOnPayment) VALUES (@BOOKINGNUMBER,GETDATE(),@AdvanceAmt,'0')
				INSERT INTO EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration) VALUES (@TransDate,@CustomerCode,'To Cash',(@CustPreBalance+@TotalCost),'0',@AdvanceAmt,(@CustPreBalance+@TotalCost)-@AdvanceAmt,'Received advance against Booking number '+@BOOKINGNUMBER)
				INSERT INTO EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration) VALUES (@TransDate,'Cash','By '+@CustLedgerName,@CASH,@AdvanceAmt,'0',@CASH+@AdvanceAmt,'Received advance against Booking number '+@BOOKINGNUMBER)
				SET @CustPreBalance=(Select CurrentBalance From LedgerMaster Where LedgerName=@CustomerCode)
				UPDATE LedgerMaster SET CurrentBalance=@CASH+@AdvanceAmt WHERE LedgerName='Cash'	
				SET @fltCustPostBalance=((@CustPreBalance+@TotalCost)-@AdvanceAmt)
			END
		UPDATE LedgerMaster SET CurrentBalance=@fltCustPostBalance WHERE LedgerName=@CustomerCode
		UPDATE LedgerMaster SET CurrentBalance=@SALES-@TotalCost WHERE LedgerName='Sales'
	END
	ELSE IF(@Flag=4)
	BEGIN
		SELECT ColorCode FROM MSTCOLOR WHERE ColorName=@ColorName
	END
	ELSE IF(@Flag=5)
	BEGIN
		SELECT ColorName FROM MSTCOLOR WHERE ColorCode=@ColorCode
	END
	ELSE IF(@Flag=6)
	BEGIN
		SELECT ItemName FROM ItemMaster WHERE ItemName=@ItemName
	END
	ELSE IF(@Flag=7)
	BEGIN
		SELECT ProcessCode  FROM ProcessMaster WHERE ProcessCode=@ProcessCode
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_NewBooking]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <03-Nov-2011>
-- Description:	<>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NewBooking]	
	@Flag VARCHAR(MAX)='',
	@Priority VARCHAR(MAX)='',
	@CustName VARCHAR(MAX)='',
	@CustAddress VARCHAR(MAX)='',
	@CustTitle VARCHAR(MAX)='',	
	@CustMobile VARCHAR(MAX)='',
	@CustRemarks VARCHAR(MAX)='',
	@CustPriority VARCHAR(MAX)='',
	@CustCode VARCHAR(MAX)='',
	@CustAreaLocation VARCHAR(MAX)='',
	@ItemSearchName VARCHAR(MAX)='',
	@ItemName VARCHAR(MAX)='',
	@NumberOfSubItems VARCHAR(MAX)='',
	@ItemCode VARCHAR(MAX)='',
	@SubItem1 VARCHAR(MAX)='',
	@SubItem2 VARCHAR(MAX)='',
	@SubItem3 VARCHAR(MAX)='',
	@CustomerName VARCHAR(MAX)='',
	@ProcessCode VARCHAR(MAX)='',
	@ProcessName VARCHAR(MAX)='',
	@Remarks VARCHAR(MAX)='',
	@ColorName VARCHAR(MAX)='',
	@BDate SMALLDATETIME='',
	@ADate SMALLDATETIME='',
	@SubItemOrder VARCHAR(MAX)='',
	@UserTypeId int='',
	@PageTitle VARCHAR(MAX)='',
	@FileName VARCHAR(MAX)='',
	@RightToView VARCHAR(MAX)='',
	@MenuItemLevel int='',
	@MenuPosition int='',
	@ParentMenu VARCHAR(MAX)='',
	@BookingNo varchar(max)='',
	@SaveItemName varchar(max)='',
	@BookingItemName varchar(max)='',
	@BarcodeISN varchar(max)=''
	
AS
BEGIN	
	IF(@Flag = 1)
		SELECT '0' PriorityID,' ----------------- NONE ----------------- ' Priority
		UNION
		SELECT PriorityID,upper(Priority) FROM PriorityMaster
	IF(@Flag = 2)
		BEGIN
			DECLARE @ID INT
			SELECT @ID=COALESCE(MAX(PriorityID),0) FROM PriorityMaster	
			SET @ID = @ID + 1
			INSERT INTO PriorityMaster VALUES(@ID, @Priority)			
		END
	IF(@Flag = 3)
		BEGIN
			DECLARE @N_ID VARCHAR
			SET @N_ID = '0'
			SELECT @N_ID = COUNT(ID) FROM CustomerMaster WHERE  CustomerName=@CustName AND CustomerAddress=@CustAddress
			IF(@N_ID='0')
				BEGIN
					DECLARE @NEW_ID INT
					SELECT @NEW_ID = COALESCE(MAX(ID),0) FROM CustomerMaster
					SET @NEW_ID = @NEW_ID + 1
					INSERT INTO CustomerMaster (ID,CustomerCode,CustomerSalutation,CustomerName,CustomerAddress,CustomerMobile,Remarks,CustomerPriority,AreaLocation,CustomerRegisterDate,BirthDate,AnniversaryDate)
					VALUES (@NEW_ID,'Cust'+ CONVERT(VARCHAR,@NEW_ID),@CustTitle,@CustName,@CustAddress,@CustMobile,@CustRemarks,@CustPriority,@CustAreaLocation,GETDATE(),@BDate,@ADate)
					
					INSERT INTO LedgerMaster Values('Cust'+ CONVERT(VARCHAR,@NEW_ID),'0','0')	
					
					SELECT 'Cust'+ CONVERT(VARCHAR,@NEW_ID) AS CustCode
				END					
		END
	 IF (@Flag=4)
		BEGIN
			declare @id1 int			
			set @id1= (select customerpriority from customermaster where customercode=@CustCode)				
			IF(@id1<>0)
			BEGIN
				SELECT  dbo.CustomerMaster.CustomerSalutation + ' ' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, 
						dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority
				FROM    dbo.CustomerMaster INNER JOIN
						dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID
				where dbo.CustomerMaster.customercode=@CustCode
			END
		ELSE
			BEGIN

				CREATE TABLE #TmpTable (CustName varchar(max),CustAddress varchar(max),CustMobile varchar(max),CustRemarks varchar(max),CustPriority varchar(max))
				INSERT INTO #TmpTable( CustName, CustAddress, CustMobile,CustRemarks,CustPriority) SELECT CustName, CustAddress,CustMobile,CustRemarks,CustPriority
				FROM(
				SELECT  CustomerSalutation + ' ' + CustomerName AS CustName, CustomerAddress AS CustAddress, CustomerMobile AS CustMobile, Remarks AS CustRemarks,'NONE' AS CustPriority FROM   dbo.CustomerMaster
				where customercode=@CustCode
			
				) AS T1	
				select * from #TmpTable				
				DROP TABLE #TmpTable
			END
		END	
	IF(@Flag=5)
		BEGIN
				SELECT DefaultSearchCriteria FROM mstConfigSettings 
		END
	IF(@Flag=6)
		BEGIN
			DECLARE @MainDate DATETIME
			DECLARE @BookDate1 INT	
			DECLARE @DueTime VARCHAR(MAX)	
			SELECT @BookDate1=DeliveryDateOffset,@DueTime=DefaultTime+' '+DefaultAmPm  FROM mstConfigSettings	
			SET @MainDate= (SELECT convert(varchar, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0) + @BookDate1,106))				
			SELECT CONVERT(VARCHAR,@MainDate,106) AS DueDate,@DueTime AS DueTime		
		END	
	IF(@Flag=7)
		BEGIN
				SELECT dbo.ItemMaster.ItemName, dbo.ProcessMaster.ProcessCode as ProcessName FROM dbo.MstConfigSettings INNER JOIN  dbo.ItemMaster ON dbo.MstConfigSettings.DefaultItemId = dbo.ItemMaster.ItemID INNER JOIN  dbo.ProcessMaster ON dbo.MstConfigSettings.DefaultProcessCode = dbo.ProcessMaster.ProcessCode
		END
	IF(@Flag=8)
		BEGIN
				SELECT ItemCode +' - '+ ItemName AS ItemName FROM ItemMaster WHERE ItemCode like '%'+@ItemSearchName+'%' or ItemName like '%'+@ItemSearchName+'%'
		END
	IF(@Flag=9)
		BEGIN
				--declare @SaveItemName varchar(max)
--				declare @ItemName varchar(max)
--				declare @NumberOfSubItems varchar(max)
--				set @NumberOfSubItems=2
--				set @ItemName='Charas'
				if(@NumberOfSubItems<= 1)
				begin
					set @SaveItemName=@ItemName					
				end
				else
				begin
					set @SaveItemName=@ItemName+ ' ' + '(' + @NumberOfSubItems + 'pcs)'					
				end
				Insert Into ItemMaster (ItemName, NumberOfSubItems,ItemCode) Values (@SaveItemName,@NumberOfSubItems,@ItemCode)
				DELETE FROM EntSubItemDetails WHERE ItemName=@ItemName
		--- WHEN QTY IS 1
				if(@NumberOfSubItems>0)
				begin
				if(@SubItem1='')
				begin
					set @SubItem1=@SaveItemName
				end
				INSERT INTO EntSubItemDetails (ItemName, SubItemName, SubItemOrder) VALUES (@SaveItemName,@SubItem1,'1')
				end
		--- WHEN QTY IS 2
				if(@NumberOfSubItems>1)
				begin
				INSERT INTO EntSubItemDetails (ItemName, SubItemName, SubItemOrder) VALUES (@SaveItemName,@SubItem2,'2')
				end
		--- WHEN QTY IS 3 
				if(@NumberOfSubItems>2)
				begin
				INSERT INTO EntSubItemDetails (ItemName, SubItemName, SubItemOrder) VALUES (@SaveItemName,@SubItem3,'3')
				end	
				SELECT @SaveItemName AS ItemNameFillGrid			
		END
	ELSE IF (@Flag=10)
		BEGIN
			if(@CustomerName<>'' AND @CustAddress<>'' AND @CustMobile<>'')
				SELECT  dbo.CustomerMaster.CustomerCode AS CustomerCode,dbo.CustomerMaster.CustomerSalutation + ' ' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority FROM dbo.CustomerMaster INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID  WHERE dbo.CustomerMaster.CustomerName LIKE '%'+@CustomerName+'%' AND dbo.CustomerMaster.CustomerAddress LIKE '%'+@CustAddress+'%' AND dbo.CustomerMaster.CustomerMobile LIKE '%'+@CustMobile+'%' ORDER BY CustomerName ASC
			ELSE IF(@CustomerName='' AND @CustAddress='' AND @CustMobile='') 
				SELECT  dbo.CustomerMaster.CustomerCode AS CustomerCode,dbo.CustomerMaster.CustomerSalutation + ' ' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority FROM dbo.CustomerMaster INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID  ORDER BY CustomerName ASC
			ELSE IF(@CustomerName<>'' AND @CustAddress<>'' AND @CustMobile='') 
				SELECT  dbo.CustomerMaster.CustomerCode AS CustomerCode,dbo.CustomerMaster.CustomerSalutation + ' ' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority FROM dbo.CustomerMaster INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID  WHERE dbo.CustomerMaster.CustomerName LIKE '%'+@CustomerName+'%' AND dbo.CustomerMaster.CustomerAddress LIKE '%'+@CustAddress+'%' ORDER BY CustomerName ASC
			ELSE IF(@CustomerName='' AND @CustAddress<>'' AND @CustMobile<>'') 
				SELECT  dbo.CustomerMaster.CustomerCode AS CustomerCode,dbo.CustomerMaster.CustomerSalutation + ' ' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority FROM dbo.CustomerMaster INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID  WHERE dbo.CustomerMaster.CustomerAddress LIKE '%'+@CustAddress+'%' AND dbo.CustomerMaster.CustomerMobile LIKE '%'+@CustMobile+'%' ORDER BY CustomerName ASC
			ELSE IF(@CustomerName<>'' AND @CustAddress='' AND @CustMobile='') 
				SELECT  dbo.CustomerMaster.CustomerCode AS CustomerCode,dbo.CustomerMaster.CustomerSalutation + ' ' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority FROM dbo.CustomerMaster INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID  WHERE dbo.CustomerMaster.CustomerName LIKE '%'+@CustomerName+'%' ORDER BY CustomerName ASC
			ELSE IF(@CustomerName='' AND @CustAddress='' AND @CustMobile<>'') 
				SELECT  dbo.CustomerMaster.CustomerCode AS CustomerCode,dbo.CustomerMaster.CustomerSalutation + ' ' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority FROM dbo.CustomerMaster INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID  WHERE dbo.CustomerMaster.CustomerMobile LIKE '%'+@CustMobile+'%' ORDER BY CustomerName ASC
			ELSE IF(@CustomerName<>'' AND @CustAddress='' AND @CustMobile<>'') 
				SELECT  dbo.CustomerMaster.CustomerCode AS CustomerCode,dbo.CustomerMaster.CustomerSalutation + ' ' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority FROM dbo.CustomerMaster INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID  WHERE dbo.CustomerMaster.CustomerName LIKE '%'+@CustomerName+'%' AND dbo.CustomerMaster.CustomerMobile LIKE '%'+@CustMobile+'%' ORDER BY CustomerName ASC
			ELSE IF(@CustomerName='' AND @CustAddress<>'' AND @CustMobile='') 
				SELECT  dbo.CustomerMaster.CustomerCode AS CustomerCode,dbo.CustomerMaster.CustomerSalutation + ' ' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority FROM dbo.CustomerMaster INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID  WHERE dbo.CustomerMaster.CustomerAddress LIKE '%'+@CustAddress+'%' ORDER BY CustomerName ASC
		END
	ELSE IF (@Flag=11)
		BEGIN
			SELECT (CustomerCode + '  ' + '-' + '  ' + CustomerName + ' ' + CustomerAddress + ' ' + CustomerMobile) AS CustName FROM   dbo.CustomerMaster  WHERE customername like '%'+@CustomerName+'%' or CustomerAddress like '%'+@CustomerName+'%' or CustomerMobile like '%'+@CustomerName+'%' order by customername asc

		END	
	ELSE IF (@Flag=12)
		BEGIN
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax) VALUES (@ProcessCode,@ProcessName,'FALSE','0','0','FALSE')
		END
	ELSE IF(@Flag=13)
		BEGIN
			SELECT ProcessCode FROM PROCESSMASTER WHERE ProcessCode=@ProcessCode
		END
	ELSE IF(@Flag=14)
		BEGIN
			IF NOT EXISTS(SELECT * FROM MstRemark where Remarks=@Remarks)
			BEGIN
			DECLARE @Remark_ID INT
			SELECT @Remark_ID = COALESCE(MAX(ID),0) FROM MstRemark
			SET @Remark_ID = @Remark_ID + 1
			INSERT INTO MstRemark (ID,Remarks) VALUES (@Remark_ID,@Remarks)
			END
		END
	ELSE IF(@Flag=15)
		BEGIN
			IF NOT EXISTS (SELECT * FROM mstColor where ColorName=@ColorName)
			BEGIN
			DECLARE @Color_ID VARCHAR(MAX),@Color_Name VARCHAR(MAX),@Color_Code VARCHAR(MAX)
					
			SELECT @Color_ID = COALESCE(MAX(ID),0) FROM mstColor
			SET @Color_ID = @Color_ID + 1
			SET @Color_Name=SUBSTRING ( @ColorName, 1 , 1 )
			SET @Color_Code=UPPER(@Color_Name)+@Color_ID

			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES (@Color_ID,UPPER(@ColorName),@Color_Code)
			END
		END
	ELSE IF(@Flag=16)
		BEGIN
			SELECT DefaultDiscountRate FROM Customermaster WHERE CustomerCode=@CustCode
		END
	ELSE IF(@Flag=17)
		BEGIN
			declare @ProcessPrice varchar(max)
			set @ProcessPrice=(SELECT dbo.ItemwiseProcessRate.ProcessPrice FROM dbo.ItemMaster INNER JOIN dbo.ItemwiseProcessRate ON dbo.ItemMaster.ItemName = dbo.ItemwiseProcessRate.ItemName INNER JOIN dbo.ProcessMaster ON dbo.ItemwiseProcessRate.ProcessCode = dbo.ProcessMaster.ProcessCode
			WHERE (dbo.ItemwiseProcessRate.ItemName = @ItemName) AND (dbo.ProcessMaster.ProcessCode = @ProcessName))
			if(@ProcessPrice<>'')
			begin
				SELECT  dbo.ItemwiseProcessRate.ProcessPrice FROM dbo.ItemMaster INNER JOIN dbo.ItemwiseProcessRate ON dbo.ItemMaster.ItemName = dbo.ItemwiseProcessRate.ItemName INNER JOIN dbo.ProcessMaster ON dbo.ItemwiseProcessRate.ProcessCode = dbo.ProcessMaster.ProcessCode WHERE (dbo.ItemwiseProcessRate.ItemName = @ItemName) AND (dbo.ProcessMaster.ProcessCode = @ProcessName)
			end
			else
			begin
				SELECT '0' AS ProcessPrice
			end
		END
	ELSE IF(@Flag=18)
		BEGIN
			SELECT COALESCE(MAX(Convert(int, BookingNumber)),0) FROM EntBookings
		END
	ELSE IF(@Flag=19)
		BEGIN
			SELECT  dbo.mstColor.ColorName FROM dbo.MstConfigSettings INNER JOIN dbo.mstColor ON dbo.MstConfigSettings.DefaultColorName = dbo.mstColor.ID
		END
	ELSE IF(@Flag=20)
		BEGIN
			INSERT INTO ItemMaster (ItemName, NumberOfSubItems,ItemCode) VALUES (@SaveItemName,@NumberOfSubItems,@ItemCode)
		END
	ELSE IF(@Flag=21)
		BEGIN
			INSERT INTO EntSubItemDetails (ItemName, SubItemName, SubItemOrder) VALUES (@SaveItemName,@SubItem2,@SubItemOrder)
		END
	ELSE IF(@Flag=22)
	BEGIN
		INSERT INTO EntMenuRights (UserTypeId,PageTitle,FileName,RightToView,MenuItemLevel,MenuPosition,ParentMenu) VALUES (@UserTypeId,@PageTitle,@FileName,@RightToView,@MenuItemLevel,@MenuPosition,@ParentMenu)
	END
	ELSE IF(@Flag=23)
	BEGIN
		SELECT TOP(1) ItemName FROM ItemMaster ORDER BY itemid DESC
	END
	ELSE IF(@Flag=24)
	BEGIN
		SELECT BookingNumber FROM EntBookings WHERE BookingNumber=@BookingNo
	END
	ELSE IF(@Flag=25)
	BEGIN
		SELECT IsDiscount FROM ProcessMaster WHERE ProcessCode=@ProcessCode
	END
	ELSE IF(@Flag=26)
	BEGIN
		SELECT CurrencyType FROM mstReceiptConfig 
	END
	ELSE IF(@Flag=27)
	BEGIN
		SELECT AmountType FROM MstConfigSettings 
	END
	ELSE IF(@Flag=28)
	BEGIN
		SELECT DeliveredQty FROM Barcodetable where BookingItemName=@BookingItemName and BarcodeISN=@BarcodeISN and BookingNo=@BookingNo
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Mearge_DataWithExistigBooking]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
EXEC SP_Mearge_DataWithExistigBooking
*/
CREATE PROCEDURE [dbo].[SP_Mearge_DataWithExistigBooking]
	
AS
BEGIN
	--Branch Master
	UPDATE BranchMaster SET BranchName= ND.BranchName, BranchAddress = ND.BranchAddress, BranchPhone = ND.BranchPhone, BranchSlogan = ND.BranchSlogan
		FROM BranchMaster INNER JOIN RoxyDryCleanForUpdate.dbo.BranchMaster ND ON BranchMaster.BranchCode = ND.BranchCode
	
	--PriorityMaster
	UPDATE PriorityMaster SET Priority = ND.Priority
		FROM PriorityMaster INNER JOIN RoxyDryCleanForUpdate.dbo.PriorityMaster ND ON PriorityMaster.PriorityID = ND.PriorityID

	INSERT INTO PriorityMaster
		SELECT * FROM RoxyDryCleanForUpdate.dbo.PriorityMaster ND
		WHERE ND.PriorityID NOT IN (SELECT DISTINCT PriorityID From RoxyDryCleanDatabase.dbo.PriorityMaster)

	--CustomerMaster
	UPDATE CustomerMaster SET CustomerSalutation = ND.CustomerSalutation, CustomerName = ND.CustomerName, CustomerPhone = ND.CustomerPhone, CustomerMobile = ND.CustomerMobile, CustomerEmailId = ND.CustomerEmailId, CustomerPriority= ND.CustomerPriority, CustomerRefferredBy = ND.CustomerRefferredBy, DefaultDiscountRate = ND.DefaultDiscountRate
		FROM CustomerMaster INNER JOIN RoxyDryCleanForUpdate.dbo.CustomerMaster ND ON CustomerMaster.CustomerCode = ND.CustomerCode

	INSERT INTO CustomerMaster 
		SELECT * FROM RoxyDryCleanForUpdate.dbo.CustomerMaster ND
		WHERE ND.CustomerCode NOT IN (SELECT DISTINCT CustomerCode From RoxyDryCleanDatabase.dbo.CustomerMaster)

	--ShiftMaster
	UPDATE ShiftMaster SET ShiftName = ND.ShiftName
		FROM ShiftMaster INNER JOIN RoxyDryCleanForUpdate.dbo.ShiftMaster ND ON ShiftMaster.ShiftID = ND.ShiftID

	INSERT INTO ShiftMaster (ShiftName)
		SELECT ShiftName FROM RoxyDryCleanForUpdate.dbo.ShiftMaster ND
		WHERE ND.ShiftID NOT IN (SELECT DISTINCT ShiftID From RoxyDryCleanDatabase.dbo.ShiftMaster)
	
	--ProcessMaster
	UPDATE ProcessMaster SET ProcessCode = ND.ProcessCode, ProcessType = ND.ProcessType, ProcessName = ND.ProcessName, ProcessUsedForVendorReport = ND.ProcessUsedForVendorReport
		FROM ProcessMaster INNER JOIN RoxyDryCleanForUpdate.dbo.ProcessMaster ND ON ProcessMaster.ProcessCode = ND.ProcessCode

	INSERT INTO ProcessMaster
		SELECT * FROM RoxyDryCleanForUpdate.dbo.ProcessMaster ND
		WHERE ND.ProcessCode NOT IN (SELECT DISTINCT ProcessCode From RoxyDryCleanDatabase.dbo.ProcessMaster)


	--ItemMaster
	UPDATE ItemMaster SET ItemName = ND.ItemName, NumberOfSubItems = ND.NumberOfSubItems
		FROM ItemMaster INNER JOIN RoxyDryCleanForUpdate.dbo.ItemMaster ND ON ItemMaster.ItemId = ND.ItemId

	INSERT INTO ItemMaster (ItemName, NumberOfSubItems)
		SELECT ItemName, NumberOfSubItems FROM RoxyDryCleanForUpdate.dbo.ItemMaster ND
		WHERE ND.ItemId NOT IN (SELECT DISTINCT ItemId From RoxyDryCleanDatabase.dbo.ItemMaster)

--	--EntSubItemDetails
--	UPDATE EntSubItemDetails SET SubItemName = ND.SubItemName, SubItemOrder = ND.SubItemOrder
--		FROM EntSubItemDetails INNER JOIN RoxyDryCleanForUpdate.dbo.EntSubItemDetails ND ON EntSubItemDetails.ItemName = ND.ItemName AND SubItemName = ND.SubItemName

	INSERT INTO EntSubItemDetails
		SELECT * FROM RoxyDryCleanForUpdate.dbo.EntSubItemDetails ND
		WHERE ND.ItemName NOT IN (SELECT DISTINCT ItemName From RoxyDryCleanDatabase.dbo.EntSubItemDetails)

	--ItemWiseProcessRate
	UPDATE ItemWiseProcessRate SET ProcessPrice = ND.ProcessPrice
		FROM ItemWiseProcessRate INNER JOIN RoxyDryCleanForUpdate.dbo.ItemWiseProcessRate ND ON ItemWiseProcessRate.ItemName = ND.ItemName AND ItemWiseProcessRate.ProcessCode = ND.ProcessCode

	INSERT INTO ItemWiseProcessRate
		SELECT * FROM RoxyDryCleanForUpdate.dbo.ItemWiseProcessRate ND
		WHERE (ND.ItemName NOT IN (SELECT DISTINCT ItemName From RoxyDryCleanDatabase.dbo.ItemWiseProcessRate)) OR (ND.ProcessCode NOT IN (SELECT DISTINCT ProcessCode From RoxyDryCleanDatabase.dbo.ItemWiseProcessRate))

--	--LedgerMaster
--	UPDATE ItemWiseProcessRate SET ProcessPrice = ND.ProcessPrice
--		FROM ItemWiseProcessRate INNER JOIN RoxyDryCleanForUpdate.dbo.ItemWiseProcessRate ND ON ItemWiseProcessRate.ItemName = ND.ItemName AND ItemWiseProcessRate.ProcessCode = ND.ProcessCode

	INSERT INTO LedgerMaster
		SELECT * FROM RoxyDryCleanForUpdate.dbo.LedgerMaster ND
		WHERE (ND.LedgerName NOT IN (SELECT DISTINCT LedgerName From RoxyDryCleanDatabase.dbo.LedgerMaster))

	--EntBookings
		IF EXISTS (SELECT * FROM EntBookings WHERE CONVERT(varchar, BookingDate,106) = CONVERT(varchar,GetDate(),106))
			BEGIN
				DELETE FROM EntBookings WHERE CONVERT(varchar, BookingDate,106) = CONVERT(varchar,GetDate(),106)
			END
		INSERT INTO EntBookings
			SELECT * FROM RoxyDryCleanForUpdate.dbo.EntBookings ND WHERE CONVERT(varchar, ND.BookingDate,106) = CONVERT(varchar,GetDate(),106)

	--EntBookingDetails
		INSERT INTO EntBookingDetails ([BookingNumber],[ISN],[ItemName],[ItemTotalQuantity],[ItemProcessType],[ItemQuantityAndRate],[ItemExtraProcessType1],[ItemExtraProcessRate1],[ItemExtraProcessType2],[ItemExtraProcessRate2],[ItemExtraProcessType3],[ItemExtraProcessRate3],[ItemSubTotal],[ItemStatus],[ItemRemark],[DeliveredQty])
			SELECT ND.BookingNumber,[ISN],[ItemName], ND.ItemTotalQuantity,[ItemProcessType],[ItemQuantityAndRate],[ItemExtraProcessType1],[ItemExtraProcessRate1],[ItemExtraProcessType2],[ItemExtraProcessRate2],[ItemExtraProcessType3],[ItemExtraProcessRate3],[ItemSubTotal],[ItemStatus],[ItemRemark],[DeliveredQty] FROM RoxyDryCleanForUpdate.dbo.EntBookingDetails ND INNER JOIN RoxyDryCleanForUpdate.dbo.EntBookings NBD ON ND.BookingNumber = NBD.BookingNumber WHERE CONVERT(varchar, NBD.BookingDate,106) = CONVERT(varchar,GetDate(),106)


	--EntPayment
		IF EXISTS (SELECT * FROM EntPayment WHERE CONVERT(varchar, PaymentDate,106) = CONVERT(varchar,GetDate(),106))
			BEGIN
				DELETE FROM EntPayment WHERE  CONVERT(varchar, PaymentDate,106) = CONVERT(varchar,GetDate(),106)
			END
		INSERT INTO EntPayment (BookingNumber,PaymentDate, PaymentMade, DiscountOnPayment)
			SELECT ND.BookingNumber, PaymentDate, PaymentMade, DiscountOnPayment FROM RoxyDryCleanForUpdate.dbo.EntPayment ND INNER JOIN RoxyDryCleanForUpdate.dbo.EntBookings NBD ON ND.BookingNumber = NBD.BookingNumber WHERE CONVERT(varchar, NBD.BookingDate,106) = CONVERT(varchar,GetDate(),106) OR CONVERT(varchar, ND.PaymentDate,106) = CONVERT(varchar,GetDate(),106)

	--EntLedgerEntries
		IF EXISTS (SELECT * FROM EntLedgerEntries WHERE CONVERT(varchar, TransDate,106) = CONVERT(varchar,GetDate(),106))
			BEGIN
				DELETE FROM EntLedgerEntries WHERE  CONVERT(varchar, TransDate,106) = CONVERT(varchar,GetDate(),106)
			END
		INSERT INTO EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance,Narration)
			SELECT TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance,Narration FROM RoxyDryCleanForUpdate.dbo.EntLedgerEntries ND WHERE CONVERT(varchar, ND.TransDate,106) = CONVERT(varchar,GetDate(),106)
		
END
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertIntoBarcodeTable]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<KaramChandSharma And Manoj Kumar Gupta>
-- Create date: <18-Nov-2011>
-- Description:	<Insert Into Barcode Table>
-- =============================================
CREATE PROCEDURE [dbo].[sp_InsertIntoBarcodeTable]
@BOOKINGNUMBER VARCHAR(MAX)	
AS
BEGIN	
DECLARE @SNO INT,@RowIndex INT, @ID VARCHAR(MAX),@Barcode VARCHAR(MAX), @ItemName1 VARCHAR(MAX),@Qty VARCHAR(MAX),@TempQty INT, @ISN VARCHAR(MAX),@ItemName VARCHAR(MAX),@ItemTotalQuantity VARCHAR(MAX),@ItemProcessType VARCHAR(MAX),@ItemQuantityAndRate VARCHAR(MAX),@ItemExtraProcessType1 VARCHAR(MAX),@ItemExtraProcessRate1 VARCHAR(MAX),@ItemExtraProcessType2 VARCHAR(MAX),@ItemExtraProcessRate2 VARCHAR(MAX),@ItemSubTotal VARCHAR(MAX),@ItemStatus VARCHAR(MAX),@ItemRemark VARCHAR(MAX),@DeliveredQty VARCHAR(MAX),@ItemColor VARCHAR(MAX),@VendorItemStatus VARCHAR(MAX),@STPAmt VARCHAR(MAX),@STEP1Amt VARCHAR(MAX),@STEP2Amt VARCHAR(MAX),@BookingItemName VARCHAR(MAX),@BarcodeISN VARCHAR(MAX)
DECLARE @Color VARCHAR(MAX)
--SET @BOOKINGNUMBER = (SELECT COALESCE(MAX(Convert(int, BookingNumber)),0) FROM EntBookings)
SET @RowIndex=1
DECLARE @BookingDate1 VARCHAR(MAX),@BookingDeliveryTime1 VARCHAR(MAX),@BookingDeliveryDate1 VARCHAR(MAX),@BookingByCustomer1 VARCHAR(MAX)		

select @BookingDate1=BookingDate,@BookingDeliveryTime1=BookingDeliveryTime,@BookingDeliveryDate1=BookingDeliveryDate,@BookingByCustomer1=BookingByCustomer from entbookings WHERE BookingNumber=@BOOKINGNUMBER
DECLARE @Details CURSOR	
SET @Details = CURSOR FOR
	SELECT BookingNumber, ISN, ItemName, ItemTotalQuantity, ItemProcessType, ItemQuantityAndRate, ItemExtraProcessType1, ItemExtraProcessRate1, ItemExtraProcessType2, ItemExtraProcessRate2, ItemSubTotal, ItemStatus, ItemRemark, DeliveredQty, ItemColor, VendorItemStatus, STPAmt, STEP1Amt, STEP2Amt FROM dbo.EntBookingDetails WHERE BOOKINGNUMBER=@BOOKINGNUMBER
	OPEN @Details
		FETCH NEXT
			FROM @Details INTO @BOOKINGNUMBER,@ISN, @ItemName, @ItemTotalQuantity, @ItemProcessType, @ItemQuantityAndRate, @ItemExtraProcessType1, @ItemExtraProcessRate1, @ItemExtraProcessType2, @ItemExtraProcessRate2, @ItemSubTotal, @ItemStatus, @ItemRemark, @DeliveredQty, @ItemColor, @VendorItemStatus, @STPAmt, @STEP1Amt, @STEP2Amt
				WHILE @@FETCH_STATUS = 0
					BEGIN
						SET @SNO=1
						--SET @TempQty=@ItemTotalQuantity
						
							BEGIN
								DECLARE @TotalQty INT,@TempQty1 INT
								SELECT @TotalQty=@ItemTotalQuantity
								WHILE(@TotalQty>0)								
								BEGIN
									DECLARE @NewQty CURSOR					
										SET @NewQty = CURSOR FOR
											SELECT dbo.EntSubItemDetails.SubItemName as ItemName,dbo.ItemMaster.NumberOfsubItems  as Qty FROM dbo.ItemMaster INNER JOIN dbo.EntSubItemDetails ON dbo.ItemMaster.ItemName = dbo.EntSubItemDetails.ItemName	WHERE dbo.ItemMaster.ItemName = @ItemName
												OPEN @NewQty
													FETCH NEXT
														FROM @NewQty INTO @ItemName1,@Qty
															SET @TempQty1=@Qty															
															WHILE @@FETCH_STATUS = 0																		
																BEGIN	
																	WHILE @TempQty1>0	
																		BEGIN																						
																			SET @ID=(SELECT COALESCE(MAX(Convert(int, Id)),0) FROM BarcodeTable)
																			SET @ID=@ID+1																	 
																			--select @Color=colorCode from mstcolor where colorname=@ItemColor
																			INSERT INTO dbo.BarcodeTable (Id,BookingDate,CurrentTime,DueDate,Item,BarCode,Process,StatusId,BookingNo,SNo,RowIndex,BookingByCustomer,Colour,ItemExtraprocessType,DrawlStatus,DrawlAlloted,DeliveredQty,ItemRemarks,ItemTotalandSubTotal,ItemExtraprocessType2,BookingItemName,BarcodeISN)
																			VALUES
																			(@ID,@BookingDate1,@BookingDeliveryTime1,@BookingDeliveryDate1,@ItemName1,('*'+@BOOKINGNUMBER+'-'+ CONVERT(VARCHAR,@RowIndex)+'*') ,@ItemProcessType,'1',@BOOKINGNUMBER,'1',@RowIndex,@BookingByCustomer1, @ItemColor,@ItemExtraProcessType1,'false','false',0,@ItemRemark,(@ItemTotalQuantity+'/'+ CONVERT(VARCHAR,@SNO)),@ItemExtraProcessType2,@ItemName,@ISN)
																				
																			SET @SNO=@SNO+1
																			SET @RowIndex=@RowIndex+1
																			SET @TempQty1=@TempQty1-1	
																			FETCH NEXT
																				FROM @NewQty INTO @ItemName1,@Qty
																		END
																IF @TempQty1=0
																	BREAK
																	FETCH NEXT
																		FROM @NewQty INTO @ItemName1,@Qty
																END	
															UPDATE EntBookings SET Barcode=('*'+@BOOKINGNUMBER+'-'+ CONVERT(VARCHAR,'1')+'*') WHERE BookingNumber=@BOOKINGNUMBER
												CLOSE @NewQty
												DEALLOCATE @NewQty
									SET @TotalQty=@TotalQty-1
								END								
							END
					FETCH NEXT
						FROM @Details INTO @BOOKINGNUMBER,@ISN, @ItemName, @ItemTotalQuantity, @ItemProcessType, @ItemQuantityAndRate, @ItemExtraProcessType1, @ItemExtraProcessRate1, @ItemExtraProcessType2, @ItemExtraProcessRate2, @ItemSubTotal, @ItemStatus, @ItemRemark, @DeliveredQty, @ItemColor, @VendorItemStatus, @STPAmt, @STEP1Amt, @STEP2Amt
					END
	CLOSE @Details
	DEALLOCATE @Details
END
GO
/****** Object:  StoredProcedure [dbo].[sp_EditRecord]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <19-Nov-2011>
-- Description:	<Read data for edit record>
-- =============================================
CREATE PROCEDURE [dbo].[sp_EditRecord]
@BOOKINGNUMBER VARCHAR(MAX)	
AS
BEGIN	
	CREATE TABLE #TmpTable (ItemName VARCHAR(MAX),ItemTotalQuantity VARCHAR(MAX),ItemProcessType VARCHAR(MAX),ItemQuantityAndRate VARCHAR(MAX),ItemExtraProcessType1 VARCHAR(MAX),ItemExtraProcessRate1 VARCHAR(MAX),ItemExtraProcessType2 VARCHAR(MAX),ItemExtraProcessRate2 VARCHAR(MAX),ItemRemark VARCHAR(MAX),ItemColor VARCHAR(MAX),CustName VARCHAR(MAX),CustAddress VARCHAR(MAX),CustRemarks VARCHAR(MAX),CustPriority VARCHAR(MAX),CustPhone VARCHAR(MAX),BookingDate VARCHAR(MAX),BookingDeliveryDate VARCHAR(MAX),BookingDeliveryTime VARCHAR(MAX),BookingRemarks VARCHAR(MAX),Discount VARCHAR(MAX),HomeDelivery VARCHAR(MAX),CheckedByEmployee VARCHAR(MAX),IsUrgent VARCHAR(MAX),TotalCost VARCHAR(MAX),NetAmount VARCHAR(MAX),STPAmt VARCHAR(MAX),STEP1Amt VARCHAR(MAX),STEP2Amt VARCHAR(MAX),ItemSubtotal VARCHAR(MAX),Advance VARCHAR(MAX),CustCode VARCHAR(MAX),ShopIn VARCHAR(MAX),BookingCancelStatus int,DiscountAmt VARCHAR(MAX),DiscountOption VARCHAR(MAX))
	DECLARE @Advance VARCHAR(MAX),@ItemSubtotal VARCHAR(MAX), @ItemName VARCHAR(MAX),@ItemTotalQuantity VARCHAR(MAX),@ItemProcessType VARCHAR(MAX),@ItemQuantityAndRate VARCHAR(MAX),@ItemExtraProcessType1 VARCHAR(MAX),@ItemExtraProcessRate1 VARCHAR(MAX),@ItemExtraProcessType2 VARCHAR(MAX),@ItemExtraProcessRate2 VARCHAR(MAX),@ItemRemark VARCHAR(MAX),@ItemColor VARCHAR(MAX),@CustName VARCHAR(MAX),@CustAddress VARCHAR(MAX),@CustRemarks VARCHAR(MAX),@CustPriority VARCHAR(MAX),@CustPhone VARCHAR(MAX),@BookingDate VARCHAR(MAX),@BookingDeliveryDate VARCHAR(MAX),@BookingDeliveryTime VARCHAR(MAX),@BookingRemarks VARCHAR(MAX),@Discount VARCHAR(MAX),@HomeDelivery VARCHAR(MAX),@CheckedByEmployee VARCHAR(MAX),@IsUrgent VARCHAR(MAX),@TotalCost VARCHAR(MAX),@NetAmount VARCHAR(MAX),@STPAmt VARCHAR(MAX),@STEP1Amt VARCHAR(MAX),@STEP2Amt VARCHAR(MAX),@CustCode VARCHAr(MAX),@ShopIn VARCHAR(MAX),@BookingCancelStatus int	,@DiscountAmt VARCHAR(MAX),@DiscountOption VARCHAR(MAX)
	DECLARE @Fill CURSOR
	SET @Fill = CURSOR FOR
		SELECT ItemName,ItemTotalQuantity,ItemProcessType,ItemQuantityAndRate,ItemExtraProcessType1 ,ItemExtraProcessRate1,ItemExtraProcessType2,ItemExtraProcessRate2,ItemRemark,ItemColor,STPAmt,STEP1Amt,STEP2Amt,ItemSubtotal from entbookingdetails where bookingnumber=@BOOKINGNUMBER
		OPEN @Fill
			FETCH NEXT
				FROM @Fill INTO @ItemName,@ItemTotalQuantity,@ItemProcessType,@ItemQuantityAndRate,@ItemExtraProcessType1,@ItemExtraProcessRate1,@ItemExtraProcessType2,@ItemExtraProcessRate2,@ItemRemark,@ItemColor,@STPAmt,@STEP1Amt,@STEP2Amt,@ItemSubtotal
				WHILE @@FETCH_STATUS = 0
					BEGIN
						select  @ShopIn= max (Statusid) from barcodetable where BookingNo=@BOOKINGNUMBER
						select  @BookingCancelStatus= BookingStatus from EntBookings where BookingNumber=@BOOKINGNUMBER
						SELECT @CustName=(dbo.CustomerMaster.CustomerSalutation + ' ' + dbo.CustomerMaster.CustomerName), @CustAddress=dbo.CustomerMaster.CustomerAddress,@CustPhone=dbo.CustomerMaster.CustomerMobile,@CustPriority= dbo.PriorityMaster.Priority,@CustRemarks= dbo.CustomerMaster.Remarks,@BookingDate=CONVERT(VARCHAR,dbo.EntBookings.BookingDate,106),@BookingDeliveryDate=CONVERT(VARCHAR,dbo.EntBookings.BookingDeliveryDate,106),@BookingDeliveryTime= dbo.EntBookings.BookingDeliveryTime,   @BookingRemarks=dbo.EntBookings.BookingRemarks,@Discount= dbo.EntBookings.Discount,@HomeDelivery=dbo.EntBookings.HomeDelivery,@CheckedByEmployee= dbo.EntBookings.CheckedByEmployee,@IsUrgent= dbo.EntBookings.IsUrgent,@TotalCost= dbo.EntBookings.TotalCost,@NetAmount=dbo.EntBookings.NetAmount,@CustCode=dbo.EntBookings.BookingByCustomer,@DiscountAmt= dbo.EntBookings.DiscountAmt,@DiscountOption=dbo.EntBookings.DiscountOption FROM dbo.EntBookings INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID WHERE (dbo.EntBookings.BookingNumber =@BOOKINGNUMBER)			
						SELECT @Advance=Sum(PaymentMade) FROM entpayment WHERE bookingnumber=@BOOKINGNUMBER
						INSERT INTO #TmpTable (ItemName,ItemTotalQuantity,ItemProcessType,ItemQuantityAndRate,ItemExtraProcessType1,ItemExtraProcessRate1,ItemExtraProcessType2,ItemExtraProcessRate2,ItemRemark,ItemColor,CustName,CustAddress,CustRemarks,CustPriority,CustPhone,BookingDate,BookingDeliveryDate,BookingDeliveryTime,BookingRemarks,Discount,HomeDelivery,CheckedByEmployee,IsUrgent,TotalCost,NetAmount,STPAmt,STEP1Amt,STEP2Amt,ItemSubtotal,Advance,CustCode,ShopIn,BookingCancelStatus,DiscountAmt,DiscountOption)
						VALUES (@ItemName,@ItemTotalQuantity,@ItemProcessType,@ItemQuantityAndRate,@ItemExtraProcessType1,@ItemExtraProcessRate1,@ItemExtraProcessType2,@ItemExtraProcessRate2,@ItemRemark,@ItemColor,@CustName,@CustAddress,@CustRemarks,@CustPriority,@CustPhone,@BookingDate,@BookingDeliveryDate,@BookingDeliveryTime,@BookingRemarks,@Discount,@HomeDelivery,@CheckedByEmployee,@IsUrgent,@TotalCost,@NetAmount,@STPAmt,@STEP1Amt,@STEP2Amt,@ItemSubtotal,@Advance,@CustCode,@ShopIn,@BookingCancelStatus,@DiscountAmt,@DiscountOption)					
						FETCH NEXT
							FROM @Fill INTO @ItemName,@ItemTotalQuantity,@ItemProcessType,@ItemQuantityAndRate,@ItemExtraProcessType1,@ItemExtraProcessRate1,@ItemExtraProcessType2,@ItemExtraProcessRate2,@ItemRemark,@ItemColor,@STPAmt,@STEP1Amt,@STEP2Amt,@ItemSubtotal
					END
		CLOSE @Fill
		DEALLOCATE @Fill
	SELECT  * FROM #TmpTable
	DROP TABLE #TmpTable
END
GO
/****** Object:  StoredProcedure [dbo].[sp_EditBooking_SaveProc]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <21-Nov-2011>
-- Description:	<EDIT DATA IN THE NEW BOOKING>
-- =============================================
CREATE PROCEDURE [dbo].[sp_EditBooking_SaveProc]	
	@BookingByCustomer VARCHAR(MAX)='',
    @BOOKINGNUMBER VARCHAR(MAX)='',
    @IsUrgent VARCHAR(MAX)='',
    @BookingDeliveryDate VARCHAR(MAX)='',
    @BookingDeliveryTime VARCHAR(MAX)='',
    @TotalCost VARCHAR(MAX)='',
    @Discount VARCHAR(MAX)='',
    @NetAmount VARCHAR(MAX)='',
    @BookingRemarks VARCHAR(MAX)='',
    @ItemTotalQuantity VARCHAR(MAX)='',
    @HomeDelivery VARCHAR(MAX)='',
    @CheckedByEmployee VARCHAR(MAX)='',
	@DiscountAmt VARCHAR(MAX)='',
	@DiscountOption VARCHAR(MAX)=''
AS
BEGIN		
	
	DECLARE @fltPreviousCustomerAmount FLOAT,@CustCode VARCHAR(MAX),@OldTotalAmount FLOAT
	SELECT @OldTotalAmount= NetAmount FROM dbo.EntBookings WHERE BookingNumber=@BOOKINGNUMBER
	SET @fltPreviousCustomerAmount=(SELECT COALESCE(SUM(PaymentMade),0) FROM EntPayment WHERE BookingNumber=@BOOKINGNUMBER)
	DELETE FROM EntPayment WHERE BookingNumber =@BOOKINGNUMBER
	DELETE FROM EntLedgerEntries WHERE Narration Like '%' +@BOOKINGNUMBER+ '%'
	IF(@fltPreviousCustomerAmount>0)
		BEGIN
			UPDATE LedgerMaster SET CurrentBalance=(CurrentBalance-@fltPreviousCustomerAmount) WHERE LedgerName='Cash'
			UPDATE LedgerMaster SET CurrentBalance=(CurrentBalance+@fltPreviousCustomerAmount) WHERE LedgerName=@BookingByCustomer
		END
	UPDATE LedgerMaster SET CurrentBalance=(CurrentBalance-@OldTotalAmount) Where LedgerName=@BookingByCustomer
	UPDATE LedgerMaster SET CurrentBalance=(CurrentBalance+@OldTotalAmount) Where LedgerName='Sales'
	UPDATE EntBookings SET BookingDeliveryDate=@BookingDeliveryDate, BookingDeliveryTime=@BookingDeliveryTime, BookingByCustomer=@BookingByCustomer, ISUrgent=@IsUrgent, TotalCost=@TotalCost, Discount=@Discount, NetAmount=@NetAmount, BookingRemarks=@BookingRemarks,HomeDelivery=@HomeDelivery,CheckedByEmployee=@CheckedByEmployee,DiscountAmt=@DiscountAmt,DiscountOption=@DiscountOption WHERE BookingNumber=@BOOKINGNUMBER
	DELETE FROM EntBookingDetails WHERE BookingNumber=@BOOKINGNUMBER
	DELETE FROM BarcodeTable WHERE BookingNo=@BOOKINGNUMBER
	DELETE FROM EntChallan WHERE BookingNumber=@BOOKINGNUMBER			
	SELECT @BOOKINGNUMBER AS BookingNumber
END
GO
/****** Object:  StoredProcedure [dbo].[sp_dry_NewChallan]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_dry_NewChallan]
	@Flag int=0,
	@BookingNo VARCHAR(MAX)='',
	@RowIndex int=null
	
AS 
BEGIN 
	IF(@Flag=1)
	 BEGIN
		SELECT dbo.EntBookings.BookingNumber, dbo.BarcodeTable.RowIndex AS ISN, dbo.BarcodeTable.Item AS SubItemName, dbo.BarcodeTable.Process AS ItemProcessType, CASE WHEN BarcodeTable.ItemExtraprocessType = '0' THEN '' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType1, dbo.EntBookings.IsUrgent, dbo.BarcodeTable.SNo AS ItemTotalQuantity FROM dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo WHERE (dbo.BarcodeTable.StatusId = '1') AND (dbo.EntBookings.BookingStatus <> '5') ORDER BY CONVERT(int, dbo.EntBookings.BookingNumber), ISN
	 END
	IF(@Flag=2)
	 BEGIN
		SELECT dbo.EntBookings.BookingNumber, dbo.BarcodeTable.RowIndex AS ISN, dbo.BarcodeTable.Item AS SubItemName, dbo.BarcodeTable.Process AS ItemProcessType, CASE WHEN BarcodeTable.ItemExtraprocessType = '0' THEN '' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType1, dbo.EntBookings.IsUrgent, dbo.BarcodeTable.SNo AS ItemTotalQuantity FROM dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo WHERE (dbo.BarcodeTable.StatusId = '1') AND (dbo.EntBookings.BookingStatus <> '5') AND dbo.BarcodeTable.BookingNo=@BookingNo  ORDER BY CONVERT(int, dbo.EntBookings.BookingNumber), ISN
	 END
	IF(@Flag=3)
	 BEGIN
		SELECT dbo.EntBookings.BookingNumber, dbo.BarcodeTable.RowIndex AS ISN, dbo.BarcodeTable.Item AS SubItemName, dbo.BarcodeTable.Process AS ItemProcessType, CASE WHEN BarcodeTable.ItemExtraprocessType = '0' THEN '' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType1, dbo.EntBookings.IsUrgent, dbo.BarcodeTable.SNo AS ItemTotalQuantity FROM dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo WHERE (dbo.BarcodeTable.StatusId = '1') AND (dbo.EntBookings.BookingStatus <> '5') AND dbo.BarcodeTable.BookingNo=@BookingNo AND dbo.BarcodeTable.RowIndex=@RowIndex  ORDER BY CONVERT(int, dbo.EntBookings.BookingNumber), ISN
	 END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Dry_EmployeeMaster]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Dry_EmployeeMaster]		
	@Flag VARCHAR(MAX)='',
	@ID int='',
	@EmployeeCode VARCHAR(MAX)='',
	@EmployeeSalutation VARCHAR(MAX)='',
	@EmployeeName VARCHAR(MAX)='',
	@EmployeeAddress  VARCHAR(MAX)='',
	@EmployeePhone VARCHAR(MAX)='',
	@EmployeeMobile VARCHAR(MAX)='',
	@EmployeeEmailId VARCHAR(MAX)='',
	@Status VARCHAR(MAX)='',
	@BookingNumber VARCHAR(MAX)='',
	@Remarks VARCHAR(MAX)='',
	@CustomerName  varchar(max)=''
AS
BEGIN
	IF(@Flag=1)
		BEGIN
			INSERT INTO dbo.EmployeeMaster
				(ID,EmployeeCode,EmployeeSalutation,EmployeeName,EmployeeAddress,EmployeePhone,EmployeeMobile,EmployeeEmailId)
			VALUES
				(@ID,@EmployeeCode,@EmployeeSalutation,@EmployeeName,@EmployeeAddress,@EmployeePhone,@EmployeeMobile,@EmployeeEmailId)
		END	
	ELSE IF (@Flag=2)
		BEGIN
			UPDATE EmployeeMaster Set EmployeeSalutation=@EmployeeSalutation,EmployeeName=@EmployeeName,EmployeeAddress=@EmployeeAddress,EmployeePhone=@EmployeePhone,EmployeeMobile=@EmployeeMobile,EmployeeEmailId=@EmployeeEmailId WHERE  EmployeeCode=@EmployeeCode			
		END
	ELSE IF (@Flag=3)
		BEGIN
			SELECT [ID], [EmployeeCode], COALESCE(EmployeeSalutation,'') + ' ' + [EmployeeName] As EmployeeName, [EmployeeAddress], [EmployeePhone], [EmployeeMobile], [EmployeeEmailId] FROM [EmployeeMaster] 
		END	
	ELSE IF (@Flag=4)
		BEGIN
			SELECT [ID], [EmployeeCode], COALESCE(EmployeeSalutation,'') + ' ' + [EmployeeName] As EmployeeName, [EmployeeAddress], [EmployeePhone], [EmployeeMobile], [EmployeeEmailId] FROM [EmployeeMaster] Where (EmployeeName Like @Status) OR (EmployeeAddress Like @Status) OR (EmployeePhone Like @Status) OR (EmployeeMobile Like @Status)
		END	
	ELSE IF (@Flag=5)
		BEGIN
			DELETE FROM dbo.EmployeeMaster WHERE EmployeeCode=@EmployeeCode
		END	
	ELSE IF (@Flag=6)
		BEGIN
			SELECT EmployeeCode,EmployeeName FROM dbo.EmployeeMaster
		END	
	ELSE IF (@Flag=7)
		BEGIN
			SELECT SUM(STPAmt) + SUM(Step1Amt) + SUM(Step2Amt) AS Servicetax FROM EntBookingDetails WHERE BookingNumber=@BookingNumber
		END
	ELSE IF (@Flag=8)
		BEGIN
			SELECT DeliveryMsg FROM dbo.EntPayment WHERE BookingNumber=@BookingNumber
		END	
	ELSE IF (@Flag=9)
		BEGIN
			UPDATE dbo.EntPayment SET DeliveryMsg=@Remarks WHERE BookingNumber=@BookingNumber 
		END	
	ELSE IF (@Flag=10)
		BEGIN
			SELECT 'Cash' AS PaymentType
			UNION
			SELECT 'Credit Card' AS PaymentType
			UNION
			SELECT 'Debit Card' AS PaymentType
			UNION
			SELECT 'Cheque/Bank' AS PaymentType
		END
	ELSE IF (@Flag=11)
		BEGIN
			SELECT BookingStatus FROM EntBookings where BookingNumber=@BookingNumber
		END			
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Dry_DrawlMaster]    Script Date: 12/26/2011 11:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Dry_DrawlMaster]		
	@Flag VARCHAR(MAX)='',
	@ID int='',
	@DrawlName VARCHAR(MAX)='',
	@ParentDrawl int='',
	@CustCode VARCHAR(MAX)='',
	@BookingNumber VARCHAR(MAX)='',
	@MsgStaus bit='',
	@MainDate smalldatetime='',
	@BookDate1 smalldatetime='',
	@BookingTime nvarchar(50)='',
	@Format varchar(10)='',
	@ItemCode VARCHAR(MAX)='',
	@SearchText VARCHAR(MAX)='',
	@challandate datetime='',
	@challanSendingShift varchar(max)='',
	@ChallanNumber varchar(max)	='',
	@RowIndex int=''
	
AS
BEGIN 
	IF(@Flag=1)
		BEGIN
			INSERT INTO dbo.mstDrwal
				(DrawlName,ParentDrawl)
			VALUES
				(@DrawlName,@ParentDrawl)
		END	
	ELSE IF(@Flag=2)
		BEGIN
			UPDATE dbo.mstDrwal SET DrawlName=@DrawlName,ParentDrawl=@ParentDrawl where Id=@ID
		END
	ELSE IF(@Flag=3)
		BEGIN
			SELECT * FROM  dbo.mstDrwal ORDER BY Id DESC
		END
	ELSE IF(@Flag=4)
		BEGIN
			SELECT * FROM  dbo.mstDrwal WHERE (DrawlName Like @DrawlName) ORDER BY Id DESC
		END
	ELSE IF(@Flag=5)
		BEGIN
			DELETE FROM  dbo.mstDrwal WHERE Id=@ID
		END
	ELSE IF(@Flag=6)
		BEGIN
			SELECT * FROM  dbo.mstDrwal WHERE DrawlName=@DrawlName AND ParentDrawl=@ParentDrawl  
		END
	ELSE IF(@Flag=7)
		BEGIN
			SELECT * FROM  dbo.mstDrawl 
		END
	ELSE IF(@Flag=8)
		BEGIN
			SELECT COALESCE(CustomerSalutation,'') + ' ' + [CustomerName] As CustomerName , CustomerMobile FROM  dbo.CustomerMaster where CustomerCode=@CustCode
		END
	ELSE IF(@Flag=9)
		BEGIN			
			DECLARE @SQL varchar(max), @PaymentMade float, @DiscountGiven float
	CREATE TABLE #TmpDeliveryInfo (BookingNumber varchar(10), BookingDate varchar(20), DeliveryDate varchar(20), CustomerCode varchar(10), CustomerName varchar(100), BookingAmount float, Discount float, NetAmount float, PaymentMade float, DuePayment float, DiscountOnPayment float)
	INSERT INTO #TmpDeliveryInfo (BookingNumber , CustomerCode , CustomerName , BookingDate, DeliveryDate, BookingAmount, Discount, NetAmount)
	 SELECT BookingNumber, BookingByCustomer, CustomerSalutation + ' '  + CustomerName As CustomerName, Convert(varchar,BookingDate,106) As BookingDate, Convert(varchar, BookingDeliveryDate, 106) + ' ' + BookingDeliveryTime As BookingDeliveryDate, TotalCost, Discount, NetAmount 
		FROM EntBookings LEFT JOIN CustomerMaster ON EntBookings.BookingByCustomer = CustomerMaster.CustomerCode
		WHERE BookingNumber = @BookingNumber 
	
	SELECT @PaymentMade = COALESCE(SUM(PaymentMade),0), @DiscountGiven = COALESCE(SUM(DiscountOnPayment),0) FROM EntPayment WHERE BookingNumber = @BookingNumber
	UPDATE #TmpDeliveryInfo SET PaymentMade = @PaymentMade + @DiscountGiven
	UPDATE #TmpDeliveryInfo SET DuePayment= COALESCE(NetAmount - PaymentMade,0), DiscountOnPayment = @DiscountGiven
	--Table(0)
	SELECT * FROM #TmpDeliveryInfo
	DROP TABLE #TmpDeliveryInfo
		END
	ELSE IF(@Flag=10)
		BEGIN	
		SELECT DeliveredQty FROM BarcodeTable where BookingNo=@BookingNumber
		END
	ELSE IF(@Flag=11)
		BEGIN	
		UPDATE EntPayment SET MsgStaus=@MsgStaus Where BookingNumber=@BookingNumber
		END
	ELSE IF(@Flag=12)
		BEGIN
		SELECT DISTINCT BookingNumber FROM entpayment WHERE MsgStaus='true'
		END
	ELSE IF(@Flag=13)
		BEGIN
		SELECT DISTINCT BookingNumber,convert(varchar, PaymentDate,106) as date from entpayment where MsgStaus='true' and bookingnumber=@BookingNumber order by date desc
		END
	ELSE IF(@Flag=14)
		BEGIN		
		set @MainDate= (SELECT convert(varchar, DATEADD(DAY, DATEDIFF(DAY, 0, @BookDate1), 0) + 3,106))		
		CREATE TABLE #TmpDate (BookingDate varchar(max))
		INSERT INTO #TmpDate (BookingDate) values (@MainDate)
		select * from #TmpDate
		drop table #TmpDate
		END
	ELSE IF(@Flag=15)
		BEGIN
		SELECT     dbo.CustomerMaster.CustomerCode
		FROM         dbo.EntBookings INNER JOIN
                      dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
		WHERE     (dbo.EntBookings.BookingNumber = @BookingNumber)
		END
	ELSE IF(@Flag=16)
		BEGIN
		SELECT CustomerMessage from MstConfigSettings
		END
	ELSE IF(@Flag=17)
		BEGIN
		UPDATE EntPayment SET MsgStaus=@MsgStaus Where BookingNumber=@BookingNumber
		END
	ELSE IF(@Flag=18)
		BEGIN
		SELECT * FROM ConfigurationSetting 
		END
	ELSE IF(@Flag=19)
		BEGIN
		SELECT Configuration FROM ConfigurationSetting WHERE Configuration='1'
		END
	ELSE IF(@Flag=20)
		BEGIN
		declare @date datetime
		declare @date1 varchar(50)
		set @date= CONVERT(VARCHAR(50),GETDATE(),108)
		set @date1=@date	
		CREATE TABLE #TmpDateTime (BookingDateTime varchar(max))
		INSERT INTO #TmpDateTime (BookingDateTime) values (@date1)
		select * from #TmpDateTime
		drop table #TmpDateTime		
		END
	ELSE IF(@Flag=21)
		BEGIN
		Update EntBookings set BookingTime=@BookingTime, Format=@Format where BookingNumber=@BookingNumber
		END
	ELSE IF(@Flag=22)
		BEGIN
		select ItemName from itemmaster where ItemCode=@ItemCode and ItemCode<>''
		END
	ELSE IF(@Flag=23)
		BEGIN
		select CustomerName from CustomerMaster where CustomerName=@SearchText 
		END
	ELSE IF(@Flag=24)
		BEGIN
		select CustomerMobile from CustomerMaster where CustomerMobile=@SearchText 
		END
	ELSE IF(@Flag=25)
		BEGIN
		select CustomerCode from CustomerMaster where CustomerName=@SearchText 
		END
	ELSE IF(@Flag=26)
		BEGIN
--		declare @challandate datetime
--		declare @ChallanNumber varchar(max)	
--		declare @challanSendingShift varchar(max)		
--		set @challanSendingShift='12 Noon'	
		set @challandate= CONVERT(VARCHAR(50),GETDATE(),106)	
		CREATE TABLE #TmpGetChallanNumber (ChallanNumber varchar(max))
		set @ChallanNumber= (select max(challannumber) from entchallan where challanSendingShift=@challanSendingShift and challandate=@challandate)
		if(@ChallanNumber<>'')
		begin		
			INSERT INTO #TmpGetChallanNumber (ChallanNumber) values (@ChallanNumber)
		end
		else
		begin
			set @ChallanNumber= (select max(challannumber)+ 1 from entchallan)
			INSERT INTO #TmpGetChallanNumber (ChallanNumber) values (@ChallanNumber)
		end
		if(@ChallanNumber is null)
		begin
			delete from #TmpGetChallanNumber
			set @ChallanNumber='1'
			INSERT INTO #TmpGetChallanNumber (ChallanNumber) values (@ChallanNumber)
		end
		select * from #TmpGetChallanNumber
		drop table #TmpGetChallanNumber	
		END
	ELSE IF(@Flag=27)
		BEGIN
		select convert(varchar,BookingDate,106) as BookingDate, convert(varchar,DueDate,106) as DueDate,ItemRemarks from barcodetable where bookingno=@BookingNumber and RowIndex=@RowIndex
		END
	ELSE IF(@Flag=28)
		BEGIN
		SELECT * FROM MstConfigSettings
		END
	ELSE IF(@Flag=29)
		BEGIN
			SELECT convert(varchar,NetAmount) As CustomerName ,Qty AS CustomerMobile FROM EntBookings WHERE BookingByCustomer=@CustCode and BookingNumber=@BookingNumber
			
			SELECT COALESCE(CustomerSalutation,'') + ' ' + [CustomerName] As CustomerName , CustomerMobile FROM  dbo.CustomerMaster where CustomerCode=@CustCode
			
		END
	END
GO
/****** Object:  Table [dbo].[EntVendorChallan]    Script Date: 12/26/2011 11:55:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EntVendorChallan](
	[ChallanNumber] [int] NOT NULL,
	[ChallanBranchCode] [varchar](5) NOT NULL,
	[ChallanDate] [datetime] NOT NULL,
	[ChallanSendingShift] [varchar](10) NOT NULL,
	[BookingNumber] [varchar](15) NOT NULL,
	[ItemSNo] [int] NOT NULL,
	[SubItemName] [varchar](50) NOT NULL,
	[ItemTotalQuantitySent] [int] NOT NULL,
	[ItemsReceivedFromVendor] [int] NOT NULL,
	[ItemReceivedFromVendorOnDate] [datetime] NULL,
	[Urgent] [bit] NOT NULL,
	[VendorId] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_BarcodeTable_DeliveredQty]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[BarcodeTable] ADD  CONSTRAINT [DF_BarcodeTable_DeliveredQty]  DEFAULT ('False') FOR [DeliveredQty]
GO
/****** Object:  Default [DF_BarcodeTable_DelQty]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[BarcodeTable] ADD  CONSTRAINT [DF_BarcodeTable_DelQty]  DEFAULT ('0') FOR [DelQty]
GO
/****** Object:  Default [DF_CustomerMaster_CustomerPriority]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[CustomerMaster] ADD  CONSTRAINT [DF_CustomerMaster_CustomerPriority]  DEFAULT ((0)) FOR [CustomerPriority]
GO
/****** Object:  Default [DF_CustomerMaster_CustomerRefferredBy]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[CustomerMaster] ADD  CONSTRAINT [DF_CustomerMaster_CustomerRefferredBy]  DEFAULT ('None') FOR [CustomerRefferredBy]
GO
/****** Object:  Default [DF_CustomerMaster_CustomerIsActive]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[CustomerMaster] ADD  CONSTRAINT [DF_CustomerMaster_CustomerIsActive]  DEFAULT ((1)) FOR [CustomerIsActive]
GO
/****** Object:  Default [DF_CustomerMaster_DefaultDiscountRate]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[CustomerMaster] ADD  CONSTRAINT [DF_CustomerMaster_DefaultDiscountRate]  DEFAULT ((0)) FOR [DefaultDiscountRate]
GO
/****** Object:  Default [DF_EntBookingDetails_EntBookings]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookingDetails] ADD  CONSTRAINT [DF_EntBookingDetails_EntBookings]  DEFAULT ((0)) FOR [BookingNumber]
GO
/****** Object:  Default [DF_EntBookingDetails_ISN]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookingDetails] ADD  CONSTRAINT [DF_EntBookingDetails_ISN]  DEFAULT ((1)) FOR [ISN]
GO
/****** Object:  Default [DF_EntBookingDetails_ItemName]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookingDetails] ADD  CONSTRAINT [DF_EntBookingDetails_ItemName]  DEFAULT ('None') FOR [ItemName]
GO
/****** Object:  Default [DF_EntBookingDetails_ItemTotalQuantity]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookingDetails] ADD  CONSTRAINT [DF_EntBookingDetails_ItemTotalQuantity]  DEFAULT ((0)) FOR [ItemTotalQuantity]
GO
/****** Object:  Default [DF_EntBookingDetails_ItemProcessType]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookingDetails] ADD  CONSTRAINT [DF_EntBookingDetails_ItemProcessType]  DEFAULT ('None') FOR [ItemProcessType]
GO
/****** Object:  Default [DF_EntBookingDetails_ItemColorAndQuantity]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookingDetails] ADD  CONSTRAINT [DF_EntBookingDetails_ItemColorAndQuantity]  DEFAULT ('None') FOR [ItemQuantityAndRate]
GO
/****** Object:  Default [DF_EntBookingDetails_ItemExtraProcessType1]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookingDetails] ADD  CONSTRAINT [DF_EntBookingDetails_ItemExtraProcessType1]  DEFAULT ('None') FOR [ItemExtraProcessType1]
GO
/****** Object:  Default [DF_EntBookingDetails_ItemExtraProcessRate1]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookingDetails] ADD  CONSTRAINT [DF_EntBookingDetails_ItemExtraProcessRate1]  DEFAULT ((0)) FOR [ItemExtraProcessRate1]
GO
/****** Object:  Default [DF_EntBookingDetails_ItemExtraProcessType2]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookingDetails] ADD  CONSTRAINT [DF_EntBookingDetails_ItemExtraProcessType2]  DEFAULT ('None') FOR [ItemExtraProcessType2]
GO
/****** Object:  Default [DF_EntBookingDetails_ItemExtraProcessRate2]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookingDetails] ADD  CONSTRAINT [DF_EntBookingDetails_ItemExtraProcessRate2]  DEFAULT ((0)) FOR [ItemExtraProcessRate2]
GO
/****** Object:  Default [DF_EntBookingDetails_ItemExtraProcessType3]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookingDetails] ADD  CONSTRAINT [DF_EntBookingDetails_ItemExtraProcessType3]  DEFAULT ('None') FOR [ItemExtraProcessType3]
GO
/****** Object:  Default [DF_EntBookingDetails_ItemExtraProcessRate3]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookingDetails] ADD  CONSTRAINT [DF_EntBookingDetails_ItemExtraProcessRate3]  DEFAULT ((0)) FOR [ItemExtraProcessRate3]
GO
/****** Object:  Default [DF_EntBookingDetails_ItemSubTotal]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookingDetails] ADD  CONSTRAINT [DF_EntBookingDetails_ItemSubTotal]  DEFAULT ((0)) FOR [ItemSubTotal]
GO
/****** Object:  Default [DF__EntBookin__Deliv__45F365D3]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookingDetails] ADD  CONSTRAINT [DF__EntBookin__Deliv__45F365D3]  DEFAULT ((0)) FOR [DeliveredQty]
GO
/****** Object:  Default [DF_EntBookings_TotalCost]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookings] ADD  CONSTRAINT [DF_EntBookings_TotalCost]  DEFAULT ((0)) FOR [TotalCost]
GO
/****** Object:  Default [DF_EntBookings_Discount]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookings] ADD  CONSTRAINT [DF_EntBookings_Discount]  DEFAULT ((0)) FOR [Discount]
GO
/****** Object:  Default [DF_EntBookings_NetAmount]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookings] ADD  CONSTRAINT [DF_EntBookings_NetAmount]  DEFAULT ((0)) FOR [NetAmount]
GO
/****** Object:  Default [DF_EntBookings_OrderStatus]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookings] ADD  CONSTRAINT [DF_EntBookings_OrderStatus]  DEFAULT ((0)) FOR [BookingStatus]
GO
/****** Object:  Default [DF_EntBookings_ItemTotalQuantity]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookings] ADD  CONSTRAINT [DF_EntBookings_ItemTotalQuantity]  DEFAULT ((0)) FOR [ItemTotalQuantity]
GO
/****** Object:  Default [DF_EntChallan_ChallanNumber]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntChallan] ADD  CONSTRAINT [DF_EntChallan_ChallanNumber]  DEFAULT ((0)) FOR [ChallanNumber]
GO
/****** Object:  Default [DF_EntChallan_ChallanSendingShift]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntChallan] ADD  CONSTRAINT [DF_EntChallan_ChallanSendingShift]  DEFAULT ((1)) FOR [ChallanSendingShift]
GO
/****** Object:  Default [DF_EntChallan_ItemsPendingToSend]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntChallan] ADD  CONSTRAINT [DF_EntChallan_ItemsPendingToSend]  DEFAULT ((0)) FOR [ItemTotalQuantitySent]
GO
/****** Object:  Default [DF_EntChallan_ItemReceivedFromVendor]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntChallan] ADD  CONSTRAINT [DF_EntChallan_ItemReceivedFromVendor]  DEFAULT ((0)) FOR [ItemsReceivedFromVendor]
GO
/****** Object:  Default [DF_EntChallan_Urgent]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntChallan] ADD  CONSTRAINT [DF_EntChallan_Urgent]  DEFAULT ((0)) FOR [Urgent]
GO
/****** Object:  Default [DF_EntMenuRights_ParentMenu]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntMenuRights] ADD  CONSTRAINT [DF_EntMenuRights_ParentMenu]  DEFAULT ('None') FOR [ParentMenu]
GO
/****** Object:  Default [DF_EntPayment_PaymentMade]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntPayment] ADD  CONSTRAINT [DF_EntPayment_PaymentMade]  DEFAULT ((0)) FOR [PaymentMade]
GO
/****** Object:  Default [DF_EntPayment_DiscountOnPayment]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntPayment] ADD  CONSTRAINT [DF_EntPayment_DiscountOnPayment]  DEFAULT ((0)) FOR [DiscountOnPayment]
GO
/****** Object:  Default [DF_EntPayment_DeliveryStatus]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntPayment] ADD  CONSTRAINT [DF_EntPayment_DeliveryStatus]  DEFAULT ('False') FOR [DeliveryStatus]
GO
/****** Object:  Default [DF_EntRecleaning_ItemSentForReclean]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntRecleaning] ADD  CONSTRAINT [DF_EntRecleaning_ItemSentForReclean]  DEFAULT ((0)) FOR [ItemSentForReclean]
GO
/****** Object:  Default [DF_EntRecleaning_ItemReceivedFromReclean]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntRecleaning] ADD  CONSTRAINT [DF_EntRecleaning_ItemReceivedFromReclean]  DEFAULT ((0)) FOR [ItemReceivedFromReclean]
GO
/****** Object:  Default [DF_EntVendorChallan_ChallanNumber]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntVendorChallan] ADD  CONSTRAINT [DF_EntVendorChallan_ChallanNumber]  DEFAULT ((0)) FOR [ChallanNumber]
GO
/****** Object:  Default [DF_EntVendorChallan_ChallanSendingShift]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntVendorChallan] ADD  CONSTRAINT [DF_EntVendorChallan_ChallanSendingShift]  DEFAULT ((1)) FOR [ChallanSendingShift]
GO
/****** Object:  Default [DF_EntVendorChallan_ItemTotalQuantitySent]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntVendorChallan] ADD  CONSTRAINT [DF_EntVendorChallan_ItemTotalQuantitySent]  DEFAULT ((0)) FOR [ItemTotalQuantitySent]
GO
/****** Object:  Default [DF_EntVendorChallan_ItemsReceivedFromVendor]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntVendorChallan] ADD  CONSTRAINT [DF_EntVendorChallan_ItemsReceivedFromVendor]  DEFAULT ((0)) FOR [ItemsReceivedFromVendor]
GO
/****** Object:  Default [DF_EntVendorChallan_Urgent]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntVendorChallan] ADD  CONSTRAINT [DF_EntVendorChallan_Urgent]  DEFAULT ((0)) FOR [Urgent]
GO
/****** Object:  Default [DF_ItemMaster_NumberOfSubItems]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[ItemMaster] ADD  CONSTRAINT [DF_ItemMaster_NumberOfSubItems]  DEFAULT ((1)) FOR [NumberOfSubItems]
GO
/****** Object:  Default [DF_LedgerMaster_LedgerOpenningBalance]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[LedgerMaster] ADD  CONSTRAINT [DF_LedgerMaster_LedgerOpenningBalance]  DEFAULT ((0)) FOR [OpenningBalance]
GO
/****** Object:  Default [DF_LedgerMaster_CurrentBalance]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[LedgerMaster] ADD  CONSTRAINT [DF_LedgerMaster_CurrentBalance]  DEFAULT ((0)) FOR [CurrentBalance]
GO
/****** Object:  Default [DF_ProcessMaster_ProcessUsedForVendorReport]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[ProcessMaster] ADD  CONSTRAINT [DF_ProcessMaster_ProcessUsedForVendorReport]  DEFAULT ((0)) FOR [ProcessUsedForVendorReport]
GO
/****** Object:  Default [DF_TmpChallan_ChallanNumber]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[TmpChallan] ADD  CONSTRAINT [DF_TmpChallan_ChallanNumber]  DEFAULT ((0)) FOR [ChallanNumber]
GO
/****** Object:  Default [DF_TmpChallan_ChallanSendingShift]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[TmpChallan] ADD  CONSTRAINT [DF_TmpChallan_ChallanSendingShift]  DEFAULT ((1)) FOR [ChallanSendingShift]
GO
/****** Object:  Default [DF_TmpChallan_ItemTotalQuantitySent]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[TmpChallan] ADD  CONSTRAINT [DF_TmpChallan_ItemTotalQuantitySent]  DEFAULT ((0)) FOR [ItemTotalQuantitySent]
GO
/****** Object:  Default [DF_TmpChallan_ItemsReceivedFromVendor]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[TmpChallan] ADD  CONSTRAINT [DF_TmpChallan_ItemsReceivedFromVendor]  DEFAULT ((0)) FOR [ItemsReceivedFromVendor]
GO
/****** Object:  Default [DF_TmpChallan_Urgent]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[TmpChallan] ADD  CONSTRAINT [DF_TmpChallan_Urgent]  DEFAULT ((0)) FOR [Urgent]
GO
/****** Object:  Default [DF_UserMaster_UserActive]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[UserMaster] ADD  CONSTRAINT [DF_UserMaster_UserActive]  DEFAULT ((1)) FOR [UserActive]
GO
/****** Object:  ForeignKey [FK_BarcodeTable_EntBookings]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[BarcodeTable]  WITH CHECK ADD  CONSTRAINT [FK_BarcodeTable_EntBookings] FOREIGN KEY([BookingNo])
REFERENCES [dbo].[EntBookings] ([BookingNumber])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BarcodeTable] CHECK CONSTRAINT [FK_BarcodeTable_EntBookings]
GO
/****** Object:  ForeignKey [FK_EntBookingDetails_EntBookings]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntBookingDetails]  WITH CHECK ADD  CONSTRAINT [FK_EntBookingDetails_EntBookings] FOREIGN KEY([BookingNumber])
REFERENCES [dbo].[EntBookings] ([BookingNumber])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EntBookingDetails] CHECK CONSTRAINT [FK_EntBookingDetails_EntBookings]
GO
/****** Object:  ForeignKey [FK_EntPayment_EntBookings]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntPayment]  WITH CHECK ADD  CONSTRAINT [FK_EntPayment_EntBookings] FOREIGN KEY([BookingNumber])
REFERENCES [dbo].[EntBookings] ([BookingNumber])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EntPayment] CHECK CONSTRAINT [FK_EntPayment_EntBookings]
GO
/****** Object:  ForeignKey [FK_EntVendorChallan_mstVendor]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[EntVendorChallan]  WITH CHECK ADD  CONSTRAINT [FK_EntVendorChallan_mstVendor] FOREIGN KEY([VendorId])
REFERENCES [dbo].[mstVendor] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[EntVendorChallan] CHECK CONSTRAINT [FK_EntVendorChallan_mstVendor]
GO
/****** Object:  ForeignKey [FK_mstVendor_mstJobType]    Script Date: 12/26/2011 11:55:20 ******/
ALTER TABLE [dbo].[mstVendor]  WITH NOCHECK ADD  CONSTRAINT [FK_mstVendor_mstJobType] FOREIGN KEY([JobType])
REFERENCES [dbo].[mstJobType] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[mstVendor] CHECK CONSTRAINT [FK_mstVendor_mstJobType]
GO
