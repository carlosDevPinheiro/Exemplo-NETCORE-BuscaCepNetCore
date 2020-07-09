using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BuscaCEPConsole.Data;
using BuscaCEPConsole.Models;
using BuscaCEPConsole.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace BuscaCEPConsole
{
    class Program
    {
        private static  Uri Url { get; set; }
         

        static async Task Main(string[] args)
        {
            // RESOLUÇÃO DE DEPENDECIAS IOC
            var services =  RegistroDepedencyInjection();
            

            bool valido = false;
            while (valido == false)
            {
                Console.WriteLine("Deseja digitar um CEP para Busca ? ");
                var searchCep = Console.ReadLine()?.Replace("-","").Trim();
                valido = ValidaCep(searchCep);
            }

            // 

            try
            {
                HttpClient _http = new HttpClient();
                var resultado = await _http.GetAsync(Url);

                if (resultado.IsSuccessStatusCode)
                {
                    var conteudoResultado = await resultado.Content.ReadAsStringAsync();
                    var cepInfo = JsonConvert.DeserializeObject<CepModel>(conteudoResultado);

                    ICepRepsository cepRepsository = services.GetService<ICepRepsository>();

                    var res = await cepRepsository.AddCepAsync(cepInfo);
                    await cepRepsository.Commit();


                    Console.WriteLine($"Linhas afetadas {res}");

                    Console.WriteLine("\n CEP's que complemento foi informado.");
                    foreach (var cep in await cepRepsository.GetAll())
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(" CEP: {0}".PadLeft(20, '-'), cep.Cep);
                    }

                    Console.WriteLine("\n CEP's Regitrados.");

                    foreach (var cep in await cepRepsository.GetAllCeps())
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(" CEP: {0}".PadLeft(20, '-'), cep.Cep);
                    }


                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" Fim da Execução.");
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);

                foreach (DictionaryEntry dictionaryEntry in ex.Data)
                {
                    Console.WriteLine($" {dictionaryEntry.Key} : {dictionaryEntry.Value}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                
            }


           

        }

        private static bool ValidaCep(string searchCep)
        {
            if (string.IsNullOrEmpty(searchCep))
            {
                Url = new Uri("https://viacep.com.br/ws/01001000/json/");
                return true;
            }
            else
            {
                // verifica se tem algum digito que não é numero
                var listNumbers = searchCep.Where(char.IsNumber).ToList();

                if (listNumbers.Count() == 8)
                {
                    Url = new Uri($"https://viacep.com.br/ws/{searchCep}/json/");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Cep invalido !! {searchCep}");
                    Console.WriteLine("Digite novamente ou se Deseja usar o cep default tecle 'S'");
                    var confirmacao = Console.ReadLine();

                    if (confirmacao?.Length == 1 && confirmacao.ToLower().StartsWith("s"))
                    {
                        Url = new Uri("https://viacep.com.br/ws/01001000/json/");
                        return true;
                    }
                }
            }

            return false;
        }

        private static IServiceProvider RegistroDepedencyInjection()
        {
            IServiceCollection collection = new ServiceCollection();
            collection.AddDbContext<ApplicationDbContext>();
            collection.AddTransient<ICepRepsository, CepRepository>();
          
            IServiceProvider serviceProvider = collection.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
