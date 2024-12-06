﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Project_P3
{
    public partial class Input : Form
    {
        public Input()
        {
            InitializeComponent();
        }


        // 24 h
        private async void button1_Click(object sender, EventArgs e)
        {
            string defaultFile1 = "2305_02_dep_lebl.xlsx"; // Archivo 1
            string defaultFile2 = "archivo_combinado_sin_dup.csv"; // Archivo 2
            string defaultFile3 = "Tabla_Clasificacion_aeronaves.xlsx"; // Archivo 3
            string defaultFile4 = "Tabla_misma_SID_06R.xlsx"; // Archivo 4
            string defaultFile5 = "Tabla_misma_SID_24L.xlsx"; // Archivo 5

            string path = null;
            progBar progBar24 = new progBar();

            // Llamar a ExecuteCode con los archivos predeterminados
            try
            {
                // Mostrar el formulario de progreso
                progBar24.Show();


                // Iniciar la barra de progreso de forma asíncrona
                var progressTask = progBar24.StartProgressAsync();

                List<DataTable> tablas = await Task.Run(() => Class1.ExecuteCode(defaultFile1, defaultFile2, defaultFile3, defaultFile4, defaultFile5));

                // Detener la barra de progreso
                progBar24.StopProgress();
                path = Class2.SaveFiles(tablas);
                // Asegurar que la barra llegue al 100% antes de cerrar
                progBar24.UpdateProgress(100);
                await Task.Delay(500); // Pausa breve para mostrar el progreso completo

                // Mostrar mensajes al usuario según el resultado
                if (path == null)
                {
                    MessageBox.Show("There has been an error with the folder selected. Make sure to close excels previously obtained by this code.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    progBar24.Close();
                }
            
                else
                {
                    MessageBox.Show("Process finished correctly. Files saved in " + path, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CloseAllForms();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                progBar24.Close();
            }
        }


        private async void button2_Click(object sender, EventArgs e)
        {            
            // Crear un OpenFileDialog para permitir la selección de múltiples archivos
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel and CSV Files (*.xlsx;*.csv)|*.xlsx;*.csv|Todos los archivos (*.*)|*.*"; // Filtro para .xlsx y .csv
            openFileDialog.Title = "Select input files";
            openFileDialog.Multiselect = true; // Permitir la selección de múltiples archivos

            // Si el usuario selecciona 4 archivos
            if (openFileDialog.ShowDialog() == DialogResult.OK && openFileDialog.FileNames.Length == 5)
            {
                // Obtener las rutas de los archivos seleccionados
                string[] selectedFiles = openFileDialog.FileNames; // Array con las rutas de los archivos seleccionados

                string path = null;
                progBar progBar = new progBar();
                // Llamar a ExecuteCode con los 4 archivos seleccionados
                try
                {
                    // Mostrar el formulario de progreso
                    progBar.Show();

                    // Iniciar la barra de progreso de forma asíncrona
                    var progressTask = progBar.StartProgressAsync();

                    // Ejecutar el código principal en segundo plano
                    List<DataTable> tablas = null;
                    try
                    {
                        tablas = await Task.Run(() => Class1.ExecuteCode(selectedFiles[0], selectedFiles[1], selectedFiles[2], selectedFiles[3], selectedFiles[4]));
                    }
                    catch
                    {
                        MessageBox.Show("Wrong format. Make sure that input files have correct structure.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    if (tablas != null)
                    {
                        // Detener la barra de progreso
                        progBar.StopProgress();

                        path = Class2.SaveFiles(tablas);
                        // Asegurar que la barra llegue al 100% antes de cerrar
                        progBar.UpdateProgress(100);
                        await Task.Delay(500); // Pausa breve para mostrar el progreso completo

                        // Mostrar mensajes al usuario según el resultado
                        if (path == null)
                        {
                            MessageBox.Show("There has been an error with the folder selected. Make sure to close excels previously obtained by this code.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            progBar.Close();
                        }

                        else
                        {
                            MessageBox.Show("Process finished correctly. Files saved in " + path, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CloseAllForms();
                        }
                    }
                    else
                    {
                        throw new Exception("Format error. Make sure the input files have the correct format.");
                        progBar.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    progBar.Close();
                }

            }
            else
            {
                MessageBox.Show("Please, select exactly 5 files");
            }

        }

        private void CloseAllForms()
        {
            // Crear una lista de formularios a cerrar (evitar modificar la colección mientras la recorres)
            var formsToClose = new List<Form>();

            foreach (Form form in Application.OpenForms)
            {
                formsToClose.Add(form);
            }

            // Cerrar los formularios después de la iteración
            foreach (Form form in formsToClose)
            {
                form.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        // 4h
        private async void button3_Click(object sender, EventArgs e)
        {
            string defaultFile1 = "2305_02_dep_lebl.xlsx"; // Archivo 1
            string defaultFile2 = "FilteredASTERIX_0812_230205.csv"; // Archivo 2
            string defaultFile3 = "Tabla_Clasificacion_aeronaves.xlsx"; // Archivo 3
            string defaultFile4 = "Tabla_misma_SID_06R.xlsx"; // Archivo 4
            string defaultFile5 = "Tabla_misma_SID_24L.xlsx"; // Archivo 5

            string path = null;

            // Crear una instancia del formulario de progreso
            progBar progBar4 = new progBar();

            try
            {
                // Mostrar el formulario de progreso
                progBar4.Show();

                var progressTask = progBar4.StartProgressAsync();

                // Ejecutar el código principal en segundo plano
                List<DataTable> tablas = await Task.Run(() => Class1.ExecuteCode(defaultFile1, defaultFile2, defaultFile3, defaultFile4, defaultFile5));

                
                    // Detener la barra de progreso
                    progBar4.StopProgress();

                    path = Class2.SaveFiles(tablas);
                    // Asegurar que la barra llegue al 100% antes de cerrar
                    progBar4.UpdateProgress(100);
                    await Task.Delay(500); // Pausa breve para mostrar el progreso completo

                    // Mostrar mensajes al usuario según el resultado
                    if (path == null)
                    {
                        MessageBox.Show("There has been an error with the folder selected. Make sure to close excels previously obtained by this code.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        progBar4.Close();
                    }
                    else
                    {
                        MessageBox.Show("Process finished. File created at: " + path, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        progBar4.Close();
                        CloseAllForms();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                progBar4.Close();
            }
        }

        private void Input_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(
            "Are you sure you want to close the application?",
            "Confirm Exit",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Environment.Exit(0);
            }
            else if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
