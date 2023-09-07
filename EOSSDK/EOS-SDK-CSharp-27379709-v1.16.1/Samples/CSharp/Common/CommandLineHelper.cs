// Copyright Epic Games, Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Epic.OnlineServices.Samples
{
	public static class CommandLineHelper
	{
		private static string CleanArg(string arg)
		{
			return arg.ToLower().Replace("-", "");
		}

		private static IEnumerable<string> CleanArgs(IEnumerable<string> args)
		{
			return args.Select(arg => CleanArg(arg));
		}

		private static int IndexOf(this IEnumerable<string> strArray, string str)
		{
			if (strArray.Contains(str))
			{
				return strArray.TakeWhile(innerStr => innerStr != str).Count();
			}

			return -1;
		}

		private static int IndexOfArg(this string[] args, string arg)
		{
			return CleanArgs(args).IndexOf(CleanArg(arg));
		}

		public static bool ContainsArg(this string[] args, string arg)
		{
			if (CleanArgs(args).Contains(CleanArg(arg)))
			{
				return true;
			}

			return false;
		}

		private static bool TryParseEnum(Type type, string value, bool ignoreCase, out object objectValue)
		{
			objectValue = null;

			try
			{
				objectValue = Enum.Parse(type, value, ignoreCase);
				return true;
			}
			catch (Exception)
			{
			}

			return false;
		}

		private static TConvertible ReadArg<TConvertible>(this string[] args, string arg, out bool hasValueArg)
			where TConvertible : IConvertible
		{
			var value = default(TConvertible);
			hasValueArg = false;

			if (args.ContainsArg(arg))
			{
				int argIndex = args.IndexOfArg(arg);
				string argValue = null;

				try
				{
					argValue = args[argIndex + 1];
				}
				catch (IndexOutOfRangeException)
				{
					// If it's a bool value, we don't require an explicit value
					if (typeof(TConvertible) == typeof(bool))
					{
						return (TConvertible)(IConvertible)true;
					}
					else
					{
						throw;
					}
				}

				try
				{
					value = (TConvertible)((IConvertible)argValue).ToType(typeof(TConvertible), System.Globalization.CultureInfo.InvariantCulture);
					hasValueArg = true;
				}
				catch (FormatException)
				{
					// Conversion from int to bool
					if (typeof(TConvertible) == typeof(bool) && int.TryParse(argValue, out var intValue))
					{
						value = (TConvertible)((IConvertible)intValue).ToType(typeof(TConvertible), System.Globalization.CultureInfo.InvariantCulture);
						hasValueArg = true;
					}
					// The existence of this arg means true
					else if (typeof(TConvertible) == typeof(bool))
					{
						value = (TConvertible)(IConvertible)true;
					}
					else
					{
						throw;
					}
				}
				catch (InvalidCastException)
				{
					// Conversion from int to enum
					if (typeof(TConvertible).IsEnum && int.TryParse(argValue, out var intValue) && Enum.IsDefined(typeof(TConvertible), intValue))
					{
						value = (TConvertible)(object)intValue;
						hasValueArg = true;
					}
					// Conversion from string to enum
					else if (typeof(TConvertible).IsEnum && TryParseEnum(typeof(TConvertible), argValue, true, out object objectValue))
					{
						value = (TConvertible)objectValue;
						hasValueArg = true;
					}
					else
					{
						throw;
					}
				}
				catch (Exception)
				{
					throw;
				}

			}

			return value;
		}

		public static TConvertible ReadArg<TConvertible>(this string[] args, string arg)
			where TConvertible : IConvertible
		{
			return ReadArg<TConvertible>(args, arg, out _);
		}

		public static TConvertible ReadArg<TConvertible>(this string[] args, string arg, TConvertible defaultValue)
			where TConvertible : IConvertible
		{
			if (args.ContainsArg(arg))
			{
				return ReadArg<TConvertible>(args, arg, out _);
			}

			return defaultValue;
		}

		public static bool TryConsumeArg<TConvertible>(ref string[] args, string arg, TConvertible defaultValue, out TConvertible value)
			where TConvertible : IConvertible
		{
			value = defaultValue;

			if (args.ContainsArg(arg))
			{
				int argIndex = args.IndexOfArg(arg);

				List<int> indicesToConsume = new List<int>();
				indicesToConsume.Add(argIndex);

				bool hasValueArg;
				value = args.ReadArg<TConvertible>(arg, out hasValueArg);

				if (hasValueArg)
				{
					indicesToConsume.Add(argIndex + 1);
				}

				args = args.Where((thisArg, thisIndex) => !indicesToConsume.Contains(thisIndex)).ToArray();

				return true;
			}

			return false;
		}

		public static bool TryConsumeArg<TConvertible>(ref string[] args, string arg, out TConvertible value)
			where TConvertible : IConvertible
		{
			return TryConsumeArg(ref args, arg, default(TConvertible), out value);
		}
	}
}
