using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Study_Step_Server
{
    public class AuthOptions
    {
        public const string ISSUER = "StudyStep"; 
        public const string AUDIENCE = "StudyStepClient";
        const string KEY = "b30f1cb3fec92948969b9a07ee1f21b348ca76b71f3ed0125803b771f52692884ed35c5e8b59f1e902a58acaa04d8aa6bcc133f2629103ca9f4e8135fe97e3bb"; // key
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
