using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quadax_Mastermind.POCOs
{
    public class Guess
    {
        public Guess(int value)
        {
            Value = value;
            IsCorrect = false;
        }

        public int Value { get; set; }
        public bool IsCorrect { get; set; }
        public bool AlmostCorrect {  get; set; }
    }
}
