using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;



namespace CapaPresentacion
{
    public partial class InicioAgenda : Form
    {
        public InicioAgenda()
        {
            InitializeComponent();
        }


        private void InicioAgenda_Load(object sender, EventArgs e)
        {
            
            cboEstado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "No Activo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;


            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible == true && columna.Name != "btnSeleccionar")
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 0;


            List<Agenda> listaPersona = new CN_Agenda().Listar();

            foreach (Agenda item in listaPersona)
            {
                dgvData.Rows.Add(new object[]
                {
                            "",
                            item.Id,
                            item.Nombre,
                            item.Apellido,
                            item.Telefono,
                            item.Correo,
                            item.Pais,
                            item.Ciudad,
                            item.Direccion,
                            item.Estado == true ? 1: 0,
                            item.Estado == true ? "Activo" : "No Activo"
                });
            }

        }
            
        private void Limpiar()
        {
            txtIndice.Text = "-1";
            txtId.Text = "0";
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            txtPais.Clear();
            txtCiudad.Clear();
            txtDireccion.Clear();
            cboEstado.SelectedIndex = 0;

            txtNombre.Focus();
        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {

                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.comprobar.Width;
                var h = Properties.Resources.comprobar.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.comprobar, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


            if (dgvData.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {

                    txtIndice.Text = indice.ToString();
                    txtId.Text = dgvData.Rows[indice].Cells["id"].Value.ToString();
                    txtNombre.Text = dgvData.Rows[indice].Cells["Nombre"].Value.ToString();
                    txtApellido.Text = dgvData.Rows[indice].Cells["Apellido"].Value.ToString();
                    txtTelefono.Text = dgvData.Rows[indice].Cells["Telefono"].Value.ToString();
                    txtCorreo.Text = dgvData.Rows[indice].Cells["Correo"].Value.ToString();
                    txtPais.Text = dgvData.Rows[indice].Cells["Pais"].Value.ToString();
                    txtCiudad.Text = dgvData.Rows[indice].Cells["Ciudad"].Value.ToString();
                    txtDireccion.Text = dgvData.Rows[indice].Cells["Direccion"].Value.ToString();

                    foreach (OpcionCombo oc in cboEstado.Items)
                    {

                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["EstadoValor"].Value))
                        {
                            int indice_combo = cboEstado.Items.IndexOf(oc);
                            cboEstado.SelectedIndex = indice_combo;
                            break;
                        }
                    }
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            {
                string mensaje = string.Empty;
              

                Agenda objcd_contactos = new Agenda()
                {
                    //Id = int.TryParse(txtId.Text, out int id) ? id : 0,
                    Id = Convert.ToInt32(txtId.Text),
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Telefono = txtTelefono.Text,
                    Correo = txtCorreo.Text,
                    Pais = txtPais.Text,
                    Ciudad = txtCiudad.Text,
                    Direccion = txtDireccion.Text,
                    Estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).Valor) == 1 ? true : false
                };

                if (objcd_contactos.Id == 0)
                {

                    int idcontactogenerado = new CN_Agenda().Registrar(objcd_contactos, out mensaje);

                    if (idcontactogenerado != 0)
                    {

                        dgvData.Rows.Add(new object[] {
                            "",
                            idcontactogenerado,
                            txtNombre.Text,
                            txtApellido.Text,
                            txtTelefono.Text,
                            txtCorreo.Text,
                            txtPais.Text,
                            txtCiudad.Text,
                            txtDireccion.Text,
                          ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString(),
                          ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString()
                        });

                        Limpiar();
                    }
                    else
                    {

                        MessageBox.Show(mensaje);
                    }
                }
                else
                {

                    bool resultado = new CN_Agenda().Editar(objcd_contactos, out mensaje);

                    if (resultado)
                    {

                        DataGridViewRow row = dgvData.Rows[Convert.ToInt32(txtIndice.Text)];
                        row.Cells["Id"].Value = txtId.Text;
                        row.Cells["Nombre"].Value = txtNombre.Text;
                        row.Cells["Apellido"].Value = txtApellido.Text;
                        row.Cells["Telefono"].Value = txtTelefono.Text;
                        row.Cells["Correo"].Value = txtCorreo.Text;
                        row.Cells["Pais"].Value = txtPais.Text;
                        row.Cells["Ciudad"].Value = txtCiudad.Text;
                        row.Cells["Direccion"].Value = txtDireccion.Text;
                        row.Cells["EstadoValor"].Value = ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString();
                        row.Cells["Estado"].Value = ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString();

                        Limpiar();
                    }
                    else
                    {

                        MessageBox.Show(mensaje);
                    }
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtId.Text) != 0)
            {
                if (MessageBox.Show("¿Desea ELIMINAR UN CONTACTO?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    string mensaje = string.Empty;
                    Agenda objcontacto = new Agenda()
                    {
                        Id = Convert.ToInt32(txtId.Text),

                    };

                    bool respuesta = new CN_Agenda().Eliminar(objcontacto, out mensaje);

                    if (respuesta)
                    {
                        dgvData.Rows.RemoveAt(Convert.ToInt32(txtIndice.Text));
                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();

            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {

                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))

                        row.Visible = true;

                    else

                        row.Visible = false;
                }
            }
        }
        private void btnLimpiarBuscador_Click(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
            foreach (DataGridViewRow row in dgvData.Rows)
            {

                row.Visible = true;
            }
        }
    }
}