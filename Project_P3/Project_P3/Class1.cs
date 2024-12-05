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
//using System.Collections.Immutable;

namespace Project_P3 
{
    internal class Class1
    {
        CultureInfo cultura = new CultureInfo("es-ES");
        //public static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();

        public static void ExecuteCode(string filePath1, string filePath2, string filePath3, string filePath4, string filePath5)
        {
            //Read CSV with take offs
            string path = filePath1;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            List<Aircraft> aircrafts_deplist = new List<Aircraft>();

            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows; // Number of rows in excel

                for (int row = 2; row <= rowCount; row++) // Skip header row
                {
                    var rutaSACTA = worksheet.Cells[row, 4].Text;

                    var aircraft = new Aircraft
                    {
                        id = worksheet.Cells[row, 1].Text,
                        Indicativo = worksheet.Cells[row, 2].Text,
                        HoraDespegue = DateTime.Parse(worksheet.Cells[row, 3].Text),
                        RutaSACTA = string.Join(", ", rutaSACTA.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)),
                        TipoAeronave = worksheet.Cells[row, 5].Text,
                        Estela = worksheet.Cells[row, 6].Text,
                        ProcDesp = worksheet.Cells[row, 7].Text,
                        PistaDespegue = worksheet.Cells[row, 8].Text
                    };


                    aircrafts_deplist.Add(aircraft);
                }


            }



            //Read CSV FROM ASTERIX

            // P2
            string astpath = filePath2;
            //string astpath = "P3_08_12h_atenea.csv";

            List<ASTmessage> aircrafts_ast = new List<ASTmessage>();

            //Read excel rows
            string[] line = System.IO.File.ReadAllLines(astpath);

            int i = 1;

            while (i < line.Length)
            {
                //Split line and save each property of the aircraft
                //P2
                string[] fields = Regex.Split(line[i], "\t");

                string altitude_csv = fields[26].Trim('"');

                //check altitude
                if (fields[26].Trim('"') != "" && fields[26].Trim('"') != "N/A")
                {
                    //Get mode C corrected (ft)
                    altitude_csv = fields[26].Trim('"');
                }

                else
                {
                    //Get FL to H (ft)
                    altitude_csv = Convert.ToString(Convert.ToDouble(fields[25].Trim('"')) * 100);
                }

                // Format de prova
                //string[] fields = Regex.Split(line[i], ";");
                var message = new ASTmessage

                //Save each message in list
                {
                    //P2
                    time = fields[2].Trim('"'),
                    time_s = fields[3].Trim('"'),
                    lat = fields[4].Trim('"'),
                    lon = fields[5].Trim('"'),
                    altitude = altitude_csv,
                    rho = fields[18].Trim('"'),
                    theta = fields[19].Trim('"'),
                    mode3A = fields[22].Trim('"'),
                    FL = fields[25].Trim('"'),
                    TA = fields[34].Trim('"').Trim(),
                    TI = fields[35].Trim('"').Trim(),
                    BP = fields[38].Trim('"'),
                    RA = fields[43].Trim('"'),
                    TTA = fields[44].Trim('"'),
                    GS = fields[45].Trim('"'),
                    TAR = fields[46].Trim('"'),
                    TAS = fields[47].Trim('"'),
                    HDG = fields[48].Trim('"'),
                    IAS = fields[49].Trim('"'),
                    BAR = fields[51].Trim('"'),
                    IVV = fields[52].Trim('"'),
                    TN = fields[53].Trim('"'),
                    HEADING = fields[57].Trim('"'),
                    stat_230 = fields[69].Trim('"'),



                    // Format de prova
                    /*time = fields[0].Trim('"'),
                    time_s = fields[1].Trim('"'),
                    lat = fields[2].Trim('"'),
                    lon = fields[3].Trim('"'),
                    height = fields[4].Trim('"'),
                    rho = fields[5].Trim('"'),
                    theta = fields[6].Trim('"'),
                    mode3A = fields[7].Trim('"'),
                    FL = fields[8].Trim('"'),
                    TA = fields[9].Trim('"').Trim(),
                    TI = fields[10].Trim('"').Trim(),
                    BP = fields[11].Trim('"'),
                    RA = fields[12].Trim('"'),
                    TTA = fields[13].Trim('"'),
                    GS = fields[14].Trim('"'),
                    TAR = fields[15].Trim('"'),
                    TAS = fields[16].Trim('"'),
                    HDG = fields[17].Trim('"'),
                    IAS = fields[18].Trim('"'),
                    BAR = fields[19].Trim('"'),
                    IVV = fields[20].Trim('"'),
                    TN = fields[21].Trim('"'),
                    HEADING = fields[22].Trim('"'),
                    stat_230 = fields[23].Trim('"'),
                    */
                };

                aircrafts_ast.Add(message);
                i += 1;


            }

            // Initialize aircraft propulsion performance

            string path_propulsion = filePath3;
            var propulsion = new Prop_CLassification();

