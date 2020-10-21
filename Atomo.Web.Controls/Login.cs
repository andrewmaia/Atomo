using System;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace Atomo.Web.Controls
{
    public class Login : CompositeControl
    {
        public event EventHandler Logar;

        #region Properties
        public string UserName
        {
            get
            {
                TextBox txt = (TextBox)FindControl("txtUsuario");
                return txt.Text;
            }
            set
            {
                TextBox txt = (TextBox)FindControl("txtUsuario");
                txt.Text = value;
            }
        }

        public string Password
        {
            get
            {
                TextBox pwd = (TextBox)FindControl("txtSenha");
                return pwd.Text;
            }
            set
            {
                TextBox pwd = (TextBox)FindControl("txtSenha");
                pwd.Text = value;
            }
        }
        #endregion

        public override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer)
        {
            writer.RenderBeginTag("div");
        }

        protected override void CreateChildControls()
        {
            HtmlGenericControl fieldset = new HtmlGenericControl("fieldset");

            HtmlGenericControl legend = new HtmlGenericControl("legend");
            legend.InnerHtml = "Login";
            fieldset.Controls.Add(legend);

            //Usuario
            Label lblUsuario = new Label();
            lblUsuario.AssociatedControlID = "txtUsuario";
            lblUsuario.Text = "Usuário";
            fieldset.Controls.Add(lblUsuario);

            TextBox txtUsuario = new TextBox();
            txtUsuario.ID = "txtUsuario";
            fieldset.Controls.Add(txtUsuario);

            RequiredFieldValidator rfvUsuario = new RequiredFieldValidator();
            rfvUsuario.ID = "rfvUsuario";
            rfvUsuario.ControlToValidate = "txtUsuario"; 
            rfvUsuario.ErrorMessage = "Preencher Email";
            rfvUsuario.Display= ValidatorDisplay.None;
            fieldset.Controls.Add(rfvUsuario);

            fieldset.Controls.Add(new LiteralControl("<p/>"));

            //Senha
            Label lblSenha = new Label();
            lblSenha.AssociatedControlID = "txtSenha";
            lblSenha.Text = "Senha";
            fieldset.Controls.Add(lblSenha);

            TextBox txtSenha = new TextBox();
            txtSenha.ID = "txtSenha";
            txtSenha.TextMode = TextBoxMode.Password;
            fieldset.Controls.Add(txtSenha);

            RequiredFieldValidator rfvSenha = new RequiredFieldValidator();
            rfvSenha.ID = "rfvSenha";
            rfvSenha.ControlToValidate = "txtSenha";
            rfvSenha.ErrorMessage = "Preencher Senha";
            rfvSenha.Display = ValidatorDisplay.None;
            fieldset.Controls.Add(rfvSenha);

            fieldset.Controls.Add(new LiteralControl("<p/>"));

            //Continuar Conectado
            CheckBox chkContinuarConectado = new CheckBox();
            chkContinuarConectado.ID = "chkContinuarConectado";
            chkContinuarConectado.Text = "Continuar Conectado";

            fieldset.Controls.Add(new LiteralControl("<p/>"));

            Controls.Add(fieldset);

            //Botão Logar
            Button btnLogar = new Button();
            btnLogar.ID = "btnLogar";
            btnLogar.Text = "Logar";
            btnLogar.Click += new EventHandler(btnLogar_Click);

            Controls.Add(btnLogar);
        }


        void btnLogar_Click(object sender, EventArgs e)
        {
            if (Logar != null) Logar(this, e);
        }
    }
}
