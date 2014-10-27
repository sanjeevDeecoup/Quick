using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAL
{
    public class BALFactory
    {
        private BALFactory() { }
        private static BALFactory _instance;

        public static BALFactory Instance
        {
            get { if (_instance == null) { _instance = new BALFactory(); } return _instance; }
        }
        public SchemeBAO SchemeBAO
        {
            get { return new SchemeBAO(); }
        }
        public ProcessBAO ProcessBAO
        {
            get { return new ProcessBAO(); }
        }
        public ItemBAO ItemBAO
        {
            get { return new ItemBAO(); }
        }
        public CustomerBAO CustomerBAO
        {
            get { return new CustomerBAO(); }
        }
    }
}
