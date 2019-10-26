using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;

using System.Linq;
using System;
using Microsoft.Phone.Shell;


namespace MediboxSTA
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        // Plik bazy danych
        private const string DATA_BASE_FILE = @"Data Source=isostore:/MediboxData.sdf";

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            // Koniec dnia
            DateTime endDay = DateTime.Today.AddHours(24);

            using (MediboxDataContext dc = new MediboxDataContext(DATA_BASE_FILE))
            {
                
                // Odśwież kafelek
                TileAdministrator(dc);
            }

            //TODO: Tylko do testów
            //ShellToast Toast = new ShellToast();
            //Toast.Title = "Medibox";
            //Toast.Content = "Test agenta";
            //Toast.Show();

            NotifyComplete();

            //TODO: Usunąć
            //ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(60));
        }


        //
        // Uaktualnia kafelek na ekranie startowym
        //
        private void TileAdministrator(MediboxDataContext dc)
        {
            /*
             * CEL:
             * Uaktualnia kafelek na ekranie startowy
             * 
             * PARAMETRU:
             * dc:MediboxDataContext - dataContext
             */

            // Zwróć liczbę zadań w bazie
            int allTasksCount = (from at in dc.MediTasksTable select at).Count();
            // Koniec dnia
            DateTime endDay = DateTime.Today.AddHours(24);
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
            //newTile.IconImage = new Uri("/Assets/Tiles/IconicTileMediumLarge.png", UriKind.Relative);
            //newTile.SmallIconImage = new Uri("/Assets/Tiles/IconicTileSmall.png", UriKind.Relative);
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
}