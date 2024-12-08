using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace AspNetCore_WebAPP_MVC_PROJE.ViewComponents
{
    public class Currency : ViewComponent
    {
        public string Invoke()
        {
            string url = "https://www.tcmb.gov.tr/kurlar/today.xml";

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(url);

            string dolar = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;

            string usdsatis = dolar.Substring(0, 5); // 18,79 -> 5 karakter

            return $"1 USD = {usdsatis} TL";
        }
    }
}