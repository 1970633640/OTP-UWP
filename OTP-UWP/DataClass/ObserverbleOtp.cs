using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTP_UWP.DataClass
{
    public class ObserverbleOtp :INotifyPropertyChanged
    {
        private string _Code;
        public string Code
        {
            get { return _Code; }
            set { _Code = value; OnPropertyChanged("Code"); }
        }

        private int _Remain;
        public int Remain
        {
            get { return _Remain; }
            set { _Remain = value; OnPropertyChanged("Remain"); }
        }

        public int period;

        public int algorithm;

        public int type;

        public int digits;

        public string logo;

        public string issuer;

        public string name;

        public string secret;

        public int id;

        public int Height = 120; //hack

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
