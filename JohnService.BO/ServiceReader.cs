using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using JohnService.VO;

namespace JohnService.BO
{
    public class ServiceReader
    {
        Dictionary<string, string> OrdemDeServico = new Dictionary<string, string>();
        public Services ServiceRequest;
        //lendo o arquivo
        List<string> linhas = File.ReadAllLines("solicitacao.txt", System.Text.Encoding.UTF8).ToList();
        //removendo as linhas em branco e só com espaço
        public ServiceReader()
        {
            linhas.RemoveAll(linha => string.IsNullOrWhiteSpace(linha));
            //se não existe linha com o header OS cria a linha
            if (!(linhas[0].Contains("OS") || linhas[0].ToLower().Contains("ordem de serviço") || linhas[0].ToLower().Contains("ordem de servico")))
            {
                linhas.Insert(0, "Ordem de Serviço");
            }

            for (var i = 0; i <= linhas.Count - 2; i += 2)
            {
                Console.WriteLine(linhas[i]);
                if (OrdemDeServico.ContainsKey(linhas[i]))
                {
                    OrdemDeServico.Add(linhas[i] + " Solicitante", linhas[i + 1].Trim());
                }
                else
                {
                    if (linhas[i].Trim().ToLower().Equals("serviço:"))
                    {
                        OrdemDeServico.Add(linhas[i].Trim(), linhas[i + 1].Trim() + "\n" + linhas[i + 2]);
                        i++;
                    }
                    if (linhas[i].Trim().ToLower().Equals("problema:"))
                    {
                        string problema = linhas[i + 1].Trim();
                        problema = problema.Split("RESIDÊNCIA OU CAPITALIZAÇÃO /".ToCharArray())[1].Trim();
                        OrdemDeServico.Add(linhas[i].Trim(), problema);
                    }
                    else
                    {
                        OrdemDeServico.Add(linhas[i], linhas[i + 1].Trim());
                    }
                }
            }

            ServiceRequest = new Services(OrdemDeServico);
        }
        
    }
}
