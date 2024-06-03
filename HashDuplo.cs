using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace apCaminhosEmMarte
{
    public class HashDuplo<Tipo> : ITabelaDeHash<Tipo>
    where Tipo : IRegistro<Tipo>
    {
        private Tipo[] tabelaDeHash;
        private int quantidadeElementos = 0;
        private int tamanhoPadrao = 131;

        public Tipo[] Dados { get => tabelaDeHash; }

        public HashDuplo()
        {
            tabelaDeHash = new Tipo[tamanhoPadrao];
        }

        private int Hash(string chave)
        {
            for (int i = 0; i < tabelaDeHash.Length; i++)
            {
                if (tabelaDeHash[i] != null && chave.Equals(tabelaDeHash[i].Chave))
                {
                    return i;
                }
            }

            long total = 0;

            for (int i = 0; i < chave.Length; i++)
                total += 37 * total + (char)chave[i];

            total = total % tabelaDeHash.Length;

            if (total < 0)
                total += tabelaDeHash.Length;


            int posicaoInicial = (int)total;
            int posicaoAtual = posicaoInicial;
            int colisoes = 0;
            int loops = 1;

            while (tabelaDeHash[posicaoAtual] != null && (posicaoAtual != posicaoInicial || colisoes == 0))
            {
                if (posicaoAtual == 0)
                    posicaoAtual = 2 + loops;


                posicaoAtual = posicaoAtual * 2;
                colisoes++;
            }

            return (int)total;
        }


        public List<Tipo> Conteudo()
        {
            var saida = new List<Tipo>();

            for (int i = 0; i < tabelaDeHash.Length; i++)
                if (tabelaDeHash[i] != null)
                    saida.Add(tabelaDeHash[i]);

            return saida;
        }

        public bool Existe(Tipo item, out int posicao)
        {
            posicao = Hash(item.Chave);

            if (tabelaDeHash[posicao].Equals(item))
                return true;


            return false;
        }

        public void Inserir(Tipo item)
        {
            if (quantidadeElementos < tabelaDeHash.Length)
            {
                int valorHash = Hash(item.Chave);
                tabelaDeHash[valorHash] = item;
            }

        }

        public bool Remover(Tipo item)
        {
            int valorDeHash = Hash(item.Chave);

            if (tabelaDeHash[valorDeHash] != null && tabelaDeHash[valorDeHash].Equals(item))
            {
                tabelaDeHash[valorDeHash].Equals(null);
                return true;
            }
            return false;
        }
    }
}