using AspNetCore_WebAPP_MVC_PROJE.Models.DbSets;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Text;
using XSystem.Security.Cryptography;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.MVVM
{
    public class cls_User
    {
        KayaliContext context = new KayaliContext();

        #region LOGIN CONTROL METHODS

        //ADMIN PANEL LOGIN CONTROL
        public async Task<User> LoginControl(User user)
        {
            string MD5Password = MD5PassConverter(user.Password);

            User? usr = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == MD5Password && u.IsAdmin == true && u.Active == true);

            return usr;
        }

        //REGULAR USER LOGIN CONTROL
        public User? SelectUserInfo(string userInfo)
        {
            User? user = context.Users.FirstOrDefault(u => u.Email == userInfo);
            return user;
        }
        #endregion


        public string MemberControl(User user)
        {
            string answer = "";

            try
            {
                string? md5Password = MD5PassConverter(user.Password);

                User? usr = context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == md5Password);

                if (usr == null)
                {
                    //Wrong password or mail adress.
                    answer = "not found";
                }
                else
                {
                    //User is registered, info is in the DB. But we need to check if it's ADMIN or Default User.
                    if (usr.IsAdmin == true)
                    {
                        //Admin login.
                        answer = "admin";
                    }
                    else
                    {
                        //Default User login.
                        answer = usr.Email;
                    }
                }
            }

            catch (Exception)
            {

                return "ERROR !";
            }

            return answer;
        }

        public static string MD5PassConverter(string value)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] btr = Encoding.UTF8.GetBytes(value);
            btr = md5.ComputeHash(btr);

            StringBuilder sb = new StringBuilder();
            foreach (byte item in btr)
            {
                sb.Append(item.ToString("x2").ToLower());
            }
            return sb.ToString();
        }

        //USER MAIL TAKEN ?
        public bool RegisterEmailControl(User user)
        {
            User? mail = context.Users.FirstOrDefault(u => u.Email == user.Email);

            if (mail == null)
            {
                return false;
            }

            else
            {
                return true;
            }
            
        }

        //USER REGISTER
        public bool AddUser(User user)
        {
            try
            {
                user.Active = true;
                user.IsAdmin = false;
                user.Password = MD5PassConverter(user.Password);

                context.Add(user);
                context.SaveChanges();

                return true;
            }

            catch (Exception)
            {
                return false;
            }

        }

        public User SelectUserDetails(int id)
        {
            User user = context.Users.FirstOrDefault(u=>u.UserID == id);
            return user;
        }

        public bool UpdateUserInfo(User user)
        {
            try
            {
                context.Update(user);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        #region SMS and Email SENDERS
        //Example SMS firm xml communication
        public void SendSMS(string OrderGroupGUID)
        {
            string ss = "";
            ss += "<?xml version='1.0' encoding='UTF-8'>";
            ss += "<mainbody>";
            ss += "<header>";
            ss += "<company dil=TR>alperkayali.com</company>";
            ss += "<usercode>0850 and given user code here</usercode>";
            ss += "<password>NetGSM123 example password</password>";
            ss += "<startdate></startdate>";
            ss += "<stopdate></stopdate>";
            ss += "<type>n:n how many people will receive this SMS ?</type>";
            ss += "<msgheader>HEADER</msgheader>";
            ss += "</header>";
            ss += "<body>";

            Order order = context.Orders.FirstOrDefault(o => o.OrderGroupGUID == OrderGroupGUID);
            User user = context.Users.FirstOrDefault(u => u.UserID == order.UserID);

            string content = "Dear " + user.NameSurname + ", your order has been set with the number of" + OrderGroupGUID + " at " + DateTime.Now;

            ss += "<mp><msg><![CDATA[" + content + "]]></msg><no>90" + user.Telephone + "</no></mp>";
            ss += "</body>";
            ss += "</mainbody>";

            string answer = XMLPOST("https://api.netgsm.com/tr/xmlbulkhttppost.asp", ss);
            if (answer != "-1")
            {
                //sms has been sent.
            }
            else { /*fail*/ }
        }

        public string XMLPOST(string url, string xmlData)
        {
            try
            {
                WebClient wUpload = new WebClient();
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                Byte[] bPostArray = Encoding.UTF8.GetBytes(xmlData);
                Byte[] bResponse = wUpload.UploadData(url, "POST", bPostArray);

                Char[] sReturnsChars = Encoding.UTF8.GetChars(bResponse);

                string sWebPage = new string(sReturnsChars);
                return sWebPage;
            }

            catch (Exception)
            {
                return "-1";
            }
        }

        public void SendEMAIL(string OrderGroupGUID)
        {
            Order order = context.Orders.FirstOrDefault(o => o.OrderGroupGUID == OrderGroupGUID);

            string mail = "sender Email is here"; //must be taken from DB, dynamically.

            User user = context.Users.FirstOrDefault(u => u.UserID == order.UserID);

            string _mail = user.Email;
            string subject = "";
            string content = "";

            content = "Dear " + user.NameSurname + ", your order has been set with the number of" + OrderGroupGUID + " at " + DateTime.Now;
            subject = "Dear " + user.NameSurname + ", your order is being prepared.";

            //below four must stay in a table on DB.
            string host = "smtp.alperkayali.com";
            int port = 587;
            string login = "admin";
            string password = "adminPass";

            MailMessage eMail = new MailMessage();
            eMail.From = new MailAddress(mail, "alperkayali info");//sender
            eMail.To.Add(_mail); //receiver
            eMail.Subject = subject;
            eMail.IsBodyHtml = true;
            eMail.Body = content;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new NetworkCredential(login, password);
            smtpClient.Port = port;
            smtpClient.Host = host;

            try
            {
                smtpClient.Send(eMail);
            }

            catch (Exception)
            {
                throw;
            }

        }
        #endregion


    }
}
