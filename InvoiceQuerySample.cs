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
    public class InvoiceQuerySample
    {
        public void DoInvoiceQuery()
        {
            bool sessionBegun = false;
            bool connectionOpen = false;
            QBSessionManager sessionManager = null;

            try
            {
                //Create the session Manager object
                sessionManager = new QBSessionManager();

                //Create the message set request object to hold our request
                IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", 13, 0);
                requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

                BuildInvoiceQueryRq(requestMsgSet);

                //Connect to QuickBooks and begin a session
                sessionManager.OpenConnection("", "Sample Code from OSR");
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

                WalkInvoiceQueryRs(responseMsgSet);
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
        void BuildInvoiceQueryRq(IMsgSetRequest requestMsgSet)
        {
            IInvoiceQuery InvoiceQueryRq = requestMsgSet.AppendInvoiceQueryRq();
            //Set attributes
            //Set field value for metaData
            InvoiceQueryRq.metaData.SetValue("IQBENmetaDataType");
            //Set field value for iterator
            InvoiceQueryRq.iterator.SetValue("IQBENiteratorType");
            //Set field value for iteratorID
            InvoiceQueryRq.iteratorID.SetValue("IQBUUIDType");
            string ORInvoiceQueryElementType11877 = "TxnIDList";
            if (ORInvoiceQueryElementType11877 == "TxnIDList")
            {
                //Set field value for TxnIDList
                //May create more than one of these if needed
                InvoiceQueryRq.ORInvoiceQuery.TxnIDList.Add("200000-1011023419");
            }
            if (ORInvoiceQueryElementType11877 == "RefNumberList")
            {
                //Set field value for RefNumberList
                //May create more than one of these if needed
                InvoiceQueryRq.ORInvoiceQuery.RefNumberList.Add("ab");
            }
            if (ORInvoiceQueryElementType11877 == "RefNumberCaseSensitiveList")
            {
                //Set field value for RefNumberCaseSensitiveList
                //May create more than one of these if needed
                InvoiceQueryRq.ORInvoiceQuery.RefNumberCaseSensitiveList.Add("ab");
            }
            if (ORInvoiceQueryElementType11877 == "InvoiceFilter")
            {
                //Set field value for MaxReturned
                InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.MaxReturned.SetValue(6);
                string ORDateRangeFilterElementType11878 = "ModifiedDateRangeFilter";
                if (ORDateRangeFilterElementType11878 == "ModifiedDateRangeFilter")
                {
                    //Set field value for FromModifiedDate
                    InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.ORDateRangeFilter.ModifiedDateRangeFilter.FromModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
                    //Set field value for ToModifiedDate
                    InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.ORDateRangeFilter.ModifiedDateRangeFilter.ToModifiedDate.SetValue(DateTime.Parse("12/15/2007 12:15:12"), false);
                }
                if (ORDateRangeFilterElementType11878 == "TxnDateRangeFilter")
                {
                    string ORTxnDateRangeFilterElementType11879 = "TxnDateFilter";
                    if (ORTxnDateRangeFilterElementType11879 == "TxnDateFilter")
                    {
                        //Set field value for FromTxnDate
                        InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.FromTxnDate.SetValue(DateTime.Parse("12/15/2007"));
                        //Set field value for ToTxnDate
                        InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.ToTxnDate.SetValue(DateTime.Parse("12/15/2007"));
                    }
                    if (ORTxnDateRangeFilterElementType11879 == "DateMacro")
                    {
                        //Set field value for DateMacro
                        InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.DateMacro.SetValue(ENDateMacro.dmAll);
                    }
                }
                string OREntityFilterElementType11880 = "ListIDList";
                if (OREntityFilterElementType11880 == "ListIDList")
                {
                    //Set field value for ListIDList
                    //May create more than one of these if needed
                    InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.EntityFilter.OREntityFilter.ListIDList.Add("200000-1011023419");
                }
                if (OREntityFilterElementType11880 == "FullNameList")
                {
                    //Set field value for FullNameList
                    //May create more than one of these if needed
                    InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.EntityFilter.OREntityFilter.FullNameList.Add("ab");
                }
                if (OREntityFilterElementType11880 == "ListIDWithChildren")
                {
                    //Set field value for ListIDWithChildren
                    InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.EntityFilter.OREntityFilter.ListIDWithChildren.SetValue("200000-1011023419");
                }
                if (OREntityFilterElementType11880 == "FullNameWithChildren")
                {
                    //Set field value for FullNameWithChildren
                    InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.EntityFilter.OREntityFilter.FullNameWithChildren.SetValue("ab");
                }
                string ORAccountFilterElementType11881 = "ListIDList";
                if (ORAccountFilterElementType11881 == "ListIDList")
                {
                    //Set field value for ListIDList
                    //May create more than one of these if needed
                    InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.AccountFilter.ORAccountFilter.ListIDList.Add("200000-1011023419");
                }
                if (ORAccountFilterElementType11881 == "FullNameList")
                {
                    //Set field value for FullNameList
                    //May create more than one of these if needed
                    InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.AccountFilter.ORAccountFilter.FullNameList.Add("ab");
                }
                if (ORAccountFilterElementType11881 == "ListIDWithChildren")
                {
                    //Set field value for ListIDWithChildren
                    InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.AccountFilter.ORAccountFilter.ListIDWithChildren.SetValue("200000-1011023419");
                }
                if (ORAccountFilterElementType11881 == "FullNameWithChildren")
                {
                    //Set field value for FullNameWithChildren
                    InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.AccountFilter.ORAccountFilter.FullNameWithChildren.SetValue("ab");
                }
                string ORRefNumberFilterElementType11882 = "RefNumberFilter";
                if (ORRefNumberFilterElementType11882 == "RefNumberFilter")
                {
                    //Set field value for MatchCriterion
                    InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.ORRefNumberFilter.RefNumberFilter.MatchCriterion.SetValue(ENMatchCriterion.mcStartsWith);
                    //Set field value for RefNumber
                    InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.ORRefNumberFilter.RefNumberFilter.RefNumber.SetValue("ab");
                }
                if (ORRefNumberFilterElementType11882 == "RefNumberRangeFilter")
                {
                    //Set field value for FromRefNumber
                    InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.ORRefNumberFilter.RefNumberRangeFilter.FromRefNumber.SetValue("ab");
                    //Set field value for ToRefNumber
                    InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.ORRefNumberFilter.RefNumberRangeFilter.ToRefNumber.SetValue("ab");
                }
                string ORCurrencyFilterElementType11883 = "ListIDList";
                if (ORCurrencyFilterElementType11883 == "ListIDList")
                {
                    //Set field value for ListIDList
                    //May create more than one of these if needed
                    InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.CurrencyFilter.ORCurrencyFilter.ListIDList.Add("200000-1011023419");
                }
                if (ORCurrencyFilterElementType11883 == "FullNameList")
                {
                    //Set field value for FullNameList
                    //May create more than one of these if needed
                    InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.CurrencyFilter.ORCurrencyFilter.FullNameList.Add("ab");
                }
                //Set field value for PaidStatus
                InvoiceQueryRq.ORInvoiceQuery.InvoiceFilter.PaidStatus.SetValue(ENPaidStatus.psAll[DEFAULT]);
            }
            //Set field value for IncludeLineItems
            InvoiceQueryRq.IncludeLineItems.SetValue(true);
            //Set field value for IncludeLinkedTxns
            InvoiceQueryRq.IncludeLinkedTxns.SetValue(true);
            //Set field value for IncludeRetElementList
            //May create more than one of these if needed
            InvoiceQueryRq.IncludeRetElementList.Add("ab");
            //Set field value for OwnerIDList
            //May create more than one of these if needed
            InvoiceQueryRq.OwnerIDList.Add(Guid.NewGuid().ToString());
        }




        void WalkInvoiceQueryRs(IMsgSetResponse responseMsgSet)
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
                        if (responseType == ENResponseType.rtInvoiceQueryRs)
                        {
                            //upcast to more specific type here, this is safe because we checked with response.Type check above
                            IInvoiceRetList InvoiceRet = (IInvoiceRetList)response.Detail;
                            WalkInvoiceRet(InvoiceRet);
                        }
                    }
                }
            }
        }




        void WalkInvoiceRet(IInvoiceRetList InvoiceRet)
        {
            if (InvoiceRet == null) return;
            //Go through all the elements of IInvoiceRetList
            //Get value of TxnID
            string TxnID11884 = (string)InvoiceRet.TxnID.GetValue();
            //Get value of TimeCreated
            DateTime TimeCreated11885 = (DateTime)InvoiceRet.TimeCreated.GetValue();
            //Get value of TimeModified
            DateTime TimeModified11886 = (DateTime)InvoiceRet.TimeModified.GetValue();
            //Get value of EditSequence
            string EditSequence11887 = (string)InvoiceRet.EditSequence.GetValue();
            //Get value of TxnNumber
            if (InvoiceRet.TxnNumber != null)
            {
                int TxnNumber11888 = (int)InvoiceRet.TxnNumber.GetValue();
            }
            //Get value of ListID
            if (InvoiceRet.CustomerRef.ListID != null)
            {
                string ListID11889 = (string)InvoiceRet.CustomerRef.ListID.GetValue();
            }
            //Get value of FullName
            if (InvoiceRet.CustomerRef.FullName != null)
            {
                string FullName11890 = (string)InvoiceRet.CustomerRef.FullName.GetValue();
            }
            if (InvoiceRet.ClassRef != null)
            {
                //Get value of ListID
                if (InvoiceRet.ClassRef.ListID != null)
                {
                    string ListID11891 = (string)InvoiceRet.ClassRef.ListID.GetValue();
                }
                //Get value of FullName
                if (InvoiceRet.ClassRef.FullName != null)
                {
                    string FullName11892 = (string)InvoiceRet.ClassRef.FullName.GetValue();
                }
            }
            if (InvoiceRet.ARAccountRef != null)
            {
                //Get value of ListID
                if (InvoiceRet.ARAccountRef.ListID != null)
                {
                    string ListID11893 = (string)InvoiceRet.ARAccountRef.ListID.GetValue();
                }
                //Get value of FullName
                if (InvoiceRet.ARAccountRef.FullName != null)
                {
                    string FullName11894 = (string)InvoiceRet.ARAccountRef.FullName.GetValue();
                }
            }
            if (InvoiceRet.TemplateRef != null)
            {
                //Get value of ListID
                if (InvoiceRet.TemplateRef.ListID != null)
                {
                    string ListID11895 = (string)InvoiceRet.TemplateRef.ListID.GetValue();
                }
                //Get value of FullName
                if (InvoiceRet.TemplateRef.FullName != null)
                {
                    string FullName11896 = (string)InvoiceRet.TemplateRef.FullName.GetValue();
                }
            }
            //Get value of TxnDate
            DateTime TxnDate11897 = (DateTime)InvoiceRet.TxnDate.GetValue();
            //Get value of RefNumber
            if (InvoiceRet.RefNumber != null)
            {
                string RefNumber11898 = (string)InvoiceRet.RefNumber.GetValue();
            }
            if (InvoiceRet.BillAddress != null)
            {
                //Get value of Addr1
                if (InvoiceRet.BillAddress.Addr1 != null)
                {
                    string Addr111899 = (string)InvoiceRet.BillAddress.Addr1.GetValue();
                }
                //Get value of Addr2
                if (InvoiceRet.BillAddress.Addr2 != null)
                {
                    string Addr211900 = (string)InvoiceRet.BillAddress.Addr2.GetValue();
                }
                //Get value of Addr3
                if (InvoiceRet.BillAddress.Addr3 != null)
                {
                    string Addr311901 = (string)InvoiceRet.BillAddress.Addr3.GetValue();
                }
                //Get value of Addr4
                if (InvoiceRet.BillAddress.Addr4 != null)
                {
                    string Addr411902 = (string)InvoiceRet.BillAddress.Addr4.GetValue();
                }
                //Get value of Addr5
                if (InvoiceRet.BillAddress.Addr5 != null)
                {
                    string Addr511903 = (string)InvoiceRet.BillAddress.Addr5.GetValue();
                }
                //Get value of City
                if (InvoiceRet.BillAddress.City != null)
                {
                    string City11904 = (string)InvoiceRet.BillAddress.City.GetValue();
                }
                //Get value of State
                if (InvoiceRet.BillAddress.State != null)
                {
                    string State11905 = (string)InvoiceRet.BillAddress.State.GetValue();
                }
                //Get value of PostalCode
                if (InvoiceRet.BillAddress.PostalCode != null)
                {
                    string PostalCode11906 = (string)InvoiceRet.BillAddress.PostalCode.GetValue();
                }
                //Get value of Country
                if (InvoiceRet.BillAddress.Country != null)
                {
                    string Country11907 = (string)InvoiceRet.BillAddress.Country.GetValue();
                }
                //Get value of Note
                if (InvoiceRet.BillAddress.Note != null)
                {
                    string Note11908 = (string)InvoiceRet.BillAddress.Note.GetValue();
                }
            }
            if (InvoiceRet.BillAddressBlock != null)
            {
                //Get value of Addr1
                if (InvoiceRet.BillAddressBlock.Addr1 != null)
                {
                    string Addr111909 = (string)InvoiceRet.BillAddressBlock.Addr1.GetValue();
                }
                //Get value of Addr2
                if (InvoiceRet.BillAddressBlock.Addr2 != null)
                {
                    string Addr211910 = (string)InvoiceRet.BillAddressBlock.Addr2.GetValue();
                }
                //Get value of Addr3
                if (InvoiceRet.BillAddressBlock.Addr3 != null)
                {
                    string Addr311911 = (string)InvoiceRet.BillAddressBlock.Addr3.GetValue();
                }
                //Get value of Addr4
                if (InvoiceRet.BillAddressBlock.Addr4 != null)
                {
                    string Addr411912 = (string)InvoiceRet.BillAddressBlock.Addr4.GetValue();
                }
                //Get value of Addr5
                if (InvoiceRet.BillAddressBlock.Addr5 != null)
                {
                    string Addr511913 = (string)InvoiceRet.BillAddressBlock.Addr5.GetValue();
                }
            }
            if (InvoiceRet.ShipAddress != null)
            {
                //Get value of Addr1
                if (InvoiceRet.ShipAddress.Addr1 != null)
                {
                    string Addr111914 = (string)InvoiceRet.ShipAddress.Addr1.GetValue();
                }
                //Get value of Addr2
                if (InvoiceRet.ShipAddress.Addr2 != null)
                {
                    string Addr211915 = (string)InvoiceRet.ShipAddress.Addr2.GetValue();
                }
                //Get value of Addr3
                if (InvoiceRet.ShipAddress.Addr3 != null)
                {
                    string Addr311916 = (string)InvoiceRet.ShipAddress.Addr3.GetValue();
                }
                //Get value of Addr4
                if (InvoiceRet.ShipAddress.Addr4 != null)
                {
                    string Addr411917 = (string)InvoiceRet.ShipAddress.Addr4.GetValue();
                }
                //Get value of Addr5
                if (InvoiceRet.ShipAddress.Addr5 != null)
                {
                    string Addr511918 = (string)InvoiceRet.ShipAddress.Addr5.GetValue();
                }
                //Get value of City
                if (InvoiceRet.ShipAddress.City != null)
                {
                    string City11919 = (string)InvoiceRet.ShipAddress.City.GetValue();
                }
                //Get value of State
                if (InvoiceRet.ShipAddress.State != null)
                {
                    string State11920 = (string)InvoiceRet.ShipAddress.State.GetValue();
                }
                //Get value of PostalCode
                if (InvoiceRet.ShipAddress.PostalCode != null)
                {
                    string PostalCode11921 = (string)InvoiceRet.ShipAddress.PostalCode.GetValue();
                }
                //Get value of Country
                if (InvoiceRet.ShipAddress.Country != null)
                {
                    string Country11922 = (string)InvoiceRet.ShipAddress.Country.GetValue();
                }
                //Get value of Note
                if (InvoiceRet.ShipAddress.Note != null)
                {
                    string Note11923 = (string)InvoiceRet.ShipAddress.Note.GetValue();
                }
            }
            if (InvoiceRet.ShipAddressBlock != null)
            {
                //Get value of Addr1
                if (InvoiceRet.ShipAddressBlock.Addr1 != null)
                {
                    string Addr111924 = (string)InvoiceRet.ShipAddressBlock.Addr1.GetValue();
                }
                //Get value of Addr2
                if (InvoiceRet.ShipAddressBlock.Addr2 != null)
                {
                    string Addr211925 = (string)InvoiceRet.ShipAddressBlock.Addr2.GetValue();
                }
                //Get value of Addr3
                if (InvoiceRet.ShipAddressBlock.Addr3 != null)
                {
                    string Addr311926 = (string)InvoiceRet.ShipAddressBlock.Addr3.GetValue();
                }
                //Get value of Addr4
                if (InvoiceRet.ShipAddressBlock.Addr4 != null)
                {
                    string Addr411927 = (string)InvoiceRet.ShipAddressBlock.Addr4.GetValue();
                }
                //Get value of Addr5
                if (InvoiceRet.ShipAddressBlock.Addr5 != null)
                {
                    string Addr511928 = (string)InvoiceRet.ShipAddressBlock.Addr5.GetValue();
                }
            }
            //Get value of IsPending
            if (InvoiceRet.IsPending != null)
            {
                bool IsPending11929 = (bool)InvoiceRet.IsPending.GetValue();
            }
            //Get value of IsFinanceCharge
            if (InvoiceRet.IsFinanceCharge != null)
            {
                bool IsFinanceCharge11930 = (bool)InvoiceRet.IsFinanceCharge.GetValue();
            }
            //Get value of PONumber
            if (InvoiceRet.PONumber != null)
            {
                string PONumber11931 = (string)InvoiceRet.PONumber.GetValue();
            }
            if (InvoiceRet.TermsRef != null)
            {
                //Get value of ListID
                if (InvoiceRet.TermsRef.ListID != null)
                {
                    string ListID11932 = (string)InvoiceRet.TermsRef.ListID.GetValue();
                }
                //Get value of FullName
                if (InvoiceRet.TermsRef.FullName != null)
                {
                    string FullName11933 = (string)InvoiceRet.TermsRef.FullName.GetValue();
                }
            }
            //Get value of DueDate
            if (InvoiceRet.DueDate != null)
            {
                DateTime DueDate11934 = (DateTime)InvoiceRet.DueDate.GetValue();
            }
            if (InvoiceRet.SalesRepRef != null)
            {
                //Get value of ListID
                if (InvoiceRet.SalesRepRef.ListID != null)
                {
                    string ListID11935 = (string)InvoiceRet.SalesRepRef.ListID.GetValue();
                }
                //Get value of FullName
                if (InvoiceRet.SalesRepRef.FullName != null)
                {
                    string FullName11936 = (string)InvoiceRet.SalesRepRef.FullName.GetValue();
                }
            }
            //Get value of FOB
            if (InvoiceRet.FOB != null)
            {
                string FOB11937 = (string)InvoiceRet.FOB.GetValue();
            }
            //Get value of ShipDate
            if (InvoiceRet.ShipDate != null)
            {
                DateTime ShipDate11938 = (DateTime)InvoiceRet.ShipDate.GetValue();
            }
            if (InvoiceRet.ShipMethodRef != null)
            {
                //Get value of ListID
                if (InvoiceRet.ShipMethodRef.ListID != null)
                {
                    string ListID11939 = (string)InvoiceRet.ShipMethodRef.ListID.GetValue();
                }
                //Get value of FullName
                if (InvoiceRet.ShipMethodRef.FullName != null)
                {
                    string FullName11940 = (string)InvoiceRet.ShipMethodRef.FullName.GetValue();
                }
            }
            //Get value of Subtotal
            if (InvoiceRet.Subtotal != null)
            {
                double Subtotal11941 = (double)InvoiceRet.Subtotal.GetValue();
            }
            if (InvoiceRet.ItemSalesTaxRef != null)
            {
                //Get value of ListID
                if (InvoiceRet.ItemSalesTaxRef.ListID != null)
                {
                    string ListID11942 = (string)InvoiceRet.ItemSalesTaxRef.ListID.GetValue();
                }
                //Get value of FullName
                if (InvoiceRet.ItemSalesTaxRef.FullName != null)
                {
                    string FullName11943 = (string)InvoiceRet.ItemSalesTaxRef.FullName.GetValue();
                }
            }
            //Get value of SalesTaxPercentage
            if (InvoiceRet.SalesTaxPercentage != null)
            {
                double SalesTaxPercentage11944 = (double)InvoiceRet.SalesTaxPercentage.GetValue();
            }
            //Get value of SalesTaxTotal
            if (InvoiceRet.SalesTaxTotal != null)
            {
                double SalesTaxTotal11945 = (double)InvoiceRet.SalesTaxTotal.GetValue();
            }
            //Get value of AppliedAmount
            if (InvoiceRet.AppliedAmount != null)
            {
                double AppliedAmount11946 = (double)InvoiceRet.AppliedAmount.GetValue();
            }
            //Get value of BalanceRemaining
            if (InvoiceRet.BalanceRemaining != null)
            {
                double BalanceRemaining11947 = (double)InvoiceRet.BalanceRemaining.GetValue();
            }
            if (InvoiceRet.CurrencyRef != null)
            {
                //Get value of ListID
                if (InvoiceRet.CurrencyRef.ListID != null)
                {
                    string ListID11948 = (string)InvoiceRet.CurrencyRef.ListID.GetValue();
                }
                //Get value of FullName
                if (InvoiceRet.CurrencyRef.FullName != null)
                {
                    string FullName11949 = (string)InvoiceRet.CurrencyRef.FullName.GetValue();
                }
            }
            //Get value of ExchangeRate
            if (InvoiceRet.ExchangeRate != null)
            {
                IQBFloatType ExchangeRate11950 = (IQBFloatType)InvoiceRet.ExchangeRate.GetValue();
            }
            //Get value of BalanceRemainingInHomeCurrency
            if (InvoiceRet.BalanceRemainingInHomeCurrency != null)
            {
                double BalanceRemainingInHomeCurrency11951 = (double)InvoiceRet.BalanceRemainingInHomeCurrency.GetValue();
            }
            //Get value of Memo
            if (InvoiceRet.Memo != null)
            {
                string Memo11952 = (string)InvoiceRet.Memo.GetValue();
            }
            //Get value of IsPaid
            if (InvoiceRet.IsPaid != null)
            {
                bool IsPaid11953 = (bool)InvoiceRet.IsPaid.GetValue();
            }
            if (InvoiceRet.CustomerMsgRef != null)
            {
                //Get value of ListID
                if (InvoiceRet.CustomerMsgRef.ListID != null)
                {
                    string ListID11954 = (string)InvoiceRet.CustomerMsgRef.ListID.GetValue();
                }
                //Get value of FullName
                if (InvoiceRet.CustomerMsgRef.FullName != null)
                {
                    string FullName11955 = (string)InvoiceRet.CustomerMsgRef.FullName.GetValue();
                }
            }
            //Get value of IsToBePrinted
            if (InvoiceRet.IsToBePrinted != null)
            {
                bool IsToBePrinted11956 = (bool)InvoiceRet.IsToBePrinted.GetValue();
            }
            //Get value of IsToBeEmailed
            if (InvoiceRet.IsToBeEmailed != null)
            {
                bool IsToBeEmailed11957 = (bool)InvoiceRet.IsToBeEmailed.GetValue();
            }
            //Get value of IsTaxIncluded
            if (InvoiceRet.IsTaxIncluded != null)
            {
                bool IsTaxIncluded11958 = (bool)InvoiceRet.IsTaxIncluded.GetValue();
            }
            if (InvoiceRet.CustomerSalesTaxCodeRef != null)
            {
                //Get value of ListID
                if (InvoiceRet.CustomerSalesTaxCodeRef.ListID != null)
                {
                    string ListID11959 = (string)InvoiceRet.CustomerSalesTaxCodeRef.ListID.GetValue();
                }
                //Get value of FullName
                if (InvoiceRet.CustomerSalesTaxCodeRef.FullName != null)
                {
                    string FullName11960 = (string)InvoiceRet.CustomerSalesTaxCodeRef.FullName.GetValue();
                }
            }
            //Get value of SuggestedDiscountAmount
            if (InvoiceRet.SuggestedDiscountAmount != null)
            {
                double SuggestedDiscountAmount11961 = (double)InvoiceRet.SuggestedDiscountAmount.GetValue();
            }
            //Get value of SuggestedDiscountDate
            if (InvoiceRet.SuggestedDiscountDate != null)
            {
                DateTime SuggestedDiscountDate11962 = (DateTime)InvoiceRet.SuggestedDiscountDate.GetValue();
            }
            //Get value of Other
            if (InvoiceRet.Other != null)
            {
                string Other11963 = (string)InvoiceRet.Other.GetValue();
            }
            //Get value of ExternalGUID
            if (InvoiceRet.ExternalGUID != null)
            {
                string ExternalGUID11964 = (string)InvoiceRet.ExternalGUID.GetValue();
            }
            if (InvoiceRet.LinkedTxnList != null)
            {
                for (int i11965 = 0; i11965 < InvoiceRet.LinkedTxnList.Count; i11965++)
                {
                    ILinkedTxn LinkedTxn = InvoiceRet.LinkedTxnList.GetAt(i11965);
                    //Get value of TxnID
                    string TxnID11966 = (string)LinkedTxn.TxnID.GetValue();
                    //Get value of TxnType
                    ENTxnType TxnType11967 = (ENTxnType)LinkedTxn.TxnType.GetValue();
                    //Get value of TxnDate
                    DateTime TxnDate11968 = (DateTime)LinkedTxn.TxnDate.GetValue();
                    //Get value of RefNumber
                    if (LinkedTxn.RefNumber != null)
                    {
                        string RefNumber11969 = (string)LinkedTxn.RefNumber.GetValue();
                    }
                    //Get value of LinkType
                    if (LinkedTxn.LinkType != null)
                    {
                        ENLinkType LinkType11970 = (ENLinkType)LinkedTxn.LinkType.GetValue();
                    }
                    //Get value of Amount
                    double Amount11971 = (double)LinkedTxn.Amount.GetValue();
                }
            }
            if (InvoiceRet.ORInvoiceLineRetList != null)
            {
                for (int i11972 = 0; i11972 < InvoiceRet.ORInvoiceLineRetList.Count; i11972++)
                {
                    IORInvoiceLineRet ORInvoiceLineRet11973 = InvoiceRet.ORInvoiceLineRetList.GetAt(i11972);
                    if (ORInvoiceLineRet11973.InvoiceLineRet != null)
                    {
                        if (ORInvoiceLineRet11973.InvoiceLineRet != null)
                        {
                            //Get value of TxnLineID
                            string TxnLineID11974 = (string)ORInvoiceLineRet11973.InvoiceLineRet.TxnLineID.GetValue();
                            if (ORInvoiceLineRet11973.InvoiceLineRet.ItemRef != null)
                            {
                                //Get value of ListID
                                if (ORInvoiceLineRet11973.InvoiceLineRet.ItemRef.ListID != null)
                                {
                                    string ListID11975 = (string)ORInvoiceLineRet11973.InvoiceLineRet.ItemRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORInvoiceLineRet11973.InvoiceLineRet.ItemRef.FullName != null)
                                {
                                    string FullName11976 = (string)ORInvoiceLineRet11973.InvoiceLineRet.ItemRef.FullName.GetValue();
                                }
                            }
                            //Get value of Desc
                            if (ORInvoiceLineRet11973.InvoiceLineRet.Desc != null)
                            {
                                string Desc11977 = (string)ORInvoiceLineRet11973.InvoiceLineRet.Desc.GetValue();
                            }
                            //Get value of Quantity
                            if (ORInvoiceLineRet11973.InvoiceLineRet.Quantity != null)
                            {
                                int Quantity11978 = (int)ORInvoiceLineRet11973.InvoiceLineRet.Quantity.GetValue();
                            }
                            //Get value of UnitOfMeasure
                            if (ORInvoiceLineRet11973.InvoiceLineRet.UnitOfMeasure != null)
                            {
                                string UnitOfMeasure11979 = (string)ORInvoiceLineRet11973.InvoiceLineRet.UnitOfMeasure.GetValue();
                            }
                            if (ORInvoiceLineRet11973.InvoiceLineRet.OverrideUOMSetRef != null)
                            {
                                //Get value of ListID
                                if (ORInvoiceLineRet11973.InvoiceLineRet.OverrideUOMSetRef.ListID != null)
                                {
                                    string ListID11980 = (string)ORInvoiceLineRet11973.InvoiceLineRet.OverrideUOMSetRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORInvoiceLineRet11973.InvoiceLineRet.OverrideUOMSetRef.FullName != null)
                                {
                                    string FullName11981 = (string)ORInvoiceLineRet11973.InvoiceLineRet.OverrideUOMSetRef.FullName.GetValue();
                                }
                            }
                            if (ORInvoiceLineRet11973.InvoiceLineRet.ORRate != null)
                            {
                                if (ORInvoiceLineRet11973.InvoiceLineRet.ORRate.Rate != null)
                                {
                                    //Get value of Rate
                                    if (ORInvoiceLineRet11973.InvoiceLineRet.ORRate.Rate != null)
                                    {
                                        double Rate11983 = (double)ORInvoiceLineRet11973.InvoiceLineRet.ORRate.Rate.GetValue();
                                    }
                                }
                                if (ORInvoiceLineRet11973.InvoiceLineRet.ORRate.RatePercent != null)
                                {
                                    //Get value of RatePercent
                                    if (ORInvoiceLineRet11973.InvoiceLineRet.ORRate.RatePercent != null)
                                    {
                                        double RatePercent11984 = (double)ORInvoiceLineRet11973.InvoiceLineRet.ORRate.RatePercent.GetValue();
                                    }
                                }
                            }
                            if (ORInvoiceLineRet11973.InvoiceLineRet.ClassRef != null)
                            {
                                //Get value of ListID
                                if (ORInvoiceLineRet11973.InvoiceLineRet.ClassRef.ListID != null)
                                {
                                    string ListID11985 = (string)ORInvoiceLineRet11973.InvoiceLineRet.ClassRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORInvoiceLineRet11973.InvoiceLineRet.ClassRef.FullName != null)
                                {
                                    string FullName11986 = (string)ORInvoiceLineRet11973.InvoiceLineRet.ClassRef.FullName.GetValue();
                                }
                            }
                            //Get value of Amount
                            if (ORInvoiceLineRet11973.InvoiceLineRet.Amount != null)
                            {
                                double Amount11987 = (double)ORInvoiceLineRet11973.InvoiceLineRet.Amount.GetValue();
                            }
                            if (ORInvoiceLineRet11973.InvoiceLineRet.InventorySiteRef != null)
                            {
                                //Get value of ListID
                                if (ORInvoiceLineRet11973.InvoiceLineRet.InventorySiteRef.ListID != null)
                                {
                                    string ListID11988 = (string)ORInvoiceLineRet11973.InvoiceLineRet.InventorySiteRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORInvoiceLineRet11973.InvoiceLineRet.InventorySiteRef.FullName != null)
                                {
                                    string FullName11989 = (string)ORInvoiceLineRet11973.InvoiceLineRet.InventorySiteRef.FullName.GetValue();
                                }
                            }
                            if (ORInvoiceLineRet11973.InvoiceLineRet.InventorySiteLocationRef != null)
                            {
                                //Get value of ListID
                                if (ORInvoiceLineRet11973.InvoiceLineRet.InventorySiteLocationRef.ListID != null)
                                {
                                    string ListID11990 = (string)ORInvoiceLineRet11973.InvoiceLineRet.InventorySiteLocationRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORInvoiceLineRet11973.InvoiceLineRet.InventorySiteLocationRef.FullName != null)
                                {
                                    string FullName11991 = (string)ORInvoiceLineRet11973.InvoiceLineRet.InventorySiteLocationRef.FullName.GetValue();
                                }
                            }
                            if (ORInvoiceLineRet11973.InvoiceLineRet.ORSerialLotNumber != null)
                            {
                                if (ORInvoiceLineRet11973.InvoiceLineRet.ORSerialLotNumber.SerialNumber != null)
                                {
                                    //Get value of SerialNumber
                                    if (ORInvoiceLineRet11973.InvoiceLineRet.ORSerialLotNumber.SerialNumber != null)
                                    {
                                        string SerialNumber11993 = (string)ORInvoiceLineRet11973.InvoiceLineRet.ORSerialLotNumber.SerialNumber.GetValue();
                                    }
                                }
                                if (ORInvoiceLineRet11973.InvoiceLineRet.ORSerialLotNumber.LotNumber != null)
                                {
                                    //Get value of LotNumber
                                    if (ORInvoiceLineRet11973.InvoiceLineRet.ORSerialLotNumber.LotNumber != null)
                                    {
                                        string LotNumber11994 = (string)ORInvoiceLineRet11973.InvoiceLineRet.ORSerialLotNumber.LotNumber.GetValue();
                                    }
                                }
                            }
                            //Get value of ServiceDate
                            if (ORInvoiceLineRet11973.InvoiceLineRet.ServiceDate != null)
                            {
                                DateTime ServiceDate11995 = (DateTime)ORInvoiceLineRet11973.InvoiceLineRet.ServiceDate.GetValue();
                            }
                            if (ORInvoiceLineRet11973.InvoiceLineRet.SalesTaxCodeRef != null)
                            {
                                //Get value of ListID
                                if (ORInvoiceLineRet11973.InvoiceLineRet.SalesTaxCodeRef.ListID != null)
                                {
                                    string ListID11996 = (string)ORInvoiceLineRet11973.InvoiceLineRet.SalesTaxCodeRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORInvoiceLineRet11973.InvoiceLineRet.SalesTaxCodeRef.FullName != null)
                                {
                                    string FullName11997 = (string)ORInvoiceLineRet11973.InvoiceLineRet.SalesTaxCodeRef.FullName.GetValue();
                                }
                            }
                            //Get value of Other1
                            if (ORInvoiceLineRet11973.InvoiceLineRet.Other1 != null)
                            {
                                string Other111998 = (string)ORInvoiceLineRet11973.InvoiceLineRet.Other1.GetValue();
                            }
                            //Get value of Other2
                            if (ORInvoiceLineRet11973.InvoiceLineRet.Other2 != null)
                            {
                                string Other211999 = (string)ORInvoiceLineRet11973.InvoiceLineRet.Other2.GetValue();
                            }
                            if (ORInvoiceLineRet11973.InvoiceLineRet.DataExtRetList != null)
                            {
                                for (int i12000 = 0; i12000 < ORInvoiceLineRet11973.InvoiceLineRet.DataExtRetList.Count; i12000++)
                                {
                                    IDataExtRet DataExtRet = ORInvoiceLineRet11973.InvoiceLineRet.DataExtRetList.GetAt(i12000);
                                    //Get value of OwnerID
                                    if (DataExtRet.OwnerID != null)
                                    {
                                        string OwnerID12001 = (string)DataExtRet.OwnerID.GetValue();
                                    }
                                    //Get value of DataExtName
                                    string DataExtName12002 = (string)DataExtRet.DataExtName.GetValue();
                                    //Get value of DataExtType
                                    ENDataExtType DataExtType12003 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
                                    //Get value of DataExtValue
                                    string DataExtValue12004 = (string)DataExtRet.DataExtValue.GetValue();
                                }
                            }
                        }
                    }
                    if (ORInvoiceLineRet11973.InvoiceLineGroupRet != null)
                    {
                        if (ORInvoiceLineRet11973.InvoiceLineGroupRet != null)
                        {
                            //Get value of TxnLineID
                            string TxnLineID12005 = (string)ORInvoiceLineRet11973.InvoiceLineGroupRet.TxnLineID.GetValue();
                            //Get value of ListID
                            if (ORInvoiceLineRet11973.InvoiceLineGroupRet.ItemGroupRef.ListID != null)
                            {
                                string ListID12006 = (string)ORInvoiceLineRet11973.InvoiceLineGroupRet.ItemGroupRef.ListID.GetValue();
                            }
                            //Get value of FullName
                            if (ORInvoiceLineRet11973.InvoiceLineGroupRet.ItemGroupRef.FullName != null)
                            {
                                string FullName12007 = (string)ORInvoiceLineRet11973.InvoiceLineGroupRet.ItemGroupRef.FullName.GetValue();
                            }
                            //Get value of Desc
                            if (ORInvoiceLineRet11973.InvoiceLineGroupRet.Desc != null)
                            {
                                string Desc12008 = (string)ORInvoiceLineRet11973.InvoiceLineGroupRet.Desc.GetValue();
                            }
                            //Get value of Quantity
                            if (ORInvoiceLineRet11973.InvoiceLineGroupRet.Quantity != null)
                            {
                                int Quantity12009 = (int)ORInvoiceLineRet11973.InvoiceLineGroupRet.Quantity.GetValue();
                            }
                            //Get value of UnitOfMeasure
                            if (ORInvoiceLineRet11973.InvoiceLineGroupRet.UnitOfMeasure != null)
                            {
                                string UnitOfMeasure12010 = (string)ORInvoiceLineRet11973.InvoiceLineGroupRet.UnitOfMeasure.GetValue();
                            }
                            if (ORInvoiceLineRet11973.InvoiceLineGroupRet.OverrideUOMSetRef != null)
                            {
                                //Get value of ListID
                                if (ORInvoiceLineRet11973.InvoiceLineGroupRet.OverrideUOMSetRef.ListID != null)
                                {
                                    string ListID12011 = (string)ORInvoiceLineRet11973.InvoiceLineGroupRet.OverrideUOMSetRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORInvoiceLineRet11973.InvoiceLineGroupRet.OverrideUOMSetRef.FullName != null)
                                {
                                    string FullName12012 = (string)ORInvoiceLineRet11973.InvoiceLineGroupRet.OverrideUOMSetRef.FullName.GetValue();
                                }
                            }
                            //Get value of IsPrintItemsInGroup
                            bool IsPrintItemsInGroup12013 = (bool)ORInvoiceLineRet11973.InvoiceLineGroupRet.IsPrintItemsInGroup.GetValue();
                            //Get value of TotalAmount
                            double TotalAmount12014 = (double)ORInvoiceLineRet11973.InvoiceLineGroupRet.TotalAmount.GetValue();
                            if (ORInvoiceLineRet11973.InvoiceLineGroupRet.InvoiceLineRetList != null)
                            {
                                for (int i12015 = 0; i12015 < ORInvoiceLineRet11973.InvoiceLineGroupRet.InvoiceLineRetList.Count; i12015++)
                                {
                                    IInvoiceLineRet InvoiceLineRet = ORInvoiceLineRet11973.InvoiceLineGroupRet.InvoiceLineRetList.GetAt(i12015);
                                    //Get value of TxnLineID
                                    string TxnLineID12016 = (string)InvoiceLineRet.TxnLineID.GetValue();
                                    if (InvoiceLineRet.ItemRef != null)
                                    {
                                        //Get value of ListID
                                        if (InvoiceLineRet.ItemRef.ListID != null)
                                        {
                                            string ListID12017 = (string)InvoiceLineRet.ItemRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (InvoiceLineRet.ItemRef.FullName != null)
                                        {
                                            string FullName12018 = (string)InvoiceLineRet.ItemRef.FullName.GetValue();
                                        }
                                    }
                                    //Get value of Desc
                                    if (InvoiceLineRet.Desc != null)
                                    {
                                        string Desc12019 = (string)InvoiceLineRet.Desc.GetValue();
                                    }
                                    //Get value of Quantity
                                    if (InvoiceLineRet.Quantity != null)
                                    {
                                        int Quantity12020 = (int)InvoiceLineRet.Quantity.GetValue();
                                    }
                                    //Get value of UnitOfMeasure
                                    if (InvoiceLineRet.UnitOfMeasure != null)
                                    {
                                        string UnitOfMeasure12021 = (string)InvoiceLineRet.UnitOfMeasure.GetValue();
                                    }
                                    if (InvoiceLineRet.OverrideUOMSetRef != null)
                                    {
                                        //Get value of ListID
                                        if (InvoiceLineRet.OverrideUOMSetRef.ListID != null)
                                        {
                                            string ListID12022 = (string)InvoiceLineRet.OverrideUOMSetRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (InvoiceLineRet.OverrideUOMSetRef.FullName != null)
                                        {
                                            string FullName12023 = (string)InvoiceLineRet.OverrideUOMSetRef.FullName.GetValue();
                                        }
                                    }
                                    if (InvoiceLineRet.ORRate != null)
                                    {
                                        if (InvoiceLineRet.ORRate.Rate != null)
                                        {
                                            //Get value of Rate
                                            if (InvoiceLineRet.ORRate.Rate != null)
                                            {
                                                double Rate12025 = (double)InvoiceLineRet.ORRate.Rate.GetValue();
                                            }
                                        }
                                        if (InvoiceLineRet.ORRate.RatePercent != null)
                                        {
                                            //Get value of RatePercent
                                            if (InvoiceLineRet.ORRate.RatePercent != null)
                                            {
                                                double RatePercent12026 = (double)InvoiceLineRet.ORRate.RatePercent.GetValue();
                                            }
                                        }
                                    }
                                    if (InvoiceLineRet.ClassRef != null)
                                    {
                                        //Get value of ListID
                                        if (InvoiceLineRet.ClassRef.ListID != null)
                                        {
                                            string ListID12027 = (string)InvoiceLineRet.ClassRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (InvoiceLineRet.ClassRef.FullName != null)
                                        {
                                            string FullName12028 = (string)InvoiceLineRet.ClassRef.FullName.GetValue();
                                        }
                                    }
                                    //Get value of Amount
                                    if (InvoiceLineRet.Amount != null)
                                    {
                                        double Amount12029 = (double)InvoiceLineRet.Amount.GetValue();
                                    }
                                    if (InvoiceLineRet.InventorySiteRef != null)
                                    {
                                        //Get value of ListID
                                        if (InvoiceLineRet.InventorySiteRef.ListID != null)
                                        {
                                            string ListID12030 = (string)InvoiceLineRet.InventorySiteRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (InvoiceLineRet.InventorySiteRef.FullName != null)
                                        {
                                            string FullName12031 = (string)InvoiceLineRet.InventorySiteRef.FullName.GetValue();
                                        }
                                    }
                                    if (InvoiceLineRet.InventorySiteLocationRef != null)
                                    {
                                        //Get value of ListID
                                        if (InvoiceLineRet.InventorySiteLocationRef.ListID != null)
                                        {
                                            string ListID12032 = (string)InvoiceLineRet.InventorySiteLocationRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (InvoiceLineRet.InventorySiteLocationRef.FullName != null)
                                        {
                                            string FullName12033 = (string)InvoiceLineRet.InventorySiteLocationRef.FullName.GetValue();
                                        }
                                    }
                                    if (InvoiceLineRet.ORSerialLotNumber != null)
                                    {
                                        if (InvoiceLineRet.ORSerialLotNumber.SerialNumber != null)
                                        {
                                            //Get value of SerialNumber
                                            if (InvoiceLineRet.ORSerialLotNumber.SerialNumber != null)
                                            {
                                                string SerialNumber12035 = (string)InvoiceLineRet.ORSerialLotNumber.SerialNumber.GetValue();
                                            }
                                        }
                                        if (InvoiceLineRet.ORSerialLotNumber.LotNumber != null)
                                        {
                                            //Get value of LotNumber
                                            if (InvoiceLineRet.ORSerialLotNumber.LotNumber != null)
                                            {
                                                string LotNumber12036 = (string)InvoiceLineRet.ORSerialLotNumber.LotNumber.GetValue();
                                            }
                                        }
                                    }
                                    //Get value of ServiceDate
                                    if (InvoiceLineRet.ServiceDate != null)
                                    {
                                        DateTime ServiceDate12037 = (DateTime)InvoiceLineRet.ServiceDate.GetValue();
                                    }
                                    if (InvoiceLineRet.SalesTaxCodeRef != null)
                                    {
                                        //Get value of ListID
                                        if (InvoiceLineRet.SalesTaxCodeRef.ListID != null)
                                        {
                                            string ListID12038 = (string)InvoiceLineRet.SalesTaxCodeRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (InvoiceLineRet.SalesTaxCodeRef.FullName != null)
                                        {
                                            string FullName12039 = (string)InvoiceLineRet.SalesTaxCodeRef.FullName.GetValue();
                                        }
                                    }
                                    //Get value of Other1
                                    if (InvoiceLineRet.Other1 != null)
                                    {
                                        string Other112040 = (string)InvoiceLineRet.Other1.GetValue();
                                    }
                                    //Get value of Other2
                                    if (InvoiceLineRet.Other2 != null)
                                    {
                                        string Other212041 = (string)InvoiceLineRet.Other2.GetValue();
                                    }
                                    if (InvoiceLineRet.DataExtRetList != null)
                                    {
                                        for (int i12042 = 0; i12042 < InvoiceLineRet.DataExtRetList.Count; i12042++)
                                        {
                                            IDataExtRet DataExtRet = InvoiceLineRet.DataExtRetList.GetAt(i12042);
                                            //Get value of OwnerID
                                            if (DataExtRet.OwnerID != null)
                                            {
                                                string OwnerID12043 = (string)DataExtRet.OwnerID.GetValue();
                                            }
                                            //Get value of DataExtName
                                            string DataExtName12044 = (string)DataExtRet.DataExtName.GetValue();
                                            //Get value of DataExtType
                                            ENDataExtType DataExtType12045 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
                                            //Get value of DataExtValue
                                            string DataExtValue12046 = (string)DataExtRet.DataExtValue.GetValue();
                                        }
                                    }
                                }
                            }
                            if (ORInvoiceLineRet11973.InvoiceLineGroupRet.DataExtRetList != null)
                            {
                                for (int i12047 = 0; i12047 < ORInvoiceLineRet11973.InvoiceLineGroupRet.DataExtRetList.Count; i12047++)
                                {
                                    IDataExtRet DataExtRet = ORInvoiceLineRet11973.InvoiceLineGroupRet.DataExtRetList.GetAt(i12047);
                                    //Get value of OwnerID
                                    if (DataExtRet.OwnerID != null)
                                    {
                                        string OwnerID12048 = (string)DataExtRet.OwnerID.GetValue();
                                    }
                                    //Get value of DataExtName
                                    string DataExtName12049 = (string)DataExtRet.DataExtName.GetValue();
                                    //Get value of DataExtType
                                    ENDataExtType DataExtType12050 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
                                    //Get value of DataExtValue
                                    string DataExtValue12051 = (string)DataExtRet.DataExtValue.GetValue();
                                }
                            }
                        }
                    }
                }
            }
            if (InvoiceRet.DataExtRetList != null)
            {
                for (int i12052 = 0; i12052 < InvoiceRet.DataExtRetList.Count; i12052++)
                {
                    IDataExtRet DataExtRet = InvoiceRet.DataExtRetList.GetAt(i12052);
                    //Get value of OwnerID
                    if (DataExtRet.OwnerID != null)
                    {
                        string OwnerID12053 = (string)DataExtRet.OwnerID.GetValue();
                    }
                    //Get value of DataExtName
                    string DataExtName12054 = (string)DataExtRet.DataExtName.GetValue();
                    //Get value of DataExtType
                    ENDataExtType DataExtType12055 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
                    //Get value of DataExtValue
                    string DataExtValue12056 = (string)DataExtRet.DataExtValue.GetValue();
                }
            }
        }




    }
}