using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lucene.Net.QueryParsers;

namespace Lyra2.LyraShell
{
  public class QueryHelper
  {
    private readonly static string Quote = Convert.ToString(QuoteChar);
    private const char QuoteChar = '\"';
    private const char SpaceChar = ' ';

    public static IEnumerable<string> ExtractWordsOrPhrases(string query, bool normalize = true)
    {
      if (string.IsNullOrEmpty(query))
      {
        return new List<string>();
      }

      var wordsOrPhrases = new List<string>();
      IList<string> parsedWords =
        query.Split(new[] { SpaceChar }, StringSplitOptions.RemoveEmptyEntries).Select(
          w => IsPhrase(w) ? w.Trim(QuoteChar) : w).ToList();

      string phrase = null;
      foreach (var word in parsedWords)
      {
        if (word.StartsWith(Quote) && phrase == null)
        {
          phrase = EscapeWord(word.TrimStart(QuoteChar));
        }
        else if (word.EndsWith(Quote) && phrase != null)
        {
          phrase += " " + EscapeWord(word.TrimEnd(QuoteChar));
          wordsOrPhrases.Add(QuoteChar + phrase + QuoteChar);
          phrase = null;
        }
        else
        {
          if (phrase == null)
          {
            wordsOrPhrases.Add(EscapeWord(word));
          }
          else
          {
            phrase += SpaceChar + word;
          }
        }
      }

      if (phrase != null)
      {
        wordsOrPhrases.Add(QuoteChar + phrase + QuoteChar);
      }

      if (normalize)
      {
        wordsOrPhrases = wordsOrPhrases.Select(NormalizeText).ToList();
      }

      return wordsOrPhrases;
    }

    private static string EscapeWord(string word)
    {
      var escaped = QueryParser.Escape(word);
      escaped = escaped.Replace("\\*", "*");
      escaped = escaped.Replace("\\?", "?");
      escaped = escaped.Replace("\\(", "(");
      escaped = escaped.Replace("\\)", ")");
      return escaped;
    }

    public static bool IsPhrase(string word)
    {
      return !String.IsNullOrEmpty(word) && word.StartsWith(Quote) && word.EndsWith(Quote);
    }

    public static string NormalizeText(string term)
    {
      if (IsPhrase(term))
      {
        return term;
      }

      // ignore apostrophe
      if (term.Contains("\'"))
      {
        return QuoteChar + term.Replace("\'", " ") + QuoteChar;
      }

      return term;
    }

    public static string FormatTag(string word, int desiredNumberLength)
    {
      int nr;
      if (Int32.TryParse(word, out nr) && nr >= 0 && nr < 10000)
      {
        return Convert.ToString(nr, CultureInfo.InvariantCulture).PadLeft(Math.Max(0, desiredNumberLength), '0');
      }

      return word.Trim(QuoteChar, '(', ')').Replace("*", "").Replace("?", "");
    }

    public static bool Contains(string text, IEnumerable<string> tags)
    {
      var preparedText = PrepareForComparison(text);
      return tags.Any(tag => preparedText.Contains(PrepareForComparison(tag)));
    }

    public static string PrepareForComparison(string text)
    {
      if (string.IsNullOrEmpty(text))
      {
        return string.Empty;
      }

      var preparedText = text.ToLowerInvariant();
      return RemoveCharacters.Aggregate(preparedText, (current, c) => current.Replace(c, SpaceChar));
    }

    private static readonly char[] RemoveCharacters = new[] { '\'', '’', ',', '´', '=', '+', '-', '&', '|', '!', '(', ')', '{', '}', '[', ']', '^', '\"', '~', '*', '?', ':', '\\' };
  }
}