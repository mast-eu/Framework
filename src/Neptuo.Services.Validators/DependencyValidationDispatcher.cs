﻿using Neptuo.Activators;
using Neptuo.Linq.Expressions;
using Neptuo.Models;
using Neptuo.Services.Validators.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Services.Validators
{
    /// <summary>
    /// Base implementation of <see cref="IValidationDispatcher"/> using <see cref="IDependencyProvider"/>.
    /// Before and after validation also uses and sets <see cref="IValidatableModel"/>.
    /// </summary>
    public class DependencyValidationDispatcher : IValidationDispatcher
    {
        /// <summary>
        /// Name of the <see cref="Handlers.IValidationHandler{T}.HandleAsync"/>.
        /// </summary>
        /// <remarks>
        /// Because of SharpKit, this can't be defined by <see cref="TypeHelper"/>.
        /// </remarks>
        private static readonly string validateMethodName = "ValidateAsync"; //TypeHelper.MethodName<IValidator<object>, object, IValidationResult>(v => v.Validate)

        /// <summary>
        /// Inner provider of validation handlers.
        /// </summary>
        private readonly IDependencyProvider dependencyProvider;

        /// <summary>
        /// Creates new instance using <paramref name="dependencyProvider"/> for resolving validation handlers.
        /// </summary>
        /// <param name="dependencyProvider">Resolver of validation handlers.</param>
        public DependencyValidationDispatcher(IDependencyProvider dependencyProvider)
        {
            Ensure.NotNull(dependencyProvider, "dependencyProvider");
            this.dependencyProvider = dependencyProvider;
        }

        public async Task<IValidationResult> ValidateAsync<TModel>(TModel model)
        {
            IValidatableModel validatable = model as IValidatableModel;
            if (validatable != null)
            {
                if (validatable.IsValid != null)
                    return new ValidationResult(validatable.IsValid.Value);
            }

            IValidationHandler<TModel> validator = dependencyProvider.Resolve<IValidationHandler<TModel>>();
            IValidationResult result = await validator.HandleAsync(model);

            if (validatable != null)
                validatable.IsValid = result.IsValid;

            return result;
        }

        public Task<IValidationResult> ValidateAsync(object model)
        {
            Ensure.NotNull(model, "model");
            Type modelType = model.GetType();
            Type validatorType = typeof(IValidationHandler<>).MakeGenericType(modelType);
            MethodInfo validateMethod = validatorType.GetMethod(validateMethodName);
            
            object validator = dependencyProvider.Resolve(validatorType);
            object validationResult = validateMethod.Invoke(validator, new object[] { model });
            return (Task<IValidationResult>)validationResult;
        }
    }
}
