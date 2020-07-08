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
        }

        public async static void Init_Database()
        {
            //await ApplicationData.Current.LocalFolder.CreateFileAsync("Otp.db", CreationCollisionOption.OpenIfExists);
            var databasePath = Path.Combine(ApplicationData.Current.RoamingFolder.Path, "Otp.db");
            var db = new SQLiteAsyncConnection(databasePath);
            await db.CreateTableAsync<OtpItem>();
            Console.WriteLine("Table created!");
        }

        public static async Task<List<OtpItem>> Get_All_Item()
        {
            var databasePath = Path.Combine(ApplicationData.Current.RoamingFolder.Path, "Otp.db");
            var db = new SQLiteAsyncConnection(databasePath);
            var query = db.Table<OtpItem>();
            var result = await query.ToListAsync();
            return result;
        }
    }
}
