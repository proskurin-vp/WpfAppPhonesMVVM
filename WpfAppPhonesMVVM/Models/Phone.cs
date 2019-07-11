using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPhonesMVVM.Models
{
    public class Phone : INotifyPropertyChanged
    {
        private string _image;
        private string _sale;
        private string _name;
        private int? _oldPrice;
        private int _price;
        private string _description;
        private bool _isEdit;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            // старый способ
            //if(PropertyChanged!=null)
            //{
            //    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            //}
            // новый способ
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Image
        {
            get { return _image; }
            set
            {
                if(_image != value)
                {
                    _image = value;
                    OnPropertyChanged("Image");
                }
            }
        }
        public string Sale
        {
            get { return _sale; }
            set
            {
                if(_sale != value)
                {
                    _sale = value;
                    OnPropertyChanged("Sale");
                }
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if(_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public int? OldPrice
        {
            get { return _oldPrice; }
            set
            {
                if(_oldPrice != value)
                {
                    _oldPrice = value;
                    OnPropertyChanged("OldPrice");
                }
            }
        }
        public int Price
        {
            get { return _price; }
            set
            {
                if(_price != value)
                {
                    _price = value;
                    OnPropertyChanged("Price");
                }
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                if(_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        public bool IsEdit
        {
            get { return _isEdit; }
            set
            {
                if(_isEdit != value)
                {
                    _isEdit = value;
                    OnPropertyChanged("IsEdit");
                }
            }
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Phone) || obj == null)
            {
                return false;
            }
            Phone phone = obj as Phone;

            return _image.Equals(phone.Image) && _sale.Equals(phone.Sale) && _name.Equals(phone.Name) && _oldPrice.Equals(phone.OldPrice)
                && _price == phone.Price && _description.Equals(phone.Description) && _isEdit == phone.IsEdit;

        }
        public override int GetHashCode()
        {
            try
            {
                return _image.GetHashCode() + _sale.GetHashCode() + _name.GetHashCode() + _oldPrice.GetHashCode()
                    + _price + _description.GetHashCode() + _isEdit.GetHashCode();
            }
            catch
            {
                return 0;
            }
        }
    }
}
