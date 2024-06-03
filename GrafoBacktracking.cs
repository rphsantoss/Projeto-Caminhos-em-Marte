﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCidadesBacktracking
{
  class GrafoBacktracking
  {
    const int tamanhoDistancia = 4;
    char tipoGrafo;
    int qtasCidades;
    int[,] matriz;

    public GrafoBacktracking(string nomeArquivo)
    {
      var arquivo = new StreamReader(nomeArquivo);
      tipoGrafo = arquivo.ReadLine()[0]; // acessa primeiro caracter com tipo do grafo
      qtasCidades = int.Parse(arquivo.ReadLine());
      matriz = new int[qtasCidades, qtasCidades];

      for (int linha = 0; linha < qtasCidades; linha++)
      {
        string arestas = arquivo.ReadLine();
        for (int coluna = 0; coluna < qtasCidades; coluna++)
          matriz[linha, coluna] =
            int.Parse(arestas.Substring(coluna * tamanhoDistancia, tamanhoDistancia));
      }
    }

    public char TipoGrafo { get => tipoGrafo; set => tipoGrafo = value; }
    public int QtasCidades { get => qtasCidades; set => qtasCidades = value; }
    public int[,] Matriz { get => matriz; set => matriz = value; }

    public void Exibir(DataGridView dgv)
    {
      dgv.RowCount = dgv.ColumnCount = qtasCidades;
      for (int coluna = 0; coluna < qtasCidades; coluna++)
      {
        dgv.Columns[coluna].HeaderText = coluna.ToString();
        dgv.Rows[coluna].HeaderCell.Value = coluna.ToString();
        dgv.Columns[coluna].Width = 50;
      }
      for (int linha = 0; linha < qtasCidades; linha++)
        for (int coluna = 0; coluna < qtasCidades; coluna++)
          if (matriz[linha, coluna] != 0)
            dgv[coluna, linha].Value = matriz[linha, coluna];
    }

    public PilhaVetor<Movimento> BuscarCaminho(
                      int origem, int destino, ListBox lsb,
                      DataGridView dgvGrafo,
                      DataGridView dgvPilha)
    {
      int cidadeAtual, saidaAtual;
      bool achouCaminho = false,
           naoTemSaida = false;
      bool[] passou = new bool[qtasCidades];
      // inicia os valores de “passou” pois ainda não foi em nenhuma cidade
      for (int indice = 0; indice < qtasCidades; indice++)
        passou[indice] = false;
      lsb.Items.Clear();
      cidadeAtual = origem;
      saidaAtual = 0;
      var pilha = new PilhaVetor<Movimento>();

      while (!achouCaminho && !naoTemSaida)
      {
        naoTemSaida = (cidadeAtual == origem && saidaAtual == qtasCidades && pilha.EstaVazia);

        if (!naoTemSaida)
        {
          while ((saidaAtual < qtasCidades) && !achouCaminho)
          {
            // se não há saída pela cidade testada, verifica a próxima
            if (matriz[cidadeAtual, saidaAtual] == 0)
              saidaAtual++;
            else
            // se já passou pela cidade testada, vê se a próxima cidade permite saída
            if (passou[saidaAtual])
              saidaAtual++;
            else
            // se chegou na cidade desejada, empilha o local
            // e termina o processo de procura de caminho
            if (saidaAtual == destino)
            {
              Movimento movim = new Movimento(cidadeAtual, saidaAtual);
              pilha.Empilhar(movim);
              achouCaminho = true;
              lsb.Items.Add($"Saiu de {cidadeAtual} para {saidaAtual}");
              ExibirUmPasso();
            }
            else
            {
              lsb.Items.Add($"Saiu de {cidadeAtual} para {saidaAtual}");
              ExibirUmPasso();
              Movimento movim = new Movimento(cidadeAtual, saidaAtual);
              pilha.Empilhar(movim);
              passou[cidadeAtual] = true;
              cidadeAtual = saidaAtual; // muda para a nova cidade
              saidaAtual = 0; // reinicia busca de saídas da nova
                              // cidade a partir da primeira cidade
            }
          }
        }
        if (!achouCaminho)
          if (!pilha.EstaVazia)
          {
            var movim = pilha.Desempilhar();
            saidaAtual = movim.Destino;
            cidadeAtual = movim.Origem;
            lsb.Items.Add($"Voltando de {saidaAtual} para {cidadeAtual}");
            ExibirUmPasso();
            saidaAtual++;
          }
      }

      var saida = new PilhaVetor<Movimento>();
      if (achouCaminho)
      { // desempilha a configuração atual da pilha
        // para a pilha da lista de parâmetros
        while (!pilha.EstaVazia)
        {
          Movimento movim = pilha.Desempilhar();
          saida.Empilhar(movim);
        }
      }
      return saida;

      void ExibirUmPasso()
      {
        dgvGrafo.CurrentCell = dgvGrafo[saidaAtual, cidadeAtual];
        //pilha.Exibir(dgvPilha);
        Thread.Sleep(1000);
      }
    }
  }
}
