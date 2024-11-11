using Family_Office.DataAccess;
using Family_Office.Models;
using Family_Office.ViewModel;

namespace Family_Office.Inteface
{
    public class SQLiteDataService : IDataService
    {
        public async Task<IEnumerable<BankAccount>> GetBankAccountsAsync()
        {
            return await Task.Run(() => BankAccountDataAccess.GetBankAccounts());
        }

        public async Task<IEnumerable<CashAsset>> GetCashAssetsAsync()
        {
            return await Task.Run(() => CashAssetDataAccess.GetCashAssets());
        }

        public async Task<BankAccount> CreateBankAccountAsync(BankAccount account)
        {
            await Task.Run(() => BankAccountDataAccess.AddBankAccount(account));
            return account;
        }

        public async Task<BankAccount> UpdateBankAccountAsync(BankAccount account)
        {
            await Task.Run(() => BankAccountDataAccess.UpdateBankAccount(account));
            return account;
        }

        public async Task<CashAsset> CreateCashAssetAsync(CashAsset asset)
        {
            await Task.Run(() => CashAssetDataAccess.AddCashAsset(asset));
            return asset;
        }

        public async Task<CashAsset> UpdateCashAssetAsync(CashAsset asset)
        {
            await Task.Run(() => CashAssetDataAccess.UpdateCashAsset(asset));
            return asset;
        }
    }
}
