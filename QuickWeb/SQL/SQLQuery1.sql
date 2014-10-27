
------------------Add field  ConfigurationSetting table-----------------

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodebookingno' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodebookingno bit default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='bookingfont' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add bookingfont varchar(50)		
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='bookingsize' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add bookingsize varchar(50)				
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='bookingalign' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add bookingalign varchar(50) 
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='bookingbold' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add bookingbold varchar(50)
GO
			
if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='bookingitilic' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add bookingitilic varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='bookingunderline' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add bookingunderline varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodeprocess' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodeprocess bit default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodexteraprocess' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodexteraprocess BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodeextraprocesssecond' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodeextraprocesssecond BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodesubtotal' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodesubtotal BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='processfont' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add processfont varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='processsize' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add processsize varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='processalign' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add processalign varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='processbold' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add processbold varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='processitalic' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add processitalic varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='processunderline' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add processunderline varchar(50) 
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcoderemark' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcoderemark BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodecolour' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodecolour BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='remarkfont' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add remarkfont varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='remarksize' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add remarksize varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='remarkremarkalign' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add remarkremarkalign varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='remarkbold' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add remarkbold varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='remarkitalic' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add remarkitalic varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='remarkunderline' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add remarkunderline varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodeprint' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodeprint BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodesize' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodesize varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodealign' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodealign varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodeitem' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodeitem BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodeduedate' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodeduedate BIT default('True')
GO
if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodetime' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodetime BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemfont' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add itemfont varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemsize' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add itemsize varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemalign' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add itemalign varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itembold' and o.name='mstReceipt			Config'  )
	alter table ConfigurationSetting add itembold varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemitalic' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add itemitalic varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemunderline' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add itemunderline varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodecusname' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodecusname BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='cusfont' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add cusfont varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='cussize' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add cussize varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='cusalign' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add cusalign varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='cusbold' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add cusbold varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='cusitalic' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add cusitalic varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='cusunderline' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add cusunderline varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='cusposition' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add cusposition varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='bookingnoposition' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add bookingnoposition varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='processposition' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add processposition varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='remarkposition' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add remarkposition varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodeposition' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodeposition varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemposition' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add itemposition varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='Addressposition' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add Addressposition varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodeaddress' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodeaddress BIT default('True')
GO
	
if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='addfont' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add addfont varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='addsize' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add addsize varchar(50)
GO


