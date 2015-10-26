using System.Text.RegularExpressions;
using System.Linq;

namespace Fulcrum.Common
{
	public static class StringExtensions
	{
		const string Vowels = "aeiouy";

		// based on http://stackoverflow.com/questions/2920744/url-slugify-algorithm-in-c
		public static string ToSlug(this string input)
		{
			input = Regex.Replace(input, @"\s+", " ").Trim();
			
			input = input.Substring(0, input.Length <= 12 ? input.Length : 12).Trim();
			
			input = Regex.Replace(input, @"\s", "-");
			
			return input.Disemvowel().ToUpper();
		}

		public static string Disemvowel(this string input)
		{
			return new string(input.Where(c => !Vowels.Contains(c)).ToArray());
		}
	}
}