            //string[] line_prop = System.IO.File.ReadAllLines(path_propulsion);
            using (var package_prop = new ExcelPackage(new FileInfo(path_propulsion)))
            {
                var worksheet_prop = package_prop.Workbook.Worksheets[0];
                int rowCount = worksheet_prop.Dimension.Rows; // Number of rows in excel

                for (int row = 2; row <= rowCount; row++) // Skip header row
                {
                    if (worksheet_prop.Cells[row, 1].Text != "")
                    {
                        propulsion.HP.Add(worksheet_prop.Cells[row, 1].Text);
                    }
                    if (worksheet_prop.Cells[row, 2].Text != "")
                    {
                        propulsion.NR.Add(worksheet_prop.Cells[row, 2].Text);
                    }
                    if (worksheet_prop.Cells[row, 3].Text != "")
                    {
                        propulsion.NR_plus.Add(worksheet_prop.Cells[row, 3].Text);
                    }
                    if (worksheet_prop.Cells[row, 4].Text != "")
                    {
                        propulsion.NR_minus.Add(worksheet_prop.Cells[row, 4].Text);
                    }
                    if (worksheet_prop.Cells[row, 5].Text != "")
                    {
                        propulsion.LP.Add(worksheet_prop.Cells[row, 5].Text);
                    }
                }
            }

            string path_SID_06R = filePath4;
            var SID_06R = new SID();
            using (var package_SID_06R = new ExcelPackage(new FileInfo(path_SID_06R)))
            {
                var worksheet_SID_06R = package_SID_06R.Workbook.Worksheets[0];
                int rowCount = worksheet_SID_06R.Dimension.Rows; // Number of rows in excel

                for (int row = 2; row <= rowCount; row++) // Skip header row
                {
                    if (worksheet_SID_06R.Cells[row, 1].Text != "")
                    {
                        SID_06R.G1.Add(worksheet_SID_06R.Cells[row, 1].Text);
                    }
                    if (worksheet_SID_06R.Cells[row, 2].Text != "")
                    {
                        SID_06R.G2.Add(worksheet_SID_06R.Cells[row, 2].Text);
                    }
                    if (worksheet_SID_06R.Cells[row, 3].Text != "")
                    {
                        SID_06R.G3.Add(worksheet_SID_06R.Cells[row, 3].Text);
                    }
                }

            }

            string path_SID_24L = filePath5;
            var SID_24L = new SID();
            using (var package_SID_24L = new ExcelPackage(new FileInfo(path_SID_24L)))
            {
                var worksheet_SID_24L = package_SID_24L.Workbook.Worksheets[0];
                int rowCount = worksheet_SID_24L.Dimension.Rows; // Number of rows in excel

                for (int row = 2; row <= rowCount; row++) // Skip header row
                {
                    if (worksheet_SID_24L.Cells[row, 1].Text != "")
                    {
                        SID_24L.G1.Add(worksheet_SID_24L.Cells[row, 1].Text);
                    }
                    if (worksheet_SID_24L.Cells[row, 2].Text != "")
                    {
                        SID_24L.G2.Add(worksheet_SID_24L.Cells[row, 2].Text);
                    }
                    if (worksheet_SID_24L.Cells[row, 3].Text != "")
                    {
                        SID_24L.G3.Add(worksheet_SID_24L.Cells[row, 3].Text);
                    }
                }

            }


            //Check aircrafts from asterix file have departured from LEBL, delete the rest

            List<ASTmessage> filtered_asterix = new List<ASTmessage>();
            foreach (var message in aircrafts_ast)
            {
                foreach (var aircraft in aircrafts_deplist)
                {

                    string timeString3 = message.time;
                    timeString3 = timeString3.Trim('"');
                    TimeSpan parsedTime3 = DateTime.ParseExact(timeString3, "HH:mm:ss:fff", CultureInfo.InvariantCulture).TimeOfDay;
                    TimeSpan aircraftTime = aircraft.HoraDespegue.TimeOfDay;
                    TimeSpan timedifference = aircraftTime - parsedTime3;

                    if (aircraft.Indicativo == message.TI && (aircraft.PistaDespegue == "LEBL-06R" || aircraft.PistaDespegue == "LEBL-24L") && Math.Abs(timedifference.TotalMinutes) < 30)
                    {

                        //Add new proporties of ASTmessage from DEP csv
                        message.RutaSACTA = aircraft.RutaSACTA;
                        message.TipoAeronave = aircraft.TipoAeronave;
                        message.Estela = aircraft.Estela;
                        message.horaSACTA = aircraft.HoraDespegue;
                        Function Func_prop = new Function();
                        Func_prop.setPropulsion(message, propulsion);
                        message.ProcDesp = aircraft.ProcDesp;
                        message.PistaDespegue = aircraft.PistaDespegue;
                        Func_prop.setSID(message, SID_06R, SID_24L);
                        message.viraje = false;
                        Func_prop.SetAirline(message);
                        filtered_asterix.Add(message);
                    }
                }
            }

            double minUmbral = 0.5;
            var umbral_06 = new ASTmessage();
            umbral_06.lat = Convert.ToString(41.0 + (16.0 / 60.0) + (56.32 / 3600.0));
            umbral_06.lon = Convert.ToString(2.0 + (4.0 / 60.0) + (27.0 / 3600.0));
            Function f_06 = new Function();
            f_06.Getstereographic(umbral_06);
            //Rectangle thr 06
            double latMax_06 = 41 + 17.0 / 60.0 + 33.0 / 3600;
            double latMin_06 = 41 + 17.0 / 60.0 + 29.0 / 3600;
            double lonMax_06 = 2 + 6.0 / 60.0 + 7.0 / 3600.0;
            double lonMin_06 = 2 + 6.0 / 60.0 + 13.0 / 3600.0;

