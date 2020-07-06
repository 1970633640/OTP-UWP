using OtpNet;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace OTP_UWP.Functions
{
    public static class SteamCode
    {
        private static char[] STEAMCHARS = new char[] {
            '2', '3', '4', '5', '6', '7', '8', '9', 'B', 'C',
            'D', 'F', 'G', 'H', 'J', 'K', 'M', 'N', 'P', 'Q',
            'R', 'T', 'V', 'W', 'X', 'Y' };

        //https://github.com/andOTP/andOTP/blob/master/app/src/main/java/org/shadowice/flocke/andotp/Utilities/TokenCalculator.java
        public static string GenerateCode(string secret)
        {
            string ans = "";
            var totp = new Totp(Base32Encoding.ToBytes(secret), mode: OtpHashMode.Sha1,totpSize:10);
            int fullToken = int.Parse(totp.ComputeTotp());
            for (int i = 0; i < 5; i++)
            {
                ans+=STEAMCHARS[fullToken % STEAMCHARS.Length];
                fullToken /= STEAMCHARS.Length;
            }
            return ans;
        }


        //algorithm 2 https://github.com/geel9/SteamAuth/blob/master/SteamAuth/SteamGuardAccount.cs
        /*
        private static byte[] steamGuardCodeTranslations = new byte[] { 50, 51, 52, 53, 54, 55, 56, 57, 66, 67, 68, 70, 71, 72, 74, 75, 77, 78, 80, 81, 82, 84, 86, 87, 88, 89 };
        public static string GenerateSteamGuardCodeForTime(string secret)
        {

            long time = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            byte[] sharedSecretArray = Base32Encoding.ToBytes(secret);
            byte[] timeArray = new byte[8];

            time /= 30L;

            for (int i = 8; i > 0; i--)
            {
                timeArray[i - 1] = (byte)time;
                time >>= 8;
            }

            HMACSHA1 hmacGenerator = new HMACSHA1();
            hmacGenerator.Key = sharedSecretArray;
            byte[] hashedData = hmacGenerator.ComputeHash(timeArray);
            byte[] codeArray = new byte[5];
            try
            {
                byte b = (byte)(hashedData[19] & 0xF);
                int codePoint = (hashedData[b] & 0x7F) << 24 | (hashedData[b + 1] & 0xFF) << 16 | (hashedData[b + 2] & 0xFF) << 8 | (hashedData[b + 3] & 0xFF);
                for (int i = 0; i < 5; ++i)
                {
                    codeArray[i] = steamGuardCodeTranslations[codePoint % steamGuardCodeTranslations.Length];
                    codePoint /= steamGuardCodeTranslations.Length;
                }
            }
            catch (Exception)
            {
                return null; //Change later, catch-alls are bad!
            }
            return Encoding.UTF8.GetString(codeArray);
        }
        */
    }
}
