//#define TRIAL
    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using Medibox.Resources;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System.Windows;
using System.Collections.ObjectModel;
using Atrx.WindowsPhone.Medibox;
using Microsoft.Phone.Marketplace;

namespace Medibox.Medibox
{
    public class MediboxManagement : INotifyPropertyChanged
    {
        // Plik bazy danych
        //private const string DATA_BASE_FILE = @"Data Source=isostore:/MediboxData.sdf";

        // Lista liczby dawek w dniu
        public int[] NumberOfDosesPerDayArray { get { return new int[] { 1, 2, 3, 4, 6, 8, 12, 24 }; } }

        //
        // Konstruktor
        //
        public MediboxManagement()
        {
            // Utwórz bazę danych
            //TODO: Zmienić 
            CreateDataBase(true);

            //TODO: Tymczasowo ustaw true
            LetTaskAgent = true;
            // Restartuj zadania w tle
            TaskAgentRestart();
            // Można dodać przypomnienia
            LetAddReminders = true;
            // Sprawdź nieaktywne zadania
            MediboxAdministrator.OldTaskAdministrator();
            // Sprawdź nieaktywne leki
            MediboxAdministrator.OldMedicineAdministrator();
            // Odśwież listę leków i zadań
            SetMedicinesList();
            SetTasksList();
        }


        // 1.0.4.2
        #region Agent działający w tle

        // Nazwa agenta
        private const string TASK_AGENT_NAME = "MediboxTaskAgent";
        // Określa czy agent jest uruchomiony
        private bool _letTaskAgent;
        public bool LetTaskAgent 
        {
            get { return _letTaskAgent; }
            set
            {
                if (_letTaskAgent != value)
                    _letTaskAgent = value;
            }
        }

        private void TaskAgentRun()
        {
            // Sprawdz czy można używać agenta
            if (LetTaskAgent)
            {
                PeriodicTask sta = new PeriodicTask(TASK_AGENT_NAME);
                sta.Description = "Zadania wykonywane w tle";

                ScheduledActionService.Add(sta);
                //TODO: Usunąć
                //ScheduledActionService.LaunchForTest(TASK_AGENT_NAME, TimeSpan.FromSeconds(30));
            }
        }

        private void TaskAgentFinish()
        {
            // Sprawdz czy uruchomiono agenta
            PeriodicTask currentAgent = ScheduledActionService.Find(TASK_AGENT_NAME) as PeriodicTask;
            // Jeśli agent istnieje to usuń go
            if (currentAgent != null)
                ScheduledActionService.Remove(TASK_AGENT_NAME);
        }

        private void TaskAgentRestart()
        {
            TaskAgentFinish();
            TaskAgentRun();
        }


        #endregion


        // 1.0.0.2
        #region Wczytywanie leków

        // Lista leków
        private ObservableCollection<Medicine> _medicinesList;
        public ObservableCollection<Medicine> MedicinesList
        {
            get { return _medicinesList; }
            set
            {
                if (_medicinesList != value)
                {
                    _medicinesList = value;
                    NotifyPropertyChanged("MedicinesList");
                }
            }
        }


        //
        // Zwraca listę wszystkich leków
        //
        public void SetMedicinesList()
        {
            /*
             * CEL:
             * Zwraca listę wszystkich leków
             */

            // Zwraca listę leków posortowanych alfabetycznie
            MedicinesManager mm = new MedicinesManager();
            MedicinesList = new ObservableCollection<Medicine>(mm.GetMedicinesSortAlphabetically());
        }

        #endregion


        // 1.0.0.2
        #region Wczytywanie zadań

        // Lista zadań
        private ObservableCollection<MediTask> _tasksList;
        public ObservableCollection<MediTask> TasksList
        {
            get { return _tasksList; }
            set
            {
                if (_tasksList != value)
                {
                    _tasksList = value;
                    NotifyPropertyChanged("TasksList");
                }
            }
        }


        // 
        // Zwraca listę zadań na dziś do TaskList
        //
        public void SetTasksList()
        {
            /*
             * CEL:
             * Zwraca listę zadań na dziś do TaskList
             */

            // Ustaw daty
            DateTime today = DateTime.Now;
            DateTime tomorrow = DateTime.Today.AddDays(1);

            // Zwróć zadania
            TasksManager ts = new TasksManager();
            TasksList = new ObservableCollection<MediTask>(ts.GetTasksSortByStartData(today, tomorrow));
        }

        #endregion


        // 1.0.0.2
        #region Dodaj lekarstwo

