using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockchainAssignment
{
    public partial class BlockchainApp : Form
    {
        Blockchain blockchain;
        public BlockchainApp()
        {
            InitializeComponent();
            blockchain = new Blockchain();
            richTextBox1.Text = "New Blockchain Initialised!";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Int32.TryParse(textBox1.Text, out int index))
                richTextBox1.Text = blockchain.getBlock(index);
            else
                richTextBox1.Text = "Not a number!";
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String privKey;
            Wallet.Wallet myNewWallet = new Wallet.Wallet(out privKey);
            publicKey.Text = myNewWallet.publicID;
            privateKey.Text = privKey;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(Wallet.Wallet.ValidatePrivateKey(privateKey.Text, publicKey.Text)){
                richTextBox1.Text = "Keys are valid";
            }
            else
            {
                richTextBox1.Text = "Keys are invalid";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Transaction newTransaction = new Transaction(publicKey.Text, receiver.Text, Double.Parse(amount.Text), Double.Parse(fee.Text), privateKey.Text);
            blockchain.TransactionPool.Add(newTransaction);
            richTextBox1.Text = newTransaction.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<Transaction> transactions = blockchain.GetPendingTransactions();

            Block newBlock = new Block(blockchain.getLastBlock(), transactions, publicKey.Text);
            blockchain.Blocks.Add(newBlock);
            richTextBox1 .Text = newBlock.ToString();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            richTextBox1.Text = blockchain.ToString();
        }

        private void button5_Click_2(object sender, EventArgs e)
        {
           richTextBox1.Text = String.Join("\n ", blockchain.TransactionPool); //Test
        }
    }
}
