using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    // Define a class representing a blockchain
    class Blockchain
    {
        // List of blocks and transaction pool
        public List<Block> Blocks = new List<Block>();
        public List<Transaction> TransactionPool;

        // Number of transactions per block
        int transactionsPerBlock = 5;

        // Constructor for creating a new blockchain
        public Blockchain()
        {
            Blocks = new List<Block>()
            {
                new Block() // Initialize blockchain with genesis block
            };
            TransactionPool = new List<Transaction>(); // Initialize transaction pool
        }

        // Method to get block by index
        public String getBlock(int index)
        {
            if (index >= 0 && index < Blocks.Count)
                return Blocks[index].ToString();
            return "Block does not exist!";
        }

        // Method to get the last block in the blockchain
        public Block getLastBlock()
        {
            return Blocks[Blocks.Count - 1];
        }

        // Method to get pending transactions based on specified algorithm
        public List<Transaction> GetPendingTransactions(string algorithm)
        {
            int n = Math.Min(transactionsPerBlock, TransactionPool.Count);

            if (algorithm == "g") // Greedy algorithm sorts transactions by fee
            {
                TransactionPool.Sort((t1, t2) => t2.fee.CompareTo(t1.fee));
            }
            else if (algorithm == "a") // Altruistic algorithm sorts transactions by timestamp
            {
                TransactionPool.Sort((transaction1, transaction2) => transaction1.timestamp.CompareTo(transaction2.timestamp));
            }
            else if (algorithm == "r") // Random algorithm shuffles transactions randomly
            {
                TransactionPool = TransactionPool.OrderBy(transaction => Guid.NewGuid()).ToList();
            }

            // Get selected number of transactions from the transaction pool
            List<Transaction> transactions = TransactionPool.GetRange(0, n);
            TransactionPool.RemoveRange(0, n);
            return transactions;
        }

        // Method to convert blockchain to string
        public override string ToString()
        {
            return String.Join("\n", Blocks);
        }

        // Method to get balance of a wallet address
        public double GetBalance(String address)
        {
            double balance = 0.0;
            foreach (Block b in Blocks)
            {
                foreach (Transaction t in b.transactionList)
                {
                    if (t.recipientAddress.Equals(address))
                    {
                        balance += t.amount;
                    }
                    if (t.senderAddress.Equals(address))
                    {
                        balance -= (t.amount + t.fee);
                    }
                }
            }

            return balance;
        }

        // Method to validate Merkle root of a block
        public bool validateMerkleRoot(Block b)
        {
            String reMerkle = Block.MerkleRoot(b.transactionList);
            return reMerkle.Equals(b.merkleRoot);
        }
    }
}
