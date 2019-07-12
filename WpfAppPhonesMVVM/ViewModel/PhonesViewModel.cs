using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using WpfAppPhonesMVVM.Models;
using WpfAppPhonesMVVM.Utils;
using WpfAppPhonesMVVM.View;

namespace WpfAppPhonesMVVM.ViewModel
{
    public class PhonesViewModel : INotifyPropertyChanged
    {
        private CustomCommand _addPhone;
        private CustomCommand _deletePhone;
        private CustomCommand _changeImage;
        private CustomCommand _savePhones;
        private CustomCommand _closeWindow;
        private CustomCommand _addToCart;
        private CustomCommand _showCart;
        private Phone _selectedPhone;
        private readonly CartWindow _cartWindow = new CartWindow();

        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }       
        public CustomCommand AddPhone
        {
            get
            {
                return _addPhone ?? (_addPhone = new CustomCommand(obj =>
                {
                    Phone phone = new Phone
                    {
                        Image = "/Images/default.png",
                        Sale = "Измените текст распродажи",
                        Name = "Измените название телефона",
                        Description = "Измените описание"
                    };
                    Phones.Insert(0, phone);
                    SelectedPhone = phone;
                }));
            }
        }
        public CustomCommand DeletePhone
        {
            get
            {
                return _deletePhone ?? (_deletePhone = new CustomCommand(obj =>
                {
                    //if (SelectedPhone != null)
                    //{
                    //    Phones.Remove(SelectedPhone);
                    //    if(Phones.Count>0)
                    //    {
                    //        SelectedPhone = Phones[0];
                    //    }
                    //}

                    // вариант метода, использующий аргумент obj
                    // (передаётся из разметки через CommandParameter="{Binding SelectedPhone}")
                    Phone phone = obj as Phone;
                    if (phone != null)
                    {
                        Phones.Remove(phone);
                        if (Phones.Count > 0)
                        {
                            SelectedPhone = Phones[0];
                        }
                    }
                },
                obj => 
                {
                    //return Phones.Count > 0 && SelectedPhone != null;

                    Phone phone = obj as Phone;
                    return Phones.Count > 0 && phone != null;                    
                }
                ));
            }
        }
        public CustomCommand ChangeImage
        {
            get
            {
               return _changeImage ?? (_changeImage = new CustomCommand(obj =>
               {
                   if(SelectedPhone != null && (bool)obj == true)
                   {
                       OpenFileDialog ofd = new OpenFileDialog
                       {
                           Filter = "Image Files (*.bmp; *.gif; *.jpg; *.png;)|*.bmp;*.gif;*.jpg;*.png",
                           InitialDirectory = Environment.CurrentDirectory,
                       };
                       if(ofd.ShowDialog() == true)
                       {
                           SelectedPhone.Image = ofd.FileName;
                       }
                   }
               },
                    obj =>
                    {
                        return SelectedPhone != null && (bool)obj == true;
                    }
               ));
            }
        }
        public CustomCommand SavePhones
        {
            get
            {
                return _savePhones ?? (_savePhones = new CustomCommand(Serialize,
                obj =>
                {
                    return Phones.Count > 0;
                }));
            }
        }
        public CustomCommand AddToCart
        {
            get
            {
                return _addToCart ?? (_addToCart = new CustomCommand(obj =>
                {
                    Phone phone = obj as Phone;
                    CartPhone cPhone = ((CartViewModel)_cartWindow.DataContext).Phones.FirstOrDefault(p => p.CPhone == phone);
                    if(cPhone == null)
                    {
                        ((CartViewModel)_cartWindow.DataContext).Phones.Add(new CartPhone { CPhone = phone, Count = 1 });
                    }
                    else
                    {
                        cPhone.Count++;
                    }
                    
                }));
            }
        }

        public CustomCommand ShowCart
        {
            get
            {
                return _showCart ?? ( _showCart = new CustomCommand(obj=>
                {
                    _cartWindow.ShowDialog();
                },
                obj=>
                {
                    return ((CartViewModel)_cartWindow.DataContext)
                    .Phones.Count() > 0;
                }
                ));
            }
        }

