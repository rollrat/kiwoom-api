// This source code is a part of KiwoomAPI Wrapper Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using AxKHOpenAPILib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KiwoomAPI
{
    public class Session
    {
        static AxKHOpenAPI api;
        public static AxKHOpenAPI API { get { if (Initialized) return api; throw new Exception("세션을 생성하고 다시시도해주세요!"); } private set { api = value; } }
        static Form dummy = new Form();
        public static bool Initialized { get; private set; }

        public static string UserId { get; private set; }
        public static string UserName { get; private set; }
        public static string ServerGubun { get; private set; }

        public static void StartSession(AxKHOpenAPI API = null, _DKHOpenAPIEvents_OnEventConnectEventHandler connect_event = null)
        {
            if (API != null)
                api = API;
            else
            {
                api = new AxKHOpenAPI();
                api.BeginInit();
                api.Enabled = true;
                byte[] bytes = Convert.FromBase64String(@"AAEAAAD/////AQAAAAAAAAAMAgAAAFdTeXN0ZW0uV2luZG93cy5Gb3JtcywgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODkFAQAAACFTeXN0ZW0uV2luZG93cy5Gb3Jtcy5BeEhvc3QrU3RhdGUBAAAABERhdGEHAgIAAAAJAwAAAA8DAAAAJQAAAAIBAAAAAQAAAAAAAAAAAAAAABAAAAACAAEAVgoAACsFAAAAAAAACw==");
                using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
                {
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Position = 0;
                    api.OcxState = (AxHost.State)(new BinaryFormatter().Deserialize(ms));
                }
                api.OnEventConnect += new _DKHOpenAPIEvents_OnEventConnectEventHandler((s, e) => { 
                    Initialized = true;
                    UserId = api.GetLoginInfo("USER_ID");
                    UserName = api.GetLoginInfo("USER_NAME");
                    ServerGubun = api.GetLoginInfo("GetServerGubun");
                    Account.AccountList = api.GetLoginInfo("ACCLIST").Split(';');
                    Stock.StockInfo = api.GetCodeListByMarket(null).Split(';').Select(x => (x, api.GetMasterCodeName(x))).ToList();
                });
                if (connect_event != null)
                    api.OnEventConnect += connect_event;
                dummy.Controls.Add(api);
                api.EndInit();
            }
            api.CommConnect();
        }
    }
}
