using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Block
    {
        int index;
        String hash, prevHash;
        DateTime timeStamp;

        public Block()
        {
            this.index = 0;
            this.timeStamp = DateTime.Now;
            this.prevHash = String.Empty;
            this.hash = CreateHash();
        }

        public Block(int index, String hash)
        {
            this.index = index + 1;
            this.timeStamp = DateTime.Now;
            this.prevHash = hash;
            this.hash = CreateHash();
        }

        public String CreateHash()
        {
            SHA256 hasher = SHA256Managed.Create();

            String input = index.ToString() + timeStamp.ToString() + prevHash;
            Byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
            
            String hash = string.Empty;

            foreach (byte x in hashByte)
                hash += String.Format("{0:x2}", x);

            return hash;

        }

        public override string ToString()
        {
            return "Index :" + index.ToString() +
                "\nTimestamp: " + timeStamp.ToString() +
                "\nHash " + hash +
                "\nPrevious Hash " + prevHash;
        }
    }
}
