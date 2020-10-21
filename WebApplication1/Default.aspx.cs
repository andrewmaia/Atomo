using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Web.Caching;
//using WebApplication1_Teste;
using Atomo.Data;

namespace WebApplication1
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ReflectionBase rb = new ReflectionBase();
                rb.Conn = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=SIIO;Data Source=ANDREW-PC";
                Ideia ideiaModel = new Ideia();
                ideiaModel.IdeiaID = 1;
                Ideia ideiaModelResultado = rb.EncapsulatedRead<Ideia>(ideiaModel, "dbo.Ideia_SelecionarPorID", 0, true);
                Label1.Text = ideiaModelResultado.PreDescricao;

                ideiaModel.IdeiaID = 2;

                ideiaModelResultado = rb.EncapsulatedRead<Ideia>(ideiaModel, "dbo.Ideia_SelecionarPorID", 0, true);
                Label2.Text = ideiaModelResultado.PreDescricao;


                //CacheBase cb = new CacheBase();
                //if (cb["TesteAndrew"] == null)
                //{
                //using (SqlConnection conn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=SIIO;Data Source=ANDREW-PC"))
                //{
                //    SqlCommand command = new SqlCommand("dbo.Ideia_SelecionarPorID", conn);
                //    command.CommandType = CommandType.StoredProcedure;
                //    command.Parameters.Add(new SqlParameter("@IdeiaID", 1));
                //    SqlDependencyExpiration dependency = new SqlDependencyExpiration(command);
                //    conn.Open();
                //    SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                //    while (dr.Read())
                //    {
                //        Label1.Text = dr["preDescricao"].ToString();
                //    }
                //    dr.Close();
                //    cb.Add("TesteAndrew", Label1.Text, dependency);
                //}
                //}
                //else
                //    Label1.Text = cb["TesteAndrew"].ToString(); 

            }
        }
    }



    public class Ideia
    {
        public int IdeiaID
        {
            get { return this.ideiaID; }
            set { this.ideiaID = value; }
        }
        private int ideiaID;

        public int CategoriaID
        {
            get { return this.categoriaID; }
            set { this.categoriaID = value; }
        }
        private int categoriaID;

        public string CategoriaDescricao
        {
            get { return this.categoriaDescricao; }
            set { this.categoriaDescricao = value; }
        }
        private string categoriaDescricao;

        public Guid Aspnet_UserID
        {
            get { return this.aspnet_UserID; }
            set { this.aspnet_UserID = value; }
        }
        private Guid aspnet_UserID;


        public string UsuarioNome
        {
            get { return this.usuarioNome; }
            set { this.usuarioNome = value; }
        }
        private string usuarioNome;

        public string Titulo
        {
            get { return this.titulo; }
            set { this.titulo = value; }
        }
        private string titulo;

        public string PreDescricao
        {
            get { return this.preDescricao; }
            set { this.preDescricao = value; }
        }
        private string preDescricao;

        public string Descricao
        {
            get { return this.descricao; }
            set { this.descricao = value; }
        }
        private string descricao;

        public bool Restrita
        {
            get { return this.restrita; }
            set { this.restrita = value; }
        }
        private bool restrita;

        public DateTime DataCadastro
        {
            get { return this.dataCadastro; }
            set { this.dataCadastro = value; }
        }
        private DateTime dataCadastro;
    }
}

