using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace MediboxSTA
{
    [Table]
    public class MediTask
    {
        // Indeks leku
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id { get; set; }

        // Nazwa leku
        [Column]
        public string MedicineName { get; set; }

        // Wielkość dawki
        [Column]
        public string SingleDose { get; set; }

        // Data wystąpienia
        [Column]
        public DateTime StartDate { get; set; }

        // Przypomnienie
        [Column]
        public bool IsReminder { get; set; }

        //Nazwa przypomnienia
        [Column]
        public string ReminderName { get; set; }

        //Nazwa przypomnienia
        [Column]
        public string ReminderContent { get; set; }

        //// Tytuł przypomnienia
        //[Column]
        //public string ReminderTitle { get; set; }

        //// Zawartość przypomnienia
        //[Column]
        //public string ReminderContent { get; set; }

        // Priorytet
        [Column]
        public bool IsHighPriority { get; set; }

        // Id leku
        [Column]
        public int MedicineId { get; set; }

        // Notatka
        [Column]
        public string Note { get; set; }
    }
}
