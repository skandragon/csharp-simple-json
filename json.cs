using System;
using System.Text;
using System.Reflection;

namespace org.flame.SimpleJson {
    public class MainClass {
            public static void Main(string[] args) {
                var x = new Example();
                x.Property1 = "string1";
                x.field1 = "field1string";
                x.Address = new PostalAddressExample("main st.", 12345);

                Console.WriteLine(SkandragonSimpleJson.ToJson(x));
                Console.WriteLine(SkandragonSimpleJson.ToJson(123));
                Console.WriteLine(SkandragonSimpleJson.ToJson(123.456));
                Console.WriteLine(SkandragonSimpleJson.ToJson("string"));
                Console.WriteLine(SkandragonSimpleJson.ToJson("string\"\\"));
                Console.WriteLine(SkandragonSimpleJson.ToJson("string\u0012"));
                Console.WriteLine(SkandragonSimpleJson.ToJson(true));
                Console.WriteLine(SkandragonSimpleJson.ToJson(false));
                Console.WriteLine(SkandragonSimpleJson.ToJson(null));
                Console.WriteLine(SkandragonSimpleJson.ToJson('c'));
                Console.WriteLine(SkandragonSimpleJson.ToJson('\\'));
                Console.WriteLine(SkandragonSimpleJson.ToJson('"'));
                Console.WriteLine(SkandragonSimpleJson.ToJson('\u0012'));
            }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SkandragonJsonName: Attribute
    {
        public string Value {get; }

        public SkandragonJsonName(string s) {
            this.Value = s;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SkandragonJsonIgnore: Attribute
    {
        public SkandragonJsonIgnore() {}
    }

    public class PostalAddressExample
    {
        public string Street {get; set;}
        public int ZipCode {get; set;}

        public PostalAddressExample(string street, int zipcode) {
            this.Street = street;
            this.ZipCode = zipcode;
        }
    }

    public class Example
    {
        [SkandragonJsonName("prop1")]
        public string Property1 {get; set; }

        public int IntField {get; set; }

        public float FloatField {get; set;}

        [SkandragonJsonIgnore]
        public int IgnoredField {get; set;}

        private int PrivateIntField {get; set;}

        public PostalAddressExample Address {get; set;}

        public string field1;
    }

    public class SkandragonSimpleJson {
        private static string GetJsonName(PropertyInfo prop)
        {
            var a = (SkandragonJsonName)Attribute.GetCustomAttribute(prop, typeof(SkandragonJsonName));
            if (a == null)
            {
                return prop.Name;
            }
            return a.Value;
        }

        private static string GetJsonName(FieldInfo f)
        {
            var a = (SkandragonJsonName)Attribute.GetCustomAttribute(f, typeof(SkandragonJsonName));
            if (a == null)
            {
                return f.Name;
            }
            return a.Value;
        }

        private static bool ShouldSkip(PropertyInfo prop)
        {
            var a = (SkandragonJsonIgnore)Attribute.GetCustomAttribute(prop, typeof(SkandragonJsonIgnore));
            return a != null;
        }

        private static bool ShouldSkip(FieldInfo f)
        {
            var a = (SkandragonJsonIgnore)Attribute.GetCustomAttribute(f, typeof(SkandragonJsonIgnore));
            return a != null;
        }

        private static string EncodeString(string o)
        {
            var sb = new StringBuilder("\"", o.Length + 2);
            foreach (char c in o)
            {
                sb.Append(RawEncodeChar(c));
            }
            sb.Append("\"");
            return sb.ToString();
        }

        private static string RawEncodeChar(char c)
        {
            if (c == '\\')
            {
                return "\\\\";
            }
            else if (c == '"')
            {
                return "\\\"";
            }
            else if (c < 0x001f)
            {
                uint x = c;
                return "\\u" + x.ToString("x04"); // TODO: handle > 2 byte characters
            }
            return c.ToString();
        }

        private static string EncodeChar(char c)
        {
            return "\"" + RawEncodeChar(c) + "\"";
        }

        public static string ToJson(object o)
        {
            if (o == null)
            {
                return "null";
            }

            var ty = o.GetType();

            if (ty == typeof(int) || ty == typeof(uint)
                || ty == typeof(long) || ty == typeof(ulong)
                || ty == typeof(float) || ty == typeof(double) || ty == typeof(decimal)
                || ty == typeof(short) || ty == typeof(ushort)
                || ty == typeof(byte) || ty == typeof(sbyte))
            {
                return o.ToString();
            }
            else if (ty == typeof(string))
            {
                return EncodeString((string)o);
            }
            else if (ty == typeof(char))
            {
                return EncodeChar((char)o);
            }
            else if (ty == typeof(bool))
            {
                return (bool)o ? "true" : "false";
            }

            var sb = new StringBuilder("{");
            var first = true;

            var props = o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                if (ShouldSkip(prop))
                {
                    continue;
                }
                if (!first)
                {
                    sb.Append(",");
                }
                first = false;
                sb.Append(EncodeString(GetJsonName(prop)) + ":");
                sb.Append(ToJson(prop.GetValue(o, null)));
            }

            var fields = o.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                if (ShouldSkip(field))
                {
                    continue;
                }
                if (!first)
                {
                    sb.Append(",");
                }
                first = false;
                sb.Append(EncodeString(GetJsonName(field)) + ":");
                sb.Append(ToJson(field.GetValue(o)));
            }

            sb.Append("}");
            return sb.ToString();
        }
    }
}