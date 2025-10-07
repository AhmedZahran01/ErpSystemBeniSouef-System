using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.HelperFunctions
{
    public static class AppGlobalCompanyId
    {
        public static int CompanyId { get; set; }

        public static List<ReturnTypeComBoxData> CompanyName()
        {
            ReturnTypeComBoxData dataValue1 = new ReturnTypeComBoxData()
            {
                TypeId = 3,
                TypeName = "جديد "
            };
            ReturnTypeComBoxData dataValue2 = new ReturnTypeComBoxData()
            {
                TypeId = 4,
                TypeName = "تالف "
            };
            List<ReturnTypeComBoxData> data = new List<ReturnTypeComBoxData>()
                { dataValue1, dataValue2 };

            return data;
        }

    }

    public class ReturnTypeComBoxData
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }

    }

}
