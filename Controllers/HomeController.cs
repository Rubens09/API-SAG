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
using WebApplication1.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        #region Index
        [HttpPost("GetBanner")]
        public Banner GetBanner()
        {
            return Metodos.GetBanner();
        }
        [HttpPost("AltaVisita")]
        public string AltaVisita(string ciudad, string pais, string latitud, string longitud, string ip, string estado, string cp,string dm)
        {
            return Metodos.AltaVisita(ciudad,pais,latitud,longitud,ip,estado,cp,dm);
        }
        [HttpPost("AltaVisitaB")]
        public string AltaVisitaB(string ciudad, string pais, string latitud, string longitud, string ip, string estado, string cp, string dm,string url)
        {
            return Metodos.AltaVisitaB(ciudad, pais, latitud, longitud, ip, estado, cp, dm,url);
        }
        #endregion Index
        [HttpGet("VersionApp")] //Retorna Versión de app
        public ResVersion SAGVer()
        {
            ResVersion res = new ResVersion();
            res.version = Metodos.VersionSAG();
            return res;
        }
        [HttpPost("CanjePromocion")]
        public string CanjePromocion(string codigo_promo,string internet_sale_id)
        {
            return Metodos.CanjePromocion(codigo_promo,internet_sale_id);
        }
        [HttpPost("CerrarConversacion")]
        public string CerrarConversacion(string id_chat,string id_usuario)
        {
            return Metodos.CerrarConversacion(id_chat,id_usuario);
        }
        [HttpPost("GetBoleto")]
        public string GetBoleto(string internet_sale_id)
        {
            return Metodos.GetBoleto(internet_sale_id);
        }
        [HttpPost("GetChatBot")]
        public string GetChatBot()
        {
            return Metodos.GetChatBot();
        }
        [HttpPost("GetMsgChatBot")]
        public string GetMsgChatBot(string id_chat)
        {
            return Metodos.GetMsgChatBot(id_chat);
        }
        [HttpPost("AltaChatbot")]
        public string AltaChatBot(Conversacion listaMensajes)
        {
            return Metodos.AltaChatBot(listaMensajes);
        }
        [HttpPost("InsertData")]
        public string InsertData(string salida, string llegada, string internet_sale, string short_id, string metodo_pago)
        {
            return Metodos.InsertData(salida,llegada,internet_sale,short_id,metodo_pago);
        }
        [HttpGet("ConsultarAsientos/{InternetSaleId}/{OriginId}/{DestinId}")]
        public ListaAsientos ConsultarAsientos(string InternetSaleId, string OriginId, string DestinId)
        {
            return Metodos.ConsultaAsientos(InternetSaleId, OriginId, DestinId);
        }

        [HttpPost("LiberaAsientos")]
        public string LiberaAsientos(AsientosPasajero listaPasajeros)
        {
            return Metodos.DesocupaAsientos(listaPasajeros);
        }
        [HttpPost("PagoPaypal")]
        public string PagoPaypal(string origen, string destino, string boletos, string monto, string short_id)
        {
            return Metodos.PagoPaypal(origen,destino,boletos,monto,short_id);
        }
        [HttpPost("CapturaPaypal")]
        public string CapturaPaypal(string id,string access_token,string boletos,string monto)
        {
            return Metodos.CapturaPaypal(id,access_token,boletos,monto);
        }
        [HttpGet("RevisarPaypal")]
        public string RevisarPaypal(string id, string access_token)
        {
            return Metodos.RevisarPaypal(id, access_token);
        }

        [HttpPost("CreaTicketAsientos")]
        public string CreaTicketAsientos(AsientosPasajero listaPasajeros)
        {
            return Metodos.ActualizaAsientos(listaPasajeros);
        }

        [HttpPost("RespuestaOpenPay")]
        public IActionResult Webhook(RespuestaWebhook msg)
        {
            return Ok();
        }

        [HttpPost("GuardaReservaciones")]
        public string GuardaReservacions(AsientosPasajero listaPasajeros)
        {
            return Metodos.GuardaReservacions(listaPasajeros);
        }
        [HttpPost("AltaUsuario")]
        public ResAlta AltaUsuario(InfoUsuario listaInformacion)
        {
            ResAlta res=new ResAlta();
            res.respuesta= Metodos.AltaUsuario(listaInformacion);
            return res;
        }
        [HttpPost("CanjeCodigo")]
        public ResCanje CanjeCodigo(InfoUsuarioInvitado listaInvitado)
        {
            ResCanje res=new ResCanje();
            res.respuesta=Metodos.CanjeCodigo(listaInvitado);
            return res;
        }
        [HttpPost("CanjeCodigoW")]
        public ResCanje CanjeCodigoW(InfoUsuarioInvitado listaInvitado)
        {
            ResCanje res = new ResCanje();
            res.respuesta = Metodos.CanjeCodigoW(listaInvitado);
            return res;
        }
        [HttpPost("InsertaDatosViaje")]
        public string InsertarDatosViaje(DatosPasajero datosPasajero)
        {

            return Metodos.InsertarDatosViaje(datosPasajero);
        }
        [HttpPost("GenerarSesion")]
        public string GetSesion(infoSesion infoSesion)
        {
            return Metodos.GetSesion(infoSesion);
        }
        [HttpPost("GenerarSesionW")]
        public string GetSesionW(infoSesionMC infoSesion)
        {
            return Metodos.GetSesionW(infoSesion);
        }
        [HttpPost("UpdateBoleto")]
        public string UpdateBoleto(string shortId, string sale_id, string amount)
        {
            return Metodos.UpdateBoleto(shortId, sale_id,amount);
        }
        [HttpPost("UpdateBoletoMC")]
        public string UpdateBoletoMC(string shortId, string sale_id)
        {
            return Metodos.UpdateBoletoMC(shortId, sale_id);
        }
        [HttpPost("GetPagoMasterCard")]
        public string GetPagoMasterCard(string short_id, string key)
        {
            return Metodos.GetPagoMasterCard(short_id,key);
        }
        #region
        [HttpPost("AltaRuleta")]
        public Respuesta AltaRuleta(InfoRuleta listaRuleta)
        {
            return Metodos.AltaRuleta(listaRuleta);
        }
        [HttpPost("ListarEstados")]
        public ListaEstado ListarEstados()
        {
            return Metodos.ListarEstados();
        }
        [HttpPost("ValidarCorreoRuleta")]
        public Respuesta ValidarCorreoRuleta(CorreoRuleta Correo)
        {
            return Metodos.ValidarCorreoRuleta(Correo);            
        }
        [HttpPost("ObtenerDescuento")]
        public Respuesta ObtenerDescuento()
        {
            return Metodos.ObtenerDescuento();
        }
        [HttpGet("ValidarPagos/{ShortId}")]
        public ListaPagos consultarPago(string ShortId)
        {
            return Metodos.ConsultaPago(ShortId);
        }
        #endregion
        #region RefiereGana
        [HttpPost("ObtenerCantidad")]
        public AcumuladoRefiereGana ObtenerCantidad(CorreoRefiereGana correoRefiereGana)
        {
            return Metodos.ExtraerCantidadAcumulada(correoRefiereGana);
        }

        [HttpPost("CanjeRefiereGana")]
        public string RefiereGana(CanjeRefiereGana canjeRefiereGana)
        {
            return Metodos.InsertarRefiereGana(canjeRefiereGana);
        }
        [HttpPost("ActualizarConceptoPago")]
        public string ActualizarConceptoPago(infoCambio listacambio)
        {
            return Metodos.ActualizarConceptoPago(listacambio);
        }
        [HttpPost("ListarCategorias")]
        public string ListarCategorias()
        {
            return Metodos.ListarCategorias();
        }
        [HttpPost("AltaCategoria")]
        public string AltaCategoria(AltaCat listaCat)
        {
            return Metodos.AltaCategoria(listaCat);
        }
        [HttpPost("EliminarCategoria")]
        public string EliminarCategoria(EliminarCat listaCat)
        {
            return Metodos.EliminarCategoria(listaCat);
        }
        [HttpPost("ModificarrCategoria")]
        public string ModificarCategoria(ModificarCat listaCat)
        {
            return Metodos.ModificarCategoria(listaCat);
        }
        [HttpPost("LoginSAG")]
        public string LoginSAG(Login listaLogin)
        {
            return Metodos.LoginSAG(listaLogin);
        }
        [HttpPost("ActualizarImagenes")]
        public string ActualizarImagenes(Imagenes listaimagenes)
        {
            return Metodos.ActualizarImagenes(listaimagenes);
        }
        [HttpPost("ListarPosts")]
        public string ListarPost()
        {
            return Metodos.ListarPosts();
        }
        [HttpPost("GetPost")]
        public string GetPost(Post listapost)
        {
            return Metodos.GetPost(listapost);
        }
        #region Admin
        [HttpPost("GetVisitaFecha")]
        public string GetVisitaFecha(string opcion)
        {
            return Metodos.GetVisitaFecha(opcion);
        }
        [HttpPost("GetCompras")]
        public string GetCompras()
        {
            return Metodos.GetCompras();
        }
        #endregion Admin
        public class Post
        {
            public string Id { get; set; }
        }
        public class Imagenes
        {
            public string imagen1 { get; set; }
            public string imagen2 { get; set; }
            public string imagen3 { get; set; }
        }
        public class Login
        {
            public string usuario { get; set; }
            public string pasword { get; set; }
        }
        public class AltaCat
        {
            public string Categoria { get; set; }
        }
        public class EliminarCat
        {
            public string id { get; set; }
        }
        public class ModificarCat
        {
            public string Id { get; set; }
            public string Categoria { get; set; }
            public string Activo {get;set; }
        }
        public class idopenpay
        {
            public int Id{ get; set; }
        }
        public class CorreoRefiereGana
        {
            public string Correo { get; set; }
        }


        public class AcumuladoRefiereGana
        {
            public double Acumulado { get; set; }
        }

        public class CanjeRefiereGana
        {
            public string intenetSaleID { get; set; }

            public string email { get; set; }
        }
        #endregion
        public class ResVersion
        {
            public string version { get; set; }
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
        public class CorreoRuleta
        {
            public string Correo { get; set; }
        }
        public class ResAlta{
            public string respuesta { get; set; }
        }
        public class ResCanje
        {
            public string respuesta { get; set; }
        }
        public class Empleado
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
        }

        public class Usuario
        {
           
            public string userId { get; set; }
            public string id { get; set; }
            public string title { get; set; }
            public string body { get; set; }
        }

        public class CatSalida
        {
            public string Salida { get; set; }
        }

        public class AsientosPasajero
        {
            public List<Pasajeros> ListaPasajeros { get; set; }
            public string OrigenId { get; set; } //starting_stop_id
            public string DestinoId { get; set; } //ending_stop_id
            public string InterteSaleId { get; set; } //internet_sale_id
            public string TipoPago { get; set; } //internet_sale_id
            public string IDPago { get; set; }
            public string FechaSalida { get; set; }
            public string FechaLlegada { get; set; }
        }
        public class InfoUsuario
        {
            public string Nombre { get; set; }
            public string apellidos { get; set; }
            public string correo { get; set; }
            public string telefono { get; set; }
        }
        public class InfoUsuarioInvitado
        {
            public string Nombre { get; set; }
            public string apellidos { get; set; }
            public string correo { get; set; }
            public string telefono { get; set; }
            public string promo { get; set; }
        }
        public class Pasajeros
        {
            public string Nombre { get; set; } //passenger_name
            public string Asiento { get; set; } //seat_name
          

        }
        public class ListaAsientos
        {
            public List<AsientosAsignados> Listado{ get; set; }
        }

        public class AsientosAsignados
        {
            public string TicketID { get; set; }
            public string Asiento { get; set; }
        }

        public class ListaPagos
        {
            public List<PagosRealizados> Pagados { get; set; }
        }

        public class PagosRealizados
        {
            public string Status { get; set; }
            public string ShortId { get; set; }
            public string Payed { get; set; }
            public string Ticket { get; set; }

        }   


        public class RespuestaWebhook
        {
            public string id { get; set; }
            public DateTime create_time { get; set; }
            public string resource_type { get; set; }
            public string event_type {get;set;}
            public string sumary { get; set; }

            public resource resource { get; set; }
            public string status { get; set; }
            public List<transmissions> transmissions { get; set; }

            public List<links2> links { get; set; }
            public string event_version { get; set; }
            public string resource_version { get; set; }

        }

        public class transmissions
        {
            public string webhook_url { get; set; }
            public string transmission_id { get; set; }
            public string status { get; set; }
            public DateTime timestamp { get; set; }
        }

        public class resource
        {
            public amount amount { get; set; }
            public seller_protection seller_protection { get; set; }
            public DateTime update_time { get; set; }
            public DateTime create_time { get; set; }
            public bool final_capture { get; set; }

            public seller_receivable_breakdown seller_receivable_breakdown { get; set; }
            public List<links> links { get; set; }
            public string id { get; set; }
            public string status { get; set; }

        }
        public class amount
        {
            public string value { get; set; }
            public string currency_code { get; set; }
        }

        public class seller_protection
        {
         public List<string> dispute_categories { get; set; }
        public string status { get; set; }

        }

        public class seller_receivable_breakdown
        {
            public paypal_fee paypal_fee { get; set; }
            public gross_amount gross_amount { get; set; }
            public net_amount net_amount { get; set; }
        }

        public class paypal_fee
        {
            public string value { get; set; }
            public string currency_code { get; set; }
        }
        public class gross_amount
        {
            public string value { get; set; }
            public string currency_code { get; set; }
        }
        public class net_amount
        {
            public string value { get; set; }
            public string currency_code { get; set; }
        }

        public class links
        {
            public string method { get; set; }
            public string rel { get; set; }
            public string href { get; set; }

        }
        public class links2
        {
            public string href { get; set; }
            public string rel { get; set; }
            public string method { get; set; }
            public string encType { get; set; }

        }
        public class DatosPasajero
        {
            public string InternetSaleId { get; set; }
            public string NombrePasajero { get; set; }
            public string Email { get; set; }
            public string Origen { get; set; }
            public string Destino { get; set; }
            public string Salida { get; set; }
            public string Llegada { get; set; }

        }
        public class Listasesion
        {
            public List<sesions> infosesion { get; set; }
        }
        public class sesions
        {
            public string short_id { get; set; }
            public string session_id { get; set; }
        }
        public class infoSesion
        {
            public string descripcion { get; set; }
            public string monto { get; set; }
            public string short_id { get; set; }
        }
        public class infoSesionMC
        {
            public string descripcion { get; set; }
            public string monto { get; set; }
            public string short_id { get; set; }
            public string internet_sale_id { get; set; }
        }
        public class infoCambio
        {
            public string short_id { get; set; }
            public string descripcion { get; set; }
        }
        #region
        public class InfoRuleta
        {
            public string correo { get; set; }
            public string telefono { get; set; }
            public string nombre { get; set; }
            public string primerApellido { get; set; }
            public string segundoApellido { get; set; }
            public DateTime fechaNacimiento { get; set; }
            public string origen { get; set; }
            public string destino { get; set; }
            public int idEstado { get; set; }
            public string descuento { get; set; }
        }
        public class ListaEstado
        {
            public List<Estados> Estado { get; set; }
        }
        public class Estados
        {
            public string Id { get; set; }
            public string Nombre { get; set; }
        }
        public class Respuesta
        {
            public List<Resultado> resultado { get; set; }
        }
        public class Resultado
        {
            public string status { get; set; }
        }
        public class Banner
        {
            public List<Listado> listado { get; set; }
        }
        public class Listado
        {
            public string url { get; set; }
            public string dispositivo { get; set; }
        }
        #endregion
        #region ChatBot
        public class Conversacion
        {
            public string asunto { get; set; }
            public string nombre { get; set; }
            public string telefono { get; set; }
            public string correo { get; set; }
            public string descripcion { get; set; }
            public List<Mensajes> mensaje { get; set; }
        }
        public class Mensajes
        {
            public string persona { get; set; }
            public string mensaje { get; set; }
        }
        #endregion ChatBot
    }
}