            var polygon_DEP06R = new List<(double lat, double lon)>
            {
                (41.2921290, 2.1038453),  // Esquina 1 (P1)
                (41.2926079, 2.1035570),  // Esquina 2 (P2)
                (41.2922937, 2.1026926),  // Esquina 3 (P3)
                (41.2918259, 2.1029718)   // Esquina 4 (P4)
            };


            var umbral_24 = new ASTmessage();
            umbral_24.lat = Convert.ToString(41 + (16.0 / 60.0) + (56.32 / 3600));
            umbral_24.lon = Convert.ToString(2 + (4.0 / 60.0) + (27.66 / 3600));
            Function f_24 = new Function();
            f_24.Getstereographic(umbral_24);
            //Rectangle thr 24
            //double latMax_24 = 41.2823617;
            //double latMin_24 = 41.2819848;
            //double lonMax_24 = 2.0753448;
            //double lonMin_24 = 2.0739664;

            var polygon_DEP24L = new List<(double lat, double lon)>
            {
                (41.2823617, 2.0753448),  // Esquina 1 (P1)
                (41.2828379, 2.0750503),  // Esquina 2 (P2)
                (41.2824615, 2.0739664),  // Esquina 3 (P3)
                (41.2819848, 2.0742412)   // Esquina 4 (P4)
            };


            GeoUtils geoUtils = new GeoUtils();
            double max = 0;
            Function f = new Function();

            List<string> TA_detected = new List<string>();



            // Crear un diccionario para acceder rápidamente a los mensajes de AST
            Dictionary<string, List<ASTmessage>> aircraftPositions = new Dictionary<string, List<ASTmessage>>();

            Dictionary<string, List<ASTmessage>> interpolatedPositions = new Dictionary<string, List<ASTmessage>>();


            // Almacenar los mensajes de AST en el diccionario
            foreach (var message in filtered_asterix)
            {
                if (!aircraftPositions.ContainsKey(message.TI))
                {
                    aircraftPositions[message.TI] = new List<ASTmessage>();
                }
                aircraftPositions[message.TI].Add(message);
            }

            foreach (var flight in aircraftPositions)
            {

                List<ASTmessage> message = flight.Value;

                // Order by time
                message = message.OrderBy(m => m.time).ToList();

                bool zoneTWR_firstdetect = false;



                for (int j = 0; j < message.Count - 1; j++)
                {
                    var message_ = message[j];



                    //Get stereographic coordinates
                    f.Getstereographic(message[j]);

                    //Calculate distance to thresholds and set it more than 0.5 NM
                    if (message[j].PistaDespegue == "LEBL-06R")
                    {
                        //Initialize first detection from 0.5 NM as true
                        double dist_umbral_24 = f.calculate_distance(message[j], umbral_24);
                        string timeString = message[j].time;
                        DateTime parsedTime = DateTime.ParseExact(timeString, "HH:mm:ss:fff", CultureInfo.InvariantCulture);

                        //Check dist > 0.5 and time is after flight plan activated
                        if (dist_umbral_24 > minUmbral && parsedTime.TimeOfDay > message[j].horaSACTA.TimeOfDay)
                        {
                            message[j].initialize_dist = true;

                        }
                        else
                        {
                            message[j].initialize_dist = false;
                        }

                        // If initialized and first detection is false, set as first detection -> zone TWR
                        if (message[j].initialize_dist && !zoneTWR_firstdetect)
                        {
                            message[j].zone = "TWR";
                            zoneTWR_firstdetect = true;
                        }

                        //if first detection already occured, zone TMA
                        else if (message[j].initialize_dist && zoneTWR_firstdetect)
                        {
                            message[j].zone = "TMA";
                        }



                    }
                    else if (message[j].PistaDespegue == "LEBL-24L")
                    {

                        //Initialize first detection from 0.5 NM as true
                        double dist_umbral_06 = f.calculate_distance(message[j], umbral_06);
                        string timeString2 = message[j].time;
                        DateTime parsedTime2 = DateTime.ParseExact(timeString2, "HH:mm:ss:fff", CultureInfo.InvariantCulture);

                        //Check dist > 0.5 and time is after flight plan activated
                        if (dist_umbral_06 > minUmbral && parsedTime2.TimeOfDay > message[j].horaSACTA.TimeOfDay)
                        {
                            message[j].initialize_dist = true;

                        }
                        else
                        {
                            message[j].initialize_dist = false;
                        }

                        // If initialized and first detection is false, set as first detection -> zone TWR
                        if (message[j].initialize_dist && !zoneTWR_firstdetect)
                        {
                            message[j].zone = "TWR";
                            zoneTWR_firstdetect = true;
                        }

                        //if first detection already occured, zone TMA
                        else if (message[j].initialize_dist && zoneTWR_firstdetect)
                        {
                            message[j].zone = "TMA";
                        }

                    }
                }
            }


