using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace appServices
{
    [ServiceContract]
    public interface IPromoSchemeService
    {
        [OperationContract]
        string GetAllTemplatesData();
    }
}
