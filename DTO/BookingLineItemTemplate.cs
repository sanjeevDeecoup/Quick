﻿namespace DTO
{
    public static class BookingLineItemTemplate
    {
        public static string GetString()
        {
            return _template;
        }

        private static string _template = "<lineItem sequence=\"1\">"
                                            + "<item id=\"0\" identity=\"0\">"
                                                + "<quantity></quantity>"
                                                + "<length></length>"
                                                + "<breadth></breadth>"
                                                + "<area1></area1>"
                                                + "<remarks></remarks>"
                                                + "<text></text>"
                                            + "</item>"
                                            + "<patterns>"
                                                + "<pattern id=\"0\" identity=\"0\">"
                                                    + "<text></text>"
                                                + "</pattern>"
                                            + "</patterns>"
                                            + "<colors>"
                                                + "<color id=\"0\" identity=\"0\">"
                                                    + "<text></text>"
                                                + "</color>"
                                            + "</colors>"
                                            + "<subItems>"
                                                + "<subItem id=\"0\" identity=\"0\">"
                                                    + "<text></text>"
                                                + "</subItem>"
                                            + "</subItems>"
                                            + "<categories>"
                                                + "<category id=\"0\" identity=\"0\">"
                                                    + "<text></text>"
                                                + "</category>"
                                            + "</categories>"
                                            + "<brands>"
                                                + "<brand id=\"0\" identity=\"0\">"
                                                    + "<text></text>"
                                                + "</brand>"
                                            + "</brands>"
                                            + "<variations>"
                                                + "<variation id=\"0\" identity=\"0\">"
                                                    + "<text></text>"
                                                + "</variation>"
                                            + "</variations>"
                                            + "<comments>"
                                                + "<comment id=\"0\" identity=\"0\">"
                                                    + "<text></text>"
                                                + "</comment>"
                                            + "</comments>"
                                            + "<processes>"
                                                + "<process id=\"0\" identity=\"0\">"
                                                    + "<rate></rate>"
                                                    + "<amount></amount>"
                                                    + "<text></text>"
                                                    + "<isdiscount></isdiscount>"
                                                    + "<tax></tax>"
                                                + "</process>"
                                            + "</processes>"
                                        + "</lineItem>";
    }
}