using System;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using DicCombo.Code;

namespace DicCombo
{
    public partial class Form1 : Form
    {
        public class Itm
        {
            public string VMember { get; set; } //  Value member
            public string DMember { get; set; } //  Display member

            public Itm()                        //  Constuctor
            {
                VMember = "";
                DMember = "";
            }
            public Itm(string v1, string v2)    //  Constructor overload
            {
                VMember = v1;
                DMember = v2;
            }

            public List<Itm> GetAllItems()
            {
                //  Return a list of items retrieved by sql command           
                List<Itm> L = new List<Itm>();

                try
                {
                    using ( SqlConnection cnn = new SqlConnection( Glb.CnnString))
                    {
                        SqlCommand cmd = new SqlCommand( "Select Id, Descr from Categories", cnn);
                        cnn.Open();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while ( rdr.Read())
                        {
                            Itm t = new Itm()
                            {
                                VMember = rdr[ "Id"].ToString(),
                                DMember = rdr[ "Descr"].ToString()
                            };
                            //  Add item to list
                            L.Add( t);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro na leitura do arquivo de items : " + ex.Message);
                }
                //  Retorna a lista de itens
                return L;
            }

            public DataSet GetAllItemsDs()
            {
                DataSet ds = new DataSet();
                try
                {
                    using ( SqlConnection cnn = new SqlConnection( Glb.CnnString))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter("Select Id, Descr from Categories", cnn))
                        {
                            cnn.Open();
                            da.Fill( ds, "Categories");
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro na leitura do arquivo de items : " + ex.Message);
                }
                //  Retorna dataset
                return ds;
            }
        }

        public Form1() => InitializeComponent();

        private void CmbBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBox.SelectedIndex > -1)
            {
                try
                {
                    if ( rbtAddItem.Checked)
                    {
                        //  Este modo de obter o conteudo evitou varios problemas reportados no Stack overflow
                        //  Nos outros casos a solucao no "else" é mais prática
                        //  Lembre-se que no caso do Dictionary não usamos a classe Itm e esta solucao nao funcionaria.
                        txtValueMember.Text   = ((Itm)cmbBox.Items[ cmbBox.SelectedIndex]).VMember;
                        txtDisplayMember.Text = ((Itm)cmbBox.Items[ cmbBox.SelectedIndex]).DMember;
                    }
                    else
                    {
                        txtDisplayMember.Text = cmbBox.Text;
                        txtValueMember.Text   = cmbBox.SelectedValue != null ? cmbBox.SelectedValue.ToString() : "";
                    };
                }
                catch ( Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void RbtAdd_Click(object sender, EventArgs e)
        {
            //  Antes de mais nada
            ClearView();

            //  Qual a opção selecionada
            if (rbtAdd.Checked)
            {
                cmbBox.Items.Add( "RS");
                cmbBox.Items.Add( "SP");
                cmbBox.Items.Add( "MG");
                cmbBox.Items.Add( "RJ");
                cmbBox.SelectedIndex = 0;
            }
            else if (rbtAddRange.Checked)
            {
                string[] stringArray = new string[]
                {
                    "RS", "SP", "MG", "RJ", "MT", "PR"
                };
                cmbBox.Items.AddRange(stringArray);
                cmbBox.SelectedIndex = 0;
            }
            else if (rbtAddItem.Checked)
            {
                cmbBox.Items.Add(new Itm( "PVBB", "Pagto a Vista - Boleto Bancario")      );
                cmbBox.Items.Add(new Itm( "PUCC", "Parcela Unica - Cartao de Credito")    );
                cmbBox.Items.Add(new Itm( "P3CC", "Pagto 3 parcelas - Cartao de credito") );

                cmbBox.ValueMember   = "VMember";
                cmbBox.DisplayMember = "DMember";
                cmbBox.SelectedIndex = 0;
            }
            else if (rbtAddList.Checked)
            {
                List<Itm> ItmLst = new List<Itm>()
                {
                    new Itm( "RS", "Rio Grande do Sul" ),
                    new Itm( "SP", "Sao Paulo"         ),
                    new Itm( "MG", "Minas Gerais"      ),
                    new Itm( "BA", "Bahia"             ),
                    new Itm( "RJ", "Rio de Janeiro"    ),
                    new Itm( "PR", "Parana"            )
                };
                cmbBox.ValueMember   = "VMember";
                cmbBox.DisplayMember = "DMember";
                cmbBox.DataSource    = ItmLst;
            }
            else if (rbtAddListaAlfa.Checked)
            {
                List<Itm> ItmLst = new List<Itm>()
                {
                    new Itm( "RS", "Rio Grande do Sul" ),
                    new Itm( "SP", "Sao Paulo"         ),
                    new Itm( "MG", "Minas Gerais"      ),
                    new Itm( "BA", "Bahia"             ),
                    new Itm( "RJ", "Rio de Janeiro"    ),
                    new Itm( "PR", "Parana"            ),
                };
                ItmLst.Sort((x, y) => x.DMember.CompareTo(y.DMember));
                cmbBox.ValueMember   = "VMember";
                cmbBox.DisplayMember = "DMember";
                cmbBox.DataSource    = ItmLst;
            }
            else if (rbtAddListaAlfaReversa.Checked)
            {
                List<Itm> ItmLst = new List<Itm>()
                {
                    new Itm( "RS", "Rio Grande do Sul" ),
                    new Itm( "SP", "Sao Paulo"         ),
                    new Itm( "MG", "Minas Gerais"      ),
                    new Itm( "BA", "Bahia"             ),
                    new Itm( "RJ", "Rio de Janeiro"    ),
                    new Itm( "PR", "Parana"            ),
                };
                ItmLst.Sort((x, y) => y.DMember.CompareTo(x.DMember));
                cmbBox.DataSource    = ItmLst;
                cmbBox.ValueMember   = "VMember";
                cmbBox.DisplayMember = "DMember";
            }
            else if (rbtDict.Checked)
            {
                Dictionary<int, string> dic = new Dictionary<int, string>()
                {
                    { 1, "Eletrodomestico"  },
                    { 2, "Informatica"      },
                    { 3, "Smartphones"      },
                    { 4, "Televisores OLED" }
                };
                cmbBox.DataSource = new BindingSource(dic, null);
                // ATENCAO : qdo usar dicionario valuemember tem que "Key" e DisplayMember = "Value"
                // por esse sao os valores da classe dictionary. Nao estamos usndo Itm aqui;
                cmbBox.ValueMember   = "Key";
                cmbBox.DisplayMember = "Value";
            }
            else if (rbtDb.Checked)
            {
                Itm T = new Itm();
                cmbBox.DataSource    = T.GetAllItems();
                cmbBox.ValueMember   = "VMember";
                cmbBox.DisplayMember = "DMember";
            }
            else if (rbtDS.Checked)
            { 
                Itm T = new Itm();
                cmbBox.DataSource    = new BindingSource( T.GetAllItemsDs(), "Categories");
                cmbBox.ValueMember   = "Id";
                cmbBox.DisplayMember = "Descr";
            }
        }

        private void ClearView()
        {
            cmbBox.DataSource     = null;
            cmbBox.Items.Clear();
            txtDisplayMember.Text = string.Empty;
            txtValueMember.Text   = string.Empty;
        }
    }
}
