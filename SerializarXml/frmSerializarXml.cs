using Conferencia.ModelSerialization;
using Conferencia.Relatorios;
using Microsoft.Reporting.WinForms;
using Newtonsoft.Json;
using SerializarXml.Serializable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SerializarXml
{
    public partial class frmSerializarXml : Form
    {
        private string status = "Não conferido";
        private Color backColor = Color.White;
        private Color foreColer = Color.Black;
        private int qtdRecebida;
        private int qtdCom;
        private List<Produto> produtos = new List<Produto>();
        private DataTable dataTable = new DataTable();

        public frmSerializarXml()
        {
            InitializeComponent();
            tabControl1.SelectedIndex = 3;
        }

        private void btnLerXml_Click(object sender, EventArgs e)
        {
            LerXml();
            tabControl1.SelectedIndex = 3;
            txtCodigo.Focus();
            dgvProdutos.FirstDisplayedScrollingRowIndex = 1;
            refrechStatus();
        }

        private void LerXml()
        {
            try
            {
                openFileXml.Reset();
                openFileXml.Filter = "Arquivo xml (*.xml)|*.xml";
                if (openFileXml.ShowDialog() == DialogResult.OK)
                {
                    txtpathXml.Text = openFileXml.SafeFileName;

                    NFeSerialization serializable = new NFeSerialization();
                    var nfe = serializable.GetObjectFromFile<NFeProc>(openFileXml.FileName);

                    if (nfe == null)
                    {
                        MessageBox.Show("Falha ao ler o arquivo xml. Verifique se o arquivo é de uma NF-e/NFC-e autorizada!", "Aviso - Leitura do Arquivo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        popularForm(nfe);
                        //MessageBox.Show("Arquivo xml da Nota Fiscal lido com Sucesso!", "Aviso - Leitura do Arquivo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Falha no processo de leitura do arquivo xml da Nota Fiscal.", "Aviso - Leitura do Arquivo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void popularForm(NFeProc nfe)
        {
            lblMessage.Visible = false;
            /* Populando tab Identificação */
            txtNaturezaOperacao.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.natOp;
            txtNumero.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.nNF;
            txtModelo.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.mod;
            txtSerie.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.serie.ToString();
            txtDataEmissao.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.dhEmi.ToShortDateString();

            /* Populando tab Emitente */
            txtRazaoSocial.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.xNome;
            txtNomeFantasia.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.xFant;
            txtCpfCnpjEmitente.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.CNPJ;
            txtInscricaoEstadual.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.IE;
            txtLogradouroEmitente.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.xLgr;
            txtNroEmitente.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.nro;
            txtMunicipioEmitente.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.xMun;
            txtUFEmitente.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.UF;

            /* Populando tab Destinatário */
            txtDestNomeFantasia.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.xNome;
            txtDestCpfCnpj.Text = string.IsNullOrEmpty(nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.CNPJ) ? nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.CPF : nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.CNPJ;
            txtDestEmail.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.email;
            txtDestLogradouro.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.Endereco.xLgr;
            txtDestNumero.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.Endereco.nro;
            txtDestMunicipio.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.Endereco.xMun;
            txtDestUF.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.Endereco.UF;
            txtDestCEP.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.Endereco.CEP;
            txtDestBairro.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Destinatario.Endereco.xBairro;

            //produtos = new BindingList<Produto>();
            produtos.Clear();
            produtoBindingSource.Clear();
            dgvProdutos.DataSource = null;
            txtTotalItens.Text = nfe.NotaFiscalEletronica.InformacoesNFe.Detalhe?.Count().ToString();
            foreach (var item in nfe.NotaFiscalEletronica.InformacoesNFe.Detalhe)
            {
                produtos.Add(item.Produto);
            }

            //dataTable.DataSet = produtos;

            produtoConferenciaBindingSource.DataSource = produtos;
            produtoBindingSource.DataSource = produtos;

            dgvProdutos.DataSource = produtoBindingSource;
        }

        private void dgvProdutos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void txtQtd_DoubleClick(object sender, EventArgs e)
        {
            txtQtd.Enabled = false;
        }

        private void txtCodigo_DoubleClick(object sender, EventArgs e)
        {
            txtQtd.Enabled = true;
            txtQtd.Focus();
        }

        private void txtQtd_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void txtQtd_Leave(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtQtd.Text) < 0)
            {
                MessageBox.Show("A quantidade não deve ser menor que zero (0)");
            }
            txtQtd.Enabled = false;
            txtCodigo.Focus();
        }

        private void txtQtd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bool isChecked = false;
                lblMessage.Visible = isChecked;

                foreach (DataGridViewRow row in dgvProdutos.Rows)
                {
                    int qtd = Convert.ToInt32(txtQtd.Text);

                    string codigo = dgvProdutos.Rows[row.Index].Cells["ean"].Value.ToString();
                    if (codigo == txtCodigo.Text)
                    {
                        dgvProdutos.ClearSelection();
                        qtdRecebida = Convert.ToInt32(dgvProdutos.Rows[row.Index].Cells["qtdRecebido"].Value) + qtd;
                        qtdCom = Convert.ToInt32(dgvProdutos.Rows[row.Index].Cells["qtdEntrada"].Value);
                        dgvProdutos.Rows[row.Index].Cells["qtdRecebido"].Selected = true;
                        dgvProdutos.Rows[row.Index].Cells["qtdRecebido"].Value = qtdRecebida;

                        Status(row.Index);

                        //validaStatus(qtdRecebida, qtdCom);

                        isChecked = true;

                        break;
                    }
                }

                refrechStatus();
                lblMessage.Visible = !isChecked;
                lblMessage.Text = "Não foi encontrado o código: " + txtCodigo.Text;
                txtCodigo.Text = "";
                txtCodigo.Focus();
                //MessageBox.Show("Botão" + " - " + e.KeyValue);
                //enter key is down
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            //Define as extensões permitidas
            saveFileDialog.Filter = "Text File|*.json";
            //Atribui um valor vazio ao nome do arquivo
            saveFileDialog.FileName = txtNumero.Text + ".json";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string output = JsonConvert.SerializeObject(this.dgvProdutos.DataSource);
                //System.IO.File.WriteAllText(txtNumero.Text + ".json", output);

                //Cria um stream usando o nome do arquivo
                FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create);

                //Cria um escrito que irá escrever no stream
                StreamWriter writer = new StreamWriter(fs);
                //escreve o conteúdo da caixa de texto no stream
                writer.Write(output);
                //fecha o escrito e o stream
                writer.Close();
            }
        }

        private void frmSerializarXml_Load(object sender, EventArgs e)
        {
        }

        private void dgvProdutos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("dgvProdutos_CellEndEdit");
        }

        private void dgvProdutos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("dgvProdutos_CellDoubleClick");
            //bool t = produtoConferenciaBindingSource.AllowEdit;
            //dgvProdutos.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
            //dgvProdutos.Rows[e.RowIndex].Cells[e.ColumnIndex].InitializeEditingControl(e.RowIndex);
        }

        private void dgvProdutos_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            MessageBox.Show("dgvProdutos_CellBeginEdit");
        }

        private void dgvProdutos_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("dgvProdutos_CellEnter");
            //Status(e.RowIndex);
        }

        private void dgvProdutos_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            //txtCodigo.Focus();
            //Status(e.RowIndex);
        }

        private void dgvProdutos_CancelRowEdit(object sender, QuestionEventArgs e)
        {
            //txtCodigo.Focus();
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            try
            {
                openFileXml.FilterIndex = 2;
                openFileXml.RestoreDirectory = true;
                //Define as extensões permitidas
                openFileXml.Filter = "Text File|*.json";
                //Atribui um valor vazio ao nome do arquivo
                //openFileXml.FileName = txtNumero.Text + ".json";

                if (openFileXml.ShowDialog() == DialogResult.OK)
                {
                    txtpathXml.Text = openFileXml.SafeFileName;
                    string filePathWithoutExt = Path.ChangeExtension(txtpathXml.Text, null);
                    txtNumero.Text = filePathWithoutExt;

                    StreamReader r = new StreamReader(openFileXml.FileName);
                    string jsonString = r.ReadToEnd();
                    produtos = JsonConvert.DeserializeObject<List<Produto>>(jsonString);
                    r.Close();
                    if (produtos == null)
                    {
                        MessageBox.Show("Falha ao ler o arquivo xml. Verifique se o arquivo é de uma NF-e/NFC-e autorizada!", "Aviso - Leitura do Arquivo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        dgvProdutos.DataSource = produtos;
                        txtCodigo.Focus();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Falha no processo de leitura do arquivo xml da Nota Fiscal.", "Aviso - Leitura do Arquivo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void Status(int rowIndex)
        {
            if (rowIndex < 0) return;
            qtdCom = Convert.ToInt32(dgvProdutos.Rows[rowIndex].Cells["qtdEntrada"].Value);
            qtdRecebida = Convert.ToInt32(dgvProdutos.Rows[rowIndex].Cells["qtdRecebido"].Value);
            //sta = (int)dgvProdutos.Rows[e.RowIndex].Cells["qtdRecebidoDataGridViewTextBoxColumn"].Value;

            validaStatus(qtdRecebida, qtdCom);

            dgvProdutos.Rows[rowIndex].Cells["statusConferencia"].Value = status;
            dgvProdutos.Rows[rowIndex].Cells["statusConferencia"].Style.BackColor = backColor;
            dgvProdutos.Rows[rowIndex].Cells["statusConferencia"].Style.ForeColor = foreColer;

            //dgvProdutos.Rows[rowIndex].Selected = true;
            dgvProdutos.FirstDisplayedScrollingRowIndex = rowIndex;
        }

        public void validaStatus(int qtdRecebida, int qtdEntrada)
        {
            status = "Não conferido";
            backColor = Color.White;
            foreColer = Color.Black;

            if (qtdRecebida > 0)
            {
                if (qtdRecebida == qtdCom)
                {
                    status = "OK";
                    backColor = Color.Lime;
                    foreColer = Color.Green;
                }
                else if (qtdRecebida < qtdCom)
                {
                    status = "Em conferência";
                    backColor = Color.Beige;
                    foreColer = Color.Goldenrod;
                }
                else
                {
                    status = "Recebido a mais";
                    backColor = Color.MistyRose;
                    foreColer = Color.Red;
                }
            }
        }

        private void dgvProdutos_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //Status(e.RowIndex);
        }

        private void dgvProdutos_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex == -1) return;
            for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
            {
                DataGridViewRow newRow = dgvProdutos.Rows[i];
                Status(newRow.Index);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvProdutos.DataSource = comboBox1.Text == "Todos" ? produtos : produtos.Where(w => w.status == comboBox1.Text).ToList();
            dgvProdutos.Update();
            dgvProdutos.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //produtoBindingSource.Filter = "xProd like '%" + textBox1.Text + "%'";
        }

        private void dgvProdutos_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            Console.WriteLine(e.Row.ToString());
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(this.dgvProdutos.Width, this.dgvProdutos.Height);
            dgvProdutos.DrawToBitmap(bm, new Rectangle(0, 0, this.dgvProdutos.Width, this.dgvProdutos.Height));
            e.Graphics.DrawImage(bm, 0, 0);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //printDGV.Print_DataGridView(dgvProdutos);
            BindingSource newDt = new BindingSource();
            newDt = (BindingSource)dgvProdutos.DataSource;
            using (var frm = new frmRelatorios(newDt))
            {
                frm.ShowDialog(this);
            }
        }

        private void toolStripStatusLabel_MouseHover(object sender, EventArgs e)
        {
            //toolTip.SetToolTip(toolStripStatusLabel.ToolTipText, null);
        }

        public void refrechStatus()
        {
            txtTotalItens.Text = produtos?.Count.ToString();
            txtItensOk.Text = produtos?.Where(w => w.status.Contains("OK")).ToList()?.Count.ToString();
            txtItensNaoConferidos.Text = produtos?.Where(w => w.status.Contains("Não conferido")).ToList()?.Count.ToString();
            txtItensEmConferecia.Text = produtos?.Where(w => w.status.Contains("Em conferência")).ToList()?.Count.ToString();
            txtItensRecebidoAMais.Text = produtos?.Where(w => w.status.Contains("Recebido a mais")).ToList()?.Count.ToString();
        }
    }
}