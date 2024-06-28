using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
using System.Configuration;

namespace discos_app
{
    public partial class frmAltaDisco : Form
    {
        private Disco disco = null;
        private OpenFileDialog archivo = null;
        public frmAltaDisco()
        {
            InitializeComponent();
        }

        public frmAltaDisco(Disco disco)
        {
            InitializeComponent();
            this.disco = disco;
            Text = "Modificar Disco";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            DiscosNegocio negocio = new DiscosNegocio();
            try
            {
                if (disco == null)
                    disco = new Disco();

                disco.Titulo = txtTitulo.Text;
                disco.FechaLanzamiento = dtpLanzamiento.Value;
                disco.CantidadCanciones = int.Parse(txtCantCanciones.Text);
                disco.UrlImagen = txtUrlImagen.Text;
                disco.Genero = (Estilo)cboGenero.SelectedItem;
                disco.TipoEdicion = (Edicion)cboTipoEdicion.SelectedItem;

                if (disco.Id != 0)
                {
                negocio.modificar(disco);
                MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                negocio.agregar(disco);
                MessageBox.Show("Agregado exitosamente");
                }

                //Guardo imagen si la levantó localmente
                if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["img-folder"] + archivo.SafeFileName);
                    txtUrlImagen.Text = ConfigurationManager.AppSettings["img-folder"] + archivo.SafeFileName;
                }

                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaDisco_Load(object sender, EventArgs e)
        {
            EstiloNegocio estiloNegocio = new EstiloNegocio();
            EdicionNegocio edicionNegocio = new EdicionNegocio();
            try
            {
                cboGenero.DataSource = estiloNegocio.listar();
                cboGenero.ValueMember = "Id";
                cboGenero.DisplayMember = "Descripcion";
                cboTipoEdicion.DataSource = edicionNegocio.listar();
                cboTipoEdicion.ValueMember = "Id";
                cboTipoEdicion.DisplayMember = "Descripcion";

                if (disco != null)
                {
                    txtTitulo.Text = disco.Titulo;
                    dtpLanzamiento.Value = disco.FechaLanzamiento;
                    txtCantCanciones.Text = disco.CantidadCanciones.ToString();
                    txtUrlImagen.Text = disco.UrlImagen;
                    cargarImagen(disco.UrlImagen);
                    cboGenero.SelectedValue = disco.Genero.Id;
                    cboTipoEdicion.SelectedValue = disco.TipoEdicion.Id;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxDisco.Load(imagen);

            }
            catch (Exception)
            {

                pbxDisco.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSkHbLOf8uW56x79abBDHXJTqcfgNcx0wAe4XGfxn8-CA&s");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);

                //guardo la imagen
                //File.Copy(archivo.FileName, ConfigurationManager.AppSettings["img-folder"] + archivo.SafeFileName);
            }
        }
    }
}