        private void Serialize(object parameter)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Phone>));
            using (FileStream stream = File.Open("phones.xml", FileMode.Create, FileAccess.Write))
            {
                foreach (Phone phone in Phones)
                {
                    phone.IsEdit = false;
                }
                xmlSerializer.Serialize(stream, Phones);
            }
        }
        public CustomCommand CloseWindow
        {
            get
            {
                return _closeWindow ?? (_closeWindow = new CustomCommand(obj =>
                {                   

                    List<Phone> listPhones = Phones.ToList();
                    List<Phone> fromFilePhones = null;

                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Phone>));
                    using (FileStream stream = File.Open("phones.xml", FileMode.Open, FileAccess.Read))
                    {
                        fromFilePhones = (List<Phone>)xmlSerializer.Deserialize(stream);
                    }

                    if (!listPhones.SequenceEqual(fromFilePhones, new PhoneComparer()))
                    {
                        if (MessageBox.Show("Сохранить изменения", "Подтвердите изменения",
                            MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            Serialize(null);
                        }
                    }

                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public Phone SelectedPhone
        {
            get { return _selectedPhone; }
            set
            {
                if(_selectedPhone != value)
                {
                    _selectedPhone = value;
                    OnPropertyChanged("SelectedPhone");
                }
            }
        }        
        public ObservableCollection<Phone> Phones { get; set; }
        public PhonesViewModel()
        {
                try
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Phone>));
                    using (FileStream stream = File.Open("phones.xml", FileMode.Open, FileAccess.Read))
                    {
                        Phones = (ObservableCollection<Phone>)xmlSerializer.Deserialize(stream);
                    }
                }
                catch (Exception ex)
                {
                    Phones = new ObservableCollection<Phone>
                    {
                        new Phone
                        {
                            Name = "Samsung Galaxy A5 2017 Duos SM-A520 Black",
                            Description = "Экран (5.2\", Super AMOLED, 1920x1080)/ Samsung Exynos 7880 (1.9 ГГц)/ основная камера: 16 Мп, фронтальная камера: 16 Мп/ RAM 3 ГБ/ 32 ГБ встроенной памяти + microSD/SDHC (до 256 ГБ)/ 3G/ LTE/ GPS/ ГЛОНАСС/ поддержка 2х SIM-карт (Nano-SIM)/ Android 6.0 (Marshmallow)/ 3000 мА*ч",
                            Sale = "Акция! Суперцена на Samsung Galaxy A5/A7 и оплата частями* на 10 месяцев!",
                            Price = 6999,
                            OldPrice = 7499,
                            Image = "/Images/samsung_sm_a520.jpg"
                        },
                        new Phone
                        {
                            Name = "Samsung Galaxy A5 2017 Duos SM-A520 Black",
                            Description = "Экран (5.7\", Super AMOLED, 1920x1080)/ Samsung Exynos 7880 (1.9 ГГц)/ основная камера: 16 Мп, фронтальная камера: 16 Мп/ RAM 3 ГБ/ 32 ГБ встроенной памяти + microSD/SDHC (до 256 ГБ)/ 3G/ LTE/ GPS/ ГЛОНАСС/ поддержка 2х SIM-карт (Nano-SIM)/ Android 6.0 (Marshmallow)/ 3600 мА*ч",
                            Sale = "Акция! Суперцена на Samsung Galaxy A5/A7 и оплата частями* на 10 месяцев!",
                            Price = 7999,
                            OldPrice = 8499,
                            Image = "/Images/samsung_sm_a720.jpg"
                        },
                        new Phone
                        {
                            Name = "Samsung Galaxy J3 2016 J320H/DS Black + чехол + защитное стекло в подарок!",
                            Description = "Экран (5\", Super AMOLED, 1280x720)/ четырёхъядерный (1.3 ГГц)/ основная камера: 8 Мп, фронтальная камера: 5 Мп/ RAM 1.5 ГБ/ 8 ГБ встроенной памяти + microSD (до 128 ГБ)/ 3G/ GPS/ поддержка 2х SIM-карт (Micro-SIM + Micro-SIM)/ Android 5.1 Lollipop / 2600 мА*ч",
                            Sale = null,
                            Price = 2699,
                            OldPrice = null,
                            Image = "/Images/samsung_sm_j320.jpg"
                        }
                    };
            }

            if (Phones.Count > 0)
            {
                SelectedPhone = Phones[0];
            }
        }
    }
}