        //
        // Dodaje nowy lek do bazy
        //
        public void AddNewMedicine(Medicine newMedicine)
        {
            /*
             * CEL:
             * Dodaje nowy lek do bazy
             * 
             * PARAMETRY WEJSCIOWE:
             * newMedicine:Medicine - nowy lek
             */


            
            // Odstępy czasu w jakich należy przyjąć lekarstwo - w godzinach
            int timeInterral = 24 / newMedicine.NumberOfDosesPerDay;
            
            // Utwórz DataContext
            using (MediboxDataContext dc = new MediboxDataContext(MediboxDataContext.DBConnectionString))
            {
                // Dodaj lekarstwo do bazy
                MediboxDataBaseManagement.AddMedicineToDataBase(dc, newMedicine);

                // Dodaj zadanie do bazy
                MediboxDataBaseManagement.AddMediTaskToDataBase(dc, newMedicine.MedicinName, newMedicine.Dose, newMedicine.StartDate, newMedicine.StopDate, newMedicine.IsReminder, newMedicine.IsHighPriority, newMedicine.Id, newMedicine.Note, timeInterral);

                //TODO: Może dodać przypomnienia

                // Uaktualnij kafelek
                //MediboxAdministrator.TileAdministrator(dc);
                
                //
                // Zmiany w bazie zapisano w podfunkcjach
                //
            }

            // Odśwież listę leków i zadań
            SetMedicinesList();
            SetTasksList();

            // Można dodać przypomnienia
            LetAddReminders = true;
        }

        #endregion


        // 1.0.0.2
        #region Wybrany lek i zadania

        // Wybrany cytat
        public Medicine SelectedMedicine { get; set; }

        // Wybrane dzisiejsze dawki wybranego leku
        public List<MediTask>SelectedTodaysDoses { get; set; }


        //
        // Ustawia wybrany lek
        //
        public void SetSelecteMedicine(int id)
        {
            /*
             * CEL:
             * Ustawia wybrany lek na podstawie id leku
             * 
             * PARAMETRY:
             * id:int - id wybranego leku
             */

            // Wybrany cytat
            Medicine selectedMedicine = null;

            // Wybierz cytat o podanym Id
            using (MediboxDataContext dc = new MediboxDataContext(MediboxDataContext.DBConnectionString))
            {
                // Wyszukaj lek o podanym id
                selectedMedicine = (from med in dc.MedicinesTable where med.Id == id select med).FirstOrDefault();
            }

            // Przekaż lek do zmiennej
            SelectedMedicine = selectedMedicine;

            // Wybierz dzisiejsze dawki wybranego lekarstwa
            List<MediTask> todaysTask = (from task in TasksList where task.MedicineId == id select task).ToList();

            // Przekaż dawki do zmiennej
            SelectedTodaysDoses = todaysTask;

            // Odśwież
            NotifyPropertyChanged("SelectedMedicine");
            NotifyPropertyChanged("SelectedTodaysDoses");
        }

        #endregion


        // 1.0.0.2
        #region Aktualizuj lek

        //
        // Aktualizuje lek w bazie
        //
        public void UpdateMedicine(int id, Medicine updatedMedicine)
        {
            /*
             * CEL:
             * Aktualizuje lek w bazie
             * 
             * PARAMETRY:
             * id:int - id leku
             * updateMedicine:Medicine - aktualizowany lek
             */

          
            // Użyj dataContext
            using (MediboxDataContext dc = new MediboxDataContext(MediboxDataContext.DBConnectionString))
            {
                // Wyszukaj modyfikowane lekarstwo
                IQueryable<Medicine> med = from me in dc.MedicinesTable where me.Id == id select me;
                Medicine medicine = med.FirstOrDefault();

                // Wprowadź zmiany
                medicine.MedicinName = updatedMedicine.MedicinName;
                medicine.Dose = updatedMedicine.Dose;
                medicine.NumberOfDosesPerDay = updatedMedicine.NumberOfDosesPerDay;
                medicine.StartDate = updatedMedicine.StartDate;
                medicine.StopDate = updatedMedicine.StopDate;
                medicine.IsReminder = updatedMedicine.IsReminder;
                medicine.IsHighPriority = updatedMedicine.IsHighPriority;
                medicine.Note = updatedMedicine.Note;

                // Wprowadź zmiany w bazie
                dc.SubmitChanges();

                // Usuń powiązane zadania
                DeleteTasksAndRemindersOfMedicinId(dc, medicine.Id);

                // Odstępy czasu w jakich należy przyjąć lekarstwo - w godzinach
                int timeInterval = 24 / medicine.NumberOfDosesPerDay;

                // Dodaj nowe powiązane zadania
                MediboxDataBaseManagement.AddMediTaskToDataBase(dc, medicine.MedicinName, medicine.Dose, medicine.StartDate, medicine.StopDate, medicine.IsReminder, medicine.IsHighPriority, medicine.Id, medicine.Note, timeInterval);

                // Wprowadź zmiany
                dc.SubmitChanges();

                //TODO: Może dodać przypomnienia

                // Uaktualnij kafelek
                //MediboxAdministrator.TileAdministrator(dc);
            }

            // Odśwież listę leków i zadań
            SetMedicinesList();
            SetTasksList();

            // Można dodać przypomnienia
            LetAddReminders = true;
        }

