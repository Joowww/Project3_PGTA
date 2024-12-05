//Programa proyecto3
using Accord.Math;
using MultiCAT6.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Diagnostics.Metrics;
//using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Functions
{

    public class Aircraft
    {
        //Aircrafts departing from LEBL
        public string id { get; set; }
        public string Indicativo { get; set; }
        public DateTime HoraDespegue { get; set; }
        public string RutaSACTA { get; set; }
        public string TipoAeronave { get; set; }
        public string Estela { get; set; }
        public string ProcDesp { get; set; }
        public string PistaDespegue { get; set; }
    }
    public class ASTmessage
    {
        //Aircrafts read from CSV asterix
        public string time { get; set; }
        public string time_s { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string altitude { get; set; }
        public string rho { get; set; }
        public string theta { get; set; }
        public string mode3A { get; set; }
        public string FL { get; set; }
        public string TA { get; set; }
        public string TI { get; set; }
        public string BP { get; set; }
        public string RA { get; set; }
        public string TTA { get; set; }
        public string GS { get; set; }
        public string TAR { get; set; }
        public string TAS { get; set; }
        public string HDG { get; set; }
        public string IAS { get; set; }
        public string BAR { get; set; }
        public string IVV { get; set; }
        public string TN { get; set; }
        public string HEADING { get; set; }
        public string stat_230 { get; set; }
        public string ModeC_corrected { get; set; }
        public string x { get; set; }
        public string y { get; set; }
        public bool first_detection { get; set; }
        public bool initialize_dist { get; set; }
        public string RutaSACTA { get; set; }
        public string TipoAeronave { get; set; }
        public string Estela { get; set; }
        public string Propulsion { get; set; }
        public string ProcDesp { get; set; }
        public string SID { get; set; }
        public string PistaDespegue { get; set; }
        public bool viraje { get; set; }
        public double distance_thr { get; set; }
        public DateTime horaSACTA { get; set; }
        public string Airline { get; set; }
        public string zone { get; set; }

    }

    public class Prop_CLassification
    {
        public List<string> HP { get; set; } = new List<string>();
        public List<string> NR { get; set; } = new List<string>();
        public List<string> NR_plus { get; set; } = new List<string>();
        public List<string> NR_minus { get; set; } = new List<string>();
        public List<string> LP { get; set; } = new List<string>();
    }

    public class SID
    {
        public List<string> G1 { get; set; } = new List<string>();
        public List<string> G2 { get; set; } = new List<string>();
        public List<string> G3 { get; set; } = new List<string>();

    }



    public class Function
    {
        // Diccionario de prefijos de callsigns a aerolíneas
        Dictionary<string, string> airlineDict = new Dictionary<string, string>
        {
            { "VLG", "Vueling" },
            { "TRA", "Transavia" },
            { "AAL", "American Airlines" },
            { "ICC", "Air Inuit" },
            { "AFR", "Air France" },
            { "SWR", "Swiss International Air Lines" },
            { "AUA", "Austrian Airlines" },
            { "IBE", "Iberia" },
            { "RYR", "Ryanair" },
            { "BAW", "British Airways" },
            { "ASL", "ASL Airlines" },
            { "FIN", "Finnair" },
            { "RUK", "RusJet" },
            { "WMT", "Wizz Air Malta" },
            { "EZY", "easyJet" },
            { "EJU", "easyJet Europe" },
            { "ELY", "El Al Israel Airlines" },
            { "WZZ", "Wizz Air" },
            { "UAE", "Emirates" },
            { "ETD", "Etihad Airways" },
            { "MAC", "Air Macau" },
            { "AHO", "Air Hamburg" },
            { "KLM", "KLM Royal Dutch Airlines" },
            { "ACA", "Air Canada" },
            { "CSD", "Czech Airlines" },
            { "EIN", "Aer Lingus" },
            { "AEE", "Aegean Airlines" },
            { "DAL", "Delta Air Lines" },
            { "TAP", "TAP Air Portugal" },
            { "GES", "Gestair" },
            { "UAL", "United Airlines" },
            { "THY", "Turkish Airlines" },
            { "NOZ", "Norwegian Air Shuttle" },
            { "NSZ", "Network Aviation" },
            { "AHY", "Azerbaijan Airlines" },
            { "DLH", "Lufthansa" },
            { "T7M", "San Marino Private Aircraft" },
            { "N", "USA Private Aircraft" },
            { "ZS", "South-Africa Private Aircraft" },
            {"VOE", "Volotea" },
            {"EWG", "Eurowings" }
        };

        public void SetAirline(ASTmessage message)
        {
            string AirlineCode = message.TI.Substring(0, 3); //Get only code
            string airlineName;
            if (airlineDict.TryGetValue(AirlineCode, out airlineName))
            {
                message.Airline = airlineName;

            }
            else
            {
                message.Airline = "Unknown";
            }
        }

        public void Getstereographic(ASTmessage message)
        {
            // Get WGS84 coordinates from asterix
            CoordinatesWGS84 Coordinates = new CoordinatesWGS84();//geodesic
            //CoordinatesWGS84(message.lat, message.lon, message.altitude);
            Coordinates.Lat = Convert.ToDouble(message.lat);
            Coordinates.Lon = Convert.ToDouble(message.lon);
            Coordinates.Height = Convert.ToDouble(message.altitude);

            Coordinates.Lat = Coordinates.Lat * Math.PI / 180;
            Coordinates.Lon = Coordinates.Lon * Math.PI / 180;

            //Set projection 
            GeoUtils geo = new GeoUtils();
            CoordinatesWGS84 myCenterProjection = new CoordinatesWGS84();


            myCenterProjection.Lat = (41 + (6.0 / 60.0) + (56.560 / 3600.0));
            myCenterProjection.Lat = myCenterProjection.Lat * Math.PI / 180.0;
            myCenterProjection.Lon = (001 + (41.0 / 60.0) + (33.010 / 3600.0));
            myCenterProjection.Lon = myCenterProjection.Lon * Math.PI / 180.0;
            myCenterProjection.Height = 0;


            CoordinatesWGS84 myCenter = new CoordinatesWGS84();
            myCenter = geo.setCenterProjection(myCenterProjection);

            // double eccentricity = Math.Sqrt(0.00669437999013);
            // double semiMajorAxis = 6378137.0;

            // Inicializa GeoUtils con tus valores deseados
            // geo = new GeoUtils(eccentricity, semiMajorAxis, myCenterProjection);

            //Convert to stereographic coordinates

            CoordinatesXYZ Coordinates_geocentric = new CoordinatesXYZ();
            CoordinatesXYZ Coordinates_cart = new CoordinatesXYZ();
            CoordinatesUVH Coordinates_ster = new CoordinatesUVH();


            Coordinates_geocentric = geo.change_geodesic2geocentric(Coordinates);
            Coordinates_cart = geo.change_geocentric2system_cartesian(Coordinates_geocentric);
            Coordinates_ster = geo.change_system_cartesian2stereographic(Coordinates_cart);

            message.x = Convert.ToString(Coordinates_ster.U);
            message.y = Convert.ToString(Coordinates_ster.V);
        }


        public double calculate_distance(ASTmessage aircraft_ahead, ASTmessage aircraft_behind)
        {
            double distance;
            double dU = Convert.ToDouble(aircraft_ahead.x) - Convert.ToDouble(aircraft_behind.x);
            double dV = Convert.ToDouble(aircraft_ahead.y) - Convert.ToDouble(aircraft_behind.y);
            distance = Math.Sqrt(dU * dU + dV * dV);
            distance = (distance / 1852.0);
            return distance;
        }

        public double wake_separation(ASTmessage aircraft_ahead, ASTmessage aircraft_behind)
        {
            string cat1 = aircraft_ahead.Estela;
            string cat2 = aircraft_behind.Estela;
            double wake_distance = 0;
            if (cat1 == "Super Pesada")
            {
                if (cat2 == "Pesada")
                {
                    wake_distance = 6; //6NM
                }
                if (cat2 == "Media")
                {
                    wake_distance = 7; //7nm
                }
                if (cat2 == "Ligera")
                {
                    wake_distance = 8; //8 NM
                }
            }
            else if (cat1 == "Pesada")
            {
                if (cat2 == "Pesada")
                {
                    wake_distance = 4; // 6NM
                }
                if (cat2 == "Media")
                {
                    wake_distance = 5; //7 NM
                }
                if (cat2 == "Ligera")
                {
                    wake_distance = 6; //8 NM
                }
            }

            else if (cat1 == "Media")
            {
                if (cat2 == "Ligera")
                {
                    wake_distance = 5; //5 NM
                }
            }


            return wake_distance;
        }

        public double LoA_separation(ASTmessage aircraft_ahead, ASTmessage aircraft_behind)
        {
            double LoA_distance = 0;
            //HP
            if (aircraft_ahead.Propulsion == "HP")
            {
                if (aircraft_behind.Propulsion == "HP" || aircraft_behind.Propulsion == "R" || aircraft_behind.Propulsion == "LP")
                {
                    if (aircraft_ahead.SID == aircraft_behind.SID)
                    {
                        LoA_distance = 5;
                    }
                    else
                    {
                        LoA_distance = 3;
                    }

                }
                else if (aircraft_behind.Propulsion == "NR+" || aircraft_behind.Propulsion == "NR-" || aircraft_behind.Propulsion == "NR")
                {
                    LoA_distance = 3;
                }
            }
            //R
            if (aircraft_ahead.Propulsion == "R")
            {
                if (aircraft_behind.Propulsion == "HP")
                {
                    if (aircraft_ahead.SID == aircraft_behind.SID)
                    {
                        LoA_distance = 7;
                    }
                    else
                    {
                        LoA_distance = 5;
                    }
                }
                else if (aircraft_behind.Propulsion == "R" || aircraft_behind.Propulsion == "LP")
                {
                    if (aircraft_ahead.SID == aircraft_behind.SID)
                    {
                        LoA_distance = 5;
                    }
                    else
                    {
                        LoA_distance = 3;
                    }
                }
                else if (aircraft_behind.Propulsion == "NR+" || aircraft_behind.Propulsion == "NR-" || aircraft_behind.Propulsion == "NR")
                {
                    LoA_distance = 3;
                }
            }
            //LP
            if (aircraft_ahead.Propulsion == "LP")
            {
                if (aircraft_behind.Propulsion == "HP")
                {
                    if (aircraft_ahead.SID == aircraft_behind.SID)
                    {
                        LoA_distance = 8;
                    }
                    else
                    {
                        LoA_distance = 6;
                    }
                }
                else if (aircraft_behind.Propulsion == "R")
                {
                    if (aircraft_ahead.SID == aircraft_behind.SID)
                    {
                        LoA_distance = 6;
                    }
                    else
                    {
                        LoA_distance = 4;
                    }
                }
                else if (aircraft_behind.Propulsion == "LP")
                {
                    if (aircraft_ahead.SID == aircraft_behind.SID)
                    {
                        LoA_distance = 5;
                    }
                    else
                    {
                        LoA_distance = 3;
                    }
                }
                else if (aircraft_behind.Propulsion == "NR+" || aircraft_behind.Propulsion == "NR-" || aircraft_behind.Propulsion == "NR")
                {
                    LoA_distance = 3;
                }
            }
            //NR+
            if (aircraft_ahead.Propulsion == "NR+")
            {
                if (aircraft_behind.Propulsion == "HP")
                {
                    if (aircraft_ahead.SID == aircraft_behind.SID)
                    {
                        LoA_distance = 1;
                    }
                    else
                    {
                        LoA_distance = 8;
                    }
                }
                else if (aircraft_behind.Propulsion == "R" || aircraft_behind.Propulsion == "LP")
                {
                    if (aircraft_ahead.SID == aircraft_behind.SID)
                    {
                        LoA_distance = 9;
                    }
                    else
                    {
                        LoA_distance = 6;
                    }
                }
                else if (aircraft_behind.Propulsion == "NR+")
                {
                    if (aircraft_ahead.SID == aircraft_behind.SID)
                    {
                        LoA_distance = 5;
                    }
                    else
                    {
                        LoA_distance = 3;
                    }
                }
                else if (aircraft_behind.Propulsion == "NR-" || aircraft_behind.Propulsion == "NR")
                {
                    LoA_distance = 3;
                }
            }
            //NR-
            if (aircraft_ahead.Propulsion == "NR-")
            {
                if (aircraft_behind.Propulsion == "HP" || aircraft_behind.Propulsion == "R" || aircraft_behind.Propulsion == "LP")
                {
                    LoA_distance = 9;

                }
                else if (aircraft_behind.Propulsion == "NR+")
                {
                    if (aircraft_ahead.SID == aircraft_behind.SID)
                    {
                        LoA_distance = 9;
                    }
                    else
                    {
                        LoA_distance = 6;
                    }
                }
                else if (aircraft_behind.Propulsion == "NR-")
                {
                    if (aircraft_ahead.SID == aircraft_behind.SID)
                    {
                        LoA_distance = 5;
                    }
                    else
                    {
                        LoA_distance = 3;
                    }
                }
                else if (aircraft_behind.Propulsion == "NR+")
                {
                    LoA_distance = 3;
                }
            }
            // NR
            if (aircraft_ahead.Propulsion == "NR")
            {
                if (aircraft_behind.Propulsion == "HP" || aircraft_behind.Propulsion == "R" || aircraft_behind.Propulsion == "LP" || aircraft_behind.Propulsion == "NR+" || aircraft_behind.Propulsion == "NR-")
                {
                    LoA_distance = 9;
                }
                else if (aircraft_behind.Propulsion == "NR")
                {
                    if (aircraft_ahead.SID == aircraft_behind.SID)
                    {
                        LoA_distance = 5;
                    }
                    else
                    {
                        LoA_distance = 3;
                    }
                }
            }
            return LoA_distance;
        }

        public void setPropulsion(ASTmessage message, Prop_CLassification propulsion)
        {
            string type = message.TipoAeronave;

            if (propulsion.HP.Contains(type))
            {
                message.Propulsion = "HP";
            }
            else if (propulsion.NR.Contains(type))
            {
                message.Propulsion = "NR";
            }
            else if (propulsion.NR_plus.Contains(type))
            {
                message.Propulsion = "NR+";
            }
            else if (propulsion.NR_minus.Contains(type))
            {
                message.Propulsion = "NR-";
            }
            else if (propulsion.LP.Contains(type))
            {
                message.Propulsion = "LP";
            }
            else
            {
                message.Propulsion = "R";
            }

        }


        public void setSID(ASTmessage message, SID SID_06, SID SID_24)
        {
            char[] sid_char = message.ProcDesp.ToArray();
            sid_char.SetValue('-', 5);
            string sid = new string(sid_char);
            if (message.PistaDespegue == "LEBL-06R")
            {
                if (SID_06.G1.Contains(sid) == true)
                {
                    message.SID = "SID_06_1";
                }
                else if (SID_06.G2.Contains(sid) == true)
                {
                    message.SID = "SID_06_2";
                }
                else if (SID_06.G3.Contains(sid) == true)
                {
                    message.SID = "SID_06_3";
                }
                else
                {
                    Console.WriteLine("SID not found.");
                }
            }

            if (message.PistaDespegue == "LEBL-24L")
            {
                if (SID_24.G1.Contains(sid) == true)
                {
                    message.SID = "SID_24_1";
                }
                else if (SID_24.G2.Contains(sid) == true)
                {
                    message.SID = "SID_24_2";
                }
                else if (SID_24.G3.Contains(sid) == true)
                {
                    message.SID = "SID_24_3";
                }
                else
                {
                    Console.WriteLine("SID not found.");
                }

            }

        }

        public bool IsVirajeRecorded(DataTable tabla, string TI)
        {
            foreach (DataRow row in tabla.Rows)
            {
                if (row["Aircraft ID"].ToString() == TI)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ExisteIncumplimiento(DataTable table, string TI_ahead, string TI_behind, string motivo, string zona)
        {
            foreach (DataRow row in table.Rows)
            {
                // Verificar si ya existe una fila con los mismos aviones, motivo y zona
                if (row["Callsign aircraft ahead"].ToString() == TI_ahead
                    && row["Callsign aircraft behind"].ToString() == TI_behind
                    && row["Type of loss"].ToString() == motivo
                    && row["Zone"].ToString() == zona) // Compara también la zona (TMA o TWR)
                {
                    return true; // Ya existe una entrada con los mismos aviones, motivo y zona
                }
            }
            return false;
        }

        public double calculate_radial(ASTmessage aircraft, double lat_dvor, double lon_dvor)
        {
            double lat_dvor_rad = lat_dvor * (Math.PI / 180.0);
            double lon_dvor_rad = lon_dvor * (Math.PI / 180.0);
            double aircraft_lat = Convert.ToDouble(aircraft.lat) * (Math.PI / 180.0);
            double aircraft_lon = Convert.ToDouble(aircraft.lon) * (Math.PI / 180.0);

            double delta_lon = aircraft_lon - lon_dvor_rad;
            double x = Math.Sin(delta_lon) * Math.Cos(aircraft_lat);
            double y = Math.Cos(lat_dvor_rad) * Math.Sin(aircraft_lat) - Math.Sin(lat_dvor_rad) * Math.Cos(aircraft_lat) * Math.Cos(delta_lon);

            double initialBearing = Math.Atan2(x, y);

            double initialBearingDegrees = (initialBearing) * (180.0 / Math.PI);
            double radial = (initialBearingDegrees + 360) % 360;
            return radial;

        }

        public void AddViolationIfNotExists(DataTable table, ASTmessage aircraftAhead, ASTmessage aircraftBehind, double distance, string typeOfLoss, string zone)
        {
            //string zone = firstDetection ? "TWR" : "TMA"; // Determine la zona
            string TI_ahead = aircraftAhead.TI;
            string TI_behind = aircraftBehind.TI;

            // Verifica si ya existe un incumplimiento antes de agregarlo
            if (!ExisteIncumplimiento(table, TI_ahead, TI_behind, typeOfLoss, zone))
            {
                table.Rows.Add(
                    TI_ahead,                     // Callsign of the aircraft ahead
                    TI_behind,                    // Callsign of the aircraft behind
                    distance.ToString("F2"),      // Formatted distance
                    typeOfLoss,                   // Type of loss
                    aircraftAhead.time,           // Time
                    zone,                        // Zone (TWR or TMA)
                    aircraftAhead.ProcDesp,
                    aircraftBehind.ProcDesp,
                    aircraftAhead.SID,
                    aircraftBehind.SID,
                    aircraftAhead.TipoAeronave,
                    aircraftBehind.TipoAeronave,
                    aircraftAhead.Airline,
                    aircraftBehind.Airline
                );
            }
        }

        public bool IsAircraftInTHR(double aircraftLat, double aircraftLon, List<(double lat, double lon)> polygon)
        {
            int n = polygon.Count;
            bool isInside = false;
            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                double xi = polygon[i].lat, yi = polygon[i].lon;
                double xj = polygon[j].lat, yj = polygon[j].lon;

                // Verifica si el punto está dentro del polígono utilizando el algoritmo ray-casting
                bool intersect = ((yi > aircraftLon) != (yj > aircraftLon)) &&
                                 (aircraftLat < (xj - xi) * (aircraftLon - yi) / (yj - yi) + xi);
                if (intersect)
                {
                    isInside = !isInside;
                }
            }
            return isInside;
        }


        public double compute_IAS(double alt_1, double alt_2, double alt_ft, double IAS1, double IAS2)
        {
            double IAS = IAS1 + ((IAS2 - IAS1) / (alt_2 - alt_1)) * (alt_ft - alt_1);
            IAS = Math.Round(IAS);
            return IAS;

        }

        public void GenerarKML(DataTable tablaAviones, string rutaArchivo)
        {
            if (tablaAviones == null || tablaAviones.Rows.Count == 0)
            {
                Console.WriteLine("La tabla de aviones está vacía.");
                return;
            }

            using (StreamWriter writer = new StreamWriter(rutaArchivo))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                writer.WriteLine("<kml xmlns=\"http://www.opengis.net/kml/2.2\">");
                writer.WriteLine("<Document>");

                foreach (DataRow row in tablaAviones.Rows)
                {
                    string indicativo = row["Aircraft ID"].ToString();
                    string latitudStr = row["Aircraft latitude [°]"].ToString();
                    string longitudStr = row["Aircraft longitude [°]"].ToString();
                    string horaSACTA = row["time [hh:mm:ss:fff]"].ToString();

                    if (string.IsNullOrEmpty(latitudStr) || string.IsNullOrEmpty(longitudStr))
                    {
                        Console.WriteLine($"Coordenadas no válidas para el vuelo {indicativo}. Omitiendo este registro.");
                        continue;
                    }

                    latitudStr = latitudStr.Replace(",", ".");
                    longitudStr = longitudStr.Replace(",", ".");

                    if (double.TryParse(latitudStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double latitud) &&
                        double.TryParse(longitudStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double longitud))
                    {
                        // Escribir el placemark con formato invariante
                        writer.WriteLine("<Placemark>");
                        writer.WriteLine($"  <name>{indicativo} - {horaSACTA}</name>");
                        writer.WriteLine("  <Point>");
                        writer.WriteLine($"    <coordinates>{longitud.ToString(CultureInfo.InvariantCulture)},{latitud.ToString(CultureInfo.InvariantCulture)},0</coordinates>");
                        writer.WriteLine("  </Point>");
                        writer.WriteLine("</Placemark>");
                    }
                    else
                    {
                        Console.WriteLine($"Error al convertir las coordenadas para el vuelo {indicativo}. Omitiendo este registro.");
                    }
                }

                writer.WriteLine("</Document>");
                writer.WriteLine("</kml>");
            }

            Console.WriteLine($"Archivo KML generado en: {rutaArchivo}");
        }



        public void SaveDataTableAsCSV(DataTable table, string filePath)
        {

            StringBuilder csvContent = new StringBuilder();

            // Headers
            string[] columnNames = new string[table.Columns.Count];
            for (int i = 0; i < table.Columns.Count; i++)
            {
                columnNames[i] = table.Columns[i].ColumnName;
            }
            csvContent.AppendLine(string.Join(",", columnNames));

            // Write rows
            foreach (DataRow row in table.Rows)
            {
                string[] rowData = new string[table.Columns.Count];
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    //Spceial characters and replace º por °
                    rowData[i] = "\"" + ReplaceSpecialCharacters(row[i]?.ToString()) + "\"";
                }
                csvContent.AppendLine(string.Join(",", rowData));
            }


            File.WriteAllText(filePath, csvContent.ToString(), Encoding.UTF8);
        }


        public string ReplaceSpecialCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            // Replace º for °
            return input.Replace("º", "°").Replace("\"", "\"\"");
        }

        

    }

}








