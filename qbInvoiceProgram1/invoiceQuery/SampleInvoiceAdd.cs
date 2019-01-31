//The following sample code is generated as an illustration of 
//Creating requests and parsing responses ONLY
//This code is NOT intended to show best practices or ideal code
//Use at your most careful discretion

using System;
using System.Xml;
using Interop.QBXMLRP2;

namespace com.intuit.idn.samples
{
    public class Sample
    {
        private XmlElement MakeSimpleElem(XmlDocument doc, string tagName, string tagVal)
        {
            XmlElement elem = doc.CreateElement(tagName);
            elem.InnerText = tagVal;
            return elem;
        }

        public void DoInvoiceAdd()
        {
            bool sessionBegun = false;
            bool connectionOpen = false;
            RequestProcessor2 rp = null;

            try
            {
                // Create the Request Processor object
                rp = new RequestProcessor2();

                // Create the XML document to hold our request
                XmlDocument requestXmlDoc = new XmlDocument();
                
                // Add the prolog processing instructions
                requestXmlDoc.AppendChild(requestXmlDoc.CreateXmlDeclaration("1.0", null, null));
                requestXmlDoc.AppendChild(requestXmlDoc.CreateProcessingInstruction("qbxml", "version=\"13.0\""));

                //Create the outer request envelope tag
                XmlElement outer = requestXmlDoc.CreateElement("QBXML");
                requestXmlDoc.AppendChild(outer);

                //Create the inner request envelope & any needed attributes
                XmlElement inner = requestXmlDoc.CreateElement("QBXMLMsgsRq");
                outer.AppendChild(inner);
                inner.SetAttribute("onError", "stopOnError");
                BuildInvoiceAddRq(requestXmlDoc, inner);

                //Connect to QuickBooks and begin a session
                rp.OpenConnection2("", "Sample Code from OSR", localQBD);
                connectionOpen = true;
                string ticket = rp.BeginSession("", QBFileMode.qbFileOpenDoNotCare);
                sessionBegun = true;

                //Send the request and get the response from QuickBooks
                string responseStr = rp.ProcessRequest(ticket, requestXmlDoc.OuterXml);

                //End the session and close the connection to QuickBooks
                rp.EndSession(ticket);
                sessionBegun = false;
                rp.CloseConnection();
                connectionOpen = false;

                WalkInvoiceAddRs(responseStr);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
                if (sessionBegun)
                {
                    sessionManager.EndSession();
                }

                if (connectionOpen)
                {
                    sessionManager.CloseConnection();
                }
            }
        }


