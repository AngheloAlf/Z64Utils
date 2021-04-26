﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#if _WINDOWS
using System.Windows.Forms;
using Z64.Forms;
#endif
using Z64;
using System.Globalization;
using System.Threading;

namespace Z64
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

#if _WINDOWS
            Z64Version.LoadRessources();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
#else

#endif

        }
    }
}