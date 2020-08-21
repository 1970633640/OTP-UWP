using OTP_UWP.DataClass;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace OTP_UWP.Functions
{
    public static class SqlAccess
    {
        public async static Task Add_Item(int type,string issuer, string name, string secret, int algorithm,int digits,int period,int logoType,string logo)
        {
            var databasePath = Path.Combine(ApplicationData.Current.RoamingFolder.Path, "Otp.db");
            var db = new SQLiteAsyncConnection(databasePath);
            var item = new OtpItem()
            {
                Type = type, Issuer = issuer, Name = name, Secret = secret, Algorithm = algorithm, Digits = digits, Period = period, LogoType=logoType, Logo=logo, Group = 0, Priority=0
            };
            await db.InsertAsync(item);
            await db.CloseAsync();
        }

        public static void Delete_Item(int id)
        {
            var databasePath = Path.Combine(ApplicationData.Current.RoamingFolder.Path, "Otp.db");
            var db = new SQLiteConnection(databasePath);
            _ = db.Delete(db.Get<OtpItem>(id));
            db.Close();
        }

        public static async Task <OtpItem> Get_Item(int id)
        {
            var databasePath = Path.Combine(ApplicationData.Current.RoamingFolder.Path, "Otp.db");
            var db = new SQLiteAsyncConnection(databasePath);
            var result= await db.GetAsync<OtpItem>(id);
            await db.CloseAsync();
            return result;
        }

        public static async Task Update_Item(OtpItem item)
        {
            var databasePath = Path.Combine(ApplicationData.Current.RoamingFolder.Path, "Otp.db");
            var db = new SQLiteAsyncConnection(databasePath);
            await db.UpdateAsync(item);
            await db.CloseAsync();
        }

        public async static void Init_Database()
        {
            //await ApplicationData.Current.LocalFolder.CreateFileAsync("Otp.db", CreationCollisionOption.OpenIfExists);
            var databasePath = Path.Combine(ApplicationData.Current.RoamingFolder.Path, "Otp.db");
            var db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTableAsync<OtpItem>();
            Console.WriteLine("Table created!");
            await db.CloseAsync();
        }

        public static async Task<List<OtpItem>> Get_All_Item()
        {
            var databasePath = Path.Combine(ApplicationData.Current.RoamingFolder.Path, "Otp.db");
            var db = new SQLiteAsyncConnection(databasePath);
            var query = db.Table<OtpItem>();
            var result = await query.ToListAsync();
            await db.CloseAsync();
            return result;
        }
    }
}
