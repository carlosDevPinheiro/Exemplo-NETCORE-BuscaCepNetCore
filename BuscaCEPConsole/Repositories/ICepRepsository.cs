using System.Collections.Generic;
using System.Threading.Tasks;
using BuscaCEPConsole.Models;

namespace BuscaCEPConsole.Repositories
{
    public interface ICepRepsository
    {
        /// <summary>
        /// Metodo adiciona um novo cep no banco.
        /// </summary>
        /// <param name="cep"></param>
        /// <returns></returns>
        public Task<int> AddCepAsync(CepModel cep);
        
        /// <summary>
        ///  Metodo obtem todos ceps registrados no banco
        /// </summary>
        /// <returns></returns>
        public Task<List<CepModel>> GetAllCeps();


        /// <summary>
        /// Metodo obtem todos ceps registrados no banco que complemento foi informado
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<CepModel>> GetAll();
        public Task<int> Commit();
    }
}