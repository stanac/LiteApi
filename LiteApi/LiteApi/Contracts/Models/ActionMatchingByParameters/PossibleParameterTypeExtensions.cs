using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteApi.Contracts.Models.ActionMatchingByParameters
{
    /// <summary>
    /// Extension used for possible parameter type determination
    /// </summary>
    public static class PossibleParameterTypeExtensions
    {
        /// <summary>
        /// Gets the possible parameter types.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Possible parameter types</returns>
        public static IEnumerable<PossibleParameterType> GetPossibleParameterTypes(this HttpRequest request)
        {
            if (request.HasBody())
            {
                yield return new PossibleParameterType
                {
                    Source = ParameterSources.Body,
                    HasValue = true,
                    OrderId = 0
                };
            }
            int queryOrder = 0;
            foreach (var param in request.Query)
            {
                var possibleType = new PossibleParameterType
                {
                    Source = ParameterSources.Query,
                    Name = param.Key,
                    OrderId = queryOrder,
                    QueryValues = param.Value
                };

                if (possibleType.QueryValues.Any())
                {
                    string first = possibleType.QueryValues.FirstOrDefault();
                    if (!string.IsNullOrEmpty(first))
                    {
                        possibleType.PossibleTypes = GetPossibleTypes(first).Select(x => new TypeWithPriority(x)).ToArray();
                    }
                }

                yield return possibleType;
            }
        }

        /// <summary>
        /// Gets the possible types.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Possible types</returns>
        private static IEnumerable<Type> GetPossibleTypes(string value)
        {
            bool tempBool;
            Int16 tempInt16;
            Int32 tempInt32;
            Int64 tempInt64;
            UInt16 tempUInt16;
            UInt32 tempUInt32;
            UInt64 tempUInt64;
            Byte tempByte;
            SByte tempSByte;
            decimal tempDecimal;
            float tempFloat;
            double tempDouble;
            Guid tempGuid;
            DateTime tempDateTime;

            yield return typeof(string);
            if (value.Length == 1) yield return typeof(char);

            if (bool.TryParse(value, out tempBool))
            {
                yield return typeof(bool);
            }
            else
            {
                if (float.TryParse(value, out tempFloat)) yield return typeof(float);
                if (decimal.TryParse(value, out tempDecimal)) yield return typeof(decimal);
                if (double.TryParse(value, out tempDouble)) yield return typeof(double);

                if (UInt16.TryParse(value, out tempUInt16))
                {
                    yield return typeof(UInt16);
                    yield return typeof(UInt32);
                    yield return typeof(UInt64);
                }
                else if (UInt32.TryParse(value, out tempUInt32))
                {
                    yield return typeof(UInt32);
                    yield return typeof(UInt64);
                }
                else if (UInt64.TryParse(value, out tempUInt64))
                {
                    yield return typeof(UInt64);
                }

                if (Int16.TryParse(value, out tempInt16))
                {
                    yield return typeof(Int16);
                    yield return typeof(Int32);
                    yield return typeof(Int64);
                }
                else if (Int32.TryParse(value, out tempInt32))
                {
                    yield return typeof(Int32);
                    yield return typeof(Int64);
                }
                else if (Int64.TryParse(value, out tempInt64))
                {
                    yield return typeof(Int64);
                }

                if (Byte.TryParse(value, out tempByte))
                {
                    yield return typeof(byte);
                }
                if (SByte.TryParse(value, out tempSByte))
                {
                    yield return typeof(sbyte);
                }
            }

            if (DateTime.TryParse(value, out tempDateTime))
            {
                yield return typeof(DateTime);
            }
            else if (Guid.TryParse(value, out tempGuid))
            {
                yield return typeof(Guid);
            }
        }

        /// <summary>
        /// Determines whether the specified possible parameter is matched by name.
        /// </summary>
        /// <param name="actionParam">The action parameter.</param>
        /// <param name="possibleParam">The possible parameter.</param>
        /// <returns>True is match is found</returns>
        public static bool IsMatchedByName(this ActionParameter actionParam, PossibleParameterType possibleParam)
            => actionParam.Name == possibleParam.Name;

        /// <summary>
        /// Determines whether the specified possible parameter is matched by name.
        /// </summary>
        /// <param name="possibleParam">The possible parameter.</param>
        /// <param name="actionParam">The action parameter.</param>
        /// <returns>True is match is found</returns>
        public static bool IsMatchedByName(this PossibleParameterType possibleParam, ActionParameter actionParam)
            => actionParam.Name == possibleParam.Name;
    }
}
