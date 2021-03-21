using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProvaApi
{
	public class LoginResult
	{
		public string Message { get; set; }

		public LoginResult(string value)
		{
			Message = value;
		}
	}
}
