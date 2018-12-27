//The following sample code is generated as an illustration of
//Creating requests and parsing responses ONLY
//This code is NOT intended to show best practices or ideal code
//Use at your most careful discretion

using System;
using System.Net;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using Interop.QBFC13;

namespace SubscribeAndHandleQBEvent
{
    public class PurchaseOrderQuerySample
    {
        // 1 / 4 function 
        public void DoPurchaseOrderQuery()
        {
            bool sessionBegun = false;
            bool connectionOpen = false;
            //js - QBSessionManager is the RequestProcessor for QBFC
            //-- this class will open connection, begin session, and manage requests and responses
            QBSessionManager sessionManager = null;

            try
            {
                //Create the session Manager object
                sessionManager = new QBSessionManager();

                //Create the "message set" request object to hold our request
                IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", 13, 0);
                requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;
                
                // Hoist a class function
                BuildPurchaseOrderQueryRq(requestMsgSet);

                //Connect to QuickBooks and begin a session
                //js - sessionManager won't return a sessionTicket because this is handled internally  
                //-- by QBFC13 when using QBXMLRP2 .openConnection() will return a sessionTicket 
                sessionManager.OpenConnection("", "Redstone Print and Mail");
                connectionOpen = true;
                sessionManager.BeginSession("", ENOpenMode.omDontCare);
                sessionBegun = true;

                //Send the request and get the response from QuickBooks
                IMsgSetResponse responseMsgSet = sessionManager.DoRequests(requestMsgSet);

                //End the session and close the connection to QuickBooks
                sessionManager.EndSession();
                sessionBegun = false;
                sessionManager.CloseConnection();
                connectionOpen = false;

                WalkPurchaseOrderQueryRs(responseMsgSet);
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

        // 2 / 4 function, gets invoked in DoPurchaseOrderQuery()
        private void BuildPurchaseOrderQueryRq(IMsgSetRequest requestMsgSet)
        {
            IPurchaseOrderQuery PurchaseOrderQueryRq = requestMsgSet.AppendPurchaseOrderQueryRq();
            //js - Set attributes
            //Set field value for metaData
            PurchaseOrderQueryRq.metaData.SetValue(ENmetaData.mdMetaDataAndResponseData);
            //Set field value for iterator
            PurchaseOrderQueryRq.iterator.SetValue(ENiterator.itContinue);
            //Set field value for iteratorID
            PurchaseOrderQueryRq.iteratorID.SetValue("IQBUUIDType");
            string ORTxnQueryElementType18203 = "TxnIDList";
            if (ORTxnQueryElementType18203 == "TxnIDList")
            {
                //Set field value for TxnIDList
                //May create more than one of these if needed
                PurchaseOrderQueryRq.ORTxnQuery.TxnIDList.Add("200000-1011023419");
            }
            if (ORTxnQueryElementType18203 == "RefNumberList")
            {
                //Set field value for RefNumberList
                //May create more than one of these if needed
                PurchaseOrderQueryRq.ORTxnQuery.RefNumberList.Add("ab");
            }
            if (ORTxnQueryElementType18203 == "RefNumberCaseSensitiveList")
            {
                //Set field value for RefNumberCaseSensitiveList
                //May create more than one of these if needed
                PurchaseOrderQueryRq.ORTxnQuery.RefNumberCaseSensitiveList.Add("ab");
            }
            if (ORTxnQueryElementType18203 == "TxnFilter")
            {
                //Set field value for MaxReturned
                PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.MaxReturned.SetValue(6);
                string ORDateRangeFilterElementType18204 = "ModifiedDateRangeFilter";
                if (ORDateRangeFilterElementType18204 == "ModifiedDateRangeFilter")
                {
                    //Set field value for FromModifiedDate
                    PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.ModifiedDateRangeFilter.FromModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
                    //Set field value for ToModifiedDate
                    PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.ModifiedDateRangeFilter.ToModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
                }
                if (ORDateRangeFilterElementType18204 == "TxnDateRangeFilter")
                {
                    string ORTxnDateRangeFilterElementType18205 = "TxnDateFilter";
                    if (ORTxnDateRangeFilterElementType18205 == "TxnDateFilter")
                    {
                        //Set field value for FromTxnDate
                        PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.FromTxnDate.SetValue(DateTime.Parse("12/15/2007"));
                        //Set field value for ToTxnDate
                        PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.ToTxnDate.SetValue(DateTime.Parse("12/15/2007"));
                    }
                    if (ORTxnDateRangeFilterElementType18205 == "DateMacro")
                    {
                        //Set field value for DateMacro
                        PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.DateMacro.SetValue(ENDateMacro.dmAll);
                    }
                }
                string OREntityFilterElementType18206 = "ListIDList";
                if (OREntityFilterElementType18206 == "ListIDList")
                {
                    //Set field value for ListIDList
                    //May create more than one of these if needed
                    PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.EntityFilter.OREntityFilter.ListIDList.Add("200000-1011023419");
                }
                if (OREntityFilterElementType18206 == "FullNameList")
                {
                    //Set field value for FullNameList
                    //May create more than one of these if needed
                    PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.EntityFilter.OREntityFilter.FullNameList.Add("ab");
                }
                if (OREntityFilterElementType18206 == "ListIDWithChildren")
                {
                    //Set field value for ListIDWithChildren
                    PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.EntityFilter.OREntityFilter.ListIDWithChildren.SetValue("200000-1011023419");
                }
                if (OREntityFilterElementType18206 == "FullNameWithChildren")
                {
                    //Set field value for FullNameWithChildren
                    PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.EntityFilter.OREntityFilter.FullNameWithChildren.SetValue("ab");
                }
                string ORAccountFilterElementType18207 = "ListIDList";
                if (ORAccountFilterElementType18207 == "ListIDList")
                {
                    //Set field value for ListIDList
                    //May create more than one of these if needed
                    PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.AccountFilter.ORAccountFilter.ListIDList.Add("200000-1011023419");
                }
                if (ORAccountFilterElementType18207 == "FullNameList")
                {
                    //Set field value for FullNameList
                    //May create more than one of these if needed
                    PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.AccountFilter.ORAccountFilter.FullNameList.Add("ab");
                }
                if (ORAccountFilterElementType18207 == "ListIDWithChildren")
                {
                    //Set field value for ListIDWithChildren
                    PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.AccountFilter.ORAccountFilter.ListIDWithChildren.SetValue("200000-1011023419");
                }
                if (ORAccountFilterElementType18207 == "FullNameWithChildren")
                {
                    //Set field value for FullNameWithChildren
                    PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.AccountFilter.ORAccountFilter.FullNameWithChildren.SetValue("ab");
                }
                string ORRefNumberFilterElementType18208 = "RefNumberFilter";
                if (ORRefNumberFilterElementType18208 == "RefNumberFilter")
                {
                    //Set field value for MatchCriterion
                    PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.ORRefNumberFilter.RefNumberFilter.MatchCriterion.SetValue(ENMatchCriterion.mcStartsWith);
                    //Set field value for RefNumber
                    PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.ORRefNumberFilter.RefNumberFilter.RefNumber.SetValue("ab");
                }
                if (ORRefNumberFilterElementType18208 == "RefNumberRangeFilter")
                {
                    //Set field value for FromRefNumber
                    PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.ORRefNumberFilter.RefNumberRangeFilter.FromRefNumber.SetValue("ab");
                    //Set field value for ToRefNumber
                    PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.ORRefNumberFilter.RefNumberRangeFilter.ToRefNumber.SetValue("ab");
                }
                string ORCurrencyFilterElementType18209 = "ListIDList";
                if (ORCurrencyFilterElementType18209 == "ListIDList")
                {
                    //Set field value for ListIDList
                    //May create more than one of these if needed
                    PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.CurrencyFilter.ORCurrencyFilter.ListIDList.Add("200000-1011023419");
                }
                if (ORCurrencyFilterElementType18209 == "FullNameList")
                {
                    //Set field value for FullNameList
                    //May create more than one of these if needed
                    PurchaseOrderQueryRq.ORTxnQuery.TxnFilter.CurrencyFilter.ORCurrencyFilter.FullNameList.Add("ab");
                }
            } // END OF: if(ORTxnQueryElementType18203 == enORTxnQueryElementType.TxnFilter){}
            
            //Set field value for IncludeLineItems
            PurchaseOrderQueryRq.IncludeLineItems.SetValue(true);
            //Set field value for IncludeLinkedTxns
            PurchaseOrderQueryRq.IncludeLinkedTxns.SetValue(true);
            //Set field value for IncludeRetElementList
            //May create more than one of these if needed
            PurchaseOrderQueryRq.IncludeRetElementList.Add("ab");
            //Set field value for OwnerIDList
            //May create more than one of these if needed
            PurchaseOrderQueryRq.OwnerIDList.Add(Guid.NewGuid().ToString());
        }

        // 3 / 4 function
        private void WalkPurchaseOrderQueryRs(IMsgSetResponse responseMsgSet)
        {
            if (responseMsgSet == null) return;
            IResponseList responseList = responseMsgSet.ResponseList;
            if (responseList == null) return;
            //if we sent only one request, there is only one response, we'll walk the list for this sample
            for (int i = 0; i < responseList.Count; i++)
            {
                IResponse response = responseList.GetAt(i);
                //check the status code of the response, 0=ok, >0 is warning
                if (response.StatusCode >= 0)
                {
                    //the request-specific response is in the details, make sure we have some
                    if (response.Detail != null)
                    {
                        //make sure the response is the type we're expecting
                        ENResponseType responseType = (ENResponseType)response.Type.GetValue();
                        if (responseType == ENResponseType.rtPurchaseOrderQueryRs)
                        {
                            // upcast to more specific type here, this is safe because we checked with response.
                            // Type check above
                            IPurchaseOrderRetList PurchaseOrderRet = (IPurchaseOrderRetList)response.Detail;
                            WalkPurchaseOrderRet(PurchaseOrderRet);
                        }
                    }
                }
            }
        }

        // 4 / 4 function, MAIN FUNCTION... I think. 
        private void WalkPurchaseOrderRet(IPurchaseOrderRetList PurchaseOrderRet)
        {
            if (PurchaseOrderRet == null) return;
            //Go through all the elements of IPurchaseOrderRetList
            //Get value of TxnID
            string TxnID18210 = (string)PurchaseOrderRet.TxnID.GetValue();
            //Get value of TimeCreated
            DateTime TimeCreated18211 = (DateTime)PurchaseOrderRet.TimeCreated.GetValue();
            //Get value of TimeModified
            DateTime TimeModified18212 = (DateTime)PurchaseOrderRet.TimeModified.GetValue();
            //Get value of EditSequence
            string EditSequence18213 = (string)PurchaseOrderRet.EditSequence.GetValue();
            //Get value of TxnNumber
            if (PurchaseOrderRet.TxnNumber != null)
            {
                int TxnNumber18214 = (int)PurchaseOrderRet.TxnNumber.GetValue();
            }
            if (PurchaseOrderRet.VendorRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.VendorRef.ListID != null)
                {
                    string ListID18215 = (string)PurchaseOrderRet.VendorRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.VendorRef.FullName != null)
                {
                    string FullName18216 = (string)PurchaseOrderRet.VendorRef.FullName.GetValue();
                }
            }
            if (PurchaseOrderRet.ClassRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.ClassRef.ListID != null)
                {
                    string ListID18217 = (string)PurchaseOrderRet.ClassRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.ClassRef.FullName != null)
                {
                    string FullName18218 = (string)PurchaseOrderRet.ClassRef.FullName.GetValue();
                }
            }
            if (PurchaseOrderRet.InventorySiteRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.InventorySiteRef.ListID != null)
                {
                    string ListID18219 = (string)PurchaseOrderRet.InventorySiteRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.InventorySiteRef.FullName != null)
                {
                    string FullName18220 = (string)PurchaseOrderRet.InventorySiteRef.FullName.GetValue();
                }
            }
            if (PurchaseOrderRet.ShipToEntityRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.ShipToEntityRef.ListID != null)
                {
                    string ListID18221 = (string)PurchaseOrderRet.ShipToEntityRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.ShipToEntityRef.FullName != null)
                {
                    string FullName18222 = (string)PurchaseOrderRet.ShipToEntityRef.FullName.GetValue();
                }
            }
            if (PurchaseOrderRet.TemplateRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.TemplateRef.ListID != null)
                {
                    string ListID18223 = (string)PurchaseOrderRet.TemplateRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.TemplateRef.FullName != null)
                {
                    string FullName18224 = (string)PurchaseOrderRet.TemplateRef.FullName.GetValue();
                }
            }
            //Get value of TxnDate
            DateTime TxnDate18225 = (DateTime)PurchaseOrderRet.TxnDate.GetValue();
            //Get value of RefNumber
            if (PurchaseOrderRet.RefNumber != null)
            {
                string RefNumber18226 = (string)PurchaseOrderRet.RefNumber.GetValue();
            }
            if (PurchaseOrderRet.VendorAddress != null)
            {
                //Get value of Addr1
                if (PurchaseOrderRet.VendorAddress.Addr1 != null)
                {
                    string Addr118227 = (string)PurchaseOrderRet.VendorAddress.Addr1.GetValue();
                }
                //Get value of Addr2
                if (PurchaseOrderRet.VendorAddress.Addr2 != null)
                {
                    string Addr218228 = (string)PurchaseOrderRet.VendorAddress.Addr2.GetValue();
                }
                //Get value of Addr3
                if (PurchaseOrderRet.VendorAddress.Addr3 != null)
                {
                    string Addr318229 = (string)PurchaseOrderRet.VendorAddress.Addr3.GetValue();
                }
                //Get value of Addr4
                if (PurchaseOrderRet.VendorAddress.Addr4 != null)
                {
                    string Addr418230 = (string)PurchaseOrderRet.VendorAddress.Addr4.GetValue();
                }
                //Get value of Addr5
                if (PurchaseOrderRet.VendorAddress.Addr5 != null)
                {
                    string Addr518231 = (string)PurchaseOrderRet.VendorAddress.Addr5.GetValue();
                }
                //Get value of City
                if (PurchaseOrderRet.VendorAddress.City != null)
                {
                    string City18232 = (string)PurchaseOrderRet.VendorAddress.City.GetValue();
                }
                //Get value of State
                if (PurchaseOrderRet.VendorAddress.State != null)
                {
                    string State18233 = (string)PurchaseOrderRet.VendorAddress.State.GetValue();
                }
                //Get value of PostalCode
                if (PurchaseOrderRet.VendorAddress.PostalCode != null)
                {
                    string PostalCode18234 = (string)PurchaseOrderRet.VendorAddress.PostalCode.GetValue();
                }
                //Get value of Country
                if (PurchaseOrderRet.VendorAddress.Country != null)
                {
                    string Country18235 = (string)PurchaseOrderRet.VendorAddress.Country.GetValue();
                }
                //Get value of Note
                if (PurchaseOrderRet.VendorAddress.Note != null)
                {
                    string Note18236 = (string)PurchaseOrderRet.VendorAddress.Note.GetValue();
                }
            }
            if (PurchaseOrderRet.VendorAddressBlock != null)
            {
                //Get value of Addr1
                if (PurchaseOrderRet.VendorAddressBlock.Addr1 != null)
                {
                    string Addr118237 = (string)PurchaseOrderRet.VendorAddressBlock.Addr1.GetValue();
                }
                //Get value of Addr2
                if (PurchaseOrderRet.VendorAddressBlock.Addr2 != null)
                {
                    string Addr218238 = (string)PurchaseOrderRet.VendorAddressBlock.Addr2.GetValue();
                }
                //Get value of Addr3
                if (PurchaseOrderRet.VendorAddressBlock.Addr3 != null)
                {
                    string Addr318239 = (string)PurchaseOrderRet.VendorAddressBlock.Addr3.GetValue();
                }
                //Get value of Addr4
                if (PurchaseOrderRet.VendorAddressBlock.Addr4 != null)
                {
                    string Addr418240 = (string)PurchaseOrderRet.VendorAddressBlock.Addr4.GetValue();
                }
                //Get value of Addr5
                if (PurchaseOrderRet.VendorAddressBlock.Addr5 != null)
                {
                    string Addr518241 = (string)PurchaseOrderRet.VendorAddressBlock.Addr5.GetValue();
                }
            }
            if (PurchaseOrderRet.ShipAddress != null)
            {
                //Get value of Addr1
                if (PurchaseOrderRet.ShipAddress.Addr1 != null)
                {
                    string Addr118242 = (string)PurchaseOrderRet.ShipAddress.Addr1.GetValue();
                }
                //Get value of Addr2
                if (PurchaseOrderRet.ShipAddress.Addr2 != null)
                {
                    string Addr218243 = (string)PurchaseOrderRet.ShipAddress.Addr2.GetValue();
                }
                //Get value of Addr3
                if (PurchaseOrderRet.ShipAddress.Addr3 != null)
                {
                    string Addr318244 = (string)PurchaseOrderRet.ShipAddress.Addr3.GetValue();
                }
                //Get value of Addr4
                if (PurchaseOrderRet.ShipAddress.Addr4 != null)
                {
                    string Addr418245 = (string)PurchaseOrderRet.ShipAddress.Addr4.GetValue();
                }
                //Get value of Addr5
                if (PurchaseOrderRet.ShipAddress.Addr5 != null)
                {
                    string Addr518246 = (string)PurchaseOrderRet.ShipAddress.Addr5.GetValue();
                }
                //Get value of City
                if (PurchaseOrderRet.ShipAddress.City != null)
                {
                    string City18247 = (string)PurchaseOrderRet.ShipAddress.City.GetValue();
                }
                //Get value of State
                if (PurchaseOrderRet.ShipAddress.State != null)
                {
                    string State18248 = (string)PurchaseOrderRet.ShipAddress.State.GetValue();
                }
                //Get value of PostalCode
                if (PurchaseOrderRet.ShipAddress.PostalCode != null)
                {
                    string PostalCode18249 = (string)PurchaseOrderRet.ShipAddress.PostalCode.GetValue();
                }
                //Get value of Country
                if (PurchaseOrderRet.ShipAddress.Country != null)
                {
                    string Country18250 = (string)PurchaseOrderRet.ShipAddress.Country.GetValue();
                }
                //Get value of Note
                if (PurchaseOrderRet.ShipAddress.Note != null)
                {
                    string Note18251 = (string)PurchaseOrderRet.ShipAddress.Note.GetValue();
                }
            }
            if (PurchaseOrderRet.ShipAddressBlock != null)
            {
                //Get value of Addr1
                if (PurchaseOrderRet.ShipAddressBlock.Addr1 != null)
                {
                    string Addr118252 = (string)PurchaseOrderRet.ShipAddressBlock.Addr1.GetValue();
                }
                //Get value of Addr2
                if (PurchaseOrderRet.ShipAddressBlock.Addr2 != null)
                {
                    string Addr218253 = (string)PurchaseOrderRet.ShipAddressBlock.Addr2.GetValue();
                }
                //Get value of Addr3
                if (PurchaseOrderRet.ShipAddressBlock.Addr3 != null)
                {
                    string Addr318254 = (string)PurchaseOrderRet.ShipAddressBlock.Addr3.GetValue();
                }
                //Get value of Addr4
                if (PurchaseOrderRet.ShipAddressBlock.Addr4 != null)
                {
                    string Addr418255 = (string)PurchaseOrderRet.ShipAddressBlock.Addr4.GetValue();
                }
                //Get value of Addr5
                if (PurchaseOrderRet.ShipAddressBlock.Addr5 != null)
                {
                    string Addr518256 = (string)PurchaseOrderRet.ShipAddressBlock.Addr5.GetValue();
                }
            }
            if (PurchaseOrderRet.TermsRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.TermsRef.ListID != null)
                {
                    string ListID18257 = (string)PurchaseOrderRet.TermsRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.TermsRef.FullName != null)
                {
                    string FullName18258 = (string)PurchaseOrderRet.TermsRef.FullName.GetValue();
                }
            }
            //Get value of DueDate
            if (PurchaseOrderRet.DueDate != null)
            {
                DateTime DueDate18259 = (DateTime)PurchaseOrderRet.DueDate.GetValue();
            }
            //Get value of ExpectedDate
            if (PurchaseOrderRet.ExpectedDate != null)
            {
                DateTime ExpectedDate18260 = (DateTime)PurchaseOrderRet.ExpectedDate.GetValue();
            }
            if (PurchaseOrderRet.ShipMethodRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.ShipMethodRef.ListID != null)
                {
                    string ListID18261 = (string)PurchaseOrderRet.ShipMethodRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.ShipMethodRef.FullName != null)
                {
                    string FullName18262 = (string)PurchaseOrderRet.ShipMethodRef.FullName.GetValue();
                }
            }
            //Get value of FOB
            if (PurchaseOrderRet.FOB != null)
            {
                string FOB18263 = (string)PurchaseOrderRet.FOB.GetValue();
            }
            //Get value of TotalAmount
            if (PurchaseOrderRet.TotalAmount != null)
            {
                double TotalAmount18264 = (double)PurchaseOrderRet.TotalAmount.GetValue();
            }
            if (PurchaseOrderRet.CurrencyRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.CurrencyRef.ListID != null)
                {
                    string ListID18265 = (string)PurchaseOrderRet.CurrencyRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.CurrencyRef.FullName != null)
                {
                    string FullName18266 = (string)PurchaseOrderRet.CurrencyRef.FullName.GetValue();
                }
            }
            //Get value of ExchangeRate
            if (PurchaseOrderRet.ExchangeRate != null)
            {
                IQBFloatType ExchangeRate18267 = (IQBFloatType)PurchaseOrderRet.ExchangeRate.GetValue();
            }
            //Get value of TotalAmountInHomeCurrency
            if (PurchaseOrderRet.TotalAmountInHomeCurrency != null)
            {
                double TotalAmountInHomeCurrency18268 = (double)PurchaseOrderRet.TotalAmountInHomeCurrency.GetValue();
            }
            //Get value of IsManuallyClosed
            if (PurchaseOrderRet.IsManuallyClosed != null)
            {
                bool IsManuallyClosed18269 = (bool)PurchaseOrderRet.IsManuallyClosed.GetValue();
            }
            //Get value of IsFullyReceived
            if (PurchaseOrderRet.IsFullyReceived != null)
            {
                bool IsFullyReceived18270 = (bool)PurchaseOrderRet.IsFullyReceived.GetValue();
            }
            //Get value of Memo
            if (PurchaseOrderRet.Memo != null)
            {
                string Memo18271 = (string)PurchaseOrderRet.Memo.GetValue();
            }
            //Get value of VendorMsg
            if (PurchaseOrderRet.VendorMsg != null)
            {
                string VendorMsg18272 = (string)PurchaseOrderRet.VendorMsg.GetValue();
            }
            //Get value of IsToBePrinted
            if (PurchaseOrderRet.IsToBePrinted != null)
            {
                bool IsToBePrinted18273 = (bool)PurchaseOrderRet.IsToBePrinted.GetValue();
            }
            //Get value of IsToBeEmailed
            if (PurchaseOrderRet.IsToBeEmailed != null)
            {
                bool IsToBeEmailed18274 = (bool)PurchaseOrderRet.IsToBeEmailed.GetValue();
            }
            //Get value of IsTaxIncluded
            if (PurchaseOrderRet.IsTaxIncluded != null)
            {
                bool IsTaxIncluded18275 = (bool)PurchaseOrderRet.IsTaxIncluded.GetValue();
            }
            if (PurchaseOrderRet.SalesTaxCodeRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.SalesTaxCodeRef.ListID != null)
                {
                    string ListID18276 = (string)PurchaseOrderRet.SalesTaxCodeRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.SalesTaxCodeRef.FullName != null)
                {
                    string FullName18277 = (string)PurchaseOrderRet.SalesTaxCodeRef.FullName.GetValue();
                }
            }
            //Get value of Other1
            if (PurchaseOrderRet.Other1 != null)
            {
                string Other118278 = (string)PurchaseOrderRet.Other1.GetValue();
            }
            //Get value of Other2
            if (PurchaseOrderRet.Other2 != null)
            {
                string Other218279 = (string)PurchaseOrderRet.Other2.GetValue();
            }
            //Get value of ExternalGUID
            if (PurchaseOrderRet.ExternalGUID != null)
            {
                string ExternalGUID18280 = (string)PurchaseOrderRet.ExternalGUID.GetValue();
            }
            if (PurchaseOrderRet.LinkedTxnList != null)
            {
                for (int i18281 = 0; i18281 < PurchaseOrderRet.LinkedTxnList.Count; i18281++)
                {
                    ILinkedTxn LinkedTxn = PurchaseOrderRet.LinkedTxnList.GetAt(i18281);
                    //Get value of TxnID
                    string TxnID18282 = (string)LinkedTxn.TxnID.GetValue();
                    //Get value of TxnType
                    ENTxnType TxnType18283 = (ENTxnType)LinkedTxn.TxnType.GetValue();
                    //Get value of TxnDate
                    DateTime TxnDate18284 = (DateTime)LinkedTxn.TxnDate.GetValue();
                    //Get value of RefNumber
                    if (LinkedTxn.RefNumber != null)
                    {
                        string RefNumber18285 = (string)LinkedTxn.RefNumber.GetValue();
                    }
                    //Get value of LinkType
                    if (LinkedTxn.LinkType != null)
                    {
                        ENLinkType LinkType18286 = (ENLinkType)LinkedTxn.LinkType.GetValue();
                    }
                    //Get value of Amount
                    double Amount18287 = (double)LinkedTxn.Amount.GetValue();
                }
            }
            if (PurchaseOrderRet.ORPurchaseOrderLineRetList != null)
            {
                for (int i18288 = 0; i18288 < PurchaseOrderRet.ORPurchaseOrderLineRetList.Count; i18288++)
                {
                    IORPurchaseOrderLineRet ORPurchaseOrderLineRet18289 = PurchaseOrderRet.ORPurchaseOrderLineRetList.GetAt(i18288);
                    if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet != null)
                    {
                        if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet != null)
                        {
                            //Get value of TxnLineID
                            string TxnLineID18290 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.TxnLineID.GetValue();
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.ItemRef != null)
                            {
                                //Get value of ListID
                                if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.ItemRef.ListID != null)
                                {
                                    string ListID18291 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.ItemRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.ItemRef.FullName != null)
                                {
                                    string FullName18292 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.ItemRef.FullName.GetValue();
                                }
                            }
                            //Get value of ManufacturerPartNumber
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.ManufacturerPartNumber != null)
                            {
                                string ManufacturerPartNumber18293 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.ManufacturerPartNumber.GetValue();
                            }
                            //Get value of Desc
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.Desc != null)
                            {
                                string Desc18294 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.Desc.GetValue();
                            }
                            //Get value of Quantity
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.Quantity != null)
                            {
                                int Quantity18295 = (int)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.Quantity.GetValue();
                            }
                            //Get value of UnitOfMeasure
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.UnitOfMeasure != null)
                            {
                                string UnitOfMeasure18296 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.UnitOfMeasure.GetValue();
                            }
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.OverrideUOMSetRef != null)
                            {
                                //Get value of ListID
                                if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.OverrideUOMSetRef.ListID != null)
                                {
                                    string ListID18297 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.OverrideUOMSetRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.OverrideUOMSetRef.FullName != null)
                                {
                                    string FullName18298 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.OverrideUOMSetRef.FullName.GetValue();
                                }
                            }
                            //Get value of Rate
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.Rate != null)
                            {
                                double Rate18299 = (double)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.Rate.GetValue();
                            }
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.ClassRef != null)
                            {
                                //Get value of ListID
                                if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.ClassRef.ListID != null)
                                {
                                    string ListID18300 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.ClassRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.ClassRef.FullName != null)
                                {
                                    string FullName18301 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.ClassRef.FullName.GetValue();
                                }
                            }
                            //Get value of Amount
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.Amount != null)
                            {
                                double Amount18302 = (double)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.Amount.GetValue();
                            }
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.InventorySiteLocationRef != null)
                            {
                                //Get value of ListID
                                if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.InventorySiteLocationRef.ListID != null)
                                {
                                    string ListID18303 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.InventorySiteLocationRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.InventorySiteLocationRef.FullName != null)
                                {
                                    string FullName18304 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.InventorySiteLocationRef.FullName.GetValue();
                                }
                            }
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.CustomerRef != null)
                            {
                                //Get value of ListID
                                if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.CustomerRef.ListID != null)
                                {
                                    string ListID18305 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.CustomerRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.CustomerRef.FullName != null)
                                {
                                    string FullName18306 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.CustomerRef.FullName.GetValue();
                                }
                            }
                            //Get value of ServiceDate
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.ServiceDate != null)
                            {
                                DateTime ServiceDate18307 = (DateTime)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.ServiceDate.GetValue();
                            }
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.SalesTaxCodeRef != null)
                            {
                                //Get value of ListID
                                if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.SalesTaxCodeRef.ListID != null)
                                {
                                    string ListID18308 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.SalesTaxCodeRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.SalesTaxCodeRef.FullName != null)
                                {
                                    string FullName18309 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.SalesTaxCodeRef.FullName.GetValue();
                                }
                            }
                            //Get value of ReceivedQuantity
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.ReceivedQuantity != null)
                            {
                                int ReceivedQuantity18310 = (int)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.ReceivedQuantity.GetValue();
                            }
                            //Get value of UnbilledQuantity
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.UnbilledQuantity != null)
                            {
                                int UnbilledQuantity18311 = (int)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.UnbilledQuantity.GetValue();
                            }
                            //Get value of IsBilled
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.IsBilled != null)
                            {
                                bool IsBilled18312 = (bool)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.IsBilled.GetValue();
                            }
                            //Get value of IsManuallyClosed
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.IsManuallyClosed != null)
                            {
                                bool IsManuallyClosed18313 = (bool)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.IsManuallyClosed.GetValue();
                            }
                            //Get value of Other1
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.Other1 != null)
                            {
                                string Other118314 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.Other1.GetValue();
                            }
                            //Get value of Other2
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.Other2 != null)
                            {
                                string Other218315 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.Other2.GetValue();
                            }
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.DataExtRetList != null)
                            {
                                for (int i18316 = 0; i18316 < ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.DataExtRetList.Count; i18316++)
                                {
                                    IDataExtRet DataExtRet = ORPurchaseOrderLineRet18289.PurchaseOrderLineRet.DataExtRetList.GetAt(i18316);
                                    //Get value of OwnerID
                                    if (DataExtRet.OwnerID != null)
                                    {
                                        string OwnerID18317 = (string)DataExtRet.OwnerID.GetValue();
                                    }
                                    //Get value of DataExtName
                                    string DataExtName18318 = (string)DataExtRet.DataExtName.GetValue();
                                    //Get value of DataExtType
                                    ENDataExtType DataExtType18319 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
                                    //Get value of DataExtValue
                                    string DataExtValue18320 = (string)DataExtRet.DataExtValue.GetValue();
                                }
                            }
                        }
                    }
                    if (ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet != null)
                    {
                        if (ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet != null)
                        {
                            //Get value of TxnLineID
                            string TxnLineID18321 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.TxnLineID.GetValue();
                            //Get value of ListID
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.ItemGroupRef.ListID != null)
                            {
                                string ListID18322 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.ItemGroupRef.ListID.GetValue();
                            }
                            //Get value of FullName
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.ItemGroupRef.FullName != null)
                            {
                                string FullName18323 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.ItemGroupRef.FullName.GetValue();
                            }
                            //Get value of Desc
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.Desc != null)
                            {
                                string Desc18324 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.Desc.GetValue();
                            }
                            //Get value of Quantity
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.Quantity != null)
                            {
                                int Quantity18325 = (int)ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.Quantity.GetValue();
                            }
                            //Get value of UnitOfMeasure
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.UnitOfMeasure != null)
                            {
                                string UnitOfMeasure18326 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.UnitOfMeasure.GetValue();
                            }
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.OverrideUOMSetRef != null)
                            {
                                //Get value of ListID
                                if (ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.OverrideUOMSetRef.ListID != null)
                                {
                                    string ListID18327 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.OverrideUOMSetRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.OverrideUOMSetRef.FullName != null)
                                {
                                    string FullName18328 = (string)ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.OverrideUOMSetRef.FullName.GetValue();
                                }
                            }
                            //Get value of IsPrintItemsInGroup
                            bool IsPrintItemsInGroup18329 = (bool)ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.IsPrintItemsInGroup.GetValue();
                            //Get value of TotalAmount
                            double TotalAmount18330 = (double)ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.TotalAmount.GetValue();
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.PurchaseOrderLineRetList != null)
                            {
                                for (int i18331 = 0; i18331 < ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.PurchaseOrderLineRetList.Count; i18331++)
                                {
                                    IPurchaseOrderLineRet PurchaseOrderLineRet = ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.PurchaseOrderLineRetList.GetAt(i18331);
                                    //Get value of TxnLineID
                                    string TxnLineID18332 = (string)PurchaseOrderLineRet.TxnLineID.GetValue();
                                    if (PurchaseOrderLineRet.ItemRef != null)
                                    {
                                        //Get value of ListID
                                        if (PurchaseOrderLineRet.ItemRef.ListID != null)
                                        {
                                            string ListID18333 = (string)PurchaseOrderLineRet.ItemRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (PurchaseOrderLineRet.ItemRef.FullName != null)
                                        {
                                            string FullName18334 = (string)PurchaseOrderLineRet.ItemRef.FullName.GetValue();
                                        }
                                    }
                                    //Get value of ManufacturerPartNumber
                                    if (PurchaseOrderLineRet.ManufacturerPartNumber != null)
                                    {
                                        string ManufacturerPartNumber18335 = (string)PurchaseOrderLineRet.ManufacturerPartNumber.GetValue();
                                    }
                                    //Get value of Desc
                                    if (PurchaseOrderLineRet.Desc != null)
                                    {
                                        string Desc18336 = (string)PurchaseOrderLineRet.Desc.GetValue();
                                    }
                                    //Get value of Quantity
                                    if (PurchaseOrderLineRet.Quantity != null)
                                    {
                                        int Quantity18337 = (int)PurchaseOrderLineRet.Quantity.GetValue();
                                    }
                                    //Get value of UnitOfMeasure
                                    if (PurchaseOrderLineRet.UnitOfMeasure != null)
                                    {
                                        string UnitOfMeasure18338 = (string)PurchaseOrderLineRet.UnitOfMeasure.GetValue();
                                    }
                                    if (PurchaseOrderLineRet.OverrideUOMSetRef != null)
                                    {
                                        //Get value of ListID
                                        if (PurchaseOrderLineRet.OverrideUOMSetRef.ListID != null)
                                        {
                                            string ListID18339 = (string)PurchaseOrderLineRet.OverrideUOMSetRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (PurchaseOrderLineRet.OverrideUOMSetRef.FullName != null)
                                        {
                                            string FullName18340 = (string)PurchaseOrderLineRet.OverrideUOMSetRef.FullName.GetValue();
                                        }
                                    }
                                    //Get value of Rate
                                    if (PurchaseOrderLineRet.Rate != null)
                                    {
                                        double Rate18341 = (double)PurchaseOrderLineRet.Rate.GetValue();
                                    }
                                    if (PurchaseOrderLineRet.ClassRef != null)
                                    {
                                        //Get value of ListID
                                        if (PurchaseOrderLineRet.ClassRef.ListID != null)
                                        {
                                            string ListID18342 = (string)PurchaseOrderLineRet.ClassRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (PurchaseOrderLineRet.ClassRef.FullName != null)
                                        {
                                            string FullName18343 = (string)PurchaseOrderLineRet.ClassRef.FullName.GetValue();
                                        }
                                    }
                                    //Get value of Amount
                                    if (PurchaseOrderLineRet.Amount != null)
                                    {
                                        double Amount18344 = (double)PurchaseOrderLineRet.Amount.GetValue();
                                    }
                                    if (PurchaseOrderLineRet.InventorySiteLocationRef != null)
                                    {
                                        //Get value of ListID
                                        if (PurchaseOrderLineRet.InventorySiteLocationRef.ListID != null)
                                        {
                                            string ListID18345 = (string)PurchaseOrderLineRet.InventorySiteLocationRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (PurchaseOrderLineRet.InventorySiteLocationRef.FullName != null)
                                        {
                                            string FullName18346 = (string)PurchaseOrderLineRet.InventorySiteLocationRef.FullName.GetValue();
                                        }
                                    }
                                    if (PurchaseOrderLineRet.CustomerRef != null)
                                    {
                                        //Get value of ListID
                                        if (PurchaseOrderLineRet.CustomerRef.ListID != null)
                                        {
                                            string ListID18347 = (string)PurchaseOrderLineRet.CustomerRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (PurchaseOrderLineRet.CustomerRef.FullName != null)
                                        {
                                            string FullName18348 = (string)PurchaseOrderLineRet.CustomerRef.FullName.GetValue();
                                        }
                                    }
                                    //Get value of ServiceDate
                                    if (PurchaseOrderLineRet.ServiceDate != null)
                                    {
                                        DateTime ServiceDate18349 = (DateTime)PurchaseOrderLineRet.ServiceDate.GetValue();
                                    }
                                    if (PurchaseOrderLineRet.SalesTaxCodeRef != null)
                                    {
                                        //Get value of ListID
                                        if (PurchaseOrderLineRet.SalesTaxCodeRef.ListID != null)
                                        {
                                            string ListID18350 = (string)PurchaseOrderLineRet.SalesTaxCodeRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (PurchaseOrderLineRet.SalesTaxCodeRef.FullName != null)
                                        {
                                            string FullName18351 = (string)PurchaseOrderLineRet.SalesTaxCodeRef.FullName.GetValue();
                                        }
                                    }
                                    //Get value of ReceivedQuantity
                                    if (PurchaseOrderLineRet.ReceivedQuantity != null)
                                    {
                                        int ReceivedQuantity18352 = (int)PurchaseOrderLineRet.ReceivedQuantity.GetValue();
                                    }
                                    //Get value of UnbilledQuantity
                                    if (PurchaseOrderLineRet.UnbilledQuantity != null)
                                    {
                                        int UnbilledQuantity18353 = (int)PurchaseOrderLineRet.UnbilledQuantity.GetValue();
                                    }
                                    //Get value of IsBilled
                                    if (PurchaseOrderLineRet.IsBilled != null)
                                    {
                                        bool IsBilled18354 = (bool)PurchaseOrderLineRet.IsBilled.GetValue();
                                    }
                                    //Get value of IsManuallyClosed
                                    if (PurchaseOrderLineRet.IsManuallyClosed != null)
                                    {
                                        bool IsManuallyClosed18355 = (bool)PurchaseOrderLineRet.IsManuallyClosed.GetValue();
                                    }
                                    //Get value of Other1
                                    if (PurchaseOrderLineRet.Other1 != null)
                                    {
                                        string Other118356 = (string)PurchaseOrderLineRet.Other1.GetValue();
                                    }
                                    //Get value of Other2
                                    if (PurchaseOrderLineRet.Other2 != null)
                                    {
                                        string Other218357 = (string)PurchaseOrderLineRet.Other2.GetValue();
                                    }
                                    if (PurchaseOrderLineRet.DataExtRetList != null)
                                    {
                                        for (int i18358 = 0; i18358 < PurchaseOrderLineRet.DataExtRetList.Count; i18358++)
                                        {
                                            IDataExtRet DataExtRet = PurchaseOrderLineRet.DataExtRetList.GetAt(i18358);
                                            //Get value of OwnerID
                                            if (DataExtRet.OwnerID != null)
                                            {
                                                string OwnerID18359 = (string)DataExtRet.OwnerID.GetValue();
                                            }
                                            //Get value of DataExtName
                                            string DataExtName18360 = (string)DataExtRet.DataExtName.GetValue();
                                            //Get value of DataExtType
                                            ENDataExtType DataExtType18361 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
                                            //Get value of DataExtValue
                                            string DataExtValue18362 = (string)DataExtRet.DataExtValue.GetValue();
                                        }
                                    }
                                }
                            }
                            if (ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.DataExtRetList != null)
                            {
                                for (int i18363 = 0; i18363 < ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.DataExtRetList.Count; i18363++)
                                {
                                    IDataExtRet DataExtRet = ORPurchaseOrderLineRet18289.PurchaseOrderLineGroupRet.DataExtRetList.GetAt(i18363);
                                    //Get value of OwnerID
                                    if (DataExtRet.OwnerID != null)
                                    {
                                        string OwnerID18364 = (string)DataExtRet.OwnerID.GetValue();
                                    }
                                    //Get value of DataExtName
                                    string DataExtName18365 = (string)DataExtRet.DataExtName.GetValue();
                                    //Get value of DataExtType
                                    ENDataExtType DataExtType18366 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
                                    //Get value of DataExtValue
                                    string DataExtValue18367 = (string)DataExtRet.DataExtValue.GetValue();
                                }
                            }
                        }
                    }
                }
            }

            if (PurchaseOrderRet.DataExtRetList != null)
            {
                for (int i18368 = 0; i18368 < PurchaseOrderRet.DataExtRetList.Count; i18368++)
                {
                    IDataExtRet DataExtRet = PurchaseOrderRet.DataExtRetList.GetAt(i18368);
                    //Get value of OwnerID
                    if (DataExtRet.OwnerID != null)
                    {
                        string OwnerID18369 = (string)DataExtRet.OwnerID.GetValue();
                    }
                    //Get value of DataExtName
                    string DataExtName18370 = (string)DataExtRet.DataExtName.GetValue();
                    //Get value of DataExtType
                    ENDataExtType DataExtType18371 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
                    //Get value of DataExtValue
                    string DataExtValue18372 = (string)DataExtRet.DataExtValue.GetValue();
                }
            }
        }
    }
}