using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class DALFactory
    {
        private DALFactory() { }
        private static DALFactory _instance;
        public static DALFactory Instance
        {
            get { if (_instance == null) { _instance = new DALFactory(); } return _instance; }
        }
        public SchemeDAO SchemeDAO
        {
            get { return new SchemeDAO(); }
        }
        public ProcessDAO ProcessDAO
        {
            get { return new ProcessDAO(); }
        }
        public ItemDAO ItemDAO
        {
            get { return new ItemDAO(); }
        }
        public CustomerDAO CustomerDAO
        {
            get { return new CustomerDAO(); }
        }
    }
}
