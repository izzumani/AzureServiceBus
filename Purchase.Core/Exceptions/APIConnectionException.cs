﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Core.Exceptions
{
    public class APIConnectionException : Exception
    {
        public APIConnectionException(string message) :
           base(message)
        {

        }
    }
}
