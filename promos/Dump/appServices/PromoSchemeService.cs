using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAL;

namespace appServices
{
    public class PromoSchemeService : IPromoSchemeService
    {
        #region IPromoSchemeService Members

        public string GetAllTemplatesData()
        {
            return BALFactory.Instance.SchemeBAO.GetAllTemplatesData();
        }

        #endregion
    }
}
