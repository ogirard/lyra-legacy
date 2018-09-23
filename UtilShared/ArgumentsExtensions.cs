using System;
using System.Collections.Generic;
using System.Linq;

namespace Lyra2.UtilShared
{
    public static class ArgumentsExtensions
    {
        private const string ArgumentIdentifier = "-";
        private const string SwitchIdentifier = "--";
        private const char ArgumentValueDelimiter = ':';
        private const char ArgumentListDelimiter = ',';

        public static bool IsSwitchSet(this IEnumerable<string> args, string argumentName)
        {
            return args != null && args.Any(a => a.StartsWith(SwitchIdentifier + argumentName, StringComparison.OrdinalIgnoreCase));
        }

        public static string GetArgumentValue(this IEnumerable<string> args, string argumentName)
        {
            return args.GetArgumentValue<string>(argumentName);
        }

        public static T GetArgumentValue<T>(this IEnumerable<string> args, string argumentName, T defaultValue = default(T))
        {
            var arg = args.FirstOrDefault(a => a.StartsWith(ArgumentIdentifier + argumentName + ArgumentValueDelimiter, StringComparison.OrdinalIgnoreCase));
            return arg.GetArgumentValue(defaultValue);
        }

        public static IEnumerable<string> SetArgumentValue(this IEnumerable<string> args, string argumentName, string value, bool overwrite = true)
        {
            var resultArgs = args.ToList();
            var arg = resultArgs.FirstOrDefault(a => a.StartsWith(ArgumentIdentifier + argumentName + ArgumentValueDelimiter, StringComparison.OrdinalIgnoreCase));
            if (arg != null)
            {
                if (!overwrite)
                {
                    return resultArgs;
                }

                resultArgs.Remove(arg);
            }

            resultArgs.Add(ArgumentIdentifier + argumentName + ArgumentValueDelimiter + '"' + value.Trim('"') + '"');
            return resultArgs;
        }

        public static string GetArgumentValue(this string arg)
        {
            return arg.GetArgumentValue<string>();
        }

        public static T GetArgumentValue<T>(this string arg, T defaultValue = default(T))
        {
            if (string.IsNullOrEmpty(arg) || !arg.Contains(ArgumentValueDelimiter))
            {
                return defaultValue;
            }

            var delimiterPosition = arg.IndexOf(ArgumentValueDelimiter);
            if (delimiterPosition == -1)
            {
                return defaultValue;
            }

            var value = arg.Substring(delimiterPosition + 1).Trim(' ', '\"');
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static string GetArgumentName(this string arg)
        {
            return arg?.Split(ArgumentValueDelimiter).FirstOrDefault()?.Remove(0, ArgumentIdentifier.Length);
        }

        public static string[] ExpandListArgument(this string arg)
        {
            if (string.IsNullOrEmpty(arg))
            {
                return new string[0];
            }

            return arg.Split(ArgumentListDelimiter).Select(a => a.Trim('"', ' ')).ToArray();
        }

        public static void CheckCount(this string[] args, int expectedCount, string usage)
        {
            args.CheckCount(expectedCount, expectedCount, usage);
        }

        public static void CheckCount(this string[] args, int minCount, int maxCount, string usage)
        {
            if (args == null || args.Length < minCount || args.Length > maxCount)
            {
                var expected = minCount == maxCount ? minCount.ToString() : $"[{minCount};{maxCount}]";
                throw new ArgumentException($"{expected} arguments expected, got {args?.Length ?? 0} instead. Usage {usage}", nameof(args));
            }
        }

        public static string GetArgumentAt(this string[] args, int index, string defaultValue = null) => args.GetArgumentAt<string>(index, defaultValue);

        public static T GetArgumentAt<T>(this string[] args, int index, T defaultValue = default(T))
        {
            if (args == null || index >= args.Length)
            {
                return defaultValue;
            }

            return (T)Convert.ChangeType(args[index], typeof(T));
        }

        public static Tuple<string, IEnumerable<string>> GetTags(this string value, string tagStart = "<<", string tagEnd = ">>")
        {
            if (string.IsNullOrEmpty(tagStart))
            {
                throw new ArgumentException("TagStart must not be null or empty", nameof(tagStart));
            }

            if (string.IsNullOrEmpty(tagEnd))
            {
                throw new ArgumentException("TagEnd must not be null or empty", nameof(tagEnd));
            }

            var tags = new List<string>();
            if (string.IsNullOrWhiteSpace(value))
            {
                return new Tuple<string, IEnumerable<string>>(value, tags);
            }

            var i = 0;
            while (i < value.Length)
            {
                var nextStart = value.IndexOf(tagStart, i, StringComparison.Ordinal);
                if (nextStart < 0)
                {
                    break;
                }

                var nextEnd = value.IndexOf(tagEnd, nextStart + tagStart.Length, StringComparison.Ordinal);
                if (nextEnd < 0)
                {
                    // tag not properly closed!
                    break;
                }

                tags.Add(value.Substring(nextStart + tagStart.Length, nextEnd - nextStart - tagStart.Length));
                i = nextEnd + tagEnd.Length;
            }

            return new Tuple<string, IEnumerable<string>>(value, tags);
        }

        public static bool IsTagSet(this Tuple<string, IEnumerable<string>> taggedValue, string tag)
        {
            if (string.IsNullOrEmpty(taggedValue?.Item1) || !taggedValue.Item2.Any())
            {
                return false;
            }

            return taggedValue.Item2.Any(t => string.Compare(t, tag, StringComparison.OrdinalIgnoreCase) == 0);
        }
    }
}