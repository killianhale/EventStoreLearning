using System;
using EventStoreLearning.Common.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace EventStoreLearning.Common.Web
{
    public static class QueryApiConvention
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status504GatewayTimeout)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Convention Definition")]
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        public static void Get()
        {
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status504GatewayTimeout)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Convention Definition")]
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
        public static void Get([ApiConventionNameMatch(ApiConventionNameMatchBehavior.Suffix)] Guid id)
        {
        }
    }
}