            //INTERPOLACIONES
            foreach (var entry in aircraftPositions)
            {
                string aircraftId = entry.Key;
                List<ASTmessage> messages = entry.Value;
                ASTmessage example_message = messages[0];
                // Order by time
                messages = messages.OrderBy(m => m.time).ToList();

                // Inicializar la lista sino esta
                if (!interpolatedPositions.ContainsKey(aircraftId))
                {
                    interpolatedPositions[aircraftId] = new List<ASTmessage>();
                }

                // Recorrer cada par de mensajes consecutivos
                for (int i3 = 0; i3 < messages.Count - 1; i3++)
                {
                    var message1 = messages[i3];
                    var message2 = messages[i3 + 1];

                    // Añadir el mensaje original al diccionario interpolado
                    interpolatedPositions[aircraftId].Add(message1);

                    // Interpolar en intervalos de 1 segundo entre message1 y message2
                    double tiempo1 = Convert.ToDouble(message1.time_s);
                    double tiempo2 = Convert.ToDouble(message2.time_s);

                    for (double targetTime = tiempo1 + 1; targetTime < tiempo2; targetTime += 1)
                    {
                        double factor = (targetTime - tiempo1) / (tiempo2 - tiempo1);

                        // Interpolar latitud, longitud 
                        double interpolatedLat = Convert.ToDouble(message1.lat) + factor * (Convert.ToDouble(message2.lat) - Convert.ToDouble(message1.lat));
                        double interpolatedLon = Convert.ToDouble(message1.lon) + factor * (Convert.ToDouble(message2.lon) - Convert.ToDouble(message1.lon));

                        //Interpolar ALT
                        double interpolatedAlt = Convert.ToDouble(message1.altitude);

                        if (message1.IVV != "N/A" && message2.IVV != "N/A")
                        {
                            interpolatedAlt = Convert.ToDouble(message1.altitude) + factor * Convert.ToDouble(message1.IVV) * (0.3048 / 60) * (tiempo2 - tiempo1);
                        }

                        else
                        {
                            interpolatedAlt = Convert.ToDouble(message1.altitude) + factor * (Convert.ToDouble(message2.altitude) - Convert.ToDouble(message1.altitude));

                        }


                        string interpolatedIAS;

                        //Interpolar IAS
                        if (message1.IAS != "N/A" && message2.IAS != "N/A")
                        {

                            interpolatedIAS = Convert.ToString(Convert.ToDouble(message1.IAS) + factor * (Convert.ToDouble(message2.IAS) - Convert.ToDouble(message1.IAS)));

                        }

                        else
                        {
                            interpolatedIAS = "N/A";
                        }

                        //Interpolar heading
                        double interpolatedHDG = Convert.ToDouble(message1.HEADING) + factor * (Convert.ToDouble(message2.HEADING) - Convert.ToDouble(message1.HEADING));

                        ASTmessage interpolatedMessage = new ASTmessage
                        {
                            TI = aircraftId,
                            lat = Convert.ToString(interpolatedLat),
                            lon = Convert.ToString(interpolatedLon),
                            altitude = Convert.ToString(interpolatedAlt),
                            time_s = Convert.ToString(targetTime),
                            time = TimeSpan.FromSeconds((double)targetTime).ToString(@"hh\:mm\:ss\:fff"),
                            HEADING = Convert.ToString(interpolatedHDG),
                            ProcDesp = example_message.ProcDesp,
                            SID = example_message.SID,
                            TipoAeronave = example_message.TipoAeronave,
                            Airline = example_message.Airline,
                            PistaDespegue = example_message.PistaDespegue,
                            IAS = interpolatedIAS,
                            horaSACTA = example_message.horaSACTA,
                        };

                        // Add to list
                        interpolatedPositions[aircraftId].Add(interpolatedMessage);
                    }
                }

                // Añadir el último mensaje original
                interpolatedPositions[aircraftId].Add(messages.Last());
                List<ASTmessage> messages_trial = interpolatedPositions[aircraftId];
            }

            

           

            //VIRAJES

            //  Table para virajes
            DataTable virajes_Table = new DataTable("Incumplimientos");
            virajes_Table.Columns.Add("Aircraft ID", typeof(string));
            virajes_Table.Columns.Add("Heading Difference [°]", typeof(double));
            virajes_Table.Columns.Add("Roll Angle [°]", typeof(double));
            virajes_Table.Columns.Add("Aircraft latitude [°]", typeof(double));
            virajes_Table.Columns.Add("Aircraft longitude [°]", typeof(double));
            virajes_Table.Columns.Add("Altitude [ft]", typeof(double));
            virajes_Table.Columns.Add("Time [hh:mm:ss:fff]", typeof(string));
            virajes_Table.Columns.Add("Radial to DVOR BCN", typeof(double));
            virajes_Table.Columns.Add("Uncompliment with SID", typeof(string));
            virajes_Table.Columns.Add("SID", typeof(string));
            virajes_Table.Columns.Add("Grupo SID", typeof(string));
            virajes_Table.Columns.Add("Aircraft type", typeof(string));
            virajes_Table.Columns.Add("Airline", typeof(string));


