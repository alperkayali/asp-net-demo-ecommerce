using AspNetCore_WebAPP_MVC_PROJE.Models.DbSets;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore_WebAPP_MVC_PROJE.ViewComponents
{
	public class Email : ViewComponent
	{
		KayaliContext context = new KayaliContext();
		public string Invoke()
		{
			string? email = context.Settings.FirstOrDefault(s => s.SettingID == 1).email;
			return $"{email}";
		}
	}
}
