namespace DTO
{
    public static class BookingReceiptHeaderTemplate
    {
        public static string GetString()
        {
            return _template;
        }

        private static string _template = "<receiptheader identity=\"0\">" +
                                            "<iswalkin>" +
                                                "<bookingnumber></bookingnumber>" +
                                            "</iswalkin>" +
                                            "<ishomebooking>" +
                                                "<homeeceiptnumber></homeeceiptnumber>" +
                                            "</ishomebooking>" +
                                            "<customerid></customerid>" +
                                            "<duedate></duedate>" +
                                            "<duetime></duetime>" +
                                            "<isurgent></isurgent>" +
                                            "<issms></issms>" +
                                            "<isemail></isemail>" +
                                            "<remarks></remarks>" +
                                            "<salesman></salesman>" +
                                            "<checkedby></checkedby>" +
                                            "<quantity></quantity>" +
                                            "<totalgrossamount></totalgrossamount>" +
                                            "<totaldiscount></totaldiscount>" +
                                            "<totaltax></totaltax>" +
                                            "<totaladvance></totaladvance>" +
                                        "</receiptheader>";
    }
}