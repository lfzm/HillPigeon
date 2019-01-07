using Microsoft.AspNetCore.Mvc.Routing;
using System.Linq;

namespace HillPigeon.Orleans.Core
{
    public static class ActionModelExtensions
    {
        /// <summary>
        /// 是否需要设置默认路由
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool IsNeedSetRoute(this ActionModel model)
        {
            return model.Attributes.Where(attr =>
            {
                if (typeof(IRouteTemplateProvider).IsAssignableFrom(attr.GetType()))
                {
                    var routeTemplate = (IRouteTemplateProvider)attr;
                    return string.IsNullOrEmpty(routeTemplate.Template);
                }
                else
                    return false;
            }).Count() == 0;
        }

        /// <summary>
        /// 是否需要设置默认Http Method
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool IsNeedSetHttpMethod(this ActionModel model)
        {
            return model.Attributes.Where(attr => typeof(IActionHttpMethodProvider).IsAssignableFrom(attr.GetType())).Count() == 0;
        }
    }
}