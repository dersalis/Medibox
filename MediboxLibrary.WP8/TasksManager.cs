using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atrx.WindowsPhone.Medibox
{
    public class TasksManager
    {
        // 
        // Zwraca listę zadań z podanego zakresu posortowaną wg. daty rozpoczęcia
        //
        public List<MediTask> GetTasksSortByStartData(DateTime startDate, DateTime endDate)
        {
            /*
            * CEL:
            * Zwraca listę zadań z podanego zakresu posortowaną wg. daty rozpoczęcia
            * 
            * ZWRACANY PARAMETR:
            * startDate:DateTime - data rozpoczęcia
            * endDate:DateTime - data zakończenia 
            */

            // Lista zwróconych zadań
            List<MediTask> tasks = null;

            // Zwraca listę zadań z podanego zakresu
            using (MediboxDataContext dc = new MediboxDataContext(MediboxDataContext.DBConnectionString))
            {
                tasks = (from task in dc.MediTasksTable where (task.StartDate > startDate) && (task.StartDate < endDate) orderby task.StartDate ascending select task).ToList();
            }

            // Zwróć listę zadań
            return tasks;
        }


    }
}
