﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Atrx.WindowsPhone.Medibox
{
    public class MediboxDataContext : DataContext
    {
        // Connection string
        public static string DBConnectionString = @"Data Source=isostore:/MediboxData.sdf";

        //
        // Konstruktor
        //
        public MediboxDataContext(string connectionString)
            : base(connectionString) { }

        // Zwróć tabele Lekarstw
        public Table<Medicine> MedicinesTable;

        // Zwróć tabelę Zadań
        public Table<MediTask> MediTasksTable;
    }
}