        #endregion


        // 1.0.0.2
        #region Usuń lek

        //
        // Usuwa lek i związane z nim zadania
        //
        public void DeleteMedicine(int id)
        {
            /*
             * CEL:
             * Usuwa lek i związane z nim zadania
             * 
             * PARAMETRY:
             * id:int - id lekarstwa
             */

            // Wykorzystaj data context
            using (MediboxDataContext dc = new MediboxDataContext(MediboxDataContext.DBConnectionString))
            {
                // Usuń lek o podanym id
                DeleteMedicineOfId(dc, id);

                // Usuń wszystkie zadania i przypomnienia związane z lekarstwem
                DeleteTasksAndRemindersOfMedicinId(dc, id);

                //TODO: Może dodać przypomnienia

                // Uaktualnij kafelek
                //MediboxAdministrator.TileAdministrator(dc);
            }

            // Odśwież listę leków i zadań
            SetMedicinesList();
            SetTasksList();

            // Można dodać przypomnienia
            LetAddReminders = true;
        }

        
        //
        // Usuwa zadania i przypomnienia związane z lekarstwem o podanym Id
        //
        private void DeleteTasksAndRemindersOfMedicinId(MediboxDataContext dataContext, int medicinId)
        {
            /*
             * CEL:
             * Usuwa zadania i przypomnienia związane z lekarstwem o podanym Id
             * 
             * PARAMETRY WEJŚCIOWE:
             * dataContext:MediboxDataContext - dataContext
             * medicinId - id lekarstwa
             */

            // Wyszukaj wszystkie wystąpienia zadań dla tego lekarstwa
            IQueryable<MediTask> tasks = from task in dataContext.MediTasksTable where task.MedicineId == medicinId select task;

            // Usuń wszystkie przypomnienia związane z lekarstwem
            foreach (var task in tasks)
                MediboxReminder.RemoveReminder(task.ReminderName);

            // Usuń zadania wszystkie zadania związane z lekarstwem
            dataContext.MediTasksTable.DeleteAllOnSubmit<MediTask>(tasks);
            // Zapisz zmiany w bazie
            dataContext.SubmitChanges();
        }


        //
        // Usuwa lekarstwo o podanym id
        //
        private void DeleteMedicineOfId(MediboxDataContext dataContext, int medicineId)
        {
            /*
            * CEL:
            * Usuwa lekarstwa o podanym Id
            * 
            * PARAMETRY WEJŚCIOWE:
            * dataContext:MediboxDataContext - dataContext
            * medicinId - id lekarstwa
            */

            // Wyszukaj usuwane lekarstwo
            //Medicine deletedMedicine = (from med in dataContext.MedicinesTable where med.Id == medicineId select med).FirstOrDefault();
            IQueryable<Medicine> deletedMedicines = from med in dataContext.MedicinesTable where med.Id == medicineId select med;
            // Usuń z bazy danych
            dataContext.MedicinesTable.DeleteAllOnSubmit<Medicine>(deletedMedicines);
            // Zapisz miany
            dataContext.SubmitChanges();
        }


        //
        // Przenosi do historii lekarstwo o podanym id
        //
        private void MoveToHistoryMedicineOfId(MediboxDataContext dataContext, int medicineId)
        {
            /*
            * CEL:
            * Przenosi do historii lekarstwa o podanym Id
            * 
            * PARAMETRY WEJŚCIOWE:
            * dataContext:MediboxDataContext - dataContext
            * medicinId - id lekarstwa
            */

            // Wyszukaj przenoszone lekarstwo
            Medicine movedMedicine = (from med in dataContext.MedicinesTable where med.Id == medicineId select med).FirstOrDefault();
            // Ustaw jako nie aktywny
            movedMedicine.IsActive = false;
            // Zapisz miany
            dataContext.SubmitChanges();
        }

        #endregion


        // 1.0.4.2
        #region Administrator

        // Określa czy można dodać przypomnienia
        // True - można dodać przypomnienia
        public bool LetAddReminders { get; set; }
        
