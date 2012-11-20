using System;

namespace Tweddle.Commons.Vies
{
	/// <summary>
	/// Summary description for Result.
	/// </summary>
	public class Result
	{
		private string name;
		private string address;
		private int isValid;
		
		public Result()
		{
		}

		public Result(string name, string address, int isValid)
		{
			this.name = name;
			this.address = address;
			this.isValid = isValid;
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		public string Address
		{
			get
			{
				return address;
			}
		}

		public int IsValid
		{
			get
			{
				return isValid;
			}
		}
	}
}
