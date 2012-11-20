using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;

namespace Tweddle.Commons.Vies
{
	/// <summary>
	/// Summary description for Vies.
	/// </summary>
	public class Vies: IVies
	{
		private Hashtable vatValidations = new Hashtable();

		public Vies()
		{
			vatValidations.Add("AT", "^U[0-9]{9}$");
			vatValidations.Add("BE", "^0?[0-9]{9}$");
			vatValidations.Add("BG", "^[0-9]{9,10}$");
			vatValidations.Add("CY", "^[0-9]{9}[A-Za-z]$");
			vatValidations.Add("CZ", "^[0-9]{8,10}$");
			vatValidations.Add("DE", "^[0-9]{9}$");
			vatValidations.Add("DK", "^[0-9]{8}$");
			vatValidations.Add("EE", "^[0-9]{9}$");
			vatValidations.Add("EL", "^[0-9]{9}$");
            vatValidations.Add("GR", "^[0-9]{9}$");
			vatValidations.Add("ES", "(^[A-Za-z][0-9]{8}$|^[A-Za-z][0-9]{7}[A-Za-z]$|^[0-9]{8}[A-Za-z]$)");
			vatValidations.Add("FI", "^[0-9]{8}$");
			vatValidations.Add("FR", "(^[0-9]{11}$|^[A-HJ-NP-Za-hj-np-z][0-9]{10}$|^[A-HJ-NP-Za-hj-np-z]{2}[0-9]{9}$|^[0-9][A-HJ-NP-Za-hj-np-z][0-9]{9}$)");
			//United Kingdom can be identified both as UK and GB
			vatValidations.Add("GB", "(^[0-9]{9}$|^[0-9]{12}$|^GD[0-9]{3}$|^HA[0-9]{3}$)");
			vatValidations.Add("UK", "(^[0-9]{9}$|^[0-9]{12}$|^GD[0-9]{3}$|^HA[0-9]{3}$)");

			vatValidations.Add("HU", "^[0-8]{8}$");
			vatValidations.Add("IE", "(^[0-9]{7}[A-Za-z]$|^[0-9][A-Za-z][0-9]{5}[A-Za-z]$)");
			vatValidations.Add("IT", "^[0-9]{11}$");
			vatValidations.Add("LT", "^[0-9]{9}$|^[0-9]{12}$");
			vatValidations.Add("LU", "^[0-9]{8}$");
			vatValidations.Add("LV", "^[0-9]{11}$");
			vatValidations.Add("MT", "^[0-9]{8}$");
			vatValidations.Add("NL", "^[0-9]{9}[bB][0-9]{2}$");
			vatValidations.Add("PL", "^[0-9]{10}$");
			vatValidations.Add("PT", "^[0-9]{9}$");
			vatValidations.Add("RO", "^[0-9]{2,10}$");
			vatValidations.Add("SE", "^[0-9]{12}$");
			vatValidations.Add("SI", "^[0-9]{8}$");
			vatValidations.Add("SK", "^[0-9]{10}$");
		}
		#region IVies Members

		public bool IsVATEligible(string countryIso2Code)
		{
			if (!StringUtil.IsNullOrEmpty(countryIso2Code))
			{
				return vatValidations.ContainsKey(countryIso2Code);
			}
			else
			{
				return false;
			}
		}

		public bool IsValidSyntax(string vatnumber)
		{
			if (!StringUtil.IsNullOrEmpty(vatnumber))
			{
				string country = vatnumber.Substring(0, 2).ToUpper();
				vatnumber = vatnumber.Substring(2).Trim();

				if(vatValidations.ContainsKey(country))
				{
					string validationExpression = vatValidations[country].ToString();
					Regex regex = new Regex(validationExpression);

					return regex.IsMatch(vatnumber);
				}
				else
					return false;
			}
			else
			{
				return false;
			}
		}

		public int IsValid(string vatnumber)
		{
			if (!StringUtil.IsNullOrEmpty(vatnumber))
			{
				if (vatnumber == null || vatnumber.Length<3)
				{
					return 0;
				}

				string country = vatnumber.Substring(0, 2).ToUpper();
				string name = string.Empty;
				string address = string.Empty;
				bool isValid = false;

				vatnumber = vatnumber.Substring(2).Trim();

				//vatChecker.checkVatService service = new vatChecker.checkVatService();
				ViesVatProxy viesproxy = new ViesVatProxy();

				try
				{
					//service.checkVat(ref country, ref vatnumber, out isValid, out name, out address);
					viesproxy.checkVat(ref country, ref vatnumber, out isValid, out name, out address);
					return  (isValid)?1:0;
				}
				catch (Exception e)
				{					
					return -1;
				}
			}
			else
			{
				return 0;
			}
		}

		public ViesResult IsValidInclDetails(string vatnumber)
		{
			if (!StringUtil.IsNullOrEmpty(vatnumber))
			{
				string country = vatnumber.Substring(0, 2).ToUpper();
				string name = string.Empty;
				string address = string.Empty;
				int valid = -1;

				bool isValid = false;

				vatnumber = vatnumber.Substring(2).Trim();

				//vatChecker.checkVatService service = new vatChecker.checkVatService();
				ViesVatProxy viesproxy = new ViesVatProxy();

				try
				{
					//service.checkVat(ref country, ref vatnumber, out isValid, out name, out address);
					viesproxy.checkVat(ref country, ref vatnumber, out isValid, out name, out address);	
					valid = isValid ? 1 : 0;
				}
				catch
				{
					valid = -1;
				}

				return new ViesResult(name, address, valid);
			}
			else
			{
				return new ViesResult(null, null, 0);
			}
		}

		#endregion
	}
}
