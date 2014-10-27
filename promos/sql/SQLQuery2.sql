drop table PID_RAW_Data
drop table PID_Data

select * from PID_Raw_Data
select * from PID_Data

exec usp_AddPIDInPID_Raw_Data 'D:\Invoicing-Operations\TestData\GM CAD\Input Files\78326.tsv'

update PID_Raw_Data set PID = null

select * from [TempTextData]

drop table SystemConfigurations

select * from PDFInvoiceData

select * from PreBilledExpenses_LandedData

DECLARE @dtDate DATETIME
SET @dtDate = '02/8/2007'
SELECT DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@dtDate)+1,0))

select DATENAME(YYYY, '02/01/2012')
	
	DECLARE @dtDate DATETIME, @retVal int, @tempDate datetime, @TestDate datetime
	SET @TestDate = '02/28/2012'
	SET @dtDate = CONVERT(datetime, @TestDate)
	set @tempDate = DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@dtDate)+1,0))
	print @tempdate
	print @dtDate
	print DATEDIFF(day, @dtDate, @tempDate)
	if (DATEDIFF(DD, @dtDate, @tempDate) = 0)
		set @retVal = 1
	else
		set @retVal =0
		
		select * from Time_Tracking_Data
		
if CONVERT(datetime, '01 Feb 2012') = CONVERT(datetime, '02/01/2012')
	print 1
else
	print 0
	
	select * into tmp1 from PreBilledExpenses_ShiftData
	select * from PreBilledExpenses_LandedData
	
	
select * from PID_Data

select Datename(YEAR, '02/29/2012')
		
						
select * from tmpSplitShiftDates

select data from dbo.Split('01/02/2012', ',')

select ISDATE('24-Feb')

select * from Time_Tracking_Data where Oracle_ID =  '81621'
select * from PID_Data


exec usp_UpdatePreBilledData

select * from PreBilledExpenses_ShiftData
select * from PreBilledExpenses_LandedData

select * from PreBilledExpenses_ShiftDataDetails

select * from PID_Data

78326

select * from OutputInvoiceHeader

select * from PDFInvoiceData

select * from tempTextData

exec usp_ExtractHeaderInformationFromFixedExpense

select * from PreBilledExpenses_LandedData where [project PID] = '78326'


--check for valid data


	declare @PID varchar(10), @Offset varchar(1000), @PIDRef varchar(1000), @tempChar varchar(1000)
	--Get PID
	select @offset = value2, @PIDRef = value1, @tempChar = value3 from SystemConfigurations where col1 = 'Fixed' and col2 = 'PIDRef'
	select @offset, @PIDRef, @tempChar
	select TempID + CONVERT(int, @Offset) from TempTextData where LineItem like '%' +  @PIDRef + '%'
	select @PID = Replace(LineItem, @tempChar, '') from TempTextData where TempID = (select top 1 TempID + CONVERT(int, @Offset) 
																		from TempTextData where LineItem  like '%' +  @PIDRef + '%')
	select @PID
	select @offset = value2, @PIDRef = value1 from SystemConfigurations 
															where col1 = 'Fixed' and col2 = 'InvoiceNumberRef'

		select @PID, 'FIXED', 'INVOICE NUMBER', LineItem from TempTextData where TempID = 
				(select TempID + CONVERT(int, @Offset) from TempTextData where LineItem = @PIDRef)
	
	--get Invoice Number
	select @offset = value2, @PIDRef = value1 from SystemConfigurations 



exec usp_UpdatePreBilledData_LandedData
	
exec usp_UpdatePreBilledData_ShiftData

select * from PID_Data
select * from Time_Tracking_data
--Check Data


select * from PID_Data where PID = '78326'
--check time tracking has data for months in shift data
--Landed data format checks
--shift data format checks



