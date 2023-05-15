using CesarDev.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CesarDev.Business.Interfaces
{
    public interface IFornecedorService
    {
        Task Adicionar(Fornecedor fornecedor);
        Task Atualizar(Fornecedor fornecedor);
        Task Remover(Fornecedor fornecedor);
        Task AtualizarEndereco(Fornecedor fornecedor);

    }
}
