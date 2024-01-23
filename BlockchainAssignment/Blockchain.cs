using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Blockchain
    {
        List<Block> Blocks = new List<Block>();

        public Blockchain()
        {
            Blocks = new List<Block>()
            {
                new Block()
            };
        }

        public String getBlock(int index)
        {
            if (index >= 0 && index < Blocks.Count)
                return Blocks[index].ToString();
            return "Block does not exist!";
        }

        public override string ToString()
        {
            return String.Join("\n", Blocks);
        }
    }
        
}
