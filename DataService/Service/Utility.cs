using DataService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Service
{
    //Class này để xử lý lặt vặt như format...
    public class Utility
    {
        public static string FormatVND(decimal money)
        {
            return money.ToString("#,##0.##")+" VND";
        }

        public static string CreateAttributeString(int productId)
        {
            ProductRepository pRepo = new ProductRepository();

            TemplateProduct master = pRepo.GetTemplateProduct(productId);
            Product detailed = pRepo.GetProduct(productId);
            string output = detailed.Name + " ";

            if (detailed.TemplateId != null)
            {
                if (master.Attr1 != null)
                {
                    output += master.Attr1 + " " + detailed.Attr1;
                }
                if (master.Attr1 == null && detailed.Attr1 != null)
                {
                    output += detailed.Attr1;
                }

                if (master.Attr2 != null)
                {
                    output += ", " + master.Attr2 + " " + detailed.Attr2;
                }
                if (master.Attr2 == null && detailed.Attr2 != null)
                {
                    output += ", " + detailed.Attr2;
                }

                if (master.Attr3 != null)
                {
                    output += ", " + master.Attr3 + " " + detailed.Attr3;
                }
                if (master.Attr3 == null && detailed.Attr3 != null)
                {
                    output += ", " + detailed.Attr3;
                }

                if (master.Attr4 != null)
                {
                    output += ", " + master.Attr4 + " " + detailed.Attr4;
                }
                if (master.Attr4 == null && detailed.Attr4 != null)
                {
                    output += ", " + detailed.Attr4;
                }

                if (master.Attr5 != null)
                {
                    output += ", " + master.Attr5 + " " + detailed.Attr5;
                }
                if (master.Attr5 == null && detailed.Attr5 != null)
                {
                    output += ", " + detailed.Attr5;
                }

                if (master.Attr6 != null)
                {
                    output += ", " + master.Attr6 + " " + detailed.Attr6;
                }
                if (master.Attr6 == null && detailed.Attr6 != null)
                {
                    output += ", " + detailed.Attr6;
                }

                if (master.Attr7 != null)
                {
                    output += ", " + master.Attr7 + " " + detailed.Attr7;
                }
                if (master.Attr7 == null && detailed.Attr7 != null)
                {
                    output += ", " + detailed.Attr7;
                }
            }
            return output;
        }

    }
}
