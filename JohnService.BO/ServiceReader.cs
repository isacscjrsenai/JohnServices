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
        public static Services ServiceRequest;
        //lendo o arquivo
        static List<string> linhas = File.ReadAllLines("solicitacao.txt", System.Text.Encoding.UTF8).ToList();
        //removendo as linhas em branco e só com espaço
        public static void FillFirstLine()
        {
            if (!(linhas[0].Contains("OS") || linhas[0].ToLower().Contains("ordem de serviço") || linhas[0].ToLower().Contains("ordem de servico")))
            {
                linhas.Insert(0, "Ordem de Serviço:");
            }
        }
        public static void GetKeyAndValue(int i,out string chave, out string valor,out int newI)
        {
            //pega a primeira linha e tira os espaços em branco
            chave = linhas[i].Trim();
            //remove os dois pontos
            chave = chave.Remove(chave.Length - 1);
            //coloca a texto da chave como um titulo ex:"Texto em Formato de Titulo"
            TextInfo textInfo = new CultureInfo("pt-BR", false).TextInfo;
            chave = textInfo.ToTitleCase(chave);
            //locola a segunda linha na variavel valor tirando qualquer espaço em branco
            valor = linhas[i + 1].Trim();
            IsRequestorInfo(chave, out chave);
            IsKeyProblem(chave,valor, out valor);
            IsEmptyValue(valor, i,out valor,out newI);
            IsMultiLineValue(i, out newI, out valor);

        }
        public static void IsEmptyValue(string value,int i, out string newValue,out int newI)
        {
            if (Regex.IsMatch(value, ".*:$"))
            {
                newValue = "";
                newI = i--;
            }
            else
            {
                newValue = value;
                newI = i;
            }
            
        }
        public static void IsKeyProblem(string key, string value, out string newValue)
        {
            if (key.Equals("Problema"))
            {
                string problem = value;
                problem = problem.Split("RESIDÊNCIA OU CAPITALIZAÇÃO /".ToCharArray())[1].Trim();
                newValue = problem;
            }
            else
            {
                newValue = value;
            }
        }
        public static void IsRequestorInfo(string key, out string newKey)
        {
            //se a chave já existe no dicionário então é porque a informação é sobre o solicitante
            if (OrdemDeServico.ContainsKey(key))
            {
                newKey = key +" do Solicitante";
            }
            else
            {
                newKey = key;
            }
        }
        public static void IsMultiLineValue(int i, out int newI, out string newValue)
        {
            //se o i for maior que o numero de linhas - 3 não há como ter
            //um valor com multiplas linhas então sai com o mesmo i que entrou
            //e retorna
            if (i > linhas.Count - 3) 
            {
                newValue = linhas[i+1].Trim();
                newI = i;
                return;
            }
            //se a linha seguinte a valor não tiver um dois pontos então a linha
            //também faz parte do valor 
            string nextLine = linhas[i+2].Trim();
            string pattern = @".+:$";
            //se a proxima linha depois do valor tem dois pontos então não se trata
            //de um valor com multiplas linhas então sai com o i que entrou e retorna.
            if (Regex.IsMatch(nextLine, pattern))
            {
                newValue = linhas[i + 1].Trim();
                newI = i;
                return;
            }
            else
            {
                newValue = $"{linhas[i + 1].Trim()}\n{nextLine}";
                newI = i++;
                IsMultiLineValue(i, out newI, out newValue);
            }
        }
        public static void ReadServiceOrder()
        {
            linhas.RemoveAll(linha => string.IsNullOrWhiteSpace(linha));
            
            //se não existe linha com o header OS cria a linha
            FillFirstLine();
            //percorre todas as linhas do texto armazenadas na lista linha
            //de duas em duas linhas a primeira é o que é a informação e a segunda a informação em si.
            for (var i = 0; i <= linhas.Count - 2; i += 2)
            {
                string chave, valor;
                GetKeyAndValue(i, out chave, out valor, out i);
                OrdemDeServico.Add(chave, valor);
            }
            ServiceRequest = new Services(OrdemDeServico);
        }
    }
}
