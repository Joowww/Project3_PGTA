using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Functions;
using OfficeOpenXml;
//using Microsoft.VisualBasic.FileIO;
using System.Text.RegularExpressions;
using MultiCAT6.Utils;
using NSpring.Logging;
using NLog;
using System.Data;
using System.Dynamic;
using Accord.Math.Wavelets;
using System.Linq;
using Accord.Math;
using static OfficeOpenXml.ExcelErrorValue;
using OfficeOpenXml.Style.Dxf;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System.Globalization;
using System.Windows.Forms;

namespace Project_P3
{
    internal class Class2
    {
        public static string SaveFiles(List<DataTable> tablas)
        {
            Function f = new Function();
            try
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Select a folder to save the results";

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFolder = folderDialog.SelectedPath;

                        f.SaveDataTableAsCSV(tablas[0], Path.Combine(selectedFolder, "Results_SeparationLoss.csv"));
                        f.SaveDataTableAsCSV(tablas[1], Path.Combine(selectedFolder, "Results_TurnInitiation.csv"));
                        f.SaveDataTableAsCSV(tablas[2], Path.Combine(selectedFolder, "Results_IASatAltitudes.csv"));
                        f.SaveDataTableAsCSV(tablas[3], Path.Combine(selectedFolder, "Results_IASandAltitudeTHR.csv"));
                        f.SaveDataTableAsCSV(tablas[4], Path.Combine(selectedFolder, "Results_MinDistanceSoundlevelmeter.csv"));
                        f.GenerarKML(tablas[1], selectedFolder);

                        return selectedFolder;
                    }
                    else
                    {
                        string result = "0";
                        return result;
                    }

                }

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting results as CSV: {ex.Message}");
                return null;
            }
        }

    }
        
        
}
