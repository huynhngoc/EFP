using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Utils
{    

    public static class EntityGreeting
    {
        public const string Name = "@chaohoi";
        public const string Value = "Chào bạn, ";
        public const bool IsDynamic = false;
        public const string Description = "Câu chào hỏi";
    }

    public static class EntityAddress
    {
        public const string Name = "@dcll";
        public const string Value = "Mời bạn liên hệ đến địa chỉ ";
        public const bool IsDynamic = false;
        public const string Description = "Địa chỉ";
    }

    public static class EntityPhone
    {
        public const string Name = "@dt";
        public const string Value = "Để biết thêm chi tiết mời bạn gọi đến số điện thoại ";
        public const bool IsDynamic = false;
        public const string Description = "Điện thoại";
    }

    public static class EntityName
    {
        public const string Name = "@ten";
        public const string Value = "";
        public const bool IsDynamic = false;
        public const string Description = "Tên";
    }

    public static class EntityIntroduction
    {
        public const string Name = "@gt";
        public const string Value = "";
        public const bool IsDynamic = false;
        public const string Description = "Giới thiệu";
    }

    public static class EntityBankAccount
    {
        public const string Name = "@tk";
        public const string Value = "Mời bạn chuyển khoản đến số ";
        public const bool IsDynamic = false;
        public const string Description = "Tài khoản ngân hàng";
    }

    public static class EntityProductPrice
    {
        public const string Name = "@sp.gia";
        public const string Value = "Hiện tại, sản phẩm _sp_ có giá là _gia_";
        public const bool IsDynamic = true;
        public const string Description = "Báo giá";        
    }

    public static class EntityProductInStock
    {
        public const string Name = "@sp.conhang";
        public const string Value = "Hiện tại, sản phẩm _sp_ có giá là _conhang_";
        public const bool IsDynamic = true;
        public const string Description = "Báo còn hàng";
    }




}
