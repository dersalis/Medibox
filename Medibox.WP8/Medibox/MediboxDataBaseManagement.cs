using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Medibox.Resources;
using Atrx.WindowsPhone.Medibox;

namespace Medibox.Medibox
{
    public class MediboxDataBaseManagement
    {
        //
        // Dodaje nowe lekarstwo do bazy danych
        //
        public static void AddMedicineToDataBase(MediboxDataContext dataContext, Medicine medicine)
        {
            /*
             * CEL:
             * Dodaje nowe lekarstwo do bazy danych
             * 
             * PARAMETRY:
             * dataContext:MediboxDataContext - dataContext
             * medicine:Medicin - nowy lek
             */

            // Dodaj lekarstwo do bazy
            dataContext.MedicinesTable.InsertOnSubmit(medicine);

            // Zapisz zmiany w bazie
            dataContext.SubmitChanges();
        }


        //
        // Dodaje zadanie do bazy danych
        //
        public static void AddMediTaskToDataBase(MediboxDataContext dataContext, string medicinName, string medicinDose, DateTime startDate, DateTime stopDate, bool isReminder, bool isHighPriority, int medicineId, string medicinNote, int timeInterval)
        {
            /*
             * CEL:
             * Dodaje zadanie do bazy danych
             * 
             * PARAMETRY:
             * dataContext:MediboxDataContext - dataContext
             * medicinName:string - nazwa lekarstwa
             * medicinDose:string - wielkość dawki
             * startDate:DateTime - data przyjęcia dawki
             * isReminder:bool - określa czy dodać przypomnienie
             * isHighPriority:bool - określa czy jest ważne
             * medicinNote:string - notatka
             * numberOfDoseInstances:int - całkowita liczba wystąpień zadańa przyjęcia leku
             * timeInterval:int - odstępy czasu w jakich należy przyjąć lekarstwo - w godzinach
             */

            // Treść przypomnienia - pojedyńcza dawka leku
            //string reminderContent = string.Format("{0} {1}", AppResources.TextSingleDose, medicinDose);
            // Nazwa przypomnienia
            string reminderName = null;

            // Dodaj wszystkie zadania
            do
            {
                // Jeśli czas wystąpienia zadania i przypomnienia jest mniejszy od aktualnego to nie dodawaj
                if(startDate > (DateTime.Now.AddMinutes(1)))
                {
                    // Wygeneruj niepowtarzalną nazwe
                    reminderName = Guid.NewGuid().ToString();

                    // Utwórz nowe zadanie
                    MediTask newTask = new MediTask()
                    {
                        MedicineName = medicinName,
                        SingleDose = medicinDose,
                        StartDate = startDate,
                        IsReminder = isReminder,
                        ReminderName = reminderName,
                        ReminderContent = string.Format("{0} {1}", AppResources.TextSingleDose, medicinDose),
                        IsHighPriority = isHighPriority,
                        MedicineId = medicineId,
                        Note = medicinNote
                    };
                    // Dodaj zadanie do bazy
                    dataContext.MediTasksTable.InsertOnSubmit(newTask);
                }

                // Zwiększ datę
                startDate += new TimeSpan(0, timeInterval, 0, 0);
            }
            while(startDate < stopDate);

            // Zapisz zmiany w bazie
            dataContext.SubmitChanges();
        }
    }
}
