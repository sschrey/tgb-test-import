using System;

namespace Tweddle.Commons.Vies
{
	/// <summary>
	/// Summary description for IVies.
	/// Authored by Bart STRUBBE.
	/// </summary>
	public interface IVies
	{
		/// <summary>
		/// European country which has to pay VAT?
		/// (Need to provide valid VAT# in order to not have to pay VAT)
		/// </summary>
		/// <param name="countryIso2Code"></param>
		/// <returns></returns>
		bool IsVATEligible(string countryIso2Code);
		/// <summary>
		/// Performs offline syntax check of VAT number
		/// </summary>
		/// <param name="vatnumber">Vat-number to validate</param>
		/// <returns>true if valid</returns>
		bool IsValidSyntax(string vatnumber);

		/// <summary>
		/// Performs online check of VAT number against:
		/// http://ec.europa.eu/taxation_customs/vies/api/checkVatPort?wsdl
		/// </summary>
		/// <param name="vatnumber">Vat-number to validate</param>
		/// <returns>-1 if webservice unavailable, 0 if invalid, 1 if valid</returns>
		int IsValid(string vatnumber);
		/// <summary>
		/// Performs online check of VAT number against:
		/// http://ec.europa.eu/taxation_customs/vies/api/checkVatPort?wsdl
		/// </summary>
		/// <param name="vatnumber">Vat-number to validate</param>
		/// <returns>Result instance containing validity-indicator AND name/address info returned by VIES if VIES is online and indicates valid VAT</returns>
		ViesResult IsValidInclDetails(string vatnumber);
	}
}
