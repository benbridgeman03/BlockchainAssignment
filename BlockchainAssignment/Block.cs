using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Threading;

namespace BlockchainAssignment
{
    // Define a class representing a block in the blockchain
    class Block
    {
        // Block properties
        int index;                  // Index of the block in the blockchain
        public String hash, prevHash; // Hash values of the current and previous blocks
        DateTime timeStamp;         // Time of block creation

        // List of transactions in the block and Merkle root
        public List<Transaction> transactionList;
        public String merkleRoot;

        // Nonce for proof-of-work, difficulty level, reward, fees, miner's address
        public long nonce = 0;
        public int difficulty = 6;
        public double reward = 1.0;
        public double fees = 0.0;
        public String minerAddress = "";

        // Number of threads for parallel mining
        int numberOfThreads = Environment.ProcessorCount;

        // Default constructor for genesis block
        public Block()
        {
            this.index = 0;
            this.timeStamp = DateTime.Now;
            this.prevHash = String.Empty;
            this.transactionList = new List<Transaction>();
            this.hash = Mine(numberOfThreads); // Mine the genesis block
        }

        // Constructor for subsequent blocks
        public Block(int index, String hash)
        {
            this.index = index + 1;
            this.timeStamp = DateTime.Now;
            this.prevHash = hash;
            this.hash = CreateHash();
        }

        // Constructor for new block with transactions
        public Block(Block block, List<Transaction> transactions, String minerAddress)
        {
            this.index = block.index + 1;
            this.timeStamp = DateTime.Now;
            this.prevHash = block.hash;
            this.minerAddress = minerAddress;
            transactions.Add(CreateRewardTransaction(transactions)); // Add reward transaction
            this.transactionList = transactions;
            merkleRoot = MerkleRoot(transactionList); // Calculate Merkle root
            this.hash = Mine(numberOfThreads); // Mine the block
        }

        // Method to create hash of the block
        public String CreateHash()
        {
            SHA256 hasher = SHA256Managed.Create();

            // Concatenate block data
            String input = index.ToString() + timeStamp.ToString() + prevHash + nonce.ToString() + reward.ToString() + merkleRoot;
            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            String hash = string.Empty;

            // Convert hash bytes to string
            foreach (byte x in hashByte)
                hash += String.Format("{0:x2}", x);

            return hash;

        }

        // Method to mine the block
        public String Mine(int numberOfThreads)
        {
            // Initialize nonce
            nonce = 0;
            // Generate initial hash
            String hash = CreateHash();

            // Define the expected prefix for the hash
            String re = new string('0', difficulty);

            // List to hold mining tasks
            List<Task<(string, int, TimeSpan)>> miningTasks = new List<Task<(string, int, TimeSpan)>>();

            // Start mining tasks on multiple threads
            for (int i = 0; i < numberOfThreads; i++)
            {
                int startingNonce = i;
                // Start a new mining task on a separate thread
                miningTasks.Add(Task.Run(() => ThreadMine(re, startingNonce)));
            }

            // Wait for any mining task to complete
            Task<(string, int, TimeSpan)> completedTask = Task.WhenAny(miningTasks).Result;

            // Clean up tasks
            foreach (var task in miningTasks)
            {
                // Dispose of completed tasks
                task.ContinueWith(t => t.Dispose(), TaskContinuationOptions.ExecuteSynchronously);
            }

            // Retrieve results from the completed task
            var (foundHash, threadId, duration) = completedTask.Result;
            // Output the result
            Console.WriteLine($"Thread {threadId} found the hash: {foundHash} in {duration.TotalSeconds} seconds");

            // Return the found hash
            return foundHash;
        }

        // Method to mine block on a separate thread
        public (string, int, TimeSpan) ThreadMine(string re, int startingNonce)
        {
            // Initialize nonce
            nonce = startingNonce;
            // Start stopwatch to measure mining time
            Stopwatch stopwatch = Stopwatch.StartNew();
            // Infinite loop to search for valid hash
            while (true)
            {
                // Generate current hash
                String currentHash = CreateHash();
                // Check if the hash meets the required difficulty
                if (currentHash.StartsWith(re))
                {
                    // Stop the stopwatch
                    stopwatch.Stop();
                    // Return the found hash, thread ID, and mining duration
                    return (currentHash, Thread.CurrentThread.ManagedThreadId, stopwatch.Elapsed);
                }
                // Increment nonce by the number of threads to search different parts of the nonce space
                nonce += numberOfThreads;
            }
        }

        // Method to create reward transaction
        public Transaction CreateRewardTransaction(List<Transaction> transactions)
        {
            double fees = transactions.Aggregate(0.0, (acc, t) => acc + t.fee);
            return new Transaction("Mine Rewards", minerAddress, (reward + fees), 0, "");
        }

        // Method to calculate Merkle root
        public static String MerkleRoot(List<Transaction> transactionList)
        {
            List<String> hashes = transactionList.Select(t => t.hash).ToList();
            if (hashes.Count == 0)
            {
                return String.Empty;
            }
            if (hashes.Count == 1)
            {
                return HashCode.HashTools.CombineHash(hashes[0], hashes[0]);
            }
            while (hashes.Count != 1)
            {
                List<String> merkleLeaves = new List<string>();
                for (int i = 0; i < hashes.Count; i += 2)
                {
                    if (i == hashes.Count - 1)
                    {
                        merkleLeaves.Add(HashCode.HashTools.CombineHash(hashes[i], hashes[i]));
                    }
                    else
                    {
                        merkleLeaves.Add(HashCode.HashTools.CombineHash(hashes[i], hashes[i + 1]));
                    }
                }
                hashes = merkleLeaves;
            }
            return hashes[0];
        }

        // Method to convert block to string
        public override string ToString()
        {
            String output = String.Empty;
            transactionList.ForEach(t => output += (t.ToString() + "\n"));

            return "Index :" + index.ToString() +
                "\nTimestamp: " + timeStamp.ToString() +
                "\nHash " + hash +
                "\nPrevious Hash " + prevHash +
                "\nNocne " + nonce +
                "\nDifficulty " + difficulty +
                "\nReward " + reward +
                "\nFees " + fees +
                "\nMiners Address " + minerAddress +
                "\nTransactions\n" + output;
        }
    }
}
