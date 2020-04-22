using AplicacionDVL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionDVL.API
{
    class Login
    {
        static HttpClient cliente;

        static string BASEURL = "http://localhost:49911/api";

        public static async Task<List<Pedido>> Pedidos()
        {
            cliente = new HttpClient();

            var uri = new Uri("/pedidos"); ///pedido

            var request = new HttpRequestMessage(HttpMethod.Get, BASEURL + uri);


            var response = await cliente.SendAsync(request);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<Pedido>>(json);
                }

                else
                    return null;
            }

            catch (Exception e) { e.ToString(); return null; }
        }


        public static async Task<string> IniciarSesion(string email, string contrasena)
        {
            cliente = new HttpClient();
            //var BaseUrl = "http://localhost:49911/api/cliente";
            var BaseUrl = "http://apidvlsystem.developmxhost.com/api/cliente/getInfo";

            //var uri = new Uri("http://apidvlsystem.developmxhost.com/api/cliente/getInfo");
            //var uri = new Uri("/getInfo");

            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl);
            request.Headers.Add("Correo_Cliente", email);
            request.Headers.Add("Contrasena", contrasena);

            var response = await cliente.SendAsync(request);

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    //return JsonConvert.DeserializeObject<Cliente>(json);
                    return json;
                }

                else
                    return null;
            }

            catch (Exception e) { e.ToString(); return null; }
        }
    }
}
