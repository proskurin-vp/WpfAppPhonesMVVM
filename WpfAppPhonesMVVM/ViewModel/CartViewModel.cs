using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppPhonesMVVM.Models;
using WpfAppPhonesMVVM.Utils;
using WpfAppPhonesMVVM.View;

namespace WpfAppPhonesMVVM.ViewModel
{
    public class CartViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<CartPhone> _phones;
        private CartPhone _selectedCartPhone;
        private CustomCommand _deletePhone;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public CartViewModel()
        {
            _phones = new ObservableCollection<CartPhone>();
        }

        public CustomCommand DeletePhone
        {
            get
            {
                return _deletePhone ?? (_deletePhone = new CustomCommand(obj =>
                {
                    if (SelectedCartPhone != null)
                    {
                        Phones.Remove(SelectedCartPhone);
                        
                        if (Phones.Count > 0)
                        {
                            SelectedCartPhone = Phones[0];
                        }
                    }                    
                }));
            }
        }
        public ObservableCollection<CartPhone> Phones
        {
            get { return _phones; }
            set
            {
                if(_phones != value)
                {
                    _phones = value;
                    OnPropertyChanged("Phones");
                }
            }
        }       
        public CartPhone SelectedCartPhone
        {
            get { return _selectedCartPhone; }
            set
            {
                if(_selectedCartPhone != value)
                {
                    _selectedCartPhone = value;
                    OnPropertyChanged("SelectedCartPhone");
                }
            }
        }       

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
