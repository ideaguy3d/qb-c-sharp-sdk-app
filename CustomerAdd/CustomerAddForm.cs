/*-----------------------------------------------------------
 * CustomerAddForm : implementation file
 *
 * Description:  This sample demonstrates the simple use 
 *               QuickBooks qbXMLRP COM object
 *				 Also it shows how to create and parse qbXML 	
 *				 using .NET XML classes
 *
 * Created On: 8/15/2002
 *
 * Copyright � 2002-2013 Intuit Inc. All rights reserved.
 * Use is subject to the terms specified at:
 *      http://developer.intuit.com/legal/devsite_tos.html
 *
 *----------------------------------------------------------
 */


using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using Interop.QBXMLRP2;

namespace CustomerAdd
{
    /// <summary>
    /// CustomerAddForm shows how to invoke QuickBooks qbXMLRP COM object
    /// It uses .NET to create qbXML request and parse qbXML response
    /// </summary>
    public class CustomerAddForm
    {
        private string logLocation = @"C:\Temp\PurchaseOrderAddRequest.xml";

        private void AddCustomer_Click(object sender, System.EventArgs e)
        {
            //-------------------------------------
            //-------------------------------------
            //js - START BUILDING THE qbXML Object
            // step2: create the qbXML request
            //-------------------------------------
            //-------------------------------------

            //-- create the XML doc/file
            XmlDocument inputXMLDoc = new XmlDocument();

            //-- <?xml version="1.0" encoding="utf-8"?>
            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));

