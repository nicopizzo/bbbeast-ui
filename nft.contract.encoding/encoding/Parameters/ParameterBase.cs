using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.Model;

namespace NFT.Contract.Encoding.Parameters
{
    public abstract class ParameterBase
    {
        public virtual Parameter[] GetParameters()
        {
            var parameters = from p in GetType().GetProperties()
                             let attr = p.GetCustomAttributes(typeof(ParameterAttribute), true)
                             where attr.Length == 1
                             let a = attr.First() as ParameterAttribute
                             select new Parameter(a.Type, a.Name, a.Order);
            return parameters.ToArray();
        }

        public virtual object[] GetParameterValues()
        {
            var props = GetType().GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(ParameterAttribute)));

            var parameterValues = new object[props.Count()];

            foreach(var p in props)
            {
                var attr = p.GetCustomAttributes(typeof(ParameterAttribute), true)?.Cast<ParameterAttribute>().FirstOrDefault();
                if(attr != null)
                {
                    var value = p.GetValue(this);
                    parameterValues[attr.Order - 1] = value;
                }
            }

            return parameterValues;
        }
    }
}
