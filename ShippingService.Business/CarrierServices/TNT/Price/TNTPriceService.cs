using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.CarrierServices.TNT.Price
{
    public class TNTPriceService
    {
        public static string Send(string xml)
        {
            string url = "https://express.tnt.com/expressconnect/pricing/getprice";

            
            System.Net.Http.HttpContent content = new System.Net.Http.StringContent(xml, Encoding.UTF8, "application/xml");
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

            var byteArray = Encoding.ASCII.GetBytes("TLITHO:M3CH3LM");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var result = client.PostAsync(url, content).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;

            return resultContent;



        }
    }
}
