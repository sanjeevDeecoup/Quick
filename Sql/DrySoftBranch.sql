SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_ChallanReturnDetailsForPrint]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <6 Aug 2010>
-- Description:	<To select Challan Details for challan return>
-- =============================================
EXEC Sp_Sel_ChallanReturnDetailsForPrint ''1431'',''1450'',''''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_ChallanReturnDetailsForPrint]
	(
		@BookNumberFrom varchar(15) = '''',
		@BookNumberUpto varchar(10) = '''',
		@ChallanShift varchar(10) = ''''
	)
AS
BEGIN
	CREATE TABLE #TmpChallanForPrint(BookingNumber int, SubItemName varchar(30), ItemProcessType varchar(50), ItemToalQunatitySent int)
	DECLARE @SQL varchar(max)
	SET @SQL = ''INSERT INTO #TmpChallanForPrint (BookingNumber, SubItemName, ItemProcessType, ItemToalQunatitySent) SELECT EC.BookingNumber, SubItemName, ItemProcessType, SUM(ItemTotalQuantitySent) FROM EntChallan EC INNER JOIN EntBookingDetails EBD ON EC.BookingNumber = EBD.BookingNumber AND EC.ItemSNo = EBD.ISN''
	SET @SQL = @SQL + '' WHERE (EC.ItemsReceivedFromVendor < EC.ItemTotalQuantitySent)''
	IF(@BookNumberFrom <> '''')
		BEGIN
			IF(@BookNumberUpto = '''')
				BEGIN
					SET @BookNumberUpto = @BookNumberFrom
				END
			SET @SQL = @SQL + '' AND (EC.BookingNumber BETWEEN '' + @BookNumberFrom + '' AND '' + @BookNumberUpto + '')''
		END
	IF (@ChallanShift <> '''')
		BEGIN
			SET @SQL = @SQL + '' AND ChallanSendingShift = '''''' + @ChallanShift + ''''''''
		END
	SET @SQL = @SQL + '' Group By EC.BookingNumber, SubItemName, ItemProcessType''
	SET @SQL = @SQL + '' ORDER BY EC.BookingNumber''
	PRINT @SQL
	EXEC (@SQL)
	SELECT DISTINCT BookingNumber FROM #TmpChallanForPrint Order by BookingNumber
	SELECT BookingNumber, SUM(ItemToalQunatitySent) AS ItemTotalQuantitySent, SubItemName FROM #TmpChallanForPrint 
	WHERE ItemProcessType <>''None''
	GROUP BY BookingNumber, SubItemName	
	ORDER BY BookingNumber, SubItemName
	
	SELECT BookingNumber, SUM(ItemToalQunatitySent) AS ItemTotalQuantitySent, SubItemName FROM #TmpChallanForPrint 
	WHERE ItemProcessType =''None''
	GROUP BY BookingNumber, SubItemName	
	ORDER BY BookingNumber, SubItemName
	DROP TABLE  #TmpChallanForPrint
END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_TimeWiseClothBookingReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport ''1 SEP 2010'',''2 sep 2010''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_TimeWiseClothBookingReport]
	(
		@BookDate1 datetime,
		@BookDate2 datetime,
		@BookingTime varchar(max),		
		@BookingTime1 nvarchar(50)='''',
		@Format varchar(10)=''''
	)
AS
BEGIN
	SELECT dbo.BarcodeTable.Item, SUM(dbo.BarcodeTable.SNo) AS Qty FROM dbo.BarcodeTable INNER JOIN
           dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN
           dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
	WHERE (dbo.EntBookings.BookingTime BETWEEN @BookingTime AND @BookingTime1) AND (dbo.EntBookings.Format = @Format) AND (dbo.BarcodeTable.BookingDate BETWEEN @BookDate1 AND @BookDate2)
	GROUP BY dbo.BarcodeTable.SNo, dbo.BarcodeTable.Item
END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_DayBookReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'




/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Challan details>
-- =============================================
EXEC Sp_Sel_DayBookReport ''1 Oct 2010'', ''30 Oct 2010''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_DayBookReport]
	(
		@BookingDate1 datetime,
		@BookingDate2 datetime,
		@BranchId Varchar(max)
	)
AS
BEGIN
	CREATE TABLE #TmpTable (ID int identity (1,1),BookingDate DateTime, BookingNumber varchar(20), DC varchar(200), WC varchar(200))
	INSERT INTO #TmpTable( BookingDate, BookingNumber, DC, WC)
		SELECT BookingDate, BookingNumber, DC, WC
		FROM (SELECT DISTINCT dbo.EntPayment.PaymentDate AS BookingDate, dbo.EntPayment.BookingNumber, dbo.EntPayment.PaymentMade AS DC, '''' AS WC
FROM         dbo.EntBookings INNER JOIN
                      dbo.EntBookingDetails ON dbo.EntBookings.BookingNumber = dbo.EntBookingDetails.BookingNumber INNER JOIN
                      dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber
				WHERE     (dbo.EntBookingDetails.ItemProcessType = ''DC'') AND dbo.EntPayment.PaymentMade<>0 AND dbo.EntBookings.BranchId=@BranchId
				UNION
				SELECT dbo.EntPayment.PaymentDate as BookingDate, EntPayment.BookingNumber, '''' AS DC,
					CASE WHEN ItemProcessType = ''None'' THEN Convert(varchar,(EntPayment.PaymentMade)) ELSE EntPayment.PaymentMade END FROM EntBookings INNER JOIN EntBookingDetails ON EntBookings.BookingNumber = EntBookingDetails.BookingNumber
					INNER JOIN  dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber
					
				WHERE EntBookingDetails.ItemProcessType <> ''DC'' AND dbo.EntPayment.PaymentMade<>0 AND dbo.EntBookings.BranchId=@BranchId ) AS T1 
				
		WHERE BookingDate BETWEEN @BookingDate1 AND @BookingDate2 

	UPDATE #TmpTable SET DC = ''0'' WHERE DC ='''' OR DC IS NULL
	UPDATE #TmpTable SET DC = DC + '';''-- WHERE CHARINDEX(''@'',DC)>0
	UPDATE #TmpTable SET WC = ''0'' WHERE WC ='''' OR WC IS NULL
	UPDATE #TmpTable SET WC = WC + '';''-- WHERE CHARINDEX(''@'',WC)>0
	--SELECT * FROM #TmpTable
	DECLARE @ID int, @BN varchar(10), @DC varchar(200), @WC varchar(200), @ItemExtra1 float, @ItemExtra2 float, @Qty FLOAT, @Rate FLOAT, @Total float, @Tmp varchar(200)
	SELECT @ID = 0, @BN='''', @DC='''', @WC = '''', @ItemExtra1 = '''', @ItemExtra2 = '''', @Qty = 0, @Rate = 0, @Total = 0, @Tmp = ''''
	DECLARE CURBN CURSOR
	FOR
		SELECT ID, BookingNumber, DC, WC FROM #TmpTable
	OPEN CURBN
		FETCH FROM CURBN INTO @ID, @BN, @DC, @WC
		WHILE @@FETCH_STATUS = 0
			BEGIN
				IF (LEN(COALESCE(@DC,''''))>0)
					BEGIN
						SET @Total = 0
						PRINT CHARINDEX('';'',@DC)
						WHILE (CHARINDEX('';'',@DC)>0)
							BEGIN
								SET @TMP = '''' + SUBSTRING (@DC,1,CHARINDEX('';'',@DC)-1)
								IF(CHARINDEX(''@'',@TMP)>0)
									BEGIN
										SET @Qty = CONVERT(FLOAT,SUBSTRING(@TMP,1, CHARINDEX(''@'', @TMP)-1))
										SET @Rate = CONVERT(FLOAT,SUBSTRING(@TMP,CHARINDEX(''@'', @TMP)+1, LEN(@Tmp)))
									END
								ELSE
									BEGIN
										SET @QTY = 1
										SET @Rate = @Tmp
									END
								SET @TOTAL = @Total + (@Qty * @Rate)
								SET @DC = SUBSTRING(@DC, CHARINDEX('';'', @DC)+1, LEN(@DC))
							END
						UPDATE #TmpTable SET DC = @Total WHERE ID = @ID
						UPDATE #TmpTable SET WC = COALESCE(@ItemExtra1,0) + COALESCE(@ItemExtra2,0)  WHERE ID = @ID
					END
				IF (@WC <> '''')
					BEGIN
						SET @Total = 0
						PRINT CHARINDEX('';'',@WC)
						WHILE (CHARINDEX('';'',@WC)>0)
							BEGIN
								SET @TMP = '''' + SUBSTRING (@WC,1,CHARINDEX('';'',@WC)-1) print @Tmp 
								IF(CHARINDEX(''@'',@TMP)>0)
									BEGIN
										SET @Qty = CONVERT(FLOAT,SUBSTRING(@TMP,1, CHARINDEX(''@'', @TMP)-1))
										SET @Rate = CONVERT(FLOAT,SUBSTRING(@TMP,CHARINDEX(''@'', @TMP)+1, LEN(@Tmp)-CHARINDEX(''@'', @TMP)))
									END
								ELSE
									BEGIN
										SET @QTY = 1
										SET @Rate = @Tmp
									END
								PRINT @QTY PRINT @Rate
								SET @TOTAL = @Total + (@Qty * @Rate) 
								SET @WC = SUBSTRING(@WC, CHARINDEX('';'', @WC)+1, LEN(@WC)- CHARINDEX('';'', @WC)) PRINT @WC
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




' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_BookingReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport ''1 SEP 2010'',''2 sep 2010''
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
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_BookingDetailsReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Booking Details for>
-- =============================================
EXEC Sp_Sel_BookingDetailsForReceipt ''2''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_BookingDetailsReport]
	(
		@BookingNumber varchar(10)=''''
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
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_BookingDetailsForEditing]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Booking Details for>
-- =============================================
EXEC Sp_Sel_BookingDetailsForEditing ''21452''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_BookingDetailsForEditing]
	(
		@BookingNumber varchar(10)=''''
	)
AS
BEGIN
	DECLARE @SQL varchar(max), @PaymentMade float, @DiscountGiven float, @CustCode varchar(10)
	DECLARE @TotNetAmount float, @TotPaymentMade float
	SET @CustCode = ''''
	SELECT @CustCode= BookingByCustomer FROM EntBookings WHERE BookingNumber = @BookingNumber
	CREATE TABLE #TmpDeliveryInfo (BookingNumber varchar(10), BookingDate varchar(20), DeliveryDate varchar(20), CustomerCode varchar(10), CustomerName varchar(100), CustomerAddress varchar(100), CustomerPhone varchar(50), CustomerPriority varchar(50), BookingAmount float, Discount float, NetAmount float, BookingRemarks varchar(200), PaymentMade float, DuePayment float, DiscountOnPayment float, ISUrgent bit, PrevDue float,HomeDelivery bit,CheckedByEmployee varchar(50))
	INSERT INTO #TmpDeliveryInfo (BookingNumber , CustomerCode , CustomerName, CustomerAddress, CustomerPhone, CustomerPriority, BookingDate, DeliveryDate, BookingAmount, Discount, NetAmount, BookingRemarks, ISUrgent,HomeDelivery,CheckedByEmployee)
	 SELECT BookingNumber, BookingByCustomer, CustomerSalutation + '' ''  + CustomerName As CustomerName, CustomerAddress, CustomerPhone, Priority, Convert(varchar,BookingDate,103) As BookingDate, Convert(varchar, BookingDeliveryDate, 103) As BookingDeliveryDate, TotalCost, Discount, NetAmount, BookingRemarks, ISUrgent ,HomeDelivery,CheckedByEmployee
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
	SELECT Convert(varchar,PaymentDate,100) As ''Paid On'', PaymentMade As ''Payment'' FROM EntPayment WHERE BookingNumber = @BookingNumber
	
	DROP TABLE #TmpDeliveryInfo
END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Sel_BookingRecordsForBulkModification]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <25 Sep 2010>
-- Description:	<To select records for booking Bulk modification>
-- =============================================
EXEC SP_Sel_BookingRecordsForBulkModification ''1 sep 2010'', ''1 Oct 2012''
*/
CREATE PROCEDURE [dbo].[SP_Sel_BookingRecordsForBulkModification]
	(
		@BookingDate1 datetime='''',
		@BookingDate2 datetime=''''
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

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_AreaWiseClothBookingReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport ''1 SEP 2010'',''2 sep 2010''
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
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_PaymentReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Challan details>
-- =============================================
EXEC Sp_Sel_PaymentReport ''1 JUL 2010'', ''1 NOV 2010'' 
*/
CREATE PROCEDURE [dbo].[Sp_Sel_PaymentReport]
	(
		@PaymentDate1 datetime,
		@PaymentDate2 datetime
	)
AS
BEGIN
	CREATE TABLE #TmpTable (PaymentDate varchar(20), BookingNumber varchar(20), PaymentMade float , BalanceAmount float,CustName VARCHAR(MAX))
	INSERT INTO #TmpTable(PaymentDate, BookingNumber, PaymentMade, BalanceAmount,CustName)
		SELECT CONVERT(varchar, dbo.EntPayment.PaymentDate, 106) AS PaymentDate, dbo.EntBookings.BookingNumber, SUM(dbo.EntPayment.PaymentMade) AS Received, dbo.EntBookings.NetAmount - SUM(dbo.EntPayment.PaymentMade) AS Balance, dbo.CustomerMaster.CustomerName
		FROM dbo.EntBookings INNER JOIN  dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode LEFT OUTER JOIN  dbo.EntPayment ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber
		WHERE PaymentDate BETWEEN @PaymentDate1 AND @PaymentDate2 and PaymentMade<>0
		GROUP BY Convert(varchar,PaymentDate,106), EntBookings.BookingNumber,Entbookings.NetAmount, dbo.CustomerMaster.CustomerName
	SELECT * FROM #TmpTable
	DROP TABLE #TmpTable
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConfigurationSetting]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConfigurationSetting](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WebsiteName] [varchar](100) NULL,
	[StoreName] [varchar](100) NULL,
	[Address] [varchar](100) NULL,
	[Timing] [varchar](100) NULL,
	[FooterName] [varchar](100) NULL,
	[Printing] [bit] NULL,
	[Configuration] [bit] NULL,
	[SetSlipInch] [int] NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_StockReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_StockReport 1,''2 Piece''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_StockReport]
	(
		@Flag int =0,
		@ItemName varchar(Max)=''''
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
			SELECT bookingno as BookingNumber , SUM(SNO) AS ItemsReceived, SUM(DelQty) As Delivered,CONVERT(VARCHAR,DueDate,106) AS DueDate
			FROM barcodeTable
			WHERE Item = @ItemName
			Group By bookingno, Item,DueDate
			ORDER BY Item
		END
END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_ReportVendorChallanReturnDetails]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <6 Aug 2010>
-- Description:	<To select Challan Details for challan return>
-- =============================================
EXEC Sp_Sel_ReportVendorChallanReturnDetails ''1/1/2011'',''1/30/2011'','''','''',''''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_ReportVendorChallanReturnDetails]
	(
		@BookDate1 datetime,
		@BookDate2	datetime,
		@BookNumberFrom varchar(15) = '''',
		@BookNumberUpto varchar(10) = '''',
		@ChallanShift varchar(10) = '''',
		@VendorId varchar(15) = ''''
	)
AS
BEGIN
	DECLARE @SQL varchar(max)
	SET @SQL = ''SELECT EntVendorChallan.ChallanNumber, EntVendorChallan.ChallanBranchCode, convert(varchar,EntVendorChallan.ChallanDate,106) as ChallanDate, EntBookingDetails.BookingNumber, EntBookingDetails.ISN, SubItemName, CASE WHEN EntBookingDetails.ItemProcessType = ''''None'''' THEN '''''''' ELSE EntBookingDetails.ItemProcessType END As ItemProcessType, CASE WHEN EntBookingDetails.ItemExtraProcessType1= ''''None'''' THEN '''''''' WHEN EntBookingDetails.ItemProcessType = ''''None'''' THEN ''''O'''' + EntBookingDetails.ItemExtraProcessType1 ELSE EntBookingDetails.ItemExtraProcessType1 END AS ItemExtraProcessType1, CASE WHEN EntBookingDetails.ItemExtraProcessType2= ''''None'''' THEN '''''''' WHEN EntBookingDetails.ItemProcessType = ''''None'''' THEN ''''O'''' + EntBookingDetails.ItemExtraProcessType2 ELSE EntBookingDetails.ItemExtraProcessType2 END AS ItemExtraProcessType2, EntBookingDetails.ItemQuantityAndRate, EntVendorChallan.ItemTotalQuantitySent, ItemsReceivedFromVendor, (ItemTotalQuantitySent - ItemsReceivedFromVendor) AS ItemsPending, EntVendorChallan.ItemReceivedFromVendorOnDate, EntVendorChallan.Urgent FROM EntVendorChallan INNER JOIN EntBookingDetails ON EntVendorChallan.BookingNumber = EntBookingDetails.BookingNumber AND EntVendorChallan.ItemSNo = EntBookingDetails.ISN INNER JOIN ItemMaster ON EntBookingDetails.ItemName = ItemMaster.ItemName''
	SET @SQL = @SQL + '' WHERE (EntVendorChallan.ChallanDate BETWEEN '''''' + Convert(varchar,@BookDate1,106) + '''''' AND '''''' + Convert(varchar,@BookDate2,106) + '''''')''
	SET @SQL = @SQL + '' AND EntVendorChallan.VendorId = '''''' + @VendorId + ''''''''
	print(@BookNumberFrom)
	IF(@BookNumberFrom <> '''')
		BEGIN
			IF(@BookNumberUpto = '''')
				BEGIN
					SET @BookNumberUpto = @BookNumberFrom
				END
			SET @SQL = @SQL + '' AND (EntVendorChallan.BookingNumber BETWEEN '' + @BookNumberFrom + '' AND '' + @BookNumberUpto + '')''
		END
	IF (@ChallanShift <> '''')
		BEGIN
			SET @SQL = @SQL + '' AND ChallanSendingShift = '''''' + @ChallanShift + ''''''''
		END
	SET @SQL = @SQL + '' ORDER BY EntBookingDetails.BookingNumber, EntBookingDetails.ISN''
	PRINT @SQL
	EXEC (@SQL)
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_SalesLedgerReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Challan details>
-- =============================================
EXEC Sp_Sel_DayBookReport ''14 SEP 2010'', ''14 SEP 2010''
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
		WHERE PaymentDate BETWEEN @PaymentDate1 AND @PaymentDate2 AND PaymentMade<>0
		GROUP BY Convert(varchar,PaymentDate,106)
	SELECT PaymentDate, PaymentMade FROM #TmpTable

	DELETE FROM #TmpTable

	INSERT INTO #TmpTable(PaymentDate, BookingNumber, PaymentMade)
		SELECT Convert(varchar,PaymentDate,106), EntBookings.BookingNumber, SUM(PaymentMade) As Received FROM EntPayment INNER JOIN EntBookings ON EntPayment.BookingNumber = EntBookings.BookingNumber
		WHERE PaymentDate BETWEEN @PaymentDate1 AND @PaymentDate2 AND PaymentMade<>0
		GROUP BY Convert(varchar,PaymentDate,106), EntBookings.BookingNumber
	SELECT PaymentDate, BookingNumber, PaymentMade FROM #TmpTable
	
	DROP TABLE #TmpTable
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_BindGrid]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

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
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_SecondTimeChallanReturnDetails]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <6 Aug 2010>
-- Description:	<To select Challan Details for challan return>
-- =============================================
EXEC Sp_Sel_ChallanReturnDetails ''1431'',''1440'',''''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_SecondTimeChallanReturnDetails]
	(
		@BookNumberFrom varchar(15) = '''',
		@BookNumberUpto varchar(10) = '''',
		@ChallanShift varchar(10) = '''',
		@SerialNo varchar(10) = '''',
		@BranchId varchar(15)=''''
	)
AS 
BEGIN
	DECLARE @SQL varchar(max)
	SET @SQL = ''SELECT dbo.BarcodeTable.BookingNo AS BookingNumber, dbo.BarcodeTable.Item AS SubItemName, dbo.EntChallan.Urgent, dbo.BarcodeTable.Process AS ItemProcessType, CASE WHEN BarcodeTable.ItemExtraprocessType = ''''0'''' THEN '''''''' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType1, CASE WHEN BarcodeTable.ItemExtraprocessType = ''''0'''' THEN '''''''' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType2, dbo.EntChallan.ItemTotalQuantitySent, dbo.EntChallan.ItemsReceivedFromVendor, dbo.EntChallan.ItemTotalQuantitySent - dbo.EntChallan.ItemsReceivedFromVendor AS ItemsPending, dbo.BarcodeTable.RowIndex AS ISN FROM  dbo.EntChallan INNER JOIN  dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex''
	SET @SQL = @SQL + '' WHERE (dbo.BarcodeTable.StatusId=''''2'''') AND (dbo.BarcodeTable.BranchId = '' + @BranchId + '')''
	print(@BookNumberFrom)
	IF(@BookNumberFrom <> '''')
		BEGIN
			IF(@BookNumberUpto = '''')
				BEGIN
					SET @BookNumberUpto = @BookNumberFrom
				END
			SET @SQL = @SQL + '' AND (EntChallan.BookingNumber = '' + @BookNumberFrom + '') AND (BarcodeTable.RowIndex = '' + @BookNumberUpto + '')''
		END		
	SET @SQL = @SQL + '' ORDER BY BarcodeTable.BookingNo, BarcodeTable.RowIndex''
	PRINT @SQL
	EXEC (@SQL)
END



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_smsTimeChecking]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <6 Aug 2010>
-- Description:	<To select Challan Details for challan return>
-- =============================================
EXEC Sp_Sel_ChallanReturnDetails ''1431'',''1440'',''''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_smsTimeChecking]
	(
		@BookNumberFrom varchar(15) = '''',
		@BookNumberUpto varchar(10) = '''',
		@ChallanShift varchar(10) = '''',
		@SerialNo varchar(10) = '''',
		@BranchId VARCHAR(MAX)=''''
	)
AS
BEGIN
	DECLARE @SQL varchar(max)	
	SET @SQL = ''SELECT dbo.BarcodeTable.BookingNo AS BookingNumber, dbo.BarcodeTable.Item AS SubItemName, dbo.EntChallan.Urgent, dbo.BarcodeTable.Process AS ItemProcessType, CASE WHEN BarcodeTable.ItemExtraprocessType = ''''0'''' THEN '''''''' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType1, CASE WHEN BarcodeTable.ItemExtraprocessType = ''''0'''' THEN '''''''' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType2, dbo.EntChallan.ItemTotalQuantitySent, dbo.EntChallan.ItemsReceivedFromVendor, dbo.EntChallan.ItemTotalQuantitySent - dbo.EntChallan.ItemsReceivedFromVendor AS ItemsPending, dbo.BarcodeTable.RowIndex AS ISN FROM  dbo.EntChallan INNER JOIN  dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex''
	SET @SQL = @SQL + '' WHERE (EntChallan.ItemsReceivedFromVendor < EntChallan.ItemTotalQuantitySent) AND (Barcodetable.BranchId = '' + @BookNumberFrom + '')''
	print(@BookNumberFrom)
	IF(@BookNumberFrom <> '''')
		BEGIN
			IF(@BookNumberUpto = '''')
				BEGIN
					SET @BookNumberUpto = @BookNumberFrom
				END
			SET @SQL = @SQL + '' AND (EntChallan.BookingNumber = '' + @BookNumberFrom + '')''
		END		
	SET @SQL = @SQL + '' ORDER BY BarcodeTable.BookingNo, BarcodeTable.RowIndex''
	PRINT @SQL
	EXEC (@SQL)
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ShiftMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ShiftMaster](
	[ShiftID] [int] IDENTITY(0,1) NOT NULL,
	[ShiftName] [varchar](100) NOT NULL,
	[BranchId] [varchar](10) NOT NULL CONSTRAINT [DF__ShiftMast__Branc__753864A1]  DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_ShiftMaster] PRIMARY KEY CLUSTERED 
(
	[ShiftName] ASC,
	[BranchId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_VendorReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <7 August 2010>
-- Description:	<To select data for vendor report>
-- =============================================
EXEC Sp_Report_VendorReport ''1/1/2011'',''1/30/2011'','''',''''
*/
CREATE PROCEDURE [dbo].[Sp_VendorReport]
	(
		@BookDate1 datetime,
		@BookDate2	datetime,
		@ProcessCode varchar(max)= '''',
		@ItemName varchar(max) = ''''
	)
AS
BEGIN
	DECLARE @SQL varchar(max)
	SET @SQL =''SELECT BookingDate, BookingNumber, SUM(ProcessCost) AS ProcessCost, Pieces
	FROM
	(
SELECT     CONVERT(varchar, dbo.EntBookings.BookingDate, 106) AS BookingDate, dbo.EntBookings.BookingNumber,dbo.EntBookingDetails.ItemQuantityAndRate, 
			dbo.EntBookingDetails.ItemTotalQuantity * dbo.ItemMaster.NumberOfSubItems AS Pieces,dbo.EntBookingDetails.ItemName,         
            dbo.EntBookingDetails.ItemSubTotal as ProcessCost,dbo.EntBookingDetails.ItemProcessType                        
FROM         dbo.EntBookings INNER JOIN
             dbo.EntBookingDetails ON dbo.EntBookings.BookingNumber = dbo.EntBookingDetails.BookingNumber INNER JOIN
             dbo.ItemMaster ON dbo.EntBookingDetails.ItemName = dbo.ItemMaster.ItemName INNER JOIN
             dbo.ProcessMaster ON dbo.EntBookingDetails.ItemProcessType = dbo.ProcessMaster.ProcessCode
	WHERE (dbo.ProcessMaster.ProcessUsedForVendorReport=''''1'''') and (BookingDate BETWEEN '''''' + Convert(varchar,@BookDate1,106) + '''''' AND '''''' + Convert(varchar,@BookDate2,106) + '''''')''
	IF(@ProcessCode <> '''')
		BEGIN			
			SET @ProcessCode = REPLACE(@ProcessCode,'','','''''','''''')
			SET @SQL = @SQL + '' AND (ItemProcessType IN ('''''' + @ProcessCode + ''''''))''
		END
	IF(@ItemName <>'''')
		BEGIN
			SET @ItemName = REPLACE(@ItemName,'','','''''','''''')
			SET @SQL = @SQL + '' AND (EntBookingDetails.ItemName IN (''''''+ @ItemName +''''''))''
		END
	SET @SQL = @SQL + '') AS T1''
	SET @SQL = @SQL + ''	Group By BookingDate, BookingNumber, Pieces ''
	
	PRINT @SQL
	EXEC (@SQL)
	
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Split]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Split]
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
END' 
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnSplitString]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fnSplitString](@str nvarchar(max),@sep nvarchar(max))
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
END;' 
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mstRemark]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[mstRemark](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Remarks] [varchar](100) NOT NULL,
	[BranchId] [varchar](10) NOT NULL CONSTRAINT [DF__mstRemark__Branc__7167D3BD]  DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_mstRemark] PRIMARY KEY CLUSTERED 
(
	[Remarks] ASC,
	[BranchId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_mstRemark] UNIQUE NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Sel_RecForItemIdUpdate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/*
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
EXEC SP_Sel_RecForItemIdUpdate ''1''
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
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mstRecordCheck]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[mstRecordCheck](
	[Status] [varchar](50) NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomerMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CustomerMaster](
	[ID] [int] NOT NULL,
	[CustomerCode] [varchar](20) NOT NULL,
	[CustomerSalutation] [varchar](10) NOT NULL,
	[CustomerName] [varchar](50) NOT NULL,
	[CustomerAddress] [varchar](100) NOT NULL,
	[CustomerPhone] [varchar](max) NULL,
	[CustomerMobile] [varchar](max) NULL,
	[CustomerEmailId] [varchar](100) NULL,
	[CustomerPriority] [int] NOT NULL CONSTRAINT [DF_CustomerMaster_CustomerPriority]  DEFAULT ((0)),
	[CustomerRefferredBy] [varchar](50) NULL CONSTRAINT [DF_CustomerMaster_CustomerRefferredBy]  DEFAULT ('None'),
	[CustomerRegisterDate] [datetime] NULL,
	[CustomerIsActive] [bit] NULL CONSTRAINT [DF_CustomerMaster_CustomerIsActive]  DEFAULT ((1)),
	[CustomerCancelDate] [smalldatetime] NULL,
	[DefaultDiscountRate] [int] NULL CONSTRAINT [DF_CustomerMaster_DefaultDiscountRate]  DEFAULT ((0)),
	[Remarks] [varchar](100) NULL,
	[BirthDate] [smalldatetime] NULL,
	[AnniversaryDate] [smalldatetime] NULL,
	[AreaLocation] [varchar](100) NULL,
	[BranchId] [varchar](10) NOT NULL CONSTRAINT [DF__CustomerM__Branc__67DE6983]  DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_CustomerMaster] PRIMARY KEY CLUSTERED 
(
	[CustomerCode] ASC,
	[BranchId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_VendorChallanReturnDetails]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <6 Aug 2010>
-- Description:	<To select Challan Details for challan return>
-- =============================================
EXEC Sp_Sel_ChallanReturnDetails ''80794'',''80795'',''''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_VendorChallanReturnDetails]
	(
		@BookNumberFrom varchar(15) = '''',
		@BookNumberUpto varchar(10) = '''',
		@ChallanShift varchar(10) = '''',
		@VendorId varchar(15) = ''''
	)
AS
BEGIN
	DECLARE @SQL varchar(max)
	SET @SQL = ''SELECT EntVendorChallan.ChallanNumber, EntVendorChallan.ChallanBranchCode, EntVendorChallan.ChallanDate, EntBookingDetails.BookingNumber, EntBookingDetails.ISN, SubItemName, CASE WHEN EntBookingDetails.ItemProcessType = ''''None'''' THEN '''''''' ELSE EntBookingDetails.ItemProcessType END As ItemProcessType, CASE WHEN EntBookingDetails.ItemExtraProcessType1= ''''None'''' THEN '''''''' WHEN EntBookingDetails.ItemProcessType = ''''None'''' THEN ''''O'''' + EntBookingDetails.ItemExtraProcessType1 ELSE EntBookingDetails.ItemExtraProcessType1 END AS ItemExtraProcessType1, CASE WHEN EntBookingDetails.ItemExtraProcessType2= ''''None'''' THEN '''''''' WHEN EntBookingDetails.ItemProcessType = ''''None'''' THEN ''''O'''' + EntBookingDetails.ItemExtraProcessType2 ELSE EntBookingDetails.ItemExtraProcessType2 END AS ItemExtraProcessType2, EntBookingDetails.ItemQuantityAndRate, EntVendorChallan.ItemTotalQuantitySent, ItemsReceivedFromVendor, (ItemTotalQuantitySent - ItemsReceivedFromVendor) AS ItemsPending, EntVendorChallan.ItemReceivedFromVendorOnDate, EntVendorChallan.Urgent FROM EntVendorChallan INNER JOIN EntBookingDetails ON EntVendorChallan.BookingNumber = EntBookingDetails.BookingNumber AND EntVendorChallan.ItemSNo = EntBookingDetails.ISN INNER JOIN ItemMaster ON EntBookingDetails.ItemName = ItemMaster.ItemName''
	SET @SQL = @SQL + '' WHERE (EntVendorChallan.ItemsReceivedFromVendor < EntVendorChallan.ItemTotalQuantitySent)''
	SET @SQL = @SQL + '' AND EntVendorChallan.VendorId = '''''' + @VendorId + ''''''''
	print(@BookNumberFrom)
	IF(@BookNumberFrom <> '''')
		BEGIN
			IF(@BookNumberUpto = '''')
				BEGIN
					SET @BookNumberUpto = @BookNumberFrom
				END
			SET @SQL = @SQL + '' AND (EntVendorChallan.BookingNumber BETWEEN '' + @BookNumberFrom + '' AND '' + @BookNumberUpto + '')''
		END
	IF (@ChallanShift <> '''')
		BEGIN
			SET @SQL = @SQL + '' AND ChallanSendingShift = '''''' + @ChallanShift + ''''''''
		END
	SET @SQL = @SQL + '' ORDER BY EntBookingDetails.BookingNumber, EntBookingDetails.ISN''
	PRINT @SQL
	EXEC (@SQL)
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EntLedgerEntries]') AND type in (N'U'))
BEGIN
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
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_EntLedgerEntries] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Report_PaymentTypeReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



/*
-- =============================================
-- Author:		<Manoj Gupta>
-- Create date: <10 Oct 2011>
-- Description:	<To select data for Payment Type report>
-- =============================================
EXEC Sp_Report_VendorReport ''1/Sep/2011'',''30/Sep/2011'','''',''''
*/
CREATE PROCEDURE [dbo].[Sp_Report_PaymentTypeReport]
	(
		@BookDate1 datetime,
		@BookDate2	datetime,
		@SearchText varchar(MAX)= '''',
		@BranchId VARCHAR(MAX)=''''
			
	)
AS
BEGIN
	DECLARE @SQL varchar(max)		
	SET @SearchText = REPLACE(@SearchText,'','','''''','''''')
	CREATE TABLE #TmpTable (BookingNumber VARCHAR(MAX), Amount FLOAT,PaymentType VARCHAR(MAX))
	SET @SQL =''INSERT INTO #TmpTable(BookingNumber, Amount, PaymentType) SELECT BookingNumber, Amount,PaymentType
		FROM(
		SELECT  dbo.EntPayment.BookingNumber + ''''-'''' + dbo.CustomerMaster.CustomerName AS BookingNumber, dbo.EntPayment.PaymentMade AS Amount, dbo.EntPayment.PaymentType
			FROM  dbo.EntPayment INNER JOIN  dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
		WHERE  dbo.EntBookings.BranchId= ''''''+ @BranchId +'''''' And (PaymentDate BETWEEN '''''' + Convert(varchar,@BookDate1,106) + '''''' AND '''''' + Convert(varchar,@BookDate2,106) + '''''') AND (PaymentType IN ('''''' + @SearchText + ''''''))
		) AS T1''


exec(@SQL)
SELECT * FROM #TmpTable
GROUP BY PaymentType,BookingNumber,Amount
DROP TABLE #TmpTable
END	





' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EntMenuRights]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EntMenuRights](
	[UserTypeId] [int] NOT NULL,
	[PageTitle] [varchar](500) NOT NULL,
	[FileName] [varchar](500) NULL,
	[RightToView] [bit] NOT NULL,
	[MenuItemLevel] [int] NULL,
	[MenuPosition] [int] NULL,
	[ParentMenu] [varchar](500) NOT NULL CONSTRAINT [DF_EntMenuRights_ParentMenu]  DEFAULT ('None'),
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_EntMenuRights] PRIMARY KEY CLUSTERED 
(
	[UserTypeId] ASC,
	[PageTitle] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Report_ServiceTaxReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'




/*
-- =============================================
-- Author:		<Manoj Gupta>
-- Create date: <10 Oct 2011>
-- Description:	<To select data for serviceTax report>
-- =============================================
EXEC Sp_Report_VendorReport ''1/Sep/2011'',''30/Sep/2011'','''',''''
*/
CREATE PROCEDURE [dbo].[Sp_Report_ServiceTaxReport]
	(
		@BookDate1 datetime,
		@BookDate2	datetime,
		@SearchText varchar(max)= '''',
		@BranchId VARCHAR(MAX)=''''
				
	)
AS
BEGIN
	DECLARE @SQL varchar(max)
	DECLARE @SQL1 varchar(max)
	DECLARE @SQL2 varchar(max)
--	declare @SearchText varchar(max)
--	set @SearchText= ''CL''''''+'',''+''''''C''''''+'',''+''''''DC''''''+'',''+''''''DY''''''+'',''+''''''RF''''''+'',''+''''''RFO''''''+'',''+''''''RE''''''+'',''+''''''ST''''''+'',''+''''''SP''''''+'',''+''''''WC''
	SET @SearchText = REPLACE(@SearchText,'','','''''','''''')
	CREATE TABLE #TmpTable (BookingNumber varchar(20), Amount float, ProcessType varchar(200),BookingDate datetime,BookingAmount float,Status varchar(20))
	SET @SQL =''INSERT INTO #TmpTable( BookingNumber, Amount, ProcessType,BookingDate,BookingAmount,Status) SELECT BookingNumber, Amount, ProcessType,convert(varchar,BookingDate,106),BookingAmount,Status
		FROM(
	SELECT     dbo.EntBookingDetails.BookingNumber, SUM(dbo.EntBookingDetails.STPAmt) AS Amount, dbo.EntBookingDetails.ItemProcessType AS ProcessType, 
                      CONVERT(varchar, dbo.EntBookings.BookingDate, 106) AS BookingDate, 
                     abs(dbo.EntBookingDetails.ItemSubTotal - dbo.EntBookingDetails.ItemExtraProcessRate1 - dbo.EntBookingDetails.ItemExtraProcessRate2) AS BookingAmount,CASE WHEN EntBookings.ReceiptDeliverd=''''True'''' THEN ''''Deliverd'''' ELSE ''''NonDeliverd'''' END AS Status
FROM         dbo.EntBookingDetails INNER JOIN
                      dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber
	WHERE dbo.EntBookings.BookingStatus<>5 AND  dbo.EntBookingDetails.BranchId='''''' + @BranchId +'''''' AND (dbo.EntBookings.BookingDate BETWEEN '''''' + Convert(varchar,@BookDate1,106) + '''''' AND '''''' + Convert(varchar,@BookDate2,106) + '''''') AND ((dbo.EntBookingDetails.STPAmt <> 0))AND (dbo.EntBookingDetails.ItemProcessType IN ('''''' + @SearchText + ''''''))
GROUP BY dbo.EntBookingDetails.BookingNumber, dbo.EntBookingDetails.STPAmt, dbo.EntBookingDetails.ItemProcessType, dbo.EntBookings.BookingDate, 
                      dbo.EntBookings.NetAmount, dbo.EntBookingDetails.ItemSubTotal, dbo.EntBookingDetails.ItemExtraProcessRate1, dbo.EntBookingDetails.BranchId,
                      dbo.EntBookingDetails.ItemExtraProcessRate2, dbo.EntBookings.ReceiptDeliverd) AS T1''


exec(@SQL)



SET @SQL1 =''INSERT INTO #TmpTable( BookingNumber, Amount, ProcessType,BookingDate,BookingAmount,Status) SELECT BookingNumber, Amount, ProcessType,convert(varchar,BookingDate,106),BookingAmount,Status
		FROM(
	SELECT     dbo.EntBookingDetails.BookingNumber, SUM(dbo.EntBookingDetails.STEP1Amt) AS Amount, 
                      dbo.EntBookingDetails.ItemExtraProcessType1 AS ProcessType, CONVERT(varchar, dbo.EntBookings.BookingDate, 106) AS BookingDate, 
                     abs((dbo.EntBookingDetails.ItemSubTotal - dbo.EntBookingDetails.ItemExtraProcessRate2) 
                      - (dbo.EntBookingDetails.ItemSubTotal - dbo.EntBookingDetails.ItemExtraProcessRate1 + dbo.EntBookingDetails.ItemExtraProcessRate2)) 
                      AS BookingAmount,CASE WHEN EntBookings.ReceiptDeliverd=''''True'''' THEN ''''Deliverd'''' ELSE ''''NonDeliverd'''' END AS Status
FROM         dbo.EntBookingDetails INNER JOIN
                      dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber
WHERE  dbo.EntBookings.BookingStatus<>5 AND  dbo.EntBookingDetails.BranchId='''''' + @BranchId +'''''' AND (dbo.EntBookings.BookingDate BETWEEN '''''' + Convert(varchar,@BookDate1,106) + '''''' AND '''''' + Convert(varchar,@BookDate2,106) + '''''') AND  ((dbo.EntBookingDetails.STEP1Amt <> 0))AND (dbo.EntBookingDetails.ItemExtraProcessType1 IN ('''''' + @SearchText + ''''''))
GROUP BY dbo.EntBookingDetails.BookingNumber, dbo.EntBookingDetails.STEP1Amt, dbo.EntBookingDetails.ItemExtraProcessType1, 
                      dbo.EntBookings.BookingDate, dbo.EntBookings.NetAmount, dbo.EntBookingDetails.ItemExtraProcessRate2, dbo.EntBookingDetails.ItemSubTotal, dbo.EntBookingDetails.BranchId,
                      dbo.EntBookingDetails.ItemExtraProcessRate1,dbo.EntBookings.ReceiptDeliverd) AS T1''


exec(@SQL1)

SET @SQL2 =''INSERT INTO #TmpTable( BookingNumber, Amount, ProcessType,BookingDate,BookingAmount,Status) SELECT BookingNumber, Amount, ProcessType,convert(varchar,BookingDate,106),BookingAmount,Status
		FROM(
	SELECT     dbo.EntBookingDetails.BookingNumber, SUM(dbo.EntBookingDetails.STEP2Amt) AS Amount, 
                      dbo.EntBookingDetails.ItemExtraProcessType2 AS ProcessType, CONVERT(varchar, dbo.EntBookings.BookingDate, 106) AS BookingDate, 
                     abs((dbo.EntBookingDetails.ItemSubTotal - dbo.EntBookingDetails.ItemExtraProcessRate1) 
                      - (dbo.EntBookingDetails.ItemSubTotal - dbo.EntBookingDetails.ItemExtraProcessRate1 + dbo.EntBookingDetails.ItemExtraProcessRate2)) 
                      AS BookingAmount,CASE WHEN EntBookings.ReceiptDeliverd=''''True'''' THEN ''''Deliverd'''' ELSE ''''NonDeliverd'''' END AS Status
FROM         dbo.EntBookingDetails INNER JOIN
                      dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber
WHERE dbo.EntBookings.BookingStatus<>5 AND  dbo.EntBookingDetails.BranchId='''''' + @BranchId +'''''' AND (dbo.EntBookings.BookingDate BETWEEN '''''' + Convert(varchar,@BookDate1,106) + '''''' AND '''''' + Convert(varchar,@BookDate2,106) + '''''') AND  ((dbo.EntBookingDetails.STEP2Amt <> 0))AND (dbo.EntBookingDetails.ItemExtraProcessType2 IN ('''''' + @SearchText + ''''''))
GROUP BY dbo.EntBookingDetails.BookingNumber, dbo.EntBookingDetails.STEP2Amt, dbo.EntBookingDetails.ItemExtraProcessType2, 
                      dbo.EntBookingDetails.BranchId,dbo.EntBookings.BookingDate, dbo.EntBookings.NetAmount, dbo.EntBookingDetails.ItemExtraProcessRate2, dbo.EntBookingDetails.ItemSubTotal, 
                      dbo.EntBookingDetails.ItemExtraProcessRate1,dbo.EntBookings.ReceiptDeliverd) AS T1''


exec(@SQL2)
--) as T1
select * from #TmpTable
group by ProcessType,BookingNumber,Amount,BookingDate,BookingAmount,Status
DROP TABLE #TmpTable
END






' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EmployeeMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EmployeeMaster](
	[ID] [int] NULL,
	[EmployeeCode] [varchar](20) NOT NULL,
	[EmployeeSalutation] [varchar](10) NULL,
	[EmployeeName] [varchar](50) NULL,
	[EmployeeAddress] [varchar](100) NULL,
	[EmployeePhone] [varchar](50) NULL,
	[EmployeeMobile] [varchar](50) NULL,
	[EmployeeEmailId] [varchar](100) NULL,
	[BranchId] [varchar](10) NOT NULL CONSTRAINT [DF__EmployeeM__Branc__603D47BB]  DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_EmployeeMaster] PRIMARY KEY CLUSTERED 
(
	[EmployeeCode] ASC,
	[BranchId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LedgerMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LedgerMaster](
	[LedgerName] [varchar](100) NOT NULL,
	[OpenningBalance] [float] NOT NULL CONSTRAINT [DF_LedgerMaster_LedgerOpenningBalance]  DEFAULT ((0)),
	[CurrentBalance] [float] NOT NULL CONSTRAINT [DF_LedgerMaster_CurrentBalance]  DEFAULT ((0)),
	[BranchId] [varchar](10) NOT NULL CONSTRAINT [DF__LedgerMas__Branc__69C6B1F5]  DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_LedgerMaster_1] PRIMARY KEY CLUSTERED 
(
	[LedgerName] ASC,
	[BranchId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EntRecleaning]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EntRecleaning](
	[BookingNumber] [int] NOT NULL,
	[SubItemName] [varchar](30) NOT NULL,
	[ItemSentForReclean] [int] NOT NULL CONSTRAINT [DF_EntRecleaning_ItemSentForReclean]  DEFAULT ((0)),
	[ItemReceivedFromReclean] [int] NOT NULL CONSTRAINT [DF_EntRecleaning_ItemReceivedFromReclean]  DEFAULT ((0)),
	[SendingDate] [datetime] NULL,
	[ReceivingDate] [datetime] NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_EntRecleaning] PRIMARY KEY CLUSTERED 
(
	[BookingNumber] ASC,
	[SubItemName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CustomerWiseDueReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		<>
-- Create date: <>
-- Description:	<>
-- =============================================
CREATE PROCEDURE [dbo].[sp_CustomerWiseDueReport]	
@CustomerName varchar(max)='''',
@Flag varchar(max)=''''
AS
BEGIN	
	IF(@Flag=1)
	BEGIN
	SELECT     TOP (100) PERCENT CustomerName, CustomerAddress, SUM(DuePayment) AS DuePayment
		FROM         (SELECT     ''('' + dbo.CustomerMaster.CustomerCode + '') '' + dbo.CustomerMaster.CustomerSalutation + '' '' + dbo.CustomerMaster.CustomerName AS CustomerName,
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
		FROM         (SELECT     ''('' + dbo.CustomerMaster.CustomerCode + '') '' + dbo.CustomerMaster.CustomerSalutation + '' '' + dbo.CustomerMaster.CustomerName AS CustomerName,
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
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PriorityMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PriorityMaster](
	[PriorityID] [int] NOT NULL,
	[Priority] [varchar](100) NOT NULL,
	[BranchId] [varchar](10) NOT NULL CONSTRAINT [DF__PriorityM__Branc__74444068]  DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_PriorityMaster] PRIMARY KEY CLUSTERED 
(
	[Priority] ASC,
	[BranchId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemwiseProcessRate]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ItemwiseProcessRate](
	[ItemName] [varchar](50) NOT NULL,
	[ProcessCode] [varchar](5) NOT NULL,
	[ProcessPrice] [float] NOT NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_ItemwiseProcessRate] PRIMARY KEY CLUSTERED 
(
	[ItemName] ASC,
	[ProcessCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ItemMaster](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[ItemName] [varchar](50) NOT NULL,
	[NumberOfSubItems] [int] NOT NULL CONSTRAINT [DF_ItemMaster_NumberOfSubItems]  DEFAULT ((1)),
	[ItemCode] [varchar](100) NULL,
	[BranchId] [varchar](10) NOT NULL CONSTRAINT [DF__ItemMaste__Branc__66EA454A]  DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_ItemMaster] PRIMARY KEY CLUSTERED 
(
	[ItemName] ASC,
	[NumberOfSubItems] ASC,
	[BranchId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_MakeAllProcedure]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

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
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''CleanDataBase''
)
exec(
''
'')
else
exec(
''create procedure CleanDataBase
(
	@j int
)
as
	select @j as number
'')
--------
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''prcTask''
)
exec(
''
'')
else
exec(
''create procedure prcTask
(
	@j int
)
as
	select @j as number 
'')
-----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''prcTodayDate''
)
exec(
''
'')
else
exec(
''create procedure prcTodayDate
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''sp_Backup''
)
exec(
''
'')
else
exec(
''create procedure sp_Backup
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''sp_BindGrid''
)
exec(
''
'')
else
exec(
''create procedure sp_BindGrid
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''sp_CustomerWiseDueReport''
)
exec(
''
'')
else
exec(
''create procedure sp_CustomerWiseDueReport
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''sp_Dry_BarcodeMaster''
)
exec(
''
'')
else
exec(
''create procedure sp_Dry_BarcodeMaster
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''sp_Dry_DefaultDataInMasters''
)
exec(
''
'')
else
exec(
''create procedure sp_Dry_DefaultDataInMasters
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''sp_Dry_DrawlMaster''
)
exec(
''
'')
else
exec(
''create procedure sp_Dry_DrawlMaster
(
	@j int
)
as
	select @j as number 
'')
-----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''sp_Dry_EmployeeMaster''
)
exec(
''
'')
else
exec(
''create procedure sp_Dry_EmployeeMaster
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''sp_dry_NewChallan''
)
exec(
''
'')
else
exec(
''create procedure sp_dry_NewChallan
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''sp_EditBooking_SaveProc''
)
exec(
''
'')
else
exec(
''create procedure sp_EditBooking_SaveProc
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''sp_EditRecord''
)
exec(
''
'')
else
exec(
''create procedure sp_EditRecord
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''sp_InsertIntoBarcodeTable''
)
exec(
''
'')
else
exec(
''create procedure sp_InsertIntoBarcodeTable
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_InsUpd_ConfigSettings''
)
exec(
''
'')
else
exec(
''create procedure Sp_InsUpd_ConfigSettings
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_InsUpd_FirstTimeConfigSettings''
)
exec(
''
'')
else
exec(
''create procedure Sp_InsUpd_FirstTimeConfigSettings
(
	@j int
)
as
	select @j as number 
'')
-----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''SP_Mearge_DataWithExistigBooking''
)
exec(
''
'')
else
exec(
''create procedure SP_Mearge_DataWithExistigBooking
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''sp_NewBooking''
)
exec(
''
'')
else
exec(
''create procedure sp_NewBooking
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''sp_NewBooking_SaveProc''
)
exec(
''
'')
else
exec(
''create procedure sp_NewBooking_SaveProc
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''sp_ReceiptConfigSetting''
)
exec(
''
'')
else
exec(
''create procedure sp_ReceiptConfigSetting
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Report_ChallanReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Report_ChallanReport
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Report_NewVendorReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Report_NewVendorReport
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Report_PaymentTypeReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Report_PaymentTypeReport
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Report_ServiceTaxReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Report_ServiceTaxReport
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Report_VendorReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Report_VendorReport
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''sp_Restore''
)
exec(
''
'')
else
exec(
''create procedure sp_Restore
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''sp_rpt_barcodprint''
)
exec(
''
'')
else
exec(
''create procedure sp_rpt_barcodprint
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_AllDeliveryReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_AllDeliveryReport
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_AreaWiseBookingReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_AreaWiseBookingReport
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_AreaWiseClothBookingReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_AreaWiseClothBookingReport
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_BookingDetailsForDelivery''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_BookingDetailsForDelivery
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_BookingDetailsForEditing''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_BookingDetailsForEditing
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_BookingDetailsForReceipt''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_BookingDetailsForReceipt
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_BookingDetailsReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_BookingDetailsReport
(
	@j int
)
as
	select @j as number 
'')
-----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_BookingDetailsReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_BookingDetailsReport
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_BookingReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_BookingReport
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_ChallanReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_ChallanReport
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_ChallanReturnDetails''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_ChallanReturnDetails
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_ChallanReturnDetailsForPrint''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_ChallanReturnDetailsForPrint
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_CustomerAllPrevoiusDue''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_CustomerAllPrevoiusDue
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_CustomerStatus''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_CustomerStatus
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_CustomerWiseBookingReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_CustomerWiseBookingReport
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_DayBookReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_DayBookReport
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_DefaultAnniveraryCustomer''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_DefaultAnniveraryCustomer
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_DefaultBirthDayCustomer''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_DefaultBirthDayCustomer
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_DefaultHomeDeliveryShow''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_DefaultHomeDeliveryShow
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_DefaultUrgentShow''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_DefaultUrgentShow
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_DeliveryQuantityandBooking''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_DeliveryQuantityandBooking
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_DeliveryReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_DeliveryReport
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_EmployeeCheckedByReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_EmployeeCheckedByReport
(
	@j int
)
as
	select @j as number 
'')
-----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_ItemWiseReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_ItemWiseReport
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_NewDeliveryReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_NewDeliveryReport
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_PaymentReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_PaymentReport
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_QuantityandBooking''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_QuantityandBooking
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''SP_Sel_RecForItemIdUpdate''
)
exec(
''
'')
else
exec(
''create procedure SP_Sel_RecForItemIdUpdate
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_RecleanReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_RecleanReport
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_RecordsForBookingCancellation''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_RecordsForBookingCancellation
(
	@j int
)
as
	select @j as number 
'')
-----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_ReportVendorChallanReturnDetails''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_ReportVendorChallanReturnDetails
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_SalesLedgerReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_SalesLedgerReport
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_SecondTimeChallanReturnDetails''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_SecondTimeChallanReturnDetails
(
	@j int
)
as
	select @j as number 
'')
--
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_smsTimeChecking''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_smsTimeChecking
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_StockReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_StockReport
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_TimeWiseClothBookingReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_TimeWiseClothBookingReport
(
	@j int
)
as
	select @j as number 
'')
---
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_Sel_VendorChallanReturnDetails''
)
exec(
''
'')
else
exec(
''create procedure Sp_Sel_VendorChallanReturnDetails
(
	@j int
)
as
	select @j as number 
'')
-----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''SP_SelectBookingRecordsForEdition''
)
exec(
''
'')
else
exec(
''create procedure SP_SelectBookingRecordsForEdition
(
	@j int
)
as
	select @j as number 
'')
----
if exists
(
	select * from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME=''Sp_VendorReport''
)
exec(
''
'')
else
exec(
''create procedure Sp_VendorReport
(
	@j int
)
as
	select @j as number 
'')
END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProcessMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProcessMaster](
	[ProcessCode] [varchar](10) NOT NULL,
	[ProcessName] [varchar](30) NOT NULL,
	[ProcessUsedForVendorReport] [bit] NOT NULL CONSTRAINT [DF_ProcessMaster_ProcessUsedForVendorReport]  DEFAULT ((1)),
	[Discount] [bit] NULL,
	[ServiceTax] [float] NULL,
	[IsActiveServiceTax] [bit] NULL,
	[IsDiscount] [bit] NULL,
	[ImagePath] [varchar](50) NULL,
	[IsChallan] [bit] NULL,
	[BranchId] [varchar](10) NOT NULL CONSTRAINT [DF__ProcessMa__Branc__640DD89F]  DEFAULT ((0)),
	[ExternalBranchId] [varchar](10) NULL,
 CONSTRAINT [PK_ProcessMaster] PRIMARY KEY CLUSTERED 
(
	[ProcessCode] ASC,
	[ProcessName] ASC,
	[BranchId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mstDate]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[mstDate](
	[DateId] [int] IDENTITY(1,1) NOT NULL,
	[TodayDate] [datetime] NULL,
	[Key1] [nvarchar](100) NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_mstDate] PRIMARY KEY CLUSTERED 
(
	[DateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mstDrawl]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[mstDrawl](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Drawl] [varchar](50) NOT NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [IX_mstDrawl] UNIQUE NONCLUSTERED 
(
	[Drawl] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EntSubItemDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EntSubItemDetails](
	[ItemName] [varchar](50) NOT NULL,
	[SubItemName] [varchar](50) NOT NULL,
	[SubItemOrder] [tinyint] NOT NULL,
	[BranchId] [varchar](10) NOT NULL CONSTRAINT [DF__EntSubIte__Branc__7CD98669]  DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mstDrwal]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[mstDrwal](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DrawlName] [varchar](50) NOT NULL,
	[ParentDrawl] [int] NOT NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mstJobType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[mstJobType](
	[ID] [int] NOT NULL,
	[JobType] [varchar](50) NOT NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_mstJobType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_mstJobType] UNIQUE NONCLUSTERED 
(
	[JobType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mstColor]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[mstColor](
	[ID] [int] NOT NULL,
	[ColorName] [varchar](50) NULL,
	[ColorCode] [varchar](50) NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [IX_mstColor] UNIQUE NONCLUSTERED 
(
	[ColorName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_mstColor_1] UNIQUE NONCLUSTERED 
(
	[ColorCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mstExpKey]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[mstExpKey](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExpKey] [nvarchar](100) NOT NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_mst_ExpKey] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BranchMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BranchMaster](
	[BranchCode] [varchar](10) NOT NULL,
	[BranchName] [varchar](30) NOT NULL,
	[BranchAddress] [varchar](100) NOT NULL,
	[BranchPhone] [varchar](50) NULL,
	[BranchSlogan] [varchar](100) NULL,
	[BranchId] [varchar](10) NOT NULL CONSTRAINT [DF__BranchMas__Branc__5E54FF49]  DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_BranchMaster] PRIMARY KEY CLUSTERED 
(
	[BranchCode] ASC,
	[BranchName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mstTask]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[mstTask](
	[Key1] [nvarchar](100) NULL,
	[Key2] [int] NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EntBookings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EntBookings](
	[BookingID] [bigint] NOT NULL,
	[BookingNumber] [varchar](15) NOT NULL,
	[BookingByCustomer] [varchar](50) NOT NULL,
	[BookingAcceptedByUserId] [varchar](20) NOT NULL,
	[IsUrgent] [bit] NOT NULL,
	[BookingDate] [datetime] NULL,
	[BookingDeliveryDate] [datetime] NOT NULL,
	[BookingDeliveryTime] [varchar](50) NULL,
	[TotalCost] [float] NOT NULL CONSTRAINT [DF_EntBookings_TotalCost]  DEFAULT ((0)),
	[Discount] [float] NOT NULL CONSTRAINT [DF_EntBookings_Discount]  DEFAULT ((0)),
	[NetAmount] [float] NOT NULL CONSTRAINT [DF_EntBookings_NetAmount]  DEFAULT ((0)),
	[BookingStatus] [int] NOT NULL CONSTRAINT [DF_EntBookings_OrderStatus]  DEFAULT ((0)),
	[BookingCancelDate] [datetime] NULL,
	[BookingCancelReason] [varchar](100) NOT NULL,
	[BookingRemarks] [varchar](200) NULL,
	[ItemTotalQuantity] [int] NOT NULL CONSTRAINT [DF_EntBookings_ItemTotalQuantity]  DEFAULT ((0)),
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
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_EntBookings] PRIMARY KEY CLUSTERED 
(
	[BookingNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_EntBookings] UNIQUE NONCLUSTERED 
(
	[BookingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'EntBookings', N'COLUMN',N'BookingStatus'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0=No Order by customer;
1=Received from customer and PendingAtShop;
2=Sent To Vendor;
3=Received fromVendor and PendingAtShop;
4=Delivered to customer;
5=Cancelled By Customer and Pending at shop;' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EntBookings', @level2type=N'COLUMN',@level2name=N'BookingStatus'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserMaster]') AND type in (N'U'))
BEGIN
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
	[UserActive] [bit] NOT NULL CONSTRAINT [DF_UserMaster_UserActive]  DEFAULT ((1)),
	[BranchId] [varchar](10) NOT NULL CONSTRAINT [DF__UserMaste__Branc__7720AD13]  DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_UserMaster_1] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[BranchId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TmpChallan]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TmpChallan](
	[ChallanNumber] [int] NOT NULL CONSTRAINT [DF_TmpChallan_ChallanNumber]  DEFAULT ((0)),
	[ChallanBranchCode] [varchar](5) NOT NULL,
	[ChallanDate] [datetime] NOT NULL,
	[ChallanSendingShift] [varchar](10) NOT NULL CONSTRAINT [DF_TmpChallan_ChallanSendingShift]  DEFAULT ((1)),
	[BookingNumber] [varchar](15) NOT NULL,
	[ItemSNo] [int] NOT NULL,
	[ItemTotalQuantitySent] [int] NOT NULL CONSTRAINT [DF_TmpChallan_ItemTotalQuantitySent]  DEFAULT ((0)),
	[ItemsReceivedFromVendor] [int] NOT NULL CONSTRAINT [DF_TmpChallan_ItemsReceivedFromVendor]  DEFAULT ((0)),
	[ItemReceivedFromVendorOnDate] [datetime] NULL,
	[Urgent] [bit] NOT NULL CONSTRAINT [DF_TmpChallan_Urgent]  DEFAULT ((0)),
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EntChallan]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EntChallan](
	[ChallanNumber] [int] NOT NULL CONSTRAINT [DF_EntChallan_ChallanNumber]  DEFAULT ((0)),
	[ChallanBranchCode] [varchar](5) NOT NULL,
	[ChallanDate] [datetime] NOT NULL,
	[ChallanSendingShift] [varchar](10) NOT NULL CONSTRAINT [DF_EntChallan_ChallanSendingShift]  DEFAULT ((1)),
	[BookingNumber] [varchar](15) NOT NULL,
	[ItemSNo] [int] NOT NULL,
	[SubItemName] [varchar](50) NOT NULL,
	[ItemTotalQuantitySent] [int] NOT NULL CONSTRAINT [DF_EntChallan_ItemsPendingToSend]  DEFAULT ((0)),
	[ItemsReceivedFromVendor] [int] NULL CONSTRAINT [DF_EntChallan_ItemReceivedFromVendor]  DEFAULT ((0)),
	[ItemReceivedFromVendorOnDate] [datetime] NULL,
	[Urgent] [bit] NOT NULL CONSTRAINT [DF_EntChallan_Urgent]  DEFAULT ((0)),
	[BranchId] [varchar](10) NOT NULL CONSTRAINT [DF__EntChalla__Branc__7814D14C]  DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserTypeMaster]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserTypeMaster](
	[UserTypeID] [int] NOT NULL,
	[UserType] [varchar](30) NOT NULL,
	[UserTypeDetails] [varchar](50) NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_UserTypeMaster] PRIMARY KEY CLUSTERED 
(
	[UserTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Backup]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


-- =============================================
-- Author:		<Sanjeev Arora>
-- Create date: <25-Aug-2011>
-- Description:	<Back Up DataBase>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Backup]	
@fileName varchar(max)
AS
BEGIN
	BACKUP DATABASE [DRYSOFT] TO  
    DISK = @fileName
    WITH NOFORMAT, NOINIT,  NAME = N''DrySoft'',
	SKIP, NOREWIND, NOUNLOAD,  STATS = 10
Select ''Success''
END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mstReceiptConfig]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[mstReceiptConfig](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HeaderText] [varchar](40) NULL,
	[HeaderFontName] [varchar](max) NULL,
	[HeaderFontSize] [varchar](max) NOT NULL,
	[HeaderFontStyle] [varchar](max) NULL,
	[AddressText] [varchar](70) NULL,
	[AddressFontName] [varchar](max) NULL,
	[AddressFontSize] [varchar](max) NULL,
	[AddressFontStyle] [varchar](max) NOT NULL,
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
	[TimeZone] [varchar](200) NULL,
	[ImagePath] [varchar](50) NULL,
	[StoreCopy] [bit] NULL,
	[ThermalPrinter] [bit] NULL,
	[DotMatrixPrinter] [bit] NULL,
	[ChallanType] [varchar](50) NULL,
	[HostName] [varchar](max) NULL CONSTRAINT [DF_mstReceiptConfig_HostName]  DEFAULT ('smtp.gmail.com'),
	[BranchEmail] [varchar](max) NULL CONSTRAINT [DF_mstReceiptConfig_BranchEmail]  DEFAULT ('deecoup.2011@gmail.com'),
	[BranchPassword] [varchar](max) NULL CONSTRAINT [DF_mstReceiptConfig_BranchPassword]  DEFAULT ('deecoup@12345'),
	[SSL] [bit] NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_mstReceiptConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Report_NewVendorReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/*
-- =============================================
-- Author:		<Manoj Kumar>
-- Create date: <27 Jan 2011>
-- Description:	<To select data for out sourced vendor report>
-- =============================================
EXEC Sp_Report_NewVendorReport ''1/1/2011'',''1/30/2011'','''','''',''1''
*/
CREATE PROCEDURE [dbo].[Sp_Report_NewVendorReport]
	(
		@BookDate1 datetime,
		@BookDate2	datetime,
		@ProcessCode varchar(max)= '''',
		@ItemName varchar(max) = '''',
		@VendorId varchar(max)=''''
	)
AS
BEGIN
	DECLARE @SQL varchar(max)
	SET @SQL =''SELECT BookingDate, BookingNumber, SUM(ProcessCost) AS ProcessCost, Pieces
	FROM
	(
	SELECT CONVERT(varchar, dbo.EntBookings.BookingDate, 106) AS BookingDate, dbo.EntBookings.BookingNumber, dbo.EntBookingDetails.ItemName, dbo.EntBookingDetails.ItemTotalQuantity, dbo.EntBookingDetails.ItemTotalQuantity * dbo.ItemMaster.NumberOfSubItems AS Pieces, dbo.EntBookingDetails.ItemProcessType, dbo.EntBookingDetails.ItemExtraProcessType1, dbo.EntBookingDetails.ItemExtraProcessRate1, CASE WHEN ItemProcessType = '''' None '''' THEN ItemExtraProcessRate1 * EntBookingDetails.ItemTotalQuantity ELSE ItemExtraProcessRate1 END AS ProcessCost, dbo.EntVendorChallan.VendorId FROM dbo.EntBookings INNER JOIN dbo.EntBookingDetails ON dbo.EntBookings.BookingNumber = dbo.EntBookingDetails.BookingNumber INNER JOIN dbo.ItemMaster ON dbo.EntBookingDetails.ItemName = dbo.ItemMaster.ItemName INNER JOIN dbo.EntVendorChallan ON dbo.EntBookings.BookingNumber = dbo.EntVendorChallan.BookingNumber
	WHERE (BookingDate BETWEEN '''''' + Convert(varchar,@BookDate1,106) + '''''' AND '''''' + Convert(varchar,@BookDate2,106) + '''''')''
	IF(@VendorId <> '''')
		BEGIN			
			SET @VendorId = REPLACE(@VendorId,'','','''''','''''')
			SET @SQL = @SQL + '' AND (dbo.EntVendorChallan.VendorId IN ('''''' + @VendorId + ''''''))''
		END
	
	SET @SQL = @SQL + '') AS T1''
	SET @SQL = @SQL + ''	Group By BookingDate, BookingNumber, Pieces ''
	PRINT @SQL
	EXEC (@SQL)
	
END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MstConfigSettings]') AND type in (N'U'))
BEGIN
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
	[AmountType] [varchar](50) NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CleanDataBase]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[CleanDataBase]	
AS
BEGIN	
	Select CustomerName,CustomerAddress From CustomerMaster
END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_SelectBookingRecordsForEdition]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/*
EXEC SP_SelectBookingRecordsForEdition ''8/1/2010'',''8/30/2010''
*/
CREATE PROCEDURE [dbo].[SP_SelectBookingRecordsForEdition]
(
	@BookingDate1 datetime='''',
	@BookingDate2 datetime=''''
)
AS
BEGIN
	SELECT Convert(varchar,EntBookings.BookingDate,106) As BookingDate, EntBookings.BookingNumber, EntBookingDetails.ItemName, 
                      EntBookingDetails.ItemTotalQuantity As ''TotalQty'', EntBookingDetails.ItemProcessType As ''Process'', EntBookingDetails.ItemQuantityAndRate As ''ItemQtyAndRate'', 
                      EntBookingDetails.ItemExtraProcessType1 As ''ExtraProcess1'', EntBookingDetails.ItemExtraProcessRate1 As ''ExtraProcessRate1'', EntBookingDetails.ItemExtraProcessType2 As ''ExtraProcess2'', 
                      EntBookingDetails.ItemExtraProcessRate2 As ''ExtraProcessRate2'', EntBookingDetails.ItemSubTotal
	FROM   EntBookings INNER JOIN EntBookingDetails ON EntBookings.BookingNumber = EntBookingDetails.BookingNumber
	WHERE (BookingDate BETWEEN @BookingDate1 AND @BookingDate2)
	ORDER BY BookingDate, EntBookings.BookingNumber

END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Report_VendorReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <7 August 2010>
-- Description:	<To select data for vendor report>
-- =============================================
EXEC Sp_Report_VendorReport ''1/Sep/2011'',''30/Sep/2011'','''',''''
*/
CREATE PROCEDURE [dbo].[Sp_Report_VendorReport]
	(
		@BookDate1 datetime,
		@BookDate2	datetime,
		@ProcessCode varchar(max)= '''',
		@ItemName varchar(max) = ''''
	)
AS
BEGIN
	DECLARE @SQL varchar(max)
	SET @SQL =''SELECT BookingDate, BookingNumber, SUM(ProcessCost) AS ProcessCost, Pieces
	FROM
	(
	SELECT Convert(varchar, BookingDate,106) AS BookingDate, EntBookings.BookingNumber, EntBookingDetails.ItemName, EntBookingDetails.ItemTotalQuantity, EntBookingDetails.ItemTotalQuantity * ItemMaster.NumberOfSubItems As Pieces, ItemProcessType, ItemExtraProcessType1, ItemExtraProcessRate1, CASE WHEN ItemProcessType = ''''None'''' THEN ItemExtraProcessRate1 * EntBookingDetails.ItemTotalQuantity ELSE ItemExtraProcessRate1 END AS ProcessCost,dbo.ProcessMaster.ProcessUsedForVendorReport
	FROM EntBookings INNER JOIN EntBookingDetails ON EntBookings.BookingNumber = EntBookingDetails.BookingNumber INNER JOIN ItemMaster ON EntBookingDetails.ItemName = ItemMaster.ItemName INNER JOIN dbo.ProcessMaster ON dbo.EntBookingDetails.ItemExtraProcessType1 = dbo.ProcessMaster.ProcessCode
	WHERE (dbo.ProcessMaster.ProcessUsedForVendorReport = ''''1'''')''
	IF(@ProcessCode <> '''')
		BEGIN			
			SET @ProcessCode = REPLACE(@ProcessCode,'','','''''','''''')
			SET @SQL = @SQL + '' AND (ItemExtraProcessType1 IN ('''''' + @ProcessCode + ''''''))''
		END
--	IF(@ItemName <>'''')
--		BEGIN
--			SET @ItemName = REPLACE(@ItemName,'','','''''','''''')
--			SET @SQL = @SQL + '' AND (EntBookingDetails.ItemName IN (''''''+ @ItemName +''''''))''
--		END
	SET @SQL = @SQL + '') AS T1''
	SET @SQL = @SQL + ''	Group By BookingDate, BookingNumber, Pieces ''
	PRINT @SQL
	--EXEC (@SQL)
	
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Restore]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
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
	FROM DISK = N''c:\INETPUB\WWWROOT\DRYSOFT_backup.bak'' WITH Replace
END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_AllDeliveryReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Challan details>
-- =============================================
EXEC Sp_Sel_DeliveryReport ''1 JUL 2010'', ''1 SEP 2010''
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
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_ChallanReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Challan details>
-- =============================================
EXEC Sp_Sel_ChallanReport ''1 SEP 2010'', ''30 SEP 2010''
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
	SELECT BookingDate, ChallanNumber, EntChallan.BookingNumber, EntBookingDetails.ItemName, CASE WHEN ItemProcessType = ''NONE'' THEN '''' ELSE ItemProcessType END As ItemProcessType, CASE WHEN ItemExtraProcessType1 = ''NONE'' THEN '''' WHEN ItemProcessType = ''NONE'' THEN ''O'' + ItemExtraProcessType1 ELSE ItemExtraProcessType1 END As ItemExtraProcessType1, CASE WHEN ItemExtraProcessType2 = ''NONE'' THEN '''' WHEN ItemProcessType = ''NONE'' THEN ''O'' + ItemExtraProcessType2 ELSE ItemExtraProcessType2 END As ItemExtraProcessType2, SUM(ItemTotalQuantitySent) AS ItemTotalQuantitySent, SUM(ItemsReceivedFromVendor) AS ItemsReceivedFromVendor
	FROM EntBookings EB INNER JOIN EntChallan ON EB.BookingNumber = EntChallan.BookingNumber INNER JOIN EntBookingDetails ON EntChallan.BookingNumber = EntBookingDetails.BookingNumber AND EntChallan.ItemSno= EntBookingDetails.ISN
	WHERE ChallanDate BETWEEN @ChallanDate1 AND @ChallanDate2
	Group By BookingDate, ChallanNumber, EntChallan.BookingNumber, EntBookingDetails.ItemName, ItemProcessType, ItemExtraProcessType1, ItemExtraProcessType2

	INSERT INTO #TmpChallanReport(BookingDate) SELECT ''''
	INSERT INTO #TmpChallanReport(BookingDate, BookingNumber) SELECT ItemProcessType, SUM(ItemsSent) FROM #TmpChallanReport WHERE ItemProcessType<>'''' GROUP BY ItemProcessType
	INSERT INTO #TmpChallanReport(BookingDate, BookingNumber) SELECT ItemExtraProcessType1, SUM(ItemsSent) FROM #TmpChallanReport WHERE ItemExtraProcessType1<>'''' GROUP BY ItemExtraProcessType1
	INSERT INTO #TmpChallanReport(BookingDate, BookingNumber) SELECT ItemExtraProcessType2, SUM(ItemsSent) FROM #TmpChallanReport WHERE ItemExtraProcessType2<>'''' GROUP BY ItemExtraProcessType2
	INSERT INTO #TmpChallanReport(BookingDate, BookingNumber) SELECT ''Total'', SUM(ItemsSent) FROM #TmpChallanReport
	SELECT * FROM #TmpChallanReport

	DROP TABLE #TmpChallanReport

END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_ChallanReturnDetails]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'




/*
-- =============================================
-- Author:		<Karam chand sharma>
-- Create date: <6 Aug 2010>
-- Description:	<To select Challan Details for challan return>
-- =============================================
EXEC Sp_Sel_ChallanReturnDetails ''1431'',''1440'',''''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_ChallanReturnDetails]
	(
		@BookNumberFrom varchar(15) = '''',
		@BookNumberUpto varchar(10) = '''',
		@ChallanShift varchar(10) = '''',
		@SerialNo varchar(10) = '''',
		@BranchId varchar(15)=''''
	)
AS
BEGIN
	DECLARE @SQL varchar(max)	
	SET @SQL = ''SELECT CONVERT(INT,BarcodeTable.BookingNo) AS BookingNumber,dbo.CustomerMaster.CustomerSalutation + '''' '''' + dbo.CustomerMaster.CustomerName AS Customer, dbo.BarcodeTable.Item AS SubItemName, dbo.EntChallan.Urgent, dbo.BarcodeTable.Process AS ItemProcessType, CASE WHEN BarcodeTable.ItemExtraprocessType = ''''0'''' THEN '''''''' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType1, CASE WHEN BarcodeTable.ItemExtraprocessType2 = ''''0'''' THEN '''''''' ELSE BarcodeTable.ItemExtraprocessType2 END AS ItemExtraprocessType2, dbo.EntChallan.ItemTotalQuantitySent, dbo.EntChallan.ItemsReceivedFromVendor, dbo.EntChallan.ItemTotalQuantitySent - dbo.EntChallan.ItemsReceivedFromVendor AS ItemsPending, dbo.BarcodeTable.RowIndex AS ISN ,''''1''''  AS ItemTotalQuantity FROM  dbo.EntChallan INNER JOIN  dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex INNER JOIN dbo.CustomerMaster ON dbo.BarcodeTable.BookingByCustomer = dbo.CustomerMaster.CustomerCode''
	SET @SQL = @SQL + '' WHERE (dbo.BarcodeTable.StatusId=''''2'''') AND (dbo.BarcodeTable.BranchId = '' + @BranchId + '')''
	print(@BookNumberFrom)
	IF(@BookNumberFrom <> '''')
		BEGIN
			IF(@BookNumberUpto = '''')
				BEGIN
					SET @BookNumberUpto = @BookNumberFrom
				END
			SET @SQL = @SQL + '' AND (EntChallan.BookingNumber = '' + @BookNumberFrom + '')''
		END		
	SET @SQL = @SQL + '' ORDER BY CONVERT(INT,BarcodeTable.BookingNo), BarcodeTable.RowIndex''
	PRINT @SQL
	EXEC (@SQL)
END






' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mstVendor]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[mstVendor](
	[ID] [int] NOT NULL,
	[VendorSalutation] [varchar](10) NOT NULL,
	[VendorName] [varchar](50) NOT NULL,
	[VendorAddress] [varchar](100) NOT NULL,
	[VendorPhone] [varchar](50) NULL,
	[VendorMobile] [varchar](50) NULL,
	[VendorEmailId] [varchar](100) NULL,
	[JobType] [int] NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL,
 CONSTRAINT [PK_mstVendor] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BarcodeTable]') AND type in (N'U'))
BEGIN
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
	[DeliveredQty] [bit] NOT NULL CONSTRAINT [DF_BarcodeTable_DeliveredQty]  DEFAULT ('False'),
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
	[DelQty] [int] NULL CONSTRAINT [DF_BarcodeTable_DelQty]  DEFAULT ('0'),
	[RemoveFrom] [varchar](50) NULL,
	[UserId] [varchar](50) NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EntPayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EntPayment](
	[BookingNumber] [varchar](15) NOT NULL,
	[PaymentDate] [smalldatetime] NOT NULL,
	[PaymentMade] [float] NOT NULL CONSTRAINT [DF_EntPayment_PaymentMade]  DEFAULT ((0)),
	[DiscountOnPayment] [float] NOT NULL CONSTRAINT [DF_EntPayment_DiscountOnPayment]  DEFAULT ((0)),
	[DeliveryStatus] [bit] NULL CONSTRAINT [DF_EntPayment_DeliveryStatus]  DEFAULT ('False'),
	[MsgStaus] [bit] NULL,
	[DeliveryMsg] [varchar](100) NULL,
	[PaymentType] [varchar](50) NULL,
	[PaymentRemarks] [varchar](100) NULL,
	[PaymentTime] [varchar](50) NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EntBookingDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EntBookingDetails](
	[BookingNumber] [varchar](15) NOT NULL CONSTRAINT [DF_EntBookingDetails_EntBookings]  DEFAULT ((0)),
	[ISN] [int] NOT NULL CONSTRAINT [DF_EntBookingDetails_ISN]  DEFAULT ((1)),
	[ItemName] [varchar](50) NOT NULL CONSTRAINT [DF_EntBookingDetails_ItemName]  DEFAULT ('None'),
	[ItemTotalQuantity] [int] NOT NULL CONSTRAINT [DF_EntBookingDetails_ItemTotalQuantity]  DEFAULT ((0)),
	[ItemProcessType] [varchar](50) NOT NULL CONSTRAINT [DF_EntBookingDetails_ItemProcessType]  DEFAULT ('None'),
	[ItemQuantityAndRate] [varchar](2000) NOT NULL CONSTRAINT [DF_EntBookingDetails_ItemColorAndQuantity]  DEFAULT ('None'),
	[ItemExtraProcessType1] [varchar](50) NULL CONSTRAINT [DF_EntBookingDetails_ItemExtraProcessType1]  DEFAULT ('None'),
	[ItemExtraProcessRate1] [float] NOT NULL CONSTRAINT [DF_EntBookingDetails_ItemExtraProcessRate1]  DEFAULT ((0)),
	[ItemExtraProcessType2] [varchar](50) NOT NULL CONSTRAINT [DF_EntBookingDetails_ItemExtraProcessType2]  DEFAULT ('None'),
	[ItemExtraProcessRate2] [float] NOT NULL CONSTRAINT [DF_EntBookingDetails_ItemExtraProcessRate2]  DEFAULT ((0)),
	[ItemExtraProcessType3] [varchar](50) NOT NULL CONSTRAINT [DF_EntBookingDetails_ItemExtraProcessType3]  DEFAULT ('None'),
	[ItemExtraProcessRate3] [float] NOT NULL CONSTRAINT [DF_EntBookingDetails_ItemExtraProcessRate3]  DEFAULT ((0)),
	[ItemSubTotal] [float] NOT NULL CONSTRAINT [DF_EntBookingDetails_ItemSubTotal]  DEFAULT ((0)),
	[ItemStatus] [int] NOT NULL,
	[ItemRemark] [varchar](100) NULL,
	[DeliveredQty] [int] NOT NULL CONSTRAINT [DF__EntBookin__Deliv__45F365D3]  DEFAULT ((0)),
	[ItemColor] [varchar](50) NULL,
	[VendorItemStatus] [int] NULL,
	[STPAmt] [float] NULL,
	[STEP1Amt] [float] NULL,
	[STEP2Amt] [float] NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EntVendorChallan]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EntVendorChallan](
	[ChallanNumber] [int] NOT NULL CONSTRAINT [DF_EntVendorChallan_ChallanNumber]  DEFAULT ((0)),
	[ChallanBranchCode] [varchar](5) NOT NULL,
	[ChallanDate] [datetime] NOT NULL,
	[ChallanSendingShift] [varchar](10) NOT NULL CONSTRAINT [DF_EntVendorChallan_ChallanSendingShift]  DEFAULT ((1)),
	[BookingNumber] [varchar](15) NOT NULL,
	[ItemSNo] [int] NOT NULL,
	[SubItemName] [varchar](50) NOT NULL,
	[ItemTotalQuantitySent] [int] NOT NULL CONSTRAINT [DF_EntVendorChallan_ItemTotalQuantitySent]  DEFAULT ((0)),
	[ItemsReceivedFromVendor] [int] NOT NULL CONSTRAINT [DF_EntVendorChallan_ItemsReceivedFromVendor]  DEFAULT ((0)),
	[ItemReceivedFromVendorOnDate] [datetime] NULL,
	[Urgent] [bit] NOT NULL CONSTRAINT [DF_EntVendorChallan_Urgent]  DEFAULT ((0)),
	[VendorId] [int] NULL,
	[BranchId] [varchar](max) NOT NULL DEFAULT ((0)),
	[ExternalBranchId] [varchar](max) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Dry_DrawlMaster]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[sp_Dry_DrawlMaster]		
	@Flag VARCHAR(MAX)='''',
	@ID int='''',
	@DrawlName VARCHAR(MAX)='''',
	@ParentDrawl int='''',
	@CustCode VARCHAR(MAX)='''',
	@BookingNumber VARCHAR(MAX)='''',
	@MsgStaus bit='''',
	@MainDate smalldatetime='''',
	@BookDate1 smalldatetime='''',
	@BookingTime nvarchar(50)='''',
	@Format varchar(10)='''',
	@ItemCode VARCHAR(MAX)='''',
	@SearchText VARCHAR(MAX)='''',
	@challandate datetime='''',
	@challanSendingShift varchar(max)='''',
	@ChallanNumber varchar(max)	='''',
	@RowIndex int='''',
	@BranchId VARCHAR(MAX)=''''
	
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
			UPDATE dbo.mstDrwal SET DrawlName=@DrawlName,ParentDrawl=@ParentDrawl where Id=@ID and BranchId=@BranchId
		END
	ELSE IF(@Flag=3)
		BEGIN
			SELECT * FROM  dbo.mstDrwal where BranchId=@BranchId ORDER BY Id DESC
		END
	ELSE IF(@Flag=4)
		BEGIN
			SELECT * FROM  dbo.mstDrwal WHERE (DrawlName Like @DrawlName) And (BranchId=@BranchId) ORDER BY Id DESC
		END
	ELSE IF(@Flag=5)
		BEGIN
			DELETE FROM  dbo.mstDrwal WHERE Id=@ID and BranchId=@BranchId
		END
	ELSE IF(@Flag=6)
		BEGIN
			SELECT * FROM  dbo.mstDrwal WHERE DrawlName=@DrawlName AND ParentDrawl=@ParentDrawl and BranchId=@BranchId  
		END
	ELSE IF(@Flag=7)
		BEGIN
			SELECT * FROM  dbo.mstDrawl Where BranchId=@BranchId
		END
	ELSE IF(@Flag=8)
		BEGIN
			SELECT COALESCE(CustomerSalutation,'''') + '' '' + [CustomerName] As CustomerName , CustomerMobile FROM  dbo.CustomerMaster where CustomerCode=@CustCode AND BranchId=@BranchId
		END
	ELSE IF(@Flag=9)
		BEGIN			
			DECLARE @SQL varchar(max), @PaymentMade float, @DiscountGiven float
	CREATE TABLE #TmpDeliveryInfo (BookingNumber varchar(10), BookingDate varchar(20), DeliveryDate varchar(20), CustomerCode varchar(10), CustomerName varchar(100), BookingAmount float, Discount float, NetAmount float, PaymentMade float, DuePayment float, DiscountOnPayment float)
	INSERT INTO #TmpDeliveryInfo (BookingNumber , CustomerCode , CustomerName , BookingDate, DeliveryDate, BookingAmount, Discount, NetAmount)
	 SELECT BookingNumber, BookingByCustomer, CustomerSalutation + '' ''  + CustomerName As CustomerName, Convert(varchar,BookingDate,106) As BookingDate, Convert(varchar, BookingDeliveryDate, 106) + '' '' + BookingDeliveryTime As BookingDeliveryDate, TotalCost, Discount, NetAmount 
		FROM EntBookings LEFT JOIN CustomerMaster ON EntBookings.BookingByCustomer = CustomerMaster.CustomerCode
		WHERE BookingNumber = @BookingNumber and EntBookings.BranchId=@BranchId
	
	SELECT @PaymentMade = COALESCE(SUM(PaymentMade),0), @DiscountGiven = COALESCE(SUM(DiscountOnPayment),0) FROM EntPayment WHERE BookingNumber = @BookingNumber and BranchId=@BranchId
	UPDATE #TmpDeliveryInfo SET PaymentMade = @PaymentMade + @DiscountGiven
	UPDATE #TmpDeliveryInfo SET DuePayment= COALESCE(NetAmount - PaymentMade,0), DiscountOnPayment = @DiscountGiven
	--Table(0)
	SELECT * FROM #TmpDeliveryInfo
	DROP TABLE #TmpDeliveryInfo
		END
	ELSE IF(@Flag=10)
		BEGIN	
		SELECT DeliveredQty FROM BarcodeTable where BookingNo=@BookingNumber and BranchId=@BranchId
		END
	ELSE IF(@Flag=11)
		BEGIN	
		UPDATE EntPayment SET MsgStaus=@MsgStaus Where BookingNumber=@BookingNumber and BranchId=@BranchId
		END
	ELSE IF(@Flag=12)
		BEGIN
		SELECT DISTINCT BookingNumber FROM entpayment WHERE MsgStaus=''true'' AND BranchId=@BranchId
		END
	ELSE IF(@Flag=13)
		BEGIN
		SELECT DISTINCT BookingNumber,convert(varchar, PaymentDate,106) as date from entpayment where MsgStaus=''true'' and bookingnumber=@BookingNumber AND BranchId=@BranchId order by date desc
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
		WHERE     (dbo.EntBookings.BookingNumber = @BookingNumber) And (EntBookings.BranchId=@BranchId)
		END
	ELSE IF(@Flag=16)
		BEGIN
		SELECT CustomerMessage from MstConfigSettings Where BranchId=@BranchId
		END
	ELSE IF(@Flag=17)
		BEGIN
		UPDATE EntPayment SET MsgStaus=@MsgStaus Where BookingNumber=@BookingNumber AND BranchId=@BranchId
		END
	ELSE IF(@Flag=18)
		BEGIN
		SELECT * FROM ConfigurationSetting WHERE BranchId=@BranchId
		END
	ELSE IF(@Flag=19)
		BEGIN
		SELECT Configuration FROM ConfigurationSetting WHERE Configuration=''1''
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
		Update EntBookings set BookingTime=@BookingTime, Format=@Format where BookingNumber=@BookingNumber and BranchId=@BranchId
		END
	ELSE IF(@Flag=22)
		BEGIN
		select ItemName from itemmaster where BranchId=@BranchId AND ItemCode=@ItemCode and ItemCode<>'''' 
		END
	ELSE IF(@Flag=23)
		BEGIN
		select CustomerName from CustomerMaster where CustomerName=@SearchText and BranchId=@BranchId
		END
	ELSE IF(@Flag=24)
		BEGIN
		select CustomerMobile from CustomerMaster where CustomerMobile=@SearchText and BranchId=@BranchId
		END
	ELSE IF(@Flag=25)
		BEGIN
		select CustomerCode from CustomerMaster where CustomerName=@SearchText and BranchId=@BranchId 
		END
	ELSE IF(@Flag=26)
		BEGIN
--		declare @challandate datetime
--		declare @ChallanNumber varchar(max)	
--		declare @challanSendingShift varchar(max)		
--		set @challanSendingShift=''12 Noon''	
		set @challandate= CONVERT(VARCHAR(50),GETDATE(),106)	
		CREATE TABLE #TmpGetChallanNumber (ChallanNumber varchar(max))
		set @ChallanNumber=  (select max(challannumber)+1 from entchallan where challanSendingShift=@challanSendingShift and challandate=@challandate and BranchId=@BranchId)
		if(@ChallanNumber<>'''')
		begin		
			INSERT INTO #TmpGetChallanNumber (ChallanNumber) values (@ChallanNumber)
		end
		else
		begin
			set @ChallanNumber= (select max(challannumber)+ 1 from entchallan where BranchId=@BranchId)
			INSERT INTO #TmpGetChallanNumber (ChallanNumber) values (@ChallanNumber)
		end
		if(@ChallanNumber is null)
		begin
			delete from #TmpGetChallanNumber
			set @ChallanNumber=''1''
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
		SELECT * FROM MstConfigSettings where BranchId=@BranchId
		END
	ELSE IF(@Flag=29)
		BEGIN
			SELECT convert(varchar,NetAmount) As CustomerName ,Qty AS CustomerMobile FROM EntBookings WHERE BookingByCustomer=@CustCode and BookingNumber=@BookingNumber And BranchId=@BranchId
			
			SELECT COALESCE(CustomerSalutation,'''') + '' '' + [CustomerName] As CustomerName , CustomerMobile FROM  dbo.CustomerMaster where CustomerCode=@CustCode And BranchId=@BranchId
			
		END
	END










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_InsUpd_FirstTimeConfigSettings]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Manoj Gupta>
-- Create date: <20 Aug 2011>
-- Description:	<To save config settings>
-- =============================================
CREATE PROCEDURE [dbo].[Sp_InsUpd_FirstTimeConfigSettings]
	(		
		@WebsiteName varchar(max)='''',
		@StoreName varchar(max)='''',
		@Address varchar(max)='''',
		@Timing varchar(max)='''',
		@FooterName varchar(max)='''',
		@SetSlipInch int='''',
		@Printing bit ='''',
		@Configuration bit =''''
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
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Dry_DefaultDataInMasters]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

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
			INSERT INTO UserTypeMaster (UserTypeID,UserType,UserTypeDetails) VALUES (''1'',''Administrator'',''BuiltInAdministrator'')
			INSERT INTO UserTypeMaster (UserTypeID,UserType,UserTypeDetails) VALUES (''2'',''SuperVisor'',''SuperVisor'')
			INSERT INTO UserTypeMaster (UserTypeID,UserType,UserTypeDetails) VALUES (''3'',''User'',''General User'')		
			
			--select * from usermaster
			-- Insert Data In The First Time UserTypeMaster
			INSERT INTO UserMaster (UserId,UserPassword,UserTypeCode,UserBranchCode,UserName,UserAddress,UserPhoneNumber,UserMobileNumber,UserEmailId,UserActive) VALUES (''admin'',''admin'',''1'',''HO'',''Admin'','''','''','''','''',''1'')		

			--select * from BranchMaster
			-- Insert Data In The First Time BranchMaster
			INSERT INTO BranchMaster (BranchCode,BranchName,BranchAddress,BranchPhone,BranchSlogan) values (''HO'',''Quick Dry Cleaning'',''E-1166, Kamla Nagar New Delhi - 110007'',''1123847402'',''Modernize Your Dry Cleaning Business'')	

			--select * from mstcolor
			-- Insert Data In The First Time ColorMaster 
			
			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES (''1'',''Red'',''RD'')
			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES (''2'',''Blue'',''BL'')
			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES (''3'',''Green'',''GR'')
			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES (''4'',''Black'',''BK'')
			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES (''5'',''Gray'',''GY'')			
			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES (''6'',''MUSTAD'',''MU'')
			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES (''7'',''ORANGE'',''OR'')
			INSERT INTO mstColor (ID,ColorName,ColorCode) VALUES (''8'',''GOLDEN'',''GO'')
			
			-- Insert Data In The First Time ProcessMaster 
			
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES (''ALT'',''Alteration'',''FALSE'',''FALSE'',''0'',''FALSE'',''FALSE'')
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES (''CL'',''Calendering'',''FALSE'',''FALSE'',''0'',''FALSE'',''FALSE'')
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES (''DC'',''Dry Cleaning'',''FALSE'',''FALSE'',''0'',''FALSE'',''FALSE'')
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES (''DY'',''DYE'',''FALSE'',''FALSE'',''0'',''FALSE'',''FALSE'')
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES (''RE'',''Repairing'',''FALSE'',''FALSE'',''0'',''FALSE'',''FALSE'')
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES (''RF'',''Raffu'',''FALSE'',''FALSE'',''0'',''FALSE'',''FALSE'')
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES (''SP'',''Steam Press'',''FALSE'',''FALSE'',''0'',''FALSE'',''FALSE'')
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES (''ST'',''Starch'',''FALSE'',''FALSE'',''0'',''FALSE'',''FALSE'')
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount) VALUES (''WC'',''Wet Cleaning'',''FALSE'',''FALSE'',''0'',''FALSE'',''FALSE'')
			

			--select * from configurationsetting
			INSERT INTO ConfigurationSetting (WebsiteName,StoreName,Address,Timing,FooterName,Printing,Configuration,SetSlipInch) VALUES (''http://www.quickdrycleaning.com'',''Quick Dry Cleaning Software'',''E-166, Kamla Nagar, New Delhi -7'',''Mon - Fri 9 to 6 , Sat - Sun 10 to 5 .'',''Visit us for garment care, carpet & curtains care and car dry celaning'',''1'',''1'',''3'')

			-- select * from mstRemark			
			INSERT INTO mstRemark (ID,Remarks) VALUES (''1'',''Stain'')
			INSERT INTO mstRemark (ID,Remarks) VALUES (''2'',''Press Mark'')
			INSERT INTO mstRemark (ID,Remarks) VALUES (''3'',''Faded'')
			INSERT INTO mstRemark (ID,Remarks) VALUES (''4'',''Loose Buttons'')
			INSERT INTO mstRemark (ID,Remarks) VALUES (''5'',''Button Missing'')
			INSERT INTO mstRemark (ID,Remarks) VALUES (''6'',''Cuts'')
			INSERT INTO mstRemark (ID,Remarks) VALUES (''7'',''Rust Stain'')

			-- select * from PriorityMaster
			INSERT INTO PriorityMaster (PriorityID,Priority) VALUES (''0'',''Hard Starch'')
			INSERT INTO PriorityMaster (PriorityID,Priority) VALUES (''1'',''No Starch'')
			INSERT INTO PriorityMaster (PriorityID,Priority) VALUES (''2'',''Medium Starch'')
			INSERT INTO PriorityMaster (PriorityID,Priority) VALUES (''3'',''Suits with hanger'')
			INSERT INTO PriorityMaster (PriorityID,Priority) VALUES (''4'',''Flat press on pleetless trousers'')
			INSERT INTO PriorityMaster (PriorityID,Priority) VALUES (''5'',''Tie in separate packing'')
			
			-- select * from CustomerMaster
			DECLARE @CurentDate DATETIME
			SET @CurentDate=(GETDATE())
			INSERT INTO CustomerMaster (ID,CustomerCode,CustomerSalutation,CustomerName,CustomerAddress,CustomerPhone,CustomerMobile,CustomerEmailId,CustomerPriority,CustomerRefferredBy,CustomerRegisterDate,CustomerIsActive,CustomerCancelDate,DefaultDiscountRate,Remarks,BirthDate,AnniversaryDate,AreaLocation) VALUES (''1'',''Cust1'',''Mr'',''Dee Coup'',''E-166, Kamla Nagar, New Delhi-110007'',''9810755331'',''9212515278'',''info@deecoup.com'',''1'','''',@CurentDate,''1'',@CurentDate,''10'','''','''','''',''Kamla Nagar'')
			INSERT INTO CustomerMaster (ID,CustomerCode,CustomerSalutation,CustomerName,CustomerAddress,CustomerPhone,CustomerMobile,CustomerEmailId,CustomerPriority,CustomerRefferredBy,CustomerRegisterDate,CustomerIsActive,CustomerCancelDate,DefaultDiscountRate,Remarks,BirthDate,AnniversaryDate,AreaLocation) VALUES (''2'',''Cust2'',''Mr'',''VIVEK'',''DFSADFJKLASDFKLADSF'','''',''9876543456'','''',''0'',''None'',@CurentDate,''1'','''',''0'','''','''','''',''LKJHGFGHJKL;'')
			INSERT INTO CustomerMaster (ID,CustomerCode,CustomerSalutation,CustomerName,CustomerAddress,CustomerPhone,CustomerMobile,CustomerEmailId,CustomerPriority,CustomerRefferredBy,CustomerRegisterDate,CustomerIsActive,CustomerCancelDate,DefaultDiscountRate,Remarks,BirthDate,AnniversaryDate,AreaLocation) VALUES (''3'',''Cust3'',''Mr'',''RAJKUMAR SINGH'',''DFSDF'','''',''9818249915'','''',''0'',''None'',@CurentDate,''1'','''',''0'',''DFHAKSJDHFKJASD'','''','''',''JKDSFHKJASHDF'')

			--- SELECT * FROM LEDGERMASTER
			INSERT INTO LedgerMaster (LedgerName,OpenningBalance,CurrentBalance) VALUES (''CASH'',''0'',''0'')
			INSERT INTO LedgerMaster (LedgerName,OpenningBalance,CurrentBalance) VALUES (''Cust1'',''0'',''0'')
			INSERT INTO LedgerMaster (LedgerName,OpenningBalance,CurrentBalance) VALUES (''Cust2'',''0'',''0'')
			INSERT INTO LedgerMaster (LedgerName,OpenningBalance,CurrentBalance) VALUES (''Cust3'',''0'',''0'')
			INSERT INTO LedgerMaster (LedgerName,OpenningBalance,CurrentBalance) VALUES (''Sales'',''0'',''0'')

			
			-- select * from mstReceiptConfig
			INSERT INTO mstReceiptConfig (HeaderText,HeaderFontName,HeaderFontSize,HeaderFontStyle,AddressText,AddressFontName,AddressFontSize,AddressFontStyle,LogoLeftRight,LogoHeight,LogoWeight,SloganText,SloganFontName,SloganFontSize,SloganFontStyle,Barcode,PreviewDue,Term1,Term2,Term3,Term4,FooterSloganText,FooterSloganFontName,FooterSloganFontSize,FooterSloganFontStyle,ST,PrinterLaserOrDotMatrix,PrintLogoonReceipt,PrePrinted,TextFontStyleUL,TextFontItalic,TextAddressUL,TextAddressItalic,HeaderSloganUL,HeaderSloganItalic,FooterSloganUL,FooterSloganItalic,ShowHeaderSlogan,ShowFooterSlogan,TermsAndConditionTrue,DotMatrixPrinter40Cloumn,TableBorder,NDB,NDI,NDU,NDFName,NDFSize,NAB,NAI,NAU,NAFName,NAFSize,CurrencyType) VALUES (''Quick DryCleaning Software'',''Arial'',''30'',''Bold'',''E-166, kamla Nagar ,Delhi-07'',''Arial'',''15'',''Bold'',''1'',''1'',''1'',''Operation Timing 10 am to 8 pm | Monday Closed'',''Arial'',''15'','''',''True'',''True'',''All Garments are accept at Owner risk.'',''All payments should cleared before delivery.'',''Clothes Once Delivered will not  be taken back.'','''',''Happy New Year'',''Arial'',''15'','''',''True'',''True'',''True'',''True'',''underline'',''italic'',''underline'',''italic'','''','''','''','''',''True'',''True'',''True'',''False'',''True'',''Bold'',''Italic'',''underline'',''Arial'',''12'',''Bold'',''Italic'',''underline'',''Arial'',''12'',''Rs'')

			-- select * from EmployeeMaster			
			INSERT INTO dbo.EmployeeMaster (ID,EmployeeCode,EmployeeSalutation,EmployeeName,EmployeeAddress,EmployeePhone,EmployeeMobile,EmployeeEmailId) VALUES (''1'',''Emp1'',''Mr'',''Vivek Saini'',''DC Web Services Pvt Ltd.  E-166, Kamla Nagar New Delhi-110007'',''9810755331'',''9212515278'',''Vivek.Saini@deecoup.com'')

			--- Insert Data In the mstRecordCheck

			INSERT INTO mstRecordCheck (Status) values (''Save'')		

			INSERT INTO mstjobtype (ID,JobType) values (''1'',''Manager'')		

			INSERT INTO ShiftMaster (ShiftName) values (''5 PM'')	
			
			-----------------------------------------------SET Menu in Our Screen---------------------------
			----- When User Type 1-----------------
			
			
			
	END 

select * from entmenurights



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ShiftMaster]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Manoj Kumar Gupta>
-- Create date: <27-Feb-2012>
-- Description:	<Shift Master>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ShiftMaster]	
	@Flag VARCHAR(MAX)='''',
	@ShiftName VARCHAR(MAX)='''',
	@BranchId VARCHAR(10)='''',
	@ShiftID INT=''''
	
AS
BEGIN	
	IF(@Flag = 1)
		BEGIN
		INSERT INTO ShiftMaster (ShiftName,BranchId) VALUES (@ShiftName,@BranchId)
		END
	IF(@Flag = 2)
		BEGIN
		UPDATE ShiftMaster SET ShiftName=@ShiftName WHERE BranchId=@BranchId AND ShiftID=@ShiftID
		END
	IF(@Flag = 3)
		BEGIN
		SELECT * FROM ShiftMaster WHERE BranchId=@BranchId
		END
	IF(@Flag = 4)
		BEGIN
		if(@ShiftName='''')
		SELECT * FROM ShiftMaster WHERE BranchId=@BranchId order by ShiftID desc
		else
		SELECT * FROM ShiftMaster WHERE BranchId=@BranchId AND ShiftName like ''%''+@ShiftName+''%'' order by ShiftID desc
		END
	IF(@Flag = 5)
		BEGIN
		DELETE FROM ShiftMaster WHERE ShiftName=@ShiftName AND BranchId=@BranchId
		END
	IF(@Flag = 6)
		BEGIN
		SELECT * FROM ShiftMaster WHERE ShiftName=@ShiftName AND BranchId=@BranchId
		END
END




' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Remarks]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Manoj Kumar Gupta>
-- Create date: <27-Feb-2012>
-- Description:	<Remark Master>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Remarks]	
	@Flag VARCHAR(MAX)='''',
	@Remarks VARCHAR(MAX)='''',
	@BranchId VARCHAR(10)='''',
	@ID INT=''''
	
AS
BEGIN	
	IF(@Flag = 1)
		BEGIN
		INSERT INTO mstRemark (Remarks,BranchId) VALUES (@Remarks,@BranchId)
		END
	IF(@Flag = 2)
		BEGIN
		UPDATE mstRemark SET Remarks=@Remarks WHERE BranchId=@BranchId AND ID=@ID
		END
	IF(@Flag = 3)
		BEGIN
		SELECT * FROM mstRemark WHERE BranchId=@BranchId order by ID desc
		END
	IF(@Flag = 4)
		BEGIN
		SELECT * FROM mstRemark WHERE BranchId=@BranchId AND Remarks like ''%''+@Remarks+''%'' order by ID desc
		END
	IF(@Flag = 5)
		BEGIN
		DELETE FROM mstRemark WHERE Remarks=@Remarks AND BranchId=@BranchId
		END
	IF(@Flag = 6)
		BEGIN
		SELECT * FROM mstRemark WHERE Remarks=@Remarks AND BranchId=@BranchId
		END
END



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_NewBooking]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <03-Nov-2011>
-- Description:	<>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NewBooking]	
	@Flag VARCHAR(MAX)='''',
	@Priority VARCHAR(MAX)='''',
	@CustName VARCHAR(MAX)='''',
	@CustAddress VARCHAR(MAX)='''',
	@CustTitle VARCHAR(MAX)='''',	
	@CustMobile VARCHAR(MAX)='''',
	@CustRemarks VARCHAR(MAX)='''',
	@CustPriority VARCHAR(MAX)='''',
	@CustCode VARCHAR(MAX)='''',
	@CustAreaLocation VARCHAR(MAX)='''',
	@ItemSearchName VARCHAR(MAX)='''',
	@ItemName VARCHAR(MAX)='''',
	@NumberOfSubItems VARCHAR(MAX)='''',
	@ItemCode VARCHAR(MAX)='''',
	@SubItem1 VARCHAR(MAX)='''',
	@SubItem2 VARCHAR(MAX)='''',
	@SubItem3 VARCHAR(MAX)='''',
	@CustomerName VARCHAR(MAX)='''',
	@ProcessCode VARCHAR(MAX)='''',
	@ProcessName VARCHAR(MAX)='''',
	@Remarks VARCHAR(MAX)='''',
	@ColorName VARCHAR(MAX)='''',
	@BDate SMALLDATETIME='''',
	@ADate SMALLDATETIME='''',
	@SubItemOrder VARCHAR(MAX)='''',
	@UserTypeId int='''',
	@PageTitle VARCHAR(MAX)='''',
	@FileName VARCHAR(MAX)='''',
	@RightToView VARCHAR(MAX)='''',
	@MenuItemLevel int='''',
	@MenuPosition int='''',
	@ParentMenu VARCHAR(MAX)='''',
	@BookingNo varchar(max)='''',
	@SaveItemName varchar(max)='''',
	@BookingItemName varchar(max)='''',
	@BarcodeISN varchar(max)='''',
	@BranchId VARCHAR(MAX)=''''
	
AS
BEGIN	
	IF(@Flag = 1)
		SELECT ''1'' PriorityID,'' ----------------- NONE ----------------- '' Priority
		UNION
		SELECT PriorityID,upper(Priority) FROM PriorityMaster WHERE BranchId=@BranchId
	IF(@Flag = 2)
		BEGIN
			DECLARE @ID INT
			SELECT @ID=COALESCE(MAX(PriorityID),0) FROM PriorityMaster	WHERE BranchId=@BranchId
			SET @ID = @ID + 1
			INSERT INTO PriorityMaster VALUES(@ID, @Priority,@BranchId,''NULL'')			
		END
	IF(@Flag = 3)
		BEGIN
			DECLARE @N_ID VARCHAR
			SET @N_ID = ''0''
			SELECT @N_ID = COUNT(ID) FROM CustomerMaster WHERE  CustomerName=@CustName AND CustomerAddress=@CustAddress AND BranchId=@BranchId
			IF(@N_ID=''0'')
				BEGIN
					DECLARE @NEW_ID INT
					SELECT @NEW_ID = COALESCE(MAX(ID),0) FROM CustomerMaster WHERE BranchId=@BranchId
					SET @NEW_ID = @NEW_ID + 1
					INSERT INTO CustomerMaster (ID,CustomerCode,CustomerSalutation,CustomerName,CustomerAddress,CustomerMobile,Remarks,CustomerPriority,AreaLocation,CustomerRegisterDate,BirthDate,AnniversaryDate,BranchId)
					VALUES (@NEW_ID,''Cust''+ CONVERT(VARCHAR,@NEW_ID),@CustTitle,@CustName,@CustAddress,@CustMobile,@CustRemarks,@CustPriority,@CustAreaLocation,GETDATE(),@BDate,@ADate,@BranchId)
					
					INSERT INTO LedgerMaster Values(''Cust''+ CONVERT(VARCHAR,@NEW_ID),''0'',''0'',@branchId,NULL)	
					
					SELECT ''Cust''+ CONVERT(VARCHAR,@NEW_ID) AS CustCode
				END					
		END
	 IF (@Flag=4)
		BEGIN
			declare @id1 int			
			set @id1= (select customerpriority from customermaster where customercode=@CustCode AND BranchId=@BranchId)				
			IF(@id1<>0)
			BEGIN
				SELECT  dbo.CustomerMaster.CustomerSalutation + '' '' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, 
						dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority
				FROM    dbo.CustomerMaster INNER JOIN
						dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID
				where dbo.CustomerMaster.customercode=@CustCode  AND dbo.CustomerMaster.BranchId=@BranchId
			END
		ELSE
			BEGIN

				CREATE TABLE #TmpTable (CustName varchar(max),CustAddress varchar(max),CustMobile varchar(max),CustRemarks varchar(max),CustPriority varchar(max))
				INSERT INTO #TmpTable( CustName, CustAddress, CustMobile,CustRemarks,CustPriority) SELECT CustName, CustAddress,CustMobile,CustRemarks,CustPriority
				FROM(
				SELECT  CustomerSalutation + '' '' + CustomerName AS CustName, CustomerAddress AS CustAddress, CustomerMobile AS CustMobile, Remarks AS CustRemarks,''NONE'' AS CustPriority FROM   dbo.CustomerMaster
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
			DECLARE @DateOffset varchar(max)
			DECLARE @MainDate DATETIME
			DECLARE @BookDate1 INT	
			DECLARE @DueTime VARCHAR(MAX)	
			SELECT @BookDate1=DeliveryDateOffset,@DueTime=DefaultTime+'' ''+DefaultAmPm ,@DateOffset=DeliveryDateOffset FROM mstConfigSettings WHERE BranchId=@Branchid	
			SET @MainDate= (SELECT convert(varchar, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0) + @BookDate1,106))				
			SELECT CONVERT(VARCHAR,@MainDate,106) AS DueDate,@DueTime AS DueTime,@DateOffset AS DateOffSet		
		END	
	IF(@Flag=7)
		BEGIN
				SELECT dbo.ItemMaster.ItemName, dbo.ProcessMaster.ProcessCode as ProcessName FROM dbo.MstConfigSettings INNER JOIN  dbo.ItemMaster ON dbo.MstConfigSettings.DefaultItemId = dbo.ItemMaster.ItemID INNER JOIN  dbo.ProcessMaster ON dbo.MstConfigSettings.DefaultProcessCode = dbo.ProcessMaster.ProcessCode  WHERE dbo.MstConfigSettings.BranchId=@BranchId
		END
	IF(@Flag=8)
		BEGIN
				SELECT ItemCode +'' - ''+ ItemName AS ItemName FROM ItemMaster WHERE BranchId=@BranchId AND ItemCode like ''%''+@ItemSearchName+''%'' or ItemName like ''%''+@ItemSearchName+''%''
		END
	IF(@Flag=9)
		BEGIN
				--declare @SaveItemName varchar(max)
--				declare @ItemName varchar(max)
--				declare @NumberOfSubItems varchar(max)
--				set @NumberOfSubItems=2
--				set @ItemName=''Charas''
				if(@NumberOfSubItems<= 1)
				begin
					set @SaveItemName=@ItemName					
				end
				else
				begin
					set @SaveItemName=@ItemName+ '' '' + ''('' + @NumberOfSubItems + ''pcs)''					
				end
				Insert Into ItemMaster (ItemName, NumberOfSubItems,ItemCode) Values (@SaveItemName,@NumberOfSubItems,@ItemCode)
				DELETE FROM EntSubItemDetails WHERE ItemName=@ItemName
		--- WHEN QTY IS 1
				if(@NumberOfSubItems>0)
				begin
				if(@SubItem1='''')
				begin
					set @SubItem1=@SaveItemName
				end
				INSERT INTO EntSubItemDetails (ItemName, SubItemName, SubItemOrder) VALUES (@SaveItemName,@SubItem1,''1'')
				end
		--- WHEN QTY IS 2
				if(@NumberOfSubItems>1)
				begin
				INSERT INTO EntSubItemDetails (ItemName, SubItemName, SubItemOrder) VALUES (@SaveItemName,@SubItem2,''2'')
				end
		--- WHEN QTY IS 3 
				if(@NumberOfSubItems>2)
				begin
				INSERT INTO EntSubItemDetails (ItemName, SubItemName, SubItemOrder) VALUES (@SaveItemName,@SubItem3,''3'')
				end	
				SELECT @SaveItemName AS ItemNameFillGrid			
		END
	ELSE IF (@Flag=10)
		BEGIN
			if(@CustomerName<>'''' AND @CustAddress<>'''' AND @CustMobile<>'''')
				SELECT  dbo.CustomerMaster.CustomerCode AS CustomerCode,dbo.CustomerMaster.CustomerSalutation + '' '' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority FROM dbo.CustomerMaster INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID  WHERE dbo.CustomerMaster.BranchId=@BranchId AND dbo.CustomerMaster.CustomerName LIKE ''%''+@CustomerName+''%'' AND dbo.CustomerMaster.CustomerAddress LIKE ''%''+@CustAddress+''%'' AND dbo.CustomerMaster.CustomerMobile LIKE ''%''+@CustMobile+''%'' ORDER BY CustomerName ASC
			ELSE IF(@CustomerName='''' AND @CustAddress='''' AND @CustMobile='''') 
				SELECT  dbo.CustomerMaster.CustomerCode AS CustomerCode,dbo.CustomerMaster.CustomerSalutation + '' '' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority FROM dbo.CustomerMaster INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID  WHERE dbo.CustomerMaster.BranchId=@BranchId ORDER BY CustomerName ASC
			ELSE IF(@CustomerName<>'''' AND @CustAddress<>'''' AND @CustMobile='''') 
				SELECT  dbo.CustomerMaster.CustomerCode AS CustomerCode,dbo.CustomerMaster.CustomerSalutation + '' '' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority FROM dbo.CustomerMaster INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID  WHERE dbo.CustomerMaster.BranchId=@BranchId AND dbo.CustomerMaster.CustomerName LIKE ''%''+@CustomerName+''%'' AND dbo.CustomerMaster.CustomerAddress LIKE ''%''+@CustAddress+''%'' ORDER BY CustomerName ASC
			ELSE IF(@CustomerName='''' AND @CustAddress<>'''' AND @CustMobile<>'''') 
				SELECT  dbo.CustomerMaster.CustomerCode AS CustomerCode,dbo.CustomerMaster.CustomerSalutation + '' '' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority FROM dbo.CustomerMaster INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID  WHERE dbo.CustomerMaster.BranchId=@BranchId AND dbo.CustomerMaster.CustomerAddress LIKE ''%''+@CustAddress+''%'' AND dbo.CustomerMaster.CustomerMobile LIKE ''%''+@CustMobile+''%'' ORDER BY CustomerName ASC
			ELSE IF(@CustomerName<>'''' AND @CustAddress='''' AND @CustMobile='''') 
				SELECT  dbo.CustomerMaster.CustomerCode AS CustomerCode,dbo.CustomerMaster.CustomerSalutation + '' '' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority FROM dbo.CustomerMaster INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID  WHERE dbo.CustomerMaster.BranchId=@BranchId AND dbo.CustomerMaster.CustomerName LIKE ''%''+@CustomerName+''%'' ORDER BY CustomerName ASC
			ELSE IF(@CustomerName='''' AND @CustAddress='''' AND @CustMobile<>'''') 
				SELECT  dbo.CustomerMaster.CustomerCode AS CustomerCode,dbo.CustomerMaster.CustomerSalutation + '' '' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority FROM dbo.CustomerMaster INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID  WHERE dbo.CustomerMaster.BranchId=@BranchId AND dbo.CustomerMaster.CustomerMobile LIKE ''%''+@CustMobile+''%'' ORDER BY CustomerName ASC
			ELSE IF(@CustomerName<>'''' AND @CustAddress='''' AND @CustMobile<>'''') 
				SELECT  dbo.CustomerMaster.CustomerCode AS CustomerCode,dbo.CustomerMaster.CustomerSalutation + '' '' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority FROM dbo.CustomerMaster INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID  WHERE dbo.CustomerMaster.BranchId=@BranchId AND dbo.CustomerMaster.CustomerName LIKE ''%''+@CustomerName+''%'' AND dbo.CustomerMaster.CustomerMobile LIKE ''%''+@CustMobile+''%'' ORDER BY CustomerName ASC
			ELSE IF(@CustomerName='''' AND @CustAddress<>'''' AND @CustMobile='''') 
				SELECT  dbo.CustomerMaster.CustomerCode AS CustomerCode,dbo.CustomerMaster.CustomerSalutation + '' '' + dbo.CustomerMaster.CustomerName AS CustName, dbo.CustomerMaster.CustomerAddress AS CustAddress, dbo.CustomerMaster.CustomerMobile AS CustMobile, dbo.CustomerMaster.Remarks AS CustRemarks, dbo.PriorityMaster.Priority AS CustPriority FROM dbo.CustomerMaster INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID  WHERE dbo.CustomerMaster.BranchId=@BranchId AND dbo.CustomerMaster.CustomerAddress LIKE ''%''+@CustAddress+''%'' ORDER BY CustomerName ASC
		END
	ELSE IF (@Flag=11)
		BEGIN
			SELECT (CustomerCode + ''  '' + ''-'' + ''  '' + CustomerName + '' '' + CustomerAddress + '' '' + CustomerMobile) AS CustName FROM   dbo.CustomerMaster  WHERE  BranchId=@BranchId AND customername like ''%''+@CustomerName+''%'' or CustomerAddress like ''%''+@CustomerName+''%'' or CustomerMobile like ''%''+@CustomerName+''%'' order by customername asc

		END	
	ELSE IF (@Flag=12)
		BEGIN
			INSERT INTO ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,BranchID) VALUES (@ProcessCode,@ProcessName,''FALSE'',''0'',''0'',''FALSE'',@BranchId)
		END
	ELSE IF(@Flag=13)
		BEGIN
			SELECT ProcessCode FROM PROCESSMASTER WHERE ProcessCode=@ProcessCode AND BranchID=@BranchId
		END
	ELSE IF(@Flag=14)
		BEGIN
			IF NOT EXISTS(SELECT * FROM MstRemark where Remarks=@Remarks AND BranchId=@BranchId)
			BEGIN
			DECLARE @Remark_ID INT
			SELECT @Remark_ID = COALESCE(MAX(ID),0) FROM MstRemark
			SET @Remark_ID = @Remark_ID + 1
			INSERT INTO MstRemark (ID,Remarks,BranchId) VALUES (@Remark_ID,@Remarks,@BranchId)
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
			WHERE (dbo.ProcessMaster.BranchId=@BranchId) AND (dbo.ItemwiseProcessRate.ItemName = @ItemName) AND (dbo.ProcessMaster.ProcessCode = @ProcessName))
			if(@ProcessPrice<>'''')
			begin
				SELECT  dbo.ItemwiseProcessRate.ProcessPrice FROM dbo.ItemMaster INNER JOIN dbo.ItemwiseProcessRate ON dbo.ItemMaster.ItemName = dbo.ItemwiseProcessRate.ItemName INNER JOIN dbo.ProcessMaster ON dbo.ItemwiseProcessRate.ProcessCode = dbo.ProcessMaster.ProcessCode WHERE (dbo.ProcessMaster.BranchId=@BranchId) AND (dbo.ItemwiseProcessRate.ItemName = @ItemName) AND (dbo.ProcessMaster.ProcessCode = @ProcessName)
			end
			else
			begin
				SELECT ''0'' AS ProcessPrice
			end
		END
	ELSE IF(@Flag=18)
		BEGIN
			SELECT COALESCE(MAX(Convert(int, BookingNumber)),0) FROM EntBookings WHERE BranchId=@BranchId
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
		SELECT TOP(1) ItemName FROM ItemMaster WHERE BranchId=@BranchId ORDER BY itemid DESC
	END
	ELSE IF(@Flag=24)
	BEGIN
		SELECT BookingNumber FROM EntBookings WHERE BookingNumber=@BookingNo
	END
	ELSE IF(@Flag=25)
	BEGIN
		SELECT IsDiscount FROM ProcessMaster WHERE ProcessCode=@ProcessCode AND BranchId=@BranchId
	END
	ELSE IF(@Flag=26)
	BEGIN
		SELECT CurrencyType FROM mstReceiptConfig 
	END
	ELSE IF(@Flag=27)
	BEGIN
		SELECT AmountType FROM MstConfigSettings WHERE BranchId=@BranchId			
	END
	ELSE IF(@Flag=28)
	BEGIN
		SELECT DeliveredQty,DeliverItemStaus,RemoveFrom,Item FROM Barcodetable where BookingItemName=@BookingItemName and BarcodeISN=@BarcodeISN and BookingNo=@BookingNo
	END
END













' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Dry_BarcodeMaster]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[sp_Dry_BarcodeMaster]		
	@Flag VARCHAR(MAX)='''',
	@ID int='''',
	@BookingDate smalldatetime='''',
	@CurrentTime VARCHAR(MAX)='''',
	@DueDate smalldatetime='''',
	@Item  VARCHAR(MAX)='''',
	@BarCode VARCHAR(MAX)='''',
	@Process VARCHAR(MAX)='''',
	@StatusId int='''',
	@BookingNo VARCHAR(MAX)='''',
	@SNo int='''',
	@RowIndex int='''',
	@BookingByCustomer varchar(max)='''',
	@Colour varchar(max)='''',
	@ItemExtraprocessType varchar(max)='''',
	@DrawlStatus bit=false,
	@DrawlAlloted bit=false,
	@DrawlName varchar(100)=null,
	@AllottedDrawl varchar(max)='''',
	@DeliveredQty int='''',
	@BookingNumber varchar(max)='''',
	@ItemName varchar(max)='''',
	@ItemsReceivedFromVendor int='''',
	@BookingUser varchar(max)='''',
	@BookDate1 datetime='''',
	@BookDate2 datetime='''',
	@ReceiptDeliverd bit='''',	
	@Qty int='''',
	@ItemRemarks varchar(max)='''',
	@CustName varchar(max)='''',
	@ItemTotalandSubTotal varchar(max)='''',
	@UserTypeId int='''',
	@BranchId VARCHAR(MAX)=''''
	
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
			UPDATE dbo.BarcodeTable SET DrawlStatus=@DrawlStatus,DrawlAlloted=@DrawlAlloted WHERE BookingNo=@BookingNo AND RowIndex=@RowIndex And BranchId=@BranchId
		END	
	ELSE IF(@Flag=3)
		BEGIN
			SELECT dbo.BarcodeTable.BookingNo AS BookingNumber, dbo.BarcodeTable.Item, dbo.BarcodeTable.RowIndex FROM dbo.EntChallan INNER JOIN  dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex WHERE (dbo.BarcodeTable.DrawlStatus = ''true'') AND (dbo.BarcodeTable.DrawlAlloted = ''false'') order by dbo.BarcodeTable.RowIndex asc
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
			WHERE  (dbo.EntBookings.BookingNumber =@BookingNumber) And dbo.EntBookings.BranchId=@BranchId
		END
	ELSE IF(@Flag=8)
		BEGIN	
			Update EntChallan Set ItemsReceivedFromVendor=''1'', ItemReceivedFromVendorOnDate=getdate() Where BookingNumber=@BookingNo And ItemSno=@RowIndex AND SubItemName=@ItemName And BranchId=@BranchId
		END
	ELSE IF(@Flag=9)
		BEGIN	
			UPDATE BarcodeTable SET DrawlStatus=''True'',DrawlAlloted=''False'',  StatusId = ''3''  where BookingNo=@BookingNo and RowIndex=@RowIndex And BranchId=@BranchId
		END
	ELSE IF(@Flag=10)
		BEGIN	
			UPDATE EntBookings SET BarCode=@BarCode where BookingNumber=@BookingNo
		END
	ELSE IF(@Flag=11)
		BEGIN	
			SELECT sum(ItemTotalQuantity) as ItemTotalQuantity FROM EntBookingDetails Where BookingNumber=@BookingNo AND BranchId=@BranchId
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
			SELECT IsActiveServiceTax FROM ProcessMaster WHERE ProcessCode=@Process AND BranchId=@BranchId
		END
	ELSE IF (@Flag=16)
		BEGIN
			SELECT ServiceTax FROM ProcessMaster WHERE ProcessCode=@Process AND BranchId=@BranchId
		END
	ELSE IF (@Flag=17)
		BEGIN
			SELECT RightToView FROM EntMenuRights WHERE (UserTypeId = @UserTypeId) AND PageTitle=''Edit Booking''
		END
	ELSE IF (@Flag=18)
		BEGIN
			Select * From EntMenuRights Where UserTypeId=@UserTypeId AND PageTitle<>''Edit Booking'' AND BranchId=@BranchId   ORDER BY MenuItemLevel, MenuPosition
		END
	ELSE IF (@Flag=19)
		BEGIN
			select * from mstRecordCheck
		END
	ELSE IF (@Flag=20)
		BEGIN
			insert into mstRecordCheck (Status) values (''Save'')			
		END
	ELSE IF (@Flag=21)
		BEGIN
			SELECT DeliverItemStaus from BarcodeTable where bookingno=@BookingNumber AND BranchId=@BranchId
		END
	ELSE IF (@Flag=22)
		BEGIN
			SELECT NetAmount FROM EntBookings where BookingNumber=@BookingNumber AND BranchId=@BranchId
			SELECT sum(PaymentMade) as PaymentMade FROM EntPayment where BookingNumber=@BookingNumber AND BranchId=@BranchId
		END
	ELSE IF (@Flag=23)
		BEGIN
			UPDATE EntBookings SET ReceiptDeliverd=@ReceiptDeliverd where BookingNumber=@BookingNumber and BranchId=@BranchId
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
			set @LedgerName=(select ''(''+customercode+'')''+ +'' ''+ CustomerSalutation+'' ''+ CustomerName as name1 from customermaster where customercode=@CustName and BranchId=@BranchId)
			set @CustomerAddress=(select CustomerAddress from customermaster where customercode=@CustName and BranchId=@BranchId)
			set @TotalDue=(select top(1) ClosingBalance from entledgerentries where ledgername=@CustName and BranchId=@BranchId order by id desc)
			INSERT INTO #TmpAcountTable(CustomerName,CustomerAddress,DuePayment) values (@LedgerName,@CustomerAddress,@TotalDue)
				
			SELECT * FROM #TmpAcountTable
			DROP TABLE #TmpAcountTable 
		END
	ELSE IF (@Flag=27)
		BEGIN
			SELECT (CustomerCode + ''  '' + ''-'' + ''  '' + CustomerName + '' '' + CustomerAddress + '' '' + CustomerMobile) AS CustName FROM   dbo.CustomerMaster  WHERE BranchId=@BranchId AND customername like ''%''+@CustName+''%'' or CustomerAddress like ''%''+@CustName+''%'' or CustomerMobile like ''%''+@CustName+''%''
		END
	ELSE IF(@Flag=28)
		BEGIN			
			declare @TotalQty int,@CustomerName varchar(max),@CustMobile varchar(max),@TotalAmount float
			--set @BookingNumber=''65''
			CREATE TABLE #TmpSMSTable (TotalQty int,CustomerName varchar(max),CustMobile varchar(max),TotalAmount float)
			set @TotalQty=(select sum(sno) as Qty from barcodetable where bookingno=@BookingNumber)
			set @CustomerName=(SELECT dbo.CustomerMaster.CustomerName FROM dbo.CustomerMaster INNER JOIN dbo.EntBookings ON dbo.CustomerMaster.CustomerCode = dbo.EntBookings.BookingByCustomer where entbookings.Bookingnumber=@BookingNumber)
			set @CustMobile=(SELECT dbo.CustomerMaster.CustomerMobile FROM dbo.CustomerMaster INNER JOIN dbo.EntBookings ON dbo.CustomerMaster.CustomerCode = dbo.EntBookings.BookingByCustomer where entbookings.Bookingnumber=@BookingNumber)
			set @TotalAmount=(SELECT dbo.EntBookings.NetAmount FROM dbo.CustomerMaster INNER JOIN dbo.EntBookings ON dbo.CustomerMaster.CustomerCode = dbo.EntBookings.BookingByCustomer where entbookings.Bookingnumber=@BookingNumber)
			INSERT INTO #TmpSMSTable(TotalQty,CustomerName,CustMobile,TotalAmount) values (@TotalQty,@CustomerName,@CustMobile,@TotalAmount)
				
			SELECT * FROM #TmpSMSTable
			DROP TABLE #TmpSMSTable 
		END
	ELSE IF (@Flag=29)
		BEGIN
			SELECT * FROM ProcessMaster WHERE ProcessCode = @Process
		END	
	ELSE IF (@Flag=30)
		BEGIN
			SELECT PageTitle, RightToView FROM EntMenuRights WHERE (UserTypeId = ''1'') AND BranchId=@BranchId ORDER BY MenuItemLevel, PageTitle
		END	  
	END

--update EntMenuRights set RightToView=''true'' WHERE (UserTypeId = ''1'')














' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proc_SalesandDelivery]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'













-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <4-Feb-2012>
-- Description:	<Sales and Delivery>
-- =============================================
CREATE PROCEDURE [dbo].[Proc_SalesandDelivery]
	@FDate DATETIME='''',
	@UDate DATETIME='''',
	@BranchId VARCHAR(MAX)=''''
	
AS
BEGIN
DECLARE @TotalAmt VARCHAR(MAX),@BDAmt VARCHAR(MAX),@ST VARCHAR(MAX),@DelAmt VARCHAR(MAX), @FRDATE DATETIME,@UPDATE DATETIME, @BookingDate VARCHAR(MAX),@BookingNumber VARCHAR(MAX),@CustomerName VARCHAR(MAX),@TotalQty INT, @AlreadyDelivered INT, @DelQty INT,@ClothDeliveryDate VARCHAR(MAX),@BalQty VARCHAR(MAX),@NetAmount VARCHAR(MAX), @AlreadyPaid FLOAT, @PaymentMade VARCHAR(MAX),@BalAmt VARCHAR(MAX),@PaymentDate VARCHAR(MAX)
CREATE TABLE #SaleAndDelivery(BookingDate VARCHAR(MAX),BookingNumber VARCHAR(MAX),CustomerName VARCHAR(MAX),TotalQty VARCHAR(MAX), AlreadyDelivered VARCHAR(MAX), DelQty VARCHAR(MAX),BalQty VARCHAR(MAX),ClothDeliveryDate VARCHAR(MAX),NetAmount VARCHAR(MAX), AlreadyPaid VARCHAR(MAX), PaymentMade VARCHAR(MAX),BalAmt VARCHAR(MAX),PaymentDate VARCHAR(MAX) ,TotalAmt VARCHAR(MAX),BDAmt VARCHAR(MAX),ST VARCHAR(MAX),DelAmt VARCHAR(MAX))

DECLARE BNumber CURSOR LOCAL FORWARD_Only DYNAMIC Read_Only FOR
	--Read unique booking number from delivery and sales 		
	SELECT dbo.EntBookings.BookingNumber FROM dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.EntBookings.BranchId=@BranchId) AND 	(dbo.BarcodeTable.DelQty = 1) AND dbo.BarcodeTable.ClothDeliveryDate BETWEEN @FDate AND @UDate AND (dbo.EntBookings.BookingStatus <> ''5'')
	UNION
	SELECT dbo.EntBookings.BookingNumber FROM dbo.EntPayment INNER JOIN  dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.EntBookings.BranchId=@BranchId) AND  dbo.EntPayment.PaymentDate BETWEEN @FDate AND @UDate AND (dbo.EntBookings.BookingStatus <> ''5'') ORDER BY dbo.EntBookings.BookingNumber 
	--Open Cursor
	
	OPEN BNumber
		FETCH NEXT FROM BNumber INTO @BookingNumber
		WHILE @@Fetch_Status=0
			BEGIN
				SET @FRDATE = @FDate
				SET @UPDATE = @UDate				
				WHILE @FRDate<=@UPDate
					BEGIN
						SET @AlreadyDelivered=''0''
						SET @DelQty=''0''
						SET @BalQty=''0''
						SET @TotalQty=''0''
						SELECT @NetAmount=NetAmount FROM dbo.EntBookings WHERE BookingNumber=@BookingNumber AND BranchId=@BranchId
						SELECT @TotalQty=dbo.EntBookings.Qty FROM dbo.EntBookings WHERE BookingNumber=@BookingNumber AND BranchId=@BranchId
						SELECT @BookingDate=dbo.EntBookings.BookingDate, @CustomerName=dbo.CustomerMaster.CustomerName, @DelQty=SUM(dbo.BarcodeTable.DelQty) ,@ClothDeliveryDate=dbo.BarcodeTable.ClothDeliveryDate FROM dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.EntBookings.BranchId=@BranchId) AND  (dbo.BarcodeTable.DelQty = 1) and (dbo.BarcodeTable.ClothDeliveryDate = @FRDate) AND (dbo.BarCodeTable.Bookingno=@BookingNumber) GROUP BY dbo.BarcodeTable.ClothDeliveryDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty	ORDER BY dbo.EntBookings.BookingNumber
						SELECT @AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty) FROM dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode	WHERE (dbo.EntBookings.BranchId=@BranchId) AND  (dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.BarcodeTable.ClothDeliveryDate < @FRDate) GROUP BY dbo.EntBookings.BookingNumber
						 
						SELECT @TotalAmt=TotalCost, @BDAmt=DiscountAmt,@ST=(SELECT ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2)   FROM dbo.EntBookingDetails  WHERE      (BookingNumber = dbo.EntBookings.BookingNumber)  GROUP BY BookingNumber) FROM dbo.EntBookings WHERE (BranchId=@BranchId) AND  (BookingStatus <> ''5'') AND  dbo.EntBookings.BookingNumber=@BookingNumber GROUP BY BookingNumber, BookingDate, NetAmount, TotalCost, DiscountAmt ORDER BY BookingNumber
						IF @TotalQty IS NULL
							SET @TotalQty=''0''						
						IF @AlreadyDelivered IS NULL
							SET @AlreadyDelivered=''0''
						IF @DelQty IS NULL 
							SET @DelQty=''0''

						

						SET @BalQty=@TotalQty-(@AlreadyDelivered+@DelQty)
						IF @BalQty IS NULL
							SET @BalQty=''0''
						--SALE REPORT
						SET @AlreadyPaid=(SELECT SUM(dbo.EntPayment.PaymentMade) FROM  dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber WHERE (dbo.EntPayment.PaymentDate < @FRDate) AND (dbo.EntBookings.BookingNumber = @BookingNumber)	)
						IF @AlreadyPaid IS NULL
							SET @AlreadyPaid=''0''
						SET @PaymentMade=(SELECT SUM(dbo.EntPayment.PaymentMade) FROM  dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber WHERE (dbo.EntPayment.PaymentDate = @FRDate) AND (dbo.EntBookings.BookingNumber = @BookingNumber))
						IF(@PaymentMade IS NULL)
							SET @PaymentMade=''0''

						SELECT  @DelAmt=SUM(DiscountOnPayment)  FROM dbo.EntPayment WHERE (BookingNumber = @BookingNumber) AND PaymentDate= @FRDate
						IF @DelAmt IS NULL
							SET @DelAmt=''0''

						SET @BalAmt=@NetAmount-(@AlreadyPaid+@PaymentMade)-@DelAmt
						IF(@ClothDeliveryDate IS NOT NULL) OR @PaymentMade<>''0''
							BEGIN								
								IF(convert(int, @DelQty) <> 0) AND convert(float, @PaymentMade) <> 0		
								BEGIN					
									SELECT @CustomerName=dbo.CustomerMaster.CustomerName, @TotalQty=dbo.EntBookings.Qty, @BookingDate=dbo.EntBookings.BookingDate FROM dbo.EntBookings INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE  dbo.EntBookings.BookingNumber=@BookingNumber
									INSERT INTO #SaleAndDelivery (BookingDate,BookingNumber,CustomerName,TotalQty, AlreadyDelivered, DelQty,BalQty,ClothDeliveryDate,NetAmount, AlreadyPaid, PaymentMade,BalAmt,PaymentDate,TotalAmt,BDAmt,ST,DelAmt) VALUES (CONVERT(VARCHAR,CONVERT(DATETIME,@BookingDate),106),@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered, @DelQty,@BalQty,CONVERT(VARCHAR,CONVERT(DATETIME,@ClothDeliveryDate),106),@NetAmount, @AlreadyPaid, @PaymentMade,@BalAmt,CONVERT(VARCHAR,CONVERT(DATETIME,@FRDate),106),Round(@TotalAmt,2),Round(@BDAmt,2),Round(@ST,2),Round(@DelAmt,2))
								END
								ELSE IF(convert(int, @DelQty) <> 0)	
								BEGIN					
									SELECT @CustomerName=dbo.CustomerMaster.CustomerName, @TotalQty=dbo.EntBookings.Qty, @BookingDate=dbo.EntBookings.BookingDate FROM dbo.EntBookings INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE  dbo.EntBookings.BookingNumber=@BookingNumber
									INSERT INTO #SaleAndDelivery (BookingDate,BookingNumber,CustomerName,TotalQty, AlreadyDelivered, DelQty,BalQty,ClothDeliveryDate,NetAmount, AlreadyPaid, PaymentMade,BalAmt,PaymentDate,TotalAmt,BDAmt,ST,DelAmt) VALUES (CONVERT(VARCHAR,CONVERT(DATETIME,@BookingDate),106),@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered, @DelQty,@BalQty,CONVERT(VARCHAR,CONVERT(DATETIME,@ClothDeliveryDate),106),@NetAmount, @AlreadyPaid, @PaymentMade,@BalAmt,CONVERT(VARCHAR,CONVERT(DATETIME,@FRDate),106),Round(@TotalAmt,2),Round(@BDAmt,2),Round(@ST,2),Round(@DelAmt,2))
								END
								ELSE IF (convert(float, @PaymentMade) <> 0)
								BEGIN					
									SELECT @CustomerName=dbo.CustomerMaster.CustomerName, @TotalQty=dbo.EntBookings.Qty, @BookingDate=dbo.EntBookings.BookingDate FROM dbo.EntBookings INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE  dbo.EntBookings.BookingNumber=@BookingNumber
									INSERT INTO #SaleAndDelivery (BookingDate,BookingNumber,CustomerName,TotalQty, AlreadyDelivered, DelQty,BalQty,ClothDeliveryDate,NetAmount, AlreadyPaid, PaymentMade,BalAmt,PaymentDate,TotalAmt,BDAmt,ST,DelAmt) VALUES (CONVERT(VARCHAR,CONVERT(DATETIME,@BookingDate),106),@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered, @DelQty,@BalQty,CONVERT(VARCHAR,CONVERT(DATETIME,@ClothDeliveryDate),106),@NetAmount, @AlreadyPaid, @PaymentMade,@BalAmt,CONVERT(VARCHAR,CONVERT(DATETIME,@FRDate),106),Round(@TotalAmt,2),Round(@BDAmt,2),Round(@ST,2),Round(@DelAmt,2))
								END
							END
					SET @FRDate=DATEADD(DAY,1,@FRDate)
					END
				
				FETCH NEXT FROM BNumber INTO @BookingNumber
			END
	CLOSE BNumber
	DEALLOCATE BNumber
SELECT * FROM #SaleAndDelivery
DROP TABLE #SaleAndDelivery

END














' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proc_CustClothDelivery]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Sanjeev Arora>
-- Create date: <16-Dec-2012>
-- Description:	<Cloth Delivery Procedure>
-- =============================================
CREATE PROCEDURE [dbo].[Proc_CustClothDelivery] 
@CName varchar(max)=null,
@FromDate varchar(max)=null,
@ToDate varchar(max)=null

AS
BEGIN

declare @BookingDate datetime
declare @BookingDate1 datetime
declare @CustomerName varchar(max)
declare @Custname varchar(max)
declare @TotalQty int 
declare @TotQty int
declare @BookingNumber varchar(max)
declare @InvoiceNo varchar(max)
declare @AlreadyDelivered int

declare @DelQty int
declare @DelQty1 int
declare @ClothDeliveryDate smalldatetime
declare @ClothDate smalldatetime
declare @BalQty int
DECLARE @TempBookingNo VARCHAR(MAX)
declare @Flag int
SET @TempBookingNo=''0''
set @Flag=1

create table #temp(BookingDate datetime,BookingNumber varchar(max),
CustomerName varchar(max),TotalQty int, AlreadyDelivered int, DelQty int,BalQty int,ClothDeliveryDate smalldatetime)

declare Delivery cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for
	SELECT
		dbo.EntBookings.BookingDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.Qty, dbo.CustomerMaster.CustomerName, SUM(dbo.BarcodeTable.DelQty) AS DelQty, dbo.BarcodeTable.ClothDeliveryDate
	FROM
		dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
	WHERE
		(dbo.BarcodeTable.DelQty = 1) and ((dbo.BarcodeTable.ClothDeliveryDate between @FromDate and @ToDate) and dbo.CustomerMaster.CustomerName=@CName)
	GROUP BY 
		dbo.BarcodeTable.ClothDeliveryDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty
	ORDER BY 
		dbo.EntBookings.BookingNumber

	Open Delivery
		fetch next from Delivery into @BookingDate,@BookingNumber,@TotalQty,@CustomerName,@DelQty,@ClothDeliveryDate
		while @@Fetch_Status=0
			Begin
				IF(@TempBookingNo<>@BookingNumber)
					BEGIN
						
							SET @AlreadyDelivered=''0''
						SET @BalQty=@TotalQty-(@AlreadyDelivered+@DelQty)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,DelQty,ClothDeliveryDate,BalQty)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@DelQty,@ClothDeliveryDate,@BalQty)
						SET @TempBookingNo=@BookingNumber	
						SET @DelQty1=@DelQty					
					END
				ELSE
					BEGIN
						SELECT  
							@AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty)
						FROM
					        dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
						WHERE
							(dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.BarcodeTable.ClothDeliveryDate < @ClothDeliveryDate)
						GROUP BY 
							dbo.EntBookings.BookingNumber
						SET @BalQty=@TotalQty-(@AlreadyDelivered+@DelQty)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,DelQty,ClothDeliveryDate,BalQty)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@DelQty,@ClothDeliveryDate,@BalQty)
						SET @TempBookingNo=@BookingNumber	
						SET @DelQty1=@DelQty				
					END						
fetch next from Delivery into
@BookingDate,@BookingNumber,@TotalQty,@CustomerName,@DelQty,@ClothDeliveryDate
End
close Delivery
deallocate Delivery
select * from #temp 
drop table #temp

END






' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proc_SalesCustReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Sanjeev Arora>
-- Create date: <20/01/2012>
-- Description:	<Balance Sales>
-- =============================================
CREATE PROCEDURE [dbo].[Proc_SalesCustReport]
@CName varchar(max)=null,
@FromDate varchar(max)=null,
@ToDate varchar(max)=null	
AS
BEGIN
declare @BookingDate datetime
declare @BookingDate1 datetime
declare @CustomerName varchar(max)
declare @NetAmount float 
declare @BookingNumber varchar(max)
declare @AlreadyPaid float
declare @PaymentMade float
declare @PaymentDate smalldatetime
declare @BalAmt float
DECLARE @TempBookingNo VARCHAR(MAX)
SET @TempBookingNo=''0''

create table #temp(BookingDate datetime,BookingNumber varchar(max),
CustomerName varchar(max),NetAmount float, AlreadyPaid float, PaymentMade float,
PaymentDate smalldatetime,BalAmt float)

declare Sales cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for
	SELECT
		dbo.EntBookings.BookingDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.NetAmount, dbo.CustomerMaster.CustomerName, SUM(dbo.EntPayment.PaymentMade) AS PaymentMade, dbo.EntPayment.PaymentDate
	FROM
		dbo.EntPayment INNER JOIN  dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
	Where
		((dbo.EntPayment.PaymentDate between @FromDate and @ToDate) and dbo.CustomerMaster.CustomerName=@CName)
	GROUP BY 
		dbo.EntPayment.PaymentDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.NetAmount
	ORDER BY 
		dbo.EntBookings.BookingNumber

	Open Sales
		fetch next from Sales into @BookingDate,@BookingNumber,@NetAmount,@CustomerName,@PaymentMade,@PaymentDate
	while @@Fetch_Status=0
			Begin
				IF(@TempBookingNo<>@BookingNumber)
					BEGIN
						
							SET @AlreadyPaid=''0''
						SET @BalAmt=@NetAmount-(@AlreadyPaid+@PaymentMade)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,NetAmount,AlreadyPaid,PaymentMade,PaymentDate,BalAmt)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@NetAmount,@AlreadyPaid,@PaymentMade,@PaymentDate,@BalAmt)
						SET @TempBookingNo=@BookingNumber	
										
					END
				ELSE
					BEGIN
						SELECT  
							@AlreadyPaid=SUM(dbo.EntPayment.PaymentMade)
						FROM
					        dbo.EntPayment INNER JOIN  dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
						WHERE
							(dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.EntPayment.PaymentDate < @PaymentDate)
						GROUP BY 
							dbo.EntBookings.BookingNumber
						SET @BalAmt=@NetAmount-(@AlreadyPaid+@PaymentMade)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,NetAmount,AlreadyPaid,PaymentMade,PaymentDate,BalAmt)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@NetAmount,@AlreadyPaid,@PaymentMade,@PaymentDate,@BalAmt)
						SET @TempBookingNo=@BookingNumber
					End
fetch next from Sales into @BookingDate,@BookingNumber,@NetAmount,@CustomerName,@PaymentMade,@PaymentDate
End
close Sales
deallocate Sales
select * from #temp
drop table #temp	

END






' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proc_ClothDeliveryByCustomerId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <28-Jan-2012>
-- Description:	<Cloth Delivery Procedure>
-- =============================================
CREATE PROCEDURE [dbo].[Proc_ClothDeliveryByCustomerId] 
@BookNo varchar(max)=null,
@FromDate varchar(max)=null,
@ToDate varchar(max)=null,
@CustId VARCHAR(MAX)='''',
@BranchId VARCHAR(MAX)=''''
AS
BEGIN

declare @BookingDate datetime,@BookingDate1 datetime,@CustomerName varchar(max),@Custname varchar(max),@TotalQty int,@TotQty int,@BookingNumber varchar(max),@InvoiceNo varchar(max),@AlreadyDelivered int,@DelQty int,@DelQty1 int,@ClothDeliveryDate smalldatetime,@ClothDate smalldatetime,@BalQty int,@TempBookingNo VARCHAR(MAX),@Flag int
SET @TempBookingNo=''0''
set @Flag=1
create table #temp(BookingDate datetime,BookingNumber varchar(max),
CustomerName varchar(max),TotalQty int, AlreadyDelivered int, DelQty int,BalQty int,ClothDeliveryDate smalldatetime)

declare Delivery cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for
	SELECT
		dbo.EntBookings.BookingDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.Qty, dbo.CustomerMaster.CustomerName, SUM(dbo.BarcodeTable.DelQty) AS DelQty, dbo.BarcodeTable.ClothDeliveryDate
	FROM
		dbo.BarcodeTable INNER JOIN dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
	WHERE
		  dbo.BarcodeTable.StatusId<>''5'' AND (dbo.BarcodeTable.BranchId=@BranchId) AND (dbo.BarcodeTable.DelQty = 1) AND (dbo.BarcodeTable.ClothDeliveryDate BETWEEN @FromDate AND @ToDate) AND (dbo.CustomerMaster.CustomerCode = @CustId) OR (dbo.BarcodeTable.DelQty = 1) AND (dbo.BarcodeTable.BookingNo = @BookNo)
	GROUP BY 
		dbo.BarcodeTable.ClothDeliveryDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty, dbo.CustomerMaster.CustomerCode
	ORDER BY 
		dbo.EntBookings.BookingNumber
	
	Open Delivery
		fetch next from Delivery into @BookingDate,@BookingNumber,@TotalQty,@CustomerName,@DelQty,@ClothDeliveryDate
		while @@Fetch_Status=0
			Begin
				IF(@TempBookingNo<>@BookingNumber)
					BEGIN
						SET @AlreadyDelivered=''0''
						SELECT  
							@AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty)
						FROM
					        dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
						WHERE
							(dbo.BarcodeTable.BranchId=@BranchId) AND (dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.BarcodeTable.ClothDeliveryDate < @ClothDeliveryDate)
						GROUP BY 
							dbo.EntBookings.BookingNumber
						IF @AlreadyDelivered IS NULL
							SET @AlreadyDelivered=''0''
						SET @BalQty=@TotalQty-(@AlreadyDelivered+@DelQty)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,DelQty,ClothDeliveryDate,BalQty)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@DelQty,@ClothDeliveryDate,@BalQty)
						SET @TempBookingNo=@BookingNumber	
						SET @DelQty1=@DelQty					
					END
				ELSE
					BEGIN
						SELECT  
							@AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty)
						FROM
					        dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
						WHERE
							(dbo.BarcodeTable.BranchId=@BranchId) AND (dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.BarcodeTable.ClothDeliveryDate < @ClothDeliveryDate)
						GROUP BY 
							dbo.EntBookings.BookingNumber
						SET @BalQty=@TotalQty-(@AlreadyDelivered+@DelQty)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,DelQty,ClothDeliveryDate,BalQty)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@DelQty,@ClothDeliveryDate,@BalQty)
						SET @TempBookingNo=@BookingNumber	
						SET @DelQty1=@DelQty				
					END					
fetch next from Delivery into
@BookingDate,@BookingNumber,@TotalQty,@CustomerName,@DelQty,@ClothDeliveryDate
End
close Delivery
deallocate Delivery
select * from #temp
drop table #temp

END








' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proc_SalesandDeliveryByCustomer]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <4-Feb-2012>
-- Description:	<Sales and Delivery>
-- =============================================
CREATE PROCEDURE [dbo].[Proc_SalesandDeliveryByCustomer]
	@FDate DATETIME='''',
	@UDate DATETIME='''',
	@CustId VARCHAR(MAX)='''',
	@BranchId VARCHAR(MAX)=''''
	
AS
BEGIN
DECLARE @TotalAmt VARCHAR(MAX),@BDAmt VARCHAR(MAX),@ST VARCHAR(MAX),@DelAmt VARCHAR(MAX), @FRDATE DATETIME,@UPDATE DATETIME, @BookingDate VARCHAR(MAX),@BookingNumber VARCHAR(MAX),@CustomerName VARCHAR(MAX),@TotalQty INT, @AlreadyDelivered INT, @DelQty INT,@ClothDeliveryDate VARCHAR(MAX),@BalQty VARCHAR(MAX),@NetAmount VARCHAR(MAX), @AlreadyPaid FLOAT, @PaymentMade VARCHAR(MAX),@BalAmt VARCHAR(MAX),@PaymentDate VARCHAR(MAX)
CREATE TABLE #SaleAndDelivery(BookingDate VARCHAR(MAX),BookingNumber VARCHAR(MAX),CustomerName VARCHAR(MAX),TotalQty VARCHAR(MAX), AlreadyDelivered VARCHAR(MAX), DelQty VARCHAR(MAX),BalQty VARCHAR(MAX),ClothDeliveryDate VARCHAR(MAX),NetAmount VARCHAR(MAX), AlreadyPaid VARCHAR(MAX), PaymentMade VARCHAR(MAX),BalAmt VARCHAR(MAX),PaymentDate VARCHAR(MAX),TotalAmt VARCHAR(MAX),BDAmt VARCHAR(MAX),ST VARCHAR(MAX),DelAmt VARCHAR(MAX) )

DECLARE BNumber CURSOR LOCAL FORWARD_Only DYNAMIC Read_Only FOR
	
	--Read unique booking number from delivery and sales 
		
		SELECT dbo.EntBookings.BookingNumber FROM dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.EntBookings.BranchId=@BranchId) AND (dbo.BarcodeTable.DelQty = 1) AND dbo.BarcodeTable.ClothDeliveryDate BETWEEN @FDate AND @UDate AND dbo.EntBookings.BookingByCustomer=@CustId AND (dbo.EntBookings.BookingStatus <> ''5'')
		UNION
		SELECT dbo.EntBookings.BookingNumber FROM dbo.EntPayment INNER JOIN  dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.EntBookings.BranchId=@BranchId) AND dbo.EntPayment.PaymentDate BETWEEN @FDate AND @UDate AND dbo.EntBookings.BookingByCustomer=@CustId AND (dbo.EntBookings.BookingStatus <> ''5'') ORDER BY dbo.EntBookings.BookingNumber 

	--Open Cursor
	
	OPEN BNumber
		FETCH NEXT FROM BNumber INTO @BookingNumber
		WHILE @@Fetch_Status=0
			BEGIN
				SET @FRDATE = @FDate
				SET @UPDATE = @UDate				
				WHILE @FRDate<=@UPDate
					BEGIN
						SET @AlreadyDelivered=''0''
						SET @DelQty=''0''
						SET @BalQty=''0''
						SET @TotalQty=''0''
						SELECT @NetAmount=NetAmount FROM dbo.EntBookings WHERE BookingNumber=@BookingNumber AND BranchId=@BranchId
						SELECT @TotalQty=dbo.EntBookings.Qty FROM dbo.EntBookings WHERE BookingNumber=@BookingNumber AND BranchId=@BranchId
						SELECT @BookingDate=dbo.EntBookings.BookingDate, @CustomerName=dbo.CustomerMaster.CustomerName, @DelQty=SUM(dbo.BarcodeTable.DelQty) ,@ClothDeliveryDate=dbo.BarcodeTable.ClothDeliveryDate FROM dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.EntBookings.BranchId=@BranchId) AND (dbo.BarcodeTable.DelQty = 1) and (dbo.BarcodeTable.ClothDeliveryDate = @FRDate) AND (dbo.BarCodeTable.Bookingno=@BookingNumber) GROUP BY dbo.BarcodeTable.ClothDeliveryDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty	ORDER BY dbo.EntBookings.BookingNumber
						SELECT @AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty) FROM dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode	WHERE (dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BranchId=@BranchId) AND (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.BarcodeTable.ClothDeliveryDate < @FRDate) GROUP BY dbo.EntBookings.BookingNumber
						SELECT @TotalAmt=TotalCost, @BDAmt=DiscountAmt,@ST=(SELECT ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2)   FROM dbo.EntBookingDetails  WHERE      (BookingNumber = dbo.EntBookings.BookingNumber)  GROUP BY BookingNumber) FROM dbo.EntBookings WHERE (BookingStatus <> ''5'') AND (dbo.EntBookings.BranchId=@BranchId) AND  dbo.EntBookings.BookingNumber=@BookingNumber GROUP BY BookingNumber, BookingDate, NetAmount, TotalCost, DiscountAmt ORDER BY BookingNumber
						IF @TotalQty IS NULL
							SET @TotalQty=''0''						
						IF @AlreadyDelivered IS NULL
							SET @AlreadyDelivered=''0''
						IF @DelQty IS NULL 
							SET @DelQty=''0''
						SET @BalQty=@TotalQty-(@AlreadyDelivered+@DelQty)
						IF @BalQty IS NULL
							SET @BalQty=''0''
						--SALE REPORT
						SET @AlreadyPaid=(SELECT SUM(dbo.EntPayment.PaymentMade) FROM  dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber WHERE (dbo.EntPayment.PaymentDate < @FRDate) AND (dbo.EntBookings.BranchId=@BranchId) AND (dbo.EntBookings.BookingNumber = @BookingNumber)	)
						IF @AlreadyPaid IS NULL
							SET @AlreadyPaid=''0''
						SET @PaymentMade=(SELECT SUM(dbo.EntPayment.PaymentMade) FROM  dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber WHERE (dbo.EntPayment.PaymentDate = @FRDate) AND (dbo.EntBookings.BranchId=@BranchId) AND (dbo.EntBookings.BookingNumber = @BookingNumber))
						IF(@PaymentMade IS NULL)
							SET @PaymentMade=''0''
						
						SELECT  @DelAmt=SUM(DiscountOnPayment)  FROM dbo.EntPayment WHERE (BookingNumber = @BookingNumber) AND PaymentDate= @FRDate AND BranchId=@BranchId
						IF @DelAmt IS NULL
							SET @DelAmt=''0''

						SET @BalAmt=@NetAmount-(@AlreadyPaid+@PaymentMade)-@DelAmt
						IF(@ClothDeliveryDate IS NOT NULL) OR @PaymentMade<>''0''
							BEGIN	
								IF(convert(int, @DelQty) <> 0) AND convert(float, @PaymentMade) <> 0		
								BEGIN					
									SELECT @CustomerName=dbo.CustomerMaster.CustomerName, @TotalQty=dbo.EntBookings.Qty, @BookingDate=dbo.EntBookings.BookingDate FROM dbo.EntBookings INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE  dbo.EntBookings.BookingNumber=@BookingNumber
									INSERT INTO #SaleAndDelivery (BookingDate,BookingNumber,CustomerName,TotalQty, AlreadyDelivered, DelQty,BalQty,ClothDeliveryDate,NetAmount, AlreadyPaid, PaymentMade,BalAmt,PaymentDate,TotalAmt,BDAmt,ST,DelAmt) VALUES (CONVERT(VARCHAR,CONVERT(DATETIME,@BookingDate),106),@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered, @DelQty,@BalQty,CONVERT(VARCHAR,CONVERT(DATETIME,@ClothDeliveryDate),106),@NetAmount, @AlreadyPaid, @PaymentMade,@BalAmt,CONVERT(VARCHAR,CONVERT(DATETIME,@FRDate),106),Round(@TotalAmt,2),Round(@BDAmt,2),Round(@ST,2),Round(@DelAmt,2))
								END
								ELSE IF(convert(int, @DelQty) <> 0)	
								BEGIN					
									SELECT @CustomerName=dbo.CustomerMaster.CustomerName, @TotalQty=dbo.EntBookings.Qty, @BookingDate=dbo.EntBookings.BookingDate FROM dbo.EntBookings INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE  dbo.EntBookings.BookingNumber=@BookingNumber
									INSERT INTO #SaleAndDelivery (BookingDate,BookingNumber,CustomerName,TotalQty, AlreadyDelivered, DelQty,BalQty,ClothDeliveryDate,NetAmount, AlreadyPaid, PaymentMade,BalAmt,PaymentDate,TotalAmt,BDAmt,ST,DelAmt) VALUES (CONVERT(VARCHAR,CONVERT(DATETIME,@BookingDate),106),@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered, @DelQty,@BalQty,CONVERT(VARCHAR,CONVERT(DATETIME,@ClothDeliveryDate),106),@NetAmount, @AlreadyPaid, @PaymentMade,@BalAmt,CONVERT(VARCHAR,CONVERT(DATETIME,@FRDate),106),Round(@TotalAmt,2),Round(@BDAmt,2),Round(@ST,2),Round(@DelAmt,2))
								END
								ELSE IF (convert(float, @PaymentMade) <> 0)
								BEGIN					
									SELECT @CustomerName=dbo.CustomerMaster.CustomerName, @TotalQty=dbo.EntBookings.Qty, @BookingDate=dbo.EntBookings.BookingDate FROM dbo.EntBookings INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE  dbo.EntBookings.BookingNumber=@BookingNumber
									INSERT INTO #SaleAndDelivery (BookingDate,BookingNumber,CustomerName,TotalQty, AlreadyDelivered, DelQty,BalQty,ClothDeliveryDate,NetAmount, AlreadyPaid, PaymentMade,BalAmt,PaymentDate,TotalAmt,BDAmt,ST,DelAmt) VALUES (CONVERT(VARCHAR,CONVERT(DATETIME,@BookingDate),106),@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered, @DelQty,@BalQty,CONVERT(VARCHAR,CONVERT(DATETIME,@ClothDeliveryDate),106),@NetAmount, @AlreadyPaid, @PaymentMade,@BalAmt,CONVERT(VARCHAR,CONVERT(DATETIME,@FRDate),106),Round(@TotalAmt,2),Round(@BDAmt,2),Round(@ST,2),Round(@DelAmt,2))
								END
							END
					SET @FRDate=DATEADD(DAY,1,@FRDate)
					END
				
				FETCH NEXT FROM BNumber INTO @BookingNumber
			END
	CLOSE BNumber
	DEALLOCATE BNumber
SELECT * FROM #SaleAndDelivery
DROP TABLE #SaleAndDelivery

END











' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proc_SalesPeriodReport_ByCustomer]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<karam chand sharma>
-- Create date: <20/01/2012>
-- Description:	<Balance Sales>
-- =============================================
CREATE PROCEDURE [dbo].[Proc_SalesPeriodReport_ByCustomer]
@BookNo varchar(max)='''',
@FromDate varchar(max)='''',
@ToDate varchar(max)='''',
@CustId VARCHAR(MAX)=''''	,
@BranchId VARCHAR(MAX)=''''
AS
BEGIN
declare @BookingDate datetime
declare @BookingDate1 datetime
declare @CustomerName varchar(max)
declare @NetAmount float 
declare @BookingNumber varchar(max)
declare @AlreadyPaid float
declare @PaymentMade float
declare @PaymentDate smalldatetime
declare @BalAmt float
DECLARE @TempBookingNo VARCHAR(MAX)
SET @TempBookingNo=''0''

create table #temp(BookingDate datetime,BookingNumber varchar(max),
CustomerName varchar(max),NetAmount float, AlreadyPaid float, PaymentMade float,
PaymentDate smalldatetime,BalAmt float)

declare Sales cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for
	SELECT 
		dbo.EntBookings.BookingDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.NetAmount, dbo.CustomerMaster.CustomerName, SUM(dbo.EntPayment.PaymentMade) AS PaymentMade, dbo.EntPayment.PaymentDate
	FROM 
		dbo.EntPayment INNER JOIN  dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
	WHERE
		 dbo.EntBookings.BookingStatus<>''5'' AND (dbo.EntPayment.PaymentDate BETWEEN @FromDate AND @ToDate) AND (dbo.CustomerMaster.CustomerCode = @CustId) AND  (dbo.EntPayment.BranchId = @BranchId)
	GROUP BY 
		dbo.EntPayment.PaymentDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName,  dbo.EntBookings.NetAmount, dbo.CustomerMaster.CustomerCode
	ORDER BY 
		dbo.EntBookings.BookingNumber
	Open Sales
		fetch next from Sales into @BookingDate,@BookingNumber,@NetAmount,@CustomerName,@PaymentMade,@PaymentDate
	while @@Fetch_Status=0
			Begin
				IF(@TempBookingNo<>@BookingNumber)
					BEGIN
						
							SET @AlreadyPaid=''0''
						SELECT  
							@AlreadyPaid=SUM(dbo.EntPayment.PaymentMade)
						FROM
					        dbo.EntPayment INNER JOIN  dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
						WHERE
							(dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.EntPayment.PaymentDate < @PaymentDate) AND  (dbo.EntPayment.BranchId = @BranchId)
						GROUP BY 
							dbo.EntBookings.BookingNumber
						IF @AlreadyPaid IS NULL
							SET @AlreadyPaid=''0''
						SET @BalAmt=@NetAmount-(@AlreadyPaid+@PaymentMade)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,NetAmount,AlreadyPaid,PaymentMade,PaymentDate,BalAmt)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@NetAmount,@AlreadyPaid,@PaymentMade,@PaymentDate,@BalAmt)
						SET @TempBookingNo=@BookingNumber	
										
					END
				ELSE
					BEGIN
						SELECT  
							@AlreadyPaid=SUM(dbo.EntPayment.PaymentMade)
						FROM
					        dbo.EntPayment INNER JOIN  dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
						WHERE
							(dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.EntPayment.PaymentDate < @PaymentDate) AND  (dbo.EntPayment.BranchId = @BranchId)
						GROUP BY 
							dbo.EntBookings.BookingNumber
						SET @BalAmt=@NetAmount-(@AlreadyPaid+@PaymentMade)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,NetAmount,AlreadyPaid,PaymentMade,PaymentDate,BalAmt)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@NetAmount,@AlreadyPaid,@PaymentMade,@PaymentDate,@BalAmt)
						SET @TempBookingNo=@BookingNumber
					End
fetch next from Sales into @BookingDate,@BookingNumber,@NetAmount,@CustomerName,@PaymentMade,@PaymentDate
End
close Sales
deallocate Sales
select * from #temp
drop table #temp	

END










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustSalesandDelivery]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Sanjeev>

-- =============================================
CREATE PROCEDURE [dbo].[CustSalesandDelivery]
@CName varchar(max)=null,
@FromDate varchar(max)=null,
@ToDate varchar(max)=null
AS
BEGIN
declare @BookingDate datetime
declare @BookingDate1 datetime
declare @CustomerName varchar(max)
declare @Custname varchar(max)
declare @TotalQty int 
declare @TotQty int
declare @BookingNumber varchar(max)
declare @InvoiceNo varchar(max)
declare @AlreadyDelivered int

declare @DelQty int
declare @DelQty1 int
declare @ClothDeliveryDate smalldatetime
declare @ClothDate smalldatetime
declare @BalQty int
declare @NetAmount float 
declare @AlreadyPaid float
declare @PaymentMade float
declare @PaymentDate smalldatetime
declare @BalAmt float

DECLARE @TempBookingNo VARCHAR(MAX)
SET @TempBookingNo=''0''

create table #temp(BookingDate datetime,BookingNumber varchar(max),
CustomerName varchar(max),TotalQty int, AlreadyDelivered int, DelQty int,ClothDeliveryDate smalldatetime,BalQty int,
NetAmount float, AlreadyPaid float, PaymentMade float,
BalAmt float,PaymentDate smalldatetime )

declare Delivery cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for
	SELECT
		dbo.EntBookings.BookingDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.Qty, dbo.CustomerMaster.CustomerName, SUM(dbo.BarcodeTable.DelQty) AS DelQty, dbo.BarcodeTable.ClothDeliveryDate
	FROM
		dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
	WHERE
		(dbo.BarcodeTable.DelQty = 1) and ((dbo.BarcodeTable.ClothDeliveryDate between @FromDate and @ToDate) and dbo.CustomerMaster.CustomerName=@CName)
	GROUP BY 
		dbo.BarcodeTable.ClothDeliveryDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty
	ORDER BY 
		dbo.EntBookings.BookingNumber
	
	Open Delivery
		fetch next from Delivery into @BookingDate,@BookingNumber,@TotalQty,@CustomerName,@DelQty,@ClothDeliveryDate
		while @@Fetch_Status=0
			Begin
				SELECT @NetAmount=NetAmount FROM dbo.EntBookings WHERE BookingNumber=@BookingNumber
				IF(@TempBookingNo<>@BookingNumber)
					BEGIN					
						SET @AlreadyDelivered=''0''
						SET @AlreadyPaid=''0''
						SET @PaymentMade=(SELECT SUM(dbo.EntPayment.PaymentMade) FROM  dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber WHERE (dbo.EntPayment.PaymentDate = @ClothDeliveryDate) AND (dbo.EntBookings.BookingNumber = @BookingNumber))
						if @PaymentMade is Null
						Begin
						set @PaymentMade=0
						End
						SET @BalAmt=@NetAmount-(@AlreadyPaid+@PaymentMade)
						SET @BalQty=@TotalQty-(@AlreadyDelivered+@DelQty)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,DelQty,ClothDeliveryDate,BalQty,NetAmount,AlreadyPaid,PaymentMade,BalAmt,PaymentDate)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@DelQty,@ClothDeliveryDate,@BalQty,@NetAmount,@AlreadyPaid,@PaymentMade,@BalAmt,@ClothDeliveryDate)
						SET @TempBookingNo=@BookingNumber	
						SET @DelQty1=@DelQty					
					END
				ELSE
					BEGIN
						SELECT  
							@AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty)
						FROM
					        dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
						WHERE
							(dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.BarcodeTable.ClothDeliveryDate < @ClothDeliveryDate)
						GROUP BY 
							dbo.EntBookings.BookingNumber
						SET @BalQty=@TotalQty-(@AlreadyDelivered+@DelQty)
						SET @AlreadyPaid=(SELECT SUM(dbo.EntPayment.PaymentMade) FROM  dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber WHERE (dbo.EntPayment.PaymentDate < @ClothDeliveryDate) AND (dbo.EntBookings.BookingNumber = @BookingNumber)	)
						SET @PaymentMade=(SELECT SUM(dbo.EntPayment.PaymentMade) FROM  dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber WHERE (dbo.EntPayment.PaymentDate = @ClothDeliveryDate) AND (dbo.EntBookings.BookingNumber = @BookingNumber))
						IF(@PaymentMade IS NULL)
							SET @PaymentMade=''0''
						SET @BalAmt=@NetAmount-(@AlreadyPaid+@PaymentMade)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,DelQty,ClothDeliveryDate,BalQty,NetAmount,AlreadyPaid,PaymentMade,BalAmt,PaymentDate)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@DelQty,@ClothDeliveryDate,@BalQty,@NetAmount,@AlreadyPaid,@PaymentMade,@BalAmt,@ClothDeliveryDate)
						SET @TempBookingNo=@BookingNumber	
						SET @DelQty1=@DelQty				
					END						
fetch next from Delivery into
@BookingDate,@BookingNumber,@TotalQty,@CustomerName,@DelQty,@ClothDeliveryDate
End
close Delivery
deallocate Delivery


Select * from  #temp
drop table #temp
END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proc_ClothDelivery]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Sanjeev Arora>
-- Create date: <16-Dec-2012>
-- Description:	<Cloth Delivery Procedure>
-- =============================================
CREATE PROCEDURE [dbo].[Proc_ClothDelivery] 
@BookNo varchar(max)='''',
@FromDate varchar(max)='''',
@ToDate varchar(max)='''',
@BranchId Varchar(Max)=''''

AS
BEGIN

declare @BookingDate datetime
declare @BookingDate1 datetime
declare @CustomerName varchar(max)
declare @Custname varchar(max)
declare @TotalQty int 
declare @TotQty int
declare @BookingNumber varchar(max)
declare @InvoiceNo varchar(max)
declare @AlreadyDelivered int

declare @DelQty int
declare @DelQty1 int
declare @ClothDeliveryDate smalldatetime
declare @ClothDate smalldatetime
declare @BalQty int
DECLARE @TempBookingNo VARCHAR(MAX)
declare @Flag int
SET @TempBookingNo=''0''
set @Flag=1

create table #temp(BookingDate datetime,BookingNumber varchar(max),
CustomerName varchar(max),TotalQty int, AlreadyDelivered int, DelQty int,BalQty int,ClothDeliveryDate smalldatetime)

declare Delivery cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for
	SELECT
		dbo.EntBookings.BookingDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.Qty, dbo.CustomerMaster.CustomerName, SUM(dbo.BarcodeTable.DelQty) AS DelQty, dbo.BarcodeTable.ClothDeliveryDate
	FROM
		dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
	WHERE
		 dbo.BarcodeTable.StatusId<>''5'' AND (dbo.BarcodeTable.DelQty = 1) and (dbo.BarcodeTable.ClothDeliveryDate between @FromDate and @ToDate) and dbo.BarCodeTable.BranchId=@BranchId
	GROUP BY 
		dbo.BarcodeTable.ClothDeliveryDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty
	ORDER BY 
		dbo.EntBookings.BookingNumber

	Open Delivery
		fetch next from Delivery into @BookingDate,@BookingNumber,@TotalQty,@CustomerName,@DelQty,@ClothDeliveryDate
		while @@Fetch_Status=0
			Begin
				IF(@TempBookingNo<>@BookingNumber)
					BEGIN
						SET @AlreadyDelivered=''0''
						SELECT  
							@AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty)
						FROM
					        dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
						WHERE
							(dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.BarcodeTable.ClothDeliveryDate < @ClothDeliveryDate) And (dbo.BarCodeTable.BranchId=@BranchId)
						IF @AlreadyDelivered IS NULL
							SET @AlreadyDelivered=''0''
						SET @BalQty=@TotalQty-(@AlreadyDelivered+@DelQty)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,DelQty,ClothDeliveryDate,BalQty)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@DelQty,@ClothDeliveryDate,@BalQty)
						SET @TempBookingNo=@BookingNumber	
						SET @DelQty1=@DelQty					
					END
				ELSE
					BEGIN
						SELECT  
							@AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty)
						FROM
					        dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
						WHERE
							(dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.BarcodeTable.ClothDeliveryDate < @ClothDeliveryDate) And (dbo.BarCodeTable.BranchId=@BranchId)
						GROUP BY 
							dbo.EntBookings.BookingNumber
						SET @BalQty=@TotalQty-(@AlreadyDelivered+@DelQty)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,DelQty,ClothDeliveryDate,BalQty)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@DelQty,@ClothDeliveryDate,@BalQty)
						SET @TempBookingNo=@BookingNumber	
						SET @DelQty1=@DelQty				
					END						
fetch next from Delivery into
@BookingDate,@BookingNumber,@TotalQty,@CustomerName,@DelQty,@ClothDeliveryDate
End
close Delivery
deallocate Delivery
select * from #temp
drop table #temp

END
















' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proc_SalesPeriodReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		<karam chand sharma>
-- Create date: <20/01/2012>
-- Description:	<Balance Sales>
-- =============================================
CREATE PROCEDURE [dbo].[Proc_SalesPeriodReport]
@BookNo varchar(max)=null,
@FromDate varchar(max)=null,
@ToDate varchar(max)=null,
@BranchId Varchar(max)=null

AS
BEGIN
declare @BookingDate datetime
declare @BookingDate1 datetime
declare @CustomerName varchar(max)
declare @NetAmount float 
declare @BookingNumber varchar(max)
declare @AlreadyPaid float
declare @PaymentMade float
declare @PaymentDate smalldatetime
declare @BalAmt float
DECLARE @TempBookingNo VARCHAR(MAX),@TotalCost VARCHAR(MAX),@DiscountAmt VARCHAR(MAX),@ST VARCHAR(MAX),@DeliveryAmt VARCHAR(MAX)
SET @TempBookingNo=''0''

create table #temp(BookingDate datetime,BookingNumber varchar(max),
CustomerName varchar(max),NetAmount float, AlreadyPaid float, PaymentMade float,
PaymentDate smalldatetime,BalAmt float,TotalCost VARCHAR(MAX),DiscountAmt VARCHAR(MAX),ST VARCHAR(MAX),DeliveryAmt VARCHAR(MAX))

declare Sales cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for
	
SELECT 
		dbo.EntBookings.BookingDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.NetAmount, dbo.CustomerMaster.CustomerName,SUM(EntPayment_1.PaymentMade) AS PaymentMade, EntPayment_1.PaymentDate, dbo.EntBookings.TotalCost, dbo.EntBookings.DiscountAmt,(SELECT     ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) AS ST FROM dbo.EntBookingDetails WHERE (BookingNumber = dbo.EntBookings.BookingNumber) GROUP BY BookingNumber) AS ST,(SELECT SUM(DiscountOnPayment) AS DeliveryAmt FROM dbo.EntPayment WHERE      (BookingNumber = dbo.EntBookings.BookingNumber)) AS DeliveryAmt
	FROM 
		dbo.EntPayment AS EntPayment_1 INNER JOIN dbo.EntBookings ON EntPayment_1.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
	Where
	   (dbo.EntBookings.BookingStatus <> ''5'') AND (EntPayment_1.PaymentDate BETWEEN @FromDate AND @ToDate) AND (EntPayment_1.BranchId = @BranchId)
	GROUP BY 
		EntPayment_1.PaymentDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.NetAmount, dbo.EntBookings.TotalCost, dbo.EntBookings.DiscountAmt
	ORDER BY 
		dbo.EntBookings.BookingNumber
	Open Sales
		fetch next from Sales into @BookingDate,@BookingNumber,@NetAmount,@CustomerName,@PaymentMade,@PaymentDate,@TotalCost,@DiscountAmt,@ST,@DeliveryAmt
	while @@Fetch_Status=0
			Begin
				IF(@TempBookingNo<>@BookingNumber)
					BEGIN
						
							SET @AlreadyPaid=''0''
						SELECT  
							@AlreadyPaid=SUM(dbo.EntPayment.PaymentMade)
						FROM
					        dbo.EntPayment INNER JOIN  dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
						WHERE
							(dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.EntPayment.PaymentDate < @PaymentDate) And (dbo.EntPayment.BranchId=@BranchId)
						GROUP BY 
							dbo.EntBookings.BookingNumber
						IF @AlreadyPaid IS NULL
							SET @AlreadyPaid=''0''
						SET @BalAmt=@NetAmount-(@AlreadyPaid+@PaymentMade)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,NetAmount,AlreadyPaid,PaymentMade,PaymentDate,BalAmt,TotalCost,DiscountAmt,ST,DeliveryAmt)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@NetAmount,@AlreadyPaid,@PaymentMade,@PaymentDate,@BalAmt,@TotalCost,@DiscountAmt,@ST,@DeliveryAmt)
						SET @TempBookingNo=@BookingNumber	
										
					END
				ELSE
					BEGIN
						SELECT  
							@AlreadyPaid=SUM(dbo.EntPayment.PaymentMade)
						FROM
					        dbo.EntPayment INNER JOIN  dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
						WHERE
							(dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.EntPayment.PaymentDate < @PaymentDate) And (dbo.EntPayment.BranchId=@BranchId)
						GROUP BY 
							dbo.EntBookings.BookingNumber
						SET @BalAmt=@NetAmount-(@AlreadyPaid+@PaymentMade)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,NetAmount,AlreadyPaid,PaymentMade,PaymentDate,BalAmt,TotalCost,DiscountAmt,ST,DeliveryAmt)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@NetAmount,@AlreadyPaid,@PaymentMade,@PaymentDate,@BalAmt,@TotalCost,@DiscountAmt,@ST,@DeliveryAmt)
						SET @TempBookingNo=@BookingNumber
					End
fetch next from Sales into @BookingDate,@BookingNumber,@NetAmount,@CustomerName,@PaymentMade,@PaymentDate,@TotalCost,@DiscountAmt,@ST,@DeliveryAmt
End
close Sales
deallocate Sales
select * from #temp
drop table #temp	

END










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EditRecord]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <19-Nov-2011>
-- Description:	<Read data for edit record>
-- =============================================
CREATE PROCEDURE [dbo].[sp_EditRecord]
@BOOKINGNUMBER VARCHAR(MAX)='''',
@BranchId VARCHAR(MAX)=''''
AS
BEGIN	
	CREATE TABLE #TmpTable (ItemName VARCHAR(MAX),ItemTotalQuantity VARCHAR(MAX),ItemProcessType VARCHAR(MAX),ItemQuantityAndRate VARCHAR(MAX),ItemExtraProcessType1 VARCHAR(MAX),ItemExtraProcessRate1 VARCHAR(MAX),ItemExtraProcessType2 VARCHAR(MAX),ItemExtraProcessRate2 VARCHAR(MAX),ItemRemark VARCHAR(MAX),ItemColor VARCHAR(MAX),CustName VARCHAR(MAX),CustAddress VARCHAR(MAX),CustRemarks VARCHAR(MAX),CustPriority VARCHAR(MAX),CustPhone VARCHAR(MAX),BookingDate VARCHAR(MAX),BookingDeliveryDate VARCHAR(MAX),BookingDeliveryTime VARCHAR(MAX),BookingRemarks VARCHAR(MAX),Discount VARCHAR(MAX),HomeDelivery VARCHAR(MAX),CheckedByEmployee VARCHAR(MAX),IsUrgent VARCHAR(MAX),TotalCost VARCHAR(MAX),NetAmount VARCHAR(MAX),STPAmt VARCHAR(MAX),STEP1Amt VARCHAR(MAX),STEP2Amt VARCHAR(MAX),ItemSubtotal VARCHAR(MAX),Advance VARCHAR(MAX),CustCode VARCHAR(MAX),ShopIn VARCHAR(MAX),BookingCancelStatus int,DiscountAmt VARCHAR(MAX),DiscountOption VARCHAR(MAX))
	DECLARE @Advance VARCHAR(MAX),@ItemSubtotal VARCHAR(MAX), @ItemName VARCHAR(MAX),@ItemTotalQuantity VARCHAR(MAX),@ItemProcessType VARCHAR(MAX),@ItemQuantityAndRate VARCHAR(MAX),@ItemExtraProcessType1 VARCHAR(MAX),@ItemExtraProcessRate1 VARCHAR(MAX),@ItemExtraProcessType2 VARCHAR(MAX),@ItemExtraProcessRate2 VARCHAR(MAX),@ItemRemark VARCHAR(MAX),@ItemColor VARCHAR(MAX),@CustName VARCHAR(MAX),@CustAddress VARCHAR(MAX),@CustRemarks VARCHAR(MAX),@CustPriority VARCHAR(MAX),@CustPhone VARCHAR(MAX),@BookingDate VARCHAR(MAX),@BookingDeliveryDate VARCHAR(MAX),@BookingDeliveryTime VARCHAR(MAX),@BookingRemarks VARCHAR(MAX),@Discount VARCHAR(MAX),@HomeDelivery VARCHAR(MAX),@CheckedByEmployee VARCHAR(MAX),@IsUrgent VARCHAR(MAX),@TotalCost VARCHAR(MAX),@NetAmount VARCHAR(MAX),@STPAmt VARCHAR(MAX),@STEP1Amt VARCHAR(MAX),@STEP2Amt VARCHAR(MAX),@CustCode VARCHAr(MAX),@ShopIn VARCHAR(MAX),@BookingCancelStatus int	,@DiscountAmt VARCHAR(MAX),@DiscountOption VARCHAR(MAX)
	DECLARE @Fill CURSOR
	SET @Fill = CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
		SELECT ItemName,ItemTotalQuantity,ItemProcessType,ItemQuantityAndRate,ItemExtraProcessType1 ,ItemExtraProcessRate1,ItemExtraProcessType2,ItemExtraProcessRate2,ItemRemark,ItemColor,STPAmt,STEP1Amt,STEP2Amt,ItemSubtotal from entbookingdetails where bookingnumber=@BOOKINGNUMBER AND BranchId=@BranchId
		OPEN @Fill
			FETCH NEXT
				FROM @Fill INTO @ItemName,@ItemTotalQuantity,@ItemProcessType,@ItemQuantityAndRate,@ItemExtraProcessType1,@ItemExtraProcessRate1,@ItemExtraProcessType2,@ItemExtraProcessRate2,@ItemRemark,@ItemColor,@STPAmt,@STEP1Amt,@STEP2Amt,@ItemSubtotal
				WHILE @@FETCH_STATUS = 0
					BEGIN
						select  @ShopIn= max (Statusid) from barcodetable where BookingNo=@BOOKINGNUMBER  AND BranchId=@BranchId
						select  @BookingCancelStatus= BookingStatus from EntBookings where BookingNumber=@BOOKINGNUMBER  AND BranchId=@BranchId
						SELECT @CustName=(dbo.CustomerMaster.CustomerSalutation + '' '' + dbo.CustomerMaster.CustomerName), @CustAddress=dbo.CustomerMaster.CustomerAddress,@CustPhone=dbo.CustomerMaster.CustomerMobile,@CustPriority= dbo.PriorityMaster.Priority,@CustRemarks= dbo.CustomerMaster.Remarks,@BookingDate=CONVERT(VARCHAR,dbo.EntBookings.BookingDate,106),@BookingDeliveryDate=CONVERT(VARCHAR,dbo.EntBookings.BookingDeliveryDate,106),@BookingDeliveryTime= dbo.EntBookings.BookingDeliveryTime,   @BookingRemarks=dbo.EntBookings.BookingRemarks,@Discount= dbo.EntBookings.Discount,@HomeDelivery=dbo.EntBookings.HomeDelivery,@CheckedByEmployee= dbo.EntBookings.CheckedByEmployee,@IsUrgent= dbo.EntBookings.IsUrgent,@TotalCost= dbo.EntBookings.TotalCost,@NetAmount=dbo.EntBookings.NetAmount,@CustCode=dbo.EntBookings.BookingByCustomer,@DiscountAmt= dbo.EntBookings.DiscountAmt,@DiscountOption=dbo.EntBookings.DiscountOption FROM dbo.EntBookings INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode INNER JOIN dbo.PriorityMaster ON dbo.CustomerMaster.CustomerPriority = dbo.PriorityMaster.PriorityID WHERE (dbo.EntBookings.BookingNumber =@BOOKINGNUMBER)	 AND dbo.EntBookings.BranchId=@BranchId		
						SELECT @Advance=Sum(PaymentMade) FROM entpayment WHERE bookingnumber=@BOOKINGNUMBER AND BranchId=@BranchId		
						INSERT INTO #TmpTable (ItemName,ItemTotalQuantity,ItemProcessType,ItemQuantityAndRate,ItemExtraProcessType1,ItemExtraProcessRate1,ItemExtraProcessType2,ItemExtraProcessRate2,ItemRemark,ItemColor,CustName,CustAddress,CustRemarks,CustPriority,CustPhone,BookingDate,BookingDeliveryDate,BookingDeliveryTime,BookingRemarks,Discount,HomeDelivery,CheckedByEmployee,IsUrgent,TotalCost,NetAmount,STPAmt,STEP1Amt,STEP2Amt,ItemSubtotal,Advance,CustCode,ShopIn,BookingCancelStatus,DiscountAmt,DiscountOption)
						VALUES (@ItemName,@ItemTotalQuantity,@ItemProcessType,@ItemQuantityAndRate,@ItemExtraProcessType1,@ItemExtraProcessRate1,@ItemExtraProcessType2,@ItemExtraProcessRate2,@ItemRemark,@ItemColor,@CustName,@CustAddress,@CustRemarks,@CustPriority,@CustPhone,@BookingDate,@BookingDeliveryDate,@BookingDeliveryTime,@BookingRemarks,@Discount,@HomeDelivery,@CheckedByEmployee,@IsUrgent,@TotalCost,@NetAmount,@STPAmt,@STEP1Amt,@STEP2Amt,@ItemSubtotal,@Advance,@CustCode,@ShopIn,@BookingCancelStatus,CASE WHEN @DiscountAmt IS NULL THEN ''0'' ELSE @DiscountAmt END ,CASE WHEN @DiscountOption IS NULL THEN ''0'' ELSE @DiscountOption END)					
						FETCH NEXT
							FROM @Fill INTO @ItemName,@ItemTotalQuantity,@ItemProcessType,@ItemQuantityAndRate,@ItemExtraProcessType1,@ItemExtraProcessRate1,@ItemExtraProcessType2,@ItemExtraProcessRate2,@ItemRemark,@ItemColor,@STPAmt,@STEP1Amt,@STEP2Amt,@ItemSubtotal
					END
		CLOSE @Fill
		DEALLOCATE @Fill
	SELECT  * FROM #TmpTable
	DROP TABLE #TmpTable
END




' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_NewBooking_SaveProc]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<MANOJ KUMAR GUPTA>
-- Create date: <15-Nov-2011>
-- Description:	<SAVE DATA IN THE NEW BOOKING>
-- =============================================
CREATE PROCEDURE [dbo].[sp_NewBooking_SaveProc]	
	@Flag VARCHAR(MAX)='''',
	@BookingAcceptedByUserId VARCHAR(MAX)='''',
	@BookingByCustomer VARCHAR(MAX)='''',
	@IsUrgent bit='''',
	@BookingDate datetime='''',
	@BookingDeliveryDate datetime='''',
	@BookingDeliveryTime VARCHAR(MAX)='''',
	@TotalCost float='''',
	@Discount float='''',
	@NetAmount float='''',
	@BookingStatus int='''',
	@BookingCancelDate datetime='''',
	@BookingCancelReason VARCHAR(MAX)='''',
	@BookingRemarks VARCHAR(MAX)='''',
	@ItemTotalQuantity int='''',
	@Qty int='''',
	@VendorOrderStatus int='''',
	@HomeDelivery bit='''',
	@CheckedByEmployee VARCHAR(MAX)='''',
	@BarCode VARCHAR(MAX)='''',
	@BookingTime NVARCHAR(MAX)='''',
	@Format VARCHAR(10)='''',
	@ReceiptDeliverd bit='''',
	@BOOKINGNUMBER VARCHAR(MAX)='''',
	@ISN INT='''',
	@ItemName VARCHAR(MAX)='''',	
	@ItemProcessType VARCHAR(MAX)='''',
	@ItemQuantityAndRate VARCHAR(MAX)='''',
	@ItemExtraProcessType1 VARCHAR(MAX)='''',
	@ItemExtraProcessRate1 FLOAT='''',
	@ItemExtraProcessType2 VARCHAR(MAX)='''',
	@ItemExtraProcessRate2 FLOAT='''',
	@ItemExtraProcessType3 VARCHAR(MAX)='''',
	@ItemExtraProcessRate3 FLOAT='''',
	@ItemSubTotal FLOAT='''',
	@ItemStatus INT='''',
	@ItemRemark VARCHAR(MAX)='''',
	@DeliveredQty INT='''',
	@ItemColor VARCHAR(MAX)='''',
	@VendorItemStatus INT='''',
	@STPAmt FLOAT='''',
	@STEP1Amt FLOAT='''',
	@STEP2Amt FLOAT='''',
	@CustomerCode VARCHAR(MAX)='''',
	@TransDate SMALLDATETIME='''',
	@AdvanceAmt float='''',
	@ID int='''',	
	@CurrentTime VARCHAR(MAX)='''',
	@DueDate smalldatetime='''',
	@Item  VARCHAR(MAX)='''',	
	@Process VARCHAR(MAX)='''',
	@StatusId int='''',
	@BookingNo VARCHAR(MAX)='''',
	@SNo int='''',
	@RowIndex int='''',	
	@Colour varchar(max)='''',
	@ItemExtraprocessType varchar(max)='''',
	@DrawlStatus bit=false,
	@DrawlAlloted bit=false,
	@DrawlName varchar(100)=null,
	@AllottedDrawl varchar(max)='''',			
	@ItemsReceivedFromVendor int='''',
	@BookingUser varchar(max)='''',
	@BookDate1 datetime='''',
	@BookDate2 datetime='''',			
	@ItemRemarks varchar(max)='''',
	@CustomerName varchar(max)='''',
	@CustomerAddress varchar(max)='''',
	@ColorName varchar(max)='''',
	@ColorCode varchar(max)='''',
	@ProcessCode varchar(max)='''',
	@DiscountAmt float='''',
	@DiscountOption varchar(1)='''',
	@DateTime VARCHAR(MAX)='''',
	@Time VARCHAR(MAX)='''',
	@BranchId VARCHAR(MAX)=''''
	
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
		
		INSERT INTO EntBookings (BookingID,BookingNumber,BookingByCustomer,BookingAcceptedByUserId,IsUrgent,BookingDate,BookingDeliveryDate,BookingDeliveryTime,TotalCost,Discount,NetAmount,BookingStatus,BookingCancelDate,BookingCancelReason,BookingRemarks, ItemTotalQuantity,VendorOrderStatus,HomeDelivery,CheckedByEmployee,BookingTime,Format,DiscountAmt,DiscountOption,BranchId)
			VALUES
								(@BOOKINGID,@BOOKINGNUMBER,@BookingByCustomer,@BookingAcceptedByUserId,@IsUrgent,@BookingDate,@BookingDeliveryDate,@BookingDeliveryTime,@TotalCost,@Discount,@NetAmount,''1'',''1/1/1900'','''',@BookingRemarks,@ItemTotalQuantity,''1'',@HomeDelivery,@CheckedByEmployee,@BookingTime,@Format,@DiscountAmt,@DiscountOption,@BranchId)
		INSERT INTO EntPayment (BookingNumber,PaymentDate,PaymentMade,DiscountOnPayment) VALUES (@BOOKINGNUMBER,GETDATE(),0,0)
		SELECT @BOOKINGNUMBER as BookingNumber 
--	select * from EntBookingDetails where bookingnumber=''25042''
	END	
	ELSE IF(@Flag = 2)
	BEGIN
		
		----- INSERT INTO ENTBOOKINGDETAILS TABLE THIS IS SECOND TABLE
		--SET @BOOKINGNUMBER = (SELECT COALESCE(MAX(Convert(int, BookingNumber)),0) FROM EntBookings)		
		INSERT INTO EntBookingDetails (BookingNumber, ISN, ItemName, ItemTotalQuantity, ItemProcessType, ItemQuantityAndRate, ItemExtraProcessType1, ItemExtraProcessRate1, ItemExtraProcessType2, ItemExtraProcessRate2, ItemSubTotal, ItemStatus, ItemRemark,ItemColor,VendorItemStatus,STPAmt,STEP1Amt,STEP2Amt,BranchId)
			VALUES
										(@BOOKINGNUMBER,@ISN,@ItemName,@ItemTotalQuantity,@ItemProcessType,@ItemQuantityAndRate,@ItemExtraProcessType1,@ItemExtraProcessRate1,@ItemExtraProcessType2,@ItemExtraProcessRate2,@ItemSubTotal,''1'',@ItemRemark,@ItemColor,''1'',@STPAmt,@STEP1Amt,@STEP2Amt,@BranchId)

	END
	ELSE IF(@Flag = 3)
	BEGIN
		------ START ACCOUNT PORTION
		DECLARE @CASH FLOAT,@fltCustPostBalance FLOAT,@SALES FLOAT,@CustLedgerName VARCHAR(MAX),@CustPreBalance FLOAT
		SET @CustLedgerName=(Select CustomerCode As CustLedgerName From CustomerMaster Where CustomerCode=@CustomerCode AND BranchId=@BranchId)
		--SET @BOOKINGNUMBER = (SELECT COALESCE(MAX(Convert(int, BookingNumber)),0) FROM EntBookings)				
		SET @CASH=(Select CurrentBalance From LedgerMaster Where LedgerName=''Cash'' AND BranchId=@BranchId)		
		SET @SALES=(Select CurrentBalance From LedgerMaster Where LedgerName=''Sales'' AND BranchId=@BranchId)		
		SET @CustPreBalance=(Select CurrentBalance From LedgerMaster Where LedgerName=@CustomerCode AND BranchId=@BranchId)
		--SET @TransDate=getdate()
		--PRINT @CASH
		IF(@CASH IS NULL)
		BEGIN
			INSERT INTO LedgerMaster (LedgerName, OpenningBalance, CurrentBalance,BranchId) Values(''CASH'',''0'',''0'',@BranchId)
			SET @CASH=(Select CurrentBalance From LedgerMaster Where LedgerName=''Cash'' AND BranchId=@BranchId)					
		END				
		IF(@SALES IS NULL)
		BEGIN
			INSERT INTO LedgerMaster (LedgerName, OpenningBalance, CurrentBalance,BranchId) Values(''Sales'',''0'',''0'',@BranchId)
			SET @SALES=(Select CurrentBalance From LedgerMaster Where LedgerName=''Sales'' AND BranchId=@BranchId)
		END			
		------ SAVE INTO ENTLEDGERENTRIES
		INSERT INTO EntLedgerEntries 
			(TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId)VALUES
			(@DateTime,@CustLedgerName,''By Sales'',@CustPreBalance,@TotalCost,''0'',@CustPreBalance+@TotalCost,''On Credit New Booking Number ''+@BOOKINGNUMBER,@BranchId)
		INSERT INTO EntLedgerEntries 
			(TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId)VALUES
			(@DateTime,''Sales'',''To''+'' ''+@CustLedgerName,@SALES,''0'',@TotalCost,@CustPreBalance-@TotalCost,''On Credit New Booking Number ''+@BOOKINGNUMBER,@BranchId)
		SET @fltCustPostBalance=@CustPreBalance+@TotalCost
		IF(@AdvanceAmt>0)
			BEGIN
				SET @CustPreBalance=(Select CurrentBalance From LedgerMaster Where LedgerName=@CustomerCode AND BranchId=@BranchId)
				INSERT INTO EntPayment (BookingNumber, PaymentDate, PaymentMade, DiscountOnPayment,PaymentTime,BranchId) VALUES (@BOOKINGNUMBER,@DateTime,@AdvanceAmt,''0'',@Time,@branchId)
				INSERT INTO EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId) VALUES (@DateTime,@CustomerCode,''To Cash'',(@CustPreBalance+@TotalCost),''0'',@AdvanceAmt,(@CustPreBalance+@TotalCost)-@AdvanceAmt,''Received advance against Booking number ''+@BOOKINGNUMBER,@BranchId)
				INSERT INTO EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId) VALUES (@DateTime,''Cash'',''By ''+@CustLedgerName,@CASH,@AdvanceAmt,''0'',@CASH+@AdvanceAmt,''Received advance against Booking number ''+@BOOKINGNUMBER,@BranchId)
				SET @CustPreBalance=(Select CurrentBalance From LedgerMaster Where LedgerName=@CustomerCode AND BranchId=@BranchId)
				UPDATE LedgerMaster SET CurrentBalance=@CASH+@AdvanceAmt WHERE LedgerName=''Cash'' 	
				SET @fltCustPostBalance=((@CustPreBalance+@TotalCost)-@AdvanceAmt)
			END
		UPDATE LedgerMaster SET CurrentBalance=@fltCustPostBalance WHERE LedgerName=@CustomerCode AND BranchId=@BranchId
		UPDATE LedgerMaster SET CurrentBalance=@SALES-@TotalCost WHERE LedgerName=''Sales'' AND BranchId=@BranchId
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
		SELECT ItemName FROM ItemMaster WHERE ItemName=@ItemName AND BranchId=@BranchId
	END
	ELSE IF(@Flag=7)
	BEGIN
		SELECT ProcessCode  FROM ProcessMaster WHERE ProcessCode=@ProcessCode
	END
END










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CustomerMaster]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Manoj Kumar Gupta>
-- Create date: <27-Feb-2012>
-- Description:	<Customer Master>
-- =============================================
CREATE PROCEDURE [dbo].[sp_CustomerMaster]	
	@Flag VARCHAR(MAX)='''',
	@BranchId VARCHAR(10)='''',
	@CustomerSalutation VARCHAR(MAX)='''',
	@CustomerName VARCHAR(MAX)='''',
	@CustomerAddress VARCHAR(MAX)='''',
	@CustomerPhone VARCHAR(MAX)='''',
	@CustomerMobile VARCHAR(MAX)='''',
	@CustomerEmailId VARCHAR(MAX)='''',
	@CustomerPriority INT='''',
	@CustomerRefferredBy VARCHAR(MAX)='''',
	@CustomerRegisterDate SMALLDATETIME='''',
	@CustomerIsActive BIT='''',
	@CustomerCancelDate SMALLDATETIME='''',
	@DefaultDiscountRate INT='''',
	@Remarks VARCHAR(MAX)='''',
	@BirthDate SMALLDATETIME='''',
	@AnniversaryDate SMALLDATETIME='''',
	@AreaLocation VARCHAR(MAX)='''',	
	@CustomerCode VARCHAR(MAX)=''''
	
AS
BEGIN	
	IF(@Flag = 1)
		BEGIN
		--- SAVE DATA IN THE CUSTOMER MASTER
		DECLARE @CustId varchar(max)
		SET @CustId=(SELECT COALESCE(MAX(Convert(int, Id)),0) From CustomerMaster WHERE BranchId=@BranchId)
		SET @CustId=@CustId+1
		SET @CustomerCode=''Cust''+@CustId
		INSERT INTO CustomerMaster VALUES (@CustId,@CustomerCode,@CustomerSalutation,@CustomerName,@CustomerAddress,@CustomerPhone,@CustomerMobile,@CustomerEmailId,@CustomerPriority,@CustomerRefferredBy,GETDATE(),''1'',GETDATE(),@DefaultDiscountRate,@Remarks,@BirthDate,@AnniversaryDate,@AreaLocation,@BranchId,'''')
		INSERT INTO LedgerMaster VALUES (@CustomerCode,''0'',''0'',@BranchId,'''')				
		END	
	IF(@Flag = 2)
		BEGIN
		--- CHECK DUPLICATE RECORD IN THE TABLE 
		SELECT ID FROM CustomerMaster WHERE CustomerName=@CustomerName AND CustomerAddress=@CustomerAddress AND BranchId=@BranchId
		END
	IF(@Flag = 3)
		BEGIN
		--- UPDATE DATA IN THE CUSTOMER MASTER
		UPDATE CustomerMaster SET CustomerSalutation =@CustomerSalutation,CustomerName=@CustomerName,CustomerAddress=@CustomerAddress,CustomerPhone=@CustomerPhone, CustomerMobile=@CustomerMobile,CustomerEmailId=@CustomerEmailId, CustomerPriority=@CustomerPriority,CustomerRefferredBy=@CustomerRefferredBy, DefaultDiscountRate =@DefaultDiscountRate, Remarks =@Remarks, BirthDate =@BirthDate, AnniversaryDate =@AnniversaryDate, AreaLocation =@AreaLocation WHERE CustomerCode=@CustomerCode AND BranchId=@BranchId
		END
	IF(@Flag = 4)
		BEGIN
		--- FILL RECORD IN THE TEXTBOXES
		Select CustomerCode, CustomerSalutation, CustomerName, CustomerAddress, CustomerPhone, CustomerMobile, CustomerEmailId, CustomerPriority, CustomerRefferredBy, Convert(varchar, CustomerRegisterDate, 103) As CustomerRegisterDate, CustomerIsActive, DefaultDiscountRate,Remarks, Convert(varchar, BirthDate,106) AS BirthDate,Convert(varchar, AnniversaryDate,106) AS AnniversaryDate,AreaLocation FROM CustomerMaster WHERE CustomerCode=@CustomerCode AND BranchId=@BranchId
		END
	IF(@Flag = 5)
		BEGIN
		--- SEARCH CUSTOMER MASTER DEFAULT CRITERIA
		IF(@CustomerName='''')
		SELECT [ID], [CustomerCode], COALESCE(CustomerSalutation,'''') + '' '' + [CustomerName] As CustomerName, [CustomerAddress], [CustomerPhone], [CustomerMobile], [CustomerEmailId], [Priority], [CustomerRefferredBy], [CustomerRegisterDate], [CustomerIsActive], [CustomerCancelDate] FROM [CustomerMaster] LEFT JOIN PriorityMaster ON CustomerMaster.CustomerPriority = PriorityMaster.PriorityId Where CustomerMaster.BranchId=@BranchId order by ID desc
		ELSE
		SELECT [ID], [CustomerCode], COALESCE(CustomerSalutation,'''') + '' '' + [CustomerName] As CustomerName, [CustomerAddress], [CustomerPhone], [CustomerMobile], [CustomerEmailId], [Priority], [CustomerRefferredBy], [CustomerRegisterDate], [CustomerIsActive], [CustomerCancelDate] FROM [CustomerMaster] LEFT JOIN PriorityMaster ON CustomerMaster.CustomerPriority = PriorityMaster.PriorityId Where CustomerMaster.BranchId=''1'' AND (CustomerName Like ''%''+@CustomerName +''%'') OR (CustomerAddress Like ''%''+@CustomerName +''%'') OR (CustomerPhone Like ''%''+@CustomerName +''%'') OR (CustomerMobile Like ''%''+@CustomerName +''%'') order by ID desc
		END
	IF(@Flag = 6)
		BEGIN
		--- CHECK CUSTOMER RECORD IN THE BOOKINGS TABLE
		SELECT COALESCE(COUNT(BookingByCustomer),0) FROM EntBookings WHERE BookingByCustomer=@CustomerCode AND BranchId=@BranchId GROUP BY BookingByCustomer
		END
	IF(@Flag = 7)
		BEGIN
		--- CHECK CUSTOMER RECORD IN THE LEDGERENTRIES TABLE
		SELECT COALESCE(COUNT(LedgerName),0) FROM EntLedgerEntries WHERE LedgerName=@CustomerCode AND BranchId=@BranchId GROUP BY LedgerName
		END
	IF(@Flag = 8)
		BEGIN
		--- DELETE CUSTOMER IN THE TABLES
		DELETE FROM LedgerMaster WHERE LedgerName=@CustomerCode AND BranchId=@BranchId
		DELETE FROM CustomerMaster WHERE CustomerCode=@CustomerCode AND BranchId=@BranchId
		END
	IF(@Flag = 9)	
		BEGIN
		SELECT CustomerCode As Code, CustomerSalutation As Title, CustomerName As Name, CustomerAddress As Address, CustomerPhone As Phone, CustomerMobile As Mobile FROM CustomerMaster WHERE BranchId=@BranchId  ORDER BY CustomerName
		END
	IF(@Flag = 10)	
		BEGIN
		SELECT PriorityID, Priority FROM [PriorityMaster] WHERE BranchId=@BranchId  order by PriorityID asc
		END
END









' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_dry_NewChallan]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



CREATE PROCEDURE [dbo].[sp_dry_NewChallan]
	@Flag int=0,
	@BookingNo VARCHAR(MAX)='''',
	@RowIndex int=null,
	@BranchId VARCHAR(MAX)=''''
	
AS 
BEGIN 
	IF(@Flag=1)
	 BEGIN
		SELECT CONVERT(int, dbo.EntBookings.BookingNumber) AS BookingNumber, dbo.CustomerMaster.CustomerSalutation + '' '' + dbo.CustomerMaster.CustomerName AS Customer, dbo.BarcodeTable.RowIndex AS ISN, dbo.BarcodeTable.Item AS SubItemName, dbo.BarcodeTable.Process AS ItemProcessType, CASE WHEN BarcodeTable.ItemExtraprocessType = ''0'' THEN '''' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType1, CASE WHEN BarcodeTable.ItemExtraprocessType2 = ''0'' THEN '''' ELSE BarcodeTable.ItemExtraprocessType2 END AS ItemExtraprocessType2, dbo.EntBookings.IsUrgent, dbo.BarcodeTable.SNo AS ItemTotalQuantity FROM dbo.EntBookings INNER JOIN  dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo INNER JOIN  dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.BarcodeTable.StatusId = ''1'') AND (dbo.EntBookings.BookingStatus <> ''5'') AND EntBookings.BranchId=@BranchId ORDER BY BookingNumber, ISN
	 END
	IF(@Flag=2)
	 BEGIN
		SELECT CONVERT(int, dbo.EntBookings.BookingNumber) AS BookingNumber, dbo.CustomerMaster.CustomerSalutation + '' '' + dbo.CustomerMaster.CustomerName AS Customer, dbo.BarcodeTable.RowIndex AS ISN, dbo.BarcodeTable.Item AS SubItemName, dbo.BarcodeTable.Process AS ItemProcessType, CASE WHEN BarcodeTable.ItemExtraprocessType = ''0'' THEN '''' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType1, CASE WHEN BarcodeTable.ItemExtraprocessType2 = ''0'' THEN '''' ELSE BarcodeTable.ItemExtraprocessType2 END AS ItemExtraprocessType2, dbo.EntBookings.IsUrgent, dbo.BarcodeTable.SNo AS ItemTotalQuantity FROM dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo INNER JOIN  dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.BarcodeTable.StatusId = ''1'') AND (dbo.EntBookings.BookingStatus <> ''5'') AND dbo.BarcodeTable.BookingNo=@BookingNo AND EntBookings.BranchId=@BranchId  ORDER BY CONVERT(int, dbo.EntBookings.BookingNumber), ISN
	 END
	IF(@Flag=3)
	 BEGIN
		SELECT CONVERT(int, dbo.EntBookings.BookingNumber) AS BookingNumber, dbo.CustomerMaster.CustomerSalutation + '' '' + dbo.CustomerMaster.CustomerName AS Customer, dbo.BarcodeTable.RowIndex AS ISN, dbo.BarcodeTable.Item AS SubItemName, dbo.BarcodeTable.Process AS ItemProcessType, CASE WHEN BarcodeTable.ItemExtraprocessType = ''0'' THEN '''' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraprocessType1, CASE WHEN BarcodeTable.ItemExtraprocessType2 = ''0'' THEN '''' ELSE BarcodeTable.ItemExtraprocessType2 END AS ItemExtraprocessType2, dbo.EntBookings.IsUrgent, dbo.BarcodeTable.SNo AS ItemTotalQuantity FROM dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.BarcodeTable.StatusId = ''1'') AND (dbo.EntBookings.BookingStatus <> ''5'') AND dbo.BarcodeTable.BookingNo=@BookingNo AND dbo.BarcodeTable.RowIndex=@RowIndex AND EntBookings.BranchId=@BranchId  ORDER BY CONVERT(int, dbo.EntBookings.BookingNumber), ISN
	 END
END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_DefaultAnniveraryCustomer]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

/*
-- =============================================
-- Author:		<Manoj Gupta>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport ''1 SEP 2010'',''2 sep 2010''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_DefaultAnniveraryCustomer]
	(
		@BookDate1 datetime,		
		@MainDate datetime,
		@BranchId VARCHAR(MAX)=''''
	)
AS
BEGIN
	--DECLARE @Date datetime,@MainDate datetime	
	set @MainDate=''''
	set @MainDate= (SELECT convert(varchar, DATEADD(DAY, DATEDIFF(DAY, 0, @BookDate1), 0) + 5,106))
	SELECT  CustomerName, CustomerAddress, CustomerPhone, CustomerMobile, convert(varchar,AnniversaryDate,106) as AnniversaryDate FROM dbo.CustomerMaster	
	where BranchId=@BranchId AND  AnniversaryDate BETWEEN @BookDate1 AND @MainDate
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_DefaultBirthDayCustomer]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

/*
-- =============================================
-- Author:		<Manoj Gupta>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport ''1 SEP 2010'',''2 sep 2010''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_DefaultBirthDayCustomer]
	(
		@BookDate1 datetime,		
		@MainDate datetime,
		@BranchId VARCHAR(MAX)=''''
	)
AS
BEGIN
	--DECLARE @Date datetime,@MainDate datetime	
	set @MainDate=''''
	set @MainDate= (SELECT convert(varchar, DATEADD(DAY, DATEDIFF(DAY, 0, @BookDate1), 0) + 5,106))
	SELECT  CustomerName, CustomerAddress, CustomerPhone, CustomerMobile, convert(varchar,BirthDate,106) as BirthDate	FROM dbo.CustomerMaster	
	where BirthDate BETWEEN @BookDate1 AND @MainDate AND BranchId=@BranchId
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SaveDataFromMultiplePaymentAndDeliveryReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'




-- =============================================
-- Author:		<Manoj Kumar Gupta>
-- Create date: <02-Feb-2012>
-- Description:	<Bulk Delivery And Payment>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SaveDataFromMultiplePaymentAndDeliveryReport]	
	@Flag VARCHAR(MAX)='''',	
	@BookingNumber VARCHAR(MAX)='''',
	@DateTime datetime='''',
	@TotalCost float='''',
	@BranchId varchar(max)=''''	

AS
BEGIN	
	IF(@Flag = 1)
		BEGIN
			UPDATE BarcodeTable SET DeliveredQty=''True'',DeliverItemStaus=''Delivered'',StatusId=''4'',DelQty=''1'',ClothDeliveryDate=@DateTime WHERE BookingNo = @BookingNumber AND DelQty=''0'' AND StatusId<>''1'' AND StatusId<>''2'' AND BranchId=@BranchId
			UPDATE EntBookingDetails SET DeliveredQty=''1'' WHERE BookingNumber=@BookingNumber AND BranchId=@BranchId		
		END
	IF(@Flag = 2)
		BEGIN
			------ START ACCOUNT PORTION
		DECLARE @CASH FLOAT,@fltCustPostBalance FLOAT,@SALES FLOAT,@CustLedgerName VARCHAR(MAX),@CustPreBalance FLOAT,@CustomerCode VARCHAR(MAX)
		SET @CustomerCode=(SELECT BookingByCustomer FROM EntBookings WHERE BookingNumber=@BOOKINGNUMBER AND BranchId=@BranchId)
		SET @CustLedgerName=(Select CustomerCode As CustLedgerName From CustomerMaster Where CustomerCode=@CustomerCode AND BranchId=@BranchId)
		--SET @BOOKINGNUMBER = (SELECT COALESCE(MAX(Convert(int, BookingNumber)),0) FROM EntBookings)				
		SET @CASH=(Select CurrentBalance From LedgerMaster Where LedgerName=''Cash'' AND BranchId=@BranchId)		
		SET @SALES=(Select CurrentBalance From LedgerMaster Where LedgerName=''Sales'' AND BranchId=@BranchId)		
		SET @CustPreBalance=(Select CurrentBalance From LedgerMaster Where LedgerName=@CustomerCode AND BranchId=@BranchId)
		
		-------- SAVE INTO ENTPAYMENT
		INSERT INTO EntPayment (BookingNumber, PaymentDate, PaymentMade, DiscountOnPayment,PaymentTime,BranchId) VALUES (@BOOKINGNUMBER,@DateTime,@TotalCost,''0'','''',@BranchId)
				
		------ SAVE INTO ENTLEDGERENTRIES
		INSERT INTO EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId) VALUES (@DateTime,@CustomerCode,''To Cash'',(@CustPreBalance+@TotalCost),''0'',@TotalCost,(@CustPreBalance+@TotalCost),''Received advance against Booking number ''+@BOOKINGNUMBER,@BranchId)
		INSERT INTO EntLedgerEntries (TransDate, LedgerName, Particulars, OpeningBalance, Debit, Credit, ClosingBalance, Narration,BranchId) VALUES (@DateTime,''Cash'',''By ''+@CustLedgerName,@CASH,@TotalCost,''0'',@CASH+@TotalCost,''Received advance against Booking number ''+@BOOKINGNUMBER,@BranchId)
		SET @fltCustPostBalance=@CustPreBalance+@TotalCost
		UPDATE LedgerMaster SET CurrentBalance=@fltCustPostBalance WHERE LedgerName=@CustomerCode AND BranchId=@BranchId
		UPDATE LedgerMaster SET CurrentBalance=@SALES-@TotalCost WHERE LedgerName=''Sales'' AND BranchId=@BranchId
		END
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Dry_PendingClothAndPaymentCustomerWise]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


-- =============================================
-- Author:		<Manoj Kumar gupta>
-- Create date: <25-Aug-2011>
-- Description:	<Multiple Payment Screen>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Dry_PendingClothAndPaymentCustomerWise]	

@Date datetime,
@CustCode varchar(max)

AS
BEGIN
	

DECLARE @Amt FLOAT, @BookingDate varchar(max),@CustomerName varchar(max),@TotalQty int ,@BookingNumber int,@AlreadyDelivered int,@BalQty int,@TotalPay FLOAT,@AllReadyPaid FLOAT,@Balance FLOAT,@Status VARCHAR(50),@ReadyClothes INT
CREATE TABLE #temp(BookingDate varchar(max),BookingNumber int,CustomerName varchar(max),TotalQty int, AlreadyDelivered int, BalQty int,TotalPay FLOAT,AllReadyPaid FLOAT,Balance FLOAT,Status VARCHAR(50))
--SET @Date=''11 Feb 2012''
DECLARE Delivery cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for


SELECT 
	SUM(dbo.EntPayment.PaymentMade) - dbo.EntBookings.NetAmount AS AMT, dbo.EntBookings.BookingNumber
FROM
	dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
WHERE
	(dbo.EntBookings.BookingDate <= @Date) AND  dbo.EntBookings.BookingByCustomer=@CustCode
GROUP BY 
	dbo.EntBookings.BookingNumber, dbo.EntBookings.NetAmount
ORDER BY 
	dbo.EntBookings.BookingNumber
OPEN Delivery
	FETCH NEXT FROM Delivery INTO @Amt, @BookingNumber
	WHILE @@Fetch_Status=0
		BEGIN			
			IF @Amt<>0
				BEGIN
					SELECT 
						@BookingDate=CONVERT(varchar, dbo.EntBookings.BookingDeliveryDate, 106), @TotalQty=dbo.EntBookings.Qty,@CustomerName=dbo.CustomerMaster.CustomerName,@TotalPay=dbo.EntBookings.NetAmount
					FROM
				        dbo.BarcodeTable INNER JOIN dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN  dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
					WHERE
						(dbo.EntBookings.BookingNumber = @BookingNumber)
					GROUP BY dbo.BarcodeTable.ClothDeliveryDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDeliveryDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty, 
						dbo.EntBookings.NetAmount
					ORDER BY 
						dbo.EntBookings.BookingNumber
					SET @AlreadyDelivered=''0''
					SET @ReadyClothes =''0''
					SELECT  @ReadyClothes=SUM(SNo) FROM BarcodeTable WHERE BookingNo=@BookingNumber AND StatusId<>''1'' AND StatusId<>''2''
					SELECT  @AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty) FROM  dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode	WHERE (dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) GROUP BY dbo.EntBookings.BookingNumber
					IF @AlreadyDelivered IS NULL
						SET @AlreadyDelivered=''0''					
					SET @BalQty=@TotalQty-@AlreadyDelivered			 
					SELECT @AllReadyPaid=SUM(dbo.EntPayment.PaymentMade) FROM dbo.EntBookings INNER JOIN dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber WHERE (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.EntPayment.PaymentDate <= @Date) 
					if @AllReadyPaid is null 
						set @AllReadyPaid	=''0''
					SET @Balance=@TotalPay-@AllReadyPaid
					IF @ReadyClothes IS NULL
					BEGIN
					SET @ReadyClothes=''0''
					END
					IF(@TotalQty=@ReadyClothes)
					BEGIN
					SET @STATUS=''READY''					
					END
					ELSE
					BEGIN
					SET @STATUS=''PENDING''					
					END
					INSERT INTO #temp
						(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,BalQty,TotalPay,AllReadyPaid,Balance,Status)
					VALUES
						(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@BalQty,@TotalPay,@AllReadyPaid,@Balance,@Status)								
					
				END
					FETCH NEXT FROM Delivery INTO @Amt, @BookingNumber
			END	
CLOSE Delivery
DEALLOCATE Delivery
----------------------Second Out Put-------------------
CREATE TABLE #temp1(BookingDate varchar(max),BookingNumber int,CustomerName varchar(max),TotalQty int, AlreadyDelivered int, BalQty int,TotalPay FLOAT,AllReadyPaid FLOAT,Balance FLOAT,Status VARCHAR(50))
DECLARE Delivery cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for
SELECT  
	convert(varchar,dbo.EntBookings.BookingDeliveryDate,106) as BookingDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.Qty, dbo.CustomerMaster.CustomerName, dbo.EntBookings.NetAmount
FROM
	dbo.BarcodeTable INNER JOIN dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
WHERE
	(dbo.BarcodeTable.DelQty = 0) AND (dbo.BarcodeTable.BookingDate <=@Date) AND  dbo.EntBookings.BookingByCustomer=@CustCode
GROUP BY 
	dbo.BarcodeTable.ClothDeliveryDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDeliveryDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty,dbo.EntBookings.NetAmount
ORDER BY 
	dbo.EntBookings.BookingNumber

OPEN Delivery
	FETCH NEXT FROM Delivery INTO @BookingDate,@BookingNumber,@TotalQty,@CustomerName,@TotalPay 
	WHILE @@Fetch_Status=0
		BEGIN
			SET @AlreadyDelivered=''0''
			SET @ReadyClothes =''0''
			SELECT  @ReadyClothes=SUM(SNo) FROM BarcodeTable WHERE BookingNo=@BookingNumber AND StatusId<>''1'' AND StatusId<>''2''
			SELECT  @AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty) FROM  dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode	WHERE (dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) GROUP BY dbo.EntBookings.BookingNumber
			IF @AlreadyDelivered IS NULL
				SET @AlreadyDelivered=''0''
			SET @BalQty=@TotalQty-@AlreadyDelivered			 
			SELECT @AllReadyPaid=SUM(dbo.EntPayment.PaymentMade) FROM dbo.EntBookings INNER JOIN dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber WHERE (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.EntPayment.PaymentDate <= @Date) 
			if @AllReadyPaid is null 
				set @AllReadyPaid	=''0''
			SET @Balance=@TotalPay-@AllReadyPaid
			IF @ReadyClothes IS NULL
			BEGIN
			SET @ReadyClothes=''0''
			END
			IF(@TotalQty=@ReadyClothes)
			BEGIN
			SET @STATUS=''READY''					
			END
			ELSE
			BEGIN
			SET @STATUS=''PENDING''					
			END
			INSERT INTO #temp1
				(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,BalQty,TotalPay,AllReadyPaid,Balance,STATUS)
			VALUES
				(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@BalQty,@TotalPay,@AllReadyPaid,@Balance,@STATUS)								
			FETCH NEXT FROM Delivery INTO @BookingDate,@BookingNumber,@TotalQty,@CustomerName,@TotalPay 
		END	
CLOSE Delivery
DEALLOCATE Delivery
SELECT * FROM #temp
UNION
SELECT * FROM #temp1
DROP TABLE #temp
DROP TABLE #temp1
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Dry_MultipleBothDatetoDate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		<Manoj Kumar gupta>
-- Create date: <25-Aug-2011>
-- Description:	<Multiple Payment Screen>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Dry_MultipleBothDatetoDate]	

@Date datetime,
@BranchId varchar(max)

AS
BEGIN
	

DECLARE @Amt FLOAT, @BookingDate varchar(max),@CustomerName varchar(max),@TotalQty int ,@BookingNumber int,@AlreadyDelivered int,@BalQty int,@TotalPay FLOAT,@AllReadyPaid FLOAT,@Balance FLOAT,@ReadyClothes INT
CREATE TABLE #temp(BookingDate varchar(max),BookingNumber int,CustomerName varchar(max),TotalQty int, AlreadyDelivered int, BalQty int,TotalPay FLOAT,AllReadyPaid FLOAT,Balance FLOAT,ReadyClothes INT)
--SET @Date=''11 Feb 2012''
DECLARE Delivery cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for


SELECT 
	SUM(dbo.EntPayment.PaymentMade) - dbo.EntBookings.NetAmount AS AMT, dbo.EntBookings.BookingNumber
FROM
	dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
WHERE
	(dbo.EntBookings.BookingDeliveryDate <= @Date) AND (dbo.EntBookings.BookingStatus <> ''5'') AND (dbo.EntBookings.BranchId=@BranchId)
GROUP BY 
	dbo.EntBookings.BookingNumber, dbo.EntBookings.NetAmount
ORDER BY 
	dbo.EntBookings.BookingNumber
OPEN Delivery
	FETCH NEXT FROM Delivery INTO @Amt, @BookingNumber
	WHILE @@Fetch_Status=0
		BEGIN			
			IF @Amt<>0
				BEGIN
					SELECT 
						@BookingDate=CONVERT(varchar, dbo.EntBookings.BookingDate, 106), @TotalQty=dbo.EntBookings.Qty,@CustomerName=dbo.CustomerMaster.CustomerName,@TotalPay=dbo.EntBookings.NetAmount
					FROM
				        dbo.BarcodeTable INNER JOIN dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN  dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
					WHERE
						(dbo.EntBookings.BookingNumber = @BookingNumber)
					GROUP BY dbo.BarcodeTable.ClothDeliveryDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty, 
						dbo.EntBookings.NetAmount
					ORDER BY 
						dbo.EntBookings.BookingNumber
					SET @AlreadyDelivered=''0''
					SET @ReadyClothes=''0''
					SELECT  @AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty) FROM  dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode	WHERE (dbo.BarcodeTable.DelQty = 1) AND (dbo.BarcodeTable.StatusId<>''1'') AND (dbo.BarcodeTable.StatusId<>''2'') AND (dbo.EntBookings.BookingNumber = @BookingNumber) GROUP BY dbo.EntBookings.BookingNumber
					SELECT  @ReadyClothes=SUM(SNo) FROM BarcodeTable WHERE BookingNo=@BookingNumber AND StatusId<>''1'' AND StatusId<>''2''
					IF @AlreadyDelivered IS NULL
						SET @AlreadyDelivered=''0''
					IF @ReadyClothes IS NULL
						SET @ReadyClothes=''0''
					SET @BalQty=@ReadyClothes-@AlreadyDelivered			 
					SELECT @AllReadyPaid=SUM(dbo.EntPayment.PaymentMade) FROM dbo.EntBookings INNER JOIN dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber WHERE (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.EntPayment.PaymentDate <= @Date) 
					if @AllReadyPaid is null 
						set @AllReadyPaid	=''0''
					SET @Balance=@TotalPay-@AllReadyPaid
					INSERT INTO #temp
						(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,BalQty,TotalPay,AllReadyPaid,Balance,ReadyClothes)
					VALUES
						(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@BalQty,@TotalPay,@AllReadyPaid,@Balance,@ReadyClothes)								
					
				END
					FETCH NEXT FROM Delivery INTO @Amt, @BookingNumber
			END	
CLOSE Delivery
DEALLOCATE Delivery
----------------------Second Out Put-------------------
CREATE TABLE #temp1(BookingDate varchar(max),BookingNumber int,CustomerName varchar(max),TotalQty int, AlreadyDelivered int, BalQty int,TotalPay FLOAT,AllReadyPaid FLOAT,Balance FLOAT,ReadyClothes INT)
DECLARE Delivery cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for
SELECT  
	convert(varchar,dbo.EntBookings.BookingDate,106) as BookingDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.Qty, dbo.CustomerMaster.CustomerName, dbo.EntBookings.NetAmount
FROM
	dbo.BarcodeTable INNER JOIN dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
WHERE
	(dbo.BarcodeTable.DelQty = 0) AND (dbo.BarcodeTable.DueDate <=@Date) AND (dbo.EntBookings.BookingStatus <> ''5'') AND (dbo.EntBookings.BranchId=@BranchId)
GROUP BY 
	dbo.BarcodeTable.ClothDeliveryDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty,dbo.EntBookings.NetAmount
ORDER BY 
	dbo.EntBookings.BookingNumber

OPEN Delivery
	FETCH NEXT FROM Delivery INTO @BookingDate,@BookingNumber,@TotalQty,@CustomerName,@TotalPay 
	WHILE @@Fetch_Status=0
		BEGIN
			SET @AlreadyDelivered=''0''
			SET @ReadyClothes=''0''
			SELECT  @ReadyClothes=SUM(SNo) FROM BarcodeTable WHERE BookingNo=@BookingNumber AND StatusId<>''1'' AND StatusId<>''2''
			SELECT  @AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty) FROM  dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode	WHERE (dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) GROUP BY dbo.EntBookings.BookingNumber
			IF @AlreadyDelivered IS NULL
				SET @AlreadyDelivered=''0''
			IF @ReadyClothes IS NULL
				SET @ReadyClothes=''0''
			SET @BalQty=@ReadyClothes-@AlreadyDelivered			 
			SELECT @AllReadyPaid=SUM(dbo.EntPayment.PaymentMade) FROM dbo.EntBookings INNER JOIN dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber WHERE (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.EntPayment.PaymentDate <= @Date) 
			if @AllReadyPaid is null 
				set @AllReadyPaid	=''0''
			SET @Balance=@TotalPay-@AllReadyPaid
			INSERT INTO #temp1
				(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,BalQty,TotalPay,AllReadyPaid,Balance,ReadyClothes)
			VALUES
				(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@BalQty,@TotalPay,@AllReadyPaid,@Balance,@ReadyClothes)								
			FETCH NEXT FROM Delivery INTO @BookingDate,@BookingNumber,@TotalQty,@CustomerName,@TotalPay 
		END	
CLOSE Delivery
DEALLOCATE Delivery
SELECT * FROM #temp
UNION
SELECT * FROM #temp1
DROP TABLE #temp
DROP TABLE #temp1
END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Dry_MultiplePaymentCustomerWise]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Manoj Kumar gupta>
-- Create date: <25-Aug-2011>
-- Description:	<Multiple Payment Screen>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Dry_MultiplePaymentCustomerWise]	

@Date datetime,
@CustCode Varchar(max)='''',
@BranchId varchar(max)

AS
BEGIN
	

DECLARE @Amt FLOAT, @BookingDate varchar(max),@CustomerName varchar(max),@TotalQty int ,@BookingNumber int,@AlreadyDelivered int,@BalQty int,@TotalPay FLOAT,@AllReadyPaid FLOAT,@Balance FLOAT,@ReadyClothes INT
CREATE TABLE #temp(BookingDate varchar(max),BookingNumber int,CustomerName varchar(max),TotalQty int, AlreadyDelivered int, BalQty int,TotalPay FLOAT,AllReadyPaid FLOAT,Balance FLOAT,ReadyClothes INT)
DECLARE Delivery cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for

SELECT 
	SUM(dbo.EntPayment.PaymentMade) - dbo.EntBookings.NetAmount AS AMT, dbo.EntBookings.BookingNumber
FROM
	dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
WHERE
	(dbo.EntBookings.BookingDeliveryDate <= @Date) AND  dbo.EntBookings.BookingByCustomer=@CustCode AND (dbo.EntBookings.BookingStatus <> ''5'') AND (dbo.EntBookings.BranchId=@BranchId)
GROUP BY 
	dbo.EntBookings.BookingNumber, dbo.EntBookings.NetAmount
ORDER BY 
	dbo.EntBookings.BookingNumber
OPEN Delivery
	FETCH NEXT FROM Delivery INTO @Amt, @BookingNumber
	WHILE @@Fetch_Status=0
		BEGIN			
			IF @Amt<>0
				BEGIN
					SELECT 
						@BookingDate=CONVERT(varchar, dbo.EntBookings.BookingDate, 106), @TotalQty=dbo.EntBookings.Qty,@CustomerName=dbo.CustomerMaster.CustomerName,@TotalPay=dbo.EntBookings.NetAmount
					FROM
				        dbo.BarcodeTable INNER JOIN dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN  dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
					WHERE
						(dbo.EntBookings.BookingNumber = @BookingNumber)
					GROUP BY dbo.BarcodeTable.ClothDeliveryDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty, 
						dbo.EntBookings.NetAmount
					ORDER BY 
						dbo.EntBookings.BookingNumber
					SET @AlreadyDelivered=''0''
					SET @ReadyClothes=''0''
					SELECT  @AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty) FROM  dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode	WHERE (dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) GROUP BY dbo.EntBookings.BookingNumber
					SELECT  @ReadyClothes=SUM(SNo) FROM BarcodeTable WHERE BookingNo=@BookingNumber AND StatusId<>''1'' AND StatusId<>''2''
					IF @AlreadyDelivered IS NULL
						SET @AlreadyDelivered=''0''
					IF @ReadyClothes IS NULL
						SET @ReadyClothes=''0''
					SET @BalQty=@ReadyClothes-@AlreadyDelivered			 
					SELECT @AllReadyPaid=SUM(dbo.EntPayment.PaymentMade) FROM dbo.EntBookings INNER JOIN dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber WHERE (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.EntPayment.PaymentDate <= @Date) 
					if @AllReadyPaid is null 
						set @AllReadyPaid	=''0''
					SET @Balance=@TotalPay-@AllReadyPaid
					INSERT INTO #temp
						(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,BalQty,TotalPay,AllReadyPaid,Balance,ReadyClothes)
					VALUES
						(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@BalQty,@TotalPay,@AllReadyPaid,@Balance,@ReadyClothes)								
					
				END
					FETCH NEXT FROM Delivery INTO @Amt, @BookingNumber
			END	
CLOSE Delivery
DEALLOCATE Delivery
SELECT * FROM #temp
order by BookingNumber
DROP TABLE #temp
END



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Dry_MultipleBothCustomerWise]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'





-- =============================================
-- Author:		<Manoj Kumar gupta>
-- Create date: <25-Aug-2011>
-- Description:	<Multiple Payment Screen>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Dry_MultipleBothCustomerWise]	

@Date datetime,
@CustCode varchar(max),
@BranchId varchar(max)

AS
BEGIN
	

DECLARE @Amt FLOAT, @BookingDate varchar(max),@CustomerName varchar(max),@TotalQty int ,@BookingNumber int,@AlreadyDelivered int,@BalQty int,@TotalPay FLOAT,@AllReadyPaid FLOAT,@Balance FLOAT,@ReadyClothes INT
CREATE TABLE #temp(BookingDate varchar(max),BookingNumber int,CustomerName varchar(max),TotalQty int, AlreadyDelivered int, BalQty int,TotalPay FLOAT,AllReadyPaid FLOAT,Balance FLOAT,ReadyClothes INT)
--SET @Date=''11 Feb 2012''
DECLARE Delivery cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for


SELECT 
	SUM(dbo.EntPayment.PaymentMade) - dbo.EntBookings.NetAmount AS AMT, dbo.EntBookings.BookingNumber
FROM
	dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
WHERE
	(dbo.EntBookings.BookingDeliveryDate <= @Date) AND  dbo.EntBookings.BookingByCustomer=@CustCode AND (dbo.EntBookings.BookingStatus <> ''5'') AND (dbo.EntBookings.BranchId=@BranchId)
GROUP BY 
	dbo.EntBookings.BookingNumber, dbo.EntBookings.NetAmount
ORDER BY 
	dbo.EntBookings.BookingNumber
OPEN Delivery
	FETCH NEXT FROM Delivery INTO @Amt, @BookingNumber
	WHILE @@Fetch_Status=0
		BEGIN			
			IF @Amt<>0
				BEGIN
					SELECT 
						@BookingDate=CONVERT(varchar, dbo.EntBookings.BookingDate, 106), @TotalQty=dbo.EntBookings.Qty,@CustomerName=dbo.CustomerMaster.CustomerName,@TotalPay=dbo.EntBookings.NetAmount
					FROM
				        dbo.BarcodeTable INNER JOIN dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN  dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
					WHERE
						(dbo.EntBookings.BookingNumber = @BookingNumber)
					GROUP BY dbo.BarcodeTable.ClothDeliveryDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty, 
						dbo.EntBookings.NetAmount
					ORDER BY 
						dbo.EntBookings.BookingNumber
					SET @AlreadyDelivered=''0''
					SET @ReadyClothes=''0''
					SELECT  @AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty) FROM  dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode	WHERE (dbo.BarcodeTable.DelQty = 1) AND (dbo.BarcodeTable.StatusId<>''1'') AND (dbo.BarcodeTable.StatusId<>''2'') AND (dbo.EntBookings.BookingNumber = @BookingNumber) GROUP BY dbo.EntBookings.BookingNumber
					SELECT  @ReadyClothes=SUM(SNo) FROM BarcodeTable WHERE BookingNo=@BookingNumber AND StatusId<>''1'' AND StatusId<>''2''
					IF @AlreadyDelivered IS NULL
						SET @AlreadyDelivered=''0''
					IF @ReadyClothes IS NULL
						SET @ReadyClothes=''0''
					SET @BalQty=@ReadyClothes-@AlreadyDelivered			 
					SELECT @AllReadyPaid=SUM(dbo.EntPayment.PaymentMade) FROM dbo.EntBookings INNER JOIN dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber WHERE (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.EntPayment.PaymentDate <= @Date) 
					if @AllReadyPaid is null 
						set @AllReadyPaid	=''0''
					SET @Balance=@TotalPay-@AllReadyPaid
					INSERT INTO #temp
						(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,BalQty,TotalPay,AllReadyPaid,Balance,ReadyClothes)
					VALUES
						(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@BalQty,@TotalPay,@AllReadyPaid,@Balance,@ReadyClothes)								
					
				END
					FETCH NEXT FROM Delivery INTO @Amt, @BookingNumber
			END	
CLOSE Delivery
DEALLOCATE Delivery
----------------------Second Out Put-------------------
CREATE TABLE #temp1(BookingDate varchar(max),BookingNumber int,CustomerName varchar(max),TotalQty int, AlreadyDelivered int, BalQty int,TotalPay FLOAT,AllReadyPaid FLOAT,Balance FLOAT,ReadyClothes INT)
DECLARE Delivery cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for
SELECT  
	convert(varchar,dbo.EntBookings.BookingDate,106) as BookingDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.Qty, dbo.CustomerMaster.CustomerName, dbo.EntBookings.NetAmount
FROM
	dbo.BarcodeTable INNER JOIN dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
WHERE
	(dbo.BarcodeTable.DelQty = 0) AND (dbo.BarcodeTable.DueDate <=@Date) AND dbo.BarcodeTable.BookingByCustomer=@CustCode AND (dbo.EntBookings.BookingStatus <> ''5'') AND (dbo.EntBookings.BranchId=@BranchId)
GROUP BY 
	dbo.BarcodeTable.ClothDeliveryDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty,dbo.EntBookings.NetAmount
ORDER BY 
	dbo.EntBookings.BookingNumber

OPEN Delivery
	FETCH NEXT FROM Delivery INTO @BookingDate,@BookingNumber,@TotalQty,@CustomerName,@TotalPay 
	WHILE @@Fetch_Status=0
		BEGIN
			SET @AlreadyDelivered=''0''
			SET @ReadyClothes=''0''
			SELECT  @ReadyClothes=SUM(SNo) FROM BarcodeTable WHERE BookingNo=@BookingNumber AND StatusId<>''1'' AND StatusId<>''2''
			SELECT  @AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty) FROM  dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode	WHERE (dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) GROUP BY dbo.EntBookings.BookingNumber
			IF @AlreadyDelivered IS NULL
				SET @AlreadyDelivered=''0''
			IF @ReadyClothes IS NULL
				SET @ReadyClothes=''0''
			SET @BalQty=@ReadyClothes-@AlreadyDelivered			 
			SELECT @AllReadyPaid=SUM(dbo.EntPayment.PaymentMade) FROM dbo.EntBookings INNER JOIN dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber WHERE (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.EntPayment.PaymentDate <= @Date) 
			if @AllReadyPaid is null 
				set @AllReadyPaid	=''0''
			SET @Balance=@TotalPay-@AllReadyPaid
			INSERT INTO #temp1
				(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,BalQty,TotalPay,AllReadyPaid,Balance,ReadyClothes)
			VALUES
				(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@BalQty,@TotalPay,@AllReadyPaid,@Balance,@ReadyClothes)								
			FETCH NEXT FROM Delivery INTO @BookingDate,@BookingNumber,@TotalQty,@CustomerName,@TotalPay 
		END	
CLOSE Delivery
DEALLOCATE Delivery
SELECT * FROM #temp
UNION
SELECT * FROM #temp1
DROP TABLE #temp
DROP TABLE #temp1
END



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_AreaWiseBookingReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'




/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport ''1 SEP 2010'',''2 sep 2010''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_AreaWiseBookingReport]
	(
		@BookDate1 datetime,
		@BookDate2 datetime,
		@AreaType varchar(max),
		@BranchId Varchar(max)
	)
AS
BEGIN
	SELECT EB.BookingNumber, convert(varchar, EB.BookingDate,106) as BookingDate, EB.NetAmount, SUM(COALESCE (EP.PaymentMade, 0)) AS PaymentMade,
    SUM(COALESCE (EP.DiscountOnPayment, 0)) AS DiscountOnPayment, EB.NetAmount - SUM(COALESCE (EP.PaymentMade, 0) 
    + COALESCE (EP.DiscountOnPayment, 0)) AS DuePayment, EB.BookingAcceptedByUserId, EB.BookingStatus, dbo.CustomerMaster.AreaLocation,CONVERT(VARCHAR,EB.BookingDeliveryDate,106) as DeliveryDate
	FROM dbo.EntBookings AS EB INNER JOIN
    dbo.CustomerMaster ON EB.BookingByCustomer = dbo.CustomerMaster.CustomerCode LEFT OUTER JOIN
    dbo.EntPayment AS EP ON EB.BookingNumber = EP.BookingNumber
	WHERE ((dbo.CustomerMaster.AreaLocation) = (@AreaType)) AND (EB.BookingDate BETWEEN @BookDate1 AND @BookDate2) And (EB.BranchId=@BranchId)
	GROUP BY EB.BookingNumber, EB.BookingDate, EB.NetAmount, EB.BookingAcceptedByUserId, EB.BookingStatus, dbo.CustomerMaster.AreaLocation,BookingDeliveryDate
END




' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rpt_barcodprint]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_rpt_barcodprint]
	@Flag int=0,
	@BookingNo VARCHAR(MAX)='''',
	@RowIndex int=null,
	@BranchId varchar(15)=''''
	
AS
BEGIN
	IF(@Flag=1)
	 BEGIN
		SELECT BookingNumber as BookingNo FROM EntBookings WHERE BranchId=@BranchId  ORDER BY BookingID desc
	 END
	IF(@Flag=2)
	 BEGIN
		SELECT distinct RowIndex from BarcodeTable WHERE BookingNo=@BookingNo AND BranchId=@BranchId
	 END
	IF(@Flag=3)
	 BEGIN
			SELECT colour, convert(varchar, BookingDate,106) as BookingDate,CurrentTime,convert(varchar, DueDate,106) as DueDate ,Item,Barcode,Process,BookingNo,RowIndex,CustomerName from BarcodeTable bt left outer join dbo.CustomerMaster cm on bt.BookingByCustomer=cm.CustomerCode


			where BookingNo=@BookingNo and ( @RowIndex is null or RowIndex=@RowIndex)

	 END
IF(@Flag=4)
	 BEGIN			
		SELECT dbo.BarcodeTable.Colour, CONVERT(varchar, dbo.BarcodeTable.BookingDate, 3) AS BookingDate, dbo.BarcodeTable.CurrentTime, 
			   CONVERT(varchar, dbo.BarcodeTable.DueDate, 3) AS DueDate, dbo.BarcodeTable.Item, dbo.BarcodeTable.BarCode, dbo.BarcodeTable.Process, 
			   dbo.BarcodeTable.BookingNo, dbo.BarcodeTable.RowIndex, dbo.CustomerMaster.CustomerName,dbo.BarcodeTable.ItemRemarks as ItemRemark,dbo.BarcodeTable.ItemExtraprocessType,dbo.BarcodeTable.ItemTotalandSubTotal,dbo.BarcodeTable.ItemExtraprocessType2
		FROM   dbo.BarcodeTable INNER JOIN dbo.CustomerMaster ON dbo.BarcodeTable.BookingByCustomer = dbo.CustomerMaster.CustomerCode
		WHERE  (dbo.BarcodeTable.BookingNo = @BookingNo) and ( @RowIndex is null or dbo.BarcodeTable.RowIndex=@RowIndex) and dbo.BarcodeTable.BranchId=@BranchId
		order by dbo.BarcodeTable.RowIndex
	 END
END






' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Dry_MultiplePendingPayment]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Manoj Kumar gupta>
-- Create date: <25-Aug-2011>
-- Description:	<Multiple Payment Screen>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Dry_MultiplePendingPayment]	

@Date datetime,
@BranchId varchar(max)

AS
BEGIN
	

DECLARE @Amt FLOAT, @BookingDate varchar(max),@CustomerName varchar(max),@TotalQty int ,@BookingNumber int,@AlreadyDelivered int,@BalQty int,@TotalPay FLOAT,@AllReadyPaid FLOAT,@Balance FLOAT,@ReadyClothes INT
CREATE TABLE #temp(BookingDate varchar(max),BookingNumber int,CustomerName varchar(max),TotalQty int, AlreadyDelivered int, BalQty int,TotalPay FLOAT,AllReadyPaid FLOAT,Balance FLOAT,ReadyClothes INT)
DECLARE Delivery cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for

SELECT 
	SUM(dbo.EntPayment.PaymentMade) - dbo.EntBookings.NetAmount AS AMT, dbo.EntBookings.BookingNumber
FROM
	dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
WHERE
	(dbo.EntBookings.BookingDeliveryDate <= @Date) AND (dbo.EntBookings.BookingStatus <> ''5'') AND (dbo.EntBookings.BranchId=@BranchId)
GROUP BY 
	dbo.EntBookings.BookingNumber, dbo.EntBookings.NetAmount
ORDER BY 
	dbo.EntBookings.BookingNumber
OPEN Delivery
	FETCH NEXT FROM Delivery INTO @Amt, @BookingNumber
	WHILE @@Fetch_Status=0
		BEGIN			
			IF @Amt<>0
				BEGIN
					SELECT 
						@BookingDate=CONVERT(varchar, dbo.EntBookings.BookingDate, 106), @TotalQty=dbo.EntBookings.Qty,@CustomerName=dbo.CustomerMaster.CustomerName,@TotalPay=dbo.EntBookings.NetAmount
					FROM
				        dbo.BarcodeTable INNER JOIN dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN  dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
					WHERE
						(dbo.EntBookings.BookingNumber = @BookingNumber)
					GROUP BY dbo.BarcodeTable.ClothDeliveryDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty, 
						dbo.EntBookings.NetAmount
					ORDER BY 
						dbo.EntBookings.BookingNumber
					SET @AlreadyDelivered=''0''
					SET @ReadyClothes=''0''
					SELECT  @AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty) FROM  dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode	WHERE (dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) GROUP BY dbo.EntBookings.BookingNumber
					SELECT  @ReadyClothes=SUM(SNo) FROM BarcodeTable WHERE BookingNo=@BookingNumber AND StatusId<>''1'' AND StatusId<>''2''
					IF @AlreadyDelivered IS NULL
						SET @AlreadyDelivered=''0''
					IF @ReadyClothes IS NULL
						SET @ReadyClothes=''0''
					SET @BalQty=@ReadyClothes-@AlreadyDelivered			 
					SELECT @AllReadyPaid=SUM(dbo.EntPayment.PaymentMade) FROM dbo.EntBookings INNER JOIN dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber WHERE (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.EntPayment.PaymentDate <= @Date) 
					if @AllReadyPaid is null 
						set @AllReadyPaid	=''0''
					SET @Balance=@TotalPay-@AllReadyPaid
					INSERT INTO #temp
						(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,BalQty,TotalPay,AllReadyPaid,Balance,ReadyClothes)
					VALUES
						(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@BalQty,@TotalPay,@AllReadyPaid,@Balance,@ReadyClothes)								
					
				END
					FETCH NEXT FROM Delivery INTO @Amt, @BookingNumber
			END	
CLOSE Delivery
DEALLOCATE Delivery
SELECT * FROM #temp
order by BookingNumber
DROP TABLE #temp
END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Dry_MultipleClothDelivery_ByCustomer]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		<Manoj Kumar gupta>
-- Create date: <25-Aug-2011>
-- Description:	<Multiple Payment Screen>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Dry_MultipleClothDelivery_ByCustomer]	

@Date datetime,
@CustCode varchar(max),
@BranchId varchar(max)=''''
AS
BEGIN
	

DECLARE @BookingDate varchar(max),@CustomerName varchar(max),@TotalQty int ,@BookingNumber INT,@AlreadyDelivered int,@BalQty int,@TotalPay FLOAT,@AllReadyPaid FLOAT,@Balance FLOAT,@ReadyClothes INT
CREATE TABLE #temp(BookingDate varchar(max),BookingNumber INT,CustomerName varchar(max),TotalQty int, AlreadyDelivered int, BalQty int,TotalPay FLOAT,AllReadyPaid FLOAT,Balance FLOAT,ReadyClothes INT)
DECLARE Delivery cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for
SELECT  
	convert(varchar,dbo.EntBookings.BookingDate,106) as BookingDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.Qty, dbo.CustomerMaster.CustomerName, dbo.EntBookings.NetAmount
FROM
	dbo.BarcodeTable INNER JOIN dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
WHERE
	(dbo.BarcodeTable.DelQty = 0) AND (dbo.BarcodeTable.DueDate <=@Date) AND  dbo.EntBookings.BookingByCustomer=@CustCode AND (dbo.EntBookings.BookingStatus <> ''5'') AND (dbo.EntBookings.BranchId=@BranchId)
GROUP BY 
	dbo.EntBookings.BookingNumber,dbo.BarcodeTable.ClothDeliveryDate,dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty,dbo.EntBookings.NetAmount
ORDER BY 
	dbo.EntBookings.BookingNumber

OPEN Delivery
	FETCH NEXT FROM Delivery INTO @BookingDate,@BookingNumber,@TotalQty,@CustomerName,@TotalPay 
	WHILE @@Fetch_Status=0
		BEGIN
			SET @AlreadyDelivered=''0''
			SET @ReadyClothes=''0''
			SELECT  @AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty) FROM  dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode	WHERE (dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) GROUP BY dbo.EntBookings.BookingNumber
			SELECT  @ReadyClothes=SUM(SNo) FROM BarcodeTable WHERE BookingNo=@BookingNumber AND StatusId<>''1'' AND StatusId<>''2''
			IF @AlreadyDelivered IS NULL
				SET @AlreadyDelivered=''0''
			SET @BalQty=@TotalQty-@AlreadyDelivered			 
			SELECT @AllReadyPaid=SUM(dbo.EntPayment.PaymentMade) FROM dbo.EntBookings INNER JOIN dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber WHERE (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.EntPayment.PaymentDate <= @Date) 
			if @AllReadyPaid is null 
				set @AllReadyPaid	=''0''
			IF @ReadyClothes IS NULL
				SET @ReadyClothes=''0''
			SET @BalQty=@ReadyClothes-@AlreadyDelivered
			SET @Balance=@TotalPay-@AllReadyPaid
			INSERT INTO #temp
				(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,BalQty,TotalPay,AllReadyPaid,Balance,ReadyClothes)
			VALUES
				(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@BalQty,@TotalPay,@AllReadyPaid,@Balance,@ReadyClothes)								
			FETCH NEXT FROM Delivery INTO @BookingDate,@BookingNumber,@TotalQty,@CustomerName,@TotalPay 
		END	
CLOSE Delivery
DEALLOCATE Delivery
SELECT * FROM #temp
order by BookingNumber
DROP TABLE #temp
END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Dry_MultipleClothDelivery]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Manoj Kumar gupta>
-- Create date: <25-Aug-2011>
-- Description:	<Multiple Payment Screen>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Dry_MultipleClothDelivery]	

@Date datetime='''',
@BranchId varchar(max)=''''

AS
BEGIN
	

DECLARE @BookingDate varchar(max),@CustomerName varchar(max),@TotalQty int ,@BookingNumber INT,@AlreadyDelivered int,@BalQty int,@TotalPay FLOAT,@AllReadyPaid FLOAT,@Balance FLOAT,@ReadyClothes INT
CREATE TABLE #temp(BookingDate varchar(max),BookingNumber INT,CustomerName varchar(max),TotalQty int, AlreadyDelivered int, BalQty int,TotalPay FLOAT,AllReadyPaid FLOAT,Balance FLOAT,ReadyClothes INT)
DECLARE Delivery cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for
SELECT  
	convert(varchar,dbo.EntBookings.BookingDate,106) as BookingDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.Qty, dbo.CustomerMaster.CustomerName, dbo.EntBookings.NetAmount
FROM
	dbo.BarcodeTable INNER JOIN dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
WHERE
	(dbo.BarcodeTable.DelQty = 0) AND (dbo.BarcodeTable.DueDate <=@Date) AND (dbo.EntBookings.BookingStatus <> ''5'') AND (dbo.EntBookings.BranchId=@BranchId)
GROUP BY 
	dbo.EntBookings.BookingNumber,dbo.BarcodeTable.ClothDeliveryDate,dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty,dbo.EntBookings.NetAmount
ORDER BY 
	dbo.EntBookings.BookingNumber

OPEN Delivery
	FETCH NEXT FROM Delivery INTO @BookingDate,@BookingNumber,@TotalQty,@CustomerName,@TotalPay 
	WHILE @@Fetch_Status=0
		BEGIN
			SET @AlreadyDelivered=''0''
			SET @ReadyClothes=''0''
			SELECT  @AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty) FROM  dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode	WHERE (dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) GROUP BY dbo.EntBookings.BookingNumber
			SELECT  @ReadyClothes=SUM(SNo) FROM BarcodeTable WHERE BookingNo=@BookingNumber AND StatusId<>''1'' AND StatusId<>''2''
			IF @AlreadyDelivered IS NULL
				SET @AlreadyDelivered=''0''
			SET @BalQty=@TotalQty-@AlreadyDelivered			 
			SELECT @AllReadyPaid=SUM(dbo.EntPayment.PaymentMade) FROM dbo.EntBookings INNER JOIN dbo.EntPayment ON dbo.EntBookings.BookingNumber = dbo.EntPayment.BookingNumber WHERE (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.EntPayment.PaymentDate <= @Date) 
			if @AllReadyPaid is null 
				set @AllReadyPaid	=''0''
			IF @ReadyClothes IS NULL
				SET @ReadyClothes=''0''
			SET @BalQty=@ReadyClothes-@AlreadyDelivered
			SET @Balance=@TotalPay-@AllReadyPaid
			INSERT INTO #temp
				(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,BalQty,TotalPay,AllReadyPaid,Balance,ReadyClothes)
			VALUES
				(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@BalQty,@TotalPay,@AllReadyPaid,@Balance,@ReadyClothes)								
			FETCH NEXT FROM Delivery INTO @BookingDate,@BookingNumber,@TotalQty,@CustomerName,@TotalPay 
		END	
CLOSE Delivery
DEALLOCATE Delivery
SELECT * FROM #temp
order by BookingNumber
DROP TABLE #temp
END



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proc_ReturnChoth]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'





-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <10-Feb-2012>
-- Description:	<Return Cloth From Challan In OR Out>
-- =============================================
CREATE PROCEDURE [dbo].[Proc_ReturnChoth]
	@FDate DATETIME='''',
	@UDate DATETIME='''',
	@InvoiceNo VARCHAR(MAX)='''',
	@CustCode VARCHAR(MAX)='''',
	@Flag VARCHAR(MAX)='''',
	@BranchId Varchar(MAX)=''''
	
AS
BEGIN
	IF @Flag=1
		SELECT ROW_NUMBER() OVER(ORDER BY dbo.BarcodeTable.BookingNo) AS BNO, dbo.BarcodeTable.BookingNo, dbo.CustomerMaster.CustomerName, CONVERT(VARCHAR ,dbo.BarcodeTable.BookingDate,106) AS BookingDate,CONVERT(VARCHAR,dbo.BarcodeTable.ClothDeliveryDate,106) AS ClothDeliveryDate, dbo.BarcodeTable.RemoveFrom, dbo.BarcodeTable.Item,  dbo.BarcodeTable.UserId AS BookingAcceptedByUserId, dbo.BarcodeTable.DeliverItemStaus FROM  dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.BarcodeTable.RemoveFrom = ''OUTWARD'') AND (dbo.BarcodeTable.ClothDeliveryDate  BETWEEN @FDate AND @UDate) AND (dbo.EntBookings.BranchId=@BranchId) ORDER BY dbo.BarcodeTable.ClothDeliveryDate 
	IF @Flag=2
		SELECT ROW_NUMBER() OVER(ORDER BY dbo.BarcodeTable.BookingNo) AS BNO, dbo.BarcodeTable.BookingNo, dbo.CustomerMaster.CustomerName, CONVERT(VARCHAR ,dbo.BarcodeTable.BookingDate,106) AS BookingDate,CONVERT(VARCHAR,dbo.BarcodeTable.ClothDeliveryDate,106) AS ClothDeliveryDate, dbo.BarcodeTable.RemoveFrom, dbo.BarcodeTable.Item,  dbo.BarcodeTable.UserId AS BookingAcceptedByUserId, dbo.BarcodeTable.DeliverItemStaus FROM  dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.BarcodeTable.RemoveFrom = ''OUTWARD'') AND (dbo.BarcodeTable.ClothDeliveryDate  BETWEEN @FDate AND @UDate) AND  dbo.CustomerMaster.CustomerCode=@CustCode AND dbo.EntBookings.BranchId=@BranchId ORDER BY dbo.BarcodeTable.ClothDeliveryDate 
	IF @Flag=3
		SELECT ROW_NUMBER() OVER(ORDER BY dbo.BarcodeTable.BookingNo) AS BNO, dbo.BarcodeTable.BookingNo, dbo.CustomerMaster.CustomerName, CONVERT(VARCHAR,dbo.BarcodeTable.BookingDate,106) AS BookingDate, CONVERT(VARCHAR,dbo.BarcodeTable.ClothDeliveryDate,106) AS ClothDeliveryDate, dbo.BarcodeTable.RemoveFrom, dbo.BarcodeTable.Item,  dbo.BarcodeTable.UserId AS BookingAcceptedByUserId, dbo.BarcodeTable.DeliverItemStaus FROM  dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.BarcodeTable.RemoveFrom = ''OUTWARD'') AND (dbo.BarcodeTable.BookingNo=@InvoiceNo) AND (dbo.EntBookings.BranchId=@BranchId)
	IF @Flag=4
		SELECT ROW_NUMBER() OVER(ORDER BY dbo.BarcodeTable.BookingNo) AS BNO, dbo.BarcodeTable.BookingNo, dbo.CustomerMaster.CustomerName, CONVERT(VARCHAR,dbo.BarcodeTable.BookingDate,106) AS BookingDate, CONVERT(VARCHAR,dbo.BarcodeTable.ClothDeliveryDate,106) AS ClothDeliveryDate, dbo.BarcodeTable.RemoveFrom, dbo.BarcodeTable.Item,  dbo.BarcodeTable.UserId AS BookingAcceptedByUserId, dbo.BarcodeTable.DeliverItemStaus FROM  dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.BarcodeTable.RemoveFrom = ''INWARD'') AND (dbo.BarcodeTable.ClothDeliveryDate  BETWEEN @FDate AND @UDate) AND (dbo.EntBookings.BranchId=@BranchId) ORDER BY dbo.BarcodeTable.ClothDeliveryDate 
	IF @Flag=5
		SELECT ROW_NUMBER() OVER(ORDER BY dbo.BarcodeTable.BookingNo) AS BNO, dbo.BarcodeTable.BookingNo, dbo.CustomerMaster.CustomerName, CONVERT(VARCHAR ,dbo.BarcodeTable.BookingDate,106) AS BookingDate,CONVERT(VARCHAR,dbo.BarcodeTable.ClothDeliveryDate,106) AS ClothDeliveryDate, dbo.BarcodeTable.RemoveFrom, dbo.BarcodeTable.Item,  dbo.BarcodeTable.UserId AS BookingAcceptedByUserId, dbo.BarcodeTable.DeliverItemStaus FROM  dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.BarcodeTable.RemoveFrom = ''INWARD'') AND (dbo.BarcodeTable.ClothDeliveryDate  BETWEEN @FDate AND @UDate) AND  dbo.CustomerMaster.CustomerCode=@CustCode AND dbo.EntBookings.BranchId=@BranchId ORDER BY dbo.BarcodeTable.ClothDeliveryDate 
	IF @Flag=6
		SELECT ROW_NUMBER() OVER(ORDER BY dbo.BarcodeTable.BookingNo) AS BNO, dbo.BarcodeTable.BookingNo, dbo.CustomerMaster.CustomerName, CONVERT(VARCHAR,dbo.BarcodeTable.BookingDate,106) AS BookingDate, CONVERT(VARCHAR,dbo.BarcodeTable.ClothDeliveryDate,106) AS ClothDeliveryDate, dbo.BarcodeTable.RemoveFrom, dbo.BarcodeTable.Item,  dbo.BarcodeTable.UserId AS BookingAcceptedByUserId, dbo.BarcodeTable.DeliverItemStaus FROM  dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.BarcodeTable.RemoveFrom = ''INWARD'') AND (dbo.BarcodeTable.BookingNo=@InvoiceNo) AND (dbo.EntBookings.BranchId=@BranchId)
	IF @Flag=7
		BEGIN
			CREATE TABLE #TmpTable(BookingNo VARCHAR(MAX),CustomerName VARCHAR(MAX),BookingDate VARCHAR(MAX),ClothDeliveryDate VARCHAR(MAX),RemoveFrom VARCHAR(MAX),Item VARCHAR(MAX),BookingAcceptedByUserId VARCHAR(MAX),DeliverItemStatus VARCHAR(MAX))
			INSERT INTO #TmpTable 
			SELECT dbo.BarcodeTable.BookingNo, dbo.CustomerMaster.CustomerName, CONVERT(VARCHAR,dbo.BarcodeTable.BookingDate,106) AS BookingDate, CONVERT(VARCHAR,dbo.BarcodeTable.ClothDeliveryDate,106) AS ClothDeliveryDate, dbo.BarcodeTable.RemoveFrom, dbo.BarcodeTable.Item,  dbo.BarcodeTable.UserId AS BookingAcceptedByUserId, dbo.BarcodeTable.DeliverItemStaus FROM  dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.BarcodeTable.RemoveFrom = ''OUTWARD'') AND (dbo.BarcodeTable.ClothDeliveryDate  BETWEEN @FDate AND @UDate) AND (dbo.EntBookings.BranchId=@BranchId)
			INSERT INTO #TmpTable 
			SELECT dbo.BarcodeTable.BookingNo, dbo.CustomerMaster.CustomerName, CONVERT(VARCHAR,dbo.BarcodeTable.BookingDate,106) AS BookingDate, CONVERT(VARCHAR,dbo.BarcodeTable.ClothDeliveryDate,106) AS ClothDeliveryDate, dbo.BarcodeTable.RemoveFrom, dbo.BarcodeTable.Item,  dbo.BarcodeTable.UserId AS BookingAcceptedByUserId, dbo.BarcodeTable.DeliverItemStaus FROM  dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE  (dbo.BarcodeTable.RemoveFrom = ''INWARD'') AND (dbo.BarcodeTable.ClothDeliveryDate  BETWEEN @FDate AND @UDate) AND (dbo.EntBookings.BranchId=@BranchId) ORDER BY RemoveFrom,ClothDeliveryDate			
			SELECT ROW_NUMBER() OVER(ORDER BY BookingNo) AS BNO, BookingNo ,CustomerName ,BookingDate ,ClothDeliveryDate AS ClothDeliveryDate ,RemoveFrom ,Item ,BookingAcceptedByUserId ,DeliverItemStatus  AS DeliverItemStaus  FROM #TmpTable ORDER BY BNO
			DROP TABLE #TmpTable
		END
	IF @Flag=8
		BEGIN
			CREATE TABLE #TmpTable1(BookingNo VARCHAR(MAX),CustomerName VARCHAR(MAX),BookingDate VARCHAR(MAX),ClothDeliveryDate VARCHAR(MAX),RemoveFrom VARCHAR(MAX),Item VARCHAR(MAX),BookingAcceptedByUserId VARCHAR(MAX),DeliverItemStatus VARCHAR(MAX))
			INSERT INTO #TmpTable1 
			SELECT dbo.BarcodeTable.BookingNo, dbo.CustomerMaster.CustomerName, CONVERT(VARCHAR,dbo.BarcodeTable.BookingDate,106) AS BookingDate, CONVERT(VARCHAR,dbo.BarcodeTable.ClothDeliveryDate,106) AS ClothDeliveryDate, dbo.BarcodeTable.RemoveFrom, dbo.BarcodeTable.Item,  dbo.BarcodeTable.UserId AS BookingAcceptedByUserId, dbo.BarcodeTable.DeliverItemStaus FROM  dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.BarcodeTable.RemoveFrom = ''OUTWARD'') AND (dbo.BarcodeTable.ClothDeliveryDate  BETWEEN @FDate AND @UDate) AND   dbo.CustomerMaster.CustomerCode=@CustCode AND dbo.EntBookings.BranchId=@BranchId
			INSERT INTO #TmpTable1 
			SELECT dbo.BarcodeTable.BookingNo, dbo.CustomerMaster.CustomerName, CONVERT(VARCHAR,dbo.BarcodeTable.BookingDate,106) AS BookingDate, CONVERT(VARCHAR,dbo.BarcodeTable.ClothDeliveryDate,106) AS ClothDeliveryDate, dbo.BarcodeTable.RemoveFrom, dbo.BarcodeTable.Item,  dbo.BarcodeTable.UserId AS BookingAcceptedByUserId, dbo.BarcodeTable.DeliverItemStaus FROM  dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE  (dbo.BarcodeTable.RemoveFrom = ''INWARD'') AND (dbo.BarcodeTable.ClothDeliveryDate  BETWEEN @FDate AND @UDate) AND  dbo.CustomerMaster.CustomerCode=@CustCode AND dbo.EntBookings.BranchId=@BranchId ORDER BY RemoveFrom,ClothDeliveryDate		
			SELECT ROW_NUMBER() OVER(ORDER BY BookingNo) AS BNO, BookingNo ,CustomerName ,BookingDate ,ClothDeliveryDate AS ClothDeliveryDate,RemoveFrom ,Item ,BookingAcceptedByUserId ,DeliverItemStatus  AS DeliverItemStaus FROM #TmpTable1  ORDER BY BNO
			DROP TABLE #TmpTable1
		END
	IF @Flag=9
		BEGIN
			CREATE TABLE #TmpTable2(BookingNo VARCHAR(MAX),CustomerName VARCHAR(MAX),BookingDate VARCHAR(MAX),ClothDeliveryDate VARCHAR(MAX),RemoveFrom VARCHAR(MAX),Item VARCHAR(MAX),BookingAcceptedByUserId VARCHAR(MAX),DeliverItemStatus VARCHAR(MAX))
			INSERT INTO #TmpTable2 
			SELECT dbo.BarcodeTable.BookingNo, dbo.CustomerMaster.CustomerName, CONVERT(VARCHAR,dbo.BarcodeTable.BookingDate,106) AS BookingDate, CONVERT(VARCHAR,dbo.BarcodeTable.ClothDeliveryDate,106) AS ClothDeliveryDate, dbo.BarcodeTable.RemoveFrom, dbo.BarcodeTable.Item,  dbo.BarcodeTable.UserId AS BookingAcceptedByUserId, dbo.BarcodeTable.DeliverItemStaus FROM  dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.BarcodeTable.RemoveFrom = ''OUTWARD'') AND (dbo.BarcodeTable.BookingNo=@InvoiceNo) AND (dbo.EntBookings.BranchId=@BranchId)
			INSERT INTO #TmpTable2 
			SELECT dbo.BarcodeTable.BookingNo, dbo.CustomerMaster.CustomerName, CONVERT(VARCHAR,dbo.BarcodeTable.BookingDate,106) AS BookingDate, CONVERT(VARCHAR,dbo.BarcodeTable.ClothDeliveryDate,106) AS ClothDeliveryDate, dbo.BarcodeTable.RemoveFrom, dbo.BarcodeTable.Item,  dbo.BarcodeTable.UserId AS BookingAcceptedByUserId, dbo.BarcodeTable.DeliverItemStaus FROM  dbo.EntBookings INNER JOIN dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.BarcodeTable.RemoveFrom = ''INWARD'') AND (dbo.BarcodeTable.BookingNo=@InvoiceNo) AND (dbo.EntBookings.BranchId=@BranchId) ORDER BY RemoveFrom,ClothDeliveryDate	
			SELECT ROW_NUMBER() OVER(ORDER BY BookingNo) AS BNO, BookingNo ,CustomerName ,BookingDate ,ClothDeliveryDate AS ClothDeliveryDate,RemoveFrom ,Item ,BookingAcceptedByUserId ,DeliverItemStatus AS DeliverItemStaus FROM #TmpTable2  ORDER BY BNO
			DROP TABLE #TmpTable2
		END
END














' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_RecordsForBookingCancellation]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To Select Records for Booking Cancellation>
-- =============================================
EXEC Sp_Sel_RecordsForBookingCancellation ''1''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_RecordsForBookingCancellation]
	(
		@BookingNumber varchar(20)='''',
		@BranchId varchar(20)=''''
	)
AS
BEGIN
	SELECT EntBookings.BookingNumber ,EntBookings.BookingDate, CustomerCode, CustomerSalutation + '' ''  + CustomerName As CustomerName, CustomerAddress, NetAmount, BookingStatus, SUM(PaymentMade) As PaymentMade FROM EntBookings INNER JOIN CustomerMaster ON EntBookings.BookingByCustomer = CustomerMaster.CustomerCode LEFT JOIN EntPayment ON EntBookings.BookingNumber = EntPayment.BookingNumber
	WHERE EntBookings.BookingNumber = @BookingNumber AND EntBookings.BranchId=@BranchId
	GROUP BY EntBookings.BookingNumber ,EntBookings.BookingDate, CustomerCode, CustomerSalutation, CustomerName, CustomerAddress, NetAmount, BookingStatus
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_DeliveryReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'





/*
-- =============================================
-- Author:		<Manoj Kumar>
-- Create date: <26 July 2010>
-- Description:	<To select Challan details>
-- =============================================
EXEC Sp_Sel_DeliveryReport ''1 JAN 2011'', ''1 SEP 2010''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_DeliveryReport]
	(
		@BookingDate1 datetime,
		@BookingDate2 datetime,
		@Customerid varchar(max)='''',
		@Flag varchar(max)='''',
		@BookingNumber varchar(max)='''',
		@BranchId Varchar(max)=''''
	)
AS
BEGIN
	IF(@Flag=1)
	BEGIN
--	CREATE TABLE #TmpTable (BookingDate varchar(20), BookingNumber varchar(20), DeliveryDate varchar(20),PaymentMade float,NetAmount float,Balance float,CustName VARCHAR(MAX))
--	INSERT INTO #TmpTable(BookingDate, BookingNumber, DeliveryDate,PaymentMade,NetAmount,Balance,CustName)
--		SELECT    CONVERT(VARCHAR,dbo.EntBookings.BookingDate,106) AS BookingDate, dbo.EntBookings.BookingNumber, CONVERT(varchar, dbo.EntPayment.PaymentDate, 106) AS PaymentDate, 
--                      SUM(dbo.EntPayment.PaymentMade) AS Received, dbo.EntBookings.NetAmount AS NetAmount,round(dbo.EntBookings.NetAmount - SUM(dbo.EntPayment.PaymentMade),2)
--                      AS Balance,dbo.CustomerMaster.CustomerName
--		FROM         dbo.EntBookings INNER JOIN
--                      dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode LEFT OUTER JOIN
--                      dbo.EntPayment ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber
--		WHERE  EntBookings.BookingNumber=@BookingNumber AND EntPayment.PaymentMade<>0 And dbo.EntBookings.BranchId=@BranchId
--		GROUP BY CONVERT(varchar, dbo.EntPayment.PaymentDate, 106), dbo.EntBookings.BookingNumber, dbo.EntBookings.NetAmount, dbo.EntBookings.BookingDate,dbo.CustomerMaster.CustomerName
--	SELECT * FROM #TmpTable
--	DROP TABLE #TmpTable

		SELECT  
			EB.BookingID, CONVERT(varchar, EB.BookingDate, 106) AS BookingDate, EB.BookingNumber, dbo.CustomerMaster.CustomerName,CONVERT(varchar, EB.BookingDeliveryDate, 106) AS DeliveryDate, EB.Qty, EB.TotalCost, SUM(COALESCE (EP.DiscountOnPayment, 0)) + EB.DiscountAmt AS DiscountOnPayment,(SELECT ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) AS ST FROM dbo.EntBookingDetails WHERE (BookingNumber = EB.BookingNumber)GROUP BY BookingNumber) AS ST, EB.TotalCost - (SUM(COALESCE (EP.DiscountOnPayment, 0)) + EB.DiscountAmt) +(SELECT     ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) AS ST FROM dbo.EntBookingDetails AS EntBookingDetails_3 WHERE (BookingNumber = EB.BookingNumber) GROUP BY BookingNumber) AS NetAmount, SUM(COALESCE (EP.PaymentMade, 0)) AS PaymentMade, EB.TotalCost - (SUM(COALESCE (EP.DiscountOnPayment, 0)) + EB.DiscountAmt) + (SELECT     ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) AS ST FROM dbo.EntBookingDetails AS EntBookingDetails_2 WHERE (BookingNumber = EB.BookingNumber) GROUP BY BookingNumber) - SUM(COALESCE (EP.PaymentMade, 0)) AS DuePayment, EB.BookingStatus 
		FROM 
			dbo.EntBookings AS EB INNER JOIN dbo.CustomerMaster ON EB.BookingByCustomer = dbo.CustomerMaster.CustomerCode LEFT OUTER JOIN  dbo.EntPayment AS EP ON EB.BookingNumber = EP.BookingNumber 
		WHERE
			EB.BookingStatus<>''5'' AND EB.BookingNumber=@BookingNumber and EB.BranchId=@BranchId AND EP.PaymentMade<>0 
		GROUP BY 
			EB.BookingNumber, EB.BookingDate, EB.NetAmount, EB.Qty, EB.BookingStatus, EB.BookingID, EB.BookingDeliveryDate, dbo.CustomerMaster.CustomerName, EB.TotalCost, EB.DiscountAmt
		ORDER BY EB.BookingID
	END
	ELSE IF(@Flag=2)
	BEGIN
--	CREATE TABLE #TmpTable1 (BookingDate varchar(20), BookingNumber varchar(20), DeliveryDate varchar(20),PaymentMade float,NetAmount float,Balance float,CustName VARCHAR(MAX))
--	INSERT INTO #TmpTable1(BookingDate, BookingNumber, DeliveryDate,PaymentMade,NetAmount,Balance,CustName)
--		SELECT      CONVERT(VARCHAR,dbo.EntBookings.BookingDate,106) AS BookingDate, dbo.EntBookings.BookingNumber, CONVERT(varchar, dbo.EntPayment.PaymentDate, 106) AS PaymentDate, 
--                      SUM(dbo.EntPayment.PaymentMade) AS Received, dbo.EntBookings.NetAmount AS NetAmount,round(dbo.EntBookings.NetAmount - SUM(dbo.EntPayment.PaymentMade),2)
--                      AS Balance,dbo.CustomerMaster.CustomerName
--		FROM         dbo.EntBookings INNER JOIN
--                      dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode LEFT OUTER JOIN
--                      dbo.EntPayment ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber
--
--		WHERE (PaymentDate BETWEEN @BookingDate1 AND @BookingDate2 ) AND EntPayment.PaymentMade<>0 And dbo.EntBookings.BranchId=@BranchId
--		GROUP BY CONVERT(varchar, dbo.EntPayment.PaymentDate, 106), dbo.EntBookings.BookingNumber, dbo.EntBookings.NetAmount, dbo.EntBookings.BookingDate,dbo.CustomerMaster.CustomerName
--	SELECT * FROM #TmpTable1
--	DROP TABLE #TmpTable1
		SELECT  
			EB.BookingID, CONVERT(varchar, EB.BookingDate, 106) AS BookingDate, EB.BookingNumber, dbo.CustomerMaster.CustomerName,CONVERT(varchar, EB.BookingDeliveryDate, 106) AS DeliveryDate, EB.Qty, EB.TotalCost, SUM(COALESCE (EP.DiscountOnPayment, 0)) + EB.DiscountAmt AS DiscountOnPayment,(SELECT ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) AS ST FROM dbo.EntBookingDetails WHERE (BookingNumber = EB.BookingNumber)GROUP BY BookingNumber) AS ST, EB.TotalCost - (SUM(COALESCE (EP.DiscountOnPayment, 0)) + EB.DiscountAmt) +(SELECT     ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) AS ST FROM dbo.EntBookingDetails AS EntBookingDetails_3 WHERE (BookingNumber = EB.BookingNumber) GROUP BY BookingNumber) AS NetAmount, SUM(COALESCE (EP.PaymentMade, 0)) AS PaymentMade, EB.TotalCost - (SUM(COALESCE (EP.DiscountOnPayment, 0)) + EB.DiscountAmt) + (SELECT     ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) AS ST FROM dbo.EntBookingDetails AS EntBookingDetails_2 WHERE (BookingNumber = EB.BookingNumber) GROUP BY BookingNumber) - SUM(COALESCE (EP.PaymentMade, 0)) AS DuePayment, EB.BookingStatus 
		FROM 
			dbo.EntBookings AS EB INNER JOIN dbo.CustomerMaster ON EB.BookingByCustomer = dbo.CustomerMaster.CustomerCode LEFT OUTER JOIN  dbo.EntPayment AS EP ON EB.BookingNumber = EP.BookingNumber 
		WHERE
			EB.BookingStatus<>''5'' AND EP.PaymentDate BETWEEN @BookingDate1 AND @BookingDate2 and EB.BranchId=@BranchId AND EP.PaymentMade<>0 
		GROUP BY 
			EB.BookingNumber, EB.BookingDate, EB.NetAmount, EB.Qty, EB.BookingStatus, EB.BookingID, EB.BookingDeliveryDate, dbo.CustomerMaster.CustomerName, EB.TotalCost, EB.DiscountAmt
		ORDER BY EB.BookingID
	END
END









' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proc_ClothDelivery_InvoiceNo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <6-Feb-2012>
-- Description:	<Cloth Delivery Procedure>
-- =============================================
CREATE PROCEDURE [dbo].[Proc_ClothDelivery_InvoiceNo] 
@BookNo varchar(max)=null,
@Date DateTime='''',
@BranchId VARCHAR(MAX)=''''

AS
BEGIN

declare @BookingDate datetime
declare @BookingDate1 datetime
declare @CustomerName varchar(max)
declare @Custname varchar(max)
declare @TotalQty int 
declare @TotQty int
declare @BookingNumber varchar(max)
declare @InvoiceNo varchar(max)
declare @AlreadyDelivered int

declare @DelQty int
declare @DelQty1 int
declare @ClothDeliveryDate smalldatetime
declare @ClothDate smalldatetime
declare @BalQty int
DECLARE @TempBookingNo VARCHAR(MAX)
declare @Flag int
SET @TempBookingNo=''0''
set @Flag=1

create table #temp(BookingDate datetime,BookingNumber varchar(max),
CustomerName varchar(max),TotalQty int, AlreadyDelivered int, DelQty int,BalQty int,ClothDeliveryDate smalldatetime)

declare Delivery cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for
	SELECT
		dbo.EntBookings.BookingDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.Qty, dbo.CustomerMaster.CustomerName, SUM(dbo.BarcodeTable.DelQty) AS DelQty, dbo.BarcodeTable.ClothDeliveryDate
	FROM
		dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
	WHERE
		 dbo.BarcodeTable.StatusId<>''5'' AND (dbo.BarcodeTable.DelQty = 1 and dbo.BarcodeTable.ClothDeliveryDate<=@Date And  dbo.BarCodeTable.Bookingno=@BookNo) And  (dbo.BarCodeTable.BranchId=@BranchId)
	GROUP BY 
		dbo.BarcodeTable.ClothDeliveryDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty
	ORDER BY 
		dbo.EntBookings.BookingNumber

	Open Delivery
		fetch next from Delivery into @BookingDate,@BookingNumber,@TotalQty,@CustomerName,@DelQty,@ClothDeliveryDate
		while @@Fetch_Status=0
			Begin
				IF(@TempBookingNo<>@BookingNumber)
					BEGIN
						SET @AlreadyDelivered=''0''
						SELECT  
							@AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty)
						FROM
					        dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
						WHERE
							(dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.BarcodeTable.ClothDeliveryDate < @ClothDeliveryDate) And  (dbo.BarCodeTable.BranchId=@BranchId)
						GROUP BY 
							dbo.EntBookings.BookingNumber
						IF @AlreadyDelivered IS NULL
							SET @AlreadyDelivered=''0''
						SET @BalQty=@TotalQty-(@AlreadyDelivered+@DelQty)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,DelQty,ClothDeliveryDate,BalQty)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@DelQty,@ClothDeliveryDate,@BalQty)
						SET @TempBookingNo=@BookingNumber	
						SET @DelQty1=@DelQty					
					END
				ELSE
					BEGIN
						SELECT  
							@AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty)
						FROM
					        dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
						WHERE
							(dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.BarcodeTable.ClothDeliveryDate < @ClothDeliveryDate) And  (dbo.BarCodeTable.BranchId=@BranchId)
						GROUP BY 
							dbo.EntBookings.BookingNumber
						SET @BalQty=@TotalQty-(@AlreadyDelivered+@DelQty)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,TotalQty,AlreadyDelivered,DelQty,ClothDeliveryDate,BalQty)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered,@DelQty,@ClothDeliveryDate,@BalQty)
						SET @TempBookingNo=@BookingNumber	
						SET @DelQty1=@DelQty				
					END	
					
fetch next from Delivery into
@BookingDate,@BookingNumber,@TotalQty,@CustomerName,@DelQty,@ClothDeliveryDate
End
close Delivery
deallocate Delivery
select * from #temp
drop table #temp

END














' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proc_SalesPeriodReport_InvoiceNo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


-- =============================================
-- Author:		<karam chand sharma>
-- Create date: <20/01/2012>
-- Description:	<Balance Sales>
-- =============================================
CREATE PROCEDURE [dbo].[Proc_SalesPeriodReport_InvoiceNo]
@BookNo varchar(max)=null,
@Date DATETIME='''',
@BranchId Varchar(Max)=''''

AS
BEGIN
declare @BookingDate datetime
declare @BookingDate1 datetime
declare @CustomerName varchar(max)
declare @NetAmount float 
declare @BookingNumber varchar(max)
declare @AlreadyPaid float
declare @PaymentMade float
declare @PaymentDate smalldatetime
declare @BalAmt float
DECLARE @TempBookingNo VARCHAR(MAX)
SET @TempBookingNo=''0''

create table #temp(BookingDate datetime,BookingNumber varchar(max),
CustomerName varchar(max),NetAmount float, AlreadyPaid float, PaymentMade float,
PaymentDate smalldatetime,BalAmt float)

declare Sales cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for
	SELECT
		dbo.EntBookings.BookingDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.NetAmount, dbo.CustomerMaster.CustomerName, SUM(dbo.EntPayment.PaymentMade) AS PaymentMade, dbo.EntPayment.PaymentDate
	FROM
		dbo.EntPayment INNER JOIN  dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
	Where
		dbo.EntBookings.BookingStatus<>''5'' AND dbo.EntPayment.BookingNumber=@BookNo AND dbo.EntPayment.PaymentDate<=@Date And dbo.EntPayment.BranchId=@BranchId
	GROUP BY 
		dbo.EntPayment.PaymentDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.NetAmount
	ORDER BY 
		dbo.EntBookings.BookingNumber

	Open Sales
		fetch next from Sales into @BookingDate,@BookingNumber,@NetAmount,@CustomerName,@PaymentMade,@PaymentDate
	while @@Fetch_Status=0
			Begin
				IF(@TempBookingNo<>@BookingNumber)
					BEGIN
						
							SET @AlreadyPaid=''0''
						SET @BalAmt=@NetAmount-(@AlreadyPaid+@PaymentMade)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,NetAmount,AlreadyPaid,PaymentMade,PaymentDate,BalAmt)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@NetAmount,@AlreadyPaid,@PaymentMade,@PaymentDate,@BalAmt)
						SET @TempBookingNo=@BookingNumber	
										
					END
				ELSE
					BEGIN
						SELECT  
							@AlreadyPaid=SUM(dbo.EntPayment.PaymentMade)
						FROM
					        dbo.EntPayment INNER JOIN  dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode
						WHERE
							(dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.EntPayment.PaymentDate < @PaymentDate) And (dbo.EntPayment.BranchId=@BranchId)
						GROUP BY 
							dbo.EntBookings.BookingNumber
						SET @BalAmt=@NetAmount-(@AlreadyPaid+@PaymentMade)
						insert into #temp
							(BookingDate,BookingNumber,CustomerName,NetAmount,AlreadyPaid,PaymentMade,PaymentDate,BalAmt)
						values
							(@BookingDate,@BookingNumber,@CustomerName,@NetAmount,@AlreadyPaid,@PaymentMade,@PaymentDate,@BalAmt)
						SET @TempBookingNo=@BookingNumber
					End
fetch next from Sales into @BookingDate,@BookingNumber,@NetAmount,@CustomerName,@PaymentMade,@PaymentDate
End
close Sales
deallocate Sales
select * from #temp
drop table #temp	

END










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proc_SalesandDeliveryByInvoiceNo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'





-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <6-Feb-2012>
-- Description:	<Sales and Delivery by invoice no>
-- =============================================
CREATE PROCEDURE [dbo].[Proc_SalesandDeliveryByInvoiceNo]
	@InvoiceNo VARCHAR(MAX)='''',
	@Date DATETIME='''',
	@BranchId VARCHAR(MAX)
	
AS
BEGIN
DECLARE @TotalAmt VARCHAR(MAX),@BDAmt VARCHAR(MAX),@ST VARCHAR(MAX),@DelAmt VARCHAR(MAX),@FDate DATETIME,@FRDATE DATETIME,@UPDATE DATETIME, @BookingDate VARCHAR(MAX),@BookingNumber VARCHAR(MAX),@CustomerName VARCHAR(MAX),@TotalQty INT, @AlreadyDelivered INT, @DelQty INT,@ClothDeliveryDate VARCHAR(MAX),@BalQty VARCHAR(MAX),@NetAmount VARCHAR(MAX), @AlreadyPaid FLOAT, @PaymentMade VARCHAR(MAX),@BalAmt VARCHAR(MAX),@PaymentDate VARCHAR(MAX)
CREATE TABLE #SaleAndDelivery(BookingDate VARCHAR(MAX),BookingNumber VARCHAR(MAX),CustomerName VARCHAR(MAX),TotalQty VARCHAR(MAX), AlreadyDelivered VARCHAR(MAX), DelQty VARCHAR(MAX),BalQty VARCHAR(MAX),ClothDeliveryDate VARCHAR(MAX),NetAmount VARCHAR(MAX), AlreadyPaid VARCHAR(MAX), PaymentMade VARCHAR(MAX),BalAmt VARCHAR(MAX),PaymentDate VARCHAR(MAX),TotalAmt VARCHAR(MAX),BDAmt VARCHAR(MAX),ST VARCHAR(MAX),DelAmt VARCHAR(MAX) )

DECLARE BNumber CURSOR LOCAL FORWARD_Only DYNAMIC Read_Only FOR
	--Read unique booking number from delivery and sales 		
	SELECT dbo.EntBookings.BookingNumber, dbo.BarcodeTable.ClothDeliveryDate FROM dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.EntBookings.BranchId=@BranchId) AND (dbo.BarcodeTable.DelQty = 1) AND (dbo.BarcodeTable.ClothDeliveryDate <= @Date) AND (dbo.EntBookings.BookingNumber = @InvoiceNo) AND (dbo.EntBookings.BookingStatus <> ''5'')
	UNION
	SELECT dbo.EntBookings.BookingNumber, dbo.EntPayment.PaymentDate FROM dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.EntBookings.BranchId=@BranchId) AND (dbo.EntBookings.BookingNumber = @InvoiceNo) AND (dbo.EntPayment.PaymentDate <= @Date) AND (dbo.EntBookings.BookingStatus <> ''5'') ORDER BY dbo.EntBookings.BookingNumber	
	--Open Cursor	
	OPEN BNumber
		FETCH NEXT FROM BNumber INTO @BookingNumber,@FDate
		WHILE @@Fetch_Status=0
			BEGIN
				SET @FRDATE = @FDate
				SET @UPDATE = @Date	
				SET @AlreadyDelivered=''0''
				SET @DelQty=''0''
				SET @BalQty=''0''
				SET @TotalQty=''0''
				SELECT @NetAmount=NetAmount FROM dbo.EntBookings WHERE BookingNumber=@BookingNumber AND (dbo.EntBookings.BranchId=@BranchId)
				SELECT @TotalQty=dbo.EntBookings.Qty FROM dbo.EntBookings WHERE BookingNumber=@BookingNumber AND (dbo.EntBookings.BranchId=@BranchId) 
				SELECT @BookingDate=dbo.EntBookings.BookingDate, @CustomerName=dbo.CustomerMaster.CustomerName, @DelQty=SUM(dbo.BarcodeTable.DelQty) ,@ClothDeliveryDate=dbo.BarcodeTable.ClothDeliveryDate FROM dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE (dbo.BarcodeTable.DelQty = 1) and (dbo.BarcodeTable.ClothDeliveryDate = @FRDate) AND (dbo.BarCodeTable.Bookingno=@BookingNumber) AND (dbo.EntBookings.BranchId=@BranchId) GROUP BY dbo.BarcodeTable.ClothDeliveryDate, dbo.EntBookings.BookingNumber, dbo.EntBookings.BookingDate, dbo.CustomerMaster.CustomerName, dbo.EntBookings.Qty	ORDER BY dbo.EntBookings.BookingNumber
				SELECT @AlreadyDelivered=SUM(dbo.BarcodeTable.DelQty) FROM dbo.BarcodeTable INNER JOIN  dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode	WHERE (dbo.BarcodeTable.DelQty = 1) AND (dbo.EntBookings.BookingNumber = @BookingNumber) AND (dbo.BarcodeTable.ClothDeliveryDate < @FRDate) AND (dbo.EntBookings.BranchId=@BranchId) GROUP BY dbo.EntBookings.BookingNumber
				SELECT @TotalAmt=TotalCost, @BDAmt=DiscountAmt,@ST=(SELECT ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2)   FROM dbo.EntBookingDetails  WHERE      (BookingNumber = dbo.EntBookings.BookingNumber)  GROUP BY BookingNumber) FROM dbo.EntBookings WHERE (BookingStatus <> ''5'') AND  dbo.EntBookings.BookingNumber=@BookingNumber AND (dbo.EntBookings.BranchId=@BranchId) GROUP BY BookingNumber, BookingDate, NetAmount, TotalCost, DiscountAmt ORDER BY BookingNumber
				IF @TotalQty IS NULL
					SET @TotalQty=''0''						
				IF @AlreadyDelivered IS NULL
					SET @AlreadyDelivered=''0''
				IF @DelQty IS NULL 
					SET @DelQty=''0''
				SET @BalQty=@TotalQty-(@AlreadyDelivered+@DelQty)
				IF @BalQty IS NULL
					SET @BalQty=''0''
				--SALE REPORT
				SET @AlreadyPaid=(SELECT SUM(dbo.EntPayment.PaymentMade) FROM  dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber WHERE (dbo.EntBookings.BranchId=@BranchId) AND (dbo.EntPayment.PaymentDate < @FRDate) AND (dbo.EntBookings.BookingNumber = @BookingNumber)	)
				IF @AlreadyPaid IS NULL
					SET @AlreadyPaid=''0''
				SET @PaymentMade=(SELECT SUM(dbo.EntPayment.PaymentMade) FROM  dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber WHERE (dbo.EntBookings.BranchId=@BranchId) AND (dbo.EntPayment.PaymentDate = @FRDate) AND (dbo.EntBookings.BookingNumber = @BookingNumber))
				IF(@PaymentMade IS NULL)
					SET @PaymentMade=''0''
				SELECT  @DelAmt=SUM(DiscountOnPayment)  FROM dbo.EntPayment WHERE (BranchId=@BranchId) AND (BookingNumber = @BookingNumber) AND PaymentDate= @FRDate
				IF @DelAmt IS NULL
					SET @DelAmt=''0''

				SET @BalAmt=@NetAmount-(@AlreadyPaid+@PaymentMade)-@DelAmt
				IF(@ClothDeliveryDate IS NOT NULL) OR @PaymentMade<>''0''
					BEGIN	
						IF(convert(int, @DelQty) <> 0) AND convert(float, @PaymentMade) <> 0		
							BEGIN					
								SELECT @CustomerName=dbo.CustomerMaster.CustomerName, @TotalQty=dbo.EntBookings.Qty, @BookingDate=dbo.EntBookings.BookingDate FROM dbo.EntBookings INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE  dbo.EntBookings.BookingNumber=@BookingNumber
								INSERT INTO #SaleAndDelivery (BookingDate,BookingNumber,CustomerName,TotalQty, AlreadyDelivered, DelQty,BalQty,ClothDeliveryDate,NetAmount, AlreadyPaid, PaymentMade,BalAmt,PaymentDate,TotalAmt,BDAmt,ST,DelAmt) VALUES (CONVERT(VARCHAR,CONVERT(DATETIME,@BookingDate),106),@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered, @DelQty,@BalQty,CONVERT(VARCHAR,CONVERT(DATETIME,@ClothDeliveryDate),106),@NetAmount, @AlreadyPaid, @PaymentMade,@BalAmt,CONVERT(VARCHAR,CONVERT(DATETIME,@FRDate),106),Round(@TotalAmt,2),Round(@BDAmt,2),Round(@ST,2),Round(@DelAmt,2))
							END
							ELSE IF(convert(int, @DelQty) <> 0)	
							BEGIN					
								SELECT @CustomerName=dbo.CustomerMaster.CustomerName, @TotalQty=dbo.EntBookings.Qty, @BookingDate=dbo.EntBookings.BookingDate FROM dbo.EntBookings INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE  dbo.EntBookings.BookingNumber=@BookingNumber
								INSERT INTO #SaleAndDelivery (BookingDate,BookingNumber,CustomerName,TotalQty, AlreadyDelivered, DelQty,BalQty,ClothDeliveryDate,NetAmount, AlreadyPaid, PaymentMade,BalAmt,PaymentDate,TotalAmt,BDAmt,ST,DelAmt) VALUES (CONVERT(VARCHAR,CONVERT(DATETIME,@BookingDate),106),@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered, @DelQty,@BalQty,CONVERT(VARCHAR,CONVERT(DATETIME,@ClothDeliveryDate),106),@NetAmount, @AlreadyPaid, @PaymentMade,@BalAmt,CONVERT(VARCHAR,CONVERT(DATETIME,@FRDate),106),Round(@TotalAmt,2),Round(@BDAmt,2),Round(@ST,2),Round(@DelAmt,2))
							END
							ELSE IF (convert(float, @PaymentMade) <> 0)
							BEGIN					
								SELECT @CustomerName=dbo.CustomerMaster.CustomerName, @TotalQty=dbo.EntBookings.Qty, @BookingDate=dbo.EntBookings.BookingDate FROM dbo.EntBookings INNER JOIN dbo.CustomerMaster ON dbo.EntBookings.BookingByCustomer = dbo.CustomerMaster.CustomerCode WHERE  dbo.EntBookings.BookingNumber=@BookingNumber
								INSERT INTO #SaleAndDelivery (BookingDate,BookingNumber,CustomerName,TotalQty, AlreadyDelivered, DelQty,BalQty,ClothDeliveryDate,NetAmount, AlreadyPaid, PaymentMade,BalAmt,PaymentDate,TotalAmt,BDAmt,ST,DelAmt) VALUES (CONVERT(VARCHAR,CONVERT(DATETIME,@BookingDate),106),@BookingNumber,@CustomerName,@TotalQty,@AlreadyDelivered, @DelQty,@BalQty,CONVERT(VARCHAR,CONVERT(DATETIME,@ClothDeliveryDate),106),@NetAmount, @AlreadyPaid, @PaymentMade,@BalAmt,CONVERT(VARCHAR,CONVERT(DATETIME,@FRDate),106),Round(@TotalAmt,2),Round(@BDAmt,2),Round(@ST,2),Round(@DelAmt,2))
							END
					END				
				FETCH NEXT FROM BNumber INTO @BookingNumber,@FDATE
			END
	CLOSE BNumber
	DEALLOCATE BNumber
SELECT * FROM #SaleAndDelivery
DROP TABLE #SaleAndDelivery

END











' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_QuantityandBooking]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



/*
-- =============================================
-- Author:		<Karam>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport ''10/19/2010'',''10/22/2010''
EXEC Sp_Sel_QuantityandBooking ''1 Dec 2010'',''31 Dec 2010''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_QuantityandBooking]
	(
		@BookDate1 datetime,
		@BookDate2 datetime,
		@Flag VARCHAR(MAX)='''',
		@CustId VARCHAR(MAX)='''',
		@BookingNo VARCHAR(MAX)='''',
		@BranchId Varchar(Max)=''''
	)
AS
BEGIN
	IF(@Flag=1)
		BEGIN
			SELECT  
				EB.BookingID, CONVERT(varchar, EB.BookingDate, 106) AS BookingDate, EB.BookingNumber, dbo.CustomerMaster.CustomerName,CONVERT(varchar, EB.BookingDeliveryDate, 106) AS DeliveryDate, EB.Qty, EB.TotalCost,EB.DiscountAmt AS DiscountOnPayment,(SELECT ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) AS ST FROM dbo.EntBookingDetails WHERE (BookingNumber = EB.BookingNumber)GROUP BY BookingNumber) AS ST,(EB.TotalCost -  EB.DiscountAmt) +(SELECT     ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) AS ST FROM dbo.EntBookingDetails AS EntBookingDetails_3 WHERE (BookingNumber = EB.BookingNumber) GROUP BY BookingNumber) AS NetAmount, SUM(COALESCE (EP.PaymentMade, 0)) AS PaymentMade, (EB.TotalCost -  EB.DiscountAmt) + (SELECT     ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) AS ST FROM dbo.EntBookingDetails AS EntBookingDetails_2 WHERE (BookingNumber = EB.BookingNumber) GROUP BY BookingNumber) - SUM(COALESCE (EP.PaymentMade, 0)) AS DuePayment, EB.BookingStatus 
			FROM
				dbo.EntBookings AS EB INNER JOIN dbo.CustomerMaster ON EB.BookingByCustomer = dbo.CustomerMaster.CustomerCode LEFT OUTER JOIN  dbo.EntPayment AS EP ON EB.BookingNumber = EP.BookingNumber 
			WHERE
				EB.BookingStatus<>''5'' AND EB.BookingDate BETWEEN @BookDate1 AND @BookDate2 and EB.BranchId=@BranchId
			GROUP BY 
				EB.BookingNumber, EB.BookingDate, EB.NetAmount, EB.Qty, EB.BookingStatus, EB.BookingID, EB.BookingDeliveryDate, dbo.CustomerMaster.CustomerName, EB.TotalCost, EB.DiscountAmt
			ORDER BY EB.BookingID
--
--			SELECT    EB.BookingID, EB.BookingNumber, CONVERT(varchar, EB.BookingDate, 106) AS BookingDate, EB.Qty, EB.NetAmount, 
--                      SUM(COALESCE (EP.PaymentMade, 0)) AS PaymentMade, SUM(COALESCE (EP.DiscountOnPayment, 0)) AS DiscountOnPayment, 
--                      EB.NetAmount - SUM(COALESCE (EP.PaymentMade, 0) + COALESCE (EP.DiscountOnPayment, 0)) AS DuePayment, EB.BookingStatus, CONVERT(varchar, 
--                      EB.BookingDeliveryDate, 106) AS DeliveryDate, dbo.CustomerMaster.CustomerName
--			FROM         dbo.EntBookings AS EB INNER JOIN
--                      dbo.CustomerMaster ON EB.BookingByCustomer = dbo.CustomerMaster.CustomerCode LEFT OUTER JOIN
--                      dbo.EntPayment AS EP ON EB.BookingNumber = EP.BookingNumber
--			WHERE EB.BookingStatus<>''5'' AND EB.BookingDate BETWEEN @BookDate1 AND @BookDate2 and EB.BranchId=@BranchId
--			GROUP BY EB.BookingNumber, EB.BookingDate, EB.NetAmount, EB.Qty, EB.BookingStatus, EB.BookingID, EB.BookingDeliveryDate, 
--                      dbo.CustomerMaster.CustomerName
--			ORDER BY EB.BookingID	
		END
	IF(@Flag=2)
		BEGIN
			SELECT  
				EB.BookingID, CONVERT(varchar, EB.BookingDate, 106) AS BookingDate, EB.BookingNumber, dbo.CustomerMaster.CustomerName,CONVERT(varchar, EB.BookingDeliveryDate, 106) AS DeliveryDate, EB.Qty, EB.TotalCost,EB.DiscountAmt AS DiscountOnPayment,(SELECT ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) AS ST FROM dbo.EntBookingDetails WHERE (BookingNumber = EB.BookingNumber)GROUP BY BookingNumber) AS ST,(EB.TotalCost -  EB.DiscountAmt) +(SELECT     ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) AS ST FROM dbo.EntBookingDetails AS EntBookingDetails_3 WHERE (BookingNumber = EB.BookingNumber) GROUP BY BookingNumber) AS NetAmount, SUM(COALESCE (EP.PaymentMade, 0)) AS PaymentMade, (EB.TotalCost -  EB.DiscountAmt) + (SELECT     ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) AS ST FROM dbo.EntBookingDetails AS EntBookingDetails_2 WHERE (BookingNumber = EB.BookingNumber) GROUP BY BookingNumber) - SUM(COALESCE (EP.PaymentMade, 0)) AS DuePayment, EB.BookingStatus 
			FROM
				dbo.EntBookings AS EB INNER JOIN dbo.CustomerMaster ON EB.BookingByCustomer = dbo.CustomerMaster.CustomerCode LEFT OUTER JOIN  dbo.EntPayment AS EP ON EB.BookingNumber = EP.BookingNumber 
			WHERE 
				EB.BookingStatus<>''5'' AND EB.BookingDate BETWEEN @BookDate1 AND @BookDate2 AND   EB.BookingByCustomer=@CustId and EB.BranchId=@BranchId
			GROUP BY 
				EB.BookingNumber, EB.BookingDate, EB.NetAmount, EB.Qty, EB.BookingStatus, EB.BookingID, EB.BookingDeliveryDate, dbo.CustomerMaster.CustomerName, EB.TotalCost, EB.DiscountAmt
			ORDER BY EB.BookingID
		END
	IF(@Flag=3)
		BEGIN
			SELECT  
				EB.BookingID, CONVERT(varchar, EB.BookingDate, 106) AS BookingDate, EB.BookingNumber, dbo.CustomerMaster.CustomerName,CONVERT(varchar, EB.BookingDeliveryDate, 106) AS DeliveryDate, EB.Qty, EB.TotalCost,EB.DiscountAmt AS DiscountOnPayment,(SELECT ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) AS ST FROM dbo.EntBookingDetails WHERE (BookingNumber = EB.BookingNumber)GROUP BY BookingNumber) AS ST,(EB.TotalCost -  EB.DiscountAmt) +(SELECT     ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) AS ST FROM dbo.EntBookingDetails AS EntBookingDetails_3 WHERE (BookingNumber = EB.BookingNumber) GROUP BY BookingNumber) AS NetAmount, SUM(COALESCE (EP.PaymentMade, 0)) AS PaymentMade, (EB.TotalCost -  EB.DiscountAmt) + (SELECT     ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) AS ST FROM dbo.EntBookingDetails AS EntBookingDetails_2 WHERE (BookingNumber = EB.BookingNumber) GROUP BY BookingNumber) - SUM(COALESCE (EP.PaymentMade, 0)) AS DuePayment, EB.BookingStatus 
			FROM
				dbo.EntBookings AS EB INNER JOIN dbo.CustomerMaster ON EB.BookingByCustomer = dbo.CustomerMaster.CustomerCode LEFT OUTER JOIN  dbo.EntPayment AS EP ON EB.BookingNumber = EP.BookingNumber 
			WHERE
				EB.BookingStatus<>''5'' AND EB.BookingNumber=@BookingNo and EB.BranchId=@BranchId
			GROUP BY 
				EB.BookingNumber, EB.BookingDate, EB.NetAmount, EB.Qty, EB.BookingStatus, EB.BookingID, EB.BookingDeliveryDate, dbo.CustomerMaster.CustomerName, EB.TotalCost, EB.DiscountAmt
			ORDER BY EB.BookingID
--
--			SELECT
--				EB.BookingID,CONVERT(varchar, EB.BookingDate, 106) AS BookingDate,EB.BookingNumber, dbo.CustomerMaster.CustomerName, CONVERT(varchar,EB.BookingDeliveryDate, 106) AS DeliveryDate,EB.Qty,EB.TotalCost,( SUM(COALESCE (EP.DiscountOnPayment, 0)) + EB.DiscountAmt) AS DiscountOnPayment, ROUND(dbo.EntBookingDetails.STPAmt + dbo.EntBookingDetails.STEP1Amt + dbo.EntBookingDetails.STEP2Amt,2) AS ST,((EB.TotalCost-(SUM(COALESCE (EP.DiscountOnPayment, 0)) + EB.DiscountAmt))+ROUND(dbo.EntBookingDetails.STPAmt + dbo.EntBookingDetails.STEP1Amt + dbo.EntBookingDetails.STEP2Amt,2)) AS NetAmount,SUM(COALESCE (EP.PaymentMade, 0)) AS PaymentMade,(((EB.TotalCost-(SUM(COALESCE (EP.DiscountOnPayment, 0)) + EB.DiscountAmt))+ROUND(dbo.EntBookingDetails.STPAmt + dbo.EntBookingDetails.STEP1Amt + dbo.EntBookingDetails.STEP2Amt,2))-  SUM(COALESCE (EP.PaymentMade, 0))) DuePayment, EB.BookingStatus  
--			FROM
--				dbo.EntBookings AS EB INNER JOIN dbo.CustomerMaster ON EB.BookingByCustomer = dbo.CustomerMaster.CustomerCode INNER JOIN dbo.EntBookingDetails ON EB.BookingNumber = dbo.EntBookingDetails.BookingNumber LEFT OUTER JOIN dbo.EntPayment AS EP ON EB.BookingNumber = EP.BookingNumber
--			WHERE EB.BookingStatus<>''5'' AND EB.BookingNumber=@BookingNo and EB.BranchId=@BranchId
--			GROUP BY 
--				EB.BookingNumber, EB.BookingDate, EB.NetAmount, EB.Qty, EB.BookingStatus, EB.BookingID, EB.BookingDeliveryDate, dbo.CustomerMaster.CustomerName, EB.TotalCost, EB.DiscountAmt, dbo.EntBookingDetails.STPAmt, dbo.EntBookingDetails.STEP1Amt, dbo.EntBookingDetails.STEP2Amt
--			ORDER BY 
--				EB.BookingID
		END
END















' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Mearge_DataWithExistigBooking]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
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
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EditBooking_SaveProc]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <21-Nov-2011>
-- Description:	<EDIT DATA IN THE NEW BOOKING>
-- =============================================
CREATE PROCEDURE [dbo].[sp_EditBooking_SaveProc]	
	@BookingByCustomer VARCHAR(MAX)='''',
    @BOOKINGNUMBER VARCHAR(MAX)='''',
    @IsUrgent VARCHAR(MAX)='''',
    @BookingDeliveryDate VARCHAR(MAX)='''',
    @BookingDeliveryTime VARCHAR(MAX)='''',
    @TotalCost VARCHAR(MAX)='''',
    @Discount VARCHAR(MAX)='''',
    @NetAmount VARCHAR(MAX)='''',
    @BookingRemarks VARCHAR(MAX)='''',
    @ItemTotalQuantity VARCHAR(MAX)='''',
    @HomeDelivery VARCHAR(MAX)='''',
    @CheckedByEmployee VARCHAR(MAX)='''',
	@DiscountAmt VARCHAR(MAX)='''',
	@DiscountOption VARCHAR(MAX)='''',
	@BranchId VARCHAR(MAX)=''''
AS
BEGIN		
	
	DECLARE @fltPreviousCustomerAmount FLOAT,@CustCode VARCHAR(MAX),@OldTotalAmount FLOAT
	SELECT @OldTotalAmount= NetAmount FROM dbo.EntBookings WHERE BookingNumber=@BOOKINGNUMBER AND BranchId=@BranchId
	SET @fltPreviousCustomerAmount=(SELECT COALESCE(SUM(PaymentMade),0) FROM EntPayment WHERE BookingNumber=@BOOKINGNUMBER AND BranchId=@BranchId)
	DELETE FROM EntPayment WHERE BookingNumber =@BOOKINGNUMBER AND BranchId=@BranchId
	DELETE FROM EntLedgerEntries WHERE BranchId=@BranchId AND Narration Like ''%'' +@BOOKINGNUMBER+ ''%''
	IF(@fltPreviousCustomerAmount>0)
		BEGIN
			UPDATE LedgerMaster SET CurrentBalance=(CurrentBalance-@fltPreviousCustomerAmount) WHERE LedgerName=''Cash'' AND BranchId=@BranchId
			UPDATE LedgerMaster SET CurrentBalance=(CurrentBalance+@fltPreviousCustomerAmount) WHERE LedgerName=@BookingByCustomer AND BranchId=@BranchId
		END
	UPDATE LedgerMaster SET CurrentBalance=(CurrentBalance-@OldTotalAmount) Where LedgerName=@BookingByCustomer AND BranchId=@BranchId
	UPDATE LedgerMaster SET CurrentBalance=(CurrentBalance+@OldTotalAmount) Where LedgerName=''Sales'' AND BranchId=@BranchId
	UPDATE EntBookings SET BookingDeliveryDate=@BookingDeliveryDate, BookingDeliveryTime=@BookingDeliveryTime, BookingByCustomer=@BookingByCustomer, ISUrgent=@IsUrgent, TotalCost=@TotalCost, Discount=@Discount, NetAmount=@NetAmount, BookingRemarks=@BookingRemarks,HomeDelivery=@HomeDelivery,CheckedByEmployee=@CheckedByEmployee,DiscountAmt=@DiscountAmt,DiscountOption=@DiscountOption WHERE BookingNumber=@BOOKINGNUMBER AND BranchId=@BranchId
	DELETE FROM EntBookingDetails WHERE BookingNumber=@BOOKINGNUMBER AND BranchId=@BranchId
	DELETE FROM BarcodeTable WHERE BookingNo=@BOOKINGNUMBER AND BranchId=@BranchId
	DELETE FROM EntChallan WHERE BookingNumber=@BOOKINGNUMBER AND BranchId=@BranchId
	INSERT INTO EntPayment (BookingNumber,PaymentDate,PaymentMade,DiscountOnPayment,BranchId) VALUES (@BOOKINGNUMBER,GETDATE(),0,0,@BranchId)			
	SELECT @BOOKINGNUMBER AS BookingNumber
END



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DetailCashReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


-- =============================================
-- Author:		<Sanjeev Arora>
-- Create date: <01 March 2012>
-- Description:	<Detail Cash Book>
-- =============================================
CREATE PROCEDURE [dbo].[sp_DetailCashReport]
@FromDate varchar(max)=null,
@ToDate varchar(max)=null,	
@BranchId Varchar(max)=null
AS
BEGIN
declare @Transdate varchar(max), @LedgerName varchar(max),@OpBal decimal(18,2),@Debit decimal(18,2),@Credit decimal(18,2),@ClBal decimal(18,2),@rowid varchar(max),@Bal decimal(18,2),@PBal decimal(18,2),@Particular varchar(max),@Desc varchar(max)

Create Table #TmpTable
(
Id int,
Transdate datetime,
LedgerName varchar(max),
Particular varchar(max),
OpeningBalance decimal(18,2),
Debit decimal(18,2),
Credit decimal(18,2),
ClosingBalance decimal(18,2)
)
  
Declare CashBook Cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for
	SELECT  TransDate,LedgerName,OpeningBalance,
	Debit,Credit,ClosingBalance, ROW_NUMBER() OVER (ORDER BY TransDate) AS rn,
    SUBSTRING(Narration,charindex(''booking number'',Narration),22),Particulars
	FROM    DrySoftBranch.dbo.EntLedgerEntries where LedgerName=''Cash'' and BranchId=@BranchId
		Open CashBook
			Fetch Next 
				From CashBook into @Transdate,@LedgerName,
				@OpBal,@Debit,@Credit,@ClBal,@rowid,@Particular,@Desc
					While @@Fetch_status=0
						Begin
							If @rowid=1
							Begin
								
							set @Bal=(@OpBal+@Debit)-@Credit
							Insert into #TmpTable(Id,TransDate,LedgerName,Particular,
							OpeningBalance,Debit,Credit,ClosingBalance)
							values (@rowid,@Transdate,@LedgerName,replace(@Particular,''booking number'',''Recd. for Invoice# ''),0,@Debit,@Credit,@Bal)
							End
							
							if @rowid>1
							Begin
							if @Particular <> ''''
							Begin
								
							Select @PBal=ClosingBalance from #tmpTable where Id<@rowid
							set @Bal=(@PBal+@Debit)-@Credit
							
							Insert into #TmpTable(Id,TransDate,LedgerName,Particular,
							OpeningBalance,Debit,Credit,ClosingBalance)
							values (@rowid,@Transdate,@LedgerName,replace(@Particular,''booking number'',''Recd. for Invoice# ''),@PBal,@Debit,@Credit,@Bal)
							End
								Else
									Begin
									Select @PBal=ClosingBalance from #tmpTable where Id<@rowid
									set @Bal=(@PBal+@Debit)-@Credit
							
									Insert into #TmpTable(Id,TransDate,LedgerName,Particular,
									OpeningBalance,Debit,Credit,ClosingBalance)
									values (@rowid,@Transdate,@LedgerName,''Paid ''+@Desc,@PBal,@Debit,@Credit,@Bal)
									End
							End
				Fetch Next 
					From CashBook into @Transdate,@LedgerName,
					@OpBal,@Debit,@Credit,@ClBal,@rowid,@Particular,@Desc
						End	
		Close CashBook
		DEALLOCATE CashBook

Select * from #tmpTable where TransDate between @FromDate And @ToDate
drop table #tmpTable

END



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_CashBookReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		<Sanjeev Arora>
-- Create date: <29/02/2012>
-- Description:	<Cash Book >
-- =============================================
CREATE PROCEDURE [dbo].[Sp_CashBookReport]
@FromDate varchar(max)=null,
@ToDate varchar(max)=null,
@BranchId varchar(max)=null
	
AS
BEGIN
declare @Transdate varchar(max),@LedgerName varchar(max),@OpBal decimal(18,2),@Debit decimal(18,2),@Credit decimal(18,2),@ClBal decimal(18,2),@rowid varchar(max),@Bal decimal(18,2),@PBal decimal(18,2),@O VARCHAR(MAX),@C VARCHAR(MAX),@De varchar(max),@Ce varchar(max)
Create Table #TmpTable
(
Id int,
Transdate datetime,
LedgerName varchar(max),
OpeningBalance decimal(18,2),
Debit decimal(18,2),
Credit decimal(18,2),
ClosingBalance decimal(18,2)
)
Create Table #TmpTable1
(
Id int,
Transdate datetime,
LedgerName varchar(max),
OpeningBalance varchar(max),
Debit decimal(18,2),
Credit decimal(18,2),
ClosingBalance decimal(18,2)
) 

Declare CashBook Cursor LOCAL FORWARD_ONLY STATIC READ_ONLY for
	SELECT  TransDate,LedgerName,OpeningBalance,
	Debit,Credit,ClosingBalance, ROW_NUMBER() OVER (ORDER BY TransDate) AS rn
    FROM    DrySoftBranch.dbo.EntLedgerEntries where LedgerName=''Cash'' And BranchId=@BranchId
		Open CashBook
			Fetch Next 
				From CashBook into @Transdate,@LedgerName,
				@OpBal,@Debit,@Credit,@ClBal,@rowid
					While @@Fetch_status=0
						Begin
							If @rowid=1
							Begin
							set @Bal=(@OpBal+@Debit)-@Credit
							Insert into #TmpTable(Id,TransDate,LedgerName,
							OpeningBalance,Debit,Credit,ClosingBalance)
							values (@rowid,@Transdate,@LedgerName,0,@Debit,@Credit,@Bal)
							End
							
							if @rowid>1
							Begin
							
							Select @PBal=ClosingBalance from #tmpTable where Id<@rowid
							set @Bal=(@PBal+@Debit)-@Credit
							
							Insert into #TmpTable(Id,TransDate,LedgerName,
							OpeningBalance,Debit,Credit,ClosingBalance)
							values (@rowid,@Transdate,@LedgerName,@PBal,@Debit,@Credit,@Bal)
							End
				Fetch Next 
					From CashBook into @Transdate,@LedgerName,
					@OpBal,@Debit,@Credit,@ClBal,@rowid
						End	
		Close CashBook
		DEALLOCATE CashBook

declare CompCashBook cursor for
SELECT Transdate,LedgerName,(select Top 1 openingbalance from #TmpTable where TransDate between @FromDate and @ToDate) as ''Op.Bal'',
Sum(Debit),Sum(credit),(select Top 1 ClosingBalance from #TmpTable where TransDate between @FromDate and @ToDate  order by Transdate desc) as ''Cl.Bal'',
 ROW_NUMBER() OVER (order by Transdate) AS ''RN'' FROM #tmptable where Transdate between @FromDate and @ToDate
group by Transdate,LedgerName

Open CompCashBook
			Fetch Next 
				From CompCashBook into @Transdate,@LedgerName,
				@OpBal,@Debit,@Credit,@ClBal,@rowid
					While @@Fetch_status=0
						Begin
								if @rowid=1				
							Begin
							set @Bal=(@OpBal+@Debit)-@Credit
							
							Insert into #TmpTable1(id,TransDate,LedgerName,
							OpeningBalance,Debit,Credit,ClosingBalance)
							values (@rowid,@Transdate,@LedgerName,@OpBal,@Debit,@Credit,@Bal)
							End
							if @rowid>1
							Begin
							Select @PBal=ClosingBalance from #tmpTable where Transdate<@Transdate
							set @Bal=(@PBal+@Debit)-@Credit
							
							Insert into #TmpTable1(id,TransDate,LedgerName,
							OpeningBalance,Debit,Credit,ClosingBalance)
							values (@rowid,@Transdate,@LedgerName,@PBal,@Debit,@Credit,@Bal)
							End
				Fetch Next 
					From CompCashBook into @Transdate,@LedgerName,
					@OpBal,@Debit,@Credit,@ClBal,@rowid
						End	
		Close CompCashBook
		DEALLOCATE CompCashBook




SELECT  Convert(varchar,Transdate,106) as Transdate,LedgerName,OpeningBalance,Debit,Credit,ClosingBalance FROM #tmptable1


drop table #tmpTable
drop table #tmpTable1
	


END






' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Dry_EmployeeMaster]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'







CREATE PROCEDURE [dbo].[sp_Dry_EmployeeMaster]		
	@Flag VARCHAR(MAX)='''',
	@ID VARCHAR(MAX)='''',
	@EmployeeCode VARCHAR(MAX)='''',
	@EmployeeSalutation VARCHAR(MAX)='''',
	@EmployeeName VARCHAR(MAX)='''',
	@EmployeeAddress  VARCHAR(MAX)='''',
	@EmployeePhone VARCHAR(MAX)='''',
	@EmployeeMobile VARCHAR(MAX)='''',
	@EmployeeEmailId VARCHAR(MAX)='''',
	@Status VARCHAR(MAX)='''',
	@BookingNumber VARCHAR(MAX)='''',
	@Remarks VARCHAR(MAX)='''',
	@CustomerName  varchar(max)='''',
	@BranchId VARCHAR(MAX)='''',
	@MaxId VARCHAR(MAX)='''',
	@BookingDate DATETIME='''',
	@DeliveryDate DATETIME='''',
	@QueryType 	VARCHAR(MAX)=''''
AS
BEGIN
	IF(@Flag=1)
		BEGIN
			SET @MaxId=(Select Max(ID) From EmployeeMaster where BranchId=@BranchId)
			IF @MaxId IS NULL
				SET @MaxId=0
			SET @EmployeeCode= ''Emp''+CONVERT(VARCHAR,(CONVERT(INT,@MaxId)+1))
			INSERT INTO dbo.EmployeeMaster
				(ID,EmployeeCode,EmployeeSalutation,EmployeeName,EmployeeAddress,EmployeePhone,EmployeeMobile,EmployeeEmailId,BranchId)
			VALUES
				((CONVERT(INT,@MaxId)+1),@EmployeeCode,@EmployeeSalutation,@EmployeeName,@EmployeeAddress,@EmployeePhone,@EmployeeMobile,@EmployeeEmailId,@BranchId)
		END	
	ELSE IF (@Flag=2)
		BEGIN
			UPDATE EmployeeMaster Set EmployeeSalutation=@EmployeeSalutation,EmployeeName=@EmployeeName,EmployeeAddress=@EmployeeAddress,EmployeePhone=@EmployeePhone,EmployeeMobile=@EmployeeMobile,EmployeeEmailId=@EmployeeEmailId WHERE  EmployeeCode=@EmployeeCode AND BranchId=@BranchId			
		END
	ELSE IF (@Flag=3)
		BEGIN
			SELECT [ID], [EmployeeCode], COALESCE(EmployeeSalutation,'''') + '' '' + [EmployeeName] As EmployeeName, [EmployeeAddress], [EmployeePhone], [EmployeeMobile], [EmployeeEmailId] FROM [EmployeeMaster] WHERE BranchId=@BranchId	Order By ID DESC
		END	
	ELSE IF (@Flag=4)
		BEGIN
			SELECT [ID], [EmployeeCode], COALESCE(EmployeeSalutation,'''') + '' '' + [EmployeeName] As EmployeeName, [EmployeeAddress], [EmployeePhone], [EmployeeMobile], [EmployeeEmailId] FROM [EmployeeMaster] Where BranchId=@BranchId AND	 (EmployeeName Like @Status) OR (EmployeeAddress Like @Status) OR (EmployeePhone Like @Status) OR (EmployeeMobile Like @Status) Order By ID DESC
		END	
	ELSE IF (@Flag=5)
		BEGIN
			DELETE FROM dbo.EmployeeMaster WHERE EmployeeCode=@EmployeeCode AND BranchId=@BranchId	
		END	
	ELSE IF (@Flag=6)
		BEGIN
			SELECT EmployeeCode,EmployeeName FROM dbo.EmployeeMaster WHERE BranchId=@BranchId
		END	
	ELSE IF (@Flag=7)
		BEGIN
			SELECT SUM(STPAmt) + SUM(Step1Amt) + SUM(Step2Amt) AS Servicetax FROM EntBookingDetails WHERE BookingNumber=@BookingNumber and BranchId=@BranchId
		END
	ELSE IF (@Flag=8)
		BEGIN
			SELECT DeliveryMsg FROM dbo.EntPayment WHERE BookingNumber=@BookingNumber and BranchId=@BranchId
		END	
	ELSE IF (@Flag=9)
		BEGIN
			UPDATE dbo.EntPayment SET DeliveryMsg=@Remarks WHERE BookingNumber=@BookingNumber and BranchId=@BranchId
		END	
	ELSE IF (@Flag=10)
		BEGIN
			SELECT ''Cash'' AS PaymentType
			UNION
			SELECT ''Credit Card'' AS PaymentType
			UNION
			SELECT ''Debit Card'' AS PaymentType
			UNION
			SELECT ''Cheque/Bank'' AS PaymentType
		END
	ELSE IF (@Flag=11)
		BEGIN
			SELECT BookingStatus FROM EntBookings where BookingNumber=@BookingNumber and BranchId=@BranchId
		END		
	ELSE IF (@Flag=14)
		BEGIN
			SELECT Row_number() over  (order by Item) as SNo, BookingNo,Item FROM barcodetable WHERE BookingNo=@BookingNumber AND BranchId=@BranchId	
		END	
	ELSE IF (@Flag=12)	
		BEGIN
			SELECT  EmployeeSalutation, EmployeeName, EmployeeAddress, EmployeePhone, EmployeeMobile, EmployeeEmailId, EmployeeCode FROM  dbo.EmployeeMaster WHERE EmployeeCode=@EmployeeCode AND BranchId=@BranchId	
		END
	ELSE IF (@Flag=15)
		BEGIN
		IF(@QueryType=''TRUE'')
			BEGIN			
				SET @BookingDate=(SELECT CONVERT(VARCHAR,BookingDate,106) FROM ENTBOOKINGS WHERE bookingnumber=@BookingNumber AND BranchId=@BranchId)						
				IF(@BookingDate <= @DeliveryDate)
				BEGIN
				SET @Status=''TRUE''			
				END
				ELSE
				BEGIN
				SET @Status=''FALSE''
				END				
				SELECT @Status AS Status
			END
		ELSE
			BEGIN
					SET @BookingDate=(SELECT CONVERT(VARCHAR,BookingDate,106) FROM ENTBOOKINGS WHERE bookingnumber=@BookingNumber AND BranchId=@BranchId)						
					IF(@BookingDate <= @DeliveryDate)
					BEGIN
					SET @Status=''TRUE''			
					END
					ELSE
					BEGIN
					SET @Status=''FALSE''
					END	
					IF(@Status=''TRUE'')
					BEGIN					
					SET @BookingDate=''''				
					SET @Status=''''
					SET @BookingDate=(SELECT  MAX(PaymentDate) FROM ENTPAYMENT WHERE BranchId=@BranchId)		
					IF(@BookingDate <= @DeliveryDate)
					BEGIN
					SET @Status=''TRUE''			
					END
					ELSE
					BEGIN
					SET @Status=''FALSE''
					END
			END
				ELSE
					BEGIN
					SET @Status=''FALSE''
					END			
				SELECT @Status AS Status				
			END
		END			
END








' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_EmployeeCheckedByReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport ''10/19/2010'',''10/22/2010''
EXEC Sp_Sel_QuantityandBooking ''1 Dec 2010'',''31 Dec 2010''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_EmployeeCheckedByReport]
	(
		@BookingNo varchar(max)='''',
		@BranchId Varchar(max)=''''
	)
AS
BEGIN
	SELECT     dbo.BarcodeTable.BookingNo, dbo.EmployeeMaster.EmployeeName, dbo.BarcodeTable.Item
	FROM         dbo.BarcodeTable INNER JOIN
                      dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber INNER JOIN
                      dbo.EmployeeMaster ON dbo.EntBookings.CheckedByEmployee = dbo.EmployeeMaster.EmployeeCode
	WHERE     (dbo.BarcodeTable.BookingNo = @BookingNo) and (dbo.BarcodeTable.BranchId=@BranchId)
	order by dbo.BarcodeTable.RowIndex
END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_RecleanReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_RecleanReport ''1 jul 2010'',''1 aug 2010''
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
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Report_ChallanReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'





/*
-- =============================================
-- Author:		<Manoj Gupta,Karam Chand sharma>
-- Create date: <10 Oct 2011,6 Jan 2012>
-- Description:	<To select data for serviceTax report>
-- =============================================
EXEC Sp_Report_VendorReport ''1/Sep/2011'',''30/Sep/2011'','''',''''
*/
CREATE PROCEDURE [dbo].[Sp_Report_ChallanReport]
	(
		@ChallanDate1 datetime='''',
		@ChallanDate2	datetime='''',
		@ChallanNumber varchar(max)= '''',
		@Flag varchar(max)='''',
		@BranchId Varchar(max)=''''
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
                      dbo.BarcodeTable.Item as ItemName, CASE WHEN barcodetable.Process = ''NONE'' THEN '''' ELSE barcodetable.Process END AS ProcessType, 
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
									  dbo.BarcodeTable.Item as ItemName, CASE WHEN barcodetable.Process = ''NONE'' THEN '''' ELSE barcodetable.Process END AS ProcessType, 
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
				SELECT DISTINCT(ChallanNumber),CONVERT(VARCHAR,ChallanDate,106) AS ChallanDate,(SELECT ChallanType FROM mstReceiptConfig) AS ChallanType FROM EntChallan
				WHERE ChallanDate BETWEEN @ChallanDate1 AND @ChallanDate2 AND BranchId=@BranchId
		END
ELSE IF(@Flag=4)
		BEGIN
				SELECT DISTINCT(ChallanNumber),CONVERT(VARCHAR,ChallanDate,106) AS ChallanDate,(SELECT ChallanType FROM mstReceiptConfig) AS ChallanType FROM EntChallan
				WHERE ChallanNumber=@ChallanNumber AND BranchId=@BranchId
		END
		ELSE IF(@Flag=5)
		BEGIN
			SELECT
				CONVERT(varchar, dbo.BarcodeTable.BookingDate, 106) AS BookingDate, dbo.EntChallan.ChallanNumber, dbo.EntChallan.BookingNumber, 
                dbo.BarcodeTable.Item AS ItemName, CASE WHEN barcodetable.Process = ''NONE'' THEN '''' ELSE barcodetable.Process END AS ProcessType, 
                dbo.BarcodeTable.ItemExtraprocessType AS ExtraProcessType,dbo.BarcodeTable.ItemExtraprocessType2 AS ExtraProcessType2, dbo.BarcodeTable.SNo AS TotalQty, CONVERT(varchar, dbo.EntChallan.ChallanDate, 
                106) AS ChallanDate, dbo.EntChallan.ChallanSendingShift, dbo.BarcodeTable.ItemRemarks, CONVERT(varchar, dbo.BarcodeTable.DueDate, 106) 
                AS DueDate, dbo.EntBookings.IsUrgent, dbo.EntBookings.HomeDelivery,dbo.BarcodeTable.Colour
			FROM 
				dbo.EntChallan INNER JOIN
                dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND 
                dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex INNER JOIN
                dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber
			WHERE
				(dbo.EntChallan.ChallanNumber = @ChallanNumber)
			Order BY BookingNumber
		END
	END





' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_NewDeliveryReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


/*
-- =============================================
-- Author:		<Manoj Gupta>
-- Create date: <5 Sep 2011>
-- Description:	<To select Records for delivery report>
-- =============================================
EXEC Sp_Sel_BookingReport ''1 SEP 2010'',''2 sep 2010''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_NewDeliveryReport]
	(
		@BookingDate1 datetime='''',
		@BookingDate2 datetime='''',
		@Flag varchar(50)='''',
		@BookingNo VARCHAR(MAX)='''',
		@BranchId Varchar(MAX)=''''
	)
AS
BEGIN 
	IF(@Flag=1)
		BEGIN
			---- Select query 
			SELECT CONVERT(VARCHAR,BookingDate,106) AS BookingDate, BookingNo, SUM(SNo) AS QTY , SUM(DelQty) AS DeliveredQty , SUM(SNO) - SUM(DelQty) AS BalanceQty  FROM BarcodeTable WHERE BarcodeTable.StatusId<>''5'' AND BookingDate BETWEEN @BookingDate1 AND @BookingDate2 AND BranchId=@BranchId GROUP BY BookingDate, BookingNo

		END	
	ELSE IF(@Flag=2)
		BEGIN
			---- Hyperlink query 
			SELECT BookingNo,CONVERT(VARCHAR,ClothDeliveryDate,106) AS DeliveryDate, Item AS ItemName , SUM(DelQty) AS DeliveredQty FROM BarcodeTable where BarcodeTable.StatusId<>''5'' AND BookingNo=@BookingNo AND DeliveredQty<>0 And BranchId=@BranchId GROUP BY item,DeliveredQty,ClothDeliveryDate,BookingNo
		END	
END



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_BookingDetailsForDelivery]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'





/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Booking Details for>
-- =============================================
EXEC Sp_Sel_BookingDetailsForDelivery ''33''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_BookingDetailsForDelivery]
	(
		@BookingNumber varchar(15)='''',
		@RowIndex int='''',
		@BranchId varchar(15)=''''
	)
AS
BEGIN
	DECLARE @SQL varchar(max), @PaymentMade float, @DiscountGiven float,@ServiceTax float,@DeliveryRemarks varchar(max)
	CREATE TABLE #TmpDeliveryInfo (BookingNumber varchar(10), BookingDate varchar(20), DeliveryDate varchar(MAX), CustomerCode varchar(10), CustomerName varchar(100), BookingAmount float, Discount float, NetAmount float, PaymentMade float, DuePayment float, DiscountOnPayment float,ServiceTax float,DeliveryRemarks varchar(max),DiscountAmt float)
	INSERT INTO #TmpDeliveryInfo (BookingNumber , CustomerCode , CustomerName , BookingDate, DeliveryDate, BookingAmount, Discount, NetAmount,DiscountAmt)
	 SELECT BookingNumber, BookingByCustomer, CustomerSalutation + '' ''  + CustomerName As CustomerName, Convert(varchar,BookingDate,106) As BookingDate, Convert(varchar, BookingDeliveryDate, 106) + '' , '' + BookingDeliveryTime As BookingDeliveryDate, TotalCost, Discount, NetAmount ,ROUND(DiscountAmt,2)
		FROM EntBookings LEFT JOIN CustomerMaster ON EntBookings.BookingByCustomer = CustomerMaster.CustomerCode
		WHERE BookingNumber = @BookingNumber AND EntBookings.BranchId=@BranchId
	
	SELECT @PaymentMade = COALESCE(SUM(PaymentMade),0), @DiscountGiven = COALESCE(SUM(DiscountOnPayment),0) FROM EntPayment WHERE BookingNumber = @BookingNumber AND BranchId=@BranchId
	set @ServiceTax=round((SELECT SUM(STPAmt) + SUM(Step1Amt) + SUM(Step2Amt) AS Servicetax FROM EntBookingDetails WHERE BookingNumber=@BookingNumber AND BranchId=@BranchId ),2) 
	set @DeliveryRemarks=(SELECT top(1) DeliveryMsg FROM dbo.EntPayment WHERE BookingNumber=@BookingNumber AND BranchId=@BranchId)
	UPDATE #TmpDeliveryInfo SET PaymentMade = @PaymentMade + @DiscountGiven
	UPDATE #TmpDeliveryInfo SET DuePayment= COALESCE(NetAmount - PaymentMade,0), DiscountOnPayment = @DiscountGiven
	UPDATE #TmpDeliveryInfo SET ServiceTax=@ServiceTax,DeliveryRemarks=@DeliveryRemarks	
	--Table(0)
	SELECT * FROM #TmpDeliveryInfo
	--Table(1)
	SELECT BookingNo, RowIndex AS ISN, Item AS ItemName, CASE WHEN BarcodeTable.ItemExtraprocessType = ''0'' THEN ''None'' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraProcessType1, SNo AS ItemTotalQuantity,CASE WHEN ItemExtraprocessType <> ''None'' THEN CASE WHEN ItemExtraprocessType2 <> ''None'' THEN Process + '','' + ItemExtraprocessType + '','' + ItemExtraprocessType2 + '''' ELSE Process + '','' + ItemExtraprocessType + '''' END ELSE CASE WHEN ItemExtraprocessType2 <> ''None'' THEN Process + '','' + ItemExtraprocessType2 + '''' ELSE Process END END AS ItemProcessType, DeliveredQty, AllottedDrawl, DeliverItemStaus, CONVERT(varchar, ClothDeliveryDate, 106) AS Date
	FROM dbo.BarcodeTable
	WHERE dbo.BarcodeTable.StatusId<>''1'' AND dbo.BarcodeTable.StatusId<>''2'' AND BookingNo = @BookingNumber AND BranchId=@BranchId
--	SELECT     dbo.BarcodeTable.BookingNo, dbo.BarcodeTable.RowIndex AS ISN, dbo.BarcodeTable.Item AS ItemName, 
--                      CASE WHEN BarcodeTable.ItemExtraprocessType = ''0'' THEN ''None'' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraProcessType1, dbo.BarcodeTable.SNo AS ItemTotalQuantity, 
--                      dbo.BarcodeTable.Process AS ItemProcessType, dbo.BarcodeTable.DeliveredQty,dbo.BarcodeTable.AllottedDrawl,dbo.BarcodeTable.DeliverItemStaus,convert(varchar,dbo.BarcodeTable.ClothDeliveryDate,106) as Date
--	FROM         dbo.EntChallan INNER JOIN
--                      dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex WHERE BookingNumber = @BookingNumber AND dbo.BarcodeTable.StatusId<>''1'' AND dbo.BarcodeTable.StatusId<>''2''
	--Table(2)
	SELECT Convert(varchar,PaymentDate,106) As ''Paid On'', PaymentMade As ''Payment'',PaymentType AS ''Payment Type'',PaymentRemarks AS ''Payment Details'' FROM EntPayment WHERE BookingNumber = @BookingNumber AND PaymentMade<>0 AND BranchId=@BranchId

	-- Table(3)
	
	SELECT 
		BookingNo, RowIndex AS ISN, Item AS ItemName, CASE WHEN BarcodeTable.ItemExtraprocessType = ''0'' THEN ''None'' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraProcessType1, SNo AS ItemTotalQuantity, Process AS ItemProcessType, DeliveredQty, AllottedDrawl, DeliverItemStaus, CONVERT(varchar, ClothDeliveryDate, 106) AS Date
	FROM
		dbo.BarcodeTable
	WHERE
		(RowIndex = @RowIndex) AND (StatusId <> ''1'') AND (StatusId <> ''2'') AND (BookingNo =@BookingNumber) AND BranchId=@BranchId
--	SELECT     dbo.BarcodeTable.BookingNo, dbo.BarcodeTable.RowIndex AS ISN, dbo.BarcodeTable.Item AS ItemName, 
--                      CASE WHEN BarcodeTable.ItemExtraprocessType = ''0'' THEN ''None'' ELSE BarcodeTable.ItemExtraprocessType END AS ItemExtraProcessType1, dbo.BarcodeTable.SNo AS ItemTotalQuantity, 
--                      dbo.BarcodeTable.Process AS ItemProcessType, dbo.BarcodeTable.DeliveredQty,dbo.BarcodeTable.AllottedDrawl,dbo.BarcodeTable.DeliverItemStaus,convert(varchar,dbo.BarcodeTable.ClothDeliveryDate,106) as Date 
--	FROM         dbo.EntChallan INNER JOIN
--                      dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex WHERE BookingNumber = @BookingNumber AND dbo.BarcodeTable.RowIndex=@RowIndex AND dbo.BarcodeTable.StatusId<>''1'' AND dbo.BarcodeTable.StatusId<>''2''

	DROP TABLE #TmpDeliveryInfo
END










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_AllAmountProcessWiseDateWise]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <23-Feb-2012>
-- Description:	<>
-- =============================================
CREATE PROCEDURE [dbo].[SP_AllAmountProcessWiseDateWise] 
@FDATE VARCHAR(MAX)='''',
@UDATE VARCHAR(MAX)='''',
@PROCESSNAME VARCHAR(MAX)='''',
@PROCESSCODE VARCHAR(MAX)='''',
@BranchId Varchar(Max)=''''

AS
BEGIN
DECLARE @RATE FLOAT,@RATE1 FLOAT,@RATE2 FLOAT,@WHOLEAMT FLOAT,@ITEM INT,@ITEM1 INT,@ITEM2 INT,@WHOLEITEM INT
CREATE TABLE #PROCESSRATE (PROCESS VARCHAR(MAX),AMOUNT FLOAT,QUANTITY VARCHAR(MAX))
					
		SET @RATE = (SELECT SUM((dbo.EntBookingDetails.ITEMSUBTOTAL-(dbo.EntBookingDetails.ITEMEXTRAPROCESSRATE1 + dbo.EntBookingDetails.ITEMEXTRAPROCESSRATE2))) FROM dbo.EntBookingDetails INNER JOIN dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber WHERE dbo.EntBookings.BookingStatus<>''5'' AND dbo.EntBookingDetails.ITEMPROCESSTYPE=@PROCESSNAME AND (dbo.EntBookings.BOOKINGDATE BETWEEN @FDATE AND @UDATE And dbo.EntBookingDetails.BranchId=@BranchId))
		SET @RATE1 = (SELECT SUM(dbo.EntBookingDetails.ItemExtraProcessRate1) FROM dbo.EntBookingDetails INNER JOIN dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber WHERE dbo.EntBookings.BookingStatus<>''5'' AND dbo.EntBookingDetails.ITEMEXTRAPROCESSTYPE1=@PROCESSNAME AND (dbo.EntBookings.BOOKINGDATE BETWEEN @FDATE AND @UDATE And dbo.EntBookingDetails.BranchId=@BranchId))
		SET @RATE2 = (SELECT SUM(dbo.EntBookingDetails.ItemExtraProcessRate2) FROM dbo.EntBookingDetails INNER JOIN dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber WHERE dbo.EntBookings.BookingStatus<>''5'' AND dbo.EntBookingDetails.ITEMEXTRAPROCESSTYPE2=@PROCESSNAME AND (dbo.EntBookings.BOOKINGDATE BETWEEN @FDATE AND @UDATE And dbo.EntBookingDetails.BranchId=@BranchId))
		IF(@RATE IS NULL)
			SET @RATE = 0
		IF(@RATE1 IS NULL)
			SET @RATE1 = 0
		IF(@RATE2 IS NULL)
			SET @RATE2 = 0
		SET	@WHOLEAMT = @RATE + @RATE1 + @RATE2
		
		SET @ITEM = (SELECT COUNT(BOOKINGNO) FROM  BarcodeTable WHERE StatusId<>''5'' AND (PROCESS=@PROCESSNAME) AND (BOOKINGDATE BETWEEN @FDATE AND @UDATE AND BranchId=@BranchId))
		SET @ITEM1 = (SELECT COUNT(BOOKINGNO) FROM  BarcodeTable WHERE StatusId<>''5'' AND (ITEMEXTRAPROCESSTYPE=@PROCESSNAME) AND (BOOKINGDATE BETWEEN @FDATE AND @UDATE And BranchId=@BranchId))
		SET @ITEM2 = (SELECT COUNT(BOOKINGNO) FROM  BarcodeTable WHERE StatusId<>''5'' AND (ITEMEXTRAPROCESSTYPE2=@PROCESSNAME) AND (BOOKINGDATE BETWEEN @FDATE AND @UDATE And BranchId=@BranchId))
		IF(@ITEM IS NULL)
			SET @RATE = 0
		IF(@ITEM1 IS NULL)
			SET @ITEM1 = 0
		IF(@ITEM2 IS NULL)
			SET @ITEM2 = 0
		SET	@WHOLEITEM = @ITEM + @ITEM1 + @ITEM2
		IF(@WHOLEAMT <> 0)
		BEGIN
			IF(@WHOLEITEM <> 0)
				INSERT INTO #PROCESSRATE VALUES(UPPER(@PROCESSCODE) , @WHOLEAMT , @WHOLEITEM)
		END
		ELSE
		BEGIN
			IF(@WHOLEITEM <> 0)
				INSERT INTO #PROCESSRATE VALUES(UPPER(@PROCESSCODE) , @WHOLEAMT , @WHOLEITEM)
		END

	SELECT * FROM #PROCESSRATE
	DROP TABLE #PROCESSRATE
END









' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_AllAmountProcessWise]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <23-Feb-2012>
-- Description:	<>
-- =============================================
CREATE PROCEDURE [dbo].[SP_AllAmountProcessWise] 
@FDATE VARCHAR(MAX)='''',
@UDATE VARCHAR(MAX)='''',
@BranchId Varchar(Max)=''''

AS
BEGIN
DECLARE @PROCESSCODE VARCHAR(MAX),@PROCESSNAME VARCHAR(MAX),@RATE FLOAT,@RATE1 FLOAT,@RATE2 FLOAT,@WHOLEAMT FLOAT,@ITEM INT,@ITEM1 INT,@ITEM2 INT,@WHOLEITEM INT
CREATE TABLE #PROCESSRATE (PROCESS VARCHAR(MAX),AMOUNT FLOAT,QUANTITY VARCHAR(MAX))
DECLARE @PROCESS CURSOR
SET @PROCESS = CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
	SELECT PROCESSCODE, PROCESSNAME AS CODE FROM PROCESSMASTER Where BranchId=@BranchId ORDER BY PROCESSCODE 
	OPEN @PROCESS
		FETCH NEXT
			FROM @PROCESS INTO @PROCESSNAME,@PROCESSCODE
				WHILE @@FETCH_STATUS = 0
				BEGIN					
					SET @RATE = (SELECT SUM((dbo.EntBookingDetails.ITEMSUBTOTAL-(dbo.EntBookingDetails.ITEMEXTRAPROCESSRATE1 + dbo.EntBookingDetails.ITEMEXTRAPROCESSRATE2))) FROM dbo.EntBookingDetails INNER JOIN dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber WHERE  dbo.EntBookings.BookingStatus<>''5'' AND dbo.EntBookingDetails.ITEMPROCESSTYPE=@PROCESSNAME AND (dbo.EntBookings.BOOKINGDATE BETWEEN @FDATE AND @UDATE And dbo.EntBookingDetails.BranchId=@BranchId))
					SET @RATE1 = (SELECT SUM(dbo.EntBookingDetails.ItemExtraProcessRate1) FROM dbo.EntBookingDetails INNER JOIN dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber WHERE dbo.EntBookings.BookingStatus<>''5'' AND dbo.EntBookingDetails.ITEMEXTRAPROCESSTYPE1=@PROCESSNAME AND (dbo.EntBookings.BOOKINGDATE BETWEEN @FDATE AND @UDATE And dbo.EntBookingDetails.BranchId=@BranchId))
					SET @RATE2 = (SELECT SUM(dbo.EntBookingDetails.ItemExtraProcessRate2) FROM dbo.EntBookingDetails INNER JOIN dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber WHERE dbo.EntBookings.BookingStatus<>''5'' AND dbo.EntBookingDetails.ITEMEXTRAPROCESSTYPE2=@PROCESSNAME AND (dbo.EntBookings.BOOKINGDATE BETWEEN @FDATE AND @UDATE And dbo.EntBookingDetails.BranchId=@BranchId))
					IF(@RATE IS NULL)
						SET @RATE = 0
					IF(@RATE1 IS NULL)
						SET @RATE1 = 0
					IF(@RATE2 IS NULL)
						SET @RATE2 = 0
					SET	@WHOLEAMT = @RATE + @RATE1 + @RATE2
					
					SET @ITEM = (SELECT COUNT(BOOKINGNO) FROM  BarcodeTable WHERE StatusId<>''5'' AND (PROCESS=@PROCESSNAME) AND (BOOKINGDATE BETWEEN @FDATE AND @UDATE And BranchId=@BranchId))
					SET @ITEM1 = (SELECT COUNT(BOOKINGNO) FROM  BarcodeTable WHERE StatusId<>''5'' AND (ITEMEXTRAPROCESSTYPE=@PROCESSNAME) AND (BOOKINGDATE BETWEEN @FDATE AND @UDATE And BranchId=@BranchId))
					SET @ITEM2 = (SELECT COUNT(BOOKINGNO) FROM  BarcodeTable WHERE StatusId<>''5'' AND (ITEMEXTRAPROCESSTYPE2=@PROCESSNAME) AND (BOOKINGDATE BETWEEN @FDATE AND @UDATE And BranchId=@BranchId))
					IF(@ITEM IS NULL)
						SET @RATE = 0
					IF(@ITEM1 IS NULL)
						SET @ITEM1 = 0
					IF(@ITEM2 IS NULL)
						SET @ITEM2 = 0
					SET	@WHOLEITEM = @ITEM + @ITEM1 + @ITEM2
					IF(@WHOLEAMT <> 0)
					BEGIN
						IF(@WHOLEITEM <> 0)
							INSERT INTO #PROCESSRATE VALUES(UPPER(@PROCESSCODE) , @WHOLEAMT , @WHOLEITEM)
					END
					ELSE
					BEGIN
						IF(@WHOLEITEM <> 0)
							INSERT INTO #PROCESSRATE VALUES(UPPER(@PROCESSCODE) , @WHOLEAMT , @WHOLEITEM)
					END
					FETCH NEXT
						FROM @PROCESS INTO @PROCESSNAME,@PROCESSCODE	
				END
	CLOSE @PROCESS
	DEALLOCATE @PROCESS
	SELECT * FROM #PROCESSRATE
	DROP TABLE #PROCESSRATE
END










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_AllAmountProcessWiseDateWiseDay]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'





-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <24-Feb-2012>
-- Description:	<>
-- =============================================
CREATE PROCEDURE [dbo].[SP_AllAmountProcessWiseDateWiseDay] 
@FDATE VARCHAR(MAX)='''',
@UDATE VARCHAR(MAX)='''',
@PROCESSNAME VARCHAR(MAX)='''',
@PROCESSCODE VARCHAR(MAX)='''',
@BranchId Varchar(Max)=''''

AS
BEGIN
DECLARE @TDATE VARCHAR(MAX),@RATE FLOAT,@RATE1 FLOAT,@RATE2 FLOAT,@WHOLEAMT FLOAT,@ITEM INT,@ITEM1 INT,@ITEM2 INT,@WHOLEITEM INT
CREATE TABLE #PROCESSRATE (DATE VARCHAR(MAX),PROCESS VARCHAR(MAX),AMOUNT FLOAT,QUANTITY VARCHAR(MAX))
		SET @TDATE = @FDATE	
		WHILE(CONVERT(DATETIME,@TDATE) <= CONVERT(DATETIME,@UDATE))		
			BEGIN		
				SET @RATE = (SELECT SUM((dbo.EntBookingDetails.ITEMSUBTOTAL-(dbo.EntBookingDetails.ITEMEXTRAPROCESSRATE1 + dbo.EntBookingDetails.ITEMEXTRAPROCESSRATE2))) FROM dbo.EntBookingDetails INNER JOIN dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber WHERE  dbo.EntBookings.BookingStatus<>''5'' AND dbo.EntBookingDetails.ITEMPROCESSTYPE=@PROCESSNAME AND (dbo.EntBookings.BOOKINGDATE BETWEEN @TDATE AND @TDATE And dbo.EntBookingDetails.BranchId=@BranchId))
				SET @RATE1 = (SELECT SUM(dbo.EntBookingDetails.ItemExtraProcessRate1) FROM dbo.EntBookingDetails INNER JOIN dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber WHERE dbo.EntBookings.BookingStatus<>''5'' AND dbo.EntBookingDetails.ITEMEXTRAPROCESSTYPE1=@PROCESSNAME AND (dbo.EntBookings.BOOKINGDATE BETWEEN @TDATE AND @TDATE And dbo.EntBookingDetails.BranchId=@BranchId))
				SET @RATE2 = (SELECT SUM(dbo.EntBookingDetails.ItemExtraProcessRate2) FROM dbo.EntBookingDetails INNER JOIN dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber WHERE dbo.EntBookings.BookingStatus<>''5'' AND dbo.EntBookingDetails.ITEMEXTRAPROCESSTYPE2=@PROCESSNAME AND (dbo.EntBookings.BOOKINGDATE BETWEEN @TDATE AND @TDATE And dbo.EntBookingDetails.BranchId=@BranchId))
				IF(@RATE IS NULL)
					SET @RATE = 0
				IF(@RATE1 IS NULL)
					SET @RATE1 = 0
				IF(@RATE2 IS NULL)
					SET @RATE2 = 0
				SET	@WHOLEAMT = @RATE + @RATE1 + @RATE2
				
				SET @ITEM = (SELECT COUNT(BOOKINGNO) FROM  BarcodeTable WHERE StatusId<>''5'' AND (PROCESS=@PROCESSNAME) AND (BOOKINGDATE BETWEEN @TDATE AND @TDATE And BranchId=@BranchId))
				SET @ITEM1 = (SELECT COUNT(BOOKINGNO) FROM  BarcodeTable WHERE  StatusId<>''5'' AND (ITEMEXTRAPROCESSTYPE=@PROCESSNAME) AND (BOOKINGDATE BETWEEN @TDATE AND @TDATE and BranchId=@BranchId))
				SET @ITEM2 = (SELECT COUNT(BOOKINGNO) FROM  BarcodeTable WHERE StatusId<>''5'' AND (ITEMEXTRAPROCESSTYPE2=@PROCESSNAME) AND (BOOKINGDATE BETWEEN @TDATE AND @TDATE and BranchId=@BranchId))
				IF(@ITEM IS NULL)
					SET @RATE = 0
				IF(@ITEM1 IS NULL)
					SET @ITEM1 = 0
				IF(@ITEM2 IS NULL)
					SET @ITEM2 = 0
				SET	@WHOLEITEM = @ITEM + @ITEM1 + @ITEM2
				IF(@WHOLEAMT <> 0)
				BEGIN
					IF(@WHOLEITEM <> 0)
						INSERT INTO #PROCESSRATE VALUES(CONVERT(VARCHAR,CONVERT(DATETIME,@TDATE),106),UPPER(@PROCESSCODE) , @WHOLEAMT , @WHOLEITEM)
				END
				ELSE
				BEGIN
					IF(@WHOLEITEM <> 0)
						INSERT INTO #PROCESSRATE VALUES(CONVERT(VARCHAR,CONVERT(DATETIME,@TDATE),106),UPPER(@PROCESSCODE) , @WHOLEAMT , @WHOLEITEM)
				END
				SET @TDATE=DATEADD(day,1,CONVERT(DATETIME,@TDATE))		
			END
			
	SELECT * FROM #PROCESSRATE
	DROP TABLE #PROCESSRATE
END











' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_AllAmountProcessWiseDayByDay]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'





-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <24-Feb-2012>
-- Description:	<>
-- =============================================
CREATE PROCEDURE [dbo].[SP_AllAmountProcessWiseDayByDay] 
@FDATE VARCHAR(MAX)='''',
@UDATE VARCHAR(MAX)='''',
@BranchId Varchar(Max)=''''

AS
BEGIN
DECLARE @TDATE VARCHAR(MAX),@PROCESSCODE VARCHAR(MAX),@PROCESSNAME VARCHAR(MAX),@RATE FLOAT,@RATE1 FLOAT,@RATE2 FLOAT,@WHOLEAMT FLOAT,@ITEM INT,@ITEM1 INT,@ITEM2 INT,@WHOLEITEM INT
CREATE TABLE #PROCESSRATE (DATE VARCHAR(MAX),PROCESS VARCHAR(MAX),AMOUNT FLOAT,QUANTITY VARCHAR(MAX))
DECLARE @PROCESS CURSOR
SET @PROCESS = CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
	SELECT PROCESSCODE, PROCESSNAME AS CODE FROM PROCESSMASTER where BranchId=@BranchId ORDER BY PROCESSCODE 
	OPEN @PROCESS
		FETCH NEXT
			FROM @PROCESS INTO @PROCESSNAME,@PROCESSCODE
				WHILE @@FETCH_STATUS = 0
				BEGIN	
					SET @TDATE=	@FDATE	
					WHILE(CONVERT(DATETIME,@TDATE) <= CONVERT(DATETIME,@UDATE))		
						BEGIN
							SET @RATE = (SELECT SUM((dbo.EntBookingDetails.ITEMSUBTOTAL-(dbo.EntBookingDetails.ITEMEXTRAPROCESSRATE1 + dbo.EntBookingDetails.ITEMEXTRAPROCESSRATE2))) FROM dbo.EntBookingDetails INNER JOIN dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber WHERE dbo.EntBookings.BookingStatus<>''5'' AND dbo.EntBookingDetails.ITEMPROCESSTYPE=@PROCESSNAME AND (dbo.EntBookings.BOOKINGDATE BETWEEN @TDATE AND @TDATE And dbo.EntBookingDetails.BranchId=@BranchId))
							SET @RATE1 = (SELECT SUM(dbo.EntBookingDetails.ItemExtraProcessRate1) FROM dbo.EntBookingDetails INNER JOIN dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber WHERE dbo.EntBookings.BookingStatus<>''5'' AND dbo.EntBookingDetails.ITEMEXTRAPROCESSTYPE1=@PROCESSNAME AND (dbo.EntBookings.BOOKINGDATE BETWEEN @TDATE AND @TDATE And dbo.EntBookingDetails.BranchId=@BranchId))
							SET @RATE2 = (SELECT SUM(dbo.EntBookingDetails.ItemExtraProcessRate2) FROM dbo.EntBookingDetails INNER JOIN dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber WHERE dbo.EntBookings.BookingStatus<>''5'' AND dbo.EntBookingDetails.ITEMEXTRAPROCESSTYPE2=@PROCESSNAME AND (dbo.EntBookings.BOOKINGDATE BETWEEN @TDATE AND @TDATE And dbo.EntBookingDetails.BranchId=@BranchId))
							IF(@RATE IS NULL)
								SET @RATE = 0
							IF(@RATE1 IS NULL)
								SET @RATE1 = 0
							IF(@RATE2 IS NULL)
								SET @RATE2 = 0
							SET	@WHOLEAMT = @RATE + @RATE1 + @RATE2
							
							SET @ITEM = (SELECT COUNT(BOOKINGNO) FROM  BarcodeTable WHERE StatusId<>''5'' AND (PROCESS=@PROCESSNAME) AND (BOOKINGDATE BETWEEN @TDATE AND @TDATE And BranchId=@BranchId))
							SET @ITEM1 = (SELECT COUNT(BOOKINGNO) FROM  BarcodeTable WHERE StatusId<>''5'' AND (ITEMEXTRAPROCESSTYPE=@PROCESSNAME) AND (BOOKINGDATE BETWEEN @TDATE AND @TDATE And BranchId=@BranchId))
							SET @ITEM2 = (SELECT COUNT(BOOKINGNO) FROM  BarcodeTable WHERE StatusId<>''5'' AND (ITEMEXTRAPROCESSTYPE2=@PROCESSNAME) AND (BOOKINGDATE BETWEEN @TDATE AND @TDATE And BranchId=@BranchId))
							IF(@ITEM IS NULL)
								SET @RATE = 0
							IF(@ITEM1 IS NULL)
								SET @ITEM1 = 0
							IF(@ITEM2 IS NULL)
								SET @ITEM2 = 0
							SET	@WHOLEITEM = @ITEM + @ITEM1 + @ITEM2
							IF(@WHOLEAMT <> 0)
							BEGIN
								IF(@WHOLEITEM <> 0)
									INSERT INTO #PROCESSRATE VALUES(CONVERT(VARCHAR,CONVERT(DATETIME,@TDATE),106),UPPER(@PROCESSCODE) , @WHOLEAMT , @WHOLEITEM)
							END
							ELSE
							BEGIN
								IF(@WHOLEITEM <> 0)
									INSERT INTO #PROCESSRATE VALUES(CONVERT(VARCHAR,CONVERT(DATETIME,@TDATE),106),UPPER(@PROCESSCODE) , @WHOLEAMT , @WHOLEITEM)
							END
							SET @TDATE=DATEADD(day,1,CONVERT(DATETIME,@TDATE))						
						END
					FETCH NEXT
						FROM @PROCESS INTO @PROCESSNAME,@PROCESSCODE	
				END
	CLOSE @PROCESS
	DEALLOCATE @PROCESS
	SELECT * FROM #PROCESSRATE
	DROP TABLE #PROCESSRATE
END











' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_DefaultUrgentShow]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

/*
-- =============================================
-- Author:		<KARAM CHAND SHARMA>
-- Create date: <30 Jan 2012>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_DefaultUrgentShow ''13/Mar/2012'',''13/Mar/2012'',''1''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_DefaultUrgentShow]
	(
		@BookDate1 datetime,
		@BookDate2 datetime	,
		@BranchId VARCHAR(MAX)=''''
	)
AS
BEGIN
	DECLARE @Tax FLOAT, @BookingNumber VARCHAR(MAX),@BookingDate VARCHAR(MAX),@BookingDeliveryTime VARCHAR(MAX),@Qty VARCHAR(MAX),@NetAmount VARCHAR(MAX),@PaymentMade VARCHAR(MAX),@DiscountOnPayment VARCHAR(MAX),@DuePayment VARCHAR(MAX),@BookingStatus VARCHAR(MAX),@BookingDeliveryDate VARCHAR(MAX),@PaymentDate VARCHAR(MAX)
	CREATE TABLE #tempUrgentDelivery(BookingNumber VARCHAR(MAX),BookingDate VARCHAR(MAX),BookingDeliveryTime VARCHAR(MAX),Qty VARCHAR(MAX),NetAmount VARCHAR(MAX),PaymentMade VARCHAR(MAX),DiscountOnPayment VARCHAR(MAX),DuePayment VARCHAR(MAX),BookingStatus VARCHAR(MAX),BookingDeliveryDate VARCHAR(MAX),PaymentDate VARCHAR(MAX))
	DECLARE Invoice CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
		SELECT DISTINCT dbo.EntBookings.BookingNumber FROM dbo.EntBookings INNER JOIN  dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo WHERE (dbo.EntBookings.BookingStatus<>''5'') AND (dbo.EntBookings.BranchId=@BranchId) AND (dbo.EntBookings.IsUrgent=''True'') AND  (dbo.BarcodeTable.DelQty <> ''1'') AND (dbo.BarcodeTable.DueDate  BETWEEN @BookDate1 AND @BookDate2)
		OPEN Invoice
			FETCH NEXT FROM Invoice into @BookingNumber
			WHILE @@Fetch_Status=0
			BEGIN				
				SELECT  @BookingNumber=EB.BookingNumber,@BookingDate=CONVERT(varchar, EB.BookingDate, 106),@BookingDeliveryTime= EB.BookingDeliveryTime, @Qty=EB.Qty, @NetAmount=EB.NetAmount,@PaymentMade= SUM(COALESCE (EP.PaymentMade, 0)),@DiscountOnPayment= SUM(COALESCE (EP.DiscountOnPayment, 0)),@DuePayment=( EB.NetAmount - SUM(COALESCE (EP.PaymentMade, 0) + COALESCE (EP.DiscountOnPayment, 0))), @BookingStatus=EB.BookingStatus,@BookingDeliveryDate= CONVERT(varchar, EB.BookingDeliveryDate, 106) ,@PaymentDate=CONVERT(varchar, EP.PaymentDate, 106) 
				FROM dbo.EntBookings AS EB LEFT OUTER JOIN dbo.EntPayment AS EP ON EB.BookingNumber = EP.BookingNumber
				WHERE EB.BookingDeliveryDate BETWEEN @BookDate1 AND @BookDate2 And (EB.IsUrgent = ''True'') AND (EB.BookingNumber=@BookingNumber)
				GROUP BY EB.BookingNumber, EB.BookingDate, EB.NetAmount, EB.Qty, EB.BookingStatus, EB.BookingDeliveryDate, EP.PaymentDate, EB.BookingDeliveryTime				
				IF(@BookingDate IS NOT NULL)
					INSERT INTO #tempUrgentDelivery (BookingNumber,BookingDate,BookingDeliveryTime,Qty,NetAmount,PaymentMade,DiscountOnPayment,DuePayment,BookingStatus,BookingDeliveryDate,PaymentDate) VALUES (@BookingNumber,@BookingDate,@BookingDeliveryTime,@Qty,@NetAmount,@PaymentMade,@DiscountOnPayment,@DuePayment,@BookingStatus,@BookingDeliveryDate,@PaymentDate)
				FETCH NEXT FROM Invoice into @BookingNumber
			END
		CLOSE  Invoice
		DEALLOCATE Invoice
		SELECT * FROM #tempUrgentDelivery
		DROP TABLE #tempUrgentDelivery
END










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_CustomerStatus]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'




/*
-- =============================================
-- Author:		<Karam chnad sharma>
-- Create date: <6 Jan 2012>
-- Description:	<To select Challan Details for challan return>
-- =============================================
EXEC Sp_Sel_CustomerStatus ''1/1/2012'',''1/30/2012'',''Cust1''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_CustomerStatus]
	(
		@BookDate1 datetime,
		@BookDate2	datetime,
		@BookNumberFrom varchar(15) = '''',
		@BookNumberUpto varchar(10) = '''',
		@ChallanShift varchar(10) = '''',
		@CustomerId varchar(15) = '''',
		@BranchId Varchar(max)=''''
	)
AS
BEGIN
	SELECT DISTINCT 
		TOP (100) PERCENT dbo.EntBookingDetails.BookingNumber, dbo.EntBookingDetails.ISN, 
		CASE WHEN EntBookingDetails.ItemExtraProcessType1 <> ''None'' THEN CASE WHEN EntBookingDetails.ItemExtraProcessType2 <> ''None'' THEN EntBookingDetails.ItemProcessType
		+ '','' + EntBookingDetails.ItemExtraProcessType1 + '','' + EntBookingDetails.ItemExtraProcessType2 + '''' ELSE EntBookingDetails.ItemProcessType + '','' +
		EntBookingDetails.ItemExtraProcessType1 + '''' END ELSE CASE WHEN EntBookingDetails.ItemExtraProcessType2 <> ''None'' THEN EntBookingDetails.ItemProcessType
		+ '','' + EntBookingDetails.ItemExtraProcessType2 + '''' ELSE EntBookingDetails.ItemProcessType END END AS ItemProcessType, 
		dbo.EntBookingDetails.ItemTotalQuantity AS Qty, dbo.ItemMaster.ItemName, 
		dbo.EntBookingDetails.ItemTotalQuantity * dbo.ItemMaster.NumberOfSubItems AS ItemTotalQuantity, COUNT(dbo.BarcodeTable.DelQty) 
		AS DeliveredQty, dbo.EntBookingDetails.ItemTotalQuantity * dbo.ItemMaster.NumberOfSubItems - COUNT(dbo.BarcodeTable.DelQty) AS ItemsPending, 
		dbo.EntBookings.BookingByCustomer, CONVERT(VARCHAR, dbo.EntBookings.BookingDate, 106) AS BookingDate, dbo.EntBookings.IsUrgent
	FROM 
		dbo.EntBookingDetails INNER JOIN
		dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber AND 
		dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber AND 
		dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber INNER JOIN
		dbo.ItemMaster ON dbo.EntBookingDetails.ItemName = dbo.ItemMaster.ItemName INNER JOIN
		dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo AND 
		dbo.EntBookingDetails.BookingNumber = dbo.BarcodeTable.BookingNo AND 
		dbo.EntBookingDetails.BookingNumber = dbo.BarcodeTable.BookingNo AND 
		dbo.EntBookingDetails.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.ItemMaster.ItemName = dbo.BarcodeTable.BookingItemName AND 
		dbo.ItemMaster.ItemName = dbo.BarcodeTable.BookingItemName AND dbo.EntBookingDetails.ISN = dbo.BarcodeTable.BarcodeISN AND 
		dbo.EntBookingDetails.ISN = dbo.BarcodeTable.BarcodeISN AND dbo.EntBookingDetails.ISN = dbo.BarcodeTable.BarcodeISN
	WHERE
		(dbo.BarcodeTable.DelQty <> ''0'') AND (dbo.EntBookings.BookingDate BETWEEN CONVERT(varchar, @BookDate1, 106) AND CONVERT(varchar, @BookDate2, 106)) AND (dbo.EntBookings.BookingByCustomer = @CustomerId) AND (dbo.EntBookingDetails.BranchId = @BranchId)
	GROUP BY 
		dbo.EntBookingDetails.BookingNumber, dbo.EntBookingDetails.ISN, dbo.EntBookingDetails.ItemProcessType, dbo.EntBookingDetails.ItemExtraProcessType1, dbo.EntBookingDetails.ItemExtraProcessType2, dbo.EntBookingDetails.ItemQuantityAndRate, dbo.ItemMaster.ItemName, dbo.EntBookingDetails.ItemTotalQuantity, dbo.ItemMaster.NumberOfSubItems, dbo.EntBookings.BookingByCustomer, dbo.EntBookings.BookingDate, dbo.EntBookings.IsUrgent, dbo.BarcodeTable.BarcodeISN
	ORDER BY 
		dbo.EntBookingDetails.BookingNumber, dbo.EntBookingDetails.ISN
END




' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_InsertIntoBarcodeTable]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<KaramChandSharma And Manoj Kumar Gupta>
-- Create date: <18-Nov-2011>
-- Description:	<Insert Into Barcode Table>
-- =============================================
CREATE PROCEDURE [dbo].[sp_InsertIntoBarcodeTable]
@BOOKINGNUMBER VARCHAR(MAX)='''',
@BranchId VARCHAR(MAX)=''''
AS
BEGIN	
DECLARE @SNO INT,@RowIndex INT,@ItemCount INT, @ID VARCHAR(MAX),@Barcode VARCHAR(MAX), @ItemName1 VARCHAR(MAX),@Qty VARCHAR(MAX),@TempQty INT, @ISN VARCHAR(MAX),@ItemName VARCHAR(MAX),@ItemTotalQuantity VARCHAR(MAX),@ItemProcessType VARCHAR(MAX),@ItemQuantityAndRate VARCHAR(MAX),@ItemExtraProcessType1 VARCHAR(MAX),@ItemExtraProcessRate1 VARCHAR(MAX),@ItemExtraProcessType2 VARCHAR(MAX),@ItemExtraProcessRate2 VARCHAR(MAX),@ItemSubTotal VARCHAR(MAX),@ItemStatus VARCHAR(MAX),@ItemRemark VARCHAR(MAX),@DeliveredQty VARCHAR(MAX),@ItemColor VARCHAR(MAX),@VendorItemStatus VARCHAR(MAX),@STPAmt VARCHAR(MAX),@STEP1Amt VARCHAR(MAX),@STEP2Amt VARCHAR(MAX),@BookingItemName VARCHAR(MAX),@BarcodeISN VARCHAR(MAX)
DECLARE @Color VARCHAR(MAX),@StatusId VARCHAR(MAX)
--SET @BOOKINGNUMBER = (SELECT COALESCE(MAX(Convert(int, BookingNumber)),0) FROM EntBookings)
SET @RowIndex=1
SET @ItemCount=0
DECLARE @BookingDate1 VARCHAR(MAX),@BookingDeliveryTime1 VARCHAR(MAX),@BookingDeliveryDate1 VARCHAR(MAX),@BookingByCustomer1 VARCHAR(MAX)		

select @BookingDate1=BookingDate,@BookingDeliveryTime1=BookingDeliveryTime,@BookingDeliveryDate1=BookingDeliveryDate,@BookingByCustomer1=BookingByCustomer from entbookings WHERE BookingNumber=@BOOKINGNUMBER AND BranchId=@BranchId
DECLARE @Details CURSOR	
SET @Details = CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
	SELECT BookingNumber, ISN, ItemName, ItemTotalQuantity, ItemProcessType, ItemQuantityAndRate, ItemExtraProcessType1, ItemExtraProcessRate1, ItemExtraProcessType2, ItemExtraProcessRate2, ItemSubTotal, ItemStatus, ItemRemark, DeliveredQty, ItemColor, VendorItemStatus, STPAmt, STEP1Amt, STEP2Amt FROM dbo.EntBookingDetails WHERE BOOKINGNUMBER=@BOOKINGNUMBER AND BranchId=@BranchId
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
										SET @NewQty = CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
											SELECT dbo.EntSubItemDetails.SubItemName as ItemName,dbo.ItemMaster.NumberOfsubItems  as Qty FROM dbo.ItemMaster INNER JOIN dbo.EntSubItemDetails ON dbo.ItemMaster.ItemName = dbo.EntSubItemDetails.ItemName	WHERE dbo.ItemMaster.ItemName = @ItemName AND dbo.EntSubItemDetails.BranchId=@BranchId
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
																			SELECT @StatusId= IsChallan FROM dbo.ProcessMaster WHERE ProcessCode=@ItemProcessType	AND BranchId=@BranchId																	
																			IF @StatusId = 0
																				BEGIN
																					SELECT @StatusId=IsChallan FROM dbo.ProcessMaster WHERE ProcessCode=@ItemExtraProcessType1 AND BranchId=@BranchId			
																					IF @StatusId = 0
																						BEGIN
																							SELECT @StatusId=IsChallan FROM dbo.ProcessMaster WHERE ProcessCode=@ItemExtraProcessType2	AND BranchId=@BranchId			
																							IF @StatusId = 0	
																								SET @StatusId=''3''
																							ELSE
																								SET @StatusId=''1''		
																						END	
																					ELSE
																						SET @StatusId=''1''	
																				END
																			ELSE
																				SET @StatusId=''1''	
																			INSERT INTO dbo.BarcodeTable (Id,BookingDate,CurrentTime,DueDate,Item,BarCode,Process,StatusId,BookingNo,SNo,RowIndex,BookingByCustomer,Colour,ItemExtraprocessType,DrawlStatus,DrawlAlloted,DeliveredQty,ItemRemarks,ItemTotalandSubTotal,ItemExtraprocessType2,BookingItemName,BarcodeISN,DelQty,BranchId)
																			VALUES
																			(@ID,@BookingDate1,@BookingDeliveryTime1,@BookingDeliveryDate1,@ItemName1,(''*''+@BOOKINGNUMBER+''-''+ CONVERT(VARCHAR,@RowIndex)+''*'') ,@ItemProcessType,@StatusId,@BOOKINGNUMBER,''1'',@RowIndex,@BookingByCustomer1, @ItemColor,@ItemExtraProcessType1,''false'',''false'',0,@ItemRemark,(@ItemTotalQuantity+''/''+ CONVERT(VARCHAR,@SNO)),@ItemExtraProcessType2,@ItemName,@ISN,''0'',@BranchId)
																			SET @ItemCount=@ItemCount+1	
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
															UPDATE EntBookings SET Barcode=(''*''+@BOOKINGNUMBER+''-''+ CONVERT(VARCHAR,''1'')+''*'') WHERE BookingNumber=@BOOKINGNUMBER AND BranchId=@BranchId			
												CLOSE @NewQty
												DEALLOCATE @NewQty
									SET @TotalQty=@TotalQty-1
								END								
							END
					FETCH NEXT
						FROM @Details INTO @BOOKINGNUMBER,@ISN, @ItemName, @ItemTotalQuantity, @ItemProcessType, @ItemQuantityAndRate, @ItemExtraProcessType1, @ItemExtraProcessRate1, @ItemExtraProcessType2, @ItemExtraProcessRate2, @ItemSubTotal, @ItemStatus, @ItemRemark, @DeliveredQty, @ItemColor, @VendorItemStatus, @STPAmt, @STEP1Amt, @STEP2Amt
					END
					UPDATE EntBookings SET QTY=@ItemCount WHERE BOOKINGNUMBER=@BOOKINGNUMBER AND BranchId=@BranchId			
	CLOSE @Details
	DEALLOCATE @Details
END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ChallanInProc]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE proc [dbo].[sp_ChallanInProc]
(	
	@Flag VARCHAR(MAX)='''',
	@BranchId VARCHAR(MAX)='''',
	@ChallanNumber INT ='''',
	@ChallanBranchCode VARCHAR(MAX)='''',
	@ChallanDate DATETIME='''',
	@ChallanSendingShift VARCHAR(MAX)='''',
	@BookingNumber VARCHAR(MAX)='''',
	@ItemSNo INT ='''',
	@SubItemName VARCHAR(MAX)='''',
	@ItemTotalQuantitySent INT ='''',
	@ItemsReceivedFromVendor INT ='''',
	@ItemReceivedFromVendorOnDate DATETIME='''',
	@Urgent BIT='''',
	@RowIndex INT=''''
)
as
BEGIN
	IF(@Flag=1)
	BEGIN
--		SET @ChallanNumber=(Select COALESCE(MAX(Convert(INT, ChallanNumber)),0) From EntChallan WHERE BranchId=@BranchId)
--		SET @ChallanNumber=@ChallanNumber+1
		Insert Into EntChallan Values(@ChallanNumber,@ChallanBranchCode,@ChallanDate,@ChallanSendingShift,@BookingNumber,@ItemSNo,@SubItemName,@ItemTotalQuantitySent,''0'',getdate(),@Urgent,@BranchId,'''')
		Update EntBookings Set BookingStatus=''2'' Where BookingNumber=@BookingNumber
		Update BarcodeTable Set StatusId=''2'' Where BookingNo=@BookingNumber And RowIndex=@RowIndex And StatusId=''1''
		Update EntBookingDetails Set ItemStatus=''2'' Where BookingNumber=@BookingNumber And ISN=@RowIndex And ItemStatus=''1''
	END
END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Item]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


-- =============================================
-- Author:		<Karam chand sharma>
-- Create date: <29-Feb-2012>
-- Description:	<>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Item]	
	@Flag VARCHAR(MAX)='''',
	@ItemName VARCHAR(MAX)='''',
	@OldItemName VARCHAR(MAX)='''',
	@NoOfItem VARCHAR(MAX)='''',
	@ItemCode VARCHAR(MAX)='''',
	@BranchId VARCHAR(MAX)='''',
	@ExternalBranchId VARCHAR(MAX)='''',
	@SubItemName VARCHAR(MAX)='''',
	@SubItemOrder VARCHAR(MAX)='''',
	@ItemID VARCHAR(MAX)=''''
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
			SELECT * FROM ItemMaster WHERE BranchId=@BranchId AND ItemName LIKE ''%''+@ItemName+''%'' OR ItemCode Like ''%''+@ItemName+''%'' ORDER BY ItemName
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
			SELECT * FROM ItemMaster WHERE BranchId=@BranchId ORDER BY ItemName
		END
	IF(@Flag = 7)
		BEGIN
			UPDATE EntBookingDetails Set ItemName=@ItemName Where ItemName=@OldItemName AND BranchId=@BranchId
			UPDATE ItemWiseProcessRate Set ItemName=@ItemName Where ItemName=@OldItemName AND BranchId=@BranchId
			UPDATE BarcodeTable Set Item=@ItemName Where Item=@OldItemName AND BranchId=@BranchId
		END
END





' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_DeliveryQuantityandBooking]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'





/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport ''10/19/2010'',''10/22/2010''
EXEC Sp_Sel_QuantityandBooking ''1 Dec 2010'',''31 Dec 2010''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_DeliveryQuantityandBooking]
	(
		@BookDate1 datetime,
		@BookDate2 datetime,
		@BranchId VARCHAR(MAX)=''''
	)
AS
BEGIN
	DECLARE  @Tax FLOAT, @BookingNumber VARCHAR(MAX),@BookingDeliveryDate VARCHAR(MAX),@Qty VARCHAR(MAX),@NetAmount VARCHAR(MAX),@PaymentMade VARCHAR(MAX),@DiscountOnPayment VARCHAR(MAX),@DuePayment VARCHAR(MAX),@BookingStatus VARCHAR(MAX)
	CREATE TABLE #tempUrgentDelivery(BookingNumber VARCHAR(MAX),BookingDeliveryDate VARCHAR(MAX),Qty VARCHAR(MAX),NetAmount VARCHAR(MAX),PaymentMade VARCHAR(MAX),DiscountOnPayment VARCHAR(MAX),DuePayment VARCHAR(MAX),BookingStatus VARCHAR(MAX))
	DECLARE Invoice CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
		SELECT DISTINCT BookingNo FROM dbo.BarcodeTable WHERE (StatusId<>''5'') AND BranchId=@BranchId AND (DelQty <> ''1'') AND (DueDate  BETWEEN @BookDate1 AND @BookDate2)
		OPEN Invoice
			FETCH NEXT FROM Invoice into @BookingNumber
			WHILE @@Fetch_Status=0
			BEGIN					
				INSERT INTO #tempUrgentDelivery (BookingNumber,BookingDeliveryDate,Qty,NetAmount,PaymentMade,DiscountOnPayment,DuePayment,BookingStatus)  
				SELECT EB.BookingNumber, convert(varchar,BookingDeliveryDate,106) as BookingDeliveryDate,Qty, NetAmount, SUM(COALESCE(PaymentMade,0)) as PaymentMade, SUM(COALESCE(DiscountOnPayment,0)) As DiscountOnPayment, NETAMOUNT-SUM(COALESCE(PaymentMade,0)+COALESCE(DiscountOnPayment,0))AS DuePayment ,EB.BookingStatus
				FROM EntBookings EB LEFT JOIN EntPayment EP ON EB.BookingNumber = EP.BookingNumber
				WHERE EB.BranchId=@BranchId AND EB.BookingDeliveryDate BETWEEN @BookDate1 AND @BookDate2 AND EB.BookingNumber=@BookingNumber
				GROUP BY EB.BookingNumber, BookingDeliveryDate, NetAmount,Qty,EB.BookingStatus
				order by EB.BookingNumber
				FETCH NEXT FROM Invoice into @BookingNumber
			END
		CLOSE  Invoice
		DEALLOCATE Invoice
		SELECT * FROM #tempUrgentDelivery
		DROP TABLE #tempUrgentDelivery
END





' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ChallanSummary]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <9-Jan-2012>
-- Description:	<Challan Summary>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ChallanSummary]	
	@ChallanNo VARCHAR(MAX)
AS
BEGIN	
	DECLARE @Count INT ,@BookingDate VARCHAR(MAX),@ChallanNumber VARCHAR(MAX),@BookingNumber VARCHAR(MAX),@DueDate VARCHAR(MAX),@IsUrgent VARCHAR(MAX),@HomeDelivery VARCHAR(MAX),@Details VARCHAR(MAX),@Temp VARCHAR(MAX),@Temp1 VARCHAR(MAX),@ChallanDate VARCHAR(MAX) ,@ChallanSendingShift VARCHAR(MAX)
CREATE TABLE #TempTable(BookingDate VARCHAR(MAX),ChallanNumber VARCHAR(MAX),BookingNumber VARCHAR(MAX),DueDate VARCHAR(MAX),IsUrgent VARCHAR(MAX),HomeDelivery VARCHAR(MAX),Details VARCHAR(MAX),ChallanDate VARCHAR(MAX),ChallanSendingShift VARCHAR(MAX),CountClothes VARCHAR(MAX))

DECLARE @Detail CURSOR	
	SET @Detail = CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
		SELECT	distinct LEFT(CONVERT(varchar, dbo.BarcodeTable.BookingDate, 106),6) AS BookingDate, dbo.EntChallan.ChallanNumber, dbo.EntChallan.BookingNumber, LEFT(CONVERT(varchar, dbo.BarcodeTable.DueDate, 106),6) AS DueDate, dbo.EntBookings.IsUrgent, dbo.EntBookings.HomeDelivery,CONVERT(VARCHAR,dbo.EntChallan.ChallanDate,106) AS ChallanDate, dbo.EntChallan.ChallanSendingShift FROM dbo.EntChallan INNER JOIN dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex INNER JOIN dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber WHERE (dbo.EntChallan.ChallanNumber =@ChallanNo) Order BY BookingNumber
		OPEN @Detail
		FETCH NEXT
			FROM @Detail INTO @BookingDate ,@ChallanNumber ,@BookingNumber ,@DueDate ,@IsUrgent ,@HomeDelivery,@ChallanDate,@ChallanSendingShift
				WHILE @@FETCH_STATUS = 0
					BEGIN
						SET @BookingNumber=@BookingNumber							
						DECLARE @Data CURSOR	
							SET @Data = CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
								SELECT    ''['' + dbo.BarcodeTable.Item + '' '' + CASE WHEN barcodetable.Process = ''NONE'' THEN '''' ELSE barcodetable.Process END + '' '' + CASE WHEN dbo.BarcodeTable.ItemExtraprocessType= ''None'' THEN '''' ELSE dbo.BarcodeTable.ItemExtraprocessType END + '' '' + CASE WHEN dbo.BarcodeTable.ItemExtraprocessType2 = ''None'' THEN '''' ELSE dbo.BarcodeTable.ItemExtraprocessType2 END + '' '' + dbo.BarcodeTable.ItemRemarks + '' '' + dbo.BarcodeTable.Colour + '']'' AS Details FROM         dbo.EntChallan INNER JOIN dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex INNER JOIN dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber WHERE (dbo.EntChallan.ChallanNumber = @ChallanNo) AND dbo.EntChallan.BookingNumber=@BookingNumber
								OPEN @Data
								FETCH NEXT
									FROM @Data INTO @Details	
										SET @Count=1
										INSERT INTO #TempTable(BookingDate,ChallanNumber,BookingNumber,DueDate,IsUrgent,HomeDelivery,Details,ChallanDate,ChallanSendingShift,CountClothes) VALUES (@BookingDate,@ChallanNumber,@BookingNumber,@DueDate,@IsUrgent,@HomeDelivery,@Details,@ChallanDate,@ChallanSendingShift,@Count)														
										FETCH NEXT
											FROM @Data INTO @Details
										WHILE @@FETCH_STATUS = 0
											BEGIN
												SELECT @Temp=Details FROM #TempTable
												SET @Temp1 = @Temp+'',''+@Details												
												UPDATE #TempTable SET Details=@Temp1 WHERE  BookingNumber=@BookingNumber
												SET @Count=@Count+1
												FETCH NEXT
													FROM @Data INTO @Details
											END
										CLOSE @Data
										DEALLOCATE @Data
										UPDATE #TempTable SET CountClothes=@Count WHERE  BookingNumber=@BookingNumber
						--INSERT INTO #TempTable(BookingDate,ChallanNumber,BookingNumber,DueDate,IsUrgent,HomeDelivery,Details) VALUES (@BookingDate,@ChallanNumber,@BookingNumber,@DueDate,@IsUrgent,@HomeDelivery,@Details)
					FETCH NEXT
							FROM @Detail INTO @BookingDate ,@ChallanNumber ,@BookingNumber ,@DueDate ,@IsUrgent ,@HomeDelivery,@ChallanDate,@ChallanSendingShift
				END
			CLOSE @Detail
			DEALLOCATE @Detail
		SELECT * FROM #TempTable
		DROP TABLE #TempTable
END



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ChallanItemWise]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'







-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <11-Jan-2012>
-- Description:	<Challan Summary>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ChallanItemWise]	
	@ChallanNo VARCHAR(MAX)='''',
	@BranchId Varchar(max)=''''
AS
BEGIN	
	DECLARE @Count1 INT ,@Count INT ,@BookingDate VARCHAR(MAX),@ChallanNumber VARCHAR(MAX),@BookingNumber VARCHAR(MAX),@DueDate VARCHAR(MAX),@IsUrgent VARCHAR(MAX),@HomeDelivery VARCHAR(MAX),@Details VARCHAR(MAX),@Temp VARCHAR(MAX),@Temp1 VARCHAR(MAX),@ChallanDate VARCHAR(MAX) ,@ChallanSendingShift VARCHAR(MAX)
CREATE TABLE #TempTable(BookingDate VARCHAR(MAX),ChallanNumber VARCHAR(MAX),BookingNumber VARCHAR(MAX),DueDate VARCHAR(MAX),IsUrgent VARCHAR(MAX),HomeDelivery VARCHAR(MAX),Details VARCHAR(MAX),ChallanDate VARCHAR(MAX),ChallanSendingShift VARCHAR(MAX),CountClothes VARCHAR(MAX))

DECLARE @Detail CURSOR	
	SET @Detail = CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
		SELECT	distinct LEFT(CONVERT(varchar, dbo.BarcodeTable.BookingDate, 106),6) AS BookingDate, dbo.EntChallan.ChallanNumber, dbo.EntChallan.BookingNumber, LEFT(CONVERT(varchar, dbo.BarcodeTable.DueDate, 106),6) AS DueDate, dbo.EntBookings.IsUrgent, dbo.EntBookings.HomeDelivery,CONVERT(VARCHAR,dbo.EntChallan.ChallanDate,106) AS ChallanDate, dbo.EntChallan.ChallanSendingShift FROM dbo.EntChallan INNER JOIN dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex INNER JOIN dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber WHERE (dbo.EntChallan.ChallanNumber =@ChallanNo) And (dbo.EntChallan.BranchId=@BranchId) Order BY BookingNumber
		OPEN @Detail
		FETCH NEXT
			FROM @Detail INTO @BookingDate ,@ChallanNumber ,@BookingNumber ,@DueDate ,@IsUrgent ,@HomeDelivery,@ChallanDate,@ChallanSendingShift
				WHILE @@FETCH_STATUS = 0
					BEGIN
						SET @BookingNumber=@BookingNumber							
						DECLARE @Data CURSOR	
							SET @Data = CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
								SELECT CONVERT(varchar, COUNT(dbo.BarcodeTable.Item)) + ''-'' + dbo.BarcodeTable.Item AS Details, COUNT(dbo.BarcodeTable.Item)  FROM dbo.EntChallan INNER JOIN dbo.BarcodeTable ON dbo.EntChallan.BookingNumber = dbo.BarcodeTable.BookingNo AND dbo.EntChallan.ItemSNo = dbo.BarcodeTable.RowIndex INNER JOIN dbo.EntBookings ON dbo.BarcodeTable.BookingNo = dbo.EntBookings.BookingNumber WHERE (dbo.EntChallan.ChallanNumber = @ChallanNo) AND dbo.EntChallan.BookingNumber=@BookingNumber And dbo.EntChallan.BranchId=@BranchId GROUP BY dbo.BarcodeTable.Item 
								OPEN @Data
								FETCH NEXT
									FROM @Data INTO @Details,@Count	
										SET @Count1=@Count
										INSERT INTO #TempTable(BookingDate,ChallanNumber,BookingNumber,DueDate,IsUrgent,HomeDelivery,Details,ChallanDate,ChallanSendingShift,CountClothes) VALUES (@BookingDate,@ChallanNumber,@BookingNumber,@DueDate,@IsUrgent,@HomeDelivery,@Details,@ChallanDate,@ChallanSendingShift,@Count1)														
										FETCH NEXT
											FROM @Data INTO @Details,@Count
										WHILE @@FETCH_STATUS = 0
											BEGIN
												SELECT @Temp=Details FROM #TempTable
												SET @Temp1 = @Temp+'',''+@Details	
												SET @Count1=@Count1+@Count											
												UPDATE #TempTable SET Details=@Temp1,@Count=@Count1 WHERE  BookingNumber=@BookingNumber 
												
												FETCH NEXT
													FROM @Data INTO @Details,@Count
											END
										CLOSE @Data
										DEALLOCATE @Data
										UPDATE #TempTable SET CountClothes=@Count WHERE  BookingNumber=@BookingNumber
						--INSERT INTO #TempTable(BookingDate,ChallanNumber,BookingNumber,DueDate,IsUrgent,HomeDelivery,Details) VALUES (@BookingDate,@ChallanNumber,@BookingNumber,@DueDate,@IsUrgent,@HomeDelivery,@Details)
					FETCH NEXT
							FROM @Detail INTO @BookingDate ,@ChallanNumber ,@BookingNumber ,@DueDate ,@IsUrgent ,@HomeDelivery,@ChallanDate,@ChallanSendingShift
				END
			CLOSE @Detail
			DEALLOCATE @Detail
		SELECT * FROM #TempTable
		DROP TABLE #TempTable
END







' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_DefaultHomeDeliveryShow]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'




/*
-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <25 JAN 2012>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_DefaultHomeDeliveryShow ''13/Mar/2012'',''13/Mar/2012'',''1''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_DefaultHomeDeliveryShow]
	(
		@BookDate1 datetime,
		@BookDate2 datetime	,
		@BranchId VARCHAR(MAX)=''''
	)
AS
BEGIN
	DECLARE @Tax FLOAT,  @BookingNumber VARCHAR(MAX),@BookingDate VARCHAR(MAX),@Qty VARCHAR(MAX),@NetAmount VARCHAR(MAX),@PaymentMade VARCHAR(MAX),@DiscountOnPayment VARCHAR(MAX),@DuePayment VARCHAR(MAX),@BookingStatus VARCHAR(MAX),@BookingDeliveryDate VARCHAR(MAX),@PaymentDate VARCHAR(MAX)
	CREATE TABLE #tempHomeDelivery(BookingNumber VARCHAR(MAX),BookingDate VARCHAR(MAX),Qty VARCHAR(MAX),NetAmount VARCHAR(MAX),PaymentMade VARCHAR(MAX),DiscountOnPayment VARCHAR(MAX),DuePayment VARCHAR(MAX),BookingStatus VARCHAR(MAX),BookingDeliveryDate VARCHAR(MAX),PaymentDate VARCHAR(MAX))
	DECLARE InvoiceNo CURSOR  LOCAL FORWARD_ONLY STATIC READ_ONLY FOR 
		SELECT DISTINCT dbo.EntBookings.BookingNumber FROM dbo.EntBookings INNER JOIN  dbo.BarcodeTable ON dbo.EntBookings.BookingNumber = dbo.BarcodeTable.BookingNo WHERE (dbo.EntBookings.BookingStatus<>''5'') AND (dbo.EntBookings.BranchId=@BranchId) AND (dbo.EntBookings.HomeDelivery=''True'') AND  (dbo.BarcodeTable.DelQty <> ''1'') AND (dbo.BarcodeTable.DueDate  BETWEEN @BookDate1 AND @BookDate2)
		OPEN InvoiceNo
			FETCH NEXT FROM InvoiceNo into @BookingNumber
			WHILE @@Fetch_Status=0
			BEGIN
				--SELECT @Tax=ROUND(SUM(STPAmt) + SUM(STEP1Amt) + SUM(STEP2Amt), 2) FROM  dbo.EntBookingDetails WHERE  (BookingNumber = @BookingNumber)
				SELECT @BookingNumber=EB.BookingNumber,@BookingDate=CONVERT(varchar, EB.BookingDate, 106),@Qty=EB.Qty, @NetAmount=EB.NetAmount,@PaymentMade= SUM(COALESCE (EP.PaymentMade, 0)) , @DiscountOnPayment=SUM(COALESCE (EP.DiscountOnPayment, 0)) ,@DuePayment= (EB.NetAmount - SUM(COALESCE (EP.PaymentMade, 0) + COALESCE (EP.DiscountOnPayment, 0))) ,@BookingStatus= EB.BookingStatus,@BookingDeliveryDate= CONVERT(varchar, EB.BookingDeliveryDate, 106) , @PaymentDate=CONVERT(varchar, EP.PaymentDate, 106) 
				FROM dbo.EntBookings AS EB LEFT OUTER JOIN dbo.EntPayment AS EP ON EB.BookingNumber = EP.BookingNumber
				WHERE (EB.BookingDeliveryDate BETWEEN @BookDate1 AND @BookDate2) AND (EB.HomeDelivery = ''True'') AND (EB.BookingNumber=@BookingNumber)
				GROUP BY EB.BookingNumber, EB.BookingDate, EB.NetAmount, EB.Qty, EB.BookingStatus, EB.BookingDeliveryDate, EP.PaymentDate, EB.BookingDeliveryTime
				IF(@BookingDate IS NOT NULL)
					INSERT INTO #tempHomeDelivery (BookingNumber,BookingDate,Qty,NetAmount,PaymentMade,DiscountOnPayment,DuePayment,BookingStatus,BookingDeliveryDate,PaymentDate) VALUES (@BookingNumber,@BookingDate,@Qty,@NetAmount,@PaymentMade,@DiscountOnPayment,@DuePayment,@BookingStatus,@BookingDeliveryDate,@PaymentDate)				
				FETCH NEXT FROM InvoiceNo into @BookingNumber
			END
		CLOSE  InvoiceNo
		DEALLOCATE InvoiceNo
		SELECT * FROM #tempHomeDelivery
		DROP TABLE #tempHomeDelivery
	
END












' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Priority]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		<Manoj Kumar Gupta>
-- Create date: <27-Feb-2012>
-- Description:	<Remark Master>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Priority]	
	@Flag VARCHAR(MAX)='''',
	@Priority VARCHAR(MAX)='''',
	@BranchId VARCHAR(10)='''',
	@MaxId VARCHAR(MAX)='''',
	@ID INT=''''
	
AS
BEGIN	
	IF(@Flag = 1)
		BEGIN
			SET @MaxId=(Select Max(PriorityID) From PriorityMaster where BranchId=@BranchId)
			IF @MaxId IS NULL
				SET @MaxId=0
			SET @ID= CONVERT(VARCHAR,(CONVERT(INT,@MaxId)+1))
		INSERT INTO PriorityMaster(PriorityID,Priority,BranchId) VALUES ((CONVERT(INT,@MaxId)+1),@Priority,@BranchId)
		END
	IF(@Flag = 2)
		BEGIN
		UPDATE PriorityMaster SET Priority=@Priority WHERE BranchId=@BranchId AND PriorityID=@ID
		END
	IF(@Flag = 3)
		BEGIN
		SELECT * FROM PriorityMaster WHERE BranchId=@BranchId order by PriorityID desc
		END
	IF(@Flag = 4)
		BEGIN
		SELECT * FROM PriorityMaster WHERE BranchId=@BranchId AND Priority like ''%''+@Priority+''%'' order by PriorityID desc
		END
	IF(@Flag = 5)
		BEGIN
		DELETE FROM PriorityMaster WHERE Priority=@Priority AND BranchId=@BranchId
		END
	IF(@Flag = 6)
		BEGIN
		SELECT * FROM PriorityMaster WHERE Priority=@Priority AND BranchId=@BranchId
		END
END








' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_sp_ProcessMaster]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		<Manoj Kumar Gupta>
-- Create date: <02-Feb-2012>
-- Description:	<Process Master>
-- =============================================
CREATE PROCEDURE [dbo].[sp_sp_ProcessMaster]	
	@Flag VARCHAR(MAX)='''',
	@ProcessCode VARCHAR(MAX)='''',
	@ProcessName VARCHAR(MAX)='''',
	@ProcessUsedForVendorReport bit='''',
	@Discount  bit='''',
	@ServiceTax float='''',
	@IsActiveServiceTax bit='''',
	@IsDiscount bit='''',
	@ImagePath VARCHAR(MAX)='''',
	@IsChallan bit='''',
	@BranchId VARCHAR(10)=''''
AS
BEGIN	
	IF(@Flag = 1)
		Begin
		Insert Into ProcessMaster (ProcessCode,ProcessName,ProcessUsedForVendorReport,Discount,ServiceTax,IsActiveServiceTax,IsDiscount,ImagePath,IsChallan,BranchId) VALUES (@ProcessCode,@ProcessName,@ProcessUsedForVendorReport,@Discount,@ServiceTax,@IsActiveServiceTax,@IsDiscount,@ImagePath,@IsChallan,@BranchId)
		End
	IF(@Flag = 2)
		BEGIN
			Update ProcessMaster Set ProcessCode=@ProcessCode,ProcessName=@ProcessName,ProcessUsedForVendorReport=@ProcessUsedForVendorReport,Discount=@Discount,ServiceTax=@ServiceTax,IsActiveServiceTax=@IsActiveServiceTax,IsDiscount=@IsDiscount,ImagePath=@ImagePath,IsChallan=@IsChallan WHERE ProcessCode=@ProcessCode AND BranchId=@BranchId
--			Update ItemwiseProcessRate Set ProcessCode=@ProcessCode
--			Update EntBookingDetails Set ItemProcessType=@ProcessCode Where ItemProcessType=@ProcessCode
--			Update EntBookingDetails Set ItemExtraProcessType1=@ProcessCode Where ItemExtraProcessType1=@ProcessCode
--			Update EntBookingDetails Set ItemExtraProcessType2=@ProcessCode Where ItemExtraProcessType2=@ProcessCode
--			Update EntBookingDetails Set ItemExtraProcessType3=@ProcessCode Where ItemExtraProcessType3=@ProcessCode
		END
	IF(@Flag = 3)
		BEGIN
			DELETE FROM ProcessMaster WHERE ProcessCode=@ProcessCode AND BranchId=@BranchId
			DELETE FROM ItemWiseProcessRate WHERE ProcessCode=@ProcessCode AND BranchId=@BranchId			
		END
	IF(@Flag = 4)
		BEGIN		
			IF(@ProcessCode='''')
			SELECT * FROM PROCESSMASTER WHERE BranchId=@BranchId
			ELSE	
			SELECT * FROM PROCESSMASTER WHERE ProcessCode=@ProcessCode AND BranchId=@BranchId
		END
	IF(@Flag = 5)
		BEGIN
			IF(@ProcessName='''')	
			SELECT ProcessCode, ProcessName, ProcessUsedForVendorReport, Discount, CASE WHEN ServiceTax = ''0'' THEN '''' ELSE ServiceTax END As ServiceTax, IsActiveServiceTax,IsDiscount,ImagePath,IsChallan FROM dbo.ProcessMaster WHERE ProcessName like ''%''+@ProcessName+''%'' AND BranchId=@BranchId
			ELSE	
			SELECT ProcessCode, ProcessName, ProcessUsedForVendorReport, Discount, CASE WHEN ServiceTax = ''0'' THEN '''' ELSE ServiceTax END As ServiceTax, IsActiveServiceTax,IsDiscount,ImagePath,IsChallan FROM dbo.ProcessMaster WHERE ProcessName like ''%''+@ProcessName+''%'' AND BranchId=@BranchId		
		END
	IF(@Flag = 6)
		BEGIN
			SELECT COUNT(*) FROM EntBookingDetails WHERE (ItemProcessType=@ProcessCode) OR (ItemExtraProcessType1=@ProcessCode) OR (ItemExtraProcessType2=@ProcessCode) OR (ItemExtraProcessType3=@ProcessCode) AND BranchId=@BranchId
		END
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ItemWisePriceList]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


-- =============================================
-- Author:		<Manoj Kumar Gupta>
-- Create date: <27-Feb-2012>
-- Description:	<Price Master>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ItemWisePriceList]	
	@Flag VARCHAR(MAX)='''',
	@ItemName VARCHAR(MAX)='''',
	@BranchId VARCHAR(10)='''',
	@ProcessCode VARCHAR(MAX)='''',
	@ProcessRate FLOAT=''''
	
AS
BEGIN	
	IF(@Flag = 1)
		BEGIN
		-- drop down bind
		SELECT ItemID, ItemName FROM ItemMaster WHERE BranchId=@BranchId ORDER BY ItemName		
		END	
	IF(@Flag = 2)
		BEGIN
		-- grid bind
		SELECT ProcessCode, ProcessName FROM ProcessMaster WHERE BranchId=@BranchId 		
		END
	IF(@Flag = 3)
		BEGIN
		Select * From ItemwiseProcessRate Where ItemName=@ItemName AND BranchId=@BranchId		
		END	
	IF(@Flag = 4)
		BEGIN
		Delete From ItemwiseProcessRate Where ItemName=@ItemName AND BranchId=@BranchId
		END
	IF(@Flag = 5)
		BEGIN
		Insert into ItemwiseProcessRate Values (@ItemName,@ProcessCode,@ProcessRate,@BranchId,'''')
		END
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ItemWiseRate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'




-- =============================================
-- Author:		<Karam Chand Sharma>
-- Create date: <24-Jan-2012>
-- Description:	<Item wise rate>
-- =============================================
CREATE PROCEDURE [dbo].[sp_ItemWiseRate]	

AS
BEGIN
	DECLARE @Item VARCHAR(MAX),@PName VARCHAR(MAX),@Sql VARCHAR(MAX),@SqlItem VARCHAR(MAX),@Rate VARCHAR(MAX)
CREATE TABLE #TmpTable (ITEMNAME VARCHAR(MAX))
DECLARE @Process CURSOR
SET @Process = CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
	SELECT ProcessName FROM dbo.ProcessMaster ORDER BY ProcessName
	OPEN @Process
		FETCH NEXT
			FROM @Process INTO @PName				
				WHILE @@FETCH_STATUS = 0
					BEGIN					
						SET @Sql=''ALTER TABLE #TmpTable ADD  ''+ REPLACE(UPPER(@PName),'' '',''_'') +'' VARCHAR(MAX) NULL''
						EXEC (@Sql)						
						FETCH NEXT
							FROM @Process INTO @PName
					END
	CLOSE @Process
	DEALLOCATE @Process	



DECLARE @ItemName CURSOR
SET @ItemName = CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
	SELECT ItemName FROM dbo.ItemMaster
	OPEN @ItemName
		FETCH NEXT
			FROM @ItemName INTO @Item				
				WHILE @@FETCH_STATUS = 0
					BEGIN	
						INSERT INTO #TmpTable (ITEMNAME) VALUES	(@Item)			
						DECLARE @Process1 CURSOR
						SET @Process1 = CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
							SELECT ProcessName FROM dbo.ProcessMaster
							OPEN @Process1
								FETCH NEXT
									FROM @Process1 INTO @PName				
										WHILE @@FETCH_STATUS = 0
											BEGIN					
												SET @Rate=(SELECT dbo.ItemwiseProcessRate.ProcessPrice FROM dbo.ItemMaster INNER JOIN dbo.ItemwiseProcessRate ON dbo.ItemMaster.ItemName = dbo.ItemwiseProcessRate.ItemName INNER JOIN dbo.ProcessMaster ON dbo.ItemwiseProcessRate.ProcessCode = dbo.ProcessMaster.ProcessCode WHERE dbo.ProcessMaster.ProcessName=@PName AND dbo.ItemMaster.ItemName=@Item)												
												IF @Rate<>''''												
													SET @SqlItem=''UPDATE #TmpTable SET ''+ REPLACE(UPPER(@PName),'' '',''_'') +''=''''''+ @Rate +'''''' WHERE ITEMNAME=''''''+ @Item +''''''''													
												ELSE
													SET @SqlItem=''UPDATE #TmpTable SET ''+ REPLACE(UPPER(@PName),'' '',''_'') +''= 0 WHERE ITEMNAME=''''''+ @Item +''''''''	
												EXEC (@SqlItem)						
												FETCH NEXT
													FROM @Process1 INTO @PName
											END
							CLOSE @Process1
							DEALLOCATE @Process1
						FETCH NEXT
							FROM @ItemName INTO @Item
					END
	CLOSE @ItemName
	DEALLOCATE @ItemName	

SELECT * FROM  #TmpTable
DROP TABLE #TmpTable


END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Import]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		<karam chand sharma>
-- Create date: <1/02/2012>
-- Description:	<Balance Sales>
-- =============================================
CREATE PROCEDURE [dbo].[Sp_Import]
@Flag VARCHAR(MAX)='''',
@ItemName VARCHAR(MAX)='''',
@ProcessPrice VARCHAR(MAX)='''',
@ProcessName VARCHAR(MAX)='''',
@Path VARCHAR(MAX)=''''
AS
BEGIN
	
	IF(@Flag=1)
		BEGIN			
			Declare @PName Varchar(max)
			Declare @Sql Varchar(max)
			CREATE TABLE #ProcessTmpTable (ITEMNAME VARCHAR(MAX))
			DECLARE @Process CURSOR
			SET @Process = CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR
				SELECT ProcessName FROM dbo.ProcessMaster ORDER BY ProcessName
				OPEN @Process
					FETCH NEXT 
						FROM @Process INTO @PName				
							WHILE @@FETCH_STATUS = 0
								BEGIN					
									SET @Sql=''ALTER TABLE #ProcessTmpTable ADD  ''+ REPLACE(UPPER(@PName),'' '',''_'') +'' VARCHAR(MAX) NULL''
									EXEC (@Sql)						
									FETCH NEXT
										FROM @Process INTO @PName
								END
				CLOSE @Process
				DEALLOCATE @Process	
--				DECLARE @Path1 VARCHAR(MAX)
--				SELECT * FROM #TmpTable
--				SET @Path1=''C:\Users\karamchand.sharma\Desktop\QuickSoft\Logo\Rate.xls''
				DECLARE @Query VARCHAR(MAX)				
				SET @Query=''INSERT INTO #ProcessTmpTable SELECT * FROM OPENROWSET''
				SET @Query=@Query +''(''''MSDASQL'''',''''Driver={Microsoft Excel Driver (*.xls)};DBQ=''+@Path+'';HDR=YES'''',''''SELECT * FROM [Rate_List$] WHERE ITEMNAME<>NULL'''')''
				PRINT @Query				
				exec (@Query)		
				--INSERT INTO #TmpTable SELECT * FROM OPENROWSET(''MSDASQL'',''Driver={Microsoft Excel Driver (*.xls)};DBQ=C:\DC\Rate.xls;HDR=YES'',''SELECT * FROM [Rate_List$] WHERE ITEMNAME<>NULL'')		
				SELECT * FROM #ProcessTmpTable
				SELECT COLUMN_NAME FROM tempdb.INFORMATION_SCHEMA.COLUMNS WHERE table_name like ''#ProcessTmpTable%''	
				SELECT ItemName FROM ItemMaster WHERE (ItemName NOT IN (SELECT ITEMNAME FROM #ProcessTmpTable))
				SELECT ProcessName FROM ProcessMaster WHERE Processname NOT IN (SELECT REPLACE(COLUMN_NAME,''_'','' '') FROM tempdb.INFORMATION_SCHEMA.COLUMNS WHERE table_name like ''#ProcessTmpTable%'' AND COLUMN_NAME <> ''ITEMNAME'')
				DROP TABLE #ProcessTmpTable
				DELETE FROM ItemwiseProcessRate
			END
		IF(@Flag=2)
			BEGIN
				DECLARE @PCode VARCHAR(MAX)				
				SELECT @PCode=ProcessCode FROM ProcessMaster WHERE ProcessName=@ProcessName
				INSERT INTO ItemwiseProcessRate (ItemName,ProcessCode,ProcessPrice) VALUES (@ItemName,@PCode,@ProcessPrice)
			END
END










' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_BookingDetailsForReceipt1]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'




/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Booking Details for>
-- =============================================
EXEC Sp_Sel_BookingDetailsForReceipt ''9''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_BookingDetailsForReceipt1]
	(
		@BookingNumber varchar(10)='''',
		@Date DATETIME='''',
		@BranchId VARCHAR(MAX)=''''
	)
AS
BEGIN
	DECLARE @SQL varchar(max), @PaymentMade float, @DiscountGiven float, @CustCode varchar(10)
	DECLARE @TotNetAmount float, @TotPaymentMade float
	SET @CustCode = ''''
	SELECT @CustCode= BookingByCustomer FROM EntBookings WHERE BookingNumber = @BookingNumber AND BranchId=@BranchId
	CREATE TABLE #TmpDeliveryInfo (BookingNumber varchar(10), BookingDate varchar(20), DeliveryDate varchar(20), DeliveryTime varchar(20), CustomerCode varchar(10), CustomerName varchar(100), CustomerAddress varchar(100), CustomerPhone varchar(50), CustomerPriority varchar(50), BookingRemarks varchar(50), BookingAmount float, Discount float, NetAmount float, PaymentMade float, DuePayment float, DiscountOnPayment float, HomeDelivery VARCHAR(10),Barcode VARCHAR(100),Urgent VARCHAR (15), PrevDue float,DiscountAmt float)
	INSERT INTO #TmpDeliveryInfo (BookingNumber , CustomerCode , CustomerName, CustomerAddress, CustomerPhone, CustomerPriority, BookingDate, DeliveryDate, DeliveryTime, BookingAmount, Discount, NetAmount, HomeDelivery,Barcode,Urgent, BookingRemarks,DiscountAmt)
	  SELECT BookingNumber, BookingByCustomer, CustomerSalutation + '' ''  + CustomerName As CustomerName, CustomerAddress, CustomerMobile, Priority, Convert(varchar,BookingDate,106) As BookingDate, Convert(varchar, BookingDeliveryDate, 106) As BookingDeliveryDate, BookingDeliveryTime, TotalCost, Discount, NetAmount,CASE WHEN HomeDelivery = ''0'' THEN '''' ELSE ''H'' END,Barcode ,CASE WHEN Isurgent = ''0'' THEN '''' ELSE ''U'' END  , Convert(varchar(50),BookingRemarks), DiscountAmt
		FROM EntBookings LEFT JOIN CustomerMaster ON EntBookings.BookingByCustomer = CustomerMaster.CustomerCode LEFT JOIN PriorityMaster ON CustomerMaster.CustomerPriority = PriorityMaster.PriorityID
		WHERE BookingNumber = @BookingNumber AND EntBookings.BranchId=@BranchId
	
	SELECT @PaymentMade = COALESCE(SUM(PaymentMade),0), @DiscountGiven = COALESCE(SUM(DiscountOnPayment),0) FROM EntPayment WHERE BookingNumber = @BookingNumber AND BranchId=@BranchId
	UPDATE #TmpDeliveryInfo SET PaymentMade = @PaymentMade
	UPDATE #TmpDeliveryInfo SET DuePayment= COALESCE(NetAmount - PaymentMade,0), DiscountOnPayment = @DiscountGiven
	
	SELECT @TotNetAmount = SUM(NetAmount) FROM EntBookings WHERE BookingByCustomer = @CustCode AND Convert(int, BookingNumber)<Convert(int, @BookingNumber) AND BranchId=@BranchId
	SELECT @TotPaymentMade = SUM(PaymentMade) FROM EntPayment WHERE BookingNumber IN (SELECT BookingNumber FROM EntBookings WHERE BookingByCustomer = @CustCode  AND Convert(int, BookingNumber)<Convert(int, @BookingNumber) AND BranchId=@BranchId)
	UPDATE #TmpDeliveryInfo SET PrevDue = COALESCE(@TotNetAmount- @TotPaymentMade,0)
	--UPDATE #TmpDeliveryInfo SET PrevDue = (PrevDue - DuePayment)
	
	SELECT * FROM #TmpDeliveryInfo
	SELECT BookingNumber, ISN, EntBookingDetails.ItemName, ItemTotalQuantity, COALESCE(ItemTotalQuantity,1)* COALESCE(NumberOfSubItems,1) As SubPieces, ItemProcessType, ItemQuantityAndRate, ItemExtraProcessType1, ItemExtraProcessRate1, ItemExtraProcessType2, ItemExtraProcessRate2, ItemSubTotal, ItemStatus, ItemRemark, DeliveredQty,ItemColor,STPAmt,STEP1Amt,STEP2Amt  FROM EntBookingDetails LEFT JOIN ItemMaster ON EntBookingDetails.ItemName = ItemMaster.ItemName WHERE BookingNumber = @BookingNumber AND EntBookingDetails.BranchId=@BranchId
	SELECT Convert(varchar,PaymentDate,100) As ''Paid On'', PaymentMade As ''Payment'' FROM EntPayment WHERE BookingNumber = @BookingNumber and PaymentMade<>0 AND PaymentDate <@Date AND BranchId=@BranchId
	SELECT SUM(PaymentMade) As ''Payment'' FROM EntPayment WHERE BookingNumber = @BookingNumber and PaymentMade<>0 AND convert(varchar,PaymentDate,6)= @Date AND BranchId=@BranchId

	
	DROP TABLE #TmpDeliveryInfo
END




' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_BookingDetailsForReceipt]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/*
-- =============================================
-- Author:		<Manoj Kumar Guest Apperance (karam Chand Sharma)>
-- Create date: <26 July 2010>
-- Description:	<To select Booking Details for>
-- =============================================
EXEC Sp_Sel_BookingDetailsForReceipt ''58''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_BookingDetailsForReceipt]
	(
		@BookingNumber varchar(10)='''',
		@BranchId varchar(max)=''''
	)
AS
BEGIN
	DECLARE @SQL varchar(max), @PaymentMade float, @DiscountGiven float, @CustCode varchar(10)
	DECLARE @TotNetAmount float, @TotPaymentMade float
	SET @CustCode = ''''
	SELECT @CustCode= BookingByCustomer FROM EntBookings WHERE BookingNumber = @BookingNumber AND BranchId=@BranchId
	CREATE TABLE #TmpDeliveryInfo (BookingNumber varchar(10), BookingDate varchar(20), DeliveryDate varchar(20), DeliveryTime varchar(20), CustomerCode varchar(10), CustomerName varchar(100), CustomerAddress varchar(100), CustomerPhone varchar(50), CustomerPriority varchar(50), BookingRemarks varchar(50), BookingAmount float, Discount float, NetAmount float, PaymentMade float, DuePayment float, DiscountOnPayment float, HomeDelivery VARCHAR(10),Barcode VARCHAR(100),Urgent VARCHAR (15), PrevDue float,DiscountAmt float,BookingAcceptedUser varchar(max),StoreAddress varchar(max),TC1 varchar(max),TC2 varchar(max),TC3 varchar(max),StoreName varchar(max),Rounded bit)
	INSERT INTO #TmpDeliveryInfo (BookingNumber , CustomerCode , CustomerName, CustomerAddress, CustomerPhone, CustomerPriority, BookingDate, DeliveryDate, DeliveryTime, BookingAmount, Discount, NetAmount, HomeDelivery,Barcode,Urgent, BookingRemarks,DiscountAmt,BookingAcceptedUser,StoreAddress,TC1,TC2,TC3,StoreName,Rounded)
	  SELECT BookingNumber, BookingByCustomer, CustomerSalutation + '' ''  + CustomerName As CustomerName, CustomerAddress, CustomerMobile, Priority, Convert(varchar,BookingDate,106) As BookingDate, Convert(varchar, BookingDeliveryDate, 106) + '' '' + BookingDeliveryTime As BookingDeliveryDate,'''' as BookingDeliveryTime, TotalCost, Discount, NetAmount,CASE WHEN HomeDelivery = ''0'' THEN '''' ELSE ''H'' END,Barcode ,CASE WHEN Isurgent = ''0'' THEN '''' ELSE ''U'' END  , Convert(varchar(50),BookingRemarks), DiscountAmt,BookingAcceptedByUserId,'''' as StoreAddress,'''' as TC1,'''' as TC2,'''' as TC3,'''' as StoreName,''False'' as Rounded
		FROM EntBookings LEFT JOIN CustomerMaster ON EntBookings.BookingByCustomer = CustomerMaster.CustomerCode LEFT JOIN PriorityMaster ON CustomerMaster.CustomerPriority = PriorityMaster.PriorityID
		WHERE BookingNumber = @BookingNumber AND  dbo.EntBookings.BranchId=@BranchId
	
	SELECT @PaymentMade = COALESCE(SUM(PaymentMade),0), @DiscountGiven = COALESCE(SUM(DiscountOnPayment),0) FROM EntPayment WHERE BookingNumber = @BookingNumber AND BranchId=@BranchId
	UPDATE #TmpDeliveryInfo SET PaymentMade = @PaymentMade 
	UPDATE #TmpDeliveryInfo SET DuePayment= COALESCE(NetAmount - PaymentMade,0), DiscountOnPayment = @DiscountGiven
	
	SELECT @TotNetAmount = SUM(NetAmount) FROM EntBookings WHERE  BranchId=@BranchId AND BookingByCustomer = @CustCode AND Convert(int, BookingNumber)<Convert(int, @BookingNumber)
	SELECT @TotPaymentMade = SUM(PaymentMade) FROM EntPayment WHERE  BranchId=@BranchId AND  BookingNumber IN (SELECT BookingNumber FROM EntBookings WHERE  BranchId=@BranchId AND BookingByCustomer = @CustCode  AND Convert(int, BookingNumber)<Convert(int, @BookingNumber))
	UPDATE #TmpDeliveryInfo SET PrevDue = COALESCE(@TotNetAmount- @TotPaymentMade,0)
	UPDATE #TmpDeliveryInfo SET StoreName =(select top(1) HeaderText from mstReceiptConfig)
	UPDATE #TmpDeliveryInfo SET StoreAddress =(select top(1) AddressText from mstReceiptConfig)
	UPDATE #TmpDeliveryInfo SET TC1 =(select top(1) Term1 from mstReceiptConfig)
	UPDATE #TmpDeliveryInfo SET TC2 =(select top(1) Term2 from mstReceiptConfig)
	UPDATE #TmpDeliveryInfo SET TC3 =(select top(1) Term3 from mstReceiptConfig)
	UPDATE #TmpDeliveryInfo SET Rounded =(select top(1) AmountType from MstConfigSettings)
	
	SELECT * FROM #TmpDeliveryInfo
	SELECT BookingNumber, ISN, EntBookingDetails.ItemName, ItemTotalQuantity, COALESCE(ItemTotalQuantity,1)* COALESCE(NumberOfSubItems,1) As SubPieces, ItemProcessType, ItemQuantityAndRate, ItemExtraProcessType1, ItemExtraProcessRate1, ItemExtraProcessType2, ItemExtraProcessRate2, ItemSubTotal, ItemStatus, ItemRemark, DeliveredQty,ItemColor,STPAmt,STEP1Amt,STEP2Amt  FROM EntBookingDetails LEFT JOIN ItemMaster ON EntBookingDetails.ItemName = ItemMaster.ItemName WHERE BookingNumber = @BookingNumber AND EntBookingDetails.BranchId=@BranchId
	SELECT Convert(varchar,PaymentDate,100) As ''Paid On'', PaymentMade As ''Payment'' FROM EntPayment WHERE BookingNumber = @BookingNumber and PaymentMade<>0 AND BranchId=@BranchId
	
	DROP TABLE #TmpDeliveryInfo
END






' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_ItemWiseReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'




/*
-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <26 July 2010>
-- Description:	<To select Records for booking report>
-- =============================================
EXEC Sp_Sel_BookingReport ''1 SEP 2010'',''2 sep 2010''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_ItemWiseReport]
	(
		@BookDate1 datetime,
		@BookDate2 datetime,
		@ItemName varchar(max),
		@BranchId Varchar(max)
	)
AS
BEGIN
	SELECT convert(int,dbo.EntBookingDetails.BookingNumber) as BookingNumber, dbo.EntBookingDetails.ItemName, dbo.EntBookingDetails.ItemQuantityAndRate, convert(varchar, dbo.EntBookings.BookingDate,106) as BookingDate,convert(varchar, dbo.EntBookings.BookingDeliveryDate,106) as Deliverydate
	FROM   dbo.EntBookingDetails INNER JOIN dbo.EntBookings ON dbo.EntBookingDetails.BookingNumber = dbo.EntBookings.BookingNumber
	WHERE  dbo.EntBookings.BookingStatus<>''5'' AND (dbo.EntBookingDetails.ItemName = @ItemName) And EntBookings.BookingDate BETWEEN @BookDate1 AND @BookDate2 AND dbo.EntBookingDetails.BranchId=@BranchId
	GROUP BY dbo.EntBookingDetails.BookingNumber, dbo.EntBookingDetails.ItemName, dbo.EntBookingDetails.ItemQuantityAndRate,dbo.EntBookings.BookingDate,BookingDeliveryDate
	order by dbo.EntBookingDetails.BookingNumber
END






' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[prcTodayDate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE proc [dbo].[prcTodayDate]
(
@TodayDate datetime=null,
@Flag int=null,
@ExpKey VARCHAR(MAX)='''',
@Key1 nvarchar(max)='''',
@BranchId VARCHAR(MAX)=''''
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
select * from msttask WHERE BranchId=@BranchId order by key1 
end
end

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_BranchMaster]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'








-- =============================================
-- Author:		<Manoj Kumar Gupta>
-- Create date: <27-Feb-2012>
-- Description:	<Branch Master>
-- =============================================
CREATE PROCEDURE [dbo].[sp_BranchMaster]	
	@Flag VARCHAR(MAX)='''',	
	@BranchCode VARCHAR(10)='''',
	@BranchName VARCHAR(MAX)='''',
	@BranchAddress VARCHAR(MAX)='''',
	@BranchPhone VARCHAR(MAX)='''',
	@BranchSlogan VARCHAR(MAX)='''',
	@BranchId VARCHAR(10)='''',
	@ExternalBranchId VARCHAR(10)=''''
	
AS
BEGIN	
	IF(@Flag = 1)
		BEGIN
		--- SAVE DATA IN THE BRANCH MASTER				
		SET @BranchId=(SELECT COALESCE(MAX(Convert(int, BranchId)),0) From BranchMaster)
		SET @BranchId=@BranchId+1		
		INSERT INTO BranchMaster VALUES (@BranchCode,@BranchName,@BranchAddress,@BranchPhone,@BranchSlogan,@BranchId,'''')						
		END	
	IF(@Flag = 2)
		BEGIN
		--- UPDATE DATA IN THE BRANCH MASTER
		UPDATE BranchMaster SET BranchCode=@BranchCode,BranchName=@BranchName,BranchAddress=@BranchAddress,BranchPhone=@BranchPhone,BranchSlogan=@BranchSlogan WHERE BranchId=@BranchId
		END
	IF(@Flag = 3)
		BEGIN
		--- BIND DATA IN THE BRANCH MASTER
		SELECT * FROM BranchMaster
		END
	IF(@Flag = 4)
		BEGIN
		--- FILL DATA IN THE TEXTBOXES BRANCH MASTER
		IF(@BranchId='''')
		SELECT * FROM BranchMaster
		ELSE
		SELECT * FROM BranchMaster WHERE BranchId=@BranchId
		END
	IF(@Flag = 5)
		BEGIN
		--- SEARCH DATA IN THE ENTBOOKINGS
		SELECT * FROM EntBookings WHERE BranchId=@BranchId
		END
	IF(@Flag = 6)
		BEGIN
		--- DELETE DATA IN THE BRANCH MASTER
		DELETE FROM BranchMaster WHERE BranchId=@BranchId
		END
	IF(@Flag=7)
	Begin
	Select BranchName,BranchId from BranchMaster order by BranchId
	End
END












' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[prcTask]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
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
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_BookingPrevoiusDue]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'







/*
-- =============================================
-- Author:		<Karam Chand sharma>
-- Create date: <4-Jan-2012>
-- Description:	<Customer>
-- =============================================
EXEC Sp_Sel_CustomerAllPrevoiusDue ''CUST4''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_BookingPrevoiusDue]
	(		
		@CustCode VARCHAR(MAX) = '''',
		@Date DATETIME='''',
		@BN VARCHAR(MAX)='''',
		@BranchId VARCHAR(MAX)=''''
	)
AS
BEGIN
	CREATE TABLE #TmpBookReport (PaymentMade float)
    DECLARE @BookingNumber varchar(20), @PaymentMade float, @WholeAmt float,@AllreadyPaid float,@BNAmt float
	SET @WholeAmt=(SELECT SUM(NetAmount) AS Amount FROM dbo.EntBookings WHERE (BookingByCustomer = @CustCode) AND (BookingDeliveryDate < @Date) AND (BookingNumber <> @BN) AND BranchId=@BranchId)	
	SET @AllreadyPaid=(SELECT SUM(dbo.EntPayment.PaymentMade) AS Amount FROM dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber WHERE     (dbo.EntPayment.BookingNumber <> @BN) AND (dbo.EntBookings.BookingDeliveryDate <= @Date) AND (dbo.EntBookings.BookingByCustomer = @CustCode) AND(dbo.EntPayment.BranchId=@BranchId))
	IF(@AllreadyPaid IS NULL)
		SET @AllreadyPaid=''0''
	IF(@WholeAmt IS NULL)
		SET @WholeAmt=''0''
	SET @PaymentMade=@WholeAmt-@AllreadyPaid	
	SET @PaymentMade=@WholeAmt-@AllreadyPaid	
	INSERT INTO #TmpBookReport (PaymentMade) VALUES (@PaymentMade)
	SELECT PaymentMade AS DuePayment FROM #TmpBookReport
	DROP TABLE #TmpBookReport
END







' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_PrevoiusDue]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'







/*
-- =============================================
-- Author:		<Karam Chand sharma>
-- Create date: <4-Jan-2012>
-- Description:	<Customer>
-- =============================================
EXEC Sp_Sel_CustomerAllPrevoiusDue ''CUST4''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_PrevoiusDue]
	(		
		@CustCode VARCHAR(MAX) = '''',
		@Date DATETIME='''',
		@BN VARCHAR(MAX)='''',
		@BranchId VARCHAR(MAX)=''''
	)
AS
BEGIN
	CREATE TABLE #TmpBookReport (PaymentMade float)
    DECLARE @BookingNumber varchar(20), @PaymentMade float, @WholeAmt float,@AllreadyPaid float,@BNAmt float
	SET @WholeAmt=(SELECT SUM(NetAmount) AS Amount FROM dbo.EntBookings WHERE (BookingByCustomer = @CustCode) AND (BookingDeliveryDate < @Date) AND (BookingNumber <> @BN) AND BranchId=@BranchId)
	--SET @BNAmt=(SELECT SUM(NetAmount) AS Amount FROM dbo.EntBookings WHERE (BookingByCustomer = @CustCode) AND (BookingNumber = @BN))
	SET @AllreadyPaid=(SELECT SUM(dbo.EntPayment.PaymentMade) AS Amount FROM dbo.EntPayment INNER JOIN dbo.EntBookings ON dbo.EntPayment.BookingNumber = dbo.EntBookings.BookingNumber WHERE (dbo.EntPayment.BookingNumber <> @BN) AND (dbo.EntBookings.BookingDeliveryDate  < @Date) AND (BookingByCustomer = @CustCode) AND dbo.EntBookings.BranchId=@BranchId)
	IF(@AllreadyPaid IS NULL)
		SET @AllreadyPaid=''0''
	IF(@WholeAmt IS NULL)
		SET @WholeAmt=''0''
	SET @PaymentMade=@WholeAmt-@AllreadyPaid	
	SET @PaymentMade=@WholeAmt-@AllreadyPaid	
	INSERT INTO #TmpBookReport (PaymentMade) VALUES (@PaymentMade)
	SELECT PaymentMade AS DuePayment FROM #TmpBookReport
	DROP TABLE #TmpBookReport
END







' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UserMaster]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



-- =============================================
-- Author:		<Manoj Kumar Gupta>
-- Create date: <27-Feb-2012>
-- Description:	<User Master>
-- =============================================
CREATE PROCEDURE [dbo].[sp_UserMaster]	
	@Flag VARCHAR(MAX)='''',
	@UserId VARCHAR(MAX)='''',
	@UserPassword VARCHAR(MAX)='''',
	@UserTypeCode INT='''',
	@UserBranchCode VARCHAR(MAX)='''',
	@UserName VARCHAR(MAX)='''',
	@UserAddress VARCHAR(MAX)='''',
	@UserPhoneNumber VARCHAR(MAX)='''',
	@UserMobileNumber VARCHAR(MAX)='''',
	@UserEmailId VARCHAR(MAX)='''',
	@UserActive BIT='''',
	@BranchId VARCHAR(10)='''',
	@UpdatePassword BIT=''''
	
AS
BEGIN	
	IF(@Flag = 1)
		BEGIN
		INSERT INTO UserMaster VALUES (@UserId,@UserPassword,@UserTypeCode,@UserBranchCode,@UserName,@UserAddress,@UserPhoneNumber,@UserMobileNumber,@UserEmailId,@UserActive,@BranchId,'''')
		END
	IF(@Flag = 2)
		BEGIN
		IF(@UpdatePassword = ''TRUE'')		
		Update UserMaster Set UserTypeCode=@UserTypeCode, UserName=@UserName, UserAddress=@UserAddress, UserPhoneNumber=@UserPhoneNumber, UserMobileNumber=@UserMobileNumber, UserEmailId=@UserEmailId, UserActive=@UserActive, UserPassword=@UserPassword WHERE UserId=@UserId AND BranchId=@BranchId
		ELSE
		Update UserMaster Set UserTypeCode=@UserTypeCode, UserName=@UserName, UserAddress=@UserAddress, UserPhoneNumber=@UserPhoneNumber, UserMobileNumber=@UserMobileNumber, UserEmailId=@UserEmailId, UserActive=@UserActive WHERE UserId=@UserId AND BranchId=@BranchId
		END
	IF(@Flag = 3)
		BEGIN
		SELECT [UserId], [UserType], [UserName], [UserAddress], [UserPhoneNumber], [UserMobileNumber], [UserEmailId], [UserActive] FROM [UserMaster] INNER JOIN [UserTypeMaster] ON [UserMaster].[UserTypeCode]= [UserTypeMaster].[UserTypeId] WHERE UserMaster.BranchId=@BranchId
		END
	IF(@Flag = 4)
		BEGIN
		if(@UserName='''')
		SELECT [UserId], [UserType], [UserName], [UserAddress], [UserPhoneNumber], [UserMobileNumber], [UserEmailId], [UserActive] FROM [UserMaster] INNER JOIN [UserTypeMaster] ON [UserMaster].[UserTypeCode]= [UserTypeMaster].[UserTypeId] WHERE UserMaster.BranchId=@BranchId 
		else
		SELECT [UserId], [UserType], [UserName], [UserAddress], [UserPhoneNumber], [UserMobileNumber], [UserEmailId], [UserActive] FROM [UserMaster] INNER JOIN [UserTypeMaster] ON [UserMaster].[UserTypeCode]= [UserTypeMaster].[UserTypeId] WHERE UserMaster.BranchId=@BranchId AND UserMaster.UserName like ''%''+@UserName+''%'' 
		END	
	IF(@Flag = 5)
		BEGIN
		SELECT [UserId], [UserType], [UserName], [UserAddress], [UserPhoneNumber], [UserMobileNumber], [UserEmailId], [UserActive] FROM [UserMaster] INNER JOIN [UserTypeMaster] ON [UserMaster].[UserTypeCode]= [UserTypeMaster].[UserTypeId] WHERE UserMaster.UserId=@UserId AND UserMaster.BranchId=@BranchId
		END
END' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_CustomerAllPrevoiusDue]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'



/*
-- =============================================
-- Author:		<Manoj Gupta>
-- Create date: <27-Sep-2011>
-- Description:	<Customer>
-- =============================================
EXEC Sp_Sel_CustomerAllPrevoiusDue ''CUST4''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_CustomerAllPrevoiusDue]
	(		
		@CustCode varchar(10) = '''',
		@Date datetime=''''
	)
AS
BEGIN
--	declare @CustCode varchar(10)
--	set @CustCode=''CUST4''
	CREATE TABLE #TmpBookReport (BookingNumber varchar(20), BookingDate smalldatetime, NetAmount float, PaymentMade float, DuePayment float)
	INSERT INTO #TmpBookReport (BookingNumber, BookingDate, NetAmount) SELECT BookingNumber, BookingDate, NetAmount FROM EntBookings WHERE BookingByCustomer = @CustCode AND (BookingDate<@Date)

	DECLARE @BookingNumber varchar(20), @PaymentMade float, @DuePayment float,@TotalAmt float
	SET @BookingNumber = ''''
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



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_Sel_CustomerWiseBookingReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

/*
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
EXEC Sp_Sel_CustomerWiseBookingReport ''9/1/2011 00:00:00'',''10/1/2011 00:00:00'',''CUST1''
*/
CREATE PROCEDURE [dbo].[Sp_Sel_CustomerWiseBookingReport]
	(
		@BookDate1 datetime,
		@BookDate2 datetime,
		@CustCode varchar(10) = ''''
	)
AS
BEGIN
	CREATE TABLE #TmpBookReport (BookingNumber varchar(20), BookingDate varchar(max), NetAmount float, PaymentMade float, DuePayment float)
	INSERT INTO #TmpBookReport (BookingNumber, BookingDate, NetAmount) SELECT BookingNumber, convert(varchar,BookingDate,106), NetAmount FROM EntBookings WHERE BookingByCustomer = @CustCode AND (BookingDate BETWEEN @BookDate1 AND @BookDate2)

	DECLARE @BookingNumber varchar(20), @PaymentMade float, @DuePayment float
	SET @BookingNumber = ''''
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


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sp_InsUpd_ConfigSettings]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

-- =============================================
-- Author:		<Abhishek Verma>
-- Create date: <16 July 2010>
-- Description:	<To save config settings>
-- =============================================
CREATE PROCEDURE [dbo].[Sp_InsUpd_ConfigSettings]
	(
		@DefItemId int = '''',
		@DefMainProcessCode varchar(5) = '''',
		@DefExtraProcessCode varchar(5) = '''',
		@StartBookingNumberFrom int = '''',
		@CustomerMessage varchar(max)='''',
		@DefaultColorName varchar(max)='''',
		@DefaultTime varchar(max)='''',
		@DefaultAmPm varchar(10) = '''',
		@DeliveryDateOffset int='''',
		@DefaultSearchCriteria varchar(50)='''',
		@AmountType varchar(50)='''',
		@BranchId varchar(15)=''''
	)
AS
BEGIN
	IF EXISTS(SELECT * FROM MstConfigSettings where BranchId=@BranchId)
		BEGIN
			UPDATE MstConfigSettings SET DefaultItemId = @DefItemId, DefaultProcessCode = @DefMainProcessCode, DefaultExtraProcessCode = @DefExtraProcessCode, StartBookingNumberFrom = @StartBookingNumberFrom	, CustomerMessage = @CustomerMessage , DefaultColorName = @DefaultColorName , DefaultTime = @DefaultTime , DefaultAmPm = @DefaultAmPm , DeliveryDateOffset = @DeliveryDateOffset , DefaultSearchCriteria = @DefaultSearchCriteria,AmountType=@AmountType where BranchId=@BranchId
		END
	ELSE
		BEGIN
			INSERT INTO MstConfigSettings (StartBookingNumberFrom, DefaultItemId, DefaultProcessCode, DefaultExtraProcessCode, CustomerMessage,DefaultColorName,DefaultTime,DefaultAmPm,DeliveryDateOffset,DefaultSearchCriteria,AmountType,BranchId)
				VALUES (@StartBookingNumberFrom, @DefItemId, @DefMainProcessCode, @DefExtraProcessCode, @CustomerMessage,@DefaultColorName,@DefaultTime,@DefaultAmPm,@DeliveryDateOffset,@DefaultSearchCriteria,@AmountType,@BranchId)
		END	
END



' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ReceiptConfigSetting]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE proc [dbo].[sp_ReceiptConfigSetting]
(
	@HeaderText VARCHAR(40)='''',
	@HeaderFontName VARCHAR(MAX)='''',
	@HeaderFontSize VARCHAR(MAX)='''',
	@HeaderFontStyle VARCHAR(MAX)='''',
	@AddressText VARCHAR(70)='''',
	@AddressFontName VARCHAR(MAX)='''',
	@AddressFontSize VARCHAR(MAX)='''',
	@AddressFontStyle VARCHAR(MAX)='''',
	@LogoLeftRight BIT='''',
	@LogoHeight VARCHAR(MAX)='''',
	@LogoWeight VARCHAR(MAX)='''',
	@SloganText VARCHAR(100)='''',
	@SloganFontName VARCHAR(MAX)='''',
	@SloganFontSize VARCHAR(MAX)='''',
	@SloganFontStyle VARCHAR(MAX)='''',
	@Barcode BIT='''',
	@PreviewDue BIT='''',
	@Term1 VARCHAR(100)='''',
	@Term2 VARCHAR(100)='''',
	@Term3 VARCHAR(100)='''',
	@Term4 VARCHAR(100)='''',
	@FooterSloganText VARCHAR(100)='''',
	@FooterSloganFontName VARCHAR(MAX)='''',
	@FooterSloganFontSize VARCHAR(MAX)='''',
	@FooterSloganFontStyle VARCHAR(MAX)='''',
	@ST BIT='''',
	@Flag VARCHAR(MAX)='''',
	@PrinterLaserOrDotMatrix bit='''',
	@PrintLogoonReceipt bit='''',
	@PrePrinted bit='''',
	@TextFontStyleUL VARCHAR(MAX)='''',
	@TextFontItalic VARCHAR(MAX)='''',
	@TextAddressUL VARCHAR(MAX)='''',
	@TextAddressItalic VARCHAR(MAX)='''',
	@HeaderSloganUL VARCHAR(MAX)='''',
	@HeaderSloganItalic VARCHAR(MAX)='''',
	@FooterSloganUL VARCHAR(MAX)='''',
	@FooterSloganItalic VARCHAR(MAX)='''',
	@ShowHeaderSlogan bit='''',
	@ShowFooterSlogan bit='''',
	@TermsAndConditionTrue bit='''',
	@DotMatrixPrinter40Cloumn bit='''',
	@TableBorder bit='''',
	@NDB VARCHAR(MAX)='''',
	@NDI VARCHAR(MAX)='''',
	@NDU VARCHAR(MAX)='''',
	@NDFName VARCHAR(MAX)='''',
	@NDFSize VARCHAR(MAX)='''',
	@NAB VARCHAR(MAX)='''',
	@NAI VARCHAR(MAX)='''',
	@NAU VARCHAR(MAX)='''',
	@NAFName VARCHAR(MAX)='''',
	@NAFSize VARCHAR(MAX)='''',
	@CurrencyType VARCHAR(MAX)='''',
	@TimeZone VARCHAR(MAX)='''',
	@ImagePath VARCHAR(MAX)='''',
	@StoreCopy BIT='''',
	@ThermalPrinter BIT='''',
	@DotMatrixPrinter BIT='''',
	@ChallanType VARCHAR(MAX)='''',
	@HostPassword VARCHAR(MAX)='''',
	@HostName VARCHAR(MAX)='''',
	@BranchPassword VARCHAR(MAX)='''',
	@BranchEmail VARCHAR(MAX)='''',
	@SSL BIT='''',
	@BranchId VARCHAR(MAX)=''''
)
as
BEGIN
	IF(@Flag=1)
		IF EXISTS(SELECT * FROM mstReceiptConfig where BranchId=@BranchId)
			BEGIN
				UPDATE  mstReceiptConfig  SET HeaderText=@HeaderText,HeaderFontName=@HeaderFontName,HeaderFontSize=@HeaderFontSize,HeaderFontStyle=@HeaderFontStyle,AddressText=@AddressText,AddressFontName=@AddressFontName,AddressFontSize=@AddressFontSize,AddressFontStyle=@AddressFontStyle,LogoLeftRight=@LogoLeftRight,LogoHeight=@LogoHeight,LogoWeight=@LogoWeight,SloganText=@SloganText,SloganFontName=@SloganFontName,SloganFontSize=@SloganFontSize,SloganFontStyle=@SloganFontStyle,Barcode=@Barcode,PreviewDue=@PreviewDue,Term1=@Term1,Term2=@Term2,Term3=@Term3,Term4=@Term4,FooterSloganText=@FooterSloganText,FooterSloganFontName=@FooterSloganFontName,FooterSloganFontSize=@FooterSloganFontSize,FooterSloganFontStyle=@FooterSloganFontStyle,ST=@ST,PrinterLaserOrDotMatrix=@PrinterLaserOrDotMatrix,PrintLogoonReceipt=@PrintLogoonReceipt,PrePrinted=@PrePrinted,TextFontStyleUL=@TextFontStyleUL,TextFontItalic=@TextFontItalic,TextAddressUL=@TextAddressUL,TextAddressItalic=@TextAddressItalic,HeaderSloganUL=@HeaderSloganUL,HeaderSloganItalic=@HeaderSloganItalic,FooterSloganUL=@FooterSloganUL,FooterSloganItalic=@FooterSloganItalic,ShowHeaderSlogan=@ShowHeaderSlogan,ShowFooterSlogan=@ShowFooterSlogan,TermsAndConditionTrue=@TermsAndConditionTrue,DotMatrixPrinter40Cloumn=@DotMatrixPrinter40Cloumn,TableBorder=@TableBorder,NDB=@NDB,NDI=@NDI,NDU=@NDU,NDFName=@NDFName,NDFSize=@NDFSize,NAB=@NAB,NAI=@NAI,NAU=@NAU,NAFName=@NAFName,NAFSize=@NAFSize,CurrencyType=@CurrencyType,ImagePath=@ImagePath,StoreCopy=@StoreCopy,ThermalPrinter=@ThermalPrinter,DotMatrixPrinter=@DotMatrixPrinter where BranchId=@BranchId
			END
		ELSE
			BEGIN
				INSERT INTO mstReceiptConfig (HeaderText,HeaderFontName,HeaderFontSize,HeaderFontStyle,AddressText,AddressFontName,AddressFontSize,AddressFontStyle,LogoLeftRight,LogoHeight,LogoWeight,SloganText,SloganFontName,SloganFontSize,SloganFontStyle,Barcode,PreviewDue,Term1,Term2,Term3,Term4,FooterSloganText,FooterSloganFontName,FooterSloganFontSize,FooterSloganFontStyle,ST,PrinterLaserOrDotMatrix,PrintLogoonReceipt,PrePrinted,TextFontStyleUL,TextFontItalic,TextAddressUL,TextAddressItalic,HeaderSloganUL,HeaderSloganItalic,FooterSloganUL,FooterSloganItalic,ShowHeaderSlogan,ShowFooterSlogan,TermsAndConditionTrue,DotMatrixPrinter40Cloumn,TableBorder,NDB,NDI,NDU,NDFName,NDFSize,NAB,NAI,NAU,NAFName,NAFSize,CurrencyType,ImagePath,StoreCopy,ThermalPrinter,DotMatrixPrinter,BranchId) 
					VALUES(@HeaderText,@HeaderFontName,@HeaderFontSize,@HeaderFontStyle,@AddressText,@AddressFontName,@AddressFontSize,@AddressFontStyle,@LogoLeftRight,@LogoHeight,@LogoWeight,@SloganText,@SloganFontName,@SloganFontSize,@SloganFontStyle,@Barcode,@PreviewDue,@Term1,@Term2,@Term3,@Term4,@FooterSloganText,@FooterSloganFontName,@FooterSloganFontSize,@FooterSloganFontStyle,@ST,@PrinterLaserOrDotMatrix,@PrintLogoonReceipt,@PrePrinted,@TextFontStyleUL,@TextFontItalic,@TextAddressUL,@TextAddressItalic,@HeaderSloganUL,@HeaderSloganItalic,@FooterSloganUL,@FooterSloganItalic,@ShowHeaderSlogan,@ShowFooterSlogan,@TermsAndConditionTrue,@DotMatrixPrinter40Cloumn,@TableBorder,@NDB,@NDI,@NDU,@NDFName,@NDFSize,@NAB,@NAI,@NAU,@NAFName,@NAFSize,@CurrencyType,@ImagePath,@StoreCopy,@ThermalPrinter,@DotMatrixPrinter,@BranchId)		
			END	
	IF(@Flag=2)
		BEGIN
			SELECT * FROM mstReceiptConfig WHERE BranchId=@BranchId
		END
	IF(@Flag=3)
		IF EXISTS(SELECT * FROM mstReceiptConfig where BranchId=@BranchId)
			BEGIN
				UPDATE mstReceiptConfig SET TimeZone=@TimeZone where BranchId=@BranchId
			END
		ELSE
			BEGIN
				INSERT INTO mstReceiptConfig (TimeZone,BranchId) VALUES (@TimeZone,@BranchId)
			END	
	IF(@Flag=4)
		IF EXISTS(SELECT * FROM mstReceiptConfig where BranchId=@BranchId)
			BEGIN
				UPDATE mstReceiptConfig SET ChallanType=@ChallanType where BranchId=@BranchId
			END
		ELSE
			BEGIN
				INSERT INTO mstReceiptConfig (ChallanType,BranchId) VALUES (@ChallanType,@BranchId)
			END	
	IF(@Flag=5)
		IF EXISTS(SELECT * FROM mstReceiptConfig where BranchId=@BranchId)
			BEGIN
				UPDATE mstReceiptConfig SET HostName=@HostName , BranchEmail=@BranchEmail , SSL=@SSL where BranchId=@BranchId				
				IF(@BranchPassword<>'''')
					UPDATE mstReceiptConfig SET BranchPassword=@BranchPassword where BranchId=@BranchId
			END
		ELSE
			BEGIN
				INSERT INTO mstReceiptConfig (HostName,BranchEmail,BranchPassword,BranchId) VALUES (@HostName,@BranchEmail,@BranchPassword,@BranchId)
			END	
END



' 
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_mstVendor_mstJobType]') AND parent_object_id = OBJECT_ID(N'[dbo].[mstVendor]'))
ALTER TABLE [dbo].[mstVendor]  WITH NOCHECK ADD  CONSTRAINT [FK_mstVendor_mstJobType] FOREIGN KEY([JobType])
REFERENCES [dbo].[mstJobType] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[mstVendor] CHECK CONSTRAINT [FK_mstVendor_mstJobType]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BarcodeTable_EntBookings]') AND parent_object_id = OBJECT_ID(N'[dbo].[BarcodeTable]'))
ALTER TABLE [dbo].[BarcodeTable]  WITH CHECK ADD  CONSTRAINT [FK_BarcodeTable_EntBookings] FOREIGN KEY([BookingNo])
REFERENCES [dbo].[EntBookings] ([BookingNumber])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BarcodeTable] CHECK CONSTRAINT [FK_BarcodeTable_EntBookings]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EntPayment_EntBookings]') AND parent_object_id = OBJECT_ID(N'[dbo].[EntPayment]'))
ALTER TABLE [dbo].[EntPayment]  WITH CHECK ADD  CONSTRAINT [FK_EntPayment_EntBookings] FOREIGN KEY([BookingNumber])
REFERENCES [dbo].[EntBookings] ([BookingNumber])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EntPayment] CHECK CONSTRAINT [FK_EntPayment_EntBookings]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EntBookingDetails_EntBookings]') AND parent_object_id = OBJECT_ID(N'[dbo].[EntBookingDetails]'))
ALTER TABLE [dbo].[EntBookingDetails]  WITH CHECK ADD  CONSTRAINT [FK_EntBookingDetails_EntBookings] FOREIGN KEY([BookingNumber])
REFERENCES [dbo].[EntBookings] ([BookingNumber])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EntBookingDetails] CHECK CONSTRAINT [FK_EntBookingDetails_EntBookings]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EntVendorChallan_mstVendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[EntVendorChallan]'))
ALTER TABLE [dbo].[EntVendorChallan]  WITH CHECK ADD  CONSTRAINT [FK_EntVendorChallan_mstVendor] FOREIGN KEY([VendorId])
REFERENCES [dbo].[mstVendor] ([ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[EntVendorChallan] CHECK CONSTRAINT [FK_EntVendorChallan_mstVendor]
