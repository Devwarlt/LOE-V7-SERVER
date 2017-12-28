#region

using System;
using System.IO;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;

#endregion

namespace gameserver
{
    public class RSA
    {
        public static readonly RSA Instance = new RSA(@"
-----BEGIN RSA PRIVATE KEY-----
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
-----END RSA PRIVATE KEY-----");

/*        
-----BEGIN PUBLIC KEY-----
MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAlLt9Hv4QMVH+jC1uQ0hJ
QTTst7QShRMmvWpNng9qE+NpzdMID8ibKqWIjTIB7Ajq3UygK5Y9IJnLJtM5bFhu
cdzk3yqsFySgmmwOYAHuH4aFNgp6B0OrFJv7UMhbN4fudt5cfBVaPJBh9nOnmPFF
VZqBLbqlWdOOnofU/x8VMrZRLN9DwsjggCLAfo/nOQp4ucb7RENm5gGegxx/kCvy
anrr/UPmH2AlInH5T8LVbwABYnxmG2vED2C2d+aXrynXNB3+tfKNpXueaGwPjHNu
goIN5rcNm9Pq+Mdq5K45nLVSgl++QAJziXN09O8vWQNe3+XEayFy4IjjapiksCi2
hXVCcSUgHt2MamJWsbJxfU3CXXSKe8oVoe+Xr2YSCmcewSStBp6tP9MRc4xH1mjT
GFIxifLXJIDiNzXgTy2bBXwtDKKAsXBXfNxmFtOzuUl+meTTe6tMxtERmIfQjOB8
2vDDuYDVjOdsYqYALW/HWPCV7d1sleOD0F4NeWiC+307lgpdK2S/z1iG+XiFqjh+
Kwt8gtkD78P0C4W5Sjoi19OeFHZj9H/WjL+FarwzJdjJHkiwORpozrDkWTi3dxfH
wyOTtsd5tIJi7JGWwY2aydu0kjliiCn5tz7OJromqLwuWf85UVblbTApoUZbdykT
K1DGrdk7bxkFkZeSQOtywO0CAwEAAQ==
-----END PUBLIC KEY-----
*/

        private readonly RsaEngine engine;
        private readonly AsymmetricKeyParameter key;

        private RSA(string privPem)
        {
            key = (new PemReader(new StringReader(privPem.Trim())).ReadObject() as AsymmetricCipherKeyPair).Private;
            engine = new RsaEngine();
            engine.Init(true, key);
        }

        public string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            byte[] dat = Convert.FromBase64String(str);
            Pkcs1Encoding encoding = new Pkcs1Encoding(engine);
            encoding.Init(false, key);
            return Encoding.UTF8.GetString(encoding.ProcessBlock(dat, 0, dat.Length));
        }

        public string Encrypt(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            byte[] dat = Encoding.UTF8.GetBytes(str);
            Pkcs1Encoding encoding = new Pkcs1Encoding(engine);
            encoding.Init(true, key);
            return Convert.ToBase64String(encoding.ProcessBlock(dat, 0, dat.Length));
        }
    }
}