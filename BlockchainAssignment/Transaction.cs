using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    // Define a class representing a transaction
    class Transaction
    {
        // Transaction properties
        public DateTime timestamp;      // Timestamp of the transaction
        public String senderAddress, recipientAddress, hash, signature; // Addresses, hash, and signature of the transaction
        public Double amount, fee;     // Amount and fee of the transaction

        // Constructor for creating a new transaction
        public Transaction(String senderAddress, String recipientAddress, Double amount, Double fee, String privateKey)
        {
            this.timestamp = DateTime.Now; // Set timestamp to current time
            this.senderAddress = senderAddress; // Set sender address
            this.recipientAddress = recipientAddress; // Set recipient address
            this.amount = amount; // Set transaction amount
            this.fee = fee; // Set transaction fee
            this.hash = CreateHash(); // Calculate transaction hash
            this.signature = Wallet.Wallet.CreateSignature(senderAddress, privateKey, hash); // Create transaction signature
        }

        // Method to create hash of the transaction
        public String CreateHash()
        {
            SHA256 hasher = SHA256Managed.Create();
            // Concatenate transaction data
            String input = timestamp.ToString() + senderAddress + recipientAddress + amount.ToString() + fee.ToString() + "Assignment Coin";

            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            String hash = string.Empty;

            // Convert hash bytes to string
            foreach (byte x in hashByte)
                hash += String.Format("{0:x2}", x);

            return hash;
        }

        // Method to convert transaction to string
        public override string ToString()
        {
            return "Timestamp: " + timestamp.ToString() +
                "\nSender Address: " + senderAddress +
                "\nRecipient Address: " + recipientAddress +
                "\nAmount: " + amount.ToString() + " Assignment Coin" +
                "\nFee: " + fee.ToString() + " Assignemnt Coin" +
                "\nHash: " + hash +
                "\nSignature: " + signature;
        }
    }
}
