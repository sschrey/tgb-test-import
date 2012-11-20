using System;

namespace Tweddle.Commons.Vies
{
	[System.Web.Services.WebServiceBindingAttribute(Name = "checkVatPortSoapBinding", Namespace = "urn:ec.europa.eu:taxud:vies:services:checkVat")]
	public class ViesVatProxy : System.Web.Services.Protocols.SoapHttpClientProtocol
	{
		public ViesVatProxy()
		{
			this.Url = "http://ec.europa.eu/taxation_customs/vies/api/checkVatPort";
		}

		[System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "urn:ec.europa.eu:taxud:vies:services:checkVat:types", ResponseNamespace = "urn:ec.europa.eu:taxud:vies:services:checkVat:types", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
		[return: System.Xml.Serialization.XmlElementAttribute("requestDate", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "date")]
		public System.DateTime checkVat([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] ref string countryCode, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] ref string vatNumber, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] out bool valid, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] out string name, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] out string address)
		{
			object[] results = this.Invoke("checkVat", new object[] {
																		countryCode,
																		vatNumber});
			countryCode = ((string)(results[1]));
			vatNumber = ((string)(results[2]));
			valid = ((bool)(results[3]));
			name = ((string)(results[4]));
			address = ((string)(results[5]));
			return ((System.DateTime)(results[0]));
		}
	}    
}
