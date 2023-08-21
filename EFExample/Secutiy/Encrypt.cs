namespace EFExample.Secutiy
{
    public class Encrypt
    {
        public static string EncryptPassword(string Text)
        {
            var PlainTextBytes = System.Text.Encoding.UTF8.GetBytes(Text);
            return System.Convert.ToBase64String(PlainTextBytes);
        }
    }
}
