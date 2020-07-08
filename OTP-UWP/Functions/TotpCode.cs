using OtpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTP_UWP.Functions
{
    public static class TotpCode
    {
        public static string GenerateCode(string secret,int Algorithm,int Digits,int Period)
        {
            var totp = new Totp(Base32Encoding.ToBytes(secret), mode:(OtpHashMode) Algorithm, totpSize: Digits,step: Period);
            return totp.ComputeTotp();
        }
    }
}
