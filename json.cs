using System;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace org.flame.SimpleJson {

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

    public static class SkandragonSimpleJson {
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

        private static bool IsGenericType(Type i, Type target)
        {
            return i.IsGenericType && i.GetGenericTypeDefinition() == target;
        }

        private static bool TestIfGeneric(Type type, Type target)
        {
            if (IsGenericType(type, target))
            {
                return true;
            }
            foreach (var i in type.GetInterfaces())
            {
                if (IsGenericType(i, target)) {
                    return true;
                }
            }
            return false;
        }

        private static bool TestIfIEnumerable(Type i) {
            return TestIfGeneric(i, typeof(IEnumerable<>));
        }

        public static string ToJson(Object o)
        {
            var sb = new StringBuilder();
            ToJson(sb, o);
            return sb.ToString();
        }

        public static void ToJson(StringBuilder sb, object o)
        {
            if (o == null)
            {
                sb.Append("null");
                return;
            }

            var ty = o.GetType();

            if (ty == typeof(int) || ty == typeof(uint)
                || ty == typeof(long) || ty == typeof(ulong)
                || ty == typeof(float) || ty == typeof(double) || ty == typeof(decimal)
                || ty == typeof(short) || ty == typeof(ushort)
                || ty == typeof(byte) || ty == typeof(sbyte))
            {
                sb.Append(o.ToString());
                return;
            }
            else if (ty == typeof(string))
            {
                sb.Append(EncodeString((string)o));
                return;
            }
            else if (ty == typeof(char))
            {
                sb.Append(EncodeChar((char)o));
                return;
            }
            else if (ty == typeof(bool))
            {
                sb.Append((bool)o ? "true" : "false");
                return;
            }
            else if (TestIfIEnumerable(ty))
            {
                EncodeList(sb, ty, o);
                return;
            }

            // try as a plain object and hope for the best...
            EncodeObject(sb, o);
        }

        private static void EncodeList(StringBuilder sb, Type ty, object o)
        {
            IEnumerable enumerable = o as IEnumerable;

            sb.Append("[");
            var first = true;
            foreach (var obj in enumerable)
            {
                if (!first)
                {
                    sb.Append(",");
                }
                first = false;
                ToJson(sb, obj);
            }
            sb.Append("]");
        }

        private static void EncodeObject(StringBuilder sb, object o)
        {
            sb.Append("{");
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
                ToJson(sb, prop.GetValue(o, null));
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
                ToJson(sb, field.GetValue(o));
            }

            sb.Append("}");
        }
    }
}