            //DVOR
            double Position_DVOR_lat = 41 + (18.0 / 60.0) + (25.6 / 3600.0);
            double Position_DVOR_lon = 2 + (6.0 / 60.0) + (28.1 / 3600.0);
            ASTmessage SID_coast_point = new ASTmessage();
            SID_coast_point.lat = Convert.ToString(41 + (16.0 / 60.0) + (5.4 / 3600.0));
            SID_coast_point.lon = Convert.ToString(2 + (2.0 / 60.0) + (0.0 / 3600.0));
            double SID_line = f.calculate_radial(SID_coast_point, Position_DVOR_lat, Position_DVOR_lon);



            //SONOMETRO
            double Position_sonometro_lat = 41.27194444440;
            double Position_sonometro_lon = 2.04777777778;
            ASTmessage sonometro = new ASTmessage();
            sonometro.lat = Convert.ToString(Position_sonometro_lat);
            sonometro.lon = Convert.ToString(Position_sonometro_lon);
            f.Getstereographic(sonometro);

            DataTable Sonometro_Table = new DataTable("Minimum distance sonometro");
            Sonometro_Table.Columns.Add("Aircraft ID", typeof(string));
            Sonometro_Table.Columns.Add("Distance sonometro [NM]", typeof(double));
            Sonometro_Table.Columns.Add("Aircraft latitude [°]", typeof(string));
            Sonometro_Table.Columns.Add("Aircraft longitude [°]", typeof(string));
            Sonometro_Table.Columns.Add("Time [hh:mm:ss:fff]", typeof(string));
            Sonometro_Table.Columns.Add("SID", typeof(string));
            Sonometro_Table.Columns.Add("Group SID", typeof(string));
            Sonometro_Table.Columns.Add("Aircraft type", typeof(string));
            Sonometro_Table.Columns.Add("Airline", typeof(string));


            // THRESHOLD IAS & ALT
            DataTable Threshold_Table = new DataTable("IAS and ALT at threshold");
            Threshold_Table.Columns.Add("Aircraft ID", typeof(string));
            Threshold_Table.Columns.Add("Runway", typeof(string));
            Threshold_Table.Columns.Add("Detected in threshold", typeof(string));
            Threshold_Table.Columns.Add("Aircraft latitude [°]", typeof(string));
            Threshold_Table.Columns.Add("Aircraft longitude [°]", typeof(string));
            Threshold_Table.Columns.Add("Altitude [ft]", typeof(string));
            Threshold_Table.Columns.Add("IAS [kt]", typeof(string));
            Threshold_Table.Columns.Add("Time [hh:mm:ss:fff]", typeof(string));
            Threshold_Table.Columns.Add("SID", typeof(string));
            Threshold_Table.Columns.Add("Group SID", typeof(string));
            Threshold_Table.Columns.Add("Aircraft Type", typeof(string));
            Threshold_Table.Columns.Add("Airline", typeof(string));

            // IAS at 850 ft, 1500 ft and 3500 ft IAS & ALT
            DataTable Altitudes_Table = new DataTable("IAS at 850 ft, 1500 ft and 3500 ft.");
            Altitudes_Table.Columns.Add("Aircraft ID", typeof(string));
            Altitudes_Table.Columns.Add("IAS at 850 ft [kt]", typeof(string));
            Altitudes_Table.Columns.Add("IAS at 1500 ft [kt]", typeof(string));
            Altitudes_Table.Columns.Add("IAS at 3500 ft [kt]", typeof(string));
            Altitudes_Table.Columns.Add("SID", typeof(string));
            Altitudes_Table.Columns.Add("Group SID", typeof(string));
            Altitudes_Table.Columns.Add("Aircraft type", typeof(string));
            Altitudes_Table.Columns.Add("Airline", typeof(string));

            int cont_viraje = 0;

