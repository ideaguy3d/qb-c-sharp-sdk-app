using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Web; 
using Interop.QBXMLRP2; 

namespace RedstoneDataEngineering
{
    class RedstoneInvoice
    {
        private static string appName = "QB Redstone App1";
        private static string logBase = @"C:\xampp\htdocs\qb\invoice\"; 

        private static void LogQuickBooksData(string filePath, string dataStr)
        {
            StreamWriter sw = new StreamWriter(filePath);
            sw.WriteLine(dataStr);
            sw.Flush();
            sw.Close(); 
        }

        private static void BuildInvoiceQueryRq(XmlDocument xmlReqDoc, XmlElement qbxmlMsgsRq)
        {
            // <InvoiceQueryRq>...</InvoiceQueryRq>
            XmlElement invoiceQueryRq = xmlReqDoc.CreateElement("InvoiceQueryRq");
            qbxmlMsgsRq.AppendChild(invoiceQueryRq); 

            //  
        }

        private static void BuildInvoiceAddRq(XmlDocument xmlReqDoc, XmlElement xmlElement)
        {

        }

        private static void InvokePHP()
        {

        }

        static void Main(string[] args)
        {
            // The QBXMLRP2 object
            RequestProcessor2 requestProcessor = null;

            // values QBXMLRP2 will return
            bool sessionBegun = false;
            bool connectionOpen = false;

            // base XML doc vars 
            XmlDocument xmlReqDoc = null;
            XmlElement qbxml = null;
            XmlElement qbxmlMsgsRq = null;

            // successful qbxml response vars 
            string ticket = null; 
            string responseStr = null;
            string qbXmlReqObj = null; 

            try // 1st - to initialize the base xml doc
            {
                requestProcessor = new RequestProcessor2();

                // Create a new XML document
                xmlReqDoc = new XmlDocument();
                // ROOT - <?xml version="1.0" encoding="utf-8"?>
                xmlReqDoc.AppendChild(xmlReqDoc.CreateXmlDeclaration("1.0", "utf-8", null));
                // ROOT - <?qbxml version="13.0"?>
                xmlReqDoc.AppendChild(xmlReqDoc.CreateProcessingInstruction("qbxml", "version=\"13.0\""));
                // ROOT - <QBXML>...</QBXML>
                qbxml = xmlReqDoc.CreateElement("QBXML");
                xmlReqDoc.AppendChild(xmlReqDoc.CreateElement("QBXML"));
                // child - <QBXMLMsgsRq>...</QBXMLMsgsRq>
                qbxmlMsgsRq = xmlReqDoc.CreateElement("QBXMLMsgsRq");
                qbxml.AppendChild(qbxmlMsgsRq);
                qbxmlMsgsRq.SetAttribute("onError", "stopOnError");
            }
            catch(Exception ex)
            {
                requestProcessor = null;
                LogQuickBooksData(logBase + "initialize-base-xml-doc.xml", ex.Message);
                return; 
            }

            Console.WriteLine("__>> RSM - Base Xml doc built");

            try // 2nd - to build the invoice qbXML request objects
            { 
                BuildInvoiceQueryRq(xmlReqDoc, qbxmlMsgsRq); 
                // BuildInvoiceAddRq(xmlReqDoc, qbxmlMsgsRq); 
            }
            catch(Exception ex)
            {
                requestProcessor = null;
                LogQuickBooksData(logBase + "build-qbxml-req-error.xml", ex.Message);
                return; 
            }

            Console.WriteLine("__>> RSM - qbXML request object built");
            
            try // 3rd - to make a request to QuickBooks
            {
                requestProcessor.OpenConnection("", appName);
                connectionOpen = true;
                ticket = requestProcessor.BeginSession("", QBFileMode.qbFileOpenDoNotCare);
                sessionBegun = true;

                // send request and get response
                responseStr = requestProcessor.ProcessRequest(ticket, qbXmlReqObj); 


            }
            catch(Exception ex)
            {
                requestProcessor = null;
                connectionOpen = false;
                sessionBegun = false;

            }
        }
    }
}
