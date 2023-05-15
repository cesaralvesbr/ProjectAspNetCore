using CesarDev.Business.Interfaces;
using CesarDev.Business.Models;
using CesarDev.Business.Models.Validations;

namespace CesarDev.Business.Services
{
    public abstract class ProdutoService : BaseService, IProdutoService
    {
        public async Task Adicionar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) ;

        }

        public async Task Atualizar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) ;

        }

        public async Task Remover(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) ;

        }
    }
}
