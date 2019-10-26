using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Phone.Shell;
using Medibox.Resources;
using Atrx.WindowsPhone.Medibox;

namespace Medibox.Medibox
{
    public class MediboxAdministrator
    {
        // Plik bazy danych
        const string DATA_BASE_FILE = @"Data Source=isostore:/MediboxData.sdf";

        //
        // Uaktualnia kafelek na ekranie startowym
        //
        public static void TileAdministrator()
        {
            /*
             * CEL:
             * Uaktualnia kafelek na ekranie startowy
             */

            // Koniec dnia
            DateTime endDay = DateTime.Today.AddHours(24);

            using (MediboxDataContext dc = new MediboxDataContext(DATA_BASE_FILE))
            {
                // Zwróć liczbę zadań w bazie
                int allTasksCount = (from at in dc.MediTasksTable select at).Count();
                // Liczba zadań do końca dnia
                int currentTasksCount = (from ct in dc.MediTasksTable where (ct.StartDate > DateTime.Now) && (ct.StartDate < endDay) select ct).Count();

                // Domyślny kafelek na ekranie start
                ShellTile currentTile = ShellTile.ActiveTiles.First();

                // Nowy kafelek Iconic
                IconicTileData newTile = new IconicTileData();
                // Ustaw kafelek
                newTile.Title = "Medibox";
                newTile.Count = currentTasksCount;
                newTile.BackgroundColor = System.Windows.Media.Colors.Red;
                newTile.IconImage = new Uri("/Assets/Tiles/IconicTileMediumLarge.png", UriKind.Relative);
                newTile.SmallIconImage = new Uri("/Assets/Tiles/IconicTileSmall.png", UriKind.Relative);
                // Jeśli allTasksCount > 0 to dodaj informacje
                // Jeśli allTasksCount == 0 to nie dodawaj informacji
                if (allTasksCount > 0)
                {
                    // Następne zadanie
                    MediTask nextTask = (from ct in dc.MediTasksTable where ct.StartDate > DateTime.Now orderby ct.StartDate ascending select ct).First();
                    // Dodaj informacje na kafelku
                    newTile.WideContent1 = nextTask.MedicineName;
                    newTile.WideContent2 = nextTask.ReminderContent;
                    newTile.WideContent3 = string.Format("{0:ddd, dd MMM yyyy, HH:mm}", nextTask.StartDate);
                }
                else
                {
                    // Nie dodawaj informacji na kafelku
                    newTile.WideContent1 = "";
                    newTile.WideContent2 = "";
                    newTile.WideContent3 = "";
                }

                // Uaktualnij kafelek
                currentTile.Update(newTile);
            }
        }


        //
        // Dodaje przypomnienia dla kolejnych godzin
        //
        public static void ShortRemindersAdministrator(MediboxDataContext dc)
        {
            // Kolejne godziny
            const int NEXT_HOURS = 4;
            // Wyszukaj zadania na kolejne godziny
            IQueryable<MediTask> nextTasks = from ct in dc.MediTasksTable where (ct.StartDate > DateTime.Now) && (ct.StartDate <= DateTime.Now.AddHours(NEXT_HOURS)) select ct;
            // Jeśli liczba zadań jest większa od 0
            if (nextTasks.Count() > 0)
            {
                // Dodaj przypomnienia dla wyszukanych zadań
                foreach (var ct in nextTasks)
                {
                    // Sprawdź czy można dodać przypomnienie i dodaj je
                    if (ct.IsReminder)
                    {
                        // Dodaj przypomnienie
                        MediboxReminder.AddReminder(ct.ReminderName, ct.StartDate, ct.MedicineName, ct.ReminderContent, ct.MedicineId);
                    }
                }
            }
        }


        //
        // Dodaje przypomnienia na kolejen 24 godziny
        //
        public static bool LongRemindersAdministrator(bool letAdd)
        {
            /*
             * CEL:
             * Dodaje przypomnienia na kolejen 24 godziny
             * 
             * PARAMETRY:
             * letAdd:bool - określa czy można dodać przypomnienia. True - można dodać
             */

            try
            {
                // Sprawdź czy można dodać przypomnienia
                if (letAdd)
                {
                    // Kolejne godziny
                    const int NEXT_HOURS = 24;
                    // Użyj dataContext
                    using (MediboxDataContext dc = new MediboxDataContext(DATA_BASE_FILE))
                    {
                        // Wyszukaj zadania na kolejne godziny
                        IQueryable<MediTask> nextTasks = from ct in dc.MediTasksTable where (ct.StartDate > DateTime.Now) && (ct.StartDate <= DateTime.Now.AddHours(NEXT_HOURS)) select ct;
                        // Jeśli liczba zadań jest większa od 0
                        if (nextTasks.Count() > 0)
                        {
                            // Dodaj przypomnienia dla wyszukanych zadań
                            foreach (var ct in nextTasks)
                            {
                                // Sprawdź czy można dodać przypomnienie i dodaj je
                                if (ct.IsReminder)
                                {
                                    // Dodaj przypomnienie
                                    MediboxReminder.AddReminder(ct.ReminderName, ct.StartDate, ct.MedicineName, ct.ReminderContent, ct.MedicineId);
                                }
                            }
                        }
                    }
                    // Ustaw że dodano przypomnienia
                    //letAdd = false;
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                // Ustaw że dodano przypomnienia
                letAdd = false;
            }

            // Zwróć
            return letAdd;
        }


        //
        // Usuń stare zadania
        //
        public static void OldTaskAdministrator()
        {
            /*
             * CEL:
             * Usuń stare zadania
             */

            using (MediboxDataContext dc = new MediboxDataContext(DATA_BASE_FILE))
            { 
                // Wyszukaj nieaktualne zadania
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
            }
        }

        //
        // Ustawia stare lekarstwa jako wymagające uwagi
        //
        public static void OldMedicineAdministrator()
        {
            /*
             * CEL:
             * Ustawia stare lekarstwa jako wymagające uwagi
             */

            using (MediboxDataContext dc = new MediboxDataContext(DATA_BASE_FILE))
            {
                // Wyszukaj nieaktualne leki
                IQueryable<Medicine> oldMedicins = from med in dc.MedicinesTable where med.StopDate < DateTime.Now select med;
                // Jeśli wybranych leków jest wiecej od 0
                if (oldMedicins.Count() > 0)
                { 
                    // Ustaw leki jako wymagające uwagi
                    foreach (var med in oldMedicins)
                    {
                        med.IsActive = false;
                        dc.SubmitChanges();
                    }
                }
            }
        }

        
    }
}