        //
        // Zarządza zadaniami
        //
        public void Administrator()
        { 
            /*
             * CEL:
             * Wyszukuje zakończone zadania - usuwa je wraz z przypomnieniami
             * Dodaje przypomnienia na kolejne 12 godzin
             * Odświeża kafelek - jeśli można
             * Odświeża informacje na ekranie blokady - jeśli można
             */

            // Obecna data
            DateTime nowTime = DateTime.Now;
            // 12 godzin później
            DateTime laterTime = DateTime.Now.AddHours(8);
            // Koniec dnia
            DateTime endDay = DateTime.Today.AddHours(24);

            // Treść przypomnienia - pojedyńcza dawka leku
            //string reminderContent;

            // Użyj dataContext
            using (MediboxDataContext dc = new MediboxDataContext(MediboxDataContext.DBConnectionString))
            {
                //
                // Usuń zakończone zadania
                // Wyszukaj zakończone zadania - których data jest mniejsza od obecnej
                IQueryable<MediTask> oldTasks = from task in dc.MediTasksTable where task.StartDate < DateTime.Now select task;
                // Jeśli zadań jest więciej od 0
                if (oldTasks.Count() > 0)
                {
                    // Sprawdź czy dla wyszukanych zadań uruchomiono przypomnienia i usuń je
                    foreach (var task in oldTasks)
                    {
                        // Sprawdź czy uruchomiono przypomnienie i usuń je
                        if (task.IsReminder)
                            MediboxReminder.RemoveReminder(task.ReminderName);
                    }

                    // Usuń zakończone zadania
                    dc.MediTasksTable.DeleteAllOnSubmit<MediTask>(oldTasks);

                    // Zapisz zmiany w bazie
                    dc.SubmitChanges();
                }


                //
                // Dodaj przypomnienia na kolejne 8 godzin
                // Wyszukaj zadania z przedziału 8 godzin
                IQueryable<MediTask> otherTasks = from task in dc.MediTasksTable where (task.StartDate > DateTime.Now) && (task.StartDate <= laterTime) select task;
                // Jeśli liczba zadań jest większa od 0
                if(otherTasks.Count() > 0)
                {
                    // Dodaj przypomnienia dla wyszukanych zadań
                    foreach (var task in otherTasks)
                    {
                        // Sprawdź czy można dodać przypomnienie i dodaj je
                        // Przypomnienia można dodać gdy zadania w tle są uruchomione
                        if (task.IsReminder && LetTaskAgent)
                        {
                            // Dodaj przypomnienie
                            MediboxReminder.AddReminder(task.ReminderName, task.StartDate, task.MedicineName, task.ReminderContent, task.MedicineId);
                        }
                    }
                }


                //
                // Odśwież kafelek
                //MediboxAdministrator.TileAdministrator(dc);
            }

            

            //TODO: Odśwież ekran blokady
            
        }


        


        

        #endregion


        //TODO: Pierwszy start - wprowadzić zmianę
        #region Pierwszy start

        //
        // Tworzy bazę danych 
        //
        private void CreateDataBase(bool create)
        {
            // Sprawdź czy utowrzyć
            if (create)
            {
                using (MediboxDataContext dataBase = new MediboxDataContext(MediboxDataContext.DBConnectionString))
                {
                    if (dataBase.DatabaseExists() == false)
                    {
                        // Utwórz bazę danych
                        dataBase.CreateDatabase();
                    }
                    else
                    {
                        //MessageBox.Show("Employee Database already exists!!!");
                    }
                }
            }
        }


       
        #endregion

        //
        #region Licencja

        private LicenseInformation _license = new LicenseInformation();

        // Określa stan czy program jest w wersji trial czy płatnej
        private bool _isTrial = true;
        public bool IsTrial
        {
            get { return _isTrial; }
            set
            {
                if (_isTrial != value)
                {
                    IsTrial = true;
                    _isTrial = value;
                    NotifyPropertyChanged("IsTrial");
                }
            }
        }

        // Sprawdz licencje
        public void CheckLicense()
        {
#if TRIAL
            IsTrial = true;
#else
            // Zwróć licencje
            IsTrial = _license.IsTrial();
#endif
        }

        #endregion


        #region Singleton

        private static MediboxManagement _instance = null;
        public static MediboxManagement Instance
        {
            get 
            {
                if (_instance == null)
                    _instance = new MediboxManagement();
                return _instance;
            }
        }

        #endregion


        #region NotifyPropertyChanged

        //
        // Implementacja NotifyPropertyChanged
        //
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            //PropertyChangedEventHandler handler = PropertyChanged;
            //if (null != handler)
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                //handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
