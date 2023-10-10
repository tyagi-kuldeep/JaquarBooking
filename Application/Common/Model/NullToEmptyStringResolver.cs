using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Application.Common.Model
{
    public class NullToEmptyStringResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return type.GetProperties()
                    .Select(p => {
                        var jp = base.CreateProperty(p, memberSerialization);
                        jp.ValueProvider = new NullToEmptyStringValueProvider(p);
                        return jp;
                    }).ToList();
        }
    }

    public class NullToEmptyStringValueProvider : IValueProvider
    {
        PropertyInfo _MemberInfo;
        public NullToEmptyStringValueProvider(PropertyInfo memberInfo)
        {
            _MemberInfo = memberInfo;
        }

        public object GetValue(object target)
        {
            object result = _MemberInfo.GetValue(target);
            if (_MemberInfo.PropertyType == typeof(string) && result == null) result = "";
            else if (_MemberInfo.PropertyType == typeof(int) && result == null) result = 0;
            else if (_MemberInfo.PropertyType == typeof(decimal) && result == null) result = 0;
            else if (_MemberInfo.PropertyType == typeof(Boolean) && result == null) result = false;
            else if (_MemberInfo.PropertyType == typeof(double) && result == null) result = 0;
            return result;

        }

        public void SetValue(object target, object value)
        {
            _MemberInfo.SetValue(target, value);
        }
    }
}
