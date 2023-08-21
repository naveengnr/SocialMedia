namespace EFExample.Secutiy
{
    public class Dcrypt
    {
       
            public static string DecryptPassword(string EncryptedText)
            {
                var base64EncodedBytes = System.Convert.FromBase64String(EncryptedText);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
        }
    }