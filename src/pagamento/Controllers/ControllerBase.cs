using Microsoft.AspNetCore.Mvc;
using QuickOrderPagamento.Core.Application.Dtos;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace QuickOrderPagamento.Adapters.Driving.Api.Controllers
{
    [ExcludeFromCodeCoverage]
    public abstract class CustomController<T> : ControllerBase
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        protected readonly ILogger<T> _logger;

        protected CustomController(ILogger<T> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gera resultado sem resposta em ServiceResult baseado no AJAX Security do OWASP.
        /// <para>OWASP: https://cheatsheetseries.owasp.org/cheatsheets/AJAX_Security_Cheat_Sheet.html</para>
        /// </summary>
        /// <param name="serviceResult"></param>
        /// <returns>ObjectResult</returns>
        protected IActionResult Result(ServiceResult serviceResult)
        {
            try
            {
                var objectResult = new ObjectResult(serviceResult)
                {
                    StatusCode = serviceResult.CodeId
                };
                return objectResult;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