select distinct l.PID, (case when t.PersonMonth is null then l.PersonMonth END) as MissingMonth
			from (select distinct [Project PID] as PID, convert(varchar(3), DateName(month, [Month])) + ' ' + DateName(year, [Month]) as PersonMonth from PreBilledExpenses_LandedData where [Project PID] in (select distinct PID from PID_Data)) l
			left outer join 
			(select distinct PID, convert(varchar(3), DATENAME(MONTH, [day])) + ' ' + DATENAME(YEAR, [day]) as PersonMonth from Time_Tracking_Data 
				where PID in (select distinct PID from PID_Data)) t
				on l.PID = t.PID and l.PersonMonth = t.PersonMonth
			where (case when t.PersonMonth is null then l.PersonMonth END) is not null
			
			
select distinct NAME from Time_Tracking_data where PID = '78326'

select * from ErrorLog

select * from PreBilledExpenses_ShiftDataDetails

select SUM(TotalAmount) from OutputInvoiceDetails
select SUM(InvoiceAmount) from OutputInvoiceHeader
select * from OutputInvoiceHeader
select * from OutputInvoiceDetails order by PersonName

select * from OutputInvoiceDetailsTemp
exec usp_MatchOutputInvoiceTotals

select * from OutputInvoiceDetails

select * from ErrorLog

select * from Pre

update OutputInvoiceDetails set PersonCurrency = p.PersonCurrency 
from (select top 1 PersonCurrency, PID from PID_Data) p inner join OutputInvoiceDetails o
on o.PID = p.PID where ExpenseType = 3 and o.PersonCurrency = ''


select * from PID_Raw_Data
select * from PID_Data

select ISDATE('27 Feb 2012')
select DATEName(WeekDay, convert(datetime, 'Feb 19, 2012'))

select * from  dbo.func_GetDescriptionValues('Garimella, Venkata Surya | Month Ending Days 27-29 February | 24 @ 30 USD /hour	Standard')

update PID_Data set PersonMonth = a.PersonMonth, WeekStartDate = WeekStartDate + ' ' + a.PersonMonth, WeekEndDate = WeekEndDate + ' ' + a.PersonMonth
		from (Select PerSonMonth, PID, SName, FName from PID_Data) a inner join PID_Data b
		on a.PID = b.PID and a.FName = b.FName and a.SName = b.SName
		where b.PersonMonth is null 
									
									
select * from PDFINvoicedata	

select * from ErrorLogs	

select distinct Name from Time_Tracking_Data where PID = '75824'

select distinct FName + ' ' + SName from PID_Data

select *from PID_Data

select * from time_tracking_data



--select * from dbo.GetWeeks('01-Feb-2012', '11-Feb-2012')

select * from PID_Data

--update PID_Data set SName = 'Babu', FName = 'Sai' where PIDID <=5

--select * from PreBilledExpenses_ShiftDataDetails where Name = 'Sai Babu'

--update PreBilledExpenses_LandedData set [T

ravel Start Date] = '02/10/2012' where LandedID = 42

select Name + ' | ' + DescType + ' | ' + [Month] + ' | ' + convert(varchar(100), [Hours]) + ' @ ' + convert(varchar(100), Rate) + ' ' + Currency + ' / hour | PID ' + PID
from OutputInvoiceHeader

select Description from OutputInvoiceHeader

select Name, SUM(InvoiceAmount) from OutputInvoiceHeader group by Name
select PersonName, SUM(TotalAmount) from OutputInvoiceDetails group by PersonName

select * from OutputInvoiceDetails
select * from OutputInvoiceHeader

select * from ErrorLog

select * from PID_Data

if (select top 1 WeekDesc from PID_Data where PID = '75824') is NOT NULL
select SUM(convert(float, PersonHours)) from PID_Data where PID = '75824' and 
(FName + ' ' + SName = 'Sai Babu' OR TimeTrackingName = 'Sai Babu')
else
select convert(float, PersonHours) from PID_Data where PID = '75824' and (FName + ' ' + SName = 'Sai Babu' OR TimeTrackingName = 'Sai Babu')
and convert(datetime, '01 ' + PersonMonth) = convert(datetime, '01-February-2012')

