using System;
using System.IO;
using System.Text;
using System.Xml;
using Interop.QBXMLRP2;

namespace invoiceQuery
{
    class InvoiceQuery
    {
        private string logLocation = @"C:\Temp\PurchaseOrderAddRequest.xml";

        private static void LogTxtData(string filePath, string strTxt)
        {
            StreamWriter sw = new StreamWriter(filePath);
            sw.WriteLine(strTxt);
            sw.Flush();
            sw.Close();
        }

        private static string PurchaseOrderAdd_AddXml()
        {
            // create an XML file/document
            XmlDocument inputXMLDocPurchaseOrder = new XmlDocument();

            // <?xml version="1.0" encoding="utf-8"?>
            inputXMLDocPurchaseOrder.AppendChild(inputXMLDocPurchaseOrder.CreateXmlDeclaration("1.0", "utf-8", null));

            // <?qbxml version="2.0"?>
            inputXMLDocPurchaseOrder.AppendChild(inputXMLDocPurchaseOrder.CreateProcessingInstruction("qbxml", "version=\"13.0\""));

            // <QBXML>...</QBXML>
            XmlElement qbXML = inputXMLDocPurchaseOrder.CreateElement("QBXML");
            inputXMLDocPurchaseOrder.AppendChild(qbXML);

            // <QBXMLMsgsRq onError="stopOnError">...</QBXMLMsgsRq>              
            XmlElement qbXMLMsgsRq = inputXMLDocPurchaseOrder.CreateElement("QBXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");

            //---------------------------------------------------------------------------------------
            // THIS IS WHERE THE qbXML starts to get unique depending on the API that is being used.
            //---------------------------------------------------------------------------------------

            // <PurchaseOrderQueryRq>...</PurchaseOrderQueryRq> 
            XmlElement purchaseOrderAddRq = inputXMLDocPurchaseOrder.CreateElement("PurchaseOrderAddRq");
            qbXMLMsgsRq.AppendChild(purchaseOrderAddRq);

            // <PurchaseOrderAdd>...</PurchaseOrderAdd>
            XmlElement purchaseOrderAdd = inputXMLDocPurchaseOrder.CreateElement("PurchaseOrderAdd");
            purchaseOrderAddRq.AppendChild(purchaseOrderAdd);
            //purchaseOrderAdd.SetAttribute("defMacro", "TxnID:0001");// 

            // <VendorRef>...</VendorRef>
            XmlElement vendorRef = inputXMLDocPurchaseOrder.CreateElement("VendorRef");
            purchaseOrderAdd.AppendChild(vendorRef);
            //vendorRef.AppendChild(inputXMLDocPurchaseOrder.CreateElement("ListID")).InnerText = "0001";
            // // I think this is referring to "Vendor Name" or "Company Name" from QB
            vendorRef.AppendChild(inputXMLDocPurchaseOrder.CreateElement("FullName")).InnerText = "Spicers LLC";

            // <TxnDate>...</TxnDate>
            XmlElement txnDate = inputXMLDocPurchaseOrder.CreateElement("TxnDate");
            purchaseOrderAdd.AppendChild(txnDate);
            txnDate.InnerText = "2019-01-10";

            // <RefNumber>...</RefNumber>
            XmlElement refNumber = inputXMLDocPurchaseOrder.CreateElement("RefNumber");
            purchaseOrderAdd.AppendChild(refNumber);
            refNumber.InnerText = "1001";

            // <VendorAddress>...</VendorAddress>
            XmlElement vendorAddress = inputXMLDocPurchaseOrder.CreateElement("VendorAddress");
            purchaseOrderAdd.AppendChild(vendorAddress);
            vendorAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("Addr1")).InnerText =
                "Spicers LLC";
            vendorAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("Addr2")).InnerText = "Jay Vincent";
            vendorAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("Addr3")).InnerText = "12310 E.Slauson Ave.";
            vendorAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("City")).InnerText = "Santa Fe Springs";
            vendorAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("State")).InnerText = "CA";
            vendorAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("PostalCode")).InnerText = "90670";

            // <ShipAddress>...</ShipAddress>
            XmlElement shipAddress = inputXMLDocPurchaseOrder.CreateElement("ShipAddress");
            purchaseOrderAdd.AppendChild(shipAddress);
            shipAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("Addr1")).InnerText =
                "Spicers LLC";
            shipAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("Addr2")).InnerText = "Jay Vincent";
            shipAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("Addr3")).InnerText = "Santa Fe Springs";
            shipAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("City")).InnerText = "Sacramento";
            shipAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("State")).InnerText = "CA";
            shipAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("PostalCode")).InnerText = "90670";

            // <DueDate>...</DueDate>
            XmlElement dueDate = inputXMLDocPurchaseOrder.CreateElement("DueDate");
            purchaseOrderAdd.AppendChild(dueDate);
            dueDate.InnerText = "2019-01-19";

            // <ExpectedDate>...</ExpectedDate>
            XmlElement expectedDate = inputXMLDocPurchaseOrder.CreateElement("ExpectedDate");
            purchaseOrderAdd.AppendChild(expectedDate);
            expectedDate.InnerText = "2019-01-19";

            // <IsToBePrinted>...</IsToBePrinted>
            XmlElement isToBePrinted = inputXMLDocPurchaseOrder.CreateElement("IsToBePrinted");
            purchaseOrderAdd.AppendChild(isToBePrinted);
            isToBePrinted.InnerText = "false";

            // <IsToBeEmailed>...</IsToBeEmailed>
            XmlElement isToBeEmailed = inputXMLDocPurchaseOrder.CreateElement("IsToBeEmailed");
            purchaseOrderAdd.AppendChild(isToBeEmailed);
            isToBeEmailed.InnerText = "false";

            // <PurchaseOrderLineAdd>...</PurchaseOrderLineAdd>
            XmlElement purchaseOrderLineAdd = inputXMLDocPurchaseOrder.CreateElement("PurchaseOrderLineAdd");
            purchaseOrderAdd.AppendChild(purchaseOrderLineAdd);
            // // construct <ItemRef><FullName>Some name</FullName></ItemRef>
            XmlElement itemRef = inputXMLDocPurchaseOrder.CreateElement("ItemRef");
            itemRef.AppendChild(inputXMLDocPurchaseOrder.CreateElement("FullName")).InnerText = "9p white blank envelope";
            purchaseOrderLineAdd.AppendChild(itemRef);
            purchaseOrderLineAdd.AppendChild(inputXMLDocPurchaseOrder.CreateElement("Desc")).InnerText =
                "#9 White Blank";
            purchaseOrderLineAdd.AppendChild(inputXMLDocPurchaseOrder.CreateElement("Quantity")).InnerText = "70";
            //purchaseOrderLineAdd.AppendChild(inputXMLDocPurchaseOrder.CreateElement("UnitOfMeasure")).InnerText = "foot";
            purchaseOrderLineAdd.AppendChild(inputXMLDocPurchaseOrder.CreateElement("Rate")).InnerText = "10.00";
            purchaseOrderLineAdd.AppendChild(inputXMLDocPurchaseOrder.CreateElement("Amount")).InnerText = "5.00";

            string strRetString = inputXMLDocPurchaseOrder.OuterXml;
            LogTxtData(@"C:\Temp\PurchaseOrderAddRequest.xml", strRetString);

            return strRetString;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("__>> Will start to do purchase order");
            RequestProcessor2 qbRequestProcessor;
            string ticket = null;
            string purchaseOrderResponse = null;
            string purchaseOrderInput = null;

            // to add a purchase order
            try
            {
                //-- do the qbXMLRP request
                qbRequestProcessor = new RequestProcessor2();
                qbRequestProcessor.OpenConnection("", "Redstone Print and Mail Data Engineering");
                ticket = qbRequestProcessor.BeginSession("", QBFileMode.qbFileOpenDoNotCare);
                // // VERY IMPORTANT, invoke the function that actually constructs the qbXML
                purchaseOrderInput = PurchaseOrderAdd_AddXml();

                purchaseOrderResponse = qbRequestProcessor.ProcessRequest(ticket, purchaseOrderInput);
                LogTxtData(@"C:\Temp\PurchaseOrderAddResponse_object.xml", purchaseOrderResponse);

                if (ticket != null)
                {
                    qbRequestProcessor.EndSession(ticket);
                }

                qbRequestProcessor.CloseConnection();
            }
            catch (Exception ex)
            {
                qbRequestProcessor = null;
                LogTxtData(@"C:\Temp\PurchaseOrderAddRequestError.xml", ex.Message);
                return;
            }

            // to parse the response
            try
            {
                XmlDocument outputXmlPurchaseOrderAdd = new XmlDocument();
                outputXmlPurchaseOrderAdd.LoadXml(purchaseOrderResponse);
                XmlNodeList qbXmlMsgsRsNodeList = outputXmlPurchaseOrderAdd.GetElementsByTagName("PurchaseOrderAddRs");

                if (qbXmlMsgsRsNodeList.Count == 1)
                {
                    StringBuilder txtMessage = new StringBuilder();

                    XmlAttributeCollection rsAttributes = qbXmlMsgsRsNodeList.Item(0).Attributes;
                    // get statusCode, statusSeverity, statusMessage
                    string statusCode = rsAttributes.GetNamedItem("statusCode").Value;
                    string statusSeverity = rsAttributes.GetNamedItem("statusSeverity").Value;
                    string statusMessage = rsAttributes.GetNamedItem("statusMessage").Value;
                    txtMessage.AppendFormat(
                        "statusCode = {0}, statusSeverity = {1}, statusMessage = {2}",
                        statusCode, statusSeverity, statusMessage
                    );

                    // get PurchaseOrderAddRs > PurchaseOrderRet node
                    XmlNodeList purchaseOrderAddRsNodeList = qbXmlMsgsRsNodeList.Item(0).ChildNodes;
                    if (purchaseOrderAddRsNodeList.Item(0).Name.Equals("PurchaseOrderRet"))
                    {
                        XmlNodeList purchaseOrderAddRetNodeList = purchaseOrderAddRsNodeList.Item(0).ChildNodes;
                        foreach (XmlNode purchaseOrderAddRetNode in purchaseOrderAddRetNodeList)
                        {
                            if (purchaseOrderAddRetNode.Name.Equals("TxnID"))
                            {
                                txtMessage.AppendFormat(
                                    "\r\n__>> Purchase_Order_Add TxnID = {0}",
                                    purchaseOrderAddRetNode.InnerText
                                );
                            }
                            else if (purchaseOrderAddRetNode.Name.Equals("VendorRef"))
                            {
                                txtMessage.AppendFormat(
                                    "\r\n__>> Purchase_Order_Add inner xml = {0}",
                                    purchaseOrderAddRetNode.InnerXml
                                );
                            }
                        }
                    }

                    LogTxtData(@"C:\Temp\PurchaseOrderAddResponse.txt", txtMessage.ToString());
                }
            }
            catch (Exception ex)
            {
                const string customMessage = "JHA - Error while parsing purchase order response: ";
                qbRequestProcessor = null;
                LogTxtData(@"C:\Temp\PurchaseOrderAddResponseError.xml", customMessage + ex.Message);
                return;
            }

            try
            {
                //

            }
            catch (Exception ex)
            {
                // 

            }
        }

    }
}
