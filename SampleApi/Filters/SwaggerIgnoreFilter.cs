using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using SampleApi.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SampleApi.Filters
{
    public class SwaggerIgnoreFilter : ISchemaFilter, IDocumentFilter
    {
        private static List<string> ExcludedKeys = new();

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.GetCustomAttribute<SwaggerIgnoreAttribute>() != null)
            {
                ExcludedKeys.Add(context.Type.FullName);
                return;
            }

            if (schema.Properties != null)
            {
                var excludedProperties = context.Type.GetProperties().Where(t => t.GetCustomAttribute<SwaggerIgnoreAttribute>() != null);

                foreach (var excludedProperty in excludedProperties)
                {
                    var propertyToRemove = schema.Properties.Keys.SingleOrDefault(x => string.Equals(x, excludedProperty.Name, StringComparison.OrdinalIgnoreCase));

                    if (propertyToRemove != null)
                    {
                        schema.Properties.Remove(propertyToRemove);
                    }
                }
            }

            if (context.Type.IsEnum || (Nullable.GetUnderlyingType(context.Type)?.IsEnum ?? false))
            {
                var type = (context.Type.IsEnum)
                    ? context.Type
                    : Nullable.GetUnderlyingType(context.Type);

                var enums = new List<IOpenApiAny>();

                foreach (var name in Enum.GetNames(type))
                {
                    var value = type.GetMember(name)[0];
                    if (!value.GetCustomAttributes<SwaggerIgnoreAttribute>().Any())
                    {
                        enums.Add(new OpenApiString(name));
                    }
                }

                schema.Enum = enums;
            }
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var key in swaggerDoc.Components.Schemas.Keys)
            {
                if (ExcludedKeys.Any(x => x.EndsWith(key)))
                {
                    swaggerDoc.Components.Schemas.Remove(key);
                }
            }
        }
    }
}