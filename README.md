# PROJECT 3 PGTA: ATM
This document is a quick guide on how to use the project software. The purpose of the project is to obtain 5 differrent *.csv files with data regarding departures from LEBL: turn initiation, separation loss between consecutive aircrafts, altitude and IAS at threshold, IAS at different altitudes and minimum distance to sound level meter.

<em>Group members: Alejandro Curiel Molina, Marina Martín Ferrer, Jose Carlos Martínez Conde, Joel Moreno de Toro, Èrica Parra Moya, Paula Valle Bové, Mireia Viladot Saló</em>.

## 1. Installation
### 1.1 Prerequisites
- [.NET Framework 4.8 SDK](https://dotnet.microsoft.com/es-es/download/dotnet/6.0) or higher.
- Clone this repository and open it in a C# compatible development environment (e.g., Visual Studio or Visual Studio Code).
### 1.2 Cloning the repository

```bash
git clone https://github.com/Joowww/Proyecto3__Def_PGTA.git
```
### 1.3 Dependencies
The main dependencies of this project are:
- **Accord** (Version 3.8.0): Library for image processing, machine learning, statistical analysis, etc.
- **Accord.Math** (Version 3.8.0): Extension of Accord.NET for mathematical operations.
- **App.Metrics.Abstractions** (Version 4.3.0): App Metrics Core abstractions and interfaces for metric types, reporting, filtering and more.
- **CsvTextFieldParser** (Version 1.2.2): A simple CSV parser based on Microsoft.VisualBasic.FileIO.TextFieldParser.
- **DocumentFormat.OpenXml** (Version 3.2.0): The Open XML SDK provides tools for working with Office Word, Excel, and PowerPoint documents.
- **DocumentFormat.OpenXml.Framework** (Version 3.2.0): The Open XML SDK provides tools for working with Office Word, Excel, and PowerPoint documents.
- **EPPlus** (Version 7.5.1): A spreadsheet library for .NET framework and .NET core
- **NLog** (Version 5.3.4): NLog is a logging platform for .NET with rich log routing and management capabilities.
- **Nspring** (Version 1.4)
- **OfficeOpenXml.Core.ExcelPackage** (Version 1.0.0)

## 2. User Manual
1. When the program starts, a welcome Windows Forms will appear with a short description of what the App does. Click "Start".
2. A new window will appear, requesting what inputs files to use. The program needs 5 inputs to execute which are: LEBL departures, Asterix decoded CSV file, Aircraft Classification Table, Same SID Table for runway 06R and Same SID Table for runway 24L.
There are 2 options do run the program:
  - Use the standard ones already present in the project. Data from periods of 4 hours (from 8 am to 12 am) or 24 hours of the 2nd of May 2023 will be analyzed.
  - User select specific inputs. If this case is selected, a new file explorer will open. The user shall do <em> Cntr + select the 5 inputs file </em>  **in the same order as specified**: LEBL departures, Asterix decoded  <em>*.csv</em>  file (must be from AsterixPro!), Aircraft Classification Table, Same SID Table for runway 06R and Same SID Table for runway 24L.
3. The program will execute until the final <em>*.csv</em> files are obtained.
4. A file explorer will open, the user shall select where to save the outputs files. The names for the files will be: "Results_SeparationLoss.csv",  "Results_TurnInitiation.csv", "Results_TurnInitiation.kml", "Results_IASatAltitudes.csv", "Results_IASandAltitudeTHR.csv" and "Results_MinDistanceSoundlevelmeter.csv".
5. A messagebox will indicate if the process has been succesful.

PD: Make sure to close excels previously obtained by this program in the same folder selected. Otherwhise a message box will appear with this same message.  