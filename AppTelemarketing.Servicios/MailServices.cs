using AppTelemarketing.Entidades;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace AppTelemarketing.Servicios
{
    //aqui construyo el servicio para enviar/recibir un correo electronico
    //Necesito los datos de la configuracion y del mail que voy a enviar, 
    //los servicios necesitan las entidades, los uno

    public class MailServices
    {

        MailSettings settingsMail; //para enviar y recibir necesito esta info de manera global


        //creo los parametros desde un constructor
        //me traigo los datos del archico de configuracion
        public MailServices() 
        {
            var Section = (NameValueCollection)ConfigurationManager.GetSection("MailSettings");
            //Establesco los valores a travez del constructor
            settingsMail = new MailSettings(
                Section["Server"], int.Parse(Section["Port"]), Section["FromName"], Section["FromEmail"],
                Section["UserName"], Section["Password"]) ;

        }

        //para enviar un correo electronico
        public void SendMail(MailData dataMail) //para quien va, asunto y body
        {
            var mail = new MailMessage();
            mail.To.Add(dataMail.Mailto);
            mail.Subject = dataMail.Subject;
            mail.From = new MailAddress(settingsMail.FromEmail,settingsMail.FromName);//nueva instancia de una dire de correo 
            mail.IsBodyHtml = true;
            mail.Body = dataMail.Body; //texto con una plantilla en HTML

            //me tengo que conectar a Gmail - creamos un cliente smtp
            var client = new SmtpClient();
            client.Host = settingsMail.Server;
            client.Port = settingsMail.Port;

            //credenciales
            client.Credentials = new NetworkCredential(settingsMail.UserName,settingsMail.Password);
            client.EnableSsl = true; //es una via segura, para que viaje protegido el user y pass a traves de la red

            // envio el correo 
            client.Send(mail);

            client.Dispose();


        }
    }
}