            foreach (var aircraft_vir in interpolatedPositions)
            {
                //Acces each aircraft in diccionary
                string aircraftID = aircraft_vir.Key;

                if (aircraftID == "SWT8168")
                {
                    Console.WriteLine("GGGGG");
                }
                //Acces each messages of aircraft
                List<ASTmessage> messages_vir = aircraft_vir.Value;

                bool detectedInThreshold = false; // Variable para rastrear si el avión fue detectado en el threshold

                bool out_of_range_vir = false;
                bool out_of_range_alt = false;
                double min_distance_sonometro = 1000; //High number
                double distance_sonometro = 0;
                string position_min_dist_lat = "";
                string position_min_dist_lon = "";
                string time_min_distance = "";
                string proc_desp = "";
                string groupSID = "";
                string typeaircraft = "";
                string airline = "";
                bool viraje_ended = false;

                string runway_aircraft = messages_vir[cont_viraje].PistaDespegue;
                string SIDgroup_aircraft = messages_vir[cont_viraje].SID;
                string SID_aircraft = messages_vir[cont_viraje].ProcDesp;
                string airline_aircraft = messages_vir[cont_viraje].Airline;
                string aircraftype_aircraft = messages_vir[cont_viraje].TipoAeronave;

                while (cont_viraje < messages_vir.Count - 1)
                {


                    ASTmessage aircraft_position1 = messages_vir[cont_viraje];
                    ASTmessage aircraft_position2 = messages_vir[cont_viraje + 1];



                    bool alt850 = false;
                    bool alt1500 = false;
                    bool alt3500 = false;


                    //Virajes only 24L
                    if (aircraft_position1.PistaDespegue == "LEBL-24L")
                    {
                        if ((aircraft_position1.HEADING != null && aircraft_position1.HEADING != "N/A") || !out_of_range_vir)  // Es podria treure perque en teoria ja no hi ha nulls per interpolacio
                        {
                            int cont_vir = cont_viraje;
                            while ((aircraft_position2.HEADING == null || aircraft_position2.HEADING == "N/A") && !out_of_range_vir)
                            {
                                cont_vir++;
                                if ((cont_vir + 1) == (messages_vir.Count - 1))
                                {
                                    out_of_range_vir = true;
                                    continue;
                                }
                                aircraft_position2 = messages_vir[cont_vir + 1];
                            }

                            //viraje
                            double HDG_difference = Math.Abs(Convert.ToDouble(aircraft_position1.HEADING) - Convert.ToDouble(aircraft_position2.HEADING));

                            bool viraje_started = f.IsVirajeRecorded(virajes_Table, aircraft_position1.TI);

                            DateTime dateTime = DateTime.ParseExact(aircraft_position2.time, "HH:mm:ss:fff", CultureInfo.InvariantCulture);
                            double RA;
                            if (aircraft_position2.RA == "N/A" || aircraft_position2.RA == null)
                            {
                                RA = 0;
                            }
                            else
                            {
                                RA = Convert.ToDouble(aircraft_position2.RA);
                            }
                            if ((HDG_difference >= 0.8 || Math.Abs(RA) >= 4) && viraje_started == false &&
                                ((dateTime.TimeOfDay > aircraft_position1.horaSACTA.TimeOfDay)
                                || (aircraft_position1.horaSACTA.TimeOfDay.TotalSeconds - dateTime.TimeOfDay.TotalSeconds <= 10)))
                            {
                                //Set viraje started
                                aircraft_position1.viraje = true;
                                viraje_started = true;

                                double radial_position = f.calculate_radial(aircraft_position2, Position_DVOR_lat, Position_DVOR_lon);


                                virajes_Table.Rows.Add(
                                aircraft_position2.TI,
                                HDG_difference,
                                RA,
                                aircraft_position2.lat,
                                aircraft_position2.lon,
                                aircraft_position2.altitude,
                                aircraft_position1.time,
                                radial_position,
                                false,
                                aircraft_position2.ProcDesp,
                                aircraft_position2.SID,
                                aircraft_position2.TipoAeronave,
                                aircraft_position2.Airline
                                    );

                            }



                            if (viraje_started && !viraje_ended)
                            {
                                if (HDG_difference <= 0.2)
                                {
                                    viraje_ended = true;
                                }

                                if (HDG_difference >= 1.52)
                                {
                                    double radial_position = f.calculate_radial(aircraft_position1, Position_DVOR_lat, Position_DVOR_lon);
                                    if (radial_position > SID_line)
                                    {
                                        DataRow[] row_vir = virajes_Table.Select($"[Aircraft ID] = '{aircraft_position1.TI}'");
                                        row_vir[0]["Uncompliment with SID"] = "True";
                                    }
                                }

                            }

                        }
                    }

                    //threshold
                    double lat_aircraft = Convert.ToDouble(aircraft_position1.lat);
                    double lon_aircraft = Convert.ToDouble(aircraft_position1.lon);
                    double distance_aircraft_thr;

                    // Comprobar si el ID ya está en la tabla
                    bool idExists = Threshold_Table.AsEnumerable().Any(row => row["Aircraft ID"].ToString() == aircraft_position1.TI);
                    bool aircraft_inTHR;

                    if (aircraft_position1.PistaDespegue == "LEBL-24L")

                    {

                        //Check if in THR
                        aircraft_inTHR = f.IsAircraftInTHR(Convert.ToDouble(aircraft_position1.lat), Convert.ToDouble(aircraft_position1.lon), polygon_DEP24L);
                        //Dist sonometro
                        distance_sonometro = f.calculate_distance(aircraft_position1, sonometro);
                        if (distance_sonometro < min_distance_sonometro)
                        {
                            min_distance_sonometro = distance_sonometro;
                            position_min_dist_lat = aircraft_position1.lat;
                            position_min_dist_lon = aircraft_position1.lon;
                            time_min_distance = aircraft_position1.time;
                            proc_desp = aircraft_position1.ProcDesp;
                            groupSID = aircraft_position1.SID;
                            typeaircraft = aircraft_position1.TipoAeronave;
                            airline = aircraft_position1.Airline;
                        }


                    }

                    else
                    {
                        //Aircraft in 06R
                        aircraft_inTHR = f.IsAircraftInTHR(Convert.ToDouble(aircraft_position1.lat), Convert.ToDouble(aircraft_position1.lon), polygon_DEP06R);
                    }

                    if (aircraft_inTHR && !idExists && aircraft_position1.IAS != "N/A")
                    {
                        //Set boolean as detected in THR
                        detectedInThreshold = true;

                        Threshold_Table.Rows.Add(
                            aircraft_position1.TI,
                            aircraft_position1.PistaDespegue,
                            "Detected",
                            Convert.ToString(aircraft_position1.lat),
                            Convert.ToString(aircraft_position1.lon),
                            Convert.ToString(Math.Round(Convert.ToDouble(aircraft_position1.altitude))),
                            Convert.ToString(Math.Round(Convert.ToDouble(aircraft_position1.IAS))),
                            aircraft_position1.time,
                            aircraft_position1.ProcDesp,
                            aircraft_position1.SID,
                            aircraft_position1.TipoAeronave,
                            aircraft_position1.Airline
                            );
                    }


                    // IAS at 850 ft, 1500 ft, 3500 ft

                    // Comprobar si el ID ya está en la tabla
                    bool idExistsAlt = Altitudes_Table.AsEnumerable().Any(row => row["Aircraft ID"].ToString() == aircraft_position1.TI);

                    int cont_alt = cont_viraje;
                    if (aircraft_position1.IAS == null || aircraft_position1.IAS == "N/A")
                    {
                        cont_viraje++;
                        continue;
                    }
                    while ((aircraft_position2.IAS == null || aircraft_position2.IAS == "N/A") && !out_of_range_alt && cont_alt < messages_vir.Count - 2)
                    {
                        cont_alt++;
                        if ((cont_alt + 1) == (messages_vir.Count - 1))
                        {
                            out_of_range_alt = true;
                            continue;
                        }
                        aircraft_position2 = messages_vir[cont_alt + 1];
                    }
                    if (out_of_range_alt)
                    {
                        cont_viraje++;
                        continue;
                    }

                    try
                    {
                        double alt_1 = Convert.ToDouble(aircraft_position1.altitude);
                        double alt_2 = Convert.ToDouble(aircraft_position2.altitude);
                        double IAS1 = Convert.ToDouble(aircraft_position1.IAS);
                        double IAS2 = Convert.ToDouble(aircraft_position2.IAS);
                        double IAS = 0;

                        if (alt_1 <= 850 && alt_2 > 850) // Convert feets required to meters
                        {
                            IAS = f.compute_IAS(alt_1, alt_2, 850, IAS1, IAS2);
                            alt850 = true;
                        }

                        if (alt_1 <= 1500 && alt_2 > 1500) // Convert feets required to meters
                        {
                            IAS = f.compute_IAS(alt_1, alt_2, 1500, IAS1, IAS2);
                            alt1500 = true;
                        }

                        if (alt_1 <= 3500 && alt_2 > 3500) // Convert feets required to meters
                        {
                            IAS = f.compute_IAS(alt_1, alt_2, 3500, IAS1, IAS2);
                            alt3500 = true;
                        }


                        if ((alt850 || alt1500 || alt3500) && !idExistsAlt)
                        {
                            if (alt850)
                            {
                                if (IAS == 0)
                                {
                                    Console.WriteLine("NULL IAS");
                                }
                                Altitudes_Table.Rows.Add(
                                aircraft_position1.TI,
                                Convert.ToString(IAS),
                                "-",
                                "-",
                                aircraft_position1.ProcDesp,
                                aircraft_position1.SID,
                                aircraft_position1.TipoAeronave,
                                aircraft_position1.Airline);
                            }
                            else if (alt1500)
                            {

                                if (IAS == 0)
                                {
                                    Console.WriteLine("NULL IAS");
                                }
                                Altitudes_Table.Rows.Add(
                                aircraft_position1.TI,
                                "-",
                                Convert.ToString(IAS),
                                "-",
                                aircraft_position1.ProcDesp,
                                aircraft_position1.SID,
                                aircraft_position1.TipoAeronave,
                                aircraft_position1.Airline)
                                ;
                            }
                            else
                            {

                                if (IAS == 0)
                                {
                                    Console.WriteLine("NULL IAS");
                                }
                                Altitudes_Table.Rows.Add(
                                aircraft_position1.TI,
                                "-",
                                "-",
                                Convert.ToString(IAS),
                                aircraft_position1.ProcDesp,
                                aircraft_position1.SID,
                                aircraft_position1.TipoAeronave,
                                aircraft_position1.Airline);
                            }
                        }
                        else if ((alt850 || alt1500 || alt3500) && idExistsAlt)
                        {

                            DataRow[] row = Altitudes_Table.Select($"[Aircraft ID] = '{aircraft_position1.TI}'");
                            if (alt1500)
                            {

                                if (IAS == 0)
                                {
                                    Console.WriteLine("NULL IAS");
                                }
                                row[0]["IAS at 1500 ft [kt]"] = Convert.ToString(IAS);
                            }

                            else if (alt3500)
                            {

                                if (IAS == 0)
                                {
                                    Console.WriteLine("NULL IAS");
                                }
                                row[0]["IAS at 3500 ft [kt]"] = Convert.ToString(IAS);
                            }
                        }
                    }

                    catch (Exception ex) { }


                    cont_viraje++;

                }

                // If aircraft not detected in THR after all messages
                if (!detectedInThreshold)
                {
                    Threshold_Table.Rows.Add(
                        aircraftID, // ID del avión
                        runway_aircraft,      // Runway
                        "Not Detected", // Detected in threshold
                        "N/A",      // Aircraft latitude
                        "N/A",      // Aircraft longitude
                        "N/A",      // Altitude
                        "N/A",      // IAS
                        "N/A",      // Time
                        SID_aircraft,      // SID
                        SIDgroup_aircraft,      // Group SID
                        aircraftype_aircraft,      // Aircraft type
                        airline_aircraft       // Airline
                    );
                }

                if (runway_aircraft == "LEBL-24L")
                {
                    //Add to sonometro table
                    Sonometro_Table.Rows.Add(
                    aircraftID,
                    min_distance_sonometro,
                    position_min_dist_lat,
                    position_min_dist_lon,
                    time_min_distance,
                    proc_desp,
                    groupSID,
                    typeaircraft,
                    airline
                    );
                }

                cont_viraje = 0;

            }