if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='addalign' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add addalign varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='addbold' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add addbold varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='additalic' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add additalic varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='addunderline' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add addunderline varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodedivider' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodedivider BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodewidth' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodewidth VARCHAR(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodeheight' and o.name='ConfigurationSetting'  )
	alter table ConfigurationSetting add barcodeheight VARCHAR(50)
GO
-------------mstReceiptConfig add field----------------------------


if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodebookingno' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodebookingno bit default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='bookingfont' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add bookingfont varchar(50)		
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='bookingsize' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add bookingsize varchar(50)				
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='bookingalign' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add bookingalign varchar(50) 
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='bookingbold' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add bookingbold varchar(50)
GO
			
if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='bookingitilic' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add bookingitilic varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='bookingunderline' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add bookingunderline varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodeprocess' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodeprocess bit default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodexteraprocess' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodexteraprocess BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodeextraprocesssecond' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodeextraprocesssecond BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodesubtotal' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodesubtotal BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='processfont' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add processfont varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='processsize' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add processsize varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='processalign' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add processalign varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='processbold' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add processbold varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='processitalic' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add processitalic varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='processunderline' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add processunderline varchar(50) 
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcoderemark' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcoderemark BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodecolour' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodecolour BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='remarkfont' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add remarkfont varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='remarksize' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add remarksize varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='remarkremarkalign' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add remarkremarkalign varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='remarkbold' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add remarkbold varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='remarkitalic' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add remarkitalic varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='remarkunderline' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add remarkunderline varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodeprint' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodeprint BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodesize' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodesize varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodealign' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodealign varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodeitem' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodeitem BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodeduedate' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodeduedate BIT default('True')
GO
if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodetime' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodetime BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemfont' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add itemfont varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemsize' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add itemsize varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemalign' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add itemalign varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itembold' and o.name='mstReceipt			Config'  )
	alter table mstReceiptConfig add itembold varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemitalic' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add itemitalic varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemunderline' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add itemunderline varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodecusname' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodecusname BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='cusfont' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add cusfont varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='cussize' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add cussize varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='cusalign' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add cusalign varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='cusbold' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add cusbold varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='cusitalic' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add cusitalic varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='cusunderline' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add cusunderline varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='cusposition' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add cusposition varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='bookingnoposition' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add bookingnoposition varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='processposition' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add processposition varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='remarkposition' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add remarkposition varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodeposition' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodeposition varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemposition' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add itemposition varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='Addressposition' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add Addressposition varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodeaddress' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodeaddress BIT default('True')
GO
	
if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='addfont' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add addfont varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='addsize' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add addsize varchar(50)
GO


if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='addalign' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add addalign varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='addbold' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add addbold varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='additalic' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add additalic varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='addunderline' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add addunderline varchar(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodedivider' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodedivider BIT default('True')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodewidth' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodewidth VARCHAR(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='barcodeheight' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add barcodeheight VARCHAR(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='backupdrive' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add backupdrive VARCHAR(50)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='backuppath' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add backuppath VARCHAR(100)
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='ReconciliationStatus' and o.name='BarcodeTable'  )
	alter table BarcodeTable add ReconciliationStatus bit default('False')
GO

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='ReconcileStatus' and o.name='BarcodeTable'  )
	alter table BarcodeTable add ReconcileStatus bit default('True')
GO

------- Puneet Field add in the barcode purpose

if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemsize1' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add itemsize1 varchar(50) default('1')
GO
if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemalign1' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add itemalign1 varchar(50)
GO
if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemfont1' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add itemfont1 varchar(50)
GO
if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itembold1' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add itembold1 varchar(50)
GO
if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemitalic1' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add itemitalic1 varchar(50)
GO
if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemunderline1' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add itemunderline1 varchar(50)
GO
if NOT EXISTS (SELECT * FROM sys.columns c inner join sys.objects o on c.object_id = o.object_id 
where c.name='itemposition1' and o.name='mstReceiptConfig'  )
	alter table mstReceiptConfig add itemposition1 varchar(50) default('1')
GO




if NOT EXISTS (select * from EntMenuRights where PageTitle='Print Sticker' and BranchId='1' and UserTypeId='1'  )
	INSERT INTO [DRYSOFT].[dbo].[EntMenuRights]([UserTypeId] ,[PageTitle],[FileName] ,[RightToView] ,[MenuItemLevel],[MenuPosition] ,[ParentMenu] ,[BranchId],[ExternalBranchId])
 VALUES(1,'Print Sticker','~/Bookings/CreateLabels.aspx','True','2','16','Bookings','1','NULL')
GO

if NOT EXISTS (select * from EntMenuRights where PageTitle='Print Sticker' and BranchId='1' and UserTypeId='2'  )
	INSERT INTO [DRYSOFT].[dbo].[EntMenuRights]([UserTypeId] ,[PageTitle],[FileName] ,[RightToView] ,[MenuItemLevel],[MenuPosition] ,[ParentMenu] ,[BranchId],[ExternalBranchId])
 VALUES(2,'Print Sticker','~/Bookings/CreateLabels.aspx','True','2','16','Bookings','1','NULL')
GO

if NOT EXISTS (select * from EntMenuRights where PageTitle='Print Sticker' and BranchId='1' and UserTypeId='3'  )
	INSERT INTO [DRYSOFT].[dbo].[EntMenuRights]([UserTypeId] ,[PageTitle],[FileName] ,[RightToView] ,[MenuItemLevel],[MenuPosition] ,[ParentMenu] ,[BranchId],[ExternalBranchId])
 VALUES(3,'Print Sticker','~/Bookings/CreateLabels.aspx','True','2','16','Bookings','1','NULL')
GO

if NOT EXISTS (select * from EntMenuRights where PageTitle='Barcode Configurator' and BranchId='1' and UserTypeId='1'  )
	INSERT INTO [DRYSOFT].[dbo].[EntMenuRights]([UserTypeId] ,[PageTitle],[FileName] ,[RightToView] ,[MenuItemLevel],[MenuPosition] ,[ParentMenu] ,[BranchId],[ExternalBranchId])
 VALUES(1,'Barcode Configurator','~/Config Setting/configBarcode.aspx','True','2','8','Admin',1,'NULL')
GO

if NOT EXISTS (select * from EntMenuRights where PageTitle='Barcode Configurator' and BranchId='1' and UserTypeId='2'  )
	INSERT INTO [DRYSOFT].[dbo].[EntMenuRights]([UserTypeId] ,[PageTitle],[FileName] ,[RightToView] ,[MenuItemLevel],[MenuPosition] ,[ParentMenu] ,[BranchId],[ExternalBranchId])
 VALUES(2,'Barcode Configurator','~/Config Setting/configBarcode.aspx','True','2','8','Admin',1,'NULL')
GO

if NOT EXISTS (select * from EntMenuRights where PageTitle='Barcode Configurator' and BranchId='1' and UserTypeId='3'  )
	INSERT INTO [DRYSOFT].[dbo].[EntMenuRights]([UserTypeId] ,[PageTitle],[FileName] ,[RightToView] ,[MenuItemLevel],[MenuPosition] ,[ParentMenu] ,[BranchId],[ExternalBranchId])
 VALUES(3,'Barcode Configurator','~/Config Setting/configBarcode.aspx','True','2','8','Admin',1,'NULL')
GO
if NOT EXISTS (select * from EntMenuRights where PageTitle='Sticker Configurator' and BranchId='1' and UserTypeId='1'  )
	INSERT INTO [DRYSOFT].[dbo].[EntMenuRights]([UserTypeId] ,[PageTitle],[FileName] ,[RightToView] ,[MenuItemLevel],[MenuPosition] ,[ParentMenu] ,[BranchId],[ExternalBranchId])
 VALUES(1,'Sticker Configurator','~/Config Setting/stickersetting.aspx','True','2','6','Admin','1','NULL')
GO

if NOT EXISTS (select * from EntMenuRights where PageTitle='Sticker Configurator' and BranchId='1' and UserTypeId='2'  )
	INSERT INTO [DRYSOFT].[dbo].[EntMenuRights]([UserTypeId] ,[PageTitle],[FileName] ,[RightToView] ,[MenuItemLevel],[MenuPosition] ,[ParentMenu] ,[BranchId],[ExternalBranchId])
 VALUES(2,'Sticker Configurator','~/Config Setting/stickersetting.aspx','True','2','6','Admin','1','NULL')
GO


--------------------------------------Add Smsconfig table--------------------------------------------
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id =     
	OBJECT_ID(N'[dbo].[Smsconfig]') AND type in (N'U'))
Create table Smsconfig
    (
    [smsid] [int] IDENTITY(1,1) NOT NULL,
	[template] [varchar](150)  NULL,
	[massage] [varchar](170)  NULL,
	[Active] [tinyint] NOT NULL CONSTRAINT [DF_Smsconfig_Active]  DEFAULT ((1)),
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_Smsconfig_DateCreated]  DEFAULT (getdate()),
	[DateModified] [datetime] NOT NULL CONSTRAINT [DF_Smsconfig_DateModified]  DEFAULT (getdate()),
	[BranchId] [varchar](50) NOT NULL,
	[ExternalBranchId] [varchar](max)  NULL
GO
if NOT EXISTS (select * from EntMenuRights where PageTitle='Sticker Configurator' and BranchId='1' and UserTypeId='3'  )
	INSERT INTO [DRYSOFT].[dbo].[EntMenuRights]([UserTypeId] ,[PageTitle],[FileName] ,[RightToView] ,[MenuItemLevel],[MenuPosition] ,[ParentMenu] ,[BranchId],[ExternalBranchId])
 VALUES(1,'SMS Configuration ','~/Config Setting/smsconfig.aspx','True','2','9','Admin','1','NULL')
GO
if NOT EXISTS (select * from EntMenuRights where PageTitle='Sticker Configurator' and BranchId='1' and UserTypeId='3'  )
	INSERT INTO [DRYSOFT].[dbo].[EntMenuRights]([UserTypeId] ,[PageTitle],[FileName] ,[RightToView] ,[MenuItemLevel],[MenuPosition] ,[ParentMenu] ,[BranchId],[ExternalBranchId])
 VALUES(2,'SMS Configuration ','~/Config Setting/smsconfig.aspx','True','2','9','Admin','1','NULL')
Go
if NOT EXISTS (select * from EntMenuRights where PageTitle='Sticker Configurator' and BranchId='1' and UserTypeId='3'  )
	INSERT INTO [DRYSOFT].[dbo].[EntMenuRights]([UserTypeId] ,[PageTitle],[FileName] ,[RightToView] ,[MenuItemLevel],[MenuPosition] ,[ParentMenu] ,[BranchId],[ExternalBranchId])
 VALUES(3,'SMS Configuration ','~/Config Setting/smsconfig.aspx','True','2','9','Admin','1','NULL')
Go