        void BuildInvoiceAddRq(XmlDocument doc, XmlElement parent)
        {
//Create InvoiceAddRq aggregate and fill in field values for it
            XmlElement InvoiceAddRq = doc.CreateElement("InvoiceAddRq");
            parent.AppendChild(InvoiceAddRq);

//Create InvoiceAdd aggregate and fill in field values for it
            XmlElement InvoiceAdd = doc.CreateElement("InvoiceAdd");
            InvoiceAddRq.AppendChild(InvoiceAdd);

//Create CustomerRef aggregate and fill in field values for it
            XmlElement CustomerRef = doc.CreateElement("CustomerRef");
            InvoiceAdd.AppendChild(CustomerRef);
//Set field value for ListID <!-- optional -->
            CustomerRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
            CustomerRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating CustomerRef aggregate


//Create ClassRef aggregate and fill in field values for it
            XmlElement ClassRef = doc.CreateElement("ClassRef");
            InvoiceAdd.AppendChild(ClassRef);
//Set field value for ListID <!-- optional -->
            ClassRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
            ClassRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating ClassRef aggregate


//Create ARAccountRef aggregate and fill in field values for it
            XmlElement ARAccountRef = doc.CreateElement("ARAccountRef");
            InvoiceAdd.AppendChild(ARAccountRef);
//Set field value for ListID <!-- optional -->
            ARAccountRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
            ARAccountRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating ARAccountRef aggregate


//Create TemplateRef aggregate and fill in field values for it
            XmlElement TemplateRef = doc.CreateElement("TemplateRef");
            InvoiceAdd.AppendChild(TemplateRef);
//Set field value for ListID <!-- optional -->
            TemplateRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
            TemplateRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating TemplateRef aggregate

//Set field value for TxnDate <!-- optional -->
            InvoiceAdd.AppendChild(MakeSimpleElem(doc, "TxnDate", "2007-12-15"));
//Set field value for RefNumber <!-- optional -->
            InvoiceAdd.AppendChild(MakeSimpleElem(doc, "RefNumber", "ab"));

//Create BillAddress aggregate and fill in field values for it
            XmlElement BillAddress = doc.CreateElement("BillAddress");
            InvoiceAdd.AppendChild(BillAddress);
//Set field value for Addr1 <!-- optional -->
            BillAddress.AppendChild(MakeSimpleElem(doc, "Addr1", "ab"));
//Set field value for Addr2 <!-- optional -->
            BillAddress.AppendChild(MakeSimpleElem(doc, "Addr2", "ab"));
//Set field value for Addr3 <!-- optional -->
            BillAddress.AppendChild(MakeSimpleElem(doc, "Addr3", "ab"));
//Set field value for Addr4 <!-- optional -->
            BillAddress.AppendChild(MakeSimpleElem(doc, "Addr4", "ab"));
//Set field value for Addr5 <!-- optional -->
            BillAddress.AppendChild(MakeSimpleElem(doc, "Addr5", "ab"));
//Set field value for City <!-- optional -->
            BillAddress.AppendChild(MakeSimpleElem(doc, "City", "ab"));
//Set field value for State <!-- optional -->
            BillAddress.AppendChild(MakeSimpleElem(doc, "State", "ab"));
//Set field value for PostalCode <!-- optional -->
            BillAddress.AppendChild(MakeSimpleElem(doc, "PostalCode", "ab"));
//Set field value for Country <!-- optional -->
            BillAddress.AppendChild(MakeSimpleElem(doc, "Country", "ab"));
//Set field value for Note <!-- optional -->
            BillAddress.AppendChild(MakeSimpleElem(doc, "Note", "ab"));
//Done creating BillAddress aggregate


//Create ShipAddress aggregate and fill in field values for it
            XmlElement ShipAddress = doc.CreateElement("ShipAddress");
            InvoiceAdd.AppendChild(ShipAddress);
//Set field value for Addr1 <!-- optional -->
            ShipAddress.AppendChild(MakeSimpleElem(doc, "Addr1", "ab"));
//Set field value for Addr2 <!-- optional -->
            ShipAddress.AppendChild(MakeSimpleElem(doc, "Addr2", "ab"));
//Set field value for Addr3 <!-- optional -->
            ShipAddress.AppendChild(MakeSimpleElem(doc, "Addr3", "ab"));
//Set field value for Addr4 <!-- optional -->
            ShipAddress.AppendChild(MakeSimpleElem(doc, "Addr4", "ab"));
//Set field value for Addr5 <!-- optional -->
            ShipAddress.AppendChild(MakeSimpleElem(doc, "Addr5", "ab"));
//Set field value for City <!-- optional -->
            ShipAddress.AppendChild(MakeSimpleElem(doc, "City", "ab"));
//Set field value for State <!-- optional -->
            ShipAddress.AppendChild(MakeSimpleElem(doc, "State", "ab"));
//Set field value for PostalCode <!-- optional -->
            ShipAddress.AppendChild(MakeSimpleElem(doc, "PostalCode", "ab"));
//Set field value for Country <!-- optional -->
            ShipAddress.AppendChild(MakeSimpleElem(doc, "Country", "ab"));
//Set field value for Note <!-- optional -->
            ShipAddress.AppendChild(MakeSimpleElem(doc, "Note", "ab"));
//Done creating ShipAddress aggregate

//Set field value for IsPending <!-- optional -->
            InvoiceAdd.AppendChild(MakeSimpleElem(doc, "IsPending", "1"));
//Set field value for IsFinanceCharge <!-- optional -->
            InvoiceAdd.AppendChild(MakeSimpleElem(doc, "IsFinanceCharge", "1"));
//Set field value for PONumber <!-- optional -->
            InvoiceAdd.AppendChild(MakeSimpleElem(doc, "PONumber", "ab"));

//Create TermsRef aggregate and fill in field values for it
            XmlElement TermsRef = doc.CreateElement("TermsRef");
            InvoiceAdd.AppendChild(TermsRef);
//Set field value for ListID <!-- optional -->
            TermsRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
            TermsRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating TermsRef aggregate

//Set field value for DueDate <!-- optional -->
            InvoiceAdd.AppendChild(MakeSimpleElem(doc, "DueDate", "2007-12-15"));

//Create SalesRepRef aggregate and fill in field values for it
            XmlElement SalesRepRef = doc.CreateElement("SalesRepRef");
            InvoiceAdd.AppendChild(SalesRepRef);
//Set field value for ListID <!-- optional -->
            SalesRepRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
            SalesRepRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating SalesRepRef aggregate

//Set field value for FOB <!-- optional -->
            InvoiceAdd.AppendChild(MakeSimpleElem(doc, "FOB", "ab"));
//Set field value for ShipDate <!-- optional -->
            InvoiceAdd.AppendChild(MakeSimpleElem(doc, "ShipDate", "2007-12-15"));

//Create ShipMethodRef aggregate and fill in field values for it
            XmlElement ShipMethodRef = doc.CreateElement("ShipMethodRef");
            InvoiceAdd.AppendChild(ShipMethodRef);
//Set field value for ListID <!-- optional -->
            ShipMethodRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
            ShipMethodRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating ShipMethodRef aggregate


//Create ItemSalesTaxRef aggregate and fill in field values for it
            XmlElement ItemSalesTaxRef = doc.CreateElement("ItemSalesTaxRef");
            InvoiceAdd.AppendChild(ItemSalesTaxRef);
//Set field value for ListID <!-- optional -->
            ItemSalesTaxRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
            ItemSalesTaxRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating ItemSalesTaxRef aggregate

//Set field value for Memo <!-- optional -->
            InvoiceAdd.AppendChild(MakeSimpleElem(doc, "Memo", "ab"));

//Create CustomerMsgRef aggregate and fill in field values for it
            XmlElement CustomerMsgRef = doc.CreateElement("CustomerMsgRef");
            InvoiceAdd.AppendChild(CustomerMsgRef);
//Set field value for ListID <!-- optional -->
            CustomerMsgRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
            CustomerMsgRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating CustomerMsgRef aggregate

//Set field value for IsToBePrinted <!-- optional -->
            InvoiceAdd.AppendChild(MakeSimpleElem(doc, "IsToBePrinted", "1"));
//Set field value for IsToBeEmailed <!-- optional -->
            InvoiceAdd.AppendChild(MakeSimpleElem(doc, "IsToBeEmailed", "1"));

//Create CustomerSalesTaxCodeRef aggregate and fill in field values for it
            XmlElement CustomerSalesTaxCodeRef = doc.CreateElement("CustomerSalesTaxCodeRef");
            InvoiceAdd.AppendChild(CustomerSalesTaxCodeRef);
//Set field value for ListID <!-- optional -->
            CustomerSalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
            CustomerSalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating CustomerSalesTaxCodeRef aggregate

//Set field value for Other <!-- optional -->
            InvoiceAdd.AppendChild(MakeSimpleElem(doc, "Other", "ab"));
//Set field value for ExchangeRate <!-- optional -->
            InvoiceAdd.AppendChild(MakeSimpleElem(doc, "ExchangeRate", ""FLOATTYPE""));
//Set field value for ExternalGUID <!-- optional -->
            InvoiceAdd.AppendChild(MakeSimpleElem(doc, "ExternalGUID", "Guid.NewGuid().ToString()"));
//Set field value for LinkToTxnID <!-- optional, may repeat -->
            InvoiceAdd.AppendChild(MakeSimpleElem(doc, "LinkToTxnID", "200000-1011023419"));

//Create SetCredit aggregate and fill in field values for it
// May create more than one of these aggregates if needed
            XmlElement SetCredit = doc.CreateElement("SetCredit");
            InvoiceAdd.AppendChild(SetCredit);
//Set field value for CreditTxnID <!-- required -->
            SetCredit.AppendChild(MakeSimpleElem(doc, "CreditTxnID", "200000-1011023419"));
//Set field value for AppliedAmount <!-- required -->
            SetCredit.AppendChild(MakeSimpleElem(doc, "AppliedAmount", "10.01"));
//Set field value for Override <!-- optional -->
            SetCredit.AppendChild(MakeSimpleElem(doc, "Override", "1"));
//Done creating SetCredit aggregate

//Begin OR
            string ORElementType = "InvoiceLineAdd";
            if (ORElementType == "InvoiceLineAdd")
            {
//Create InvoiceLineAdd aggregate and fill in field values for it
                XmlElement InvoiceLineAdd = doc.CreateElement("InvoiceLineAdd");
                InvoiceAdd.AppendChild(InvoiceLineAdd);

//Create ItemRef aggregate and fill in field values for it
                XmlElement ItemRef = doc.CreateElement("ItemRef");
                InvoiceLineAdd.AppendChild(ItemRef);
//Set field value for ListID <!-- optional -->
                ItemRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
                ItemRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating ItemRef aggregate

//Set field value for Desc <!-- optional -->
                InvoiceLineAdd.AppendChild(MakeSimpleElem(doc, "Desc", "ab"));
//Set field value for Quantity <!-- optional -->
                InvoiceLineAdd.AppendChild(MakeSimpleElem(doc, "Quantity", "2"));
//Set field value for UnitOfMeasure <!-- optional -->
                InvoiceLineAdd.AppendChild(MakeSimpleElem(doc, "UnitOfMeasure", "ab"));
//Begin OR
                string ORElementType = "Rate";
                if (ORElementType == "Rate")
                {
//Set field value for Rate <!-- optional -->
                    InvoiceLineAdd.AppendChild(MakeSimpleElem(doc, "Rate", "15.65"));
                }

                if (ORElementType == "RatePercent")
                {
//Set field value for RatePercent <!-- optional -->
                    InvoiceLineAdd.AppendChild(MakeSimpleElem(doc, "RatePercent", "20.00"));
                }

                if (ORElementType == "PriceLevelRef")
                {
//Create PriceLevelRef aggregate and fill in field values for it
                    XmlElement PriceLevelRef = doc.CreateElement("PriceLevelRef");
                    InvoiceLineAdd.AppendChild(PriceLevelRef);
//Set field value for ListID <!-- optional -->
                    PriceLevelRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
                    PriceLevelRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating PriceLevelRef aggregate
                }

//Create ClassRef aggregate and fill in field values for it
                XmlElement ClassRef = doc.CreateElement("ClassRef");
                InvoiceLineAdd.AppendChild(ClassRef);
//Set field value for ListID <!-- optional -->
                ClassRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
                ClassRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating ClassRef aggregate

//Set field value for Amount <!-- optional -->
                InvoiceLineAdd.AppendChild(MakeSimpleElem(doc, "Amount", "10.01"));
//Set field value for OptionForPriceRuleConflict <!-- optional -->
                InvoiceLineAdd.AppendChild(MakeSimpleElem(doc, "OptionForPriceRuleConflict", "Zero"));

//Create InventorySiteRef aggregate and fill in field values for it
                XmlElement InventorySiteRef = doc.CreateElement("InventorySiteRef");
                InvoiceLineAdd.AppendChild(InventorySiteRef);
//Set field value for ListID <!-- optional -->
                InventorySiteRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
                InventorySiteRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating InventorySiteRef aggregate


//Create InventorySiteLocationRef aggregate and fill in field values for it
                XmlElement InventorySiteLocationRef = doc.CreateElement("InventorySiteLocationRef");
                InvoiceLineAdd.AppendChild(InventorySiteLocationRef);
//Set field value for ListID <!-- optional -->
                InventorySiteLocationRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
                InventorySiteLocationRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating InventorySiteLocationRef aggregate

//Begin OR
                string ORElementType = "SerialNumber";
                if (ORElementType == "SerialNumber")
                {
//Set field value for SerialNumber <!-- optional -->
                    InvoiceLineAdd.AppendChild(MakeSimpleElem(doc, "SerialNumber", "ab"));
                }

                if (ORElementType == "LotNumber")
                {
//Set field value for LotNumber <!-- optional -->
                    InvoiceLineAdd.AppendChild(MakeSimpleElem(doc, "LotNumber", "ab"));
                }

//Set field value for ServiceDate <!-- optional -->
                InvoiceLineAdd.AppendChild(MakeSimpleElem(doc, "ServiceDate", "2007-12-15"));

//Create SalesTaxCodeRef aggregate and fill in field values for it
                XmlElement SalesTaxCodeRef = doc.CreateElement("SalesTaxCodeRef");
                InvoiceLineAdd.AppendChild(SalesTaxCodeRef);
//Set field value for ListID <!-- optional -->
                SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
                SalesTaxCodeRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating SalesTaxCodeRef aggregate


//Create OverrideItemAccountRef aggregate and fill in field values for it
                XmlElement OverrideItemAccountRef = doc.CreateElement("OverrideItemAccountRef");
                InvoiceLineAdd.AppendChild(OverrideItemAccountRef);
//Set field value for ListID <!-- optional -->
                OverrideItemAccountRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
                OverrideItemAccountRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating OverrideItemAccountRef aggregate

//Set field value for Other1 <!-- optional -->
                InvoiceLineAdd.AppendChild(MakeSimpleElem(doc, "Other1", "ab"));
//Set field value for Other2 <!-- optional -->
                InvoiceLineAdd.AppendChild(MakeSimpleElem(doc, "Other2", "ab"));

//Create LinkToTxn aggregate and fill in field values for it
                XmlElement LinkToTxn = doc.CreateElement("LinkToTxn");
                InvoiceLineAdd.AppendChild(LinkToTxn);
//Set field value for TxnID <!-- required -->
                LinkToTxn.AppendChild(MakeSimpleElem(doc, "TxnID", "200000-1011023419"));
//Set field value for TxnLineID <!-- required -->
                LinkToTxn.AppendChild(MakeSimpleElem(doc, "TxnLineID", "200000-1011023419"));
//Done creating LinkToTxn aggregate


//Create DataExt aggregate and fill in field values for it
// May create more than one of these aggregates if needed
                XmlElement DataExt = doc.CreateElement("DataExt");
                InvoiceLineAdd.AppendChild(DataExt);
//Set field value for OwnerID <!-- required -->
                DataExt.AppendChild(MakeSimpleElem(doc, "OwnerID", "Guid.NewGuid().ToString()"));
//Set field value for DataExtName <!-- required -->
                DataExt.AppendChild(MakeSimpleElem(doc, "DataExtName", "ab"));
//Set field value for DataExtValue <!-- required -->
                DataExt.AppendChild(MakeSimpleElem(doc, "DataExtValue", "ab"));
//Done creating DataExt aggregate

//Done creating InvoiceLineAdd aggregate
            }

            if (ORElementType == "InvoiceLineGroupAdd")
            {
//Create InvoiceLineGroupAdd aggregate and fill in field values for it
                XmlElement InvoiceLineGroupAdd = doc.CreateElement("InvoiceLineGroupAdd");
                InvoiceAdd.AppendChild(InvoiceLineGroupAdd);

//Create ItemGroupRef aggregate and fill in field values for it
                XmlElement ItemGroupRef = doc.CreateElement("ItemGroupRef");
                InvoiceLineGroupAdd.AppendChild(ItemGroupRef);
//Set field value for ListID <!-- optional -->
                ItemGroupRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
                ItemGroupRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating ItemGroupRef aggregate

//Set field value for Quantity <!-- optional -->
                InvoiceLineGroupAdd.AppendChild(MakeSimpleElem(doc, "Quantity", "2"));
//Set field value for UnitOfMeasure <!-- optional -->
                InvoiceLineGroupAdd.AppendChild(MakeSimpleElem(doc, "UnitOfMeasure", "ab"));

//Create InventorySiteRef aggregate and fill in field values for it
                XmlElement InventorySiteRef = doc.CreateElement("InventorySiteRef");
                InvoiceLineGroupAdd.AppendChild(InventorySiteRef);
//Set field value for ListID <!-- optional -->
                InventorySiteRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
                InventorySiteRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating InventorySiteRef aggregate


//Create InventorySiteLocationRef aggregate and fill in field values for it
                XmlElement InventorySiteLocationRef = doc.CreateElement("InventorySiteLocationRef");
                InvoiceLineGroupAdd.AppendChild(InventorySiteLocationRef);
//Set field value for ListID <!-- optional -->
                InventorySiteLocationRef.AppendChild(MakeSimpleElem(doc, "ListID", "200000-1011023419"));
//Set field value for FullName <!-- optional -->
                InventorySiteLocationRef.AppendChild(MakeSimpleElem(doc, "FullName", "ab"));
//Done creating InventorySiteLocationRef aggregate


//Create DataExt aggregate and fill in field values for it
// May create more than one of these aggregates if needed
                XmlElement DataExt = doc.CreateElement("DataExt");
                InvoiceLineGroupAdd.AppendChild(DataExt);
//Set field value for OwnerID <!-- required -->
                DataExt.AppendChild(MakeSimpleElem(doc, "OwnerID", "Guid.NewGuid().ToString()"));
//Set field value for DataExtName <!-- required -->
                DataExt.AppendChild(MakeSimpleElem(doc, "DataExtName", "ab"));
//Set field value for DataExtValue <!-- required -->
                DataExt.AppendChild(MakeSimpleElem(doc, "DataExtValue", "ab"));
//Done creating DataExt aggregate

//Set field value for undefined
                InvoiceLineGroupAdd.AppendChild(MakeSimpleElem(doc, "undefined", ""undefined""));
//Done creating InvoiceLineGroupAdd aggregate
            }
//Done creating InvoiceAdd aggregate

//Set field value for IncludeRetElement <!-- optional, may repeat -->
            InvoiceAddRq.AppendChild(MakeSimpleElem(doc, "IncludeRetElement", "ab"));
//Done creating InvoiceAddRq aggregate
        }


