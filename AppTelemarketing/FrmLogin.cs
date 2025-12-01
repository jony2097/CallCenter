using AppTelemarketing.Entidades;
using AppTelemarketing.Servicios;
using WinFormsApp2;

namespace AppTelemarketing
{
    public partial class FrmLogin : Form
    {
        public SecurityService SecurityService_ = new SecurityService();
        public UsuarioService UsuarioService_ = new UsuarioService();

        public FrmLogin()
        {
            InitializeComponent();
            txtClave.PasswordChar = '*';
        }

        public static string usuariologueado { get; set; }
        public static int idUser { get; set; }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            // Cargar roles UNA SOLA VEZ
            var listaRoles = UsuarioService_.ObtenerTipoUsuarios();

            comboBoxRol.DisplayMember = "Descripcion";  // Lo que se muestra
            comboBoxRol.ValueMember = "IdTipoUsuario";  // El ID (1 o 2)
            comboBoxRol.DataSource = listaRoles;        // List<TipoUsuarios>
        }

        private void linkRegistrese_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            FrmRegistrarse frm = new FrmRegistrarse();
            frm.Show();
        }

        public void btnIniciar_Click(object sender, EventArgs e)
        {
     
            if (comboBoxRol.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un rol.");
                return;
            }

            int idTipoUsuario = Convert.ToInt32(comboBoxRol.SelectedValue);

            bool valido = SecurityService_.UsuarioValido(
                txtUsuario.Text,
                txtClave.Text,
                idTipoUsuario
            );

            if (valido)
            {
                usuariologueado = txtUsuario.Text;
                idUser = UsuarioService_.TraerUserId(usuariologueado);
                RedirigirSegunRol();
            }
            else
            {
                MessageBox.Show("Usuario incorrecto o su cuenta aún no ha sido aprobada por el administrador");
            }
        }

        private void RedirigirSegunRol()
        {
            TipoUsuarios rol = comboBoxRol.SelectedItem as TipoUsuarios;

            if (rol == null) return;

            if (rol.IdTipoUsuario == 1)   // Administrador
            {
                this.Hide();
                frmMenuAdmi frm = new frmMenuAdmi();
                frm.Show();
            }
            else if (rol.IdTipoUsuario == 2)  // Telemarketer
            {
                this.Hide();
                FrmEmpresas frm = new FrmEmpresas();
                frm.Show();
            }

        }


        private void pictureOcerrado_Click(object sender, EventArgs e)
        {
            pictureOabierto.BringToFront();
            txtClave.PasswordChar = '\0';
        }

        private void pictureOabierto_Click(object sender, EventArgs e)
        {
            pictureOcerrado.BringToFront();
            txtClave.PasswordChar = '*';
        }
    }
}
