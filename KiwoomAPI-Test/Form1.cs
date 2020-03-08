using KiwoomAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KiwoomAPI_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Session.StartSession(null, new AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEventHandler((s,e) => {
                _ = Account.계좌평가잔고내역요청(Account.AccountList[0], "0000", "1");
            }));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
