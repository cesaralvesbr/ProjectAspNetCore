using CesarDev.App.Extensions;
using CesarDev.Business.Interfaces;
using CesarDev.Business.Notificacoes;
using CesarDev.Business.Services;
using CesarDev.Data.Context;
using CesarDev.Data.Repository;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace CesarDev.App.Configurarions
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencias(this IServiceCollection services)
        {
            services.AddScoped<DevDbContext>();           
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IEnderecoRepositoy, EnderecoRepository>();
            services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdapterProvider>();

            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IFornecedorService, FornecedorService>();
            services.AddScoped<IProdutoService, ProdutoService>();


            return services;
        }
    }
}
