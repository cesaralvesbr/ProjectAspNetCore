﻿using CesarDev.Business.Models;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CesarDev.Business.Services
{
    public abstract class BaseService
    {
        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }

        protected void Notificar(string mensagem)
        {
        }

        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entidade
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            Notificar(validator);
            return false;
        }
    }
}
