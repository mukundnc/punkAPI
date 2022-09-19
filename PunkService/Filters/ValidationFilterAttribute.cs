using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PunkModels;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PunkService.Filters
{
    public class ValidationFilterAttribute : IAsyncActionFilter
    {
        private const string action1 = "GetBeersByName";
        private const string action2 = "AddUserRating";
        private Regex emailRegex = new Regex(Constants.EmailRegex);
        private readonly IPunkProxy _punkProxy;
        private readonly ILogger<ValidationFilterAttribute> _logger;

        public ValidationFilterAttribute(IPunkProxy punkProxy, ILogger<ValidationFilterAttribute> logger)
        {
            _punkProxy = punkProxy;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            (bool, string[]) validateObj = (false, null);
            try
            {
                var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
                if (actionDescriptor.ActionName == action1)
                {
                    context.ActionArguments.TryGetValue(Constants.ActionAttributeName, out var name);
                    validateObj = ValidateAction1(name as string);
                }
                else if (actionDescriptor.ActionName == action2)
                {
                    context.ActionArguments.TryGetValue(Constants.ActionAttributeBeerId, out var beerId);
                    context.ActionArguments.TryGetValue(Constants.ActionAttributeUserRating, out var rating);
                    validateObj = await ValidateAction2Async((int)beerId, rating as UserRating);
                }
                if (validateObj.Item1)
                {
                    context.Result = new BadRequestObjectResult(validateObj.Item2);
                    return;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in action filter {ex}");
                context.Result = new StatusCodeResult(500);
                return;
            }
            await next();
        }

        private (bool, string[]) ValidateAction1(string name)
        {
            return (string.IsNullOrWhiteSpace(name as string), new string[] { Constants.NameError });
        }

        private async Task<(bool, string[])> ValidateAction2Async(int beerId, UserRating rating)
        {
            var flag = false;
            var errors = new List<string>();
            var beer = await _punkProxy.GetBeerById(beerId);
            if (beer is null || beer.Id <= 0)
            {
                flag = true;
                errors.Add(Constants.InvalidIdError);
                return (flag, errors.ToArray());
            }
            if (rating is null)
            {
                flag = true;
                errors.Add(Constants.RrError);
                return (flag, errors.ToArray());
            }
            if (rating.Rating < 1 || rating.Rating > 5)
            {
                flag = true;
                errors.Add(Constants.RatingError);
            }
            if (!emailRegex.IsMatch(rating.Username))
            {
                flag = true;
                errors.Add(Constants.UserError);
            }
            return (flag, errors.ToArray());
        }

    }
}