        void WalkInvoiceAddRs(string response)
        {
//Parse the response XML string into an XmlDocument
            XmlDocument responseXmlDoc = new XmlDocument();
            responseXmlDoc.LoadXml(response);

//Get the response for our request
            XmlNodeList InvoiceAddRsList = responseXmlDoc.GetElementsByTagName("InvoiceAddRs");
            if (InvoiceAddRsList.Count == 1) //Should always be true since we only did one request in this sample
            {
                XmlNode responseNode = InvoiceAddRsList.Item(0);
//Check the status code, info, and severity
                XmlAttributeCollection rsAttributes = responseNode.Attributes;
                string statusCode = rsAttributes.GetNamedItem("statusCode").Value;
                string statusSeverity = rsAttributes.GetNamedItem("statusSeverity").Value;
                string statusMessage = rsAttributes.GetNamedItem("statusMessage").Value;

//status code = 0 all OK, > 0 is warning
                if (Convert.ToInt32(statusCode) >= 0)
                {
                    XmlNodeList InvoiceRetList = responseNode.SelectNodes("//InvoiceRet"); //XPath Query
                    for (int i = 0; i < InvoiceRetList.Count; i++)
                    {
                        XmlNode InvoiceRet = InvoiceRetList.Item(i);
                        WalkInvoiceRet(InvoiceRet);
                    }
                }
            }
        }


