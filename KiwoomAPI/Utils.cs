// This source code is a part of KiwoomAPI Wrapper Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using AxKHOpenAPILib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KiwoomAPI
{
    public class Utils
    {
        static SemaphoreSlim signal;
        static _DKHOpenAPIEvents_OnReceiveTrDataEventHandler wait_handler;

        public static void StartTrRequest()
        {
            signal = new SemaphoreSlim(0, 1);
            if (wait_handler == null)
            {
                wait_handler = new _DKHOpenAPIEvents_OnReceiveTrDataEventHandler((s, e) => { signal.Release(); });
                Session.API.OnReceiveTrData += wait_handler;
            }
        }

        public static async Task WaitTrRequest(_DKHOpenAPIEvents_OnReceiveTrDataEventHandler trevent)
        {
            Session.API.OnReceiveTrData += trevent;
            await signal.WaitAsync();
            Session.API.OnReceiveTrData -= trevent;
        }
    }
}
