using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnityEngine;

namespace DebugMenu.Scripts.Utils;

public static partial class Helpers
{
	public static string GetDefaultValue(Type classType)
	{
		if (classType.IsAbstract || classType.IsPointer)
			return null;
            
		if (classType.IsPrimitive)
		{
			if (classType == typeof(bool))
				return "false";
			if (classType == typeof(char))
				return "\0";
			if (classType == typeof(byte) || classType == typeof(sbyte) || classType == typeof(short) || 
			    classType == typeof(ushort) || classType == typeof(uint) || classType == typeof(long) ||
			    classType == typeof(ulong) || classType == typeof(int))
				return "0";
			if (classType == typeof(float) || classType == typeof(double) || classType == typeof(decimal))
				return "0.0";
		}

		if (classType.IsValueType)
		{
			if (classType == typeof(void))
				return null;

			// TODO: Return constructor???
			return null;//Activator.CreateInstance(classType);
		}
            
		if (classType == typeof(string))
		{
			return "";
		}

		if (classType == typeof(object))
		{
			return null;
		}

		Debug.LogWarning("Unhandled primitive type: " + classType);
		return null;
	}
	
	public static bool ContainsText(this string text, string substring, bool caseSensitive = true)
	{
		if (string.IsNullOrEmpty(text))
		{
			if (string.IsNullOrEmpty(substring))
			{
				// null.ContainsText(null)
				// "".ContainsText("")
				return true;
			}
			
			// null.ContainsText("Hello)
			// "".ContainsText("Hello")
			return false;
		}
		else if (string.IsNullOrEmpty(substring))
		{
			// "Hello".ContainsText(null)
			// "Hello".ContainsText("")
			return false;
		}

		return text.IndexOf(substring, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) >= 0;
	}
	
	public static bool TryParseEnum<T>(this string value, out T defaultValue)
	{
		try
		{
			object o = Enum.Parse(typeof(T), value, true);
			defaultValue = (T)o;
			return true;
		}
		catch (Exception)
		{
			defaultValue = default;
			return false;
		}
	}
	
	public static string ToLiteral(string input) 
	{
		StringBuilder literal = new(input.Length + 2);
		literal.Append("\"");
		foreach (var c in input) {
			switch (c) {
				case '\"': literal.Append("\\\""); break;
				case '\\': literal.Append(@"\\"); break;
				case '\0': literal.Append(@"\0"); break;
				case '\a': literal.Append(@"\a"); break;
				case '\b': literal.Append(@"\b"); break;
				case '\f': literal.Append(@"\f"); break;
				case '\n': literal.Append(@"\n"); break;
				case '\r': literal.Append(@"\r"); break;
				case '\t': literal.Append(@"\t"); break;
				case '\v': literal.Append(@"\v"); break;
				default:
					// ASCII printable character
					if (c >= 0x20 && c <= 0x7e) {
						literal.Append(c);
						// As UTF16 escaped character
					} else {
						literal.Append(@"\u");
						literal.Append(((int)c).ToString("x4"));
					}
					break;
			}
		}
		literal.Append("\"");
		return literal.ToString();
	}
}

public static class KeyCodeExtensions
{
	public static string Serialize(this KeyCode keyCode)
	{
		return keyCode.ToString();
	}
	
	public static string Serialize(this IEnumerable<KeyCode> keyCode, string separator = ",", bool includeUnassigned = false)
	{
		string serialized = "";
		if (keyCode != null)
		{
			serialized = string.Join(separator, keyCode.Select((a)=>a.Serialize()).ToArray());
		}

		if (includeUnassigned && string.IsNullOrEmpty(separator))
		{
			serialized = "Unassigned";
		}
		return serialized;
	}
}