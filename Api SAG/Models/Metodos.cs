using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using static Api_SAG.Controllers.HomeController;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using JavaScriptEngineSwitcher.Jurassic;
using JavaScriptEngineSwitcher.Core;
using Microsoft.AspNetCore.Http;
using System.Security.Policy;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using PayPal.Api;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using ConsultaWebApisPagos;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Routing;
using System.Xml;

namespace Api_SAG.Models
{
    public class Metodos
    {
        #region CuentaUsuario
        public static string GetMD5(string str)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
        public static string LogIn(string correo_telefono, string password)
        {
            var res = "";
            int tick = 0, cont = 0;
            string qry = "", qryi = "", ticketid = GenShortId(), qry2 = "";
            DataTable tbl = new DataTable();
            DataTable tbl2 = new DataTable();
            qry2 = "SELECT * FROM cat_users WHERE email='" + correo_telefono + "' OR phone_number='"+correo_telefono+"'";
            tbl = mandaQry(qry2);
            if ((tbl.Rows.Count) > 0)
            {
                //password = GetMD5(password);
                qry2 = "SELECT * FROM cat_users WHERE (email='" + correo_telefono + "' OR phone_number='" + correo_telefono + "') AND user_password='"+password+"'";
                tbl = mandaQry(qry2);
                if ((tbl.Rows.Count) > 0)
                {
                    list_LogIn Lista = new list_LogIn();
                    Lista.listado = new List<user_data>();
                    foreach (DataRow row in tbl.Rows)
                    {
                        res = "{\"id\":\""+ row["id"].ToString() + "\",\"user_names\":\""+ row["user_names"].ToString() + "\",\"surname\":\""+ row["surname"].ToString() + "\",\"second_surname\":\""+ row["second_surname"].ToString() + "\",\"phone_number\":\""+ row["phone_number"].ToString() + "\",\"birth_date\":\""+ Convert.ToDateTime(row["birth_date"]).ToString("yyyy-MM-dd") + "\",\"gender\":\""+ row["gender"].ToString() + "\",\"email\":\""+ row["email"].ToString() + "\",\"status\":\""+ row["status"].ToString() + "\"}";
                    }
                }
                else
                {
                    res = "{\"respuesta\":\"Contraseña Incorrecta\"}";
                }
            }
            else
            {
                res = "{\"respuesta\":\"No existe cuenta con este correo o número\"}";
            }
            return res;
        }
        public static string update_information(string nombre, string first_name, string second_name,string phone_number, string email)
        {
            var res = "";
            string qry2 = "";
            DataTable tbl = new DataTable();
            qry2 = "UPDATE cat_users SET user_names='"+nombre+"', surname='"+first_name+"', second_surname='"+second_name+"', phone_number='"+phone_number+"',last_updated='"+new DateTime().ToString("yyyy-MM-dd")+"' WHERE email='"+email+"';";
            try
            {
                tbl = mandaQry(qry2);
                res = "{\"respuesta\":\"Se ha realizado las modificaciones\"}";
            }
            catch
            {
                res = "{\"respuesta\":\"Error al realizar la modificacion\"}";
            }
            return res;
        }
        public static string update_password(string password, string email)
        {
            var res = "";
            string qry2 = "";
            DataTable tbl = new DataTable();
            qry2 = "UPDATE cat_users SET user_password='" + password + "' WHERE email='" + email + "';";
            try
            {
                tbl = mandaQry(qry2);
                res = "{\"respuesta\":\"Se ha realizado las modificaciones\"}";
            }
            catch
            {
                res = "{\"respuesta\":\"Error al realizar la modificacion\"}";
            }
            return res;
        }
        public static string List_trips(string email)
        {
            var res = "";
            int tick = 0, cont = 0;
            string qry = "", qry2 = "";
            DataTable tbl = new DataTable();
            DataTable tbl2 = new DataTable();
            qry2 = "SELECT t1.bill as factura,t2.status as estatus,t1.id,t1.short_id as numero,t1.payment_provider as metodo_pago,t1.payment_provider as plataforma,t3.name as origen,t4.name as destino,t5.description as ruta,t6.departure_date as fecha_salida,COUNT(t2.internet_sale_id) as numero_boleto,t1.total_amount as monto FROM internet_sale t1 " +
            "INNER JOIN trip_seat t2 ON t1.id=t2.internet_sale_id "+
            "INNER JOIN stop_off t3 ON t2.starting_stop_id=t3.id "+
            "INNER JOIN stop_off t4 ON t2.ending_stop_id=t4.id "+
            "INNER JOIN route t5 ON t3.route_id=t5.id "+
            "INNER JOIN trip t6 ON t6.id=t2.trip_id "+
            "WHERE t1.email='"+email+"' AND t1.payed=true "+
            "GROUP BY t1.id,t1.short_id,t1.payment_provider,t1.payment_provider,t3.name,t4.name,t5.description,t6.departure_date,t2.internet_sale_id,t1.bill,t2.status,t1.total_amount ORDER BY t1.date_Created ASC";
            tbl = mandaQry(qry2);
            if ((tbl.Rows.Count) > 0)
            {
                foreach (DataRow row in tbl.Rows)
                {
                    string api = GetBoleto(row["id"].ToString(), row["origen"].ToString(), row["destino"].ToString());
                    api=api.Replace("[","");
                    api=api.Replace("]", "");
                    class_list_trip myDeserializedClass = JsonConvert.DeserializeObject<class_list_trip>(api);
                    //class_list_trip myDeserializedClass = JsonConvert.DeserializeObject<class_list_trip>(api);
                    if(cont>0)
                        res = res + ",{\"metodo_pago\":\"" + row["metodo_pago"].ToString() +"\",\"factura\":\"" + row["factura"].ToString() + "\",\"estatus\":\"" + row["estatus"].ToString() +"\",\"numero_compra\":\"" + row["numero"].ToString() + "\",\"origen\":\"" + myDeserializedClass.origen + "\",\"destino\":\"" + myDeserializedClass.destino + "\",\"ruta\":\"" + row["ruta"] + "\",\"salida\":\"" + myDeserializedClass.fechaSalida + "\",\"llegada\":\"" + myDeserializedClass.fechaLlegada + "\",\"numero_boletos\":\"" + row["numero_boleto"].ToString() +"\",\"monto\":\"" + row["monto"].ToString() +"\"}";
                    else
                        res = "{\"metodo_pago\":\"" + row["metodo_pago"].ToString() +"\",\"factura\":\"" + row["factura"].ToString() + "\",\"estatus\":\"" + row["estatus"].ToString() +"\",\"numero_compra\":\"" + row["numero"].ToString() + "\",\"origen\":\"" + myDeserializedClass.origen + "\",\"destino\":\"" + myDeserializedClass.destino + "\",\"ruta\":\"" + row["ruta"] + "\",\"salida\":\"" + myDeserializedClass.fechaSalida + "\",\"llegada\":\"" + myDeserializedClass.fechaLlegada + "\",\"numero_boletos\":\"" + row["numero_boleto"].ToString() +"\",\"monto\":\"" + row["monto"].ToString() +"\"}";
                    cont++;
                }
                res = "[" + res + "]";
            }
            else
            {
                res = "{\"respuesta\":\"No existe registros\"}";
            }
            return res;
        }
        public static string List_tickets(string email)
        {
            var res = "";
            int tick = 0, cont = 0;
            string qry = "", qry2 = "";
            DataTable tbl = new DataTable();
            DataTable tbl2 = new DataTable();
            qry2 = "SELECT t1.id,t1.total_amount as monto,t2.status as estatus,t1.short_id as numero_compra,t1.payment_provider as metodo_pago,t3.name as origen,t4.name as destino,t5.description as ruta,t6.departure_date as fecha_salida,t2.ticket_id as folio,t2.passenger_name as nombre_pasajero,t2.seat_name as numero_asiento,t2.passenger_type as tipo_pasajero FROM internet_sale t1 "+
            "INNER JOIN trip_seat t2 ON t1.id=t2.internet_sale_id "+
            "INNER JOIN stop_off t3 ON t2.starting_stop_id=t3.id "+
            "INNER JOIN stop_off t4 ON t2.ending_stop_id=t4.id "+
            "INNER JOIN route t5 ON t3.route_id=t5.id "+
            "INNER JOIN trip t6 ON t6.id=t2.trip_id "+
            "WHERE email='"+email+"'";
            tbl = mandaQry(qry2);
            if ((tbl.Rows.Count) > 0)
            {
                foreach (DataRow row in tbl.Rows)
                {
                    string api = GetBoleto(row["id"].ToString(), row["origen"].ToString(), row["destino"].ToString());
                    api = api.Replace("[", "");
                    api = api.Replace("]", "");
                    class_list_trip myDeserializedClass = JsonConvert.DeserializeObject<class_list_trip>(api);
                    //class_list_trip myDeserializedClass = JsonConvert.DeserializeObject<class_list_trip>(api);
                    if (cont > 0)
                        res = res + ",{\"metodo_pago\":\"" + row["metodo_pago"].ToString() + "\",\"estatus\":\"" + row["estatus"].ToString() + "\",\"numero_compra\":\"" + row["numero_compra"].ToString() + "\",\"origen\":\"" + myDeserializedClass.origen + "\",\"destino\":\"" + myDeserializedClass.destino + "\",\"ruta\":\"" + row["ruta"] + "\",\"salida\":\"" + myDeserializedClass.fechaSalida + "\",\"llegada\":\"" + myDeserializedClass.fechaLlegada + "\",\"numero_boleto\":\"" + row["folio"].ToString() + "\",\"monto\":\"" + row["monto"].ToString() + "\",\"nombre_pasajero\":\"" + row["nombre_pasajero"] + "\",\"numero_asiento\":\"" + row["numero_asiento"] + "\",\"tipo_pasajero\":\"" + row["tipo_pasajero"] +"\"}";
                    else
                        res = "{\"metodo_pago\":\"" + row["metodo_pago"].ToString() + "\",\"estatus\":\"" + row["estatus"].ToString() + "\",\"numero_compra\":\"" + row["numero_compra"].ToString() + "\",\"origen\":\"" + myDeserializedClass.origen + "\",\"destino\":\"" + myDeserializedClass.destino + "\",\"ruta\":\"" + row["ruta"] + "\",\"salida\":\"" + myDeserializedClass.fechaSalida + "\",\"llegada\":\"" + myDeserializedClass.fechaLlegada + "\",\"numero_boleto\":\"" + row["folio"].ToString() + "\",\"monto\":\"" + row["monto"].ToString() + "\",\"nombre_pasajero\":\"" + row["nombre_pasajero"] + "\",\"numero_asiento\":\"" + row["numero_asiento"] + "\",\"tipo_pasajero\":\"" + row["tipo_pasajero"] + "\"}";
                    cont++;
                }
                res = "[" + res + "]";
            }
            else
            {
                res = "{\"respuesta\":\"No existe registros\"}";
            }
            return res;
        }
        #region Clase_list_trips
        public class class_list_trip
        {
            public string origen { get; set; }
            public string destino { get; set; }
            public string asiento { get; set; }
            public string nombre { get; set; }
            public string fechaSalida { get; set; }
            public string fechaLlegada { get; set; }
            public string total { get; set; }
            public string precioVenta { get; set; }
            public string idTicket { get; set; }
            public string tipoPasajero { get; set; }
            public string tipopasajero { get; set; }
        }
        #endregion Clase_list_trips
        public static string proceso_correo(string correo_anterior, string correo_nuevo)
        {
            var res = "";
            string qry = "", qryi = "", ticketid = GenShortId(), qry2 = "", qry3 = "";
            DataTable tbl = new DataTable();
            DataTable tbl3 = new DataTable();
            string correo = "";
            if (correo_nuevo != "" && correo_nuevo != null)
            {
                qry3 = "SELECT * FROM cat_users WHERE email='" + correo_nuevo + "'";
                tbl3 = mandaQry(qry3);
                if ((tbl3.Rows.Count) == 0)
                {
                    qry2 = "UPDATE cat_users SET email='" + correo_nuevo + "' WHERE email='" + correo_anterior + "'";
                    try
                    {
                        tbl = mandaQry(qry2);
                        correo = correo_nuevo;
                    }
                    catch { }
                }
                else
                {
                    res = "{\"respuesta\":\"Ya existe el nuevo correo electronico.\"}";
                }
            }
            else
            {
                correo = correo_anterior;
            }
            int tick = 0, cont = 0;
            DataTable tbl2 = new DataTable();
            qry2 = "SELECT * FROM cat_users WHERE email='" + correo + "'";
            tbl = mandaQry(qry2);
            if ((tbl.Rows.Count) > 0)
            {
                foreach (DataRow row in tbl.Rows)
                {
                    try
                    {
                        Correo.EnviaCorreo(correo, Convert.ToString(row["user_names"]), Convert.ToString(row["surname"])+" "+ Convert.ToString(row["second_surname"]), Convert.ToString(row["verifier_code"]));
                        res = "{\"respuesta\":\"Envio de correo correcto.\"}";
                    }
                    catch { }
                }
            }
            else
            {
                res = "{\"respuesta\":\"Error al enviar correo, vuelve a intentarlo mas tarde.\"}";
            }
            return res;
        }
        public static string verificar_correo(string correo, string fecha_nacimiento)
        {
            var res = "";
            string qry = "";
            DataTable tbl = new DataTable();
            qry = "SELECT * FROM cat_users WHERE email='" + correo + "' AND birth_date='" + fecha_nacimiento + "'";
            try
            {
                tbl = mandaQry(qry);
                if ((tbl.Rows.Count) > 0)
                {
                    foreach (DataRow row in tbl.Rows)
                    {
                        res = "{\"respuesta\":\"" + row["status"].ToString() + "\"}";
                    }
                }
                else
                {
                    res = "{\"respuesta\":\"0\"}";
                }
            }
            catch
            {
                res = "{\"respuesta\":\"0\"}";
            }
            return res;
        }
        public static string Lectura_notificacion(string id,string id_user)
        {
            var res = "";
            string qry = "";
            DataTable tbl = new DataTable();
            qry = "UPDATE user_notification SET estatus=1 WHERE id IN(";
            var ids_temp = id.Split(',');
            if (ids_temp.Length > 0)
            {
                for (int x = 0; x < ids_temp.Length; x++)
                {
                    if (x == 0)
                        qry = qry + ids_temp[x];
                    else
                        qry = qry + "," + ids_temp[x];
                }
                qry = qry + ") AND id_user<>'0' AND id_user='" + id_user + "'";
                try
                {
                    tbl = mandaQry(qry);
                    res = "{\"respuesta\":\"1\"}";
                }
                catch
                {
                    res = "{\"respuesta\":\"0\"}";
                }
            }
            else
            {
                res = "{\"respuesta\":\"0\"}";
            }
            return res;
        }
        public static string Listar_notificaciones(string id)
        {
            var res = "";
            string qry = "";
            DataTable tbl = new DataTable();
            qry = "SELECT * FROM user_notification WHERE (id_user='" + id + "' OR id_user='0') AND estatus='0'";
            int cont = 0;
            try
            {
                tbl = mandaQry(qry);
                if ((tbl.Rows.Count) > 0)
                {
                    foreach (DataRow row in tbl.Rows)
                    {
                        if (cont == 0)
                        {
                            res = "{\"id\":\"" + row["id"] + "\",\"title\":\"" + row["title"].ToString() + "\",\"message\":\"" + row["message"] + "\",\"action\":\"" + row["action"] + "\"}";
                        }
                        else
                        {
                            res = res + ",{\"id\":\"" + row["id"] + "\",\"title\":\"" + row["title"].ToString() + "\",\"message\":\"" + row["message"] + "\",\"action\":\"" + row["action"] + "\"}";
                        }
                        cont++;
                    }
                    res = "[" + res + "]";
                }
                else
                {
                    res = "{\"respuesta\":\"0\"}";
                }
            }
            catch
            {
                res = "{\"respuesta\":\"0\"}";
            }
            return res;
        }
        public static string proceso_verificar(string correo, string codigo)
        {
            string qry = "", qryi = "", ticketid = GenShortId(), qry2 = "";
            DataTable tbl = new DataTable();
            var res = "";
            int tick = 0, cont = 0;
            DataTable tbl2 = new DataTable();
            qry2 = "SELECT * FROM cat_users WHERE email='" + correo + "' AND verifier_code='"+codigo+"'";
            tbl = mandaQry(qry2);
            if ((tbl.Rows.Count) > 0)
            {
                try
                {
                    qry = "UPDATE cat_users SET verifier_code='',status='2' WHERE email='" + correo + "' AND verifier_code='" + codigo + "'";
                    try
                    {
                        tbl2 = mandaQry(qry);
                        res = "{\"respuesta\":\"Se válido el correo.\"}";
                    }
                    catch
                    {
                        res = "{\"respuesta\":\"Error al verificar correo, vuelve a intentarlo mas tarde.\"}";
                    }
                }
                catch {
                    res = "{\"respuesta\":\"Error al verificar correo, vuelve a intentarlo mas tarde.\"}";
                }                
            }
            else
            {
                res = "{\"respuesta\":\"Error al verificar correo, vuelve a intentarlo mas tarde.\"}";
            }
            return res;
        }
        public static string SignUp(string nombre, string apellidos,string correo,string password,string telefono,string fecha_cumpleanos,string genero)
        {
            var res = "";
            int tick = 0, cont = 0;
            string qry = "", qryi = "", ticketid = GenShortId(), qry2 = "";
            DataTable tbl = new DataTable();
            DataTable tbl2 = new DataTable();
            qry2 = "SELECT * FROM cat_users WHERE email='" + correo + "' AND phone_number='" + telefono + "'";
            tbl = mandaQry(qry2);
            if ((tbl.Rows.Count) > 0)
            {
                res = "{\"respuesta\":\"Ya existe cuenta con este correo o número\"}";
            }
            else
            {
                //password = GetMD5(password);
                string [] ap_temp;
                ap_temp=apellidos.Split(" ");
                string first_ap = ap_temp[0];
                string second_ap = "";
                if (ap_temp.Length>1)
                    second_ap = ap_temp[1];
                //password= GetMD5(password);
                DateTime today = DateTime.UtcNow;
                string date = today.ToString("yyyy-MM-dd HH:mm:ss");
                var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var Charsarr = new char[8];
                var random = new Random();

                for (int i = 0; i < Charsarr.Length; i++)
                {
                    Charsarr[i] = characters[random.Next(characters.Length)];
                }

                var resultString = new String(Charsarr);
                qry2 = "INSERT INTO cat_users(user_names,surname,second_surname,phone_number,birth_date,gender,email,status,user_password,date_created,last_updated,verifier_code) VALUES ('"+nombre+"','"+first_ap+"','"+second_ap+"','"+telefono+"','"+fecha_cumpleanos+"','"+genero+"','"+correo+"',1,'"+password+"','"+date+"','"+date+"','VR-"+ resultString + "')";
                try
                {
                    tbl = mandaQry(qry2);
                    res = "{\"respuesta\":\"Se ha creado la cuenta correctamente, por favor verifica tu correo.\"}";
                    Correo.EnviaCorreo(correo,nombre,apellidos,"VR-"+resultString);
                }
                catch
                {
                    res = "{\"respuesta\":\"Error al crear cuenta, vuelve a intentarlo mas tarde.\"}";
                }
                
            }
            return res;
        }
        #endregion CuentaUsuario
        #region FuncionesInternas
        public static string GenShortId()
        {
            var sh = "";
            var characters = "ab0cde1fghi2jkl3mno4pqrs5tuvw6xyz789";
            var Charsarr = new char[8];
            var random = new Random();
            for (int i = 0; i < Charsarr.Length; i++)
            {
                sh = sh + characters[random.Next(characters.Length)];
            }
            return sh;
        }
        public static string Fecha()
        {
            string Date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ms");
            return Date;
        }
        #endregion FuncionesInternas
        #region Principal
        public static DataTable mandaQry(string sqry)
        {
            var cadenaConexion = "server = medrano-prod.ceaa4imnbtld.us-west-2.rds.amazonaws.com; port = 5432;Database=medherprod;User ID=medherdb;Password=sBMH8fvnPM;";
            NpgsqlConnection conexion = new NpgsqlConnection(cadenaConexion);
            NpgsqlCommand comando;
            NpgsqlDataAdapter adaptador;
            DataTable tbl = new DataTable();

            string Result = string.Empty;

            if (!string.IsNullOrWhiteSpace(cadenaConexion))
            {
                comando = new NpgsqlCommand(sqry, conexion);
                comando.CommandType = CommandType.Text;
                conexion.Open();
                conexion.Close();
                tbl = new DataTable();
                adaptador = new NpgsqlDataAdapter(comando);
                adaptador.Fill(tbl);
                Result = "Conectado Exitoso";

            }
            return tbl;
        }
        public static string InsertShortId(string shortId, string amount, string session,string descripcion)
        {
            string sh = shortId.ToString();
            string res = "";
            string qry = "", qry2 = "";
            try
            {
                qry = "UPDATE internet_sale set full_response='{\"sessionId\":\"" + session + "\",\"authorized\":\"false\",\"amount\":" + amount + "}',total_amount=" + amount + ",change_amount= -" + amount + " WHERE short_id='" + shortId + "'";
                mandaQry(qry);
                res = "1";
            }
            catch (Exception ex)
            {
                res = ex.ToString();
            }
            return res;
        }
        public static string ActualizarConceptoPago(infoCambio listacambio)
        {
            string descripcion = listacambio.descripcion;
            string sale_id = listacambio.short_id;
            var res = "";
            string qry = "";
            try
            {
                qry = "UPDATE internet_sale set payment_provider = '" + descripcion + "' WHERE short_id='" + sale_id + "';";
                mandaQry(qry);
                res = "1";
            }
            catch (Exception ex)
            {
                res = ex.ToString();
            }
            return res;
        }
        public static string UpdateBoleto(string shortId, string sale_id, string amount)
        {
            var res = "";
            string sh = shortId.ToString();
            int tick = 0, cont = 0;
            string qry = "", qryi = "", ticketid = GenShortId(), qry2 = "";
            DataTable tbl = new DataTable();
            while (tick == 0)
            {
                qry2 = "SELECT * FROM trip_seat WHERE ticket_id='" + ticketid + "'";
                tbl = mandaQry(qry2);
                foreach (DataRow row in tbl.Rows)
                {
                    ticketid = GenShortId();
                    cont++;
                }
                if (ticketid.Length > 8)
                    cont++;
                if (cont == 0)
                    tick = 1;
            }
            qryi = "SELECT * FROM trip_seat WHERE internet_sale_id='" + sale_id + "'";
            tbl = mandaQry(qryi);
            foreach (DataRow row in tbl.Rows)
            {
                qry2 = "UPDATE trip_seat set status = 'OCCUPIED',ticket_id='" + ticketid + "', sold_price = '" + amount + "',version=1 WHERE id='" + row["id"] + "'";
                mandaQry(qry2);
                ticketid = GenShortId();
            }
            qry = "UPDATE internet_sale set version=2,last_updated='" + Fecha() + "',payed='true',payed_amount=" + amount + ",payment_origin='App' WHERE short_id='" + shortId + "';";
            mandaQry(qry);
            res = shortId;
            return res;
        }
        public static string UpdateBoletoMC(string shortId, string sale_id)
        {
            var res = "";
            int tick = 0, cont = 0;
            string sh = shortId.ToString();
            string qry = "", qryi = "", ticketid = GenShortId(), qry2 = "";
            DataTable tbl = new DataTable();
            DataTable tbl2 = new DataTable();
            while (tick == 0)
            {
                qry2 = "SELECT * FROM trip_seat WHERE ticket_id='" + ticketid + "'";
                tbl = mandaQry(qry2);
                foreach (DataRow row in tbl.Rows)
                {
                    ticketid = GenShortId();
                    cont++;
                }
                if (ticketid.Length > 8)
                    cont++;
                if (cont == 0)
                    tick = 1;
            }
            qryi = "SELECT * FROM trip_seat WHERE internet_sale_id='" + sale_id + "'";
            tbl = mandaQry(qryi);
            foreach (DataRow row in tbl.Rows)
            {
                qry2 = "UPDATE trip_seat set status = 'OCCUPIED',ticket_id='" + ticketid + "',version=1 WHERE id='" + row["id"] + "'";
                mandaQry(qry2);
                ticketid = GenShortId();
                tick = 0;
                cont = 0;
                while (tick == 0)
                {
                    qry2 = "SELECT * FROM trip_seat WHERE ticket_id='" + ticketid + "'";
                    tbl = mandaQry(qry2);
                    if ((tbl.Rows.Count) > 0)
                    {
                        ticketid = GenShortId();
                        cont++;
                    }
                    if (cont == 0)
                        tick = 1;
                }
            }
            qry = "UPDATE internet_sale SET version=2,last_updated='" + Fecha() + "',payed='true',payment_origin='Web' WHERE short_id='" + shortId + "';";
            mandaQry(qry);
            res = shortId;
            return res;
        }
        public static string GetBoleto(string internet_sale_id,string origen,string destino)
        {
            DateTime temp_departure=new DateTime(), temp_ending=new DateTime();
            int aux_ruta = 0;
            int acum_minutes_starting = 0, acum_minutes_ending = 0;
            string fecha = "";
            string departure_date = "";
            string reverse = "";
            string descripcion = "";
            DataTable tbl = new DataTable();
            string sqry = "SELECT reverse,departure_date FROM trip t1 INNER JOIN trip_seat t2 ON t1.id=t2.trip_id WHERE t2.internet_sale_id='" + internet_sale_id + "' LIMIT 1";
            tbl = mandaQry(sqry);
            if (tbl.Rows.Count > 0)
            {
                foreach (DataRow row in tbl.Rows)
                {
                    departure_date = Convert.ToString(row["departure_date"]);
                    reverse = Convert.ToString(row["reverse"]);
                }
            }
            DataTable tbl2 = new DataTable();
            string sqry2 = "select * from stop_off where route_id like (select t2.route_id from trip_seat t1 inner join stop_off t2 on t1.starting_stop_id=t2.id inner join stop_off t3 on t1.ending_stop_id=t3.id where  t1.internet_sale_id like '" + internet_sale_id + "' LIMIT 1) order by order_index asc";
            tbl2 = mandaQry(sqry2);
            if (tbl2.Rows.Count > 0)
            {
                foreach (DataRow row in tbl2.Rows)
                {
                    if (reverse == "False")
                    {
                        if (Convert.ToString(row["name"]) == origen)
                        {
                            acum_minutes_starting += Convert.ToInt32(row["travel_minutes"]);
                            acum_minutes_starting += Convert.ToInt32(row["waiting_minutes"]);
                            aux_ruta = 1;
                        }
                        else if (aux_ruta == 1)
                        {
                            if (Convert.ToString(row["name"]) == destino)
                            {
                                acum_minutes_ending += Convert.ToInt32(row["travel_minutes"]);
                                aux_ruta = 0;
                                break;
                            }
                            else
                            {
                                acum_minutes_ending += Convert.ToInt32(row["travel_minutes"]);
                                acum_minutes_ending += Convert.ToInt32(row["waiting_minutes"]);
                            }
                        }
                        else
                        {
                            acum_minutes_starting += Convert.ToInt32(row["travel_minutes"]);
                            acum_minutes_starting += Convert.ToInt32(row["waiting_minutes"]);
                        }
                    }
                    else if (reverse == "True")
                    {
                        if (Convert.ToString(row["name"]) == destino)
                        {
                            acum_minutes_starting += Convert.ToInt32(row["travel_minutes"]);
                            acum_minutes_starting += Convert.ToInt32(row["waiting_minutes"]);
                            aux_ruta = 1;
                        }
                        else if (aux_ruta == 1)
                        {
                            if (Convert.ToString(row["name"]) == origen)
                            {
                                acum_minutes_ending += Convert.ToInt32(row["travel_minutes"]);
                                aux_ruta = 0;
                                break;
                            }
                            else
                            {
                                acum_minutes_ending += Convert.ToInt32(row["travel_minutes"]);
                                acum_minutes_ending += Convert.ToInt32(row["waiting_minutes"]);
                            }
                        }
                        else
                        {
                            acum_minutes_starting += Convert.ToInt32(row["travel_minutes"]);
                            acum_minutes_starting += Convert.ToInt32(row["waiting_minutes"]);
                        }
                    }
                }
                temp_departure = Convert.ToDateTime(departure_date).AddMinutes(acum_minutes_starting);
                temp_ending = Convert.ToDateTime(temp_departure).AddMinutes(acum_minutes_ending);
                temp_departure = temp_departure.AddHours(-6);
                temp_ending = temp_ending.AddHours(-6);
                fecha = temp_departure.ToString("dd-MM-yyyy HH:mm:ss") + "," + temp_ending.ToString("dd-MM-yyyy HH:mm:ss");
            }
            else
            {
                fecha = "0";
            }
            string res = "";
            DataTable tbl3 = new DataTable();
            string sqry3 = "SELECT t1.*,t2.* FROM internet_sale t1 INNER JOIN trip_seat t2 ON t1.id=t2.internet_sale_id WHERE t2.internet_sale_id='" + internet_sale_id + "'";
            tbl3 = mandaQry(sqry3);
            if (tbl3.Rows.Count > 0)
            {
                foreach (DataRow row in tbl3.Rows)
                {
                    string tipo_pasajero = Convert.ToString(row["passenger_type"]) == "OLDER_ADULT" ? "INAPAM" : Convert.ToString(row["passenger_type"]) == "ADULT" ? "ADULTO" : Convert.ToString(row["passenger_type"]) == "CHILD" ? "NIÑO" : Convert.ToString(row["passenger_type"]) == "STUDENT" ? "ESTUDIANTE" : "";
                    if (res != "")
                    {
                        res = res + ",{\"origen\":\"" + origen + "\",\"destino\":\"" + destino + "\",\"asiento\":\"" + row["seat_name"] + "\",\"nombre\":\"" + row["passenger_name"] + "\",\"fechaSalida\":\"" + temp_departure.ToString("dd-MM-yyyy HH:mm:ss") + "\",\"fechaLlegada\":\"" + temp_ending.ToString("dd-MM-yyyy HH:mm:ss") + "\",\"total\":\"" + row["original_price"] + "\",\"precioVenta\":\"" + row["sold_price"] + "\",\"idTicket\":\"" + row["ticket_id"] + "\",\"tipoPasajero\":\"" + row["passenger_type"] + "\",\"tipopasajero\":\"" + tipo_pasajero + "\"}";
                    }
                    else
                    {
                        res = "{\"origen\":\"" + origen + "\",\"destino\":\"" + destino + "\",\"asiento\":\"" + row["seat_name"] + "\",\"nombre\":\"" + row["passenger_name"] + "\",\"fechaSalida\":\"" + temp_departure.ToString("dd-MM-yyyy HH:mm:ss") + "\",\"fechaLlegada\":\"" + temp_ending.ToString("dd-MM-yyyy HH:mm:ss") + "\",\"total\":\"" + row["original_price"] + "\",\"precioVenta\":\"" + row["sold_price"] + "\",\"idTicket\":\"" + row["ticket_id"] + "\",\"tipoPasajero\":\"" + row["passenger_type"] + "\",\"tipopasajero\":\"" + tipo_pasajero + "\"}";
                    }
                }
            }
            return "["+res+"]";
        }
        #region ClasesPrincipal
        #endregion ClasesPrincipal
        #endregion Principal
        #region PayPal
        public static string CapturaPaypal(string id, string access_token, string boletos, string monto)
        {
            /*PRODUCCION*/
            //string responseBody = "";
            //string url = "";
            //string session = "";
            String domain = "https://api-m.paypal.com"; // for production mode
            String clientId = "AXCUW-mhnLRHO99qRhp8nVh1TCLKH0u3XxXiu2OBrH-sXgfwjrs27AAe8JOsldF344a-n1LIdbAhWijP"; // Productivo EM
            String secret = "EGRPr1iphqPIlj5x-AaOLjugeC4qvdGQNTCqc0n5hDK1vLwDpYhgXEwenUgNux_GpAdSMVJAtcCwZO5U"; //Productivo EM
            string finalsecret = "Basic QVhDVVctbWhuTFJITzk5cVJocDhuVmgxVENMS0gwdTNYeFhpdTJPQnJILXNYZ2Z3anJzMjdBQWU4Sk9zbGRGMzQ0YS1uMUxJZGJBaFdpalA6RUdSUHIxaXBocVBJbGo1eC1BYU9ManVnZUM0cXZkR1FOVENxYzBuNWhESzF2THdEcFloZ1hFd2VuVWdOdXhfR3BBZFNNVkpBdGNDd1pPNVU=";
            String apiUrlOxxo = "https://api-m.paypal.com/v2/checkout/orders/"; //Productivo EM oxxo 
            ///////**SANDBOX DYLAN**////////////
            //String domain = "https://api.sandbox.paypal.com"; // for sandbox mode
            //String clientId ='Ab_8lMWrTS0zFMC4JCx45UrqLaKY5RRRBvXC9Y-mSdZb9fFKCOcDfGyw75BxcuA1pAD_GDPTxMqPusd2'; //Sandbox DZ
            //String secret ='EPzIN9hAwpII6yX1eVJvqvxde7Ya9pzQyu6g3SwwtVXZZ3uzcqkiIVlV0CWk9apJq3AqOtGo2oMi0QfH'; //Sandbox DZ 
            //string finalsecret = "Basic QWJfOGxNV3JUUzB6Rk1DNEpDeDQ1VXJxTGFLWTVSUlJCdlhDOVktbVNkWmI5ZkZLQ09jRGZHeXc3NUJ4Y3VBMXBBRF9HRFBUeE1xUHVzZDI6RVB6SU45aEF3cElJNnlYMWVWSnZxdnhkZTdZYTlwelF5dTZnM1N3d3RWWFpaM3V6Y3FraUlWbFYwQ1drOWFwSnEzQXFPdEdvMm9NaTBRZkg=";
            //String clientId = "ARWwoOuBXa4Do7LfrOtqiz3na07n8rQARH_vnqZA_Z5cRN3QJ7LOlNHPMwtZEK6o7c-6V3m-vCsxbwOp"; //Sandbox EM
            //String secret = "EF4N03yvULZ1TI650f9XbkGQycGovgFDprhFWR8hheUna72inyRN4PJCEJKxB9ww1VtiUn7bSix-gwnS"; //Sandbox EM
            //string finalsecret = "Basic QVJXd29PdUJYYTREbzdMZnJPdHFpejNuYTA3bjhyUUFSSF92bnFaQV9aNWNSTjNRSjdMT2xOSFBNd3RaRUs2bzdjLTZWM20tdkNzeGJ3T3A6RUY0TjAzeXZVTFoxVEk2NTBmOVhia0dReWNHb3ZnRkRwcmhGV1I4aGhlVW5hNzJpbnlSTjRQSkNFSkt4Qjl3dzFWdGlVbjdiU2l4LWd3blM=";
            //String apiUrlOxxo = "https://api.sandbox.paypal.com/v2/checkout/orders/"; //Sandbox */
            string responseBody = "";
            string estatus = "";
            string session = "";
            string json = "{" +
            "\"intent\": \"CAPTURE\"," +
            "\"purchase_units\": [{" +
            "\"items\": [{" +
            "\"name\": \"Ticket SAG\"," +
            "\"description\": \"Boleto\"," +
            "\"quantity\": \"" + boletos + "\"," +
            "\"unit_amount\": {" +
            "\"currency_code\": \"MXN\"," +
            "\"value\": \"" + monto + "\"" +
            "}}]," +
            "\"amount\": {" +
            "\"currency_code\": \"MXN\"," +
            "\"value\": \"" + monto + "\"," +
            "\"breakdown\": {" +
            "\"item_total\": {" +
            "\"currency_code\": \"MXN\"," +
            "\"value\": \"" + monto + "\"" +
            "}}}}]," +
            "\"application_context\": {" +
            "\"return_url\": \"https://example.com/return\"," +
            "\"cancel_url\": \"https://example.com/cancel\"" +
            "}}";
            string res = domain + "/v2/checkout/orders/" + id + "/capture";
            var requestToken = (HttpWebRequest)WebRequest.Create(res);
            requestToken.Method = "POST";
            requestToken.Headers["Authorization"] = "Bearer " + access_token;
            requestToken.Headers["Content-Type"] = "application/json";
            using (var streamWriter = new StreamWriter(requestToken.GetRequestStream()))
            {
                streamWriter.Write(json);
            }
            using (WebResponse responseToken = requestToken.GetResponse())
            {
                using (Stream strReader = responseToken.GetResponseStream())
                {
                    if (strReader == null) return "";
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        responseBody = objReader.ReadToEnd();
                        responseBody = (responseBody.Replace("\n", "")).ToString();
                    }
                    var rs = JsonConvert.DeserializeObject<CPayPal>(responseBody);
                    estatus = rs.status;
                }
            }
            return estatus;
        }
        public static string RevisarPaypal(string id, string access_token)
        {
            /*PRODUCCION*/
            //string responseBody = "";
            //string url = "";
            //string session = "";
            String domain = "https://api-m.paypal.com"; // for production mode
            String clientId = "AXCUW-mhnLRHO99qRhp8nVh1TCLKH0u3XxXiu2OBrH-sXgfwjrs27AAe8JOsldF344a-n1LIdbAhWijP"; // Productivo EM
            String secret = "EGRPr1iphqPIlj5x-AaOLjugeC4qvdGQNTCqc0n5hDK1vLwDpYhgXEwenUgNux_GpAdSMVJAtcCwZO5U"; //Productivo EM
            string finalsecret = "Basic QVhDVVctbWhuTFJITzk5cVJocDhuVmgxVENMS0gwdTNYeFhpdTJPQnJILXNYZ2Z3anJzMjdBQWU4Sk9zbGRGMzQ0YS1uMUxJZGJBaFdpalA6RUdSUHIxaXBocVBJbGo1eC1BYU9ManVnZUM0cXZkR1FOVENxYzBuNWhESzF2THdEcFloZ1hFd2VuVWdOdXhfR3BBZFNNVkpBdGNDd1pPNVU=";
            String apiUrlOxxo = "https://api-m.paypal.com/v2/checkout/orders/"; //Productivo EM oxxo 
            ///////**SANDBOX DYLAN**////////////
            //String domain = "https://api.sandbox.paypal.com"; // for sandbox mode
            //String clientId ='Ab_8lMWrTS0zFMC4JCx45UrqLaKY5RRRBvXC9Y-mSdZb9fFKCOcDfGyw75BxcuA1pAD_GDPTxMqPusd2'; //Sandbox DZ
            //String secret ='EPzIN9hAwpII6yX1eVJvqvxde7Ya9pzQyu6g3SwwtVXZZ3uzcqkiIVlV0CWk9apJq3AqOtGo2oMi0QfH'; //Sandbox DZ 
            //string finalsecret = "Basic QWJfOGxNV3JUUzB6Rk1DNEpDeDQ1VXJxTGFLWTVSUlJCdlhDOVktbVNkWmI5ZkZLQ09jRGZHeXc3NUJ4Y3VBMXBBRF9HRFBUeE1xUHVzZDI6RVB6SU45aEF3cElJNnlYMWVWSnZxdnhkZTdZYTlwelF5dTZnM1N3d3RWWFpaM3V6Y3FraUlWbFYwQ1drOWFwSnEzQXFPdEdvMm9NaTBRZkg=";
            //String clientId = "ARWwoOuBXa4Do7LfrOtqiz3na07n8rQARH_vnqZA_Z5cRN3QJ7LOlNHPMwtZEK6o7c-6V3m-vCsxbwOp"; //Sandbox EM
            //String secret = "EF4N03yvULZ1TI650f9XbkGQycGovgFDprhFWR8hheUna72inyRN4PJCEJKxB9ww1VtiUn7bSix-gwnS"; //Sandbox EM
            //string finalsecret = "Basic QVJXd29PdUJYYTREbzdMZnJPdHFpejNuYTA3bjhyUUFSSF92bnFaQV9aNWNSTjNRSjdMT2xOSFBNd3RaRUs2bzdjLTZWM20tdkNzeGJ3T3A6RUY0TjAzeXZVTFoxVEk2NTBmOVhia0dReWNHb3ZnRkRwcmhGV1I4aGhlVW5hNzJpbnlSTjRQSkNFSkt4Qjl3dzFWdGlVbjdiU2l4LWd3blM=";
            //String apiUrlOxxo = "https://api.sandbox.paypal.com/v2/checkout/orders/"; //Sandbox */
            string responseBody = "";
            string estatus = "";
            string session = "";
            string res = domain + "/v2/checkout/orders/" + id;
            var requestToken = (HttpWebRequest)WebRequest.Create(res);
            requestToken.Method = "GET";
            requestToken.Headers["Authorization"] = "Bearer " + access_token;
            requestToken.Headers["Content-Type"] = "application/json";
            using (WebResponse responseToken = requestToken.GetResponse())
            {
                using (Stream strReader = responseToken.GetResponseStream())
                {
                    if (strReader == null) return "";
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        responseBody = objReader.ReadToEnd();
                        responseBody = (responseBody.Replace("\n", "")).ToString();
                    }
                }
            }
            return responseBody;
        }
        public static string PagoPaypal(string origen, string destino, string boletos, string monto, string short_id,string descripcion)
        {
            /*PRODUCCION*/
            //string responseBody = "";
            //string url = "";
            //string session = "";
            String domain = "https://api-m.paypal.com"; // for production mode
            String clientId = "AXCUW-mhnLRHO99qRhp8nVh1TCLKH0u3XxXiu2OBrH-sXgfwjrs27AAe8JOsldF344a-n1LIdbAhWijP"; // Productivo EM
            String secret = "EGRPr1iphqPIlj5x-AaOLjugeC4qvdGQNTCqc0n5hDK1vLwDpYhgXEwenUgNux_GpAdSMVJAtcCwZO5U"; //Productivo EM
            string finalsecret = "Basic QVhDVVctbWhuTFJITzk5cVJocDhuVmgxVENMS0gwdTNYeFhpdTJPQnJILXNYZ2Z3anJzMjdBQWU4Sk9zbGRGMzQ0YS1uMUxJZGJBaFdpalA6RUdSUHIxaXBocVBJbGo1eC1BYU9ManVnZUM0cXZkR1FOVENxYzBuNWhESzF2THdEcFloZ1hFd2VuVWdOdXhfR3BBZFNNVkpBdGNDd1pPNVU=";
            String apiUrlOxxo = "https://api-m.paypal.com/v2/checkout/orders/"; //Productivo EM oxxo 
            ///////**SANDBOX DYLAN**////////////
            //String domain = "https://api.sandbox.paypal.com"; // for sandbox mode
            //String clientId ='Ab_8lMWrTS0zFMC4JCx45UrqLaKY5RRRBvXC9Y-mSdZb9fFKCOcDfGyw75BxcuA1pAD_GDPTxMqPusd2'; //Sandbox DZ
            //String secret ='EPzIN9hAwpII6yX1eVJvqvxde7Ya9pzQyu6g3SwwtVXZZ3uzcqkiIVlV0CWk9apJq3AqOtGo2oMi0QfH'; //Sandbox DZ 
            //string finalsecret = "Basic QWJfOGxNV3JUUzB6Rk1DNEpDeDQ1VXJxTGFLWTVSUlJCdlhDOVktbVNkWmI5ZkZLQ09jRGZHeXc3NUJ4Y3VBMXBBRF9HRFBUeE1xUHVzZDI6RVB6SU45aEF3cElJNnlYMWVWSnZxdnhkZTdZYTlwelF5dTZnM1N3d3RWWFpaM3V6Y3FraUlWbFYwQ1drOWFwSnEzQXFPdEdvMm9NaTBRZkg=";
            //String clientId = "ARWwoOuBXa4Do7LfrOtqiz3na07n8rQARH_vnqZA_Z5cRN3QJ7LOlNHPMwtZEK6o7c-6V3m-vCsxbwOp"; //Sandbox EM
            //String secret = "EF4N03yvULZ1TI650f9XbkGQycGovgFDprhFWR8hheUna72inyRN4PJCEJKxB9ww1VtiUn7bSix-gwnS"; //Sandbox EM
            //string finalsecret = "Basic QVJXd29PdUJYYTREbzdMZnJPdHFpejNuYTA3bjhyUUFSSF92bnFaQV9aNWNSTjNRSjdMT2xOSFBNd3RaRUs2bzdjLTZWM20tdkNzeGJ3T3A6RUY0TjAzeXZVTFoxVEk2NTBmOVhia0dReWNHb3ZnRkRwcmhGV1I4aGhlVW5hNzJpbnlSTjRQSkNFSkt4Qjl3dzFWdGlVbjdiU2l4LWd3blM=";
            //String apiUrlOxxo = "https://api.sandbox.paypal.com/v2/checkout/orders/"; //Sandbox */
            string responseBody = "";
            string url = "";
            string access_token = "";
            string res = domain + "/v1/oauth2/token?grant_type=client_credentials";
            var requestToken = (HttpWebRequest)WebRequest.Create(res);
            requestToken.Method = "POST";
            requestToken.Headers["Authorization"] = finalsecret;
            requestToken.Headers["ContentType"] = "application/json";
            requestToken.Headers["Content-Length"] = "0";
            using (WebResponse responseToken = requestToken.GetResponse())
            {
                using (Stream strReader = responseToken.GetResponseStream())
                {
                    if (strReader == null) return "";
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        responseBody = objReader.ReadToEnd();
                        responseBody = (responseBody.Replace("\n", "")).ToString();
                    }
                    var rs = JsonConvert.DeserializeObject<Token>(responseBody);
                    access_token = rs.access_token;
                }
            }
            if (access_token != "")
            {
                url = PagarPayPal(access_token, origen, destino, monto, boletos, short_id,descripcion);
            }
            return url;
        }
        public static string PagarPayPal(string access_token, string origen, string destino, string monto, string boletos, string short_id,string descripcion)
        {
            /*PRODUCCION*/
            //string responseBody = "";
            //string url = "";
            //string session = "";
            String domain = "https://api-m.paypal.com"; // for production mode
            String clientId = "AXCUW-mhnLRHO99qRhp8nVh1TCLKH0u3XxXiu2OBrH-sXgfwjrs27AAe8JOsldF344a-n1LIdbAhWijP"; // Productivo EM
            String secret = "EGRPr1iphqPIlj5x-AaOLjugeC4qvdGQNTCqc0n5hDK1vLwDpYhgXEwenUgNux_GpAdSMVJAtcCwZO5U"; //Productivo EM
            string finalsecret = "Basic QVhDVVctbWhuTFJITzk5cVJocDhuVmgxVENMS0gwdTNYeFhpdTJPQnJILXNYZ2Z3anJzMjdBQWU4Sk9zbGRGMzQ0YS1uMUxJZGJBaFdpalA6RUdSUHIxaXBocVBJbGo1eC1BYU9ManVnZUM0cXZkR1FOVENxYzBuNWhESzF2THdEcFloZ1hFd2VuVWdOdXhfR3BBZFNNVkpBdGNDd1pPNVU=";
            String apiUrlOxxo = "https://api-m.paypal.com/v2/checkout/orders/"; //Productivo EM oxxo 
            ///////**SANDBOX DYLAN**////////////
            //String domain = "https://api.sandbox.paypal.com"; // for sandbox mode
            //String clientId ='Ab_8lMWrTS0zFMC4JCx45UrqLaKY5RRRBvXC9Y-mSdZb9fFKCOcDfGyw75BxcuA1pAD_GDPTxMqPusd2'; //Sandbox DZ
            //String secret ='EPzIN9hAwpII6yX1eVJvqvxde7Ya9pzQyu6g3SwwtVXZZ3uzcqkiIVlV0CWk9apJq3AqOtGo2oMi0QfH'; //Sandbox DZ 
            //string finalsecret = "Basic QWJfOGxNV3JUUzB6Rk1DNEpDeDQ1VXJxTGFLWTVSUlJCdlhDOVktbVNkWmI5ZkZLQ09jRGZHeXc3NUJ4Y3VBMXBBRF9HRFBUeE1xUHVzZDI6RVB6SU45aEF3cElJNnlYMWVWSnZxdnhkZTdZYTlwelF5dTZnM1N3d3RWWFpaM3V6Y3FraUlWbFYwQ1drOWFwSnEzQXFPdEdvMm9NaTBRZkg=";
            //String clientId = "ARWwoOuBXa4Do7LfrOtqiz3na07n8rQARH_vnqZA_Z5cRN3QJ7LOlNHPMwtZEK6o7c-6V3m-vCsxbwOp"; //Sandbox EM
            //String secret = "EF4N03yvULZ1TI650f9XbkGQycGovgFDprhFWR8hheUna72inyRN4PJCEJKxB9ww1VtiUn7bSix-gwnS"; //Sandbox EM
            //string finalsecret = "Basic QVJXd29PdUJYYTREbzdMZnJPdHFpejNuYTA3bjhyUUFSSF92bnFaQV9aNWNSTjNRSjdMT2xOSFBNd3RaRUs2bzdjLTZWM20tdkNzeGJ3T3A6RUY0TjAzeXZVTFoxVEk2NTBmOVhia0dReWNHb3ZnRkRwcmhGV1I4aGhlVW5hNzJpbnlSTjRQSkNFSkt4Qjl3dzFWdGlVbjdiU2l4LWd3blM=";
            //String apiUrlOxxo = "https://api.sandbox.paypal.com/v2/checkout/orders/"; //Sandbox */
            string responseBody = "", session = "";
            string url = "";
            string json = "{" +
                "\"intent\": \"CAPTURE\"," +
                "\"purchase_units\": [{" +
                "\"items\": [{" +
                    "\"name\": \"Viaja con SAG México\"," +
                    "\"description\": \"Viaje de " + origen + " - " + destino + "\"," +
                    "\"quantity\": \"" + boletos + "\"," +
                    "\"unit_amount\": {" +
                        "\"currency_code\": \"MXN\"," +
                        "\"value\": " + monto + "" +
                    "}}]," +
            "\"amount\": {" +
                "\"currency_code\": \"MXN\"," +
                "\"value\": " + monto + "," +
                "\"breakdown\": {" +
                    "\"item_total\": {" +
                        "\"currency_code\": \"MXN\"," +
                        "\"value\": \"" + monto + "\"" +
                    "}}}}]," +
            "\"application_context\": {" +
                "\"return_url\": \"http://www.sagautobuses.com/Boletos?Version=20&Boleto=" + short_id + "\"," +
                "\"cancel_url\": \"http://www.sagautobuses.com/\"" +
            "}}";
            string res = domain + "/v2/checkout/orders";
            var requestToken = (HttpWebRequest)WebRequest.Create(res);
            requestToken.Method = "POST";
            requestToken.Headers["Authorization"] = "Bearer " + access_token;
            requestToken.Headers["Content-Type"] = "application/json";
            using (var streamWriter = new StreamWriter(requestToken.GetRequestStream()))
            {
                streamWriter.Write(json);
            }
            using (WebResponse responseToken = requestToken.GetResponse())
            {
                using (Stream strReader = responseToken.GetResponseStream())
                {
                    if (strReader == null) return "";
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        responseBody = objReader.ReadToEnd();
                        responseBody = (responseBody.Replace("\n", "")).ToString();
                    }
                    var rs = JsonConvert.DeserializeObject<PPaypal>(responseBody);
                    url = rs.links[1].href;
                    session = rs.id;
                }
            }
            InsertShortId(short_id, monto, (url + "&Access_token=" + access_token),descripcion);
            return "{\"url\":\"" + url + "\",\"id\":\"" + session + "\",\"access_token\":\"" + access_token + "\"}";
        }
        #region ClasesPayPal
        public class Token
        {
            public string scope { get; set; }
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string app_id { get; set; }
            public int expires_in { get; set; }
            public string nonce { get; set; }
        }
        public class Link
        {
            public string href { get; set; }
            public string rel { get; set; }
            public string method { get; set; }
        }

        public class PPaypal
        {
            public string id { get; set; }
            public string status { get; set; }
            public List<Link> links { get; set; }
        }
        ///
        public class Address
        {
            public string country_code { get; set; }
            public string address_line_1 { get; set; }
            public string address_line_2 { get; set; }
            public string admin_area_2 { get; set; }
            public string admin_area_1 { get; set; }
            public string postal_code { get; set; }
        }

        public class Amount
        {
            public string currency_code { get; set; }
            public string value { get; set; }
        }

        public class Capture
        {
            public string id { get; set; }
            public string status { get; set; }
            public Amount amount { get; set; }
            public bool final_capture { get; set; }
            public SellerProtection seller_protection { get; set; }
            public SellerReceivableBreakdown seller_receivable_breakdown { get; set; }
            public List<Link> links { get; set; }
            public DateTime create_time { get; set; }
            public DateTime update_time { get; set; }
        }

        public class GrossAmount
        {
            public string currency_code { get; set; }
            public string value { get; set; }
        }
        public class Name
        {
            public string given_name { get; set; }
            public string surname { get; set; }
            public string full_name { get; set; }
        }

        public class NetAmount
        {
            public string currency_code { get; set; }
            public string value { get; set; }
        }

        public class Payer
        {
            public Name name { get; set; }
            public string email_address { get; set; }
            public string payer_id { get; set; }
            public Address address { get; set; }
        }

        public class Payments
        {
            public List<Capture> captures { get; set; }
        }

        public class PaymentSource
        {
            public Paypal paypal { get; set; }
        }

        public class Paypal
        {
            public string email_address { get; set; }
            public string account_id { get; set; }
            public Name name { get; set; }
            public Address address { get; set; }
        }

        public class PaypalFee
        {
            public string currency_code { get; set; }
            public string value { get; set; }
        }

        public class PurchaseUnit
        {
            public string reference_id { get; set; }
            public Shipping shipping { get; set; }
            public Payments payments { get; set; }
        }

        public class CPayPal
        {
            public string id { get; set; }
            public string status { get; set; }
            public PaymentSource payment_source { get; set; }
            public List<PurchaseUnit> purchase_units { get; set; }
            public Payer payer { get; set; }
            public List<Link> links { get; set; }
        }

        public class SellerProtection
        {
            public string status { get; set; }
            public List<string> dispute_categories { get; set; }
        }

        public class SellerReceivableBreakdown
        {
            public GrossAmount gross_amount { get; set; }
            public PaypalFee paypal_fee { get; set; }
            public NetAmount net_amount { get; set; }
        }

        public class Shipping
        {
            public Name name { get; set; }
            public Address address { get; set; }
        }
        #endregion ClasesPaypal
        #endregion PayPal
        #region MasterCard
        public static string GetSesionW(infoSesionMC infoSesion)
        {
            string descripcion;
            if(infoSesion.descripcion== "Boleto de SAG Autobuses")
                descripcion = "mastercard chatbot";
            else
                descripcion = "mastercard web";
            string session = "";
            string internet_sale_id = infoSesion.internet_sale_id;
            string shortId = infoSesion.short_id;
            string res = "https://evopaymentsmexico.gateway.mastercard.com/api/rest/version/61/merchant/1079224/session";
            var requestToken = (HttpWebRequest)WebRequest.Create(res);
            requestToken.Method = "POST";
            requestToken.Headers["Authorization"] = "Basic bWVyY2hhbnQuMTA3OTIyNDpjNGUyYzAwY2JjYjc5NDNmOTEyZGZmZTI1ZDFiNTc1ZQ==";
            requestToken.ContentType = "application/json";
            string json = "{\"apiOperation\":\"CREATE_CHECKOUT_SESSION\"," +
                  "\"interaction\":{" +
                  "\"operation\": \"PURCHASE\"," +
                  "\"cancelUrl\": \"http://www.sagautobuses.com/\"," +
                  "\"displayControl\": {" +
                  "\"orderSummary\": \"SHOW\"," +
                  "\"billingAddress\": \"HIDE\"}," +
                  "\"locale\": \"es_MX\"," +
                  "\"returnUrl\": \"http://www.sagautobuses.com/Boletos?Token=" + internet_sale_id + "&Boleto=" + shortId + "&Version=10\"," +
                  "\"timeout\": \"600\"," +
                  "\"timeoutUrl\": \"https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcTuOfL1_7BKsBXyZiGx21Ok_ozIdodDmuPeq1eSD85bS92UvKPZ&usqp=CAU\"}," +
                  "\"order\": {" +
                  "\"amount\": \"" + infoSesion.monto + "\"," +
                  "\"customerReference\": \"Boletos SAG AUTOBUSES\"," +
                  "\"currency\": \"MXN\"," +
                  "\"description\":\"" + infoSesion.descripcion + "\"," +
                  "\"id\": \"" + shortId + "\"}}";
            using (var streamWriter = new StreamWriter(requestToken.GetRequestStream()))
            {
                streamWriter.Write(json);
            }
            using (WebResponse responseToken = requestToken.GetResponse())
            {
                using (Stream strReader = responseToken.GetResponseStream())
                {
                    if (strReader == null) return "";
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        string responseBody = objReader.ReadToEnd();
                        var TC = JsonConvert.DeserializeObject<Root>(responseBody);
                        session = TC.session.id;
                    }
                }
            }
            InsertShortId(shortId, infoSesion.monto, session,descripcion);
            string qry = "";
            try
            {
                qry = "UPDATE internet_sale set payment_provider = 'mastercard web' WHERE short_id='" + shortId + "';";
                mandaQry(qry);
                res = "1";
            }
            catch (Exception ex)
            {
                res = ex.ToString();
            }
            return "http://www.sagautobuses.com/Pago?Session=" + session + "&id=" + shortId;
        }
        public static string ActualizarMasterCard(string short_id,string descripcion)
        {
            var res = "";
            string qry = "";
            try
            {
                qry = "UPDATE internet_sale set payment_provider = '" + descripcion + "' WHERE short_id='" + short_id + "';";
                mandaQry(qry);
                res = "1";
            }
            catch (Exception ex)
            {
                res = ex.ToString();
            }
            return res;
        }
        #endregion MasterCard
        #region PagoEfectivo
        public static string ActualizarEfectivo(string descripcion, string monto, string short_id, string internet_sale_id, string sales_terminal_id, string salesman_id)
        {
            var res = "";
            int tick = 0, cont = 0;
            string qry = "", qryi = "", ticketid = GenShortId(), qry2 = "";
            DataTable tbl = new DataTable();
            DataTable tbl2 = new DataTable();
            while (tick == 0)
            {
                qry2 = "SELECT * FROM trip_seat WHERE ticket_id='" + ticketid + "'";
                tbl = mandaQry(qry2);
                foreach (DataRow row in tbl.Rows)
                {
                    ticketid = GenShortId();
                    cont++;
                }
                if (ticketid.Length > 8)
                    cont++;
                if (cont == 0)
                    tick = 1;
            }
            qryi = "SELECT * FROM trip_seat WHERE internet_sale_id='" + internet_sale_id + "'";
            tbl = mandaQry(qryi);
            foreach (DataRow row in tbl.Rows)
            {
                qry2 = "UPDATE trip_seat set status = 'OCCUPIED',ticket_id='" + ticketid + "',version=1 WHERE id='" + row["id"] + "'";
                mandaQry(qry2);
                ticketid = GenShortId();
                tick = 0;
                cont = 0;
                while (tick == 0)
                {
                    qry2 = "SELECT * FROM trip_seat WHERE ticket_id='" + ticketid + "'";
                    tbl = mandaQry(qry2);
                    if ((tbl.Rows.Count) > 0)
                    {
                        ticketid = GenShortId();
                        cont++;
                    }
                    if (cont == 0)
                        tick = 1;
                }
            }
            qry = "UPDATE internet_sale SET version=2,last_updated='" + Fecha() + "',payed='true',payment_origin='Web',payment_provider='"+descripcion+"',sales_terminal_id='"+sales_terminal_id+"',salesman_id='"+salesman_id+"' WHERE short_id='" + short_id + "'";
            mandaQry(qry);
            res = short_id;
            return res;
        }
        #endregion PagoEfectivo
        #region Blog
        public static string GetPost()
        {
            var res = "";
            int tick = 0, cont = 0;
            string qry = "";
            DataTable tbl = new DataTable();
            qry = "SELECT t1.tittle, t1.summary, t1.text, t1.date_updated, t2.url_image, t2.idx_image, t3.description as category FROM detail_post t1 INNER JOIN cat_image t2 ON t2.id_post= t1.id_post INNER JOIN cat_Category t3 ON t1.id_category= t3.id_category WHERE t3.status= 1 ORDER BY date_updated DESC";
            tbl=mandaQry(qry);
            foreach (DataRow row in tbl.Rows)
            {
                if (cont == 0)
                {
                    res = "{\"tittle\":\"" + row["tittle"].ToString() + "\",\"summary\":\"" + row["summary"].ToString() + "\",\"text\":\"" + row["text"].ToString() + "\",\"date_updated\":\"" + row["date_updated"].ToString() + "\",\"url_image\":\"" + row["url_image"].ToString() + "\",\"idx_image\":\"" + row["idx_image"].ToString() + "\",\"category\":\"" + row["category"].ToString() + "\"}";
                }
                else
                {
                    res = res + ",{\"tittle\":\"" + row["tittle"].ToString() + "\",\"summary\":\"" + row["summary"].ToString() + "\",\"text\":\"" + row["text"].ToString() + "\",\"date_updated\":\"" + row["date_updated"].ToString() + "\",\"url_image\":\"" + row["url_image"].ToString() + "\",\"idx_image\":\"" + row["idx_image"].ToString() + "\",\"category\":\"" + row["category"].ToString() + "\"}";
                }
                cont++;
            }
            return "[" + res + "]";
        }
        public static string ViewPost(string id_post)
        {
            var res = "";
            int tick = 0, cont = 0;
            string qry = "";
            DataTable tbl = new DataTable();
            qry = "SELECT t1.tittle,t1.summary,t1.text,t1.date_updated,t2.url_image,t2.idx_image,t3.description as category FROM detail_post t1 INNER JOIN cat_image t2 ON t2.id_post=t1.id_post INNER JOIN cat_Category t3 ON t1.id_category=t3.id_category WHERE t3.status=1 AND t1.id_post="+id_post+" ORDER BY date_updated DESC";
            tbl = mandaQry(qry);
            foreach (DataRow row in tbl.Rows)
            {
                if (cont == 0)
                {
                    res = "{\"tittle\":\"" + row["tittle"].ToString() + "\",\"summary\":\"" + row["summary"].ToString() + "\",\"text\":\"" + row["text"].ToString() + "\",\"date_updated\":\"" + row["date_updated"].ToString() + "\",\"url_image\":\"" + row["url_image"].ToString() + "\",\"idx_image\":\"" + row["idx_image"].ToString() + "\",\"category\":\"" + row["category"].ToString() + "\"}";
                }
                else
                {
                    res = res + ",{\"tittle\":\"" + row["tittle"].ToString() + "\",\"summary\":\"" + row["summary"].ToString() + "\",\"text\":\"" + row["text"].ToString() + "\",\"date_updated\":\"" + row["date_updated"].ToString() + "\",\"url_image\":\"" + row["url_image"].ToString() + "\",\"idx_image\":\"" + row["idx_image"].ToString() + "\",\"category\":\"" + row["category"].ToString() + "\"}";
                }
                cont++;
            }
            return "[" + res + "]";
        }
        #endregion Blog
        #region Index
        public static Banner GetBanner()
        {
            string sqry = "SELECT * FROM cat_banner";
            DataTable tbl = new DataTable();
            Banner Lista = new Banner();
            Lista.listado = new List<Listado>();
            tbl = mandaQry(sqry);
            foreach (DataRow row in tbl.Rows)
            {
                Listado info = new Listado();
                info.url = row["url_picture"].ToString();
                info.dispositivo = row["device"].ToString();
                Lista.listado.Add(info);
            }
            return Lista;
    }
        #endregion Index
    }
}