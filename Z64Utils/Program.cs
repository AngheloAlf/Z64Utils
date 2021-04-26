using System;
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

            Z64Version.LoadRessources();
#if _WINDOWS
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
#else
            Z64Game game = new Z64Game("baserom.z64");
            Z64File file = game.GetFileFromIndex(791);
            byte[] data = file.Data;
            Z64Object obj = new Z64Object(data);
            int segmentId = 6;
            Z64ObjectAnalyzer.Config config = new Z64ObjectAnalyzer.Config();
            Z64ObjectAnalyzer.FindDlists(obj, data, segmentId, config);
            Z64ObjectAnalyzer.AnalyzeDlists(obj, data, segmentId);

            //Console.Write(obj.ToString() + "\n");
            //Console.Write(Z64Version.FileTable[Z64VersionEnum.OotEuropeMqDbg][] + "\n");

            foreach (var entry in obj.Entries)
            {
                Console.Write(entry.Name + "\n");
                /*
                var item = listView_map.Items.Add($"{new SegmentedAddress(_segment, _obj.OffsetOf(entry)).VAddr:X8}");
                item.SubItems.Add(entry.Name);
                item.SubItems.Add(entry.GetEntryType().ToString());
                */
            }
#endif

        }
    }
}
