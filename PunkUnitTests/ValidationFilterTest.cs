using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PunkAPI.Controllers;
using PunkModels;
using PunkService.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunkUnitTests
{
    [TestClass]
    public class ValidationFilterTest
    {
        [TestMethod]
        public async Task GetBeersByNameValidDataAsync()
        {
            var logger = new MockLogger<ValidationFilterAttribute>();
            var punkProxyMock = new Mock<IPunkProxy>();
            var attrib = new ValidationFilterAttribute(punkProxyMock.Object, logger);
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext
            {
                HttpContext = httpContext,
                RouteData = new RouteData(),
                ActionDescriptor = new ControllerActionDescriptor() { ActionName = "GetBeersByName" },
            };
            var metadata = new List<IFilterMetadata>();
            var context = new ActionExecutingContext(actionContext, metadata,
                new Dictionary<string, object> { { "name", "test" } }, Mock.Of<Controller>());
            ActionExecutionDelegate next = () =>
            {
                var ctx = new ActionExecutedContext(actionContext, metadata, Mock.Of<Controller>());
                context.Result = new OkResult();
                return Task.FromResult(ctx);
            };

            await attrib.OnActionExecutionAsync(context, next);

            Assert.IsTrue(context.Result is OkResult);
        }
        [TestMethod]
        public async Task GetBeersByNameInValidDataAsync()
        {
            var logger = new MockLogger<ValidationFilterAttribute>();
            var punkProxyMock = new Mock<IPunkProxy>();
            var attrib = new ValidationFilterAttribute(punkProxyMock.Object, logger);
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext
            {
                HttpContext = httpContext,
                RouteData = new RouteData(),
                ActionDescriptor = new ControllerActionDescriptor() { ActionName = "GetBeersByName" },
            };
            var metadata = new List<IFilterMetadata>();
            var context = new ActionExecutingContext(actionContext, metadata,
                new Dictionary<string, object> { { "name", "" } }, Mock.Of<Controller>());
            ActionExecutionDelegate next = () =>
            {
                var ctx = new ActionExecutedContext(actionContext, metadata, Mock.Of<Controller>());
                context.Result = new OkResult();
                return Task.FromResult(ctx);
            };

            await attrib.OnActionExecutionAsync(context, next);

            Assert.IsTrue(context.Result is BadRequestObjectResult);
        }
        [TestMethod]
        public async Task AddUserRatingValidDataAsync()
        {
            var logger = new MockLogger<ValidationFilterAttribute>();
            var punkProxyMock = new Mock<IPunkProxy>();
            punkProxyMock.Setup(x => x.GetBeerById(1)).ReturnsAsync(new Beer() { Id = 1, Name = "test" });
            var attrib = new ValidationFilterAttribute(punkProxyMock.Object, logger);
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext
            {
                HttpContext = httpContext,
                RouteData = new RouteData(),
                ActionDescriptor = new ControllerActionDescriptor() { ActionName = "AddUserRating" },
            };
            var metadata = new List<IFilterMetadata>();
            var context = new ActionExecutingContext(actionContext, metadata,
                new Dictionary<string, object> { { "beerId", 1 }, { "rating", new UserRating() { Rating=1.5f, Username="muk@dad.ddd" } } }, 
                Mock.Of<Controller>());
            ActionExecutionDelegate next = () =>
            {
                var ctx = new ActionExecutedContext(actionContext, metadata, Mock.Of<Controller>());
                context.Result = new OkResult();
                return Task.FromResult(ctx);
            };

            await attrib.OnActionExecutionAsync(context, next);

            Assert.IsTrue(context.Result is OkResult);
        }
        [TestMethod]
        public async Task AddUserRatingInValidDataAsync()
        {
            var logger = new MockLogger<ValidationFilterAttribute>();
            var punkProxyMock = new Mock<IPunkProxy>();
            punkProxyMock.Setup(x => x.GetBeerById(1)).ReturnsAsync(new Beer() { Id = 1, Name = "test" });
            var attrib = new ValidationFilterAttribute(punkProxyMock.Object, logger);
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext
            {
                HttpContext = httpContext,
                RouteData = new RouteData(),
                ActionDescriptor = new ControllerActionDescriptor() { ActionName = "AddUserRating" },
            };
            var metadata = new List<IFilterMetadata>();
            var context = new ActionExecutingContext(actionContext, metadata,
                new Dictionary<string, object> { { "beerId", 1 }, { "rating", new UserRating() { Rating = 5.5f, Username = "muk@dad" } } }, 
                Mock.Of<Controller>());
            ActionExecutionDelegate next = () =>
            {
                var ctx = new ActionExecutedContext(actionContext, metadata, Mock.Of<Controller>());
                context.Result = new OkResult();
                return Task.FromResult(ctx);
            };

            await attrib.OnActionExecutionAsync(context, next);

            Assert.IsTrue(context.Result is BadRequestObjectResult);
        }
    }
}
