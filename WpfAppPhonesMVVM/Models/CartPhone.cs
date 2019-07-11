using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppPhonesMVVM.Models
{
    public class CartPhone : INotifyPropertyChanged
    {
        private Phone _cPhone;
        private int _count;
        private int _totalCost;
        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public Phone CPhone
        {
            get { return _cPhone; }
            set { _cPhone = value; }
        }
        public int Count
        {
            get { return _count; }
            set
            {
                if(value != _count)
                {
                    _count = value;
                    _totalCost = _cPhone.Price * _count;
                    OnPropertyChanged("Count");
                    OnPropertyChanged("TotalCost");
                }
            }
        }
        public int TotalCost
        {
            get { return _totalCost;}           
        }      
    }
}
