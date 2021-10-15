using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DatesCompare
{
    public class ListaItens
    {
        public int ID { get; set; }
        public string DataIncio { get; set; }
        public string DataFim { get; set; }
        public string Observacoes { get; set; }
    }

    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            ResetDatas();
        }

        private void dtpInicio_ValueChanged(object sender, EventArgs e)
        {
            this.dtpFim.MinDate = this.dtpInicio.Value.AddDays(7);
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtObservations.Text))
            {
                if (dgvDados.Rows.Count == 0)
                {
                    dgvDados.Rows.Add(dgvDados.Rows.Count + 1, dtpInicio.Value.ToString("dd/MM/yyyy"), dtpFim.Value.ToString("dd/MM/yyyy"), txtObservations.Text);

                    ResetDatas();

                    //MessageBox.Show("Sucesso ao incluir o agendamento", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);                   
                }                    
                else
                {
                    List<ListaItens> listaItens = new List<ListaItens>();

                    foreach (DataGridViewRow row in dgvDados.Rows)
                    {
                        listaItens.Add(new ListaItens
                        {
                            ID = Convert.ToInt32(row.Cells[0].Value),
                            DataIncio = row.Cells[1].Value.ToString(),
                            DataFim = row.Cells[2].Value.ToString(),
                            Observacoes = row.Cells[3].Value.ToString()
                        });
                    }

                    DateTime minDataInicio = listaItens.Min(x => ConvertInfoData(x.DataIncio)).Date,
                             maxDataFim = listaItens.Max(x => ConvertInfoData(x.DataFim)).Date,
                             datainicio = ConvertInfoData(dtpInicio.Text).Date,
                             dataFim = ConvertInfoData(dtpFim.Text).Date;

                    bool flagPermiteAfastamento = false;

                    if (dataFim < minDataInicio)
                        flagPermiteAfastamento = true;

                    if (datainicio > maxDataFim)
                        flagPermiteAfastamento = true;

                    if (flagPermiteAfastamento)
                        dgvDados.Rows.Add(dgvDados.Rows.Count + 1, dtpInicio.Value.ToString("dd/MM/yyyy"), dtpFim.Value.ToString("dd/MM/yyyy"), txtObservations.Text);
                    else
                        MessageBox.Show("Já existe um agendamento ativo ou agendado para as datas informadas", "Aviso!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    //Final
                    //ResetDatas();
                }                
            }
            else
            {
                MessageBox.Show("Favor preencher as observações", "Aviso!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private DateTime ConvertInfoData(string textoData)
        {
            return Convert.ToDateTime(textoData.Substring(3, 2) + "-" + textoData.Substring(0, 2) + "-" + textoData.Substring(6, 4));
        }

        private void ResetDatas()
        {
            this.dtpInicio.Value = DateTime.Now.AddDays(5);
            this.dtpFim.Value = dtpInicio.Value.AddDays(7);
        }
    }
}