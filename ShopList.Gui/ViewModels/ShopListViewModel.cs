using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShopList.Gui.Models;
using ShopList.Gui.Persistence;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace ShopList.Gui.ViewModels
{
    public partial class ShopListViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _nombreDelArticulo = string.Empty;

        [ObservableProperty]
        private int _cantidadAComprar = 1;

        [ObservableProperty]
        private Item? _itemSeleccionado = null;

        [ObservableProperty]
        private ObservableCollection<Item> _items = null;

        private ShopListDataBase? _database = null;

        public ShopListViewModel()
        {
            _database = new ShopListDataBase();
            Items = new ObservableCollection<Item>();
            GetItems();

            if (Items.Count > 0)
            {
                ItemSeleccionado = Items.First();
            }
            else
            {
                ItemSeleccionado = null;
            }
        }

        [RelayCommand]
        public async Task AgregarShopListItem()
        {
            if (string.IsNullOrEmpty(NombreDelArticulo) || CantidadAComprar <= 0)
                return;

            var item = new Item
            {
                Nombre = NombreDelArticulo,
                Cantidad = CantidadAComprar,
                Comprado = false
            };

            await _database.SaveItemAsync(item);
            GetItems();

            ItemSeleccionado = item;
            NombreDelArticulo = string.Empty;
            CantidadAComprar = 1;
        }

        [RelayCommand]
        public void EliminarShopListItem()
        {
            if (ItemSeleccionado == null)
                return;

            if (Items.Contains(ItemSeleccionado))
                Items.Remove(ItemSeleccionado);

            ItemSeleccionado = null;
        }

        private async void GetItems()
        {
            IEnumerable<Item> itemsFromDb = await _database.GetAllItemAsync();
            Items = new ObservableCollection<Item>(itemsFromDb);
        }

        private void CargarDatos()
        {
            Items.Add(new Item() { Id = 1, Nombre = "Leche", Cantidad = 2, Comprado = false });
            Items.Add(new Item() { Id = 2, Nombre = "Pan de caja", Cantidad = 1, Comprado = false });
            Items.Add(new Item() { Id = 3, Nombre = "Jamón", Cantidad = 500, Comprado = false });
        }
    }
}
