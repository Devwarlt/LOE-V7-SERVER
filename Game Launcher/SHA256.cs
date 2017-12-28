using System.Security.Cryptography;
using System.Text;

namespace Game_Launcher
{
    public class SHA256
    {
        private string _type { get; set; }
        private string _salt { get; set; }

        public SHA256(byte type, string salt)
        {
            _type = type.ToString();
            _salt = salt;
        }

        public string Encrypt()
        {
            SHA256Managed crypt = new SHA256Managed();
            string hash = string.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes($"{_type}{_salt}"), 0, Encoding.ASCII.GetByteCount($"{_type}{_salt}"));
            foreach (byte theByte in crypto)
                hash += theByte.ToString("x2");
            return hash;
        }
    }
}