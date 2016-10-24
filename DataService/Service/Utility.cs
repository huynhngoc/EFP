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
            string output = detailed.Name;

            if (detailed.TemplateId != null)
            {
                if (master.Attr1 != null && master.Attr1.Trim().Length >0)
                {
                    if (detailed.Attr1 != null && detailed.Attr1.Trim().Length > 0)
                    {
                        output += ", " + master.Attr1 + " " + detailed.Attr1;
                    }
                    
                }
                //if ((master.Attr1 == null || master.Attr1.Trim().Length == 0) && detailed.Attr1 != null)
                //{
                //    output += detailed.Attr1;
                //}

                //if (master.Attr2 != null)
                //{
                //    output += ", " + master.Attr2 + " " + detailed.Attr2;
                //}
                //if (master.Attr2 == null && detailed.Attr2 != null)
                //{
                //    output += ", " + detailed.Attr2;
                //}
                if (master.Attr2 != null && master.Attr2.Trim().Length > 0)
                {
                    if (detailed.Attr2 != null && detailed.Attr2.Trim().Length > 0)
                    {
                        output += ", " + master.Attr2 + " " + detailed.Attr2;
                    }

                }

                if (master.Attr3 != null && master.Attr3.Trim().Length > 0)
                {
                    if (detailed.Attr3 != null && detailed.Attr3.Trim().Length > 0)
                    {
                        output += ", " + master.Attr3 + " " + detailed.Attr3;
                    }

                }

                if (master.Attr4 != null && master.Attr4.Trim().Length > 0)
                {
                    if (detailed.Attr4 != null && detailed.Attr4.Trim().Length > 0)
                    {
                        output += ", " + master.Attr4 + " " + detailed.Attr4;
                    }

                }

                if (master.Attr5 != null && master.Attr5.Trim().Length > 0)
                {
                    if (detailed.Attr5 != null && detailed.Attr5.Trim().Length > 0)
                    {
                        output += ", " + master.Attr5 + " " + detailed.Attr5;
                    }

                }

                if (master.Attr6 != null && master.Attr6.Trim().Length > 0)
                {
                    if (detailed.Attr6 != null && detailed.Attr6.Trim().Length > 0)
                    {
                        output += ", " + master.Attr6 + " " + detailed.Attr6;
                    }

                }

                if (master.Attr7 != null && master.Attr7.Trim().Length > 0)
                {
                    if (detailed.Attr7 != null && detailed.Attr7.Trim().Length > 0)
                    {
                        output += ", " + master.Attr7 + " " + detailed.Attr7;
                    }

                }
            }
            return output;
        }

    }
}
