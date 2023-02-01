using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;
using System.Net.Mime;

namespace ConsultaWebApisPagos
{
    class Correo
    {
        public static bool EnviaCorreo(string mailDestinatario, string nombre,string apellido,string codigo)
        {
            mailDestinatario = mailDestinatario.ToLower();
            mailDestinatario.Replace(" ", "");
            MailMessage correo = new MailMessage();
            //correo.From = new MailAddress("dylan.zamorac@gmail.com", "DYLAN", System.Text.Encoding.UTF8);//Correo de salida
            correo.From = new MailAddress("validacion@sagautobuses.com", "Equipo SAG Autobuses (No reply)", System.Text.Encoding.UTF8);//Correo de salida
            try
            {
                correo.To.Add(mailDestinatario); //Correo destino?
                correo.Subject = "Verificación de correo"; //Asunto
                                                //correo.Body = MensajeHtml(ShortdId,Origen,Destino,fechaSalida.ToString("dd/MM/yyyy HH:mm"),fechaLlegada.ToString("dd/MM/yyyy HH:mm"), Total);
                correo.AlternateViews.Add(MensajeHtml(mailDestinatario,nombre,apellido,codigo));
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.Normal;
                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                smtp.Host = "mail.sagautobuses.com"; //Host del servidor de correo
                smtp.Port = 26; //Puerto de salida
                smtp.Credentials = new System.Net.NetworkCredential("validacion@sagautobuses.com", "Med*96850");//Cuenta de correo
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtp.EnableSsl = true;//True si el servidor de correo permite ssl
                try
                {
                    smtp.Send(correo);
                    Console.WriteLine("Exito se envio el correo a: " + mailDestinatario);
                    return true;
                }
                catch
                {
                    Console.WriteLine("Error en smtp");
                    return false;
                }
            }
            catch
            {
                Console.WriteLine("Error con el correo");
                return false;
            }
        }
        public static AlternateView MensajeHtml(string mailDestinatario, string nombre,string apellido, string codigo)
        {
            string msg = "";

            msg = "<!DOCTYPE html>"+
 "<html>"+
    "<head>"+
        "<title>"+"</title>"+
        "<meta http-equiv=\"Content-Type\" content=\"text/html;charset=utf-8\"/>"+
        "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">"+"<meta http - equiv=\"X-UA-Compatible\" content=\"IE=edge\"/>"+
        "<style type=\"text/css\">"+
            ".icon-social:hover{"+
                "color: #0000;"+
                "font-size: 25px;"+
            "}"+
            "/* CLIENT-SPECIFIC STYLES */"+
            "body, table, td, a { -webkit - text - size - adjust: 100 %; -ms - text - size - adjust: 100 %; }"+
            "table, td { mso - table - lspace: 0pt; mso - table - rspace: 0pt; }"+
            "img { -ms - interpolation - mode: bicubic; }"+
            "/* RESET STYLES */"+
            "img { border: 0; height: auto; line - height: 100 %; outline: none; text - decoration: none; }"+
            "table { border - collapse: collapse!important; }"+
            "body { height: 100 % !important; margin: 0!important; padding: 0!important; width: 100 % !important; }"+
            "/* iOS BLUE LINKS */"+
            "a[x - apple - data - detectors] {"+
                "color: inherit!important;"+
                "text-decoration: none!important;"+
                "font-size: inherit!important;"+
                "font-family: inherit!important;"+
                "font-weight: inherit!important;"+
                "line-height: inherit!important;"+
            "}"+
            "/* MEDIA QUERIES */"+
            "@media screen and(max - width: 480px) {"+
                ".mobile - hide {"+
                    "display: none!important;"+
                "}"+
                ".mobile - center {"+
                    "text - align: center!important;"+
                "}"+
            "}"+
            "/* ANDROID CENTER FIX */"+
            "div[style *= \"margin: 16px 0; \"] { margin: 0!important; }"+
        "</style>" +
        "<link rel=\"stylesheet\" href=\"https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.3/font/bootstrap-icons.css\">" +
    "</head>" +
    "<body style=\"margin:0 !important; padding: 0 !important; background-color:#eeeeee;\" bgcolor=\"#eeeeee\">" +
    "<!--HIDDEN PREHEADER TEXT -->" +
        "<div style=\"display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: Open Sans, Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;\">Registro exitoso, ya estamos casi listos para que seas parte de nostros.</div>"+
        "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">" +
            "<tr>" +
                "<td align=\"center\" style=\"background-color: #eeeeee;\" bgcolor=\"#eeeeee\">" +
                    "<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width:600px;\">" +
                        "<tr>" +
                            "<td align=\"center\" valign=\"top\" style=\"font-size:0; padding: 35px;\" bgcolor=\"#0F62AC\">" +
                                "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" +
                                    "<tr>" +
                                        "<td align=\"right\" valign=\"top\" style=\"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 36px; font-weight: 800; line-height: 48px;\" class=\"mobile-center\">" +
                                            "<label style=\"font-size: 16px;color: #eeeeee;\">Conecta con nosotros</label>" +
                                        "</td>" +
                                        "<td align=\"right\" valign=\"top\" style=\"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 36px; font-weight: 800; line-height: 48px;width: 250px;\" class=\"mobile-center\">" + "</td>" +
                                        "<td align=\"right\" valign=\"top\" style=\"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 36px; font-weight: 800; line-height: 48px;\" class=\"mobile-center\">" +
                                            "<a href=\"https://www.facebook.com/pages/category/Bus-Line/SAG-Oficial-117311680396992/\" class=\"facebook icon-social\" target=\"_blank\">" + "<i class=\"bi bi-facebook\" style=\"color: #eeeeee;font-size: 25px;\">" + "</i>" + "</a>" +
                                        "</td>" +
                                        "<td align=\"right\" valign=\"top\" style=\"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 36px; font-weight: 800; line-height: 48px;width: 25px;\" class=\"mobile-center\">" + "</td>" +
                                        "<td align=\"right\" valign=\"top\" style=\"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 36px; font-weight: 800; line-height: 48px;\" class=\"mobile-center\">" +
                                            "<a href=\"https://www.instagram.com/s_a_g_autobuses/\" class=\"instagram icon-social\" target=\"_blank\">" + "<i class=\"bi bi-instagram\" style=\"color: #eeeeee;font-size: 25px;\">" + "</i>" + "</a>" +
                                        "</td>" +
                                        "<td align=\"right\" valign=\"top\" style=\"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 36px; font-weight: 800; line-height: 48px;width: 25px;\" class=\"mobile-center\">" + "</td>" +
                                        "<td align=\"right\" valign=\"top\" style=\"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 36px; font-weight: 800; line-height: 48px;\" class=\"mobile-center\">" +
                                            "<a href=\"https://www.youtube.com/channel/UCATP2RZ5-iazmJwyJOPe9Bw\" class=\"linkedin icon-social\" target=\"_blank\">" + "<i class=\"bi bi-youtube\" style=\"color: #eeeeee;font-size: 25px;\">" + "</i>" + "</a>" +
                                        "</td>" +
                                        "<td align=\"right\" valign=\"top\" style=\"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 36px; font-weight: 800; line-height: 48px;width: 25px;\" class=\"mobile-center\">" + "</td>" +
                                        "<td align=\"right\" valign=\"top\" style=\"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 36px; font-weight: 800; line-height: 48px;\" class=\"mobile-center\">" +
                                            "<a href=\"https://tiktok.com/@sag_autobuses?_t=8VSx6wykyGc&amp;_r=1\" class=\"tiktok icon-social\" target=\"_blank\">" + "<i class=\"bi bi-tiktok\" style=\"color: #eeeeee;font-size: 25px;\">" + "</i>" + "</a>" +
                                        "</td>" +
                                    "</tr>" +
                                "</table>" +
                                "<div style=\"display:inline-block; max-width:50%; min-width:100px; vertical-align:top; width:100%;\" class=\"mobile-hide\">" +
                                    "<table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width:300px;\">" +
                                        "<tr>" +
                                            "<td align=\"right\" valign=\"top\" style=\"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; line-height: 48px;\">" +
                                                "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"right\">" +
                                                    "<tr>" +
                                                        "<td style=\"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400;\">" +
                                                            "<p style=\"font-size: 18px; font-weight: 400; margin: 0; color: #ffffff;\">" +
                                                                "<a href=\"http://sagautobuses.com/\" target=\"_blank\" style=\"color: #ffffff; text-decoration: none;\">" + "</a>" +
                                                            "</p>" +
                                                        "</td>" +
                                                        "<td style=\"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 24px;\">" +
                                                            "<a href=\"http://sagautobuses.com/\" target=\"_blank\" style=\"color: #ffffff; text-decoration: none;\">" + "</a>" +
                                                        "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                            "</td>" +
                                        "</tr>" +
                                    "</table>" +
                                "</div>" +
                            "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td align=\"center\" style=\"padding: 35px 35px 20px 35px; background-color: #ffffff;\" bgcolor=\"#ffffff\">" +
                                "<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width:600px;\">" +
                                    "<tr>" +
                                        "<td align=\"center\" style=\"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding-top: 25px;\">" +
                                            "<img src=\"https://i.ibb.co/KqfxXcx/logo-footer.png\" 'cid:hero-image-receipt' width=\"250\" style=\"display: block; border: 0px;\" />" + "<br>" +
                                            "<h2 style=\"font-size: 30px; font-weight: 800; line-height: 36px; color: #333333; margin: 0;\">¡Te damos la bienvenida!</h2>" +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td align=\"left\" style=\"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding-top: 10px;\">" +
                                            "<p style=\"font-size: 16px; font-weight: 400; line-height: 24px; color: #777777;text-align: justify;margin-bottom: 40px;\">Hola "+nombre+" "+apellido+":</p>" +
                                            "<p style=\"font-size: 16px; font-weight: 400; line-height: 24px; color: #777777;text-align: justify;\">Gracias por registrarte en SAG Autobuses.Nos alegramos de que pertenezcas a nostros.</p>" +
                                            "<p style=\"font-size: 16px; font-weight: 400; line-height: 24px; color: #777777;text-align: justify;\">Puedes utilizar nuestra plataforma para administrar tus viajes.En un momento, inicia sesión para comenzar.</p>" +
                                            "<p style=\"font-size: 16px; font-weight: 400; line-height: 24px; color: #777777;text-align: justify;\">A continuación, verifica tu correo electrónico en unos pasos sencillos, presionando el botón.</p>" +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr style=\"height: 50px;\">" + "</tr>" +
                                    "<tr>" +
                                        "<td align=\"center\" style= \"border-radius: 5px;\" bgcolor= \"#66b3b7\">" +
                                            "<a href=\"http://sagautobuses.com/Proceso?token_verifier="+codigo+"&email_verifier="+mailDestinatario+"\"&version=0\" target= \"_blank\" style= \"font-size: 18px; font-family: Open Sans, Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; border-radius: 5px; background-color: #0F62AC; padding: 15px 30px; border: 1px solid #0F62AC; display: block;\">Verificar Correo</a>" +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr style=\"height: 30px;\">" + "</tr>" +
                                    "<tr>" +
                                        "<td align=\"left\" style=\"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding-top: 10px;\">" +
                                            "<p style=\"font-size: 16px; font-weight: 400; line-height: 24px; color: #777777;text-align: justify;\">¿Listo para empezar?</p>" +
                                            "<p style=\"font-size: 16px; font-weight: 400; line-height: 24px; color: #777777;text-align: justify;\">Gracias por unirte a SAG Autobuses.Estamos aquí para ayudarte, así que, si tienes cualquier pregunta, no dudes en enviar un mensaje de whatsapp o llamada al número <a style=\"color: #0F62AC;\" href=\"tel:+5255 2899 0715\"><span>55 2899 0715</span></a> o al correo<a style=\"color: #0F62AC;\" href=\"mailto:hola@sagautobuses.com\">hola@sagautobuses.com</a></p>" +
                                            "<p style=\"font-size: 16px; font-weight: 400; line-height: 24px; color: #777777;text-align: justify;\">Responderemos a todas tus preguntas.</p>" +
                                            "<p style=\"font-size: 16px; font-weight: 400; line-height: 24px; color: #777777;text-align: justify;\"></p>" +
                                            "<p style=\"font-size: 16px; font-weight: 400; line-height: 24px; color: #777777;text-align: justify;\"></p>" +
                                        "</td>" +
                                    "</tr>" +
                                "</table>" +
                            "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td align=\"center\" style= \" padding: 35px; background-color: #0F62AC;\">" +
                                "<table align=\"center\" border= \"0\" cellpadding= \"0\" cellspacing= \"0\" width= \"100%\" style= \"max-width:600px;\">" +
                                    "<tr>" +
                                        "<td align=\"center\" style= \"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding-top: 25px;\">" +
                                            "<h2 style=\"font-size: 24px; font-weight: 800; line-height: 30px; color: #ffffff; margin: 0;\">DESCARGA NUESTRA APP</h2>" +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td align=\"center\" style= \"padding: 25px 0 15px 0;\">" +
                                            "<table border=\"0\" cellspacing= \"0\" cellpadding= \"0\">" +
                                                "<tr>" +
                                                    "<td align=\"center\" style= \"border-radius: 5px;\">" +
                                                        "<a href=\"https://play.google.com/store/apps/details?id=com.gm.sagautobuses&hl=es_MX\" target= \"_blank\">" + "<img src=\"http://www.sagautobuses.com/assets/img/Logo-GooglePlay.png\" style=\"width: 200px;\">" + "</a>" +
                                                    "</td>" +
                                                    "<td style=\"column-count:4;column-gap:5px;column-rule:4px double black;list-style: none;\">" + "</td>" +
                                                    "<td align=\"center\" style= \"border-radius: 5px;\">" +
                                                        "<a href=\"https://apps.apple.com/mx/app/sag-autobuses/id1628424278?l=en\" target= \"_blank\">" + "<img src=\"http://www.sagautobuses.com/assets/img/Logo-Appstore.png\" style=\"width: 200px;\"/>" + "</a>" +
                                                    "</td>" +
                                                    "<td style=\"column-count:4;column-gap:5px;column-rule:4px double black;list-style: none;\">" + "</td>" +
                                                    "<td align=\"center\" style= \"border-radius: 5px;\">" +
                                                        "<a href=\"https://appgallery.huawei.com/app/C106487869\" target= \"_blank\">" + "<img src=\"http://www.sagautobuses.com/assets/img/Logo-AppGallery.png\" style=\"width: 200px;\"/>" + "</a>" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</table>" +
                                        "</td>" +
                                    "</tr>" +
                                "</table>" +
                            "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td align=\"center\" style= \"padding: 35px; background-color: #ffffff;\" bgcolor= \"#ffffff\">" +
                                "<table align=\"center\" border= \"0\" cellpadding= \"0\" cellspacing= \"0\" width= \"100%\" style= \"max-width:600px;\">" +
                                    "<tr>" +
                                        "<td align=\"center\">" +
                                            "<a href=\"http://sagautobuses.com/\" target= \"_blank\" style= \"color: #ffffff; text-decoration: none;\">" + "<img src= \"https://i.ibb.co/KqfxXcx/logo-footer.png\" width= \"120\" height= \"70\" style= \"display: block; border: 0px;\" />" + "</a>" +
                                        "</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                        "<td align=\"center\" style= \"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 24px; padding: 5px 0 10px 0;\">" +
                                            "<p style=\"font-size: 14px; font-weight: 800; line-height: 18px; color: #333333;\">Av.Politécnico Nacional 4912 Col.Maximino<br>Avila Camacho(21, 57 km) 07380 Ciudad de México, México </p>"+
                                        "</td>" +
                                    "</tr>" +
                                "</table>" +
                            "</td>" +
                        "</tr>" +
                    "</table>" +
                "</td>" +
            "</tr>" +
        "</table>" +
        "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">" +
            "<tr>" +
                "<td bgcolor=\"#ffffff\" align=\"center\">" + "</td>" +
            "</tr>" +
        "</table>" +
        "<!--END LITMUS ATTRIBUTION -->" +
    "</body>" +
"</html>";
            AlternateView htmlView =
            AlternateView.CreateAlternateViewFromString(msg,
                           Encoding.UTF8,
                           MediaTypeNames.Text.Html);
            return htmlView;
        }
    }
}
