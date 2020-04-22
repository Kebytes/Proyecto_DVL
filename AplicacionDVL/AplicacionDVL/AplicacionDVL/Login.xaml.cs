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
	public partial class Login : ContentPage
	{
		public Login ()
		{
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (Application.Current.Properties.ContainsKey("Usuario"))
                await((NavigationPage)this.Parent).PushAsync(new MainPage());
            if (Application.Current.Properties.ContainsKey("Ciudades"))
                await((NavigationPage)this.Parent).PushAsync(new MainPage());
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Usuario.Text) || string.IsNullOrEmpty(Password.Text))
            {
                await DisplayAlert("Un momento...", "Llenar todos los campos.", "Ok");
            }

            else
            {
                string email = Usuario.Text.ToString();
                string password = Password.Text.ToString();
                //List<Pedido> listado = await API.Login.Pedidos();
                //Cliente cliente = await API.Login.IniciarSesion(email, password);
                string json = await API.Login.IniciarSesion(email, password);
                Clientes cliente = JsonConvert.DeserializeObject<Clientes>(json);

                if (cliente != null)
                {
                    //Información del usuario logeado
                    Application.Current.Properties["Usuario"] = json;
                    //Información de las estaciones del usuario logeado
                    string estaciones = await Estaciones_Controller.GetEstacionesPorId(cliente.id_Clientes.ToString());
                    string pedidos = await Pedidos_Controller.GetPedidosOnly(cliente.id_Clientes);
                    Application.Current.Properties["Pedidos"] = pedidos;
                    Application.Current.Properties["Estaciones"] = estaciones;
                    await Application.Current.SavePropertiesAsync();
                    await((NavigationPage)this.Parent).PushAsync(new MainPage()); //MainPage
                    //await Navigation.PopAsync();
                }
            }
        }
    }
}