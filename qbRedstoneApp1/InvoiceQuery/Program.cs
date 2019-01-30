using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Web; 
using Interop.QBXMLRP2; 

namespace RedstoneDataEngineering
{
    class RedstoneInvoiceQuery
    {
        private static string appName = "QB Redstone App1"; 

        private static void LogQuickBooksData(string filePath, string dataStr)
        {
            StreamWriter sw = new StreamWriter(filePath);
            sw.WriteLine(dataStr);
            sw.Flush();
            sw.Close(); 
        }

        private static void BuildInvoiceQueryRq(XmlDocument xmlDoc, XmlElement xmlElement)
        {

        }

        private static void InvokePHP()
        {

        }

        static void Main(string[] args)
        {


            
        }
    }
}
