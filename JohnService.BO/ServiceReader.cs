using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using JohnService.VO;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Text.RegularExpressions;

namespace JohnService.BO
{
    public static class ServiceReader
    {
        static Dictionary<string, string> OrdemDeServico = new Dictionary<string, string>();
        public static Services ServiceRequest = new Services(null);
        //lendo o arquivo
        static List<string> linhas = File.ReadAllLines("solicitacao.txt", System.Text.Encoding.UTF8).ToList();
        //removendo as linhas em branco e só com espaço
        public static void ReadServiceOrder()
        {
            linhas.RemoveAll(linha => string.IsNullOrWhiteSpace(linha));
            
            //se não existe linha com o header OS cria a linha
            if (!(linhas[0].Contains("OS") || linhas[0].ToLower().Contains("ordem de serviço") || linhas[0].ToLower().Contains("ordem de servico")))
            {
                linhas.Insert(0, "Ordem de Serviço:");
            }

            string pattern = ".*:$"; //texto que acaba com ":"
            for (var i = 0; i <= linhas.Count - 2; i += 2)
            {
                string chave = linhas[i].Trim();
                chave = chave.Remove(chave.Length - 1);
                TextInfo textInfo = new CultureInfo("pt-BR", false).TextInfo;
                chave = textInfo.ToTitleCase(chave);

                string valor = linhas[i + 1].Trim();
                string proxLinha ="";
                if( i <= linhas.Count - 3)
                {
                   proxLinha = linhas[i + 2].Trim();
                }
                

                while (!Regex.IsMatch(proxLinha, pattern)) 
                {
                    valor += "\n" + proxLinha;
                    i++;
                    if (i <= linhas.Count - 3)
                    {
                        proxLinha = linhas[i + 2].Trim();
                    }
                    else break;
                }

                if (Regex.IsMatch(valor, pattern)) 
                {
                    valor = "";
                    i--;
                }

                if (OrdemDeServico.ContainsKey(chave))
                {
                    chave += " do Solicitante";
                }
                if (chave.Equals("Problema"))
                {
                    string problema = valor;
                    problema = problema.Split("RESIDÊNCIA OU CAPITALIZAÇÃO /".ToCharArray())[1].Trim();
                    valor = problema;
                }
                
                
                OrdemDeServico.Add(chave, valor);
                
                
            }

            ServiceRequest = new Services(OrdemDeServico);
        }
        
        
    }
}
