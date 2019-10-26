using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Phone.Scheduler;

namespace Medibox.Medibox
{
    public class MediboxReminder
    {
        //
        // Dodaje przypomnienie 
        //
        public static void AddReminder(string name, DateTime startTime, string title, string content, int medicineId)
        {
            // Dodaj przypomnienie jeśli nie istnieje
            // Dodaj przypomnienie jeśli data przypomnienia jest większa od obecnej
            if((ScheduledActionService.Find(name) == null) && (startTime > (DateTime.Now.AddMinutes(1))))
            {
                Reminder reminder = new Reminder(name);
                reminder.BeginTime = startTime;
                reminder.Title = title;
                reminder.Content = content;
                reminder.NavigationUri = new Uri("/Pages/ViewMedicinePage.xaml?selectedItem=" + medicineId, UriKind.Relative);

                ScheduledActionService.Add(reminder);
            }
        }


        //
        // Usuwa przypomnienie
        //
        public static void RemoveReminder(string name)
        {
            // Sprawdź czy istnieje przypomnienie i usuń je
            if (ScheduledActionService.Find(name) != null)
                ScheduledActionService.Remove(name);
        }
    }
}
