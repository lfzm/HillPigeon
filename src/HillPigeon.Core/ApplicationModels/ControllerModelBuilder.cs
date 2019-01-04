using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HillPigeon.ApplicationModels
{
    internal class ControllerModelBuilder
    {
        private readonly ActionModelBuilder _actionModelBuilder;
        private readonly IControllerModelConvention[] _controllerModelConventions;
        public ControllerModelBuilder(ActionModelBuilder actionModelBuilder, IEnumerable<IControllerModelConvention> controllerModelConventions)
        {
            _actionModelBuilder = actionModelBuilder;
            _controllerModelConventions = controllerModelConventions.ToArray();
        }
        public ControllerModel Build(TypeInfo typeInfo, string moduleName)
        {
            ControllerModel controller = new ControllerModel( typeInfo);
            this.WithAttributes(controller, typeInfo);
            this.WithActionModel(controller, typeInfo);
            this.WithConvention(controller);
            return controller;
        }
        private void WithActionModel(ControllerModel controller, TypeInfo typeInfo)
        {
            var methods = typeInfo.DeclaredMethods;
            foreach (var method in methods)
            {
                if (!IsAction(method))
                    continue;
                var actionModel = _actionModelBuilder.Build(controller, method);
                controller.Actions.Add(actionModel);
            }
        }
        private void WithAttributes(ControllerModel controller, TypeInfo typeInfo)
        {
            var attributes = typeInfo.GetCustomAttributes(inherit: true);
            foreach (var attribute in attributes)
            {
                controller.Attributes.Add((Attribute)attribute);
            }
        }
        private void WithConvention(ControllerModel controller)
        {
            foreach (var item in _controllerModelConventions)
            {
                item.Apply(controller);
            }
        }

        private bool IsAction(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                throw new ArgumentNullException(nameof(methodInfo));
            }

            // The SpecialName bit is set to flag members that are treated in a special way by some compilers
            // (such as property accessors and operator overloading methods).
            if (methodInfo.IsSpecialName)
            {
                return false;
            }

            if (methodInfo.IsDefined(typeof(NonActionAttribute)))
            {
                return false;
            }

            // Overridden methods from Object class, e.g. Equals(Object), GetHashCode(), etc., are not valid.
            if (methodInfo.GetBaseDefinition().DeclaringType == typeof(object))
            {
                return false;
            }

            // Dispose method implemented from IDisposable is not valid
            if (IsIDisposableMethod(methodInfo))
            {
                return false;
            }

            if (methodInfo.IsStatic)
            {
                return false;
            }

            if (methodInfo.IsAbstract)
            {
                return false;
            }

            if (methodInfo.IsConstructor)
            {
                return false;
            }

            if (methodInfo.IsGenericMethod)
            {
                return false;
            }

            return methodInfo.IsPublic;
        }
        private bool IsIDisposableMethod(MethodInfo methodInfo)
        {
            // Ideally we do not want Dispose method to be exposed as an action. However there are some scenarios where a user
            // might want to expose a method with name "Dispose" (even though they might not be really disposing resources)
            // Example: A controller deriving from MVC's Controller type might wish to have a method with name Dispose,
            // in which case they can use the "new" keyword to hide the base controller's declaration.

            // Find where the method was originally declared
            var baseMethodInfo = methodInfo.GetBaseDefinition();
            var declaringTypeInfo = baseMethodInfo.DeclaringType.GetTypeInfo();

            return
                (typeof(IDisposable).GetTypeInfo().IsAssignableFrom(declaringTypeInfo) &&
                 declaringTypeInfo.GetRuntimeInterfaceMap(typeof(IDisposable)).TargetMethods[0] == baseMethodInfo);
        }

    }
}
