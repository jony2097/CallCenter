using AppTelemarketing.Entidades;
using AppTelemarketing.Servicios;
using System.Data;
using WinFormsApp2;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppTelemarketing
{
    public partial class FrmConsultarAgendados : Form
    {
        GestionDeLlamadosService gestionDeLlamadosService_ = new GestionDeLlamadosService();
        ProspectoService prospectoService_ = new ProspectoService();
        public int userId = FrmLogin.idUser;
        public FrmConsultarAgendados()
        {
            InitializeComponent();
        }
        private void InicializeComponent()
        {
            // Inicializar y configurar el DateTimePicker
            dateTimePicker1 = new DateTimePicker
            {
                Name = "dateTimePicker1",
                Dock = DockStyle.Top,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd HH:mm:ss"
            };
            this.Controls.Add(dateTimePicker1);
        }


        private void FrmConsultarAgendados_Load(object sender, EventArgs e)
        {
            // Cargar data
            DataTable agendados = gestionDeLlamadosService_.ListarAgendados(userId);

            if (agendados.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron agendados para mostrar.");
                return;
            }

            // Bind al DGV
            ListadoAgendados.DataSource = agendados;

            // Verificar que la columna no exista para no duplicarla
            if (!ListadoAgendados.Columns.Contains("NombreCompleto"))
            {
                DataGridViewTextBoxColumn nombreCompleto = new DataGridViewTextBoxColumn();
                nombreCompleto.Name = "NombreCompleto";
                nombreCompleto.HeaderText = "NOMBRE Y APELLIDO";
                nombreCompleto.ReadOnly = true;
                ListadoAgendados.Columns.Insert(1, nombreCompleto);
            }

            ListadoAgendados.CellFormatting += ListadoAgendados_CellFormatting;
        }


        //Formato de celdas del DatagridViewer
        private void ListadoAgendados_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                // Solo para la columna NombreCompleto
                if (ListadoAgendados.Columns[e.ColumnIndex].Name != "NombreCompleto")
                    return;

                // Si la fila es nueva o no está ligada a datos, evitar error
                if (ListadoAgendados.Rows[e.RowIndex].IsNewRow)
                    return;

                var dataBoundItem = ListadoAgendados.Rows[e.RowIndex].DataBoundItem;

                if (dataBoundItem == null)
                    return;

                DataRow row = ((DataRowView)dataBoundItem).Row;

                string nombre = row["Nombre"]?.ToString() ?? "";
                string apellido = row["Apellido"]?.ToString() ?? "";

                e.Value = $"{nombre} {apellido}";
            }
            catch
            {
                // Silenciar errores de formato
            }
        }





        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnVolverLlamado_Click(object sender, EventArgs e)
        {
            Prospecto prospectoSeleccionado;
            if (ListadoAgendados.SelectedCells.Count > 0)
            {
                int datoSeleccionado = ListadoAgendados.SelectedCells[0].RowIndex;
                DataGridViewRow filaSeleccionada = ListadoAgendados.Rows[datoSeleccionado];

                //solo con el id del prospecto podemos taer los datos a pantalla
                int idProspecto = Convert.ToInt32(filaSeleccionada.Cells["IdProspecto"].Value);
                

                // Instancia del formulario FrmRealizarLlamada
                // en el panel de la derecha 
                
                frmMenu menuForm = this.ParentForm as frmMenu;
                if (menuForm != null)
                {
                    menuForm.AbrirFormInPanel(new FrmRealizarLlamada(idProspecto));
                }
                else
                {
                    MessageBox.Show("No se puede abrir Formulario", "Información", MessageBoxButtons.OK);
                }
                
                // cuando el formulario se cierra eliminamos
                // el prospecto del DataGridView y la base de datos
                gestionDeLlamadosService_.EliminarAgendado(idProspecto);
                ListadoAgendados.Rows.RemoveAt(datoSeleccionado);
            }
            else
            {
                MessageBox.Show("Seleccione un prospecto para continuar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}

