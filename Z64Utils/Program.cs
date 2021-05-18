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
            string[] lines = System.IO.File.ReadAllLines("objects_list.txt");
            foreach(string line in lines) {
                if (line.StartsWith("#")) {
                    continue;
                }
                string[] row = line.Split("\t");
                int objectIndex = Convert.ToInt32(row[0], 10);
                string objectName = row[1];
                PrintObjectData(game, objectIndex, objectName);
            }
#endif

        }


        static void PrintObjectData(Z64Game game, int objectIndex, string objectName) {
            Console.Write($"{objectIndex} ~ {objectName}\n");

            Z64File file = game.GetFileFromIndex(objectIndex);
            byte[] data = file.Data;
            Z64Object obj = new Z64Object(data);
            int segmentId = 6;
            Z64ObjectAnalyzer.Config config = new Z64ObjectAnalyzer.Config();
            config.ImprobableOpCodes = new List<F3DZEX.Command.CmdID>()
            {
                F3DZEX.Command.CmdID.G_BRANCH_Z,
                F3DZEX.Command.CmdID.G_CULLDL,
                F3DZEX.Command.CmdID.G_NOOP,
                F3DZEX.Command.CmdID.G_SPNOOP,
                F3DZEX.Command.CmdID.G_LOAD_UCODE,
            };
            Z64ObjectAnalyzer.FindDlists(obj, data, segmentId, config);
            Z64ObjectAnalyzer.AnalyzeDlists(obj, data, segmentId);
            obj.WriteXml($"xmls/{objectName}.xml", objectName, "6");

            foreach (var entry in obj.Entries)
            {
                //Console.Write(entry.Name + "\n");
            }
            //Console.Write("\n");
            Console.Write("\n");

        }
    }
}
