using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppPhonesMVVM.Models;

namespace WpfAppPhonesMVVM.Utils
{
    public class PhoneComparer : IEqualityComparer<Phone>
    {
        public bool Equals(Phone x, Phone y)
        {
            bool isOldPriceEqual = x.OldPrice == null || y.OldPrice == null ? false : x.OldPrice == y.OldPrice;
            bool isNameEqual = x.Name == null || y.Name == null ? false : x.Name.Equals(y.Name);
            bool isPriceEqual = x.Price == y.Price;
            bool isSaleEqual = x.Sale == null || y.Sale == null ? false : x.Sale.Equals(y.Sale);
            bool isImageEqual = x.Image == null || y.Image == null ? false : x.Image.Equals(y.Image);
            bool isDescriptionEqual = x.Description == null || y.Description == null ? false : x.Description.Equals(y.Description);

            return isOldPriceEqual && isNameEqual && isPriceEqual && isSaleEqual && isImageEqual && isDescriptionEqual;
        }

        public int GetHashCode(Phone obj)
        {
            return obj.Name.GetHashCode() + obj.OldPrice.GetHashCode() + obj.Price.GetHashCode() +
                obj.Image.GetHashCode() + obj.Description.GetHashCode() + obj.Sale.GetHashCode();
        }
    }
}
