using SQLite;

namespace OTP_UWP.DataClass
{
    public class OtpItem
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public int Type { get; set; } //0 totp, 1 steam
		public string Issuer { get; set; }
		public string Name { get; set; }
		public string Secret { get; set; }
		public int Algorithm { get; set; } //0 SHA1,1 SHA256,2 SHA512, default: 0
		public int Digits { get; set; } //4-10, default: 6
		public int Period { get; set; } //default: 30
	}
}
