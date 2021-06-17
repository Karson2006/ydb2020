using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using iTR.Lib;

namespace iTR.Tool
{
    public partial class RSAForm : Form
    {
        RSACryptoServiceProvider rsa;
        public RSAForm()
        {
            InitializeComponent();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void RSAForm_Load(object sender, EventArgs e)
        {
            publickeyBox.Text = "<RSAKeyValue>" +
                                "<Modulus>7ZCc4DTkpqkzbOqwphgyLA6+/dRF3dKfCseIN6H5/7GLHc2BwGasQNgQwkAfH5iTKmsqJ4qqiksOvSRS5fH4Q8IQ4QLkyMuwWedZiuhCbyl+/NDtmckQV4a+9+byXxGdPYFfnQVEOi1qlsA+iCqP27GShx3tgol6fCL1dm94QCs=</Modulus>" +
                                "<Exponent>AQAB</Exponent>" +
                                "</RSAKeyValue>";
            rsa = new RSACryptoServiceProvider();
        }


        private void Keybutton_Click(object sender, EventArgs e)
        {
            publickeyBox.Text= rsa.ToXmlString(false);
            privatekeyBox.Text = rsa.ToXmlString(true);
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
           encryptedBox.Text= iTR.Lib.Common.RSAEncrypt(publickeyBox.Text, inputBox.Text);
        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {
            decryptedBox.Text=iTR.Lib.Common.RSADecrypt(privatekeyBox.Text, encryptedBox.Text);
        }
    }
}
