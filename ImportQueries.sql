/*If case of Multiple Bookings */

delete from BarCodeTable where BookingNo between 13 And 16
delete from EntBookings where BookingNumber between 13 And 16
delete from EntBookingDetails where BookingNumber between 13 And 16
delete from EntPayment where BookingNumber between 13 and 16
/*EntLedgerEntries one by one Deleted */
delete from EntLedgerEntries where Narration='On Credit New Booking Number 16'

/* Delete Single Bookings */

delete from BarCodeTable where BookingNo='2073'
delete from EntBookings where BookingNumber='2073'
delete from EntBookingDetails where BookingNumber='2073'
delete from EntPayment where BookingNumber='2073'
delete from EntLedgerEntries where Narration='On Credit New Booking Number 1'
