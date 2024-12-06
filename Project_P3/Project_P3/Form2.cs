using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_P3
{
    public partial class progBar : Form
    {
        private bool keepRunning = true; // Controla si la barra debe seguir avanzando
        public progBar()
        {
            InitializeComponent();
        }

        public async Task StartProgressAsync()
        {
            while (keepRunning)
            {
                for (int i = 0; i <= 100; i += 5)
                {
                    if (!keepRunning) break; // Detener si se indica
                    UpdateProgress(i);
                    await Task.Delay(100); // Pausa de 100 ms
                }
            }
        }

        // Método para detener el progreso
        public void StopProgress()
        {
            keepRunning = false;
            progressBar1.Value = 90;
        }

        public void UpdateProgress(int value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => progressBar1.Value = value));
            }
            else
            {
                progressBar1.Value = value;
            }
        }

        private void progBar_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    
}
