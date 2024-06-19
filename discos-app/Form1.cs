using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace discos_app
{
    public partial class Form1 : Form
    {
        private List<Disco> listaDisco;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DiscosNegocio negocio = new DiscosNegocio();
            listaDisco = negocio.listar();
            dgvDiscos.DataSource = listaDisco;
            dgvDiscos.Columns["UrlImagen"].Visible = false;
            cargarImagen(listaDisco[0].UrlImagen);
        }

        private void dgvDiscos_SelectionChanged(object sender, EventArgs e)
        {
            Disco seleccionado = (Disco)dgvDiscos.CurrentRow.DataBoundItem;
            cargarImagen(seleccionado.UrlImagen);
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxDiscos.Load(imagen);

            }
            catch (Exception)
            {

                pbxDiscos.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSkHbLOf8uW56x79abBDHXJTqcfgNcx0wAe4XGfxn8-CA&s");
            }
        }

    }
}
