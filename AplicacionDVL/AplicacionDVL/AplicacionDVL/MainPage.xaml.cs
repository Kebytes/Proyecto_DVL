using AplicacionDVL.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AplicacionDVL
{
    public partial class MainPage : MasterDetailPage
    {
        public List<MasterPageItem> menuItems { get; set; }

        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            menuItems = new List<MasterPageItem>();
            menuItems.Add(new MasterPageItem { MenuTitle = "Realizar pedido", Icon = "cesta.png", Page = typeof(Pedidos) });
            menuItems.Add(new MasterPageItem { MenuTitle = "Tus pedidos", Icon = "litros.png", Page = typeof(Historial_Pedidos) });
            //menuItems.Add(new MasterPageItem { MenuTitle = "Tus estaciones", Icon = "estacion.png", Page = typeof(Historial_Estaciones) });
            //menuItems.Add(new MasterPageItem { MenuTitle = "Agregar estación", Icon = "combustible.png", Page = typeof(AgregarEstacion) });
            menuItems.Add(new MasterPageItem { MenuTitle = "Cerrar sesión", Icon = "salida.png", Page = typeof(Login) });
            navigationDrawerList.ItemsSource = menuItems;
            Detail = new NavigationPage(new Home());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            int x = Navigation.NavigationStack.IndexOf(this) - 1;
            if (x >= 0)
            {
                var previousPage = Navigation.NavigationStack[Navigation.NavigationStack.IndexOf(this) - 1];
                Navigation.RemovePage(previousPage);
            }
        }

        private async void NavigationDrawerList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var menu = e.SelectedItem as MasterPageItem;

            if (menu.MenuTitle.Equals("Realizar pedido"))
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(menu.Page));
                IsPresented = false;
            }

            if (menu.MenuTitle.Equals("Agregar estación"))
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(menu.Page));
                IsPresented = false;
            }

            if (menu.MenuTitle.Equals("Tus estaciones"))
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(menu.Page));
                IsPresented = false;
            }

            if (menu.MenuTitle.Equals("Tus pedidos"))
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(menu.Page));
                IsPresented = false;
            }

            if (menu.MenuTitle.Equals("Cerrar sesión"))
            {
                var display = await DisplayAlert("Cerrar sesión", "¿Estás seguro de cerrar tu sesión?", "Sí", "No");
                if (display)
                {
                    Application.Current.Properties.Clear();
                    Detail = new NavigationPage((Page)Activator.CreateInstance(menu.Page));
                    IsPresented = false;
                }
            }
        }
    }
}
