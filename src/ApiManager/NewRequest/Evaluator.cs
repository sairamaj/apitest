using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	}
}