        void WalkInvoiceRet(XmlNode InvoiceRet)
        {
            if (InvoiceRet == null) return;

//Go through all the elements of InvoiceRet
//Get value of TxnID
            string TxnID = InvoiceRet.SelectSingleNode("./TxnID").InnerText;
//Get value of TimeCreated
            string TimeCreated = InvoiceRet.SelectSingleNode("./TimeCreated").InnerText;
//Get value of TimeModified
            string TimeModified = InvoiceRet.SelectSingleNode("./TimeModified").InnerText;
//Get value of EditSequence
            string EditSequence = InvoiceRet.SelectSingleNode("./EditSequence").InnerText;
//Get value of TxnNumber
            if (InvoiceRet.SelectSingleNode("./TxnNumber") != null)
            {
                string TxnNumber = InvoiceRet.SelectSingleNode("./TxnNumber").InnerText;
            }

//Get all field values for CustomerRef aggregate 
//Get value of ListID
            if (InvoiceRet.SelectSingleNode("./CustomerRef/ListID") != null)
            {
                string ListID = InvoiceRet.SelectSingleNode("./CustomerRef/ListID").InnerText;
            }

//Get value of FullName
            if (InvoiceRet.SelectSingleNode("./CustomerRef/FullName") != null)
            {
                string FullName = InvoiceRet.SelectSingleNode("./CustomerRef/FullName").InnerText;
            }
//Done with field values for CustomerRef aggregate

//Get all field values for ClassRef aggregate 
            XmlNode ClassRef = InvoiceRet.SelectSingleNode("./ClassRef");
            if (ClassRef != null)
            {
//Get value of ListID
                if (InvoiceRet.SelectSingleNode("./ClassRef/ListID") != null)
                {
                    string ListID = InvoiceRet.SelectSingleNode("./ClassRef/ListID").InnerText;
                }

//Get value of FullName
                if (InvoiceRet.SelectSingleNode("./ClassRef/FullName") != null)
                {
                    string FullName = InvoiceRet.SelectSingleNode("./ClassRef/FullName").InnerText;
                }
            }
//Done with field values for ClassRef aggregate

//Get all field values for ARAccountRef aggregate 
            XmlNode ARAccountRef = InvoiceRet.SelectSingleNode("./ARAccountRef");
            if (ARAccountRef != null)
            {
//Get value of ListID
                if (InvoiceRet.SelectSingleNode("./ARAccountRef/ListID") != null)
                {
                    string ListID = InvoiceRet.SelectSingleNode("./ARAccountRef/ListID").InnerText;
                }

//Get value of FullName
                if (InvoiceRet.SelectSingleNode("./ARAccountRef/FullName") != null)
                {
                    string FullName = InvoiceRet.SelectSingleNode("./ARAccountRef/FullName").InnerText;
                }
            }
//Done with field values for ARAccountRef aggregate

//Get all field values for TemplateRef aggregate 
            XmlNode TemplateRef = InvoiceRet.SelectSingleNode("./TemplateRef");
            if (TemplateRef != null)
            {
//Get value of ListID
                if (InvoiceRet.SelectSingleNode("./TemplateRef/ListID") != null)
                {
                    string ListID = InvoiceRet.SelectSingleNode("./TemplateRef/ListID").InnerText;
                }

//Get value of FullName
                if (InvoiceRet.SelectSingleNode("./TemplateRef/FullName") != null)
                {
                    string FullName = InvoiceRet.SelectSingleNode("./TemplateRef/FullName").InnerText;
                }
            }
//Done with field values for TemplateRef aggregate

//Get value of TxnDate
            string TxnDate = InvoiceRet.SelectSingleNode("./TxnDate").InnerText;
//Get value of RefNumber
            if (InvoiceRet.SelectSingleNode("./RefNumber") != null)
            {
                string RefNumber = InvoiceRet.SelectSingleNode("./RefNumber").InnerText;
            }

//Get all field values for BillAddress aggregate 
            XmlNode BillAddress = InvoiceRet.SelectSingleNode("./BillAddress");
            if (BillAddress != null)
            {
//Get value of Addr1
                if (InvoiceRet.SelectSingleNode("./BillAddress/Addr1") != null)
                {
                    string Addr1 = InvoiceRet.SelectSingleNode("./BillAddress/Addr1").InnerText;
                }

//Get value of Addr2
                if (InvoiceRet.SelectSingleNode("./BillAddress/Addr2") != null)
                {
                    string Addr2 = InvoiceRet.SelectSingleNode("./BillAddress/Addr2").InnerText;
                }

//Get value of Addr3
                if (InvoiceRet.SelectSingleNode("./BillAddress/Addr3") != null)
                {
                    string Addr3 = InvoiceRet.SelectSingleNode("./BillAddress/Addr3").InnerText;
                }

//Get value of Addr4
                if (InvoiceRet.SelectSingleNode("./BillAddress/Addr4") != null)
                {
                    string Addr4 = InvoiceRet.SelectSingleNode("./BillAddress/Addr4").InnerText;
                }

//Get value of Addr5
                if (InvoiceRet.SelectSingleNode("./BillAddress/Addr5") != null)
                {
                    string Addr5 = InvoiceRet.SelectSingleNode("./BillAddress/Addr5").InnerText;
                }

//Get value of City
                if (InvoiceRet.SelectSingleNode("./BillAddress/City") != null)
                {
                    string City = InvoiceRet.SelectSingleNode("./BillAddress/City").InnerText;
                }

//Get value of State
                if (InvoiceRet.SelectSingleNode("./BillAddress/State") != null)
                {
                    string State = InvoiceRet.SelectSingleNode("./BillAddress/State").InnerText;
                }

//Get value of PostalCode
                if (InvoiceRet.SelectSingleNode("./BillAddress/PostalCode") != null)
                {
                    string PostalCode = InvoiceRet.SelectSingleNode("./BillAddress/PostalCode").InnerText;
                }

//Get value of Country
                if (InvoiceRet.SelectSingleNode("./BillAddress/Country") != null)
                {
                    string Country = InvoiceRet.SelectSingleNode("./BillAddress/Country").InnerText;
                }

//Get value of Note
                if (InvoiceRet.SelectSingleNode("./BillAddress/Note") != null)
                {
                    string Note = InvoiceRet.SelectSingleNode("./BillAddress/Note").InnerText;
                }
            }
//Done with field values for BillAddress aggregate

//Get all field values for BillAddressBlock aggregate 
            XmlNode BillAddressBlock = InvoiceRet.SelectSingleNode("./BillAddressBlock");
            if (BillAddressBlock != null)
            {
//Get value of Addr1
                if (InvoiceRet.SelectSingleNode("./BillAddressBlock/Addr1") != null)
                {
                    string Addr1 = InvoiceRet.SelectSingleNode("./BillAddressBlock/Addr1").InnerText;
                }

//Get value of Addr2
                if (InvoiceRet.SelectSingleNode("./BillAddressBlock/Addr2") != null)
                {
                    string Addr2 = InvoiceRet.SelectSingleNode("./BillAddressBlock/Addr2").InnerText;
                }

//Get value of Addr3
                if (InvoiceRet.SelectSingleNode("./BillAddressBlock/Addr3") != null)
                {
                    string Addr3 = InvoiceRet.SelectSingleNode("./BillAddressBlock/Addr3").InnerText;
                }

//Get value of Addr4
                if (InvoiceRet.SelectSingleNode("./BillAddressBlock/Addr4") != null)
                {
                    string Addr4 = InvoiceRet.SelectSingleNode("./BillAddressBlock/Addr4").InnerText;
                }

//Get value of Addr5
                if (InvoiceRet.SelectSingleNode("./BillAddressBlock/Addr5") != null)
                {
                    string Addr5 = InvoiceRet.SelectSingleNode("./BillAddressBlock/Addr5").InnerText;
                }
            }
//Done with field values for BillAddressBlock aggregate

//Get all field values for ShipAddress aggregate 
            XmlNode ShipAddress = InvoiceRet.SelectSingleNode("./ShipAddress");
            if (ShipAddress != null)
            {
//Get value of Addr1
                if (InvoiceRet.SelectSingleNode("./ShipAddress/Addr1") != null)
                {
                    string Addr1 = InvoiceRet.SelectSingleNode("./ShipAddress/Addr1").InnerText;
                }

//Get value of Addr2
                if (InvoiceRet.SelectSingleNode("./ShipAddress/Addr2") != null)
                {
                    string Addr2 = InvoiceRet.SelectSingleNode("./ShipAddress/Addr2").InnerText;
                }

//Get value of Addr3
                if (InvoiceRet.SelectSingleNode("./ShipAddress/Addr3") != null)
                {
                    string Addr3 = InvoiceRet.SelectSingleNode("./ShipAddress/Addr3").InnerText;
                }

//Get value of Addr4
                if (InvoiceRet.SelectSingleNode("./ShipAddress/Addr4") != null)
                {
                    string Addr4 = InvoiceRet.SelectSingleNode("./ShipAddress/Addr4").InnerText;
                }

//Get value of Addr5
                if (InvoiceRet.SelectSingleNode("./ShipAddress/Addr5") != null)
                {
                    string Addr5 = InvoiceRet.SelectSingleNode("./ShipAddress/Addr5").InnerText;
                }

//Get value of City
                if (InvoiceRet.SelectSingleNode("./ShipAddress/City") != null)
                {
                    string City = InvoiceRet.SelectSingleNode("./ShipAddress/City").InnerText;
                }

//Get value of State
                if (InvoiceRet.SelectSingleNode("./ShipAddress/State") != null)
                {
                    string State = InvoiceRet.SelectSingleNode("./ShipAddress/State").InnerText;
                }

//Get value of PostalCode
                if (InvoiceRet.SelectSingleNode("./ShipAddress/PostalCode") != null)
                {
                    string PostalCode = InvoiceRet.SelectSingleNode("./ShipAddress/PostalCode").InnerText;
                }

//Get value of Country
                if (InvoiceRet.SelectSingleNode("./ShipAddress/Country") != null)
                {
                    string Country = InvoiceRet.SelectSingleNode("./ShipAddress/Country").InnerText;
                }

//Get value of Note
                if (InvoiceRet.SelectSingleNode("./ShipAddress/Note") != null)
                {
                    string Note = InvoiceRet.SelectSingleNode("./ShipAddress/Note").InnerText;
                }
            }
//Done with field values for ShipAddress aggregate

//Get all field values for ShipAddressBlock aggregate 
            XmlNode ShipAddressBlock = InvoiceRet.SelectSingleNode("./ShipAddressBlock");
            if (ShipAddressBlock != null)
            {
//Get value of Addr1
                if (InvoiceRet.SelectSingleNode("./ShipAddressBlock/Addr1") != null)
                {
                    string Addr1 = InvoiceRet.SelectSingleNode("./ShipAddressBlock/Addr1").InnerText;
                }

//Get value of Addr2
                if (InvoiceRet.SelectSingleNode("./ShipAddressBlock/Addr2") != null)
                {
                    string Addr2 = InvoiceRet.SelectSingleNode("./ShipAddressBlock/Addr2").InnerText;
                }

//Get value of Addr3
                if (InvoiceRet.SelectSingleNode("./ShipAddressBlock/Addr3") != null)
                {
                    string Addr3 = InvoiceRet.SelectSingleNode("./ShipAddressBlock/Addr3").InnerText;
                }

//Get value of Addr4
                if (InvoiceRet.SelectSingleNode("./ShipAddressBlock/Addr4") != null)
                {
                    string Addr4 = InvoiceRet.SelectSingleNode("./ShipAddressBlock/Addr4").InnerText;
                }

//Get value of Addr5
                if (InvoiceRet.SelectSingleNode("./ShipAddressBlock/Addr5") != null)
                {
                    string Addr5 = InvoiceRet.SelectSingleNode("./ShipAddressBlock/Addr5").InnerText;
                }
            }
//Done with field values for ShipAddressBlock aggregate

//Get value of IsPending
            if (InvoiceRet.SelectSingleNode("./IsPending") != null)
            {
                string IsPending = InvoiceRet.SelectSingleNode("./IsPending").InnerText;
            }

//Get value of IsFinanceCharge
            if (InvoiceRet.SelectSingleNode("./IsFinanceCharge") != null)
            {
                string IsFinanceCharge = InvoiceRet.SelectSingleNode("./IsFinanceCharge").InnerText;
            }

//Get value of PONumber
            if (InvoiceRet.SelectSingleNode("./PONumber") != null)
            {
                string PONumber = InvoiceRet.SelectSingleNode("./PONumber").InnerText;
            }

//Get all field values for TermsRef aggregate 
            XmlNode TermsRef = InvoiceRet.SelectSingleNode("./TermsRef");
            if (TermsRef != null)
            {
//Get value of ListID
                if (InvoiceRet.SelectSingleNode("./TermsRef/ListID") != null)
                {
                    string ListID = InvoiceRet.SelectSingleNode("./TermsRef/ListID").InnerText;
                }

//Get value of FullName
                if (InvoiceRet.SelectSingleNode("./TermsRef/FullName") != null)
                {
                    string FullName = InvoiceRet.SelectSingleNode("./TermsRef/FullName").InnerText;
                }
            }
//Done with field values for TermsRef aggregate

//Get value of DueDate
            if (InvoiceRet.SelectSingleNode("./DueDate") != null)
            {
                string DueDate = InvoiceRet.SelectSingleNode("./DueDate").InnerText;
            }

//Get all field values for SalesRepRef aggregate 
            XmlNode SalesRepRef = InvoiceRet.SelectSingleNode("./SalesRepRef");
            if (SalesRepRef != null)
            {
//Get value of ListID
                if (InvoiceRet.SelectSingleNode("./SalesRepRef/ListID") != null)
                {
                    string ListID = InvoiceRet.SelectSingleNode("./SalesRepRef/ListID").InnerText;
                }

//Get value of FullName
                if (InvoiceRet.SelectSingleNode("./SalesRepRef/FullName") != null)
                {
                    string FullName = InvoiceRet.SelectSingleNode("./SalesRepRef/FullName").InnerText;
                }
            }
//Done with field values for SalesRepRef aggregate

//Get value of FOB
            if (InvoiceRet.SelectSingleNode("./FOB") != null)
            {
                string FOB = InvoiceRet.SelectSingleNode("./FOB").InnerText;
            }

//Get value of ShipDate
            if (InvoiceRet.SelectSingleNode("./ShipDate") != null)
            {
                string ShipDate = InvoiceRet.SelectSingleNode("./ShipDate").InnerText;
            }

//Get all field values for ShipMethodRef aggregate 
            XmlNode ShipMethodRef = InvoiceRet.SelectSingleNode("./ShipMethodRef");
            if (ShipMethodRef != null)
            {
//Get value of ListID
                if (InvoiceRet.SelectSingleNode("./ShipMethodRef/ListID") != null)
                {
                    string ListID = InvoiceRet.SelectSingleNode("./ShipMethodRef/ListID").InnerText;
                }

//Get value of FullName
                if (InvoiceRet.SelectSingleNode("./ShipMethodRef/FullName") != null)
                {
                    string FullName = InvoiceRet.SelectSingleNode("./ShipMethodRef/FullName").InnerText;
                }
            }
//Done with field values for ShipMethodRef aggregate

//Get value of Subtotal
            if (InvoiceRet.SelectSingleNode("./Subtotal") != null)
            {
                string Subtotal = InvoiceRet.SelectSingleNode("./Subtotal").InnerText;
            }

//Get all field values for ItemSalesTaxRef aggregate 
            XmlNode ItemSalesTaxRef = InvoiceRet.SelectSingleNode("./ItemSalesTaxRef");
            if (ItemSalesTaxRef != null)
            {
//Get value of ListID
                if (InvoiceRet.SelectSingleNode("./ItemSalesTaxRef/ListID") != null)
                {
                    string ListID = InvoiceRet.SelectSingleNode("./ItemSalesTaxRef/ListID").InnerText;
                }

//Get value of FullName
                if (InvoiceRet.SelectSingleNode("./ItemSalesTaxRef/FullName") != null)
                {
                    string FullName = InvoiceRet.SelectSingleNode("./ItemSalesTaxRef/FullName").InnerText;
                }
            }
//Done with field values for ItemSalesTaxRef aggregate

//Get value of SalesTaxPercentage
            if (InvoiceRet.SelectSingleNode("./SalesTaxPercentage") != null)
            {
                string SalesTaxPercentage = InvoiceRet.SelectSingleNode("./SalesTaxPercentage").InnerText;
            }

//Get value of SalesTaxTotal
            if (InvoiceRet.SelectSingleNode("./SalesTaxTotal") != null)
            {
                string SalesTaxTotal = InvoiceRet.SelectSingleNode("./SalesTaxTotal").InnerText;
            }

//Get value of AppliedAmount
            if (InvoiceRet.SelectSingleNode("./AppliedAmount") != null)
            {
                string AppliedAmount = InvoiceRet.SelectSingleNode("./AppliedAmount").InnerText;
            }

//Get value of BalanceRemaining
            if (InvoiceRet.SelectSingleNode("./BalanceRemaining") != null)
            {
                string BalanceRemaining = InvoiceRet.SelectSingleNode("./BalanceRemaining").InnerText;
            }

//Get all field values for CurrencyRef aggregate 
            XmlNode CurrencyRef = InvoiceRet.SelectSingleNode("./CurrencyRef");
            if (CurrencyRef != null)
            {
//Get value of ListID
                if (InvoiceRet.SelectSingleNode("./CurrencyRef/ListID") != null)
                {
                    string ListID = InvoiceRet.SelectSingleNode("./CurrencyRef/ListID").InnerText;
                }

//Get value of FullName
                if (InvoiceRet.SelectSingleNode("./CurrencyRef/FullName") != null)
                {
                    string FullName = InvoiceRet.SelectSingleNode("./CurrencyRef/FullName").InnerText;
                }
            }
//Done with field values for CurrencyRef aggregate

//Get value of ExchangeRate
            if (InvoiceRet.SelectSingleNode("./ExchangeRate") != null)
            {
                string ExchangeRate = InvoiceRet.SelectSingleNode("./ExchangeRate").InnerText;
            }

//Get value of BalanceRemainingInHomeCurrency
            if (InvoiceRet.SelectSingleNode("./BalanceRemainingInHomeCurrency") != null)
            {
                string BalanceRemainingInHomeCurrency =
                    InvoiceRet.SelectSingleNode("./BalanceRemainingInHomeCurrency").InnerText;
            }

//Get value of Memo
            if (InvoiceRet.SelectSingleNode("./Memo") != null)
            {
                string Memo = InvoiceRet.SelectSingleNode("./Memo").InnerText;
            }

//Get value of IsPaid
            if (InvoiceRet.SelectSingleNode("./IsPaid") != null)
            {
                string IsPaid = InvoiceRet.SelectSingleNode("./IsPaid").InnerText;
            }

//Get all field values for CustomerMsgRef aggregate 
            XmlNode CustomerMsgRef = InvoiceRet.SelectSingleNode("./CustomerMsgRef");
            if (CustomerMsgRef != null)
            {
//Get value of ListID
                if (InvoiceRet.SelectSingleNode("./CustomerMsgRef/ListID") != null)
                {
                    string ListID = InvoiceRet.SelectSingleNode("./CustomerMsgRef/ListID").InnerText;
                }

//Get value of FullName
                if (InvoiceRet.SelectSingleNode("./CustomerMsgRef/FullName") != null)
                {
                    string FullName = InvoiceRet.SelectSingleNode("./CustomerMsgRef/FullName").InnerText;
                }
            }
//Done with field values for CustomerMsgRef aggregate

//Get value of IsToBePrinted
            if (InvoiceRet.SelectSingleNode("./IsToBePrinted") != null)
            {
                string IsToBePrinted = InvoiceRet.SelectSingleNode("./IsToBePrinted").InnerText;
            }

//Get value of IsToBeEmailed
            if (InvoiceRet.SelectSingleNode("./IsToBeEmailed") != null)
            {
                string IsToBeEmailed = InvoiceRet.SelectSingleNode("./IsToBeEmailed").InnerText;
            }

//Get all field values for CustomerSalesTaxCodeRef aggregate 
            XmlNode CustomerSalesTaxCodeRef = InvoiceRet.SelectSingleNode("./CustomerSalesTaxCodeRef");
            if (CustomerSalesTaxCodeRef != null)
            {
//Get value of ListID
                if (InvoiceRet.SelectSingleNode("./CustomerSalesTaxCodeRef/ListID") != null)
                {
                    string ListID = InvoiceRet.SelectSingleNode("./CustomerSalesTaxCodeRef/ListID").InnerText;
                }

//Get value of FullName
                if (InvoiceRet.SelectSingleNode("./CustomerSalesTaxCodeRef/FullName") != null)
                {
                    string FullName = InvoiceRet.SelectSingleNode("./CustomerSalesTaxCodeRef/FullName").InnerText;
                }
            }
//Done with field values for CustomerSalesTaxCodeRef aggregate

//Get value of SuggestedDiscountAmount
            if (InvoiceRet.SelectSingleNode("./SuggestedDiscountAmount") != null)
            {
                string SuggestedDiscountAmount = InvoiceRet.SelectSingleNode("./SuggestedDiscountAmount").InnerText;
            }

//Get value of SuggestedDiscountDate
            if (InvoiceRet.SelectSingleNode("./SuggestedDiscountDate") != null)
            {
                string SuggestedDiscountDate = InvoiceRet.SelectSingleNode("./SuggestedDiscountDate").InnerText;
            }

//Get value of Other
            if (InvoiceRet.SelectSingleNode("./Other") != null)
            {
                string Other = InvoiceRet.SelectSingleNode("./Other").InnerText;
            }

//Get value of ExternalGUID
            if (InvoiceRet.SelectSingleNode("./ExternalGUID") != null)
            {
                string ExternalGUID = InvoiceRet.SelectSingleNode("./ExternalGUID").InnerText;
            }

//Walk list of LinkedTxn aggregates
            XmlNodeList LinkedTxnList = InvoiceRet.SelectNodes("./LinkedTxn");
            if (LinkedTxnList != null)
            {
                for (int i = 0; i < LinkedTxnList.Count; i++)
                {
                    XmlNode LinkedTxn = LinkedTxnList.Item(i);
//Get value of TxnID
                    string TxnID = LinkedTxn.SelectSingleNode("./TxnID").InnerText;
//Get value of TxnType
                    string TxnType = LinkedTxn.SelectSingleNode("./TxnType").InnerText;
//Get value of TxnDate
                    string TxnDate = LinkedTxn.SelectSingleNode("./TxnDate").InnerText;
//Get value of RefNumber
                    if (LinkedTxn.SelectSingleNode("./RefNumber") != null)
                    {
                        string RefNumber = LinkedTxn.SelectSingleNode("./RefNumber").InnerText;
                    }

//Get value of LinkType
                    if (LinkedTxn.SelectSingleNode("./LinkType") != null)
                    {
                        string LinkType = LinkedTxn.SelectSingleNode("./LinkType").InnerText;
                    }

//Get value of Amount
                    string Amount = LinkedTxn.SelectSingleNode("./Amount").InnerText;
                }
            }

            XmlNodeList ORInvoiceLineRetListChildren = InvoiceRet.SelectNodes("./*");
            for (int i = 0; i < ORInvoiceLineRetListChildren.Count; i++)
            {
                XmlNode Child = ORInvoiceLineRetListChildren.Item(i);
                if (Child.Name == "InvoiceLineRet")
                {
//Get value of TxnLineID
                    string TxnLineID = Child.SelectSingleNode("./TxnLineID").InnerText;
//Get all field values for ItemRef aggregate 
                    XmlNode ItemRef = Child.SelectSingleNode("./ItemRef");
                    if (ItemRef != null)
                    {
//Get value of ListID
                        if (Child.SelectSingleNode("./ItemRef/ListID") != null)
                        {
                            string ListID = Child.SelectSingleNode("./ItemRef/ListID").InnerText;
                        }

//Get value of FullName
                        if (Child.SelectSingleNode("./ItemRef/FullName") != null)
                        {
                            string FullName = Child.SelectSingleNode("./ItemRef/FullName").InnerText;
                        }
                    }
//Done with field values for ItemRef aggregate

//Get value of Desc
                    if (Child.SelectSingleNode("./Desc") != null)
                    {
                        string Desc = Child.SelectSingleNode("./Desc").InnerText;
                    }

//Get value of Quantity
                    if (Child.SelectSingleNode("./Quantity") != null)
                    {
                        string Quantity = Child.SelectSingleNode("./Quantity").InnerText;
                    }

//Get value of UnitOfMeasure
                    if (Child.SelectSingleNode("./UnitOfMeasure") != null)
                    {
                        string UnitOfMeasure = Child.SelectSingleNode("./UnitOfMeasure").InnerText;
                    }

//Get all field values for OverrideUOMSetRef aggregate 
                    XmlNode OverrideUOMSetRef = Child.SelectSingleNode("./OverrideUOMSetRef");
                    if (OverrideUOMSetRef != null)
                    {
//Get value of ListID
                        if (Child.SelectSingleNode("./OverrideUOMSetRef/ListID") != null)
                        {
                            string ListID = Child.SelectSingleNode("./OverrideUOMSetRef/ListID").InnerText;
                        }

//Get value of FullName
                        if (Child.SelectSingleNode("./OverrideUOMSetRef/FullName") != null)
                        {
                            string FullName = Child.SelectSingleNode("./OverrideUOMSetRef/FullName").InnerText;
                        }
                    }
//Done with field values for OverrideUOMSetRef aggregate

                    XmlNodeList ORRateChildren = Child.SelectNodes("./*");
                    for (int i = 0; i < ORRateChildren.Count; i++)
                    {
                        XmlNode Child = ORRateChildren.Item(i);
                        if (Child.Name == "Rate")
                        {
                        }

                        if (Child.Name == "RatePercent")
                        {
                        }
                    }

//Get all field values for ClassRef aggregate 
                    XmlNode ClassRef = Child.SelectSingleNode("./ClassRef");
                    if (ClassRef != null)
                    {
//Get value of ListID
                        if (Child.SelectSingleNode("./ClassRef/ListID") != null)
                        {
                            string ListID = Child.SelectSingleNode("./ClassRef/ListID").InnerText;
                        }

//Get value of FullName
                        if (Child.SelectSingleNode("./ClassRef/FullName") != null)
                        {
                            string FullName = Child.SelectSingleNode("./ClassRef/FullName").InnerText;
                        }
                    }
//Done with field values for ClassRef aggregate

//Get value of Amount
                    if (Child.SelectSingleNode("./Amount") != null)
                    {
                        string Amount = Child.SelectSingleNode("./Amount").InnerText;
                    }

//Get all field values for InventorySiteRef aggregate 
                    XmlNode InventorySiteRef = Child.SelectSingleNode("./InventorySiteRef");
                    if (InventorySiteRef != null)
                    {
//Get value of ListID
                        if (Child.SelectSingleNode("./InventorySiteRef/ListID") != null)
                        {
                            string ListID = Child.SelectSingleNode("./InventorySiteRef/ListID").InnerText;
                        }

//Get value of FullName
                        if (Child.SelectSingleNode("./InventorySiteRef/FullName") != null)
                        {
                            string FullName = Child.SelectSingleNode("./InventorySiteRef/FullName").InnerText;
                        }
                    }
//Done with field values for InventorySiteRef aggregate

//Get all field values for InventorySiteLocationRef aggregate 
                    XmlNode InventorySiteLocationRef = Child.SelectSingleNode("./InventorySiteLocationRef");
                    if (InventorySiteLocationRef != null)
                    {
//Get value of ListID
                        if (Child.SelectSingleNode("./InventorySiteLocationRef/ListID") != null)
                        {
                            string ListID = Child.SelectSingleNode("./InventorySiteLocationRef/ListID").InnerText;
                        }

//Get value of FullName
                        if (Child.SelectSingleNode("./InventorySiteLocationRef/FullName") != null)
                        {
                            string FullName = Child.SelectSingleNode("./InventorySiteLocationRef/FullName").InnerText;
                        }
                    }
//Done with field values for InventorySiteLocationRef aggregate

                    XmlNodeList ORSerialLotNumberChildren = Child.SelectNodes("./*");
                    for (int i = 0; i < ORSerialLotNumberChildren.Count; i++)
                    {
                        XmlNode Child = ORSerialLotNumberChildren.Item(i);
                        if (Child.Name == "SerialNumber")
                        {
                        }

                        if (Child.Name == "LotNumber")
                        {
                        }
                    }

//Get value of ServiceDate
                    if (Child.SelectSingleNode("./ServiceDate") != null)
                    {
                        string ServiceDate = Child.SelectSingleNode("./ServiceDate").InnerText;
                    }

//Get all field values for SalesTaxCodeRef aggregate 
                    XmlNode SalesTaxCodeRef = Child.SelectSingleNode("./SalesTaxCodeRef");
                    if (SalesTaxCodeRef != null)
                    {
//Get value of ListID
                        if (Child.SelectSingleNode("./SalesTaxCodeRef/ListID") != null)
                        {
                            string ListID = Child.SelectSingleNode("./SalesTaxCodeRef/ListID").InnerText;
                        }

//Get value of FullName
                        if (Child.SelectSingleNode("./SalesTaxCodeRef/FullName") != null)
                        {
                            string FullName = Child.SelectSingleNode("./SalesTaxCodeRef/FullName").InnerText;
                        }
                    }
//Done with field values for SalesTaxCodeRef aggregate

//Get value of Other1
                    if (Child.SelectSingleNode("./Other1") != null)
                    {
                        string Other1 = Child.SelectSingleNode("./Other1").InnerText;
                    }

//Get value of Other2
                    if (Child.SelectSingleNode("./Other2") != null)
                    {
                        string Other2 = Child.SelectSingleNode("./Other2").InnerText;
                    }

//Walk list of DataExtRet aggregates
                    XmlNodeList DataExtRetList = Child.SelectNodes("./DataExtRet");
                    if (DataExtRetList != null)
                    {
                        for (int i = 0; i < DataExtRetList.Count; i++)
                        {
                            XmlNode DataExtRet = DataExtRetList.Item(i);
//Get value of OwnerID
                            if (DataExtRet.SelectSingleNode("./OwnerID") != null)
                            {
                                string OwnerID = DataExtRet.SelectSingleNode("./OwnerID").InnerText;
                            }

//Get value of DataExtName
                            string DataExtName = DataExtRet.SelectSingleNode("./DataExtName").InnerText;
//Get value of DataExtType
                            string DataExtType = DataExtRet.SelectSingleNode("./DataExtType").InnerText;
//Get value of DataExtValue
                            string DataExtValue = DataExtRet.SelectSingleNode("./DataExtValue").InnerText;
                        }
                    }
                }

                if (Child.Name == "InvoiceLineGroupRet")
                {
//Get value of TxnLineID
                    string TxnLineID = Child.SelectSingleNode("./TxnLineID").InnerText;
//Get all field values for ItemGroupRef aggregate 
//Get value of ListID
                    if (Child.SelectSingleNode("./ItemGroupRef/ListID") != null)
                    {
                        string ListID = Child.SelectSingleNode("./ItemGroupRef/ListID").InnerText;
                    }

//Get value of FullName
                    if (Child.SelectSingleNode("./ItemGroupRef/FullName") != null)
                    {
                        string FullName = Child.SelectSingleNode("./ItemGroupRef/FullName").InnerText;
                    }
//Done with field values for ItemGroupRef aggregate

//Get value of Desc
                    if (Child.SelectSingleNode("./Desc") != null)
                    {
                        string Desc = Child.SelectSingleNode("./Desc").InnerText;
                    }

//Get value of Quantity
                    if (Child.SelectSingleNode("./Quantity") != null)
                    {
                        string Quantity = Child.SelectSingleNode("./Quantity").InnerText;
                    }

//Get value of UnitOfMeasure
                    if (Child.SelectSingleNode("./UnitOfMeasure") != null)
                    {
                        string UnitOfMeasure = Child.SelectSingleNode("./UnitOfMeasure").InnerText;
                    }

//Get all field values for OverrideUOMSetRef aggregate 
                    XmlNode OverrideUOMSetRef = Child.SelectSingleNode("./OverrideUOMSetRef");
                    if (OverrideUOMSetRef != null)
                    {
//Get value of ListID
                        if (Child.SelectSingleNode("./OverrideUOMSetRef/ListID") != null)
                        {
                            string ListID = Child.SelectSingleNode("./OverrideUOMSetRef/ListID").InnerText;
                        }

//Get value of FullName
                        if (Child.SelectSingleNode("./OverrideUOMSetRef/FullName") != null)
                        {
                            string FullName = Child.SelectSingleNode("./OverrideUOMSetRef/FullName").InnerText;
                        }
                    }
//Done with field values for OverrideUOMSetRef aggregate

//Get value of IsPrintItemsInGroup
                    string IsPrintItemsInGroup = Child.SelectSingleNode("./IsPrintItemsInGroup").InnerText;
//Get value of TotalAmount
                    string TotalAmount = Child.SelectSingleNode("./TotalAmount").InnerText;
//Walk list of InvoiceLineRet aggregates
                    XmlNodeList InvoiceLineRetList = Child.SelectNodes("./InvoiceLineRet");
                    if (InvoiceLineRetList != null)
                    {
                        for (int i = 0; i < InvoiceLineRetList.Count; i++)
                        {
                            XmlNode InvoiceLineRet = InvoiceLineRetList.Item(i);
//Get value of TxnLineID
                            string TxnLineID = InvoiceLineRet.SelectSingleNode("./TxnLineID").InnerText;
//Get all field values for ItemRef aggregate 
                            XmlNode ItemRef = InvoiceLineRet.SelectSingleNode("./ItemRef");
                            if (ItemRef != null)
                            {
//Get value of ListID
                                if (InvoiceLineRet.SelectSingleNode("./ItemRef/ListID") != null)
                                {
                                    string ListID = InvoiceLineRet.SelectSingleNode("./ItemRef/ListID").InnerText;
                                }

//Get value of FullName
                                if (InvoiceLineRet.SelectSingleNode("./ItemRef/FullName") != null)
                                {
                                    string FullName = InvoiceLineRet.SelectSingleNode("./ItemRef/FullName").InnerText;
                                }
                            }
//Done with field values for ItemRef aggregate

//Get value of Desc
                            if (InvoiceLineRet.SelectSingleNode("./Desc") != null)
                            {
                                string Desc = InvoiceLineRet.SelectSingleNode("./Desc").InnerText;
                            }

//Get value of Quantity
                            if (InvoiceLineRet.SelectSingleNode("./Quantity") != null)
                            {
                                string Quantity = InvoiceLineRet.SelectSingleNode("./Quantity").InnerText;
                            }

//Get value of UnitOfMeasure
                            if (InvoiceLineRet.SelectSingleNode("./UnitOfMeasure") != null)
                            {
                                string UnitOfMeasure = InvoiceLineRet.SelectSingleNode("./UnitOfMeasure").InnerText;
                            }

//Get all field values for OverrideUOMSetRef aggregate 
                            XmlNode OverrideUOMSetRef = InvoiceLineRet.SelectSingleNode("./OverrideUOMSetRef");
                            if (OverrideUOMSetRef != null)
                            {
//Get value of ListID
                                if (InvoiceLineRet.SelectSingleNode("./OverrideUOMSetRef/ListID") != null)
                                {
                                    string ListID = InvoiceLineRet.SelectSingleNode("./OverrideUOMSetRef/ListID")
                                        .InnerText;
                                }

//Get value of FullName
                                if (InvoiceLineRet.SelectSingleNode("./OverrideUOMSetRef/FullName") != null)
                                {
                                    string FullName = InvoiceLineRet.SelectSingleNode("./OverrideUOMSetRef/FullName")
                                        .InnerText;
                                }
                            }
//Done with field values for OverrideUOMSetRef aggregate

                            XmlNodeList ORRateChildren = InvoiceLineRet.SelectNodes("./*");
                            for (int i = 0; i < ORRateChildren.Count; i++)
                            {
                                XmlNode Child = ORRateChildren.Item(i);
                                if (Child.Name == "Rate")
                                {
                                }

                                if (Child.Name == "RatePercent")
                                {
                                }
                            }

//Get all field values for ClassRef aggregate 
                            XmlNode ClassRef = InvoiceLineRet.SelectSingleNode("./ClassRef");
                            if (ClassRef != null)
                            {
//Get value of ListID
                                if (InvoiceLineRet.SelectSingleNode("./ClassRef/ListID") != null)
                                {
                                    string ListID = InvoiceLineRet.SelectSingleNode("./ClassRef/ListID").InnerText;
                                }

//Get value of FullName
                                if (InvoiceLineRet.SelectSingleNode("./ClassRef/FullName") != null)
                                {
                                    string FullName = InvoiceLineRet.SelectSingleNode("./ClassRef/FullName").InnerText;
                                }
                            }
//Done with field values for ClassRef aggregate

//Get value of Amount
                            if (InvoiceLineRet.SelectSingleNode("./Amount") != null)
                            {
                                string Amount = InvoiceLineRet.SelectSingleNode("./Amount").InnerText;
                            }

//Get all field values for InventorySiteRef aggregate 
                            XmlNode InventorySiteRef = InvoiceLineRet.SelectSingleNode("./InventorySiteRef");
                            if (InventorySiteRef != null)
                            {
//Get value of ListID
                                if (InvoiceLineRet.SelectSingleNode("./InventorySiteRef/ListID") != null)
                                {
                                    string ListID = InvoiceLineRet.SelectSingleNode("./InventorySiteRef/ListID")
                                        .InnerText;
                                }

//Get value of FullName
                                if (InvoiceLineRet.SelectSingleNode("./InventorySiteRef/FullName") != null)
                                {
                                    string FullName = InvoiceLineRet.SelectSingleNode("./InventorySiteRef/FullName")
                                        .InnerText;
                                }
                            }
//Done with field values for InventorySiteRef aggregate

//Get all field values for InventorySiteLocationRef aggregate 
                            XmlNode InventorySiteLocationRef =
                                InvoiceLineRet.SelectSingleNode("./InventorySiteLocationRef");
                            if (InventorySiteLocationRef != null)
                            {
//Get value of ListID
                                if (InvoiceLineRet.SelectSingleNode("./InventorySiteLocationRef/ListID") != null)
                                {
                                    string ListID = InvoiceLineRet.SelectSingleNode("./InventorySiteLocationRef/ListID")
                                        .InnerText;
                                }

//Get value of FullName
                                if (InvoiceLineRet.SelectSingleNode("./InventorySiteLocationRef/FullName") != null)
                                {
                                    string FullName = InvoiceLineRet
                                        .SelectSingleNode("./InventorySiteLocationRef/FullName").InnerText;
                                }
                            }

                            //Done with field values for InventorySiteLocationRef aggregate
                            XmlNodeList ORSerialLotNumberChildren = InvoiceLineRet.SelectNodes("./*");
                            for (int i = 0; i < ORSerialLotNumberChildren.Count; i++)
                            {
                                XmlNode Child = ORSerialLotNumberChildren.Item(i);
                                if (Child.Name == "SerialNumber")
                                {
                                }

                                if (Child.Name == "LotNumber")
                                {
                                }
                            }

//Get value of ServiceDate
                            if (InvoiceLineRet.SelectSingleNode("./ServiceDate") != null)
                            {
                                string ServiceDate = InvoiceLineRet.SelectSingleNode("./ServiceDate").InnerText;
                            }

//Get all field values for SalesTaxCodeRef aggregate 
                            XmlNode SalesTaxCodeRef = InvoiceLineRet.SelectSingleNode("./SalesTaxCodeRef");
                            if (SalesTaxCodeRef != null)
                            {
//Get value of ListID
                                if (InvoiceLineRet.SelectSingleNode("./SalesTaxCodeRef/ListID") != null)
                                {
                                    string ListID = InvoiceLineRet.SelectSingleNode("./SalesTaxCodeRef/ListID")
                                        .InnerText;
                                }

                                //Get value of FullName
                                if (InvoiceLineRet.SelectSingleNode("./SalesTaxCodeRef/FullName") != null)
                                {
                                    string FullName = InvoiceLineRet.SelectSingleNode("./SalesTaxCodeRef/FullName")
                                        .InnerText;
                                }
                            }
                            //Done with field values for SalesTaxCodeRef aggregate

                            //Get value of Other1
                            if (InvoiceLineRet.SelectSingleNode("./Other1") != null)
                            {
                                string Other1 = InvoiceLineRet.SelectSingleNode("./Other1").InnerText;
                            }

                            // Get value of Other2
                            if (InvoiceLineRet.SelectSingleNode("./Other2") != null)
                            {
                                string Other2 = InvoiceLineRet.SelectSingleNode("./Other2").InnerText;
                            }

                            // Walk list of DataExtRet aggregates
                            XmlNodeList DataExtRetList = InvoiceLineRet.SelectNodes("./DataExtRet");
                            if (DataExtRetList != null)
                            {
                                for (int i = 0; i < DataExtRetList.Count; i++)
                                {
                                    XmlNode DataExtRet = DataExtRetList.Item(i);
                                    // Get value of OwnerID
                                    if (DataExtRet.SelectSingleNode("./OwnerID") != null)
                                    {
                                        string OwnerID = DataExtRet.SelectSingleNode("./OwnerID").InnerText;
                                    }

                                    //Get value of DataExtName
                                    string DataExtName = DataExtRet.SelectSingleNode("./DataExtName").InnerText;
                                    //Get value of DataExtType
                                    string DataExtType = DataExtRet.SelectSingleNode("./DataExtType").InnerText;
                                    //Get value of DataExtValue
                                    string DataExtValue = DataExtRet.SelectSingleNode("./DataExtValue").InnerText;
                                }
                            }
                        }
                    }

//Walk list of DataExtRet aggregates
                    XmlNodeList DataExtRetList = Child.SelectNodes("./DataExtRet");
                    if (DataExtRetList != null)
                    {
                        for (int i = 0; i < DataExtRetList.Count; i++)
                        {
                            XmlNode DataExtRet = DataExtRetList.Item(i);
//Get value of OwnerID
                            if (DataExtRet.SelectSingleNode("./OwnerID") != null)
                            {
                                string OwnerID = DataExtRet.SelectSingleNode("./OwnerID").InnerText;
                            }

//Get value of DataExtName
                            string DataExtName = DataExtRet.SelectSingleNode("./DataExtName").InnerText;
//Get value of DataExtType
                            string DataExtType = DataExtRet.SelectSingleNode("./DataExtType").InnerText;
//Get value of DataExtValue
                            string DataExtValue = DataExtRet.SelectSingleNode("./DataExtValue").InnerText;
                        }
                    }
                }
            }

//Walk list of DataExtRet aggregates
            XmlNodeList DataExtRetList = InvoiceRet.SelectNodes("./DataExtRet");
            if (DataExtRetList != null)
            {
                for (int i = 0; i < DataExtRetList.Count; i++)
                {
                    XmlNode DataExtRet = DataExtRetList.Item(i);
//Get value of OwnerID
                    if (DataExtRet.SelectSingleNode("./OwnerID") != null)
                    {
                        string OwnerID = DataExtRet.SelectSingleNode("./OwnerID").InnerText;
                    }

//Get value of DataExtName
                    string DataExtName = DataExtRet.SelectSingleNode("./DataExtName").InnerText;
//Get value of DataExtType
                    string DataExtType = DataExtRet.SelectSingleNode("./DataExtType").InnerText;
//Get value of DataExtValue
                    string DataExtValue = DataExtRet.SelectSingleNode("./DataExtValue").InnerText;
                }
            }
        }
    }
}