            //-- <?qbxml version="13.0"?>
            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"2.0\""));

            //-- <QBXML>...</QBXML>
            XmlElement qbXML = inputXMLDoc.CreateElement("QBXML");
            inputXMLDoc.AppendChild(qbXML);

            //-- <QBXMLMsgsRq onError="stopOnError">...</QBXMLMsgsRq>  
            XmlElement qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");

            //---------------------------------------------------------------------------------------
            // THIS IS WHERE THE qbXML starts to get unique depending on the API that is being used.
            //---------------------------------------------------------------------------------------

            //-- <CustomerAddRq>...</CustomerAddRq>
            XmlElement custAddRq = inputXMLDoc.CreateElement("CustomerAddRq");
            qbXMLMsgsRq.AppendChild(custAddRq);
            custAddRq.SetAttribute("requestID", "1");

            XmlElement custAdd = inputXMLDoc.CreateElement("CustomerAdd");
            custAddRq.AppendChild(custAdd);
            custAdd.AppendChild(inputXMLDoc.CreateElement("Name")).InnerText = "Julius";


            //-- this is the xml string variable that will literally get sent to QBXMLRP2 
            string input = inputXMLDoc.OuterXml;

            //-- step3: do the qbXMLRP request
            RequestProcessor2 rp = new RequestProcessor2();

            rp.OpenConnection("", "Redstone Print and Mail");

            string ticket = rp.BeginSession("", QBFileMode.qbFileOpenDoNotCare);
            string response = rp.ProcessRequest(ticket, input);

            if (ticket != null)
            {
                rp.EndSession(ticket);
            }

            if (rp != null)
            {
                rp.CloseConnection();
            }

            //step4: parse the XML response and show a message
            XmlDocument outputXMLDoc = new XmlDocument();
            outputXMLDoc.LoadXml(response);
            XmlNodeList qbXMLMsgsRsNodeList = outputXMLDoc.GetElementsByTagName("CustomerAddRs");

            if (qbXMLMsgsRsNodeList.Count == 1) //it's always true, since we added a single Customer
            {
                System.Text.StringBuilder popupMessage = new System.Text.StringBuilder();

                XmlAttributeCollection rsAttributes = qbXMLMsgsRsNodeList.Item(0).Attributes;
                //get the status Code, info and Severity
                string retStatusCode = rsAttributes.GetNamedItem("statusCode").Value;
                string retStatusSeverity = rsAttributes.GetNamedItem("statusSeverity").Value;
                string retStatusMessage = rsAttributes.GetNamedItem("statusMessage").Value;
                popupMessage.AppendFormat("statusCode = {0}, statusSeverity = {1}, statusMessage = {2}",
                    retStatusCode, retStatusSeverity, retStatusMessage);

                //-- get the CustomerRet node for detailed info: 

                //a CustomerAddRs contains max one childNode for "CustomerRet"
                XmlNodeList custAddRsNodeList = qbXMLMsgsRsNodeList.Item(0).ChildNodes;

                if (custAddRsNodeList.Count == 1 && custAddRsNodeList.Item(0).Name.Equals("CustomerRet"))
                {
                    XmlNodeList custRetNodeList = custAddRsNodeList.Item(0).ChildNodes;

                    foreach (XmlNode custRetNode in custRetNodeList)
                    {
                        if (custRetNode.Name.Equals("ListID"))
                        {
                            popupMessage.AppendFormat("\r\nCustomer ListID = {0}", custRetNode.InnerText);
                        }
                        else if (custRetNode.Name.Equals("Name"))
                        {
                            popupMessage.AppendFormat("\r\nCustomer Name = {0}", custRetNode.InnerText);
                        }
                        else if (custRetNode.Name.Equals("FullName"))
                        {
                            popupMessage.AppendFormat("\r\nCustomer FullName = {0}", custRetNode.InnerText);
                        }
                    }
                } // End of customerRet
            } //End of customerAddRs
        } // END OF: AddCustomer_Click() {}

        private static string PurchaseOrderAddAddXml()
        {
            // create a .XML file/document
            XmlDocument inputXMLDocPurchaseOrder = new XmlDocument();

            // <?xml version="1.0" encoding="utf-8"?>
            inputXMLDocPurchaseOrder.AppendChild(inputXMLDocPurchaseOrder.CreateXmlDeclaration("1.0", "utf-8", null));

            // <?qbxml version="2.0"?>
            inputXMLDocPurchaseOrder.AppendChild(
                inputXMLDocPurchaseOrder.CreateProcessingInstruction("qbxml", "version=\"13.0\""));

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
            vendorRef.AppendChild(inputXMLDocPurchaseOrder.CreateElement("ListID")).InnerText = "0001";
            vendorRef.AppendChild(inputXMLDocPurchaseOrder.CreateElement("FullName")).InnerText = "Spicers LLC";

            // <TxnDate>...</TxnDate>
            XmlElement txnDate = inputXMLDocPurchaseOrder.CreateElement("TxnDate");
            purchaseOrderAdd.AppendChild(txnDate);
            txnDate.InnerText = "2019-1-10";

            // <RefNumber>...</RefNumber>
            XmlElement refNumber = inputXMLDocPurchaseOrder.CreateElement("RefNumber");
            purchaseOrderAdd.AppendChild(refNumber);
            refNumber.InnerText = "777";

            // <VendorAddress>...</VendorAddress>
            XmlElement vendorAddress = inputXMLDocPurchaseOrder.CreateElement("VendorAddress");
            purchaseOrderAdd.AppendChild(vendorAddress);
            vendorAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("Addr1")).InnerText =
                "Jeff Miller Landscaping";
            vendorAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("Addr2")).InnerText = "Jeff Miller";
            vendorAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("Addr3")).InnerText = "123 main st";
            vendorAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("City")).InnerText = "Sacramento";
            vendorAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("State")).InnerText = "CA";
            vendorAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("PostalCode")).InnerText = "95825";

            // <ShipAddress>...</ShipAddress>
            XmlElement shipAddress = inputXMLDocPurchaseOrder.CreateElement("ShipAddress");
            purchaseOrderAdd.AppendChild(shipAddress);
            shipAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("Addr1")).InnerText =
                "Jeff Miller Landscaping";
            shipAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("Addr2")).InnerText = "Jeff Miller";
            shipAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("Addr3")).InnerText = "123 main st";
            shipAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("City")).InnerText = "Sacramento";
            shipAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("State")).InnerText = "CA";
            shipAddress.AppendChild(inputXMLDocPurchaseOrder.CreateElement("PostalCode")).InnerText = "95825";

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

            string strRetString = inputXMLDocPurchaseOrder.OuterXml;

            LogTxtData(@"C:\Temp\PurchaseOrderAddRequest.xml", strRetString);

            return strRetString;
        }

        private static void LogXmlData(string strFile, string strXML)
        {
            StreamWriter sw = new StreamWriter(strFile);
            sw.WriteLine(strXML);
            sw.Flush();
            sw.Close();
        }

        private static void LogTxtData(string filePath, string strTxt)
        {
            StreamWriter sw = new StreamWriter(filePath);
            sw.WriteLine(strTxt);
            sw.Flush();
            sw.Close();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("__>> Will start to do purchase order");
            RequestProcessor2 qbRequestProcessor;
            string ticket = null;
            string purchaseOrderResponse = null;
            string purchaseOrderInput = null;

            try // to add a purchase order
            {
                //-- do the qbXMLRP request
                qbRequestProcessor = new RequestProcessor2();
                qbRequestProcessor.OpenConnection("", "Redstone Print and Mail Data Engineering");
                ticket = qbRequestProcessor.BeginSession("", QBFileMode.qbFileOpenDoNotCare);
                purchaseOrderInput = PurchaseOrderAddAddXml();

                purchaseOrderResponse = qbRequestProcessor.ProcessRequest(ticket, purchaseOrderInput);

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

            try // to parse the response
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
                Console.WriteLine("JHA - Error while parsing purchase order response: " + ex.Message);
                qbRequestProcessor = null;
                LogTxtData(@"C:\Temp\PurchaseOrderAddResponseError.xml", ex.Message);
                return;
            }
        }
    }
}