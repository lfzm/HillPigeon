using HillPigeon.ApplicationModels;
using HillPigeon.Orleans.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HillPigeon.Orleans.Core
{
    public class OrleansActionModelConvention : IActionModelConvention
    {
        private readonly OrleansRouteingOptions _options;
        private readonly OrleansActionILGeneratFactory _actionILGeneratFactory;
        public OrleansActionModelConvention(IOptions<OrleansRouteingOptions> options, OrleansActionILGeneratFactory actionILGeneratFactory)
        {
            _options = options.Value;
            _actionILGeneratFactory = actionILGeneratFactory;
        }
        public void Apply(ActionModel actionModel)
        {
            if (!actionModel.Controller.ControllerType.IsGrain())
            {
                return;
            }
            actionModel.GeneratActionIL = _actionILGeneratFactory.GeneratActionIL;
            this.SetGrainParameter(actionModel);//设置IGrainFactory：keyExtension,grainClassNamePrefix
            this.SetHttpMethod(actionModel);
            this.SetRoute(actionModel);// 定义PrimaryKey 路由
        }

        private void SetHttpMethod(ActionModel actionModel)
        {
            if(!actionModel.IsNeedSetHttpMethod())
            {
                return;
            }
            actionModel.Attributes.Add(new HttpPostAttribute());
            if (actionModel.Parameters.Count(f => f.Attributes.Where(attr => attr is FromBodyAttribute || attr is FromFormAttribute).Count() > 0) == 0)
            {
                actionModel.Attributes.Add(new HttpGetAttribute());
            }
        }
        private void SetRoute(ActionModel actionModel)
        {
            //判断是否需要设置路由
            if (!actionModel.IsNeedSetRoute())
            {
                return;
            }
            // 已经标注了NotPrimaryKeyAttribute
            if (actionModel.MethodInfo.GetCustomAttribute<NotPrimaryKeyAttribute>() != null)
            {
                return;
            }

            var controllerType = actionModel.Controller.ControllerType;
            string primaryKeyName = "__primaryKey";

            (Type type, object defaultValue) grainPrimaryKey = _actionILGeneratFactory.GrainInterfaceToKeyType(controllerType);
            if (grainPrimaryKey.type == typeof(Guid))
            {
                Attribute routeAttr = new RouteAttribute(actionModel.ActionName);
                actionModel.Attributes.Add(routeAttr);
                return;
            }
            else
            {
                Attribute routeAttr = new RouteAttribute(actionModel.ActionName + "/{" + primaryKeyName + "}");
                actionModel.Attributes.Add(routeAttr);
            }

            var keyParameter = this.BuildParameterModel(actionModel, primaryKeyName, typeof(string), new List<Attribute> { new FromRouteAttribute() });
            keyParameter.HasDefaultValue = true;
            keyParameter.DefaultValue = grainPrimaryKey.defaultValue;
            actionModel.Parameters.Add(keyParameter);
        }
        private void SetGrainParameter(ActionModel actionModel)
        {
            var controllerType = actionModel.Controller.ControllerType;

            IList<Attribute> attributes = new List<Attribute>
            {
                new FromQueryAttribute(),
                new FromHeaderAttribute()
            };
            //添加 __grainClassNamePrefix 参数
            var grainClassParam = this.BuildParameterModel(actionModel, "__grainClassNamePrefix", typeof(string), attributes);
            grainClassParam.HasDefaultValue = true;
            actionModel.Parameters.Add(grainClassParam);

            //添加__keyExtension 参数
            if (typeof(IGrainWithGuidCompoundKey).IsAssignableFrom(controllerType) ||
                typeof(IGrainWithIntegerCompoundKey).IsAssignableFrom(controllerType))
            {

                var keyExtensionParam = this.BuildParameterModel(actionModel, "__keyExtension", typeof(string), attributes);
                keyExtensionParam.HasDefaultValue = true;
                actionModel.Parameters.Add(keyExtensionParam);
            }
        }

        private ParameterModel BuildParameterModel(ActionModel actionModel, string name, Type type, IList<Attribute> attributes)
        {
            var param = new ParameterModel()
            {
                ActionModel = actionModel,
                ParameterAttributes = ParameterAttributes.None,
                ParameterName = name,
                ParameterType = type,
                Position = actionModel.Parameters.Max(f => f.Position) + 1,
            };
            if (attributes != null)
            {
                foreach (var item in attributes)
                {
                    param.Attributes.Add(item);
                }
            }
            return param;
        }
   
    }
}
