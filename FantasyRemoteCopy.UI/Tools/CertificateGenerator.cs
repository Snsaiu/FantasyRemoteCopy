using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace FantasyRemoteCopy.UI.Tools;

public class CertificateGenerator
{
    private static readonly string Password = "FantasyRemoteCopy";
    private static readonly string SubjectName = "CN=FantasyRemoteCopy";

    public static void CreateCertificate(string filePath)
    {
        using (var rsa = RSA.Create(2048)) // 生成2048位的RSA密钥
        {
            var request = new CertificateRequest(SubjectName, rsa, HashAlgorithmName.SHA256,
                RSASignaturePadding.Pkcs1);
            var cert = request.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));

            // 将证书保存为PFX文件
            var certBytes = cert.Export(X509ContentType.Pfx, Password); // 设定证书的密码
            File.WriteAllBytes(filePath, certBytes);
        }
    }

    public static void InstallCertificate(X509Certificate2 cert)
    {
        // 将证书安装到受信任的根证书颁发机构
        using (var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine))
        {
            store.Open(OpenFlags.ReadWrite);
            store.Add(cert);
            store.Close();
        }
    }

    public static bool IsCertificateInstalled()
    {
        using (var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine))
        {
            store.Open(OpenFlags.ReadOnly);

            foreach (var cert in store.Certificates)
                // 检查证书的主题名称
                if (cert.Subject.Contains(SubjectName))
                    return true; // 找到证书
        }

        return false; // 没有找到证书
    }
}