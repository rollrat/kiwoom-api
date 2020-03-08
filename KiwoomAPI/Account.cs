// This source code is a part of KiwoomAPI Wrapper Project.
// Copyright (C) 2020. rollrat. Licensed under the MIT Licence.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KiwoomAPI
{
    public class Account
    {
        public static string[] AccountList { get; set; }

        public static async Task<Dictionary<string, string>> 계좌평가잔고내역요청(string account, string password, string gubun = "1")
        {
            Utils.StartTrRequest();

            Session.API.SetInputValue("계좌번호", account);
            Session.API.SetInputValue("비밀번호", password);
            Session.API.SetInputValue("비밀번호입력매체구분", "00");
            Session.API.SetInputValue("조회구분", gubun);

            Session.API.CommRqData("계좌평가잔고내역요청", "opw00018", 0, "8100");

            var dict = new Dictionary<string, string>();

            await Utils.WaitTrRequest(new AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEventHandler((s, e) => {
                Action<string> append = (string name) => { dict.Add(name, Session.API.GetCommData(e.sTrCode, e.sRQName, 0, name)); };
                append("총매입금액");
                append("총평가금액");
                append("총평가손익금액");
                append("총수익률(%)");
                append("추정예탁자산");
                append("총대출금");
                append("총융자금액");
                append("총대주금액");
                append("조회건수");
            }));

            return dict;
        }

    }
}
