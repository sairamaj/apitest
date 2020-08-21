using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiManager.NewRequest
{
	// this is purely temporary ( need to convert to proper dsl)
	internal class Evaluator
	{
		public static string TranslateHeaderItem(string input, IDictionary<string,string> variables)
		{
			var base64Index = input.IndexOf("base64");
			if (base64Index < 0)
			{
				return input;
			}

			var output = input.Substring(0, base64Index);
			var base64Value = input.Substring(base64Index + "base64".Length);
			foreach (var kv in variables)
			{
				var variable = $"{{{{{kv.Key}}}}}";
				base64Value = base64Value.Replace(variable, kv.Value);
			}

			return output + System.Convert.ToBase64String(Encoding.UTF8.GetBytes(base64Value.Trim()));
		}

		public static string Evaluate(string input, IDictionary<string, string> dataVariables)
		{
			var output = input;
			foreach (var variable in GetVariables(input))
			{
				Console.WriteLine(variable);
				if (dataVariables.ContainsKey(variable))
				{
					// try for {{{variable}}}
					var valToReplace = $"{{{{{{{variable}}}}}}}";
					output = output.Replace(valToReplace, dataVariables[variable]);

					// try for {{variable}}
					valToReplace = $"{{{{{variable}}}}}";
					output = output.Replace(valToReplace, dataVariables[variable]);
				}
			}

			return output;
		}

		static IEnumerable<string> GetVariables(string input)
		{
			var regex = new Regex(@"(?<variable>{{\w+)}}");
			var variables = new List<string>();
			var output = input;
			var matches = regex.Matches(input);
			foreach (Match m in matches)
			{
				foreach (var m1 in m.Captures)
				{
					variables.Add(m.ToString().Replace("{", string.Empty).Replace("}", string.Empty));
				}
			}

			return variables;
		}
	}
}
