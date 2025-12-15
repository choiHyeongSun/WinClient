using System.Text;

namespace WinClient.Sources.Utilities
{
    internal class ConvertUtility
    {
        public static void ByteConvertByString(ref byte[] destination, string source)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            int minLen = Math.Min(bytes.Length, destination.Length);
            Array.Copy(bytes, destination, minLen);
        }
    }
}