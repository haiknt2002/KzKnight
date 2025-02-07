using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Assets.Script.Model
{
    public class Player
    {
        public string Name { get; set; }
        public float HP { get; }
        public string Status { get; set; }
        public float Speed { get; set; }

        public Player()
        {
            HP = 100f;
            Speed = 5f;
        }
    }
}
