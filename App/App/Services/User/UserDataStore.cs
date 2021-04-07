using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Services.User
{
    public class UserDataStore: IDataStore<Models.User>
    {
        public Task<bool> AddItemAsync(Models.User item)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(Models.User item)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Models.User> GetItemAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Models.User>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new System.NotImplementedException();
        }
    }
}