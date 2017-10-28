using System;

namespace LiteApi.OpenApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class OpenApiOperationAttribute: Attribute
    {
        public string Id { get; }
        public int[] PossibleStatusCodes { get; }

        public OpenApiOperationAttribute(string id, params int[] possibleStatusCodes)
        {
            PossibleStatusCodes = possibleStatusCodes;
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }
    }
}
