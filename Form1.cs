﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosEmMarte
{
  public partial class FrmCaminhos : Form
  {
    public FrmCaminhos()
    {
      InitializeComponent();
    }

    ITabelaDeHash<Cidade> tabela;

    private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
    {

    }

    private void btnLerArquivo_Click(object sender, EventArgs e)
    {
      if (dlgAbrir.ShowDialog() == DialogResult.OK)
      {
        if (rbBucketHash.Checked)
           tabela = new BucketHash<Cidade>();
        else
          if (rbHashLinear.Checked)
             tabela = new HashLinear<Cidade>();
          else 
            if (rbHashQuadratico.Checked)
               tabela = new HashQuadratico<Cidade>();
            else
              if (rbHashDuplo.Checked)
                tabela = new HashDuplo<Cidade>();

        var arquivo = new StreamReader(dlgAbrir.FileName);
        while (! arquivo.EndOfStream) 
        {
          Cidade umaCidade = new Cidade();
          umaCidade.LerRegistro(arquivo);
          tabela.Inserir(umaCidade);
        }
        lsbCidades.Items.Clear();  // limpa o listBox
        var asCidades = tabela.Conteudo();
        foreach (Cidade cid in asCidades)
          lsbCidades.Items.Add(cid);
        arquivo.Close();
      }
    }

    private void FrmCaminhos_FormClosing(object sender, FormClosingEventArgs e)
    {
      // abrir o arquivo para saida, se houver um arquivo selecionado
      // obter todo o conteúdo da tabela de hash
      // percorrer o conteúdo da tabela de hash, acessando
      // cada cidade individualmente e usar esse objeto Cidade
      // para gravar seus próprios dados no arquivo
      // fechar o arquivo ao final do percurso
    }

    private void FrmCaminhos_Load(object sender, EventArgs e)
    {

    }

    private void tabControl1_Enter(object sender, EventArgs e)
    {

    }

    Cidade[] asCidades;
    int quantasCidades;   // tamanho lógico

    private void tpCaminhos_Enter(object sender, EventArgs e)
    {
      asCidades = new Cidade[25];
      quantasCidades = 0;
      // abrir o arquivo de cidades
      // enquanto o arquivo de cidades não acabar
      //    instancie um objeto da classe cidade
      //    faça esse objeto ler um registro de cidade
      //    adicione esse registro de cidade após a última
      //    posição usada do vetor de cidades
      //    incremente quantasCidades

      // fechar o arquivo de cidades
      // ordenar o vetor de cidades pelo atributo nome

      OrdenarCidades();
      // copiar os nomes de cada cidade nos cbxOrigem e cbxDestino
    }

    private void OrdenarCidades()
    {
      //asCidades[0] = new Cidade("Campinas", 0, 0);
      //asCidades[1] = new Cidade("Americana", 0, 0); 
      //asCidades[2] = new Cidade("Sumaré", 0, 0);
      //asCidades[3] = new Cidade("Estiva Gerbi", 0, 0);
      //asCidades[4] = new Cidade("Rafard", 0, 0); 
      //asCidades[5] = new Cidade("Rifaina", 0, 0);
      //asCidades[6] = new Cidade("Hortolândia", 0, 0);
      //quantasCidades = 7;

      // Ordenação por seleção direta ou
      // Selection Sort
      for (int lento= 0; lento < quantasCidades; lento++)
      {
        int indiceMenorCidade = lento;
        for (int rapido = lento + 1; rapido < quantasCidades; rapido++)
          if (asCidades[rapido].NomeCidade.CompareTo(
                asCidades[indiceMenorCidade].NomeCidade) < 0)
            indiceMenorCidade = rapido;

          if (indiceMenorCidade != lento)
          {
            Cidade auxiliar = asCidades[indiceMenorCidade];
            asCidades[indiceMenorCidade] = asCidades[lento];
            asCidades[lento] = auxiliar;
          }
      }
    }
  }
}
