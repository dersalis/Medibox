using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;

namespace Atrx.WindowsPhone.Medibox
{
    [Table]
    public class Medicine : INotifyPropertyChanging, INotifyPropertyChanged
    {
        // Indeks leku
        private int _id;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id
        {
            get { return _id; }
            set
            {
                RaisePropertyChanging("Id");
                _id = value;
                RaisePropertyChanged("Id");
            }
        }

        // Nazwa leku
        private string _medicinName;
        [Column]
        public string MedicinName
        {
            get { return _medicinName; }
            set
            {
                RaisePropertyChanging("MedicinName");
                _medicinName = value;
                RaisePropertyChanged("MedicinName");
            }
        }

         // Liczba dawek leku na dzień
        private int _numberOfDosesPerDay;
        [Column]
        public int NumberOfDosesPerDay
        {
            get { return _numberOfDosesPerDay; }
            set
            {
                RaisePropertyChanging("NumberOfDosesPerDay");
                _numberOfDosesPerDay = value;
                RaisePropertyChanged("NumberOfDosesPerDay");
            }
        }

        // Wielkość dawki
        private string _dose;
        [Column]
        public string Dose
        {
            get { return _dose; }
            set
            {
                RaisePropertyChanging("Dose");
                _dose = value;
                RaisePropertyChanged("Dose");
            }
        }

        // Liczba dni na przyjmowanie leku
        //[Column]
        //public int NumberOfDays { get; set; }

        // Przypomnienie
        private bool _isReminder;
        [Column]
        public bool IsReminder
        {
            get { return _isReminder; }
            set
            {
                RaisePropertyChanging("IsReminder");
                _isReminder = value;
                RaisePropertyChanged("IsReminder");
            }
        }

        // Data rozpoczęcia
        private DateTime _startDate;
        [Column]
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                RaisePropertyChanging("StartDate");
                _startDate = value;
                RaisePropertyChanged("StartDate");
            }
        }

        // Data zakończenia
        private DateTime _stopDate;
        [Column]
        public DateTime StopDate
        {
            get { return _stopDate; }
            set
            {
                RaisePropertyChanging("StopDate");
                _stopDate = value;
                RaisePropertyChanged("StopDate");
            }
        }

        // Określa czy nadal lekarstwo jest przyjmowane
        private bool _isActive;
        [Column]
        public bool IsActive
        {
            get { return _isActive; }
            set 
            {
                RaisePropertyChanging("IsActive");
                _isActive = value;
                RaisePropertyChanged("IsActive");
            }
        }

        // Priorytet
        private bool _isHighPriority;
        [Column]
        public bool IsHighPriority
        {
            get { return _isHighPriority; }
            set
            {
                RaisePropertyChanging("IsHighPriority");
                _isHighPriority = value;
                RaisePropertyChanged("IsHighPriority");
            }
        }

        // Kategoria
        //[Column]
        //public int Category { get; set; }

        // Notatka
        private string _note;
        [Column]
        public string Note
        {
            get { return _note; }
            set 
            {
                RaisePropertyChanging("Note");
                _note = value;
                RaisePropertyChanged("Note");
            }
        }


        // Deklaracja ReisePropertyChanging
        public event PropertyChangingEventHandler PropertyChanging;
        private void RaisePropertyChanging(string propName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propName));
            }
        }

        // Deklaracja ReisePropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
