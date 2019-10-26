using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atrx.WindowsPhone.Medibox
{
    public class MedicinesManager
    {
        //
        // Zwraca listę leków wszystkich leków posortowanych alfabetycznie
        //
        public List<Medicine> GetMedicinesSortAlphabetically()
        {
            /*
            * CEL:
            * Zwraca listę leków wszystkich leków posortowanych alfabetycznie
            */

            // Lista zwróconych cytatów
            List<Medicine> medicines = null;

            // Zwraca listę leków posortowanych alfabetycznie
            using (MediboxDataContext dc = new MediboxDataContext(MediboxDataContext.DBConnectionString))
            {
                medicines = (from med in dc.MedicinesTable orderby med.MedicinName ascending select med).ToList();
            }

            // Zwróć leki
            return medicines;
        }
    }
}
