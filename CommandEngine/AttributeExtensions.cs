using System;

namespace CommandEngine
{
    static class AttributeExtensions
    {
        /// <summary>
        /// Gets the first Attribute of given type in the given property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static ParametersDefinition GetProperty<ParametersDefinition>(this System.Reflection.PropertyInfo propertyInfo)
            where ParametersDefinition : Attribute
        {
            try
            {
                var def = (ParametersDefinition)propertyInfo.GetCustomAttributes(typeof(ParametersDefinition), false)[0];
                return def;
            }
            catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException($"Attribute {nameof(ParametersDefinition)} not found on property {propertyInfo.Name} on object {propertyInfo.DeclaringType.Name}");
            }
        }
    }
}
