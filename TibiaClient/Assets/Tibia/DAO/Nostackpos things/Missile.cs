﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Tibia.DAO
{
    public class Missile : Thing
    {
        public Missile(int shotId)
        {
            this.DatId = shotId;
        }
    }
}
