using System;

namespace Tweddle.Commons.Vies
{
	/// <summary>
	/// Summary description for Result.
	/// </summary>
	public class ViesResult
	{
		public static ViesResult UNCERTAIN = new ViesResult("Uncertain", "Dummy", -1);

		private string name;
		private string address;
		private int isValid;
		
		public ViesResult()
		{
		}

		public ViesResult(string name, string address, int isValid)
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
