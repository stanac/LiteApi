﻿using System;
using System.Collections.Generic;
using System.Linq;
using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;

namespace LiteApi.Services.Validators
{
    /// <summary>
    /// Class that validates controllers before first request is received.
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IControllersValidator" />
    public class ControllersValidator : IControllersValidator
    {
        private readonly IActionsValidator _actionValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllersValidator"/> class.
        /// </summary>
        /// <param name="actionValidator">The action validator.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ControllersValidator(IActionsValidator actionValidator)
        {
            if (actionValidator == null) throw new ArgumentNullException(nameof(actionValidator));
            _actionValidator = actionValidator;
        }

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <param name="controllerCtxs">The controller context.</param>
        /// <returns>Collection of strings that contains errors, if not empty an exception should be raised.</returns>
        public IEnumerable<string> GetValidationErrors(ControllerContext[] controllerCtxs)
        {
            foreach (ControllerContext ctrl in controllerCtxs)
            {
                if (controllerCtxs.Count(x => x.RouteAndName == ctrl.RouteAndName) > 1)
                {
                    yield return $"There are more than one controller with matching name: {ctrl.RouteAndName}";
                }
                foreach (string error in _actionValidator.GetValidationErrors(ctrl.Actions))
                {
                    yield return error;
                }
            }
        }
    }
}