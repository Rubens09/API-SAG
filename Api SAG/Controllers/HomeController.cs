using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NodaTime;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Api_SAG.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace Api_SAG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        #region CuentaUsuario
        [HttpPost("LogIn")]
        public string LogIn(string correo_telefono, string password)
        {
            return Metodos.LogIn(correo_telefono, password);
        }
        [HttpPost("proceso_correo")]
        public string proceso_correo(string correo_anterior, string correo_nuevo)
        {
            return Metodos.proceso_correo(correo_anterior,correo_nuevo);
        }
        [HttpPost("Listar_notificaciones")]
        public string Listar_notificaciones(string id)
        {
            return Metodos.Listar_notificaciones(id);
        }
        [HttpPost("Lectura_notificacion")]
        public string Lectura_notificacion(string id,string id_user)
        {
            return Metodos.Lectura_notificacion(id, id_user);
        }
        [HttpPost("verificar_correo")]
        public string verificar_correo(string correo, string fecha_nacimiento)
        {
            return Metodos.verificar_correo(correo, fecha_nacimiento);
        }
        [HttpPost("proceso_verificar")]
        public string proceso_verificar(string correo, string codigo)
        {
            return Metodos.proceso_verificar(correo, codigo);
        }
        [HttpPost("SignUp")]
        public string SignUp(string nombre, string apellidos, string correo, string password, string telefono, string fecha_cumpleanos, string genero)
        {
            return Metodos.SignUp(nombre,apellidos,correo,password,telefono,fecha_cumpleanos,genero);
        }
        [HttpPost("update_information")]
        public string update_information(string nombre, string first_name, string second_name, string phone_number, string email)
        {
            return Metodos.update_information(nombre, first_name, second_name, phone_number, email);
        }
        [HttpPost("update_password")]
        public string update_password(string password, string email)
        {
            return Metodos.update_password(password, email);
        }
        #region Clases
        public class list_LogIn
        {
            public List<user_data> listado { get; set; }
        }
        public class user_data
        {
            public string id { get; set; }
            public string user_names { get; set; }
            public string surname { get; set; }
            public string second_name { get; set; }
            public string phone_number { get; set; }
            public string birth_date { get; set; }
            public string gender { get; set; }
            public string email { get; set; }
            public string status { get; set; }
        }
        #endregion Clases
        #endregion CuentaUsuario
        #region Principal
        [HttpPost("UpdateBoleto")]
        public string UpdateBoleto(string shortId, string sale_id, string amount)
        {
            return Metodos.UpdateBoleto(shortId, sale_id, amount);
        }
        [HttpPost("UpdateBoletoMC")]
        public string UpdateBoletoMC(string shortId, string sale_id)
        {
            return Metodos.UpdateBoletoMC(shortId, sale_id);
        }
        [HttpPost("GetBoleto")]
        public string GetBoleto(string internet_sale_id,string origen,string destino)
        {
            return Metodos.GetBoleto(internet_sale_id,origen,destino);
        }
        [HttpPost("List_trips")]
        public string List_trips(string email)
        {
            return Metodos.List_trips(email);
        }
        [HttpPost("List_tickets")]
        public string List_tickets(string email)
        {
            return Metodos.List_tickets(email);
        }
        #endregion Principal
        #region PagoMastercard
        //
        [HttpPost("GenerarSesionW")]
        public string GetSesionW(infoSesionMC infoSesion)
        {
            return Metodos.GetSesionW(infoSesion);
        }
        #region Clases
        public class infoSesionMC
        {
            public string descripcion { get; set; }
            public string monto { get; set; }
            public string short_id { get; set; }
            public string internet_sale_id { get; set; }
        }
        public class Root
        {
            public string merchant { get; set; }
            public string result { get; set; }
            public Session session { get; set; }
            public string successIndicator { get; set; }
        }

        public class Session
        {
            public string id { get; set; }
            public string updateStatus { get; set; }
            public string version { get; set; }
        }
        #endregion Clases
        [HttpPost("ActualizarMasterCard")]
        public string ActualizarMasterCard(string short_id,string descripcion)
        {
            return Metodos.ActualizarMasterCard(short_id,descripcion);
        }
        [HttpGet("RevisarPaypal")]
        public string RevisarPaypal(string id, string access_token)
        {
            return Metodos.RevisarPaypal(id, access_token);
        }
        #endregion PagoMasterCard
        #region Index
        [HttpPost("GetBanner")]
        public Banner GetBanner()
        {
            return Metodos.GetBanner();
        }
        #region Clases
        public class Banner
        {
            public List<Listado> listado { get; set; }
        }
        public class Listado
        {
            public string url { get; set; }
            public string dispositivo { get; set; }
        }
        #endregion Clases
        #endregion Index
        #region PayPal
        [HttpPost("CapturaPaypal")]
        public string CapturaPaypal(string id, string access_token, string boletos, string monto)
        {
            return Metodos.CapturaPaypal(id, access_token, boletos, monto);
        }
        [HttpPost("PagoPaypal")]
        public string PagoPaypal(string origen, string destino, string boletos, string monto, string short_id,string descripcion)
        {
            return Metodos.PagoPaypal(origen, destino, boletos, monto, short_id,descripcion);
        }
        [HttpPost("ActualizarConceptoPago")]
        public string ActualizarConceptoPago(infoCambio listacambio)
        {
            return Metodos.ActualizarConceptoPago(listacambio);
        }
        #endregion
        #region PagoEfectivo
        [HttpPost("ActualizarEfectivo")]
        public string ActualizarEfectivo(string descripcion, string monto, string short_id, string internet_sale_id, string sales_terminal_id, string salesman_id)
        {
            return Metodos.ActualizarEfectivo(descripcion,monto,short_id,internet_sale_id,sales_terminal_id,salesman_id);
        }
        #endregion PagoEfectivo
        #region Clases
        public class infoCambio
        {
            public string short_id { get; set; }
            public string descripcion { get; set; }
        }
        #endregion Clases
        #region Blog
        [HttpGet("GetPost")]
        public string GetPost()
        {
            return Metodos.GetPost();
        }
        [HttpPost("ViewPost")]
        public string viewPost(string id_post)
        {
            return Metodos.ViewPost(id_post);
        }
        #endregion Blog
    }
}
