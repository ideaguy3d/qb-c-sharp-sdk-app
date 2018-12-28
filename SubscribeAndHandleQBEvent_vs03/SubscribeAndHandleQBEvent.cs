using System;
using System.Net; 
using System.Collections; 
using System.ComponentModel; 
using System.Data; 
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Interop.QBFC13;
using Interop.QBXMLRP2;
using Microsoft.Win32;

namespace SubscribeAndHandleQBEvent
{
    [Flags]
    enum COINIT : uint
    {
        /// Initializes the thread for multi-threaded object concurrency.
        COINIT_MULTITHREADED = 0x0,

        /// Initializes the thread for apartment-threaded object concurrency. 
        COINIT_APARTMENTTHREADED = 0x2,

        /// Disables DDE for Ole1 support.
        COINIT_DISABLE_OLE1DDE = 0x4,

        /// Trades memory for speed.
        COINIT_SPEED_OVER_MEMORY = 0x8
    }

    // class context 
    [Flags]
    enum CLSCTX : uint
    {
        CLSCTX_INPROC_SERVER = 0x1,
        CLSCTX_INPROC_HANDLER = 0x2,
        CLSCTX_LOCAL_SERVER = 0x4,
        CLSCTX_INPROC_SERVER16 = 0x8,
        CLSCTX_REMOTE_SERVER = 0x10,
        CLSCTX_INPROC_HANDLER16 = 0x20,
        CLSCTX_RESERVED1 = 0x40,
        CLSCTX_RESERVED2 = 0x80,
        CLSCTX_RESERVED3 = 0x100,
        CLSCTX_RESERVED4 = 0x200,
        CLSCTX_NO_CODE_DOWNLOAD = 0x400,
        CLSCTX_RESERVED5 = 0x800,
        CLSCTX_NO_CUSTOM_MARSHAL = 0x1000,
        CLSCTX_ENABLE_CODE_DOWNLOAD = 0x2000,
        CLSCTX_NO_FAILURE_LOG = 0x4000,
        CLSCTX_DISABLE_AAA = 0x8000,
        CLSCTX_ENABLE_AAA = 0x10000,
        CLSCTX_FROM_DEFAULT_CONTEXT = 0x20000,
        CLSCTX_INPROC = CLSCTX_INPROC_SERVER | CLSCTX_INPROC_HANDLER,
        CLSCTX_SERVER = CLSCTX_INPROC_SERVER | CLSCTX_LOCAL_SERVER | CLSCTX_REMOTE_SERVER,
        CLSCTX_ALL = CLSCTX_SERVER | CLSCTX_INPROC_HANDLER
    }

    [Flags]
    enum REGCLS : uint
    {
        REGCLS_SINGLEUSE = 0,
        REGCLS_MULTIPLEUSE = 1,
        REGCLS_MULTI_SEPARATE = 2,
        REGCLS_SUSPENDED = 4,
        REGCLS_SURROGATE = 8
    }

    // We import the POINT structure because it is referenced
    // by the MSG structure.
    [ComVisible(false)]
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static implicit operator Point(POINT p)
        {
            return new Point(p.X, p.Y);
        }

