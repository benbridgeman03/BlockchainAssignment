using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Blockchain
    {
        public List<Block> Blocks = new List<Block>();
        public List<Transaction> TransactionPool;

        int transactionsPerBlock = 5;

        public Blockchain()
        {
            Blocks = new List<Block>()
            {
                new Block()
            };
            TransactionPool = new List<Transaction>();
        }

        public String getBlock(int index)
        {
            if (index >= 0 && index < Blocks.Count)
                return Blocks[index].ToString();
            return "Block does not exist!";
        }

        //returns the last block in the blockchain
        public Block getLastBlock()
        {
            return Blocks[Blocks.Count -1];
        }

        public List<Transaction> GetPendingTransactions()
        {
            int n = Math.Min(transactionsPerBlock, TransactionPool.Count);
            List<Transaction> transactions = TransactionPool.GetRange(0, n);
            TransactionPool.RemoveRange(0, n);
            return TransactionPool;
        }

        public override string ToString()
        {
            return String.Join("\n", Blocks);
        }
    }
        
}
