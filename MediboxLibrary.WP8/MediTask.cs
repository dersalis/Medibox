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
    public class MediTask : INotifyPropertyChanging, INotifyPropertyChanged
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
        private string _medicineName;
        [Column]
        public string MedicineName
        {
            get { return _medicineName; }
            set
            {
                RaisePropertyChanging("MedicineName");
                _medicineName = value;
                RaisePropertyChanged("MedicineName");
            }
        }

        // Wielkość dawki
        private string _singleDose;
        [Column]
        public string SingleDose
        {
            get { return _singleDose; }
            set
            {
                RaisePropertyChanging("SingleDose");
                _singleDose = value;
                RaisePropertyChanged("SingleDose");
            }
        }

        // Data wystąpienia
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

        //Nazwa przypomnienia
        private string _reminderName;
        [Column]
        public string ReminderName
        {
            get { return _reminderName; }
            set 
            {
                RaisePropertyChanging("ReminderName");
                _reminderName = value;
                RaisePropertyChanged("ReminderName");
            }
        }

        //Nazwa przypomnienia
        private string _reminderContent;
        [Column]
        public string ReminderContent
        {
            get { return _reminderContent; }
            set 
            {
                RaisePropertyChanging("ReminderContent");
                _reminderContent = value;
                RaisePropertyChanged("ReminderContent");
            }
        }

        //// Tytuł przypomnienia
        //[Column]
        //public string ReminderTitle { get; set; }

        //// Zawartość przypomnienia
        //[Column]
        //public string ReminderContent { get; set; }

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

        // Id leku
        private int _medicineId;
        [Column]
        public int MedicineId
        {
            get { return _medicineId; }
            set
            {
                RaisePropertyChanging("MedicineId");
                _medicineId = value;
                RaisePropertyChanged("MedicineId");
            }
        }

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
