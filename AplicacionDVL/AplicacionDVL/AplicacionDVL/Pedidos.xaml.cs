using AplicacionDVL.API;
using AplicacionDVL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AplicacionDVL
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Pedidos : ContentPage
	{
		public Pedidos ()
		{
			InitializeComponent ();
		}

        private void IncrementoMagna_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Magna.Text = IncrementoMagna.Value.ToString();
        }

        private void IncrementoPremium_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Premium.Text = IncrementoPremium.Value.ToString();
        }

        private void IncrementoDiesel_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Diesel.Text = IncrementoDiesel.Value.ToString();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (validarDatos())
            {
                Pedido pedido = new Pedido
                {
                    Fecha_Programada = FechaDeseada.Date,
                    id_Estacion = ListaElementos.getIdEstacion(Estacion.Items[Estacion.SelectedIndex]),
                    Estatus = "A",
                    Autotanque = Autotanque.getValor(AutotanqueOpcion.Items[AutotanqueOpcion.SelectedIndex]),
                    Litros_Magna = Int32.Parse(Magna.Text.ToString()),
                    Litros_Premium = Int32.Parse(Premium.Text.ToString()),
                    Litros_Diesel = Int32.Parse(Diesel.Text.ToString()),
                    Fecha_Entregada = DateTime.Now
                };
                if (Application.Current.Properties.ContainsKey("Usuario"))
                {
                    pedido.cliente = JsonConvert.DeserializeObject<Clientes>(Application.Current.Properties["Usuario"].ToString());
                }

                var display = await DisplayAlert("Pedido.", "¿Confirmar pedido?", "Sí", "No");

                if (display)
                {
                    Pedido temporal = await Pedidos_Controller.InsertarPedido(pedido);

                    if (temporal != null)
                    {
                        Application.Current.Properties["Pedidos"] = await Pedidos_Controller.GetPedidosOnly(temporal.cliente.id_Clientes); ;
                        await Application.Current.SavePropertiesAsync();
                        await((NavigationPage)this.Parent).PushAsync(new Historial_Pedidos());
                        await DisplayAlert("Pedido.", "Pedido realizado.", "Ok");
                    }

                    else
                        await DisplayAlert("Pedido.", "Pedido no realizado", "Aceptar");
                }
                else
                    await DisplayAlert("Pedido", "Acción cancelada", "Ok");
            }

            else
            {
                await DisplayAlert("Pedido.", "Es necesario llenar todos los campos requeridos.", "Ok");
            }
        }

        private void Diesel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Diesel.Text.Equals(""))
                IncrementoDiesel.Value = Double.Parse(Diesel.Text);
        }

        private void Premium_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Premium.Text.Equals(""))
                IncrementoPremium.Value = Double.Parse(Premium.Text);
        }

        private void Magna_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Magna.Text.Equals(""))
                IncrementoMagna.Value = Double.Parse(Magna.Text);
        }

        public bool validarDatos()
        {
            bool bandera;

            if (string.IsNullOrEmpty(FechaDeseada.Date.ToString()) || string.IsNullOrEmpty(Magna.Text) || string.IsNullOrEmpty(Premium.Text)
                || string.IsNullOrEmpty(Diesel.Text) || string.IsNullOrEmpty(AutotanqueOpcion.SelectedItem.ToString()) || string.IsNullOrEmpty(Estacion.SelectedItem.ToString()))
            {
                bandera = false;
            }
            else
                bandera = true;

            return bandera;
        }

        public class ListaElementos
        {
            public List<Models.Estacion> elementos { get; set; }

            public ListaElementos()
            {
                elementos = new List<Models.Estacion>();
                loadElementos();
            }

            public void loadElementos()
            {
                if (Application.Current.Properties.ContainsKey("Estaciones"))
                {
                    elementos = JsonConvert.DeserializeObject<List<Models.Estacion>>(Application.Current.Properties["Estaciones"].ToString());
                }
                //elementos.Add(new Elemento { Nombre_Estacion = "Despacho 1", Colonia = "Jacarandas", Calle = "Primera", Contacto = "José Luis" });
                //elementos.Add(new Elemento { Nombre_Estacion = "Despacho 2", Colonia = "Villa Nueva", Calle = "Bravo", Contacto = "Don Chuy" });
                //elementos.Add(new Elemento { Nombre_Estacion = "Despacho 3", Colonia = "Santa Rosa", Calle = "Mina", Contacto = "Luis Pérez" });
                //elementos.Add(new Elemento { Nombre_Estacion = "Despacho 4", Colonia = "Las Torres", Calle = "Panamá", Contacto = "Memo Espadas" });
            }

            public static int getIdEstacion(string nombre)
            {
                int id = 0;
                ListaElementos elemento = new ListaElementos();
                List<Models.Estacion> estaciones = elemento.elementos;
                for (int i = 0; i < estaciones.Count; i++)
                {
                    Models.Estacion xp = estaciones[i];
                    if (xp.Nombre_Estacion.Equals(nombre))
                    {
                        id = xp.id_Estaciones;
                        break;
                    }
                }
                return id;
            }
        }

        public class Autotanque
        {
            public List<Opcion> Opciones { get; set; }

            public Autotanque()
            {
                Opciones = new List<Opcion>();
                getNombres();
            }

            public void getNombres()
            {
                Opciones.Add(new Opcion { Nombre = "Propio" });
                Opciones.Add(new Opcion { Nombre = "Foraneo" });
            }

            public static string getValor(string valor)
            {
                string nombre = "";
                Autotanque elemento = new Autotanque();
                List<Opcion> estaciones = elemento.Opciones;
                for (int i = 0; i < estaciones.Count; i++)
                {
                    Opcion xp = estaciones[i];
                    if (xp.Nombre.Equals(valor))
                    {
                        nombre = xp.Nombre;
                        break;
                    }
                }
                return nombre;
            }

            public class Opcion
            {
                public string Nombre { get; set; }
            }
        }
    }
}