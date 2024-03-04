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
        Blockchain blockchain; // Declare a variable to hold the blockchain

        public BlockchainApp()
        {
            InitializeComponent();
            blockchain = new Blockchain(); // Initialize the blockchain
            richTextBox1.Text = "New Blockchain Initialised!"; // Display a message indicating a new blockchain is initialized
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // No actions required when the form loads
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Button to retrieve a block by index
            if (Int32.TryParse(textBox1.Text, out int index))
                richTextBox1.Text = blockchain.getBlock(index); // Display the block's information
            else
                richTextBox1.Text = "Not a number!"; // Display an error message if input is not a number
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Button to create a new wallet
            String privKey;
            Wallet.Wallet myNewWallet = new Wallet.Wallet(out privKey); // Create a new wallet
            publicKey.Text = myNewWallet.publicID; // Display the public key
            privateKey.Text = privKey; // Display the private key
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Button to validate keys
            if (Wallet.Wallet.ValidatePrivateKey(privateKey.Text, publicKey.Text))
            {
                richTextBox1.Text = "Keys are valid"; // Display message if keys are valid
            }
            else
            {
                richTextBox1.Text = "Keys are invalid"; // Display message if keys are invalid
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Button to add a new transaction to the blockchain
            Transaction newTransaction = new Transaction(publicKey.Text, receiver.Text, Double.Parse(amount.Text), Double.Parse(fee.Text), privateKey.Text); // Create a new transaction
            blockchain.TransactionPool.Add(newTransaction); // Add the transaction to the transaction pool
            richTextBox1.Text = newTransaction.ToString(); // Display the transaction details
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Button to mine a new block
            string algorithm = "";

            if (radioButtonGreedy.Checked)
            {
                algorithm = "g"; // Use greedy algorithm for mining
            }
            else if (radioButtonAlturistic.Checked)
            {
                algorithm = "a"; // Use altruistic algorithm for mining
            }
            else if (radioButtonRandom.Checked)
            {
                algorithm = "r"; // Use random algorithm for mining
            }

            List<Transaction> transactions = blockchain.GetPendingTransactions(algorithm); // Get pending transactions based on selected algorithm

            Block newBlock = new Block(blockchain.getLastBlock(), transactions, publicKey.Text); // Create a new block
            blockchain.Blocks.Add(newBlock); // Add the new block to the blockchain
            richTextBox1.Text = newBlock.ToString(); // Display the new block details
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            richTextBox1.Text = blockchain.ToString(); // Display the entire blockchain
        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            richTextBox1.Text = String.Join("\n ", blockchain.TransactionPool); // Display the transaction pool
        }

        private void receiver_TextChanged(object sender, EventArgs e)
        {
            // No actions required when the receiver text changes
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // No actions required when the radio button state changes
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Button to validate the blockchain
            bool valid = true;

            if (blockchain.Blocks.Count == 1)
            {
                if (!blockchain.validateMerkleRoot(blockchain.Blocks[0]))
                {
                    richTextBox1.Text = "Blockchain is invalid"; // Display message if blockchain is invalid
                }
                else
                {
                    richTextBox1.Text = "Blockchain is valid"; // Display message if blockchain is valid
                }

                richTextBox1.Text = "Blockchain is valid"; // Display message if blockchain is valid
                return;
            }

            for (int i = 1; i < blockchain.Blocks.Count - 1; i++)
            {
                if (blockchain.Blocks[i].prevHash != blockchain.Blocks[i - 1].hash || !blockchain.validateMerkleRoot(blockchain.Blocks[i]))
                {
                    richTextBox1.Text = "Blockchain is invalid"; // Display message if blockchain is invalid
                    return;
                }
            }

            if (valid)
            {
                richTextBox1.Text = "Blockchain is valid"; // Display message if blockchain is valid
            }
            else
            {
                richTextBox1.Text = "Blockchain is invalid"; // Display message if blockchain is invalid
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = blockchain.GetBalance(publicKey.Text).ToString() + " Assignment Coin"; // Display the balance of the specified public key
        }
    }
}