            Function f4 = new Function();
            f4.GenerarKML(virajes_Table, "posiciones_viraje.kml");

            //Check distances 
            DataTable table = new DataTable("Loss of Distances");
            table.Columns.Add("Callsign aircraft ahead", typeof(string));
            table.Columns.Add("Callsign aircraft behind", typeof(string));
            table.Columns.Add("Distance [NM]", typeof(string));
            table.Columns.Add("Type of loss", typeof(string));
            table.Columns.Add("Time [hh:mm:ss:fff]", typeof(string));
            table.Columns.Add("Zone", typeof(string));
            table.Columns.Add("SID aircraft ahead", typeof(string));
            table.Columns.Add("SID aircraft behind", typeof(string));
            table.Columns.Add("Group SID aircraft ahead", typeof(string));
            table.Columns.Add("Group SID aircraft behind", typeof(string));
            table.Columns.Add("Type aircraft ahead", typeof(string));
            table.Columns.Add("Type aircraft behind", typeof(string));
            table.Columns.Add("Airline aircraft ahead", typeof(string));
            table.Columns.Add("Airline aircraft behind", typeof(string));

            double SEP_minima_radar = 3;  //3NM

            Function Function = new Function();

            for (int cont2 = 0; cont2 < aircrafts_deplist.Count - 1; cont2++)
            {
                var aircraft_ahead = aircrafts_deplist[cont2];
                var aircraft_behind = aircrafts_deplist[cont2 + 1];

                // Verificar si ambos aviones están en el mismo camino
                if (aircraft_ahead.PistaDespegue != aircraft_behind.PistaDespegue)
                {
                    continue; // Salta si no están en la misma pista
                }

                // Acceder a las posiciones del avión "adelante"
                if (aircraftPositions.TryGetValue(aircraft_ahead.Indicativo, out List<ASTmessage> aheadPositions))
                {

                    foreach (var aircraft_ahead_ast in aheadPositions)
                    {
                        // Acceder a las posiciones del avión "detrás"
                        if (aircraftPositions.TryGetValue(aircraft_behind.Indicativo, out List<ASTmessage> behindPositions) && aircraft_ahead_ast.initialize_dist == true)
                        {
                            foreach (var aircraft_behind_ast in behindPositions)
                            {

                                if (Math.Abs(Convert.ToDouble(aircraft_ahead_ast.time_s) - Convert.ToDouble(aircraft_behind_ast.time_s)) <= 3)
                                {
                                    double distance = Function.calculate_distance(aircraft_ahead_ast, aircraft_behind_ast);
                                    double distance_wake = Function.wake_separation(aircraft_ahead_ast, aircraft_behind_ast);
                                    double distance_LoA = Function.LoA_separation(aircraft_ahead_ast, aircraft_behind_ast);
                                    string zone = aircraft_behind_ast.zone;

                                    if (aircraft_behind_ast.initialize_dist)
                                    {
                                        // Radar
                                        if (distance < SEP_minima_radar)
                                        {
                                            Function.AddViolationIfNotExists(table, aircraft_ahead_ast, aircraft_behind_ast, distance, "Radar", zone);
                                        }

                                        // Estela
                                        if (distance < distance_wake)
                                        {
                                            Function.AddViolationIfNotExists(table, aircraft_ahead_ast, aircraft_behind_ast, distance, "Estela", zone);
                                        }

                                        // LoA
                                        if (aircraft_behind_ast.zone == "TWR" && distance < distance_LoA)
                                        {
                                            Function.AddViolationIfNotExists(table, aircraft_ahead_ast, aircraft_behind_ast, distance, "LoA", zone);
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }

            try
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Select a folder to save the results";

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFolder = folderDialog.SelectedPath;

                        f4.SaveDataTableAsCSV(table, Path.Combine(selectedFolder, "Results_SeparationLoss.csv"));
                        f4.SaveDataTableAsCSV(virajes_Table, Path.Combine(selectedFolder, "Results_TurnInitiation.csv"));
                        f4.SaveDataTableAsCSV(Altitudes_Table, Path.Combine(selectedFolder, "Results_IASatAltitudes.csv"));
                        f4.SaveDataTableAsCSV(Threshold_Table, Path.Combine(selectedFolder, "Results_IASandAltitudeTHR.csv"));
                        f4.SaveDataTableAsCSV(Sonometro_Table, Path.Combine(selectedFolder, "Results_MinDistanceSoundlevelmeter.csv"));

                    }

                }

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting results as CSV: {ex.Message}");
            }





        }
    }
}
