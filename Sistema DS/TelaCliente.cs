using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistema_DS
{
    public partial class TelaCliente : Form
    {

        string conexao = ConfigurationManager.ConnectionStrings["bd_aula"].ConnectionString;
        public TelaCliente()
        {
            InitializeComponent();
            listar();
        }

        public void limpartxtbox()
        {
            txtcodigo.Text = "";
            txtnome.Text = "";
            txttelefone.Text = "";
            txtemail.Text = "";
        }

        //funcao listar

        public void listar()
        {
            try
            {

                //1 passo - conexao com banco de dados
                MySqlConnection con = new MySqlConnection(conexao);

                //2 passo montar e executar o comando sql
                string sql_select_cliente = "select * from tb_cliente";

                //3 passo montar e organizar o comando sql
                MySqlCommand executacmdMySql_select = new MySqlCommand(sql_select_cliente, con);
                con.Open();
                executacmdMySql_select.ExecuteNonQuery();


                //4 passo criar uma data table
                DataTable tabela_cliente = new DataTable();

                //5 passo criar o MySqlDataAdapter
                MySqlDataAdapter da_cliente = new MySqlDataAdapter(executacmdMySql_select);

                da_cliente.Fill(tabela_cliente);

                //6 passo exicbir o data table no datagridview
                DgvListaCliente.DataSource = tabela_cliente;

                con.Close();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Deu o erro: " + erro);
            }
        }
        

        private void btncadastrar_Click(object sender, EventArgs e)
        {
            

            try
            {
                //Conexao com o banco de dados
                MySqlConnection con = new MySqlConnection(conexao);

                //2 passo - pegar os dados da tela

                string nome, telefone, email;
                nome = txtnome.Text;
                telefone = txttelefone.Text;
                email = txtemail.Text;

                //3 passo - montar e executar o comando sql (Insert into)
                string sql_insert = @"insert into tb_cliente (tb_cliente_nome, tb_cliente_telefone, tb_cliente_email) 
                                                            values (@cliente_nome, @cliente_tel, @cliente_email)";

                //Montar e organizar o comando
                MySqlCommand executacmdMysql_insert = new MySqlCommand(sql_insert, con);
                executacmdMysql_insert.Parameters.AddWithValue("@cliente_nome", nome);
                executacmdMysql_insert.Parameters.AddWithValue("@cliente_tel", telefone);
                executacmdMysql_insert.Parameters.AddWithValue("@cliente_email", email);

                //abri a conexao
                con.Open();

                //Executa comando sql
                executacmdMysql_insert.ExecuteNonQuery();


                //fechar a conexao
                con.Close();
                MessageBox.Show("Cadastro de cliente realizado com sucesso!");

                limpartxtbox();
                listar();

            }
            catch (Exception erro)
            {
                MessageBox.Show("Ocorreu o erro: " + erro);

            }
        }

        private void DgvListaCliente_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtcodigo.Text = DgvListaCliente.CurrentRow.Cells[0].Value.ToString();
            txtnome.Text = DgvListaCliente.CurrentRow.Cells[1].Value.ToString();
            txttelefone.Text = DgvListaCliente.CurrentRow.Cells[2].Value.ToString();
            txtemail.Text = DgvListaCliente.CurrentRow.Cells[3].Value.ToString();
        }

        private void btnatualizar_Click(object sender, EventArgs e)
        {
            try
            {
                int codigo;
                string nome;
                string telefone;
                string email;


                codigo = int.Parse(txtcodigo.Text);
                nome = txtnome.Text;
                telefone = txttelefone.Text;
                email = txtemail.Text;



                MySqlConnection con = new MySqlConnection(conexao);
                con.Open();

                string sql_update_cliente = @"update tb_cliente set tb_cliente_nome = @nome,tb_cliente_telefone = @telefone, tb_cliente_email = @email where tb_cliente_id = @id";

                MySqlCommand executaMySql_Update_cliente = new MySqlCommand(sql_update_cliente, con);

                executaMySql_Update_cliente.Parameters.AddWithValue("@id", codigo);
                executaMySql_Update_cliente.Parameters.AddWithValue("@nome", nome);
                executaMySql_Update_cliente.Parameters.AddWithValue("@telefone", telefone);
                executaMySql_Update_cliente.Parameters.AddWithValue("@email", email);

                executaMySql_Update_cliente.ExecuteNonQuery();

                MessageBox.Show("Registro atualizado!");
                listar();
                limpartxtbox();
                con.Close();
                
            }
            catch (Exception erro)
            {
                MessageBox.Show("Ocorreu o erro: " + erro);
            }
        }

        private void btnexcluir_Click(object sender, EventArgs e)
        {
            try
            {

                //1 conexao com o banco de dados
                MySqlConnection con = new MySqlConnection(conexao);

                //2 receber os dados do cliente que sera excluido

                int id = int.Parse(txtcodigo.Text);

                string sql_delete_cliente = @"delete from tb_cliente where tb_cliente_id = @id";


                // abri a conexao
                con.Open();
                // Montar e organizar o sql command
                MySqlCommand executacmdMySql_delete = new MySqlCommand(sql_delete_cliente, con);

                executacmdMySql_delete.Parameters.AddWithValue("@id", id);

                //executar o comando

                executacmdMySql_delete.ExecuteNonQuery();

                con.Close();
                MessageBox.Show("Cliente excluido com sucesso!!");

                listar();
                limpartxtbox();


            }
            catch (Exception erro)
            {

                string mensagem = "Ocorreu o erro:" + erro;
                MessageBox.Show(mensagem, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
