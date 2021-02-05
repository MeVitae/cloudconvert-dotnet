using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace cloudconvert_dotnet.Helpers
{
    public class StaticMethods
    {
        // https://stackoverflow.com/questions/11454004/calculate-a-md5-hash-from-a-string
        // By Anant Dabhi
        public static string CreateMD5(byte[] fileBytes)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(fileBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        // https://stackoverflow.com/questions/54774643/get-json-propertyname-from-c-sharp-class-like-nameofclass-prop-for-json-pro
        // By Vlad, elgonzo
        public static string GetJsonPropertyName<TC>(Expression<Func<TC, object>> expr)
        {
            // in case the property type is a value type, the expression contains
            // an outer Convert, so we need to remove it
            var body = (expr.Body is UnaryExpression unary) ? unary.Operand : expr.Body;

            if (body is System.Linq.Expressions.MemberExpression memberEx)
                return memberEx.Member.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName;
            else
                throw new ArgumentException("expect field access lambda");
        }

        public static void PopulateBaseProperties<T, TB>(T Target, TB BaseObject) where T : TB
        {
            Type t = typeof(T);
            Type tb = typeof(TB);

            PropertyInfo[] baseProperties = tb.GetProperties();
            foreach (var baseProperty in baseProperties)
            {
                if (baseProperty.CanRead && baseProperty.CanWrite)
                {
                    var baseValue = baseProperty.GetValue(BaseObject, null);
                    var targetProperty = t.GetProperty(baseProperty.Name);
                    targetProperty.SetValue(Target, baseValue, null);
                }
            }
        }
    }
}
