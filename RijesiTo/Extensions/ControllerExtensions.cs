using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace RijesiTo.Extensions
{
    public static class ControllerExtensions
    {
        public static async Task<string> RenderViewAsync<TModel>(this Controller controller, string viewName, TModel model, bool partial = false)
        {
            controller.ViewData.Model = model;
            using var writer = new StringWriter();
            var viewResult = controller.HttpContext.RequestServices
                .GetService(typeof(IRazorViewEngine)) as IRazorViewEngine;

            var viewEngineResult = viewResult.FindView(controller.ControllerContext, viewName, !partial);

            if (!viewEngineResult.Success)
                throw new InvalidOperationException($"Couldn't find view '{viewName}'");

            var viewContext = new ViewContext(
                controller.ControllerContext,
                viewEngineResult.View,
                controller.ViewData,
                controller.TempData,
                writer,
                new HtmlHelperOptions()
            );

            await viewEngineResult.View.RenderAsync(viewContext);
            return writer.GetStringBuilder().ToString();
        }
    }
}
