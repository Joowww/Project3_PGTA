using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_P3
{
    public partial class Input : Form
    {
        public Input()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           

            string defaultFile1 = "2305_02_dep_lebl.xlsx"; // Archivo 1
            string defaultFile2 = "archivo_combinado_sin_dup.csv"; // Archivo 2
            string defaultFile3 = "Tabla_Clasificacion_aeronaves.xlsx"; // Archivo 3
            string defaultFile4 = "Tabla_misma_SID_06R.xlsx"; // Archivo 4
            string defaultFile5 = "Tabla_misma_SID_24L.xlsx"; // Archivo 5

            // Llamar a ExecuteCode con los archivos predeterminados
            Class1.ExecuteCode(defaultFile1, defaultFile2, defaultFile3, defaultFile4, defaultFile5);

            MessageBox.Show("Process finished correctly", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            CloseAllForms();
        }

        private void button2_Click(object sender, EventArgs e)
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

                // Llamar a ExecuteCode con los 4 archivos seleccionados
                Class1.ExecuteCode(selectedFiles[0], selectedFiles[1], selectedFiles[2], selectedFiles[3], selectedFiles[4]);
            }
            else
            {
                MessageBox.Show("Please, select exactly 5 files");
                
            }

            
            MessageBox.Show("Process finished correctly", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);


            CloseAllForms();
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

        private void button3_Click(object sender, EventArgs e)
        {
            

            string defaultFile1 = "2305_02_dep_lebl.xlsx"; // Archivo 1
            string defaultFile2 = "FilteredASTERIX_0812_230205.csv"; // Archivo 2
            string defaultFile3 = "Tabla_Clasificacion_aeronaves.xlsx"; // Archivo 3
            string defaultFile4 = "Tabla_misma_SID_06R.xlsx"; // Archivo 4
            string defaultFile5 = "Tabla_misma_SID_24L.xlsx"; // Archivo 5

            // Llamar a ExecuteCode con los archivos predeterminados
            Class1.ExecuteCode(defaultFile1, defaultFile2, defaultFile3, defaultFile4, defaultFile5);

            MessageBox.Show("Process finished correctly", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            CloseAllForms();
        }
    }
}
