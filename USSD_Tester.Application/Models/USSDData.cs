using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace USSD.App.Models
{
    public class USSDData : INotifyPropertyChanged
    {
        private string response;
        private string request;
        public string Response
        {
            get
            {
                return response;
              
            }
            set
            {
                response = value;
                OnPropertyChanged("Response");

            }
        }
        public string Request
        {
            get
            {
                return request;

            }
            set
            {
                request = value;
                OnPropertyChanged("Request");

            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
