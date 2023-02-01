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
using static WebApplication1.Controllers.HomeController;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using JavaScriptEngineSwitcher.Jurassic;
using JavaScriptEngineSwitcher.Core;
using Microsoft.AspNetCore.Http;
using System.Security.Policy;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using PayPal.Api;

namespace WebApplication1.Models
{
    public class Metodos
    {
        #region Admin
        public static string GetCompras()
        {
            int cont = 0;
            DataTable tbl;
            string sqry = "";
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            Banner Lista = new Banner();
            Lista.listado = new List<Listado>();
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ReporteCompras";
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            tbl = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            tbl.Load(dr);
            conn.Close();
            foreach (DataRow row in tbl.Rows)
            {
                if(cont==0)
                    sqry = "{\"metodo_pago\":\"" + row["metodo_pago"] + "\",\"total\":\"" + row["total"] +"\"}";
                else
                    sqry = sqry+ ",{\"metodo_pago\":\"" + row["metodo_pago"] + "\",\"total\":\"" + row["total"] + "\"}";
                cont++;
            }
            return sqry;
        }
        public static string GetVisitaFecha(string opcion)
        {
            int cont = 0;
            DataTable tbl;
            string sqry = "";
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            Banner Lista = new Banner();
            Lista.listado = new List<Listado>();
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ReportVisitasFecha";
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@opcion", opcion));
            tbl = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            tbl.Load(dr);
            conn.Close();
            foreach (DataRow row in tbl.Rows)
            {
                if (cont == 0)
                    sqry = "{\"fecha\":\"" + row["Fecha"] + "\",\"total_visita\":\"" + row["total_visita"] + "\",\"total_compra\":\"" + row["total_compra"] + "\"}";
                else
                    sqry = sqry + ",{\"fecha\":\"" + row["Fecha"] + "\",\"total_visita\":\"" + row["total_visita"] + "\",\"total_compra\":\"" + row["total_compra"] + "\"}";
                cont++;
            }
            return sqry;
        }
        #endregion Admin
        #region PayPal
        public static string PagoPaypal(string origen,string destino,string boletos,string monto,string short_id) {
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
            requestToken.Headers["Authorization"] =finalsecret;
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
                url = PagarPayPal(access_token,origen,destino,monto,boletos,short_id);
            }
            return url;
        }
        public static string PagarPayPal(string access_token,string origen,string destino,string monto,string boletos,string short_id)
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
            string responseBody = "",session="";
            string url = "";
            string json = "{"+
                "\"intent\": \"CAPTURE\","+
                "\"purchase_units\": [{"+
                "\"items\": [{"+
                    "\"name\": \"Viaja con SAG México\","+
                    "\"description\": \"Viaje de "+origen+" - "+destino+"\","+
                    "\"quantity\": \""+boletos+"\","+
                    "\"unit_amount\": {"+
                        "\"currency_code\": \"MXN\","+
                        "\"value\": "+monto+""+
                    "}}],"+
            "\"amount\": {"+
                "\"currency_code\": \"MXN\","+
                "\"value\": "+monto+","+
                "\"breakdown\": {"+
                    "\"item_total\": {"+
                        "\"currency_code\": \"MXN\","+
                        "\"value\": \""+monto +"\""+
                    "}}}}],"+
            "\"application_context\": {"+
                "\"return_url\": \"http://www.sagautobuses.com/Boletos?Version=20&Boleto="+short_id+"&boletos=" + boletos+"&origen="+origen+"&destino="+destino+"&amount="+monto+"&access_token="+access_token+"\"," +
                "\"cancel_url\": \"http://www.sagautobuses.com/\"" +
            "}}";
            string res = domain + "/v2/checkout/orders";
            var requestToken = (HttpWebRequest)WebRequest.Create(res);
            requestToken.Method = "POST";
            requestToken.Headers["Authorization"] = "Bearer "+access_token;
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
            InsertShortId(short_id, monto, (url + "&Access_token=" + access_token));
            return "{\"url\":\""+url+"\",\"id\":\""+session+"\",\"access_token\":\""+access_token+"\"}";
        }
        public static string CapturaPaypal(string id, string access_token, string boletos,string monto)
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
            string json = "{"+
            "\"intent\": \"CAPTURE\","+
            "\"purchase_units\": [{"+
            "\"items\": [{"+
            "\"name\": \"Ticket SAG\","+
            "\"description\": \"Boleto\","+
            "\"quantity\": \""+boletos+"\","+
            "\"unit_amount\": {"+
            "\"currency_code\": \"MXN\","+
            "\"value\": \""+monto+"\"" +
            "}}],"+
            "\"amount\": {"+
            "\"currency_code\": \"MXN\","+
            "\"value\": \""+monto+"\","+
            "\"breakdown\": {"+
            "\"item_total\": {"+
            "\"currency_code\": \"MXN\","+
            "\"value\": \""+monto+"\""+
            "}}}}],"+
            "\"application_context\": {"+
            "\"return_url\": \"https://example.com/return\","+
            "\"cancel_url\": \"https://example.com/cancel\""+
            "}}";
            string res = domain + "/v2/checkout/orders/"+id+"/capture";
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
        #endregion
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
                comando.ExecuteNonQuery();//Revisar, se ejecuta de nuevo el query
                conexion.Close();
                tbl = new DataTable();
                adaptador = new NpgsqlDataAdapter(comando);
                adaptador.Fill(tbl);
                Result = "Conectado Exitoso";

            }
            return tbl;
        }
        public static string GetPagoMasterCard(string short_id,string key)
        {
            DataTable tbl;
            string sqry = "";
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            Banner Lista = new Banner();
            Lista.listado = new List<Listado>();
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetPagoMasterCard";
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@short_id", short_id));
            cmd.Parameters.Add(new SqlParameter("@key", key));
            tbl = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            tbl.Load(dr);
            conn.Close();
            foreach (DataRow row in tbl.Rows)
            {
                sqry = row["estatus"].ToString();
            }
            return sqry;
        }
        public static string AltaVisita(string ciudad, string pais, string latitud, string longitud, string ip, string estado, string cp,string dm)
        {
            DataTable tbl;
            string sqry = "";
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            Banner Lista = new Banner();
            Lista.listado = new List<Listado>();
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "AltaVisita";
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ciudad", ciudad));
            cmd.Parameters.Add(new SqlParameter("@pais", pais));
            cmd.Parameters.Add(new SqlParameter("@latitud", latitud));
            cmd.Parameters.Add(new SqlParameter("@longitud", longitud));
            cmd.Parameters.Add(new SqlParameter("@ip", ip));
            cmd.Parameters.Add(new SqlParameter("@estado", estado));
            cmd.Parameters.Add(new SqlParameter("@cp", cp));
            cmd.Parameters.Add(new SqlParameter("@dm", dm));
            //cmd.Parameters.Add(new SqlParameter("@url", url));
            tbl = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            tbl.Load(dr);
            conn.Close();
            foreach (DataRow row in tbl.Rows)
            {
                sqry = row["estatus"].ToString();
            }
            return sqry;
        }
        public static string AltaVisitaB(string ciudad, string pais, string latitud, string longitud, string ip, string estado, string cp, string dm,string url)
        {
            DataTable tbl;
            string sqry = "";
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            Banner Lista = new Banner();
            Lista.listado = new List<Listado>();
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "AltaVisitaB";
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ciudad", ciudad));
            cmd.Parameters.Add(new SqlParameter("@pais", pais));
            cmd.Parameters.Add(new SqlParameter("@latitud", latitud));
            cmd.Parameters.Add(new SqlParameter("@longitud", longitud));
            cmd.Parameters.Add(new SqlParameter("@ip", ip));
            cmd.Parameters.Add(new SqlParameter("@estado", estado));
            cmd.Parameters.Add(new SqlParameter("@cp", cp));
            cmd.Parameters.Add(new SqlParameter("@dm", dm));
            cmd.Parameters.Add(new SqlParameter("@url", url));
            tbl = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            tbl.Load(dr);
            conn.Close();
            foreach (DataRow row in tbl.Rows)
            {
                sqry = row["estatus"].ToString();
            }
            return sqry;
        }
        public static Banner GetBanner()
        {
            DataTable tbl;
            string sqry = "";
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            Banner Lista = new Banner();
            Lista.listado = new List<Listado>();
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetBanner";
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            tbl = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            tbl.Load(dr);
            conn.Close();
            foreach (DataRow row in tbl.Rows)
            {
                Listado info = new Listado();
                info.url = row["url_image"].ToString();
                info.dispositivo = row["dispositivo"].ToString();
                Lista.listado.Add(info);
            }
            return Lista;
        }
        public static string GetBoleto(string internet_sale_id)
        {
            int cont = 0;
            string res = "";
            DataTable tbl;
            string sqry = "";
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "DatosPdf";
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@InternetSaleId", internet_sale_id));
            tbl = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            tbl.Load(dr);
            conn.Close();
            foreach (DataRow row in tbl.Rows)
            {
                if (cont == 0)
                {
                    res = "{\"origen\":\"" + row["origen"].ToString() + "\",\"destino\":\"" + row["destino"].ToString() + "\",\"asiento\":\"" + row["seat_name"].ToString() + "\",\"nombre\":\"" + row["passenger_name"].ToString() + "\",\"fechaSalida\":\"" + row["FECHASALIDA"].ToString() + "\",\"fechaLlegada\":\"" + row["FECHALLEGADA"].ToString() + "\",\"total\":\"" + row["total"].ToString() + "\",\"precioVenta\":\"" + row["Sold_Price"].ToString() + "\",\"idTicket\":\"" + row["ticket_id"].ToString() + "\",\"tipoPasajero\":\"" + row["passenger_type"].ToString() +"\",\"tipopasajero\":\"" + row["tipopasajero"].ToString() +"\"}";
                }
                else
                {
                    res = res + ",{\"origen\":\"" + row["origen"].ToString() + "\",\"destino\":\"" + row["destino"].ToString() + "\",\"asiento\":\"" + row["seat_name"].ToString() + "\",\"nombre\":\"" + row["passenger_name"].ToString() + "\",\"fechaSalida\":\"" + row["FECHASALIDA"].ToString() + "\",\"fechaLlegada\":\"" + row["FECHALLEGADA"].ToString() + "\",\"total\":\"" + row["total"].ToString() + "\",\"precioVenta\":\"" + row["Sold_Price"].ToString() + "\",\"idTicket\":\"" + row["ticket_id"].ToString() + "\",\"tipoPasajero\":\"" + row["passenger_type"].ToString() +"\",\"tipopasajero\":\"" + row["tipopasajero"].ToString() +"\"}";
                }
                cont++;
            }
            return "["+res+"]";
        }
        public static bool AccesoAPP(string usuario, string psw, out DataTable tbl)
        {

            bool Result = true;
            string sqry = "select tu.id,tur.user_roles_id,tu.username, r.name from tickets_user tu inner join tickets_user_role tur on tu.id = tur.user_roles_id inner join role r on tur.role_id = r.id" +
                            " where tu.username = '" + usuario + "'";

            tbl = mandaQry(sqry);

            if (tbl.Rows.Count == 0)
                Result = false;

            return Result;
        }

        public static bool CatSalida(out DataTable tbl)
        {

            bool Result = true;
            string sqry = "SELECT SO.name FROM STOP_OFF SO INNER JOIN product pr  ON SO.id = pr.beginning_id INNER JOIN route r on r.id = SO.route_id group by SO.name order by SO.name";

            tbl = mandaQry(sqry);

            if (tbl.Rows.Count == 0)
                Result = false;

            return Result;
        }

        public static string ReservaAsientos(AsientosPasajero listaPasajero)
        {
            try
            {
                return "";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public static string ActualizaAsientos(AsientosPasajero listaPasajeros)
        {
            try
            {
                string origen = listaPasajeros.OrigenId;
                string destino = listaPasajeros.DestinoId;
                string internet = listaPasajeros.InterteSaleId;
                string OrigenPago = listaPasajeros.TipoPago;
                string IDPago = listaPasajeros.IDPago;
                string sqry = "";
                DataTable tbl;
                string Result = "";

                //HashCode Ticket = hash[0];
                for (int i = 0; i < listaPasajeros.ListaPasajeros.Count; i++)
                {
                    Result = GenId();
                    sqry = sqry + "UPDATE TRIP_SEAT SET STATUS='OCCUPIED',Ticket_Id='" + Result + "',last_updated=CURRENT_TIMESTAMP "
                    + "WHERE starting_stop_id='" + origen + "' AND ending_stop_id = '" + destino + "' and internet_sale_id = '" + internet + "' AND SEAT_NAME='" + listaPasajeros.ListaPasajeros[i].Asiento + "' " +
                    "AND passenger_name='" + listaPasajeros.ListaPasajeros[i].Nombre + "'; " +
                    "UPDATE INTERNET_SALE SET PAYED=TRUE,payment_Provider='" + OrigenPago + "',payment_origin='App' WHERE ID ='" + internet + "'; ";
                }

                tbl = mandaQry(sqry);

                return "Exito";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public static string GenId()
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[32 / 16];
            rng.GetBytes(bytes);
            BigInteger num = new BigInteger(bytes);

            string mDato = (num.GetHashCode()).ToString();
            SHA1 hash = new SHA1CryptoServiceProvider();
            StringBuilder x = new StringBuilder();
            //SHA3Managed hash = new SHA3Managed(mTamaño);
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] MessageBytes = encoding.GetBytes(mDato);
            byte[] ComputeHashBytes = hash.ComputeHash(MessageBytes);
            foreach (var item in MessageBytes) { x.Append(item.ToString("x2")); }
            return x.ToString();
        }

        public static string DesocupaAsientos(AsientosPasajero listaPasajeros)
        {
            try
            {
                DataTable tbl;
                string sqry = "";
                string origen = listaPasajeros.OrigenId;
                string destino = listaPasajeros.DestinoId;
                string internet = listaPasajeros.InterteSaleId;
                string OrigenPago = listaPasajeros.TipoPago;
                string IDPago = listaPasajeros.IDPago;

                for (int i = 0; i < listaPasajeros.ListaPasajeros.Count; i++)
                {
                    sqry = sqry + "DELETE FROM TRIP_SEAT WHERE starting_stop_id='" + origen + "' AND ending_stop_id = '" + destino + "' and internet_sale_id = '" + internet + "' AND SEAT_NAME='" + listaPasajeros.ListaPasajeros[i].Asiento + "' " +
                   "AND passenger_name='" + listaPasajeros.ListaPasajeros[i].Nombre + "'; DELETE FROM INTERNET_SALE WHERE ID='" + internet + "'; ";
                }

                tbl = mandaQry(sqry);

                return "Exito";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public static string GuardaReservacion(AsientosPasajero listaPasajeros)
        {
            try
            {
                DataTable tbl;
                string sqry = "";
                string origen = listaPasajeros.OrigenId;
                string destino = listaPasajeros.DestinoId;
                string internet = listaPasajeros.InterteSaleId;
                string OrigenPago = listaPasajeros.TipoPago;
                string IDPago = listaPasajeros.IDPago;

                for (int i = 0; i < listaPasajeros.ListaPasajeros.Count; i++)
                {
                    sqry = sqry + "DELETE FROM TRIP_SEAT WHERE starting_stop_id='" + origen + "' AND ending_stop_id = '" + destino + "' and internet_sale_id = '" + internet + "' " +
                        "AND SEAT_NAME='" + listaPasajeros.ListaPasajeros[i].Asiento + "' " +
                   "AND passenger_name='" + listaPasajeros.ListaPasajeros[i].Nombre + "'; DELETE FROM INTERNET_SALE WHERE ID='" + internet + "'; ";
                }

                tbl = mandaQry(sqry);

                return "Exito";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public static string AltaUsuario(InfoUsuario listaInformacion)
        {
            string sqry = "";
            string nombre = listaInformacion.Nombre.ToUpper();
            string apellidos = listaInformacion.apellidos.ToUpper();
            string correo = listaInformacion.correo.ToLower();
            string telefono = listaInformacion.telefono;
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_TurismoOmnibus; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "InsertarRegistro";
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@nombre", nombre));
            cmd.Parameters.Add(new SqlParameter("@apellidos", apellidos));
            cmd.Parameters.Add(new SqlParameter("@email", correo));
            cmd.Parameters.Add(new SqlParameter("@telefono", telefono));
            sqry = (string)cmd.ExecuteScalar();
            conn.Close();
            return sqry;
        }

        public static string CanjeCodigo(InfoUsuarioInvitado listaInformacion)
        {
            string sqry = "";
            string nombre = (listaInformacion.Nombre).ToUpper();
            string apellidos = (listaInformacion.apellidos).ToUpper();
            string correo = listaInformacion.correo;
            string telefono = listaInformacion.telefono;
            string promo = listaInformacion.promo;
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_TurismoOmnibus; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "InsertarCodigoOcupa";
            conn.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@nombre", nombre));
            cmd.Parameters.Add(new SqlParameter("@internet_Sale_ID", 1));
            cmd.Parameters.Add(new SqlParameter("@apellidos", apellidos));
            cmd.Parameters.Add(new SqlParameter("@email", correo));
            cmd.Parameters.Add(new SqlParameter("@telefono", telefono));
            cmd.Parameters.Add(new SqlParameter("@codigoPromo", promo));
            sqry = cmd.ExecuteScalar().ToString();
            Console.WriteLine(sqry);
            Console.WriteLine();
            conn.Close();
            return sqry;
        }
        public static string CanjeCodigoW(InfoUsuarioInvitado listaInformacion)
        {
            string sqry = "";
            string nombre = (listaInformacion.Nombre).ToUpper();
            string apellidos = (listaInformacion.apellidos).ToUpper();
            string correo = listaInformacion.correo;
            string telefono = listaInformacion.telefono;
            string promo = listaInformacion.promo;
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_TurismoOmnibus; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "InsertarCodigoOcupa";
            conn.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@nombre", nombre));
            cmd.Parameters.Add(new SqlParameter("@internet_Sale_ID", 2));
            cmd.Parameters.Add(new SqlParameter("@apellidos", apellidos));
            cmd.Parameters.Add(new SqlParameter("@email", correo));
            cmd.Parameters.Add(new SqlParameter("@telefono", telefono));
            cmd.Parameters.Add(new SqlParameter("@codigoPromo", promo));
            sqry = cmd.ExecuteScalar().ToString();
            Console.WriteLine(sqry);
            Console.WriteLine();
            conn.Close();
            return sqry;
        }
        public static ListaAsientos ConsultaAsientos(string internetSaleId, string originId, string DestinId)
        {
            try
            {
                DataTable tbl;
                string sqry = "";


                sqry = sqry + "SELECT ticket_id,seat_name FROM TRIP_SEAT WHERE starting_stop_id='" + originId + "' AND ending_stop_id = '" + DestinId + "' and internet_sale_id = '" + internetSaleId + "'; ";


                tbl = mandaQry(sqry);

                ListaAsientos Lista = new ListaAsientos();
                Lista.Listado = new List<AsientosAsignados>();
                foreach (DataRow row in tbl.Rows)
                {
                    AsientosAsignados asientos = new AsientosAsignados();
                    asientos.TicketID = row["ticket_id"].ToString();
                    asientos.Asiento = row["seat_name"].ToString();
                    Lista.Listado.Add(asientos);
                }


                return Lista;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GuardaReservacions(AsientosPasajero listaPasajeros)
        {
            try
            {
                DataTable tbl;
                string sqry = "";
                string origen = listaPasajeros.OrigenId;
                string destino = listaPasajeros.DestinoId;
                string internet = listaPasajeros.InterteSaleId;
                string OrigenPago = listaPasajeros.TipoPago;
                string IDPago = listaPasajeros.IDPago;
                string FechaLlegada = DateTime.Parse(listaPasajeros.FechaLlegada).ToString("dd/MM/yyyy HH:mm.ss");
                string FechaSalida = DateTime.Parse(listaPasajeros.FechaSalida).ToString("dd/MM/yyyy HH:mm.ss");
                string IdProveedor = listaPasajeros.TipoPago == "Open Pay" ? "2" : "1";

                sqry = String.Format("InsertaPagosProcesados '{0}',{1},'{2}','{3}','{4}';", IDPago, IdProveedor, internet, FechaSalida, FechaLlegada);

                tbl = mandaQrySql(sqry);

                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static DataTable mandaQrySql(string sqry)
        {
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_TurismoOmnibus; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conexionSql = new SqlConnection(cadenaConexion);
            SqlCommand comandoSql;
            SqlDataAdapter adaptadorSql;
            DataTable tbl = new DataTable();

            string Result = string.Empty;

            if (!string.IsNullOrWhiteSpace(cadenaConexion))
            {
                comandoSql = new SqlCommand(sqry, conexionSql);
                comandoSql.CommandType = CommandType.Text;
                conexionSql.Open();
                comandoSql.ExecuteNonQuery();
                conexionSql.Close();
                tbl = new DataTable();
                adaptadorSql = new SqlDataAdapter(comandoSql);
                adaptadorSql.Fill(tbl);
                Result = "Conectado Exitoso";
            }
            return tbl;
        }
        public static string InsertarDatosViaje(DatosPasajero datosPasajero)
        {
            try
            {

                string sqry = "";
                string internetSaleId = datosPasajero.InternetSaleId;
                string nombrePasajero = datosPasajero.NombrePasajero;
                string email = datosPasajero.Email;
                string origen = datosPasajero.Origen;
                string destino = datosPasajero.Destino;
                string salida = DateTime.Parse(datosPasajero.Salida).ToString("yyyy-MM-ddTHH:mm:ss");
                string llegada = DateTime.Parse(datosPasajero.Llegada).ToString("yyyy-MM-ddTHH:mm:ss");


                sqry = String.Format("InsertaDatosPasajero '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}';",
                                      internetSaleId, nombrePasajero, email, origen, destino, salida, llegada);
                mandaDatosSql(sqry);

                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static bool mandaDatosSql(string sqry)
        {
            var Conexion = "Server = 192.168.0.245;Database = DB_TurismoOmnibus; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection ConnectionSQL = new SqlConnection(Conexion);
            SqlCommand cmmd;
            //SqlDataAdapter adaptador;
            //DataTable datos = new DataTable();

            if (!string.IsNullOrWhiteSpace(Conexion))
            {
                cmmd = new SqlCommand(sqry, ConnectionSQL);
                cmmd.CommandType = CommandType.Text;
                ConnectionSQL.Open();
                cmmd.ExecuteNonQuery();
                ConnectionSQL.Close();
                //adaptador = new SqlDataAdapter(cmmd);
                //adaptador.Fill(datos);
            }
            return true;
        }
        #region
        public static ListaEstado ListarEstados()
        {
            DataTable tbl;
            string sqry = "";
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_TurismoOmnibus; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            ListaEstado Lista = new ListaEstado();
            Lista.Estado = new List<Estados>();
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ListarEstados";
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            tbl = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            tbl.Load(dr);
            conn.Close();
            foreach (DataRow row in tbl.Rows)
            {
                Estados info = new Estados();
                info.Id = row["Id"].ToString();
                info.Nombre = row["estado"].ToString();
                Lista.Estado.Add(info);
            }
            return Lista;
        }
        public static Respuesta AltaRuleta(InfoRuleta listaRuleta)
        {
            string sqry = "";
            string correo = listaRuleta.correo.ToLower();
            string telefono = listaRuleta.telefono;
            string nombre = listaRuleta.nombre.ToUpper();
            string APaterno = listaRuleta.primerApellido.ToUpper();
            string AMaterno = listaRuleta.segundoApellido.ToUpper();
            DateTime fecha = listaRuleta.fechaNacimiento;
            string origen = listaRuleta.origen.ToUpper();
            string destino = listaRuleta.destino.ToUpper();
            int idEstado = listaRuleta.idEstado;
            string descuento = listaRuleta.descuento;

            var cadenaConexion = "Server = 192.168.0.245;Database = DB_TurismoOmnibus; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            Respuesta Lista = new Respuesta();
            Lista.resultado = new List<Resultado>();
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "RegistroRuleta";
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@correo", correo));
            cmd.Parameters.Add(new SqlParameter("@telefono", telefono));
            cmd.Parameters.Add(new SqlParameter("@nombre", nombre));
            cmd.Parameters.Add(new SqlParameter("@primerApellido", APaterno));
            cmd.Parameters.Add(new SqlParameter("@segundoApellido", AMaterno));
            cmd.Parameters.Add(new SqlParameter("@fechaNacimiento", fecha));
            cmd.Parameters.Add(new SqlParameter("@origen", origen));
            cmd.Parameters.Add(new SqlParameter("@destino", destino));
            cmd.Parameters.Add(new SqlParameter("@idEstado", idEstado));
            cmd.Parameters.Add(new SqlParameter("@descuento", descuento));
            sqry = (string)cmd.ExecuteScalar();
            conn.Close();
            Resultado info = new Resultado();
            info.status = sqry;
            Lista.resultado.Add(info);
            return Lista;
        }
        public static Respuesta ValidarCorreoRuleta(CorreoRuleta Correo)
        {
            string sqry = "";
            string correo = Correo.Correo.ToLower();
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_TurismoOmnibus; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            Respuesta Lista = new Respuesta();
            Lista.resultado = new List<Resultado>();
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ValidarCorreoRuleta";
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@correo", correo));
            sqry = (string)cmd.ExecuteScalar();
            conn.Close();
            Resultado info = new Resultado();
            info.status = sqry;
            Lista.resultado.Add(info);
            return Lista;
        }
        public static Respuesta ObtenerDescuento()
        {
            DataTable tbl;
            string sqry = "";
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_TurismoOmnibus; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            Respuesta Lista = new Respuesta();
            Lista.resultado = new List<Resultado>();
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "ListarDescuentos";
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            tbl = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            tbl.Load(dr);
            conn.Close();
            foreach (DataRow row in tbl.Rows)
            {
                Resultado info = new Resultado();
                info.status = row["PorcentajeDescuento"].ToString();
                Lista.resultado.Add(info);
            }
            return Lista;
        }
        public static ListaPagos ConsultaPago(string shid)
        {
            try
            {
                DataTable tbl;
                string sqry = "";

                sqry = sqry + "select trs.status, ins.SHORT_ID, ins.PAYED, trs.ticket_id from internet_sale ins inner join trip_seat trs on ins.id = trs.internet_sale_id where short_id like '" + shid + "'";

                tbl = mandaQry(sqry);

                SqlCommand comando = new SqlCommand();

                ListaPagos Lista = new ListaPagos();
                Lista.Pagados = new List<PagosRealizados>();
                foreach (DataRow row in tbl.Rows)
                {
                    PagosRealizados pagosv = new PagosRealizados();
                    pagosv.Status = row["status"].ToString();
                    pagosv.ShortId = row["short_id"].ToString();
                    pagosv.Payed = row["payed"].ToString();
                    pagosv.Ticket = row["ticket_id"].ToString();
                    Lista.Pagados.Add(pagosv);
                }
                return Lista;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static string GenLargeId()
        {
            var characters = "abcdefghijklmnopqrstuvwxyz0123456789";
            var Charsarr = new char[32];
            var Large = "";
            var random = new Random();

            for (int i = 0; i < Charsarr.Length; i++)
            {
                if ((i == 8) || (i == 13) || (i == 19))
                    Large=Large + '-';
                else
                    Large = Large + characters[random.Next(characters.Length)];
            }
            return Large;
        }
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
        public static string private_key()
        {
            var sh = "";
            var characters = "ab0cde1fghDFHYWSZXBi2AjklB3mno4pCqrsD5tuvEw6xFyz7G8JKL9";
            var Charsarr = new char[19];
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
        public static string GetSesion(infoSesion infoSesion)
        {
            string resInsert = "";
            string session = "";
            string shortId = infoSesion.short_id;
            string res = "https://evopaymentsmexico.gateway.mastercard.com/api/rest/version/61/merchant/1079224/session";
            var requestToken = (HttpWebRequest)WebRequest.Create(res);
            requestToken.Method = "POST";
            requestToken.Headers["Authorization"] = "Basic bWVyY2hhbnQuMTA3OTIyNDpjNGUyYzAwY2JjYjc5NDNmOTEyZGZmZTI1ZDFiNTc1ZQ==";
            requestToken.ContentType = "application/json";
            string json = "{\"apiOperation\":\"CREATE_CHECKOUT_SESSION\"," +
                  "\"interaction\":{" +
                  "\"operation\": \"PURCHASE\","+
                  "\"cancelUrl\": \"https://image.shutterstock.com/z/stock-vector-grunge-red-cancelled-with-star-icon-round-rubber-seal-stamp-on-white-background-789630058.jpg\","+
                  "\"displayControl\": {"+
                  "\"orderSummary\": \"SHOW\","+
                  "\"billingAddress\": \"HIDE\"},"+
                  "\"locale\": \"es_MX\","+
                  "\"returnUrl\": \"https://thumbs.dreamstime.com/z/mobile-payment-nfc-smart-phone-concept-flat-icon-black-image-white-background-mobile-payment-nfc-smart-phone-concept-flat-icon-126060095.jpg\","+
                  "\"timeout\": \"600\","+
                  "\"timeoutUrl\": \"https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcTuOfL1_7BKsBXyZiGx21Ok_ozIdodDmuPeq1eSD85bS92UvKPZ&usqp=CAU\"},"+
                  "\"order\": {"+
                  "\"amount\": \""+infoSesion.monto+"\","+
                  "\"customerReference\": \"Boletos SAG AUTOBUSES\","+
                  "\"currency\": \"MXN\","+
                  "\"description\":\""+infoSesion.descripcion+"\","+
                  "\"id\": \""+shortId+"\"}}";
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
                        session= TC.session.id;
                    }
                }
            }
            InsertShortId(shortId, infoSesion.monto, session);
            try
            {
                string qry = "";
                qry = "UPDATE internet_sale set payment_provider = 'mastercard app' WHERE short_id='" + shortId + "';";
                mandaQry(qry);
                res = "1";
            }
            catch (Exception ex)
            {
                res = ex.ToString();
            }
            return "http://www.sagautobuses.com/Pago?Session=" + session/*+"&id="+shortId*/;
        }
        public static string GetSesionW(infoSesionMC infoSesion)
        {
            string pk = private_key();
            string session = "";
            string internet_sale_id=infoSesion.internet_sale_id;
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
                  "\"returnUrl\": \"http://www.sagautobuses.com/Boletos?Token=" + internet_sale_id+"&Private_key=" + pk+"&Boleto="+shortId+"&Version=10\"," +
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
            Private_key(pk,shortId,internet_sale_id);
            InsertShortId(shortId, infoSesion.monto, session);
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
        public static string Private_key(string session,string private_key,string internet_sale)
        {
            string sqry = "";
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "InsertKey";
            conn.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@session", session));
            cmd.Parameters.Add(new SqlParameter("@private", private_key));
            cmd.Parameters.Add(new SqlParameter("@internet_sale", internet_sale));
            sqry = cmd.ExecuteScalar().ToString();
            Console.WriteLine(sqry);
            Console.WriteLine();
            conn.Close();
            return sqry;
        }
        public static string CanjePromocion(string codigo_promo, string internet_sale_id)
        {
            string sqry = "";
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_TurismoOmnibus; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "CanjePromocion";
            conn.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@codigo_promo", codigo_promo));
            cmd.Parameters.Add(new SqlParameter("@internet_sale_id", internet_sale_id));
            sqry = cmd.ExecuteScalar().ToString();
            Console.WriteLine(sqry);
            Console.WriteLine();
            conn.Close();
            return sqry;
        }
        public static string InsertData(string salida, string llegada,string internet_sale,string short_id,string metodo_pago)
        {
            string sqry = "";
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "InsertData";
            conn.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@salida", salida));
            cmd.Parameters.Add(new SqlParameter("@llegada", llegada));
            cmd.Parameters.Add(new SqlParameter("@internet_sale", internet_sale));
            cmd.Parameters.Add(new SqlParameter("@short_id", short_id));
            cmd.Parameters.Add(new SqlParameter("@metodo_pago", metodo_pago));
            sqry = cmd.ExecuteScalar().ToString();
            Console.WriteLine(sqry);
            Console.WriteLine();
            conn.Close();
            return sqry;
        }
        public static string InsertShortId(string shortId, string amount, string session)
        {
            string sh= shortId.ToString();
            string res = "";
            string qry = "",qry2="";
            try
            {
                qry = "UPDATE internet_sale set full_response='{\"sessionId\":\"" + session + "\",\"authorized\":\"false\",\"amount\":" + amount + "}',total_amount=" + amount + ",change_amount= -" + amount +" WHERE short_id='"+shortId+"'";
                mandaQry(qry);
                res = "1";
            }
            catch(Exception ex)
            {
                res = ex.ToString();
            }
            return res;
        }
        public static string UpdateBoleto(string shortId,string sale_id,string amount)
        {
            var res = "";
            string sh = shortId.ToString();
            int tick = 0, cont = 0;
            string qry = "",qryi="",ticketid= GenShortId(),qry2="";
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
            qryi = "SELECT * FROM trip_seat WHERE internet_sale_id='"+sale_id+"'";
            tbl=mandaQry(qryi);
            foreach (DataRow row in tbl.Rows)
            {
                qry2 = "UPDATE trip_seat set status = 'OCCUPIED',ticket_id='" + ticketid + "', sold_price = '" + amount + "',version=1 WHERE id='" + row["id"] + "'";
                mandaQry(qry2);
                ticketid = GenShortId();
            }
            qry = "UPDATE internet_sale set version=2,last_updated='" + Fecha()+ "',payed='true',payed_amount="+amount+ ",payment_origin='App' WHERE short_id='" + shortId + "';";
            mandaQry(qry);
            res = shortId;
            return res;
        }
        public static string UpdateBoletoMC(string shortId, string sale_id)
        {
            var res = "";
            int tick = 0,cont=0;
            string sh = shortId.ToString();
            string qry = "", qryi = "", ticketid = GenShortId(), qry2 = "";
            DataTable tbl = new DataTable();
            DataTable tbl2 = new DataTable();
            while(tick==0)
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
                    if((tbl.Rows.Count)>0)
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
        public static string ActualizarConceptoPago(infoCambio listacambio)
        {
            string descripcion = listacambio.descripcion;
            string sale_id = listacambio.short_id;
            var res = "";
            string qry = "";
            try
            {
                qry = "UPDATE internet_sale set payment_provider = '"+descripcion+"' WHERE short_id='" + sale_id + "';";
                mandaQry(qry);
                res = "1";
            }
            catch (Exception ex)
            {
                res = ex.ToString();
            }
            return res;
        }
        #endregion
        #region RefiereGana
        public static AcumuladoRefiereGana ExtraerCantidadAcumulada(CorreoRefiereGana refiereGana)
        {
            string sqry = "";
            string email = refiereGana.Correo;

            sqry = string.Format("AcumuladoCorreo '{0}';",
                                  email);
            DataTable tbl = new DataTable();
            tbl = ExtraerDatos(sqry);
            AcumuladoRefiereGana var = new AcumuladoRefiereGana();
            var.Acumulado = Convert.ToDouble(tbl.Rows[0][0]);

            return var;
        }


        public static string InsertarRefiereGana(CanjeRefiereGana canjeRefiereGana)
        {
            string sqry = "";
            string email = canjeRefiereGana.email;
            string internetSaleId = canjeRefiereGana.intenetSaleID;
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_TurismoOmnibus; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "RefiereGana";
            conn.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@InternetSaleID ", internetSaleId));
            cmd.Parameters.Add(new SqlParameter("@Email", email));
            sqry = cmd.ExecuteScalar().ToString();
            Console.WriteLine(sqry);
            Console.WriteLine();
            conn.Close();
            return sqry;
        }


        public static DataTable ExtraerDatos(string sqry)
        {
            DataTable tbl = new DataTable();
            string cadenaconexion = "Server = 192.168.0.245;Database = DB_TurismoOmnibus; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            SqlCommand comando = new SqlCommand();
            SqlDataAdapter adaptador;
            if (!string.IsNullOrWhiteSpace(cadenaconexion))
            {
                comando = new SqlCommand(sqry, conexion);
                comando.CommandType = CommandType.Text;
                conexion.Open();
                comando.ExecuteNonQuery();
                conexion.Close();
                tbl = new DataTable();
                adaptador = new SqlDataAdapter(comando);
                adaptador.Fill(tbl);
            }
            return tbl;
        }
        #endregion
        #region Blog
        public static string ListarCategorias()
        {
            int cont = 0;
            string sqry = "SELECT * FROM cat_categoriaBlog";
            DataTable tbl = new DataTable();
            string cadenaconexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            SqlCommand comando = new SqlCommand();
            SqlDataAdapter adaptador;
            if (!string.IsNullOrWhiteSpace(cadenaconexion))
            {
                comando = new SqlCommand(sqry, conexion);
                comando.CommandType = CommandType.Text;
                conexion.Open();
                comando.ExecuteNonQuery();
                conexion.Close();
                tbl = new DataTable();
                adaptador = new SqlDataAdapter(comando);
                adaptador.Fill(tbl);
                sqry = "";
                foreach (DataRow row in tbl.Rows)
                {
                    if(cont>0)
                        sqry = sqry + ",{\"id_categoria\":\"" + row["id_categoria"].ToString() + "\",\"descripcion\":\"" + row["descripcion"] + "\",\"activo\":\"" + row["activo"] + "\",\"fechaCreacion\":\"" + row["fecha_creacion"] + "\",\"fechaModificacion\":\"" + row["fecha_modificacion"] + "\"}";
                    else
                        sqry = sqry + "{\"id_categoria\":\"" + row["id_categoria"].ToString() + "\",\"descripcion\":\"" + row["descripcion"] + "\",\"activo\":\"" + row["activo"] + "\",\"fechaCreacion\":\"" + row["fecha_creacion"] + "\",\"fechaModificacion\":\"" + row["fecha_modificacion"] + "\"}";
                    cont++;
                }
            }
            return "{\"categorias\":["+sqry+"]}";
        }
        public static string AltaCategoria(AltaCat listaCat)
        {
            string sqry = "INSERT INTO cat_categoriaBlog (descripcion,activo,fecha_creacion,fecha_modificacion) values ('"+listaCat.Categoria+"',1,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP)";
            DataTable tbl = new DataTable();
            string cadenaconexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            SqlCommand comando = new SqlCommand();
            SqlDataAdapter adaptador;
            if (!string.IsNullOrWhiteSpace(cadenaconexion))
            {
                comando = new SqlCommand(sqry, conexion);
                comando.CommandType = CommandType.Text;
                try
                {
                    conexion.Open();
                    conexion.Close();
                    tbl = new DataTable();
                    adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(tbl);
                    sqry = "1";
                }
                catch (Exception ex)
                {
                    sqry = "0";
                }

            }
            return sqry;
        }
        public static string EliminarCategoria(EliminarCat listaCat)
        {
            string sqry = "DELETE FROM cat_categoriaBlog WHERE id_categoria="+listaCat.id;
            DataTable tbl = new DataTable();
            string cadenaconexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            SqlCommand comando = new SqlCommand();
            SqlDataAdapter adaptador;
            if (!string.IsNullOrWhiteSpace(cadenaconexion))
            {
                comando = new SqlCommand(sqry, conexion);
                comando.CommandType = CommandType.Text;
                try
                {
                    conexion.Open();
                    conexion.Close();
                    tbl = new DataTable();
                    adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(tbl);
                    sqry = "1";
                }
                catch (Exception ex)
                {
                    sqry = "0";
                }

            }
            return sqry;
        }
        public static string ModificarCategoria(ModificarCat listaCat)
        {
            string sqry = "UPDATE cat_categoriaBlog SET descripcion='"+listaCat.Categoria+"',activo="+listaCat.Activo+" WHERE id_categoria="+listaCat.Id;
            DataTable tbl = new DataTable();
            string cadenaconexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            SqlCommand comando = new SqlCommand();
            SqlDataAdapter adaptador;
            if (!string.IsNullOrWhiteSpace(cadenaconexion))
            {
                comando = new SqlCommand(sqry, conexion);
                comando.CommandType = CommandType.Text;
                try
                {
                    conexion.Open();
                    conexion.Close();
                    tbl = new DataTable();
                    adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(tbl);
                    sqry = "1";
                }
                catch (Exception ex)
                {
                    sqry = "0";
                }

            }
            return sqry;
        }
        public static string LoginSAG(Login listaLogin)
        {
            DataTable tbl = new DataTable();
            SqlDataAdapter adaptador;
            string sqry = "";
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "Login";
            conn.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Usuario ", listaLogin.usuario));
            cmd.Parameters.Add(new SqlParameter("@Password", listaLogin.pasword));
            tbl = new DataTable();
            adaptador = new SqlDataAdapter(cmd);
            adaptador.Fill(tbl);
            foreach (DataRow row in tbl.Rows)
            {
                if (row["Status"].ToString() == "1")
                    sqry = "\"id_usuario\":\"" + row["id_usuario"].ToString() + "\",\"nombre\":\"" + row["nombre"].ToString() + "\",\"usuario\":\"" + row["usuario"] + "\",\"apellido_paterno\":\"" + row["apellido_paterno"] + "\",\"apellido_materno\":\"" + row["apellido_materno"] + "\",\"puesto\":\"" + row["Puesto"] + "\",\"departamento\":\"" + row["Departamento"] + "\"";
                else
                    sqry = "0";
            }
            conn.Close();
            return sqry;
        }
        public static string ActualizarImagenes(Imagenes listaimagenes)
        {
            string sqry = "";
            if((listaimagenes.imagen1!="")&& (listaimagenes.imagen2 =="")&&(listaimagenes.imagen3 == ""))
                sqry = "UPDATE cat_post SET principal_1='"+listaimagenes.imagen1+"' WHERE id_post=1";
            else if ((listaimagenes.imagen1 != "") && (listaimagenes.imagen2 != "") && (listaimagenes.imagen3 == ""))
                sqry = "UPDATE cat_post SET principal_1='" + listaimagenes.imagen1 + "',principal_2='" + listaimagenes.imagen2 + "' WHERE id_post=1";
            else if ((listaimagenes.imagen1 != "") && (listaimagenes.imagen2 != "") && (listaimagenes.imagen3 != ""))
                sqry = "UPDATE cat_post SET principal_1='" + listaimagenes.imagen1 + "',principal_2='" + listaimagenes.imagen2 + "',principal_3='" + listaimagenes.imagen3 + "' WHERE id_post=1";
            DataTable tbl = new DataTable();
            string cadenaconexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            SqlCommand comando = new SqlCommand();
            SqlDataAdapter adaptador;
            if (!string.IsNullOrWhiteSpace(cadenaconexion))
            {
                comando = new SqlCommand(sqry, conexion);
                comando.CommandType = CommandType.Text;
                try
                {
                    conexion.Open();
                    conexion.Close();
                    tbl = new DataTable();
                    adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(tbl);
                    sqry = "1";
                }
                catch (Exception ex)
                {
                    sqry = "0";
                }

            }
            return sqry;
        }
        public static string ListarPosts()
        {
            int cont = 0;
            string sqry = "SELECT * FROM cat_post ORDER BY fecha_creacion desc";
            DataTable tbl = new DataTable();
            string cadenaconexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            SqlCommand comando = new SqlCommand();
            SqlDataAdapter adaptador;
            if (!string.IsNullOrWhiteSpace(cadenaconexion))
            {
                comando = new SqlCommand(sqry, conexion);
                comando.CommandType = CommandType.Text;
                try
                {
                    sqry = "";
                    conexion.Open();
                    conexion.Close();
                    tbl = new DataTable();
                    adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(tbl);
                    foreach (DataRow row in tbl.Rows)
                    {
                        if (cont > 0)
                            sqry = sqry + ",{\"id_post\":\"" + row["id_post"].ToString() + "\",\"principal_1\":\"" + row["principal_1"] + "\",\"principal_2\":\"" + row["principal_2"] + "\",\"principal_3\":\"" + row["principal_3"] + "\",\"titulo\":\"" + row["titulo"] + "\",\"categoria\":\"" + row["categoria"] + "\",\"sinopsis\":\"" + row["sinopsis"] + "\",\"fecha_creacion\":\"" + row["fecha_creacion"] + "\",\"fecha_modificacion\":\"" + row["fecha_modificacion"] + "\",\"contenido\":\"" + row["contenido"] + "\",\"activo\":\"" + row["activo"] + "\"}";
                        else
                            sqry = sqry + "{\"id_post\":\"" + row["id_post"].ToString() + "\",\"principal_1\":\"" + row["principal_1"] + "\",\"principal_2\":\"" + row["principal_2"] + "\",\"principal_3\":\"" + row["principal_3"] + "\",\"titulo\":\"" + row["titulo"] + "\",\"categoria\":\"" + row["categoria"] + "\",\"sinopsis\":\"" + row["sinopsis"] + "\",\"fecha_creacion\":\"" + row["fecha_creacion"] + "\",\"fecha_modificacion\":\"" + row["fecha_modificacion"] + "\",\"contenido\":\"" + row["contenido"] + "\",\"activo\":\"" + row["activo"] + "\"}";
                        cont++;
                    }
                    return "{\"post\":[" + sqry + "]}";
                }
                catch (Exception ex)
                {
                    sqry = "0";
                }

            }
            return sqry;
        }
        public static string GetPost(Post listapost)
        {
            string sqry = "SELECT * FROM cat_post WHERE id_post="+listapost.Id;
            DataTable tbl = new DataTable();
            string cadenaconexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            SqlCommand comando = new SqlCommand();
            SqlDataAdapter adaptador;
            if (!string.IsNullOrWhiteSpace(cadenaconexion))
            {
                comando = new SqlCommand(sqry, conexion);
                comando.CommandType = CommandType.Text;
                try
                {
                    sqry = "";
                    conexion.Open();
                    conexion.Close();
                    tbl = new DataTable();
                    adaptador = new SqlDataAdapter(comando);
                    adaptador.Fill(tbl);
                    foreach (DataRow row in tbl.Rows)
                    {
                        sqry = "\"id_post\":\"" + row["id_post"].ToString() + "\",\"principal_1\":\"" + row["principal_1"] + "\",\"principal_2\":\"" + row["principal_2"] + "\",\"principal_3\":\"" + row["principal_3"] + "\",\"titulo\":\"" + row["titulo"] + "\",\"categoria\":\"" + row["categoria"] + "\",\"sinopsis\":\"" + row["sinopsis"] + "\",\"fecha_creacion\":\"" + row["fecha_creacion"] + "\",\"fecha_modificacion\":\"" + row["fecha_modificacion"] + "\",\"contenido\":\"" + row["contenido"] + "\",\"activo\":\"" + row["activo"] + "\"";
                    }
                }
                catch (Exception ex)
                {
                    sqry = "0";
                }

            }
            return sqry;
        }
        public static string VersionSAG()
        {
            string sqry = "";
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_TurismoOmnibus; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select * from version_app";
            conn.Open();
            sqry = cmd.ExecuteScalar().ToString();
            Console.WriteLine(sqry);
            Console.WriteLine();
            conn.Close();
            return sqry;
        }
        #endregion
        #region ChatBot
        public static string GetChatBot()
        {
            DataTable tbl = new DataTable();
            SqlDataAdapter adaptador;
            string sqry = "";
            int cont = 0;
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "ObtenerChat";
            conn.Open();
            tbl = new DataTable();
            adaptador = new SqlDataAdapter(cmd);
            adaptador.Fill(tbl);
            foreach (DataRow row in tbl.Rows)
            {
                if(cont==0)
                    sqry = "{\"id_chat\":\"" + row["id_chat"].ToString() + "\",\"fecha_creacion\":\"" + row["fecha_creacion"].ToString() + "\",\"asunto\":\"" + row["asunto"] + "\",\"nombre\":\"" + row["nombre"] + "\",\"telefono\":\"" + row["telefono"] + "\"}";
                else
                    sqry = sqry + ",{\"id_chat\":\"" + row["id_chat"].ToString() + "\",\"fecha_creacion\":\"" + row["fecha_creacion"].ToString() + "\",\"asunto\":\"" + row["asunto"] + "\",\"nombre\":\"" + row["nombre"] + "\",\"telefono\":\"" + row["telefono"] + "\"}";
                cont++;
            }
            conn.Close();
            return "[" + sqry + "]";
        }
        public static string CerrarConversacion(string id_chat,string id_usuario)
        {
            DataTable tbl = new DataTable();
            SqlDataAdapter adaptador;
            string sqry = "";
            int cont = 0;
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "CerrarConversacion";
            conn.Open();
            cmd.Parameters.Add(new SqlParameter("@id_chat", id_chat));
            cmd.Parameters.Add(new SqlParameter("@id_usuario", id_usuario));
            tbl = new DataTable();
            adaptador = new SqlDataAdapter(cmd);
            adaptador.Fill(tbl);
            foreach (DataRow row in tbl.Rows)
            {
                sqry = "1";
            }
            conn.Close();
            return sqry;
        }
        public static string GetMsgChatBot(string id_chat)
        {
            DataTable tbl = new DataTable();
            SqlDataAdapter adaptador;
            string sqry = "";
            int cont = 0;
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "ObtenerMsgChat";
            conn.Open();
            cmd.Parameters.Add(new SqlParameter("@id_chat", id_chat));
            tbl = new DataTable();
            adaptador = new SqlDataAdapter(cmd);
            adaptador.Fill(tbl);
            foreach (DataRow row in tbl.Rows)
            {
                if (cont == 0)
                    sqry = "{\"id_mensaje\":\"" + row["id_mensaje"].ToString() + "\",\"persona\":\"" + row["persona"].ToString() + "\",\"mensaje\":\"" + row["mensaje"] + "\",\"fecha_creacion\":\"" + row["fecha_creacion"] + "\"}";
                else
                    sqry = sqry + ",{\"id_mensaje\":\"" + row["id_mensaje"].ToString() + "\",\"persona\":\"" + row["persona"].ToString() + "\",\"mensaje\":\"" + row["mensaje"] + "\",\"fecha_creacion\":\"" + row["fecha_creacion"] + "\"}";
                cont++;
            }
            conn.Close();
            return "["+sqry+"]";
        }
        public static string AltaChatBot(Conversacion listaMensajes)
        {
            string sqry = "";
            var cadenaConexion = "Server = 192.168.0.245;Database = DB_PortalSAG; Connection Timeout = 2; Pooling = false;User ID = sa; Password = Med*8642; Trusted_Connection = False";
            SqlConnection conn = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            cmd = new SqlCommand(sqry, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "AltaChatBot";
            conn.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@asunto", listaMensajes.asunto));
            cmd.Parameters.Add(new SqlParameter("@nombre", listaMensajes.nombre));
            cmd.Parameters.Add(new SqlParameter("@telefono", listaMensajes.telefono));
            cmd.Parameters.Add(new SqlParameter("@correo", listaMensajes.correo));
            cmd.Parameters.Add(new SqlParameter("@descripcion", listaMensajes.descripcion));
            sqry = cmd.ExecuteScalar().ToString();
            var id_chat = sqry;
            sqry = "";
            conn.Close();
            if (sqry != "0")
            {
                for (var x = 0; x < (listaMensajes.mensaje.Count); x++)
                {
                    cmd = new SqlCommand(sqry, conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "AltaMensajeChatBot";
                    conn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@id_chat", id_chat));
                    cmd.Parameters.Add(new SqlParameter("@persona", listaMensajes.mensaje[x].persona));
                    cmd.Parameters.Add(new SqlParameter("@mensaje", listaMensajes.mensaje[x].mensaje));
                    sqry = cmd.ExecuteScalar().ToString();
                    conn.Close();
                }
            }
            return sqry;
        }
        #endregion ChatBot
        #region Clases
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

        #endregion Clases
    }
}