        public static implicit operator POINT(Point p)
        {
            return new POINT(p.X, p.Y);
        }
    }

    // We import the MSG structure because it is referenced 
    // by the GetMessage(), TranslateMessage() and DispatchMessage()
    // Win32 APIs.
    [ComVisible(false)]
    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr hwnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public POINT pt;
    }

    // Note that SubscribeAndHandleQBEvent is NOT declared as public.
    // This is so that it will not be exposed to COM when we call regasm
    // or tlbexp.
    class SubscribeAndHandleQBEvent
    {
        enum QBSubscriptionType
        {
            Data,
            UI,
            UIExtension
        };

        enum enORTxnQueryElementType
        {
            TxnIDList,
            RefNumberList,
            RefNumberCaseSensitiveList,
            TxnFilter
        };

        static string strAppName = "Redstone Print and Mail";

        // CoInitializeEx() can be used to set the apartment model
        // of individual threads.
        [DllImport("ole32.dll")]
        static extern int CoInitializeEx(IntPtr pvReserved, uint dwCoInit);

        // CoUninitialize() is used to uninitialize a COM thread.
        // I do not know what a COM thread is
        [DllImport("ole32.dll")]
        static extern void CoUninitialize();

        // PostThreadMessage() allows us to post a Windows Message to
        // a specific thread (identified by its thread id).
        // We will need this API to post a WM_QUIT message to the main 
        // thread in order to terminate this application.
        [DllImport("user32.dll")]
        static extern bool PostThreadMessage(uint idThread, uint Msg, UIntPtr wParam, IntPtr lParam);

        // GetCurrentThreadId() allows us to obtain the thread id of the
        // calling thread. This allows us to post the WM_QUIT message to
        // the main thread.
        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();

        // We will be manually performing a Message Loop within the main thread
        // of this application. Hence we will need to import GetMessage(), 
        // TranslateMessage() and DispatchMessage().
        [DllImport("user32.dll")]
        static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

        // Define two common GUID objects for public usage.
        public static Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");
        public static Guid IID_IDispatch = new Guid("{00020400-0000-0000-C000-000000000046}");

        protected static uint m_uiMainThreadId; // Stores the main thread's thread id.
        protected static int m_iObjsInUse; // Keeps a count on the total number of objects alive.
        protected static int m_iServerLocks; // Keeps a lock count on this application.

        // This property returns the main thread's id.
        public static uint MainThreadId
        {
            get { return m_uiMainThreadId; }
        }

        // This method performs a thread-safe incrementation of the objects count.
        public static int InterlockedIncrementObjectsCount()
        {
            Console.WriteLine("InterlockedIncrementObjectsCount()");
            // Increment the global count of objects.
            return Interlocked.Increment(ref m_iObjsInUse);
        }

        // This method performs a thread-safe decrementation the objects count.
        public static int InterlockedDecrementObjectsCount()
        {
            Console.WriteLine("InterlockedDecrementObjectsCount()");
            // Decrement the global count of objects.
            return Interlocked.Decrement(ref m_iObjsInUse);
        }

        // Returns the total number of objects alive currently.
        public static int ObjectsCount
        {
            get
            {
                lock (typeof(SubscribeAndHandleQBEvent))
                {
                    return m_iObjsInUse;
                }
            }
        }

        // This method performs a thread-safe incrementation of the 
        // server lock count.
        public static int InterlockedIncrementServerLockCount()
        {
            Console.WriteLine("InterlockedIncrementServerLockCount()");
            // Increment the global lock count of this server.
            return Interlocked.Increment(ref m_iServerLocks);
        }

        // This method performs a thread-safe decrementation the 
        // server lock count.
        public static int InterlockedDecrementServerLockCount()
        {
            Console.WriteLine("InterlockedDecrementServerLockCount()");
            // Decrement the global lock count of this server.
            return Interlocked.Decrement(ref m_iServerLocks);
        }

        // Returns the current server lock count.
        public static int ServerLockCount
        {
            get
            {
                lock (typeof(SubscribeAndHandleQBEvent))
                {
                    return m_iServerLocks;
                }
            }
        }

        // AttemptToTerminateServer() will check to see if 
        // the objects count and the server lock count has
        // both dropped to zero.
        // If so, we post a WM_QUIT message to the main thread's
        // message loop. This will cause the message loop to
        // exit and hence the termination of this application.
        public static void AttemptToTerminateServer()
        {
            lock (typeof(SubscribeAndHandleQBEvent))
            {
                Console.WriteLine("AttemptToTerminateServer()");

                // Get the most up-to-date values of these critical data.
                int iObjsInUse = ObjectsCount;
                int iServerLocks = ServerLockCount;

                // Print out these info for debug purposes.
                StringBuilder sb = new StringBuilder("");
                sb.AppendFormat("m_iObjsInUse : {0}. m_iServerLocks : {1}", iObjsInUse, iServerLocks);
                Console.WriteLine(sb.ToString());

                if ((iObjsInUse > 0) || (iServerLocks > 0))
                {
                    Console.WriteLine("There are still referenced objects or the server lock count is non-zero.");
                }
                else
                {
                    UIntPtr wParam = new UIntPtr(0);
                    IntPtr lParam = new IntPtr(0);
                    Console.WriteLine("PostThreadMessage(WM_QUIT)");
                    PostThreadMessage(MainThreadId, 0x0012, wParam, lParam);
                }
            }
        }

        // ProcessArguments() will process the command-line arguments
        // of this application. 
        // If the return value is true, we carry
        // on and start this application.
        // If the return value is false, we terminate
        // this application immediately.
        protected static bool ProcessArguments(string[] args)
        {
            bool bRet = true;

            if (args.Length > 0)
            {
                RegistryKey key = null;
                RegistryKey key2 = null;

                switch (args[0].ToLower())
                {
                    case "-h": //Help
                    case "/h": //Help
                        DisplayUsage();
                        bRet = false;
                        break;

                    case "-embedding": //COM sends this as the argument when starting a out of proc server
                        Console.WriteLine("Request to start as out-of-process COM server.");
                        break;

                    case "-regserver":
                    case "/regserver":
                        try
                        {
                            string subkey =
                                "CLSID\\" + Marshal.GenerateGuidForType(typeof(EventHandlerObj)).ToString("B");
                            key = Registry.ClassesRoot.CreateSubKey(subkey);
                            key2 = key.CreateSubKey("LocalServer32");
                            key2.SetValue(null, Application.ExecutablePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error while registering the server:\n" + ex.ToString());
                        }
                        finally
                        {
                            if (key != null)
                                key.Close();
                            if (key2 != null)
                                key2.Close();
                        }

                        bRet = false;
                        break;

                    case "-unregserver":
                    case "/unregserver":
                        try
                        {
                            key = Registry.ClassesRoot.OpenSubKey(
                                "CLSID\\" + Marshal.GenerateGuidForType(typeof(EventHandlerObj)).ToString("B"), true);
                            key.DeleteSubKey("LocalServer32");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error while unregistering the server:\n" + ex.ToString());
                        }
                        finally
                        {
                            if (key != null)
                                key.Close();
                            if (key2 != null)
                                key2.Close();
                            //unsubscribe from QB events
                            UnsubscribeForEvents(QBSubscriptionType.Data, true); // don't display error 
                            UnsubscribeForEvents(QBSubscriptionType.UIExtension, true); // don't display error 
                        }

                        bRet = false;
                        break;

                    //subscribing for data Event
                    case "-d":
                    case "/d":
                        //Subscribe for Quick Books Data Event - Customer add/Modify/Delete event, if not already subscribed
                        SubscribeForEvents(QBSubscriptionType.Data, String.Empty);
                        bRet = false;
                        break;

                    //subscribing for UI Extension Event
                    case "-u":
                    case "/u":
                    {
                        //Subscribe for UI Extension event - Adding a menu item under Customer Menu in QB.
                        //Get the Menu Item Name from Arguments
                        if (args.Length < 2)
                        {
                            // Menu item name is not provided.
                            // Display Usage and exit
                            DisplayUsage();
                        }
                        else
                        {
                            SubscribeForEvents(QBSubscriptionType.UIExtension, args[1]);
                        }

                        bRet = false;
                        break;
                    }

                    case "-dd":
                    case "/dd":
                        //unsubscribe for Quick Books Data Event
                        UnsubscribeForEvents(QBSubscriptionType.Data, false);
                        bRet = false;
                        break;

                    case "-ud":
                    case "/ud":
                        //unsubscribe for Quick Books UIExtension Event
                        UnsubscribeForEvents(QBSubscriptionType.UIExtension, false);
                        bRet = false;
                        break;

                    default:
                        DisplayUsage();
                        bRet = false;
                        break;
                }
            }

            return bRet;
        }

        // These are the options shown in the CLI when this program is invoked 
        // with the -h flag
        private static void DisplayUsage()
        {
            string strUsage = "Usage";
            strUsage += "\n -regserver \n\t:register as COM out of proc server";
            strUsage +=
                "\n -unregserver \n\t: unregister COM out of proc server. Also unsubscribes from all the events.";
            strUsage += "\n -d		\n\t: subscribe for customer add/modify/delete data event";
            strUsage +=
                "\n -u <Menu Name>   \n\t: subscribe for UI extension event. <Menu Name> will appear under customers menu in QB";
            strUsage += "\n -dd 		\n\t: unsubscribe for customer data event";
            strUsage += "\n -ud 		\n\t: unsubscribe for UI extension event";
            strUsage += "\n";

            Console.Write(strUsage);
        }

        /**          ---- Julius ----
         ****************************************** 
          **** The more important functions!! ****
         ****************************************** 
         */

        // Subscribes this application to listen for Data event or UI extension event
        private static void SubscribeForEvents(QBSubscriptionType strType, string strData)
        {
            //js - The most critical Class, RequestProcessor2Class, this class enables me to
            //-- 1) open connection
            //-- 2) begin a session and specify file access preferences and authorization options
            //-- 3) Send qbXML requests and Get qbXML responses 
            RequestProcessor2Class qbRequestProcessor;
            //js - Use the QBFC class to invoke the Request Processor
            QBSessionManager sessionManager = null;
            //js - function vars
            bool sessionBegun = false;
            bool connectionOpen = false; 

            try
            {
                // Get an instance of the qbXMLRP Request Processor and
                // call OpenConnection if that has not been done already.
                qbRequestProcessor = new RequestProcessor2Class();
                qbRequestProcessor.OpenConnection("", strAppName);

                StringBuilder strRequest = new StringBuilder();

                switch (strType)
                {
                    case QBSubscriptionType.Data:
                        strRequest = new StringBuilder(GetDataEventSubscriptionAddXML());
                        break;

                    case QBSubscriptionType.UIExtension:
                        strRequest = new StringBuilder(GetUIExtensionSubscriptionAddXML(strData));
                        break;

                    default:
                        return;
                }

                string strResponse = qbRequestProcessor.ProcessSubscription(strRequest.ToString());

                //Parse the XML response to check the status
                XmlDocument outputXMLDoc = new XmlDocument();
                outputXMLDoc.LoadXml(strResponse);
                XmlNodeList qbXMLMsgsRsNodeList = outputXMLDoc.GetElementsByTagName("DataEventSubscriptionAddRs");
                if (qbXMLMsgsRsNodeList.Count == 1)
                {
                    XmlAttributeCollection rsAttributes = qbXMLMsgsRsNodeList.Item(0).Attributes;
                    //get the status Code, info and Severity
                    string retStatusCode = rsAttributes.GetNamedItem("statusCode").Value;
                    string retStatusSeverity = rsAttributes.GetNamedItem("statusSeverity").Value;
                    string retStatusMessage = rsAttributes.GetNamedItem("statusMessage").Value;

                    // 3180 : if subscription already subscribed. NOT A NEAT WAY TO DO THIS, NEED TO EXPLORE THIS
                    if ((retStatusCode != "0") && (retStatusCode != "3180"))
                    {
                        Console.WriteLine(
                            "Error while subscribing for events\n\terror Code - {0},\n\tSeverity - {1},\n\tError Message - {2}\n",
                            retStatusCode, retStatusSeverity, retStatusMessage);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while registering for QB events - " + ex.Message);
                qbRequestProcessor = null;
                return;
            }

            /*********************************************/
            /**************** CUSTOM CODE ****************/
            /*********************************************/

            //js -  Now try to do a purchase order Query 
            try
            {
                //-- create an instance of a session manager
                sessionManager = new QBSessionManager();
                //-- create the message set request object
                IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", 13, 0);
                requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

                //-- hoist a class function
                BuildPurchaseOrderQueryRq(requestMsgSet);

                //-- connect to qb
                sessionManager.OpenConnection("", "Redstone Print and Mail");
                connectionOpen = true; 
                sessionManager.BeginSession("", ENOpenMode.omDontCare);
                sessionBegun = true; 
                
                //-- send the request and get the response from qb
                IMsgSetResponse resMsgSet = sessionManager.DoRequests(requestMsgSet); 
                
                //-- end the session and close the connection to qb
                sessionManager.EndSession();
                sessionBegun = false; 
                sessionManager.CloseConnection();
                connectionOpen = false; 
                
                //-- walk response set
                WalkPurchaseOrderQueryRs(resMsgSet);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                
                if (sessionBegun)
                {
                    sessionManager.EndSession();    
                }

                if (connectionOpen)
                {
                    sessionManager.CloseConnection();
                }
            }
            
        } // END OF: SubscribeForEvents () {}

        // Unsubscribes this application from listening to add/modify/delete custmor event
        private static void UnsubscribeForEvents(QBSubscriptionType strType, bool bSilent)
        {
            RequestProcessor2Class qbRequestProcessor;

            try
            {
                // Get an instance of the qbXMLRP Request Processor and
                // call OpenConnection if that has not been done already.
                qbRequestProcessor = new RequestProcessor2Class();
                qbRequestProcessor.OpenConnection("", strAppName);

                StringBuilder strRequest = new StringBuilder(GetSubscriptionDeleteXML(strType));
                string strResponse = qbRequestProcessor.ProcessSubscription(strRequest.ToString());

                //Parse the XML response to check the status
                XmlDocument outputXMLDoc = new XmlDocument();
                outputXMLDoc.LoadXml(strResponse);
                XmlNodeList qbXMLMsgsRsNodeList = outputXMLDoc.GetElementsByTagName("SubscriptionDelRs");

                XmlAttributeCollection rsAttributes = qbXMLMsgsRsNodeList.Item(0).Attributes;
                //get the status Code, info and Severity
                string retStatusCode = rsAttributes.GetNamedItem("statusCode").Value;
                string retStatusSeverity = rsAttributes.GetNamedItem("statusSeverity").Value;
                string retStatusMessage = rsAttributes.GetNamedItem("statusMessage").Value;

                if ((retStatusCode != "0") && (!bSilent))
                {
                    Console.WriteLine(
                        "Error while unsubscribing from events\n\terror Code - {0},\n\tSeverity - {1},\n\tError Message - {2}\n",
                        retStatusCode, retStatusSeverity, retStatusMessage);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while unsubscribing from QB events - " + ex.Message);
                qbRequestProcessor = null;
                return;
            }

            return;
            
        } // END OF: UnsubscribeForEvents 

        // This Method returns the qbXML for Subscribing this application to QB for listening 
        // to customer add/modify/delete event.
        private static string GetDataEventSubscriptionAddXML()
        {
            // Create the qbXML request
            XmlDocument requestXMLDoc = new XmlDocument();
            requestXMLDoc.AppendChild(requestXMLDoc.CreateXmlDeclaration("1.0", null, null));
            requestXMLDoc.AppendChild(requestXMLDoc.CreateProcessingInstruction("qbxml", "version=\"5.0\""));
            XmlElement qbXML = requestXMLDoc.CreateElement("QBXML");
            requestXMLDoc.AppendChild(qbXML);

            // Subscription Message request
            XmlElement qbXMLMsgsRq = requestXMLDoc.CreateElement("QBXMLSubscriptionMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);

            // Data Event Subscription ADD request
            XmlElement dataEventSubscriptionAddRq = requestXMLDoc.CreateElement("DataEventSubscriptionAddRq");
            qbXMLMsgsRq.AppendChild(dataEventSubscriptionAddRq);

            // Data Event Subscription ADD
            XmlElement dataEventSubscriptionAdd = requestXMLDoc.CreateElement("DataEventSubscriptionAdd");
            dataEventSubscriptionAddRq.AppendChild(dataEventSubscriptionAdd);

            // Add Subscription ID
            dataEventSubscriptionAdd.AppendChild(requestXMLDoc.CreateElement("SubscriberID")).InnerText =
                "{8327c7fc-7f05-41ed-a5b4-b6618bb27bf1}";

            // Add COM CallbackInfo
            XmlElement comCallbackInfo = requestXMLDoc.CreateElement("COMCallbackInfo");
            dataEventSubscriptionAdd.AppendChild(comCallbackInfo);

            // Appname and CLSID
            comCallbackInfo.AppendChild(requestXMLDoc.CreateElement("AppName")).InnerText = strAppName;
            comCallbackInfo.AppendChild(requestXMLDoc.CreateElement("CLSID")).InnerText =
                "{62447F81-C195-446f-8201-94F0614E49D5}";

            // Delivery Policy
            dataEventSubscriptionAdd.AppendChild(requestXMLDoc.CreateElement("DeliveryPolicy")).InnerText =
                "DeliverAlways";

            // track lost events
            dataEventSubscriptionAdd.AppendChild(requestXMLDoc.CreateElement("TrackLostEvents")).InnerText = "All";

            // ListEventSubscription
            XmlElement listEventSubscription = requestXMLDoc.CreateElement("ListEventSubscription");
            dataEventSubscriptionAdd.AppendChild(listEventSubscription);

            // Add Customer List and operations
            listEventSubscription.AppendChild(requestXMLDoc.CreateElement("ListEventType")).InnerText = "Customer";
            listEventSubscription.AppendChild(requestXMLDoc.CreateElement("ListEventOperation")).InnerText = "Add";
            listEventSubscription.AppendChild(requestXMLDoc.CreateElement("ListEventOperation")).InnerText = "Modify";
            listEventSubscription.AppendChild(requestXMLDoc.CreateElement("ListEventOperation")).InnerText = "Delete";

            string strRetString = requestXMLDoc.OuterXml;
            LogXmlData(@"C:\Temp\DataEvent.xml", strRetString);
            return strRetString;
            
        } // END OF: GetDataEventSubscriptionAddXML(){}

        // This Method returns the qbXML for the Adding a UI extension to the customer menu.
        // Event will be received any time the menu is clicked 
        private static string GetUIExtensionSubscriptionAddXML(string strMenuName)
        {
            // Create the qbXML request
            XmlDocument requestXMLDoc = new XmlDocument();
            requestXMLDoc.AppendChild(requestXMLDoc.CreateXmlDeclaration("1.0", null, null));
            requestXMLDoc.AppendChild(requestXMLDoc.CreateProcessingInstruction("qbxml", "version=\"5.0\""));
            XmlElement qbXML = requestXMLDoc.CreateElement("QBXML");
            requestXMLDoc.AppendChild(qbXML);

            // Subscription Message request
            XmlElement qbXMLMsgsRq = requestXMLDoc.CreateElement("QBXMLSubscriptionMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);

            // UI Extension Subscription ADD request
            XmlElement uiExtSubscriptionAddRq = requestXMLDoc.CreateElement("UIExtensionSubscriptionAddRq");
            qbXMLMsgsRq.AppendChild(uiExtSubscriptionAddRq);


            // UI Extension Subscription ADD
            XmlElement uiExtEventSubscriptionAdd = requestXMLDoc.CreateElement("UIExtensionSubscriptionAdd");
            uiExtSubscriptionAddRq.AppendChild(uiExtEventSubscriptionAdd);

            // Add Subscription ID
            uiExtEventSubscriptionAdd.AppendChild(requestXMLDoc.CreateElement("SubscriberID")).InnerText =
                "{8327c7fc-7f05-41ed-a5b4-b6618bb27bf1}";

            // Add COM CallbackInfo
            XmlElement comCallbackInfo = requestXMLDoc.CreateElement("COMCallbackInfo");
            uiExtEventSubscriptionAdd.AppendChild(comCallbackInfo);

            // Appname and CLSID
            comCallbackInfo.AppendChild(requestXMLDoc.CreateElement("AppName")).InnerText = strAppName;
            comCallbackInfo.AppendChild(requestXMLDoc.CreateElement("CLSID")).InnerText =
                "{62447F81-C195-446f-8201-94F0614E49D5}";


            //  MenuEventSubscription
            XmlElement menuExtensionSubscription = requestXMLDoc.CreateElement("MenuExtensionSubscription");
            uiExtEventSubscriptionAdd.AppendChild(menuExtensionSubscription);

            // Add To menu Item // To Cusomter Menu
            menuExtensionSubscription.AppendChild(requestXMLDoc.CreateElement("AddToMenu")).InnerText = "Customers";


            XmlElement menuItem = requestXMLDoc.CreateElement("MenuItem");
            menuExtensionSubscription.AppendChild(menuItem);

            // Add Menu Name
            menuItem.AppendChild(requestXMLDoc.CreateElement("MenuText")).InnerText = strMenuName;
            menuItem.AppendChild(requestXMLDoc.CreateElement("EventTag")).InnerText = "menu_" + strMenuName;


            XmlElement displayCondition = requestXMLDoc.CreateElement("DisplayCondition");
            menuItem.AppendChild(displayCondition);

            displayCondition.AppendChild(requestXMLDoc.CreateElement("VisibleIf")).InnerText = "HasCustomers";
            displayCondition.AppendChild(requestXMLDoc.CreateElement("EnabledIf")).InnerText = "HasCustomers";


            string strRetString = requestXMLDoc.OuterXml;
            LogXmlData(@"C:\Temp\UIExtension.xml", strRetString);
            
            return strRetString;
            
        } // END OF: GetUIExtensionSubscriptionAddXML(){}

        // This Method returns the qbXML for deleting the event subscription 
        // for this application from QB
        private static string GetSubscriptionDeleteXML(QBSubscriptionType subscriptionType)
        {
            //Create the qbXML request
            XmlDocument requestXMLDoc = new XmlDocument();
            requestXMLDoc.AppendChild(requestXMLDoc.CreateXmlDeclaration("1.0", null, null));
            requestXMLDoc.AppendChild(requestXMLDoc.CreateProcessingInstruction("qbxml", "version=\"5.0\""));
            XmlElement qbXML = requestXMLDoc.CreateElement("QBXML");
            requestXMLDoc.AppendChild(qbXML);

            //subscription Message request
            XmlElement qbXMLMsgsRq = requestXMLDoc.CreateElement("QBXMLSubscriptionMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);

            //Data Event Subscription ADD request
            XmlElement qbSubscriptionDelRq = requestXMLDoc.CreateElement("SubscriptionDelRq");
            qbXMLMsgsRq.AppendChild(qbSubscriptionDelRq);

            //Subscription ID
            qbSubscriptionDelRq.AppendChild(requestXMLDoc.CreateElement("SubscriberID")).InnerText =
                "{8327c7fc-7f05-41ed-a5b4-b6618bb27bf1}";

            //Subscription Type
            qbSubscriptionDelRq.AppendChild(requestXMLDoc.CreateElement("SubscriptionType")).InnerText =
                subscriptionType.ToString();


            string strRetString = requestXMLDoc.OuterXml;
            LogXmlData(@"C:\Temp\Unsubscribe.xml", strRetString);
            return strRetString;
        } // GetSubscriptionDeleteXML(){}

        // Used only for debug purpose
        // strFile Name should have complete Path of the file too
        private static void LogXmlData(string strFile, string strXML)
        {
            StreamWriter sw = new StreamWriter(strFile);
            sw.WriteLine(strXML);
            sw.Flush();
            sw.Close();
        }

        /*****************************/
        /******** Custom Code ********/
        /*****************************/
        private static void BuildPurchaseOrderQueryRq(IMsgSetRequest requestMsgSet)
        {
            //-- prep obj for appending po info
            IPurchaseOrderQuery purchaseOrderQueryRq = requestMsgSet.AppendPurchaseOrderQueryRq();

            //-- set values for the purchaseOrderQueryRq 
            purchaseOrderQueryRq.metaData.SetValue(ENmetaData.mdMetaDataAndResponseData);
            purchaseOrderQueryRq.iterator.SetValue(ENiterator.itContinue);
            purchaseOrderQueryRq.iteratorID.SetValue("j1_build");

            var ORTxnQueryElementType18203 = enORTxnQueryElementType.TxnIDList;
            
            if (ORTxnQueryElementType18203 == enORTxnQueryElementType.TxnIDList)
            {
                purchaseOrderQueryRq.ORTxnQuery.TxnIDList.Add("200000-1011023419");
            }

            if (ORTxnQueryElementType18203 == enORTxnQueryElementType.RefNumberList)
            {
                purchaseOrderQueryRq.ORTxnQuery.RefNumberList.Add("ab");
            }

            if (ORTxnQueryElementType18203 == enORTxnQueryElementType.RefNumberCaseSensitiveList)
            {
                purchaseOrderQueryRq.ORTxnQuery.RefNumberCaseSensitiveList.Add("ab");
            }

            if (ORTxnQueryElementType18203 == enORTxnQueryElementType.TxnFilter)
            {
                //-- do a TON of stuff
            }

            purchaseOrderQueryRq.IncludeLineItems.SetValue(true);
            purchaseOrderQueryRq.IncludeLinkedTxns.SetValue(true);
            purchaseOrderQueryRq.IncludeRetElementList.Add("ab");
            purchaseOrderQueryRq.OwnerIDList.Add(Guid.NewGuid().ToString());
        }

        private static void WalkPurchaseOrderQueryRs(IMsgSetResponse resMsgSet)
        {
            if (resMsgSet == null) return;
            IResponseList responseList = resMsgSet.ResponseList;
            if (responseList == null) return;

            //-- if 1 request then 1 response, will walk list for practice           
            for (int i = 0; i < responseList.Count; i++)
            {
                IResponse response = responseList.GetAt(i); 
                
                //-- check status code, 0=ok, >0 warning
                if (response.StatusCode >= 0)
                {
                    // the request specific response is in .Detail
                    if (response.Detail != null)
                    {
                        // make sure response is expected type
                        ENResponseType responseType = (ENResponseType) response.Type.GetValue();

                        if (responseType == ENResponseType.rtPurchaseOrderQueryRs)
                        {
                            // upcast to a more specific type, safe because it was checked with response type check above
                            IPurchaseOrderRetList purchaseOrderRet = (IPurchaseOrderRetList) response.Detail;
                            WalkPurchaseOrderRet(purchaseOrderRet);
                        }
                    }
                }
            }
        }

         private static void WalkPurchaseOrderRet(IPurchaseOrderRetList PurchaseOrderRet)
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

            // vendor ref
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

            // class ref
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

            // inventory site ref
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

            // ship to entity ref
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

            // template ref
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
            
            // vendor address's 
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

            // vendor address block
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

            // ship address
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

            // ship address block
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

            // terms ref
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

            // ship method ref
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

            // currency ref
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

            // sales tax code ref
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

            // linked transaction list
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

            // OR purchase order line return list, LOOP OVER THIS LIST
            // I think these are equivelent to the "line items" in the UI  
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

                            // list id and full name
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

                            // inventory site location ref
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

                            // customer ref "list id" and "full name" 
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

                            // data extention return list
                            // INNER FOR-LOOP 
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

                    // purchase order line group 
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

                            //
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

                            // purchase order line return list
                            // INNER FOR-LOOP
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


                                    // inventory site location ref
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

                                    // customer ref
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

                                    // sales tax code ref
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

                                    // data extention return list
                                    // INNER FOR-LOOP
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

                            // data extention return list
                            // INNER FOR-LOOP
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

            // data extention return list
            // INNER FOR-LOOP
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

        /// <summary>
        /// *****************************************************
        ///  **** The MAIN ENTRY POINT for the Application. ****
        /// *****************************************************
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                if (!ProcessArguments(args))
                {
                    return;
                }

                // Initialize critical member variables.
                m_iObjsInUse = 0;
                m_iServerLocks = 0;
                m_uiMainThreadId = GetCurrentThreadId();
                
                // Register the EventHandlerObjClassFactory.
                EventHandlerObjClassFactory factory = new EventHandlerObjClassFactory();
                factory.ClassContext = (uint) CLSCTX.CLSCTX_LOCAL_SERVER;
                factory.ClassId = Marshal.GenerateGuidForType(typeof(EventHandlerObj));
                factory.Flags = (uint) REGCLS.REGCLS_MULTIPLEUSE | (uint) REGCLS.REGCLS_SUSPENDED;
                factory.RegisterClassObject();
                ClassFactoryBase.ResumeClassObjects();

                Console.WriteLine("Waiting for QB Customer Add Event .....\n");

                // Start the message loop.
                MSG msg;
                IntPtr null_hwnd = new IntPtr(0);
                while (GetMessage(out msg, null_hwnd, 0, 0) != false)
                {
                    Console.WriteLine("JHA - Invoke Custom Redstone Print and Mail functionality.");
                    TranslateMessage(ref msg);
                    DispatchMessage(ref msg);
                }

                Console.WriteLine("Out of message loop.");

                // Revoke the class factory immediately.
                // Don't wait until the thread has stopped before
                // we perform revokation.
                factory.RevokeClassObject();
                Console.WriteLine("EventHandlerObjClassFactory Revoked.");

                // Just an indication that this COM EXE Server is stopped.
                Console.WriteLine("Press [ENTER] to exit.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error in program - " + ex.Message);
            }
        }
    }
}