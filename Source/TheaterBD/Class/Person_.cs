﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheaterBD.Class
{
    public class Person_
    {
        public Person_(int id, string name)
        {
            Id = id;
            FIO = name;
        }

        public int Id { get; private set; }
        public string FIO { get; private set; }
    }
}
