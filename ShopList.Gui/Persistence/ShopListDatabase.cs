using SQLite;
using ShopList.Gui.Persistence.Configuration;
using ShopList.Gui.Models;


namespace ShopList.Gui.Persistence
{
    public class ShopListDataBase
    {
        private SQLiteAsyncConnection? _connection;

        private async Task InitAsync()
        {
            if (_connection != null)
            {
                return;
            }

            _connection = new SQLiteAsyncConnection(
                Constants.DatabasePath, 
                Constants.Flags
                );
            await _connection.CreateTableAsync<Item>();
        }
        public async Task<int> SaveItemAsync(Item item)
        {
            await InitAsync();
            return await _connection.InsertAsync(item);
        }
        public async Task<IEnumerable<Item>> GetAllItemAsync()
        {
            await InitAsync();
            return await _connection!.Table<Item>()
                .ToListAsync();
        }
        public async Task<int> RemoveItemAsync(Item item)
        {
            await InitAsync();
            return await _connection!.DeleteAsync(item);
        }
    }
}
