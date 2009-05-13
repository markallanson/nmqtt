using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace nMqttTests
{
    public class ReflectionBindingConstants
    {
        public const BindingFlags NonpublicMethod = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod;
        public const BindingFlags NonPublicGetter = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty;
        public const BindingFlags NonPublicSetter = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty;
        public const BindingFlags NonPublisStaticMethod = BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Static;
        public const BindingFlags PublicConstructor = BindingFlags.Public | BindingFlags.CreateInstance;
        public const BindingFlags PublicMethod = BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod;
    }
}
