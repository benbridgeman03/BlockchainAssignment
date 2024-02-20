using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using System.Threading;

namespace BlockchainAssignment
{
    class Block
    {
        int index;
        String hash, prevHash;
        DateTime timeStamp;

        List<Transaction> transactionList;

        public long nonce = 0;
        public int difficulty = 4;

        public double reward = 1.0;
        public String minerAddress = "";

        public Block()
        {
            this.index = 0;
            this.timeStamp = DateTime.Now;
            this.prevHash = String.Empty;
            this.transactionList = new List<Transaction>();
            this.hash = Mine();
        }

        public Block(int index, String hash)
        {
            this.index = index + 1;
            this.timeStamp = DateTime.Now;
            this.prevHash = hash;
            this.hash = Mine();
        }

        public Block(Block block, List<Transaction> transactions, String minerAddress)
        {
            this.index = block.index + 1;
            this.timeStamp = DateTime.Now;
            this.prevHash = block.hash;
            this.minerAddress = minerAddress;
            transactions.Add(CreateRewardTransaction(transactions));
            this.transactionList = transactions;
            this.hash = Mine();
        }

        public String CreateHash()
        {
            SHA256 hasher = SHA256Managed.Create();

            String input = index.ToString() + timeStamp.ToString() + prevHash + nonce;
            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
            
            String hash = string.Empty;

            foreach (byte x in hashByte)
                hash += String.Format("{0:x2}", x);

            return hash;

        }

        public String Mine()
        {
            nonce = 0;
            String hash = CreateHash();

            String re = new string('0', difficulty);

            while (!hash.StartsWith(re))
            {
                nonce++;
                hash = CreateHash();
            }

            return hash;
        }

        public Transaction CreateRewardTransaction(List<Transaction> transactions)
        {
            double fees = transactions.Aggregate(0.0, (acc, t) => acc + t.fee);
            return new Transaction("Mine Rewards",minerAddress, (reward + fees), 0, "");
        }

        public override string ToString()
        {
            return "Index :" + index.ToString() +
                "\nTimestamp: " + timeStamp.ToString() +
                "\nHash " + hash +
                "\nPrevious Hash " + prevHash +
                "\nNocne " + nonce;
        }
    }
}
