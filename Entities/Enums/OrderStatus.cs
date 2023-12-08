﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enums
{
    public enum OrderStatus
    {
        Accepted,
        Prepared,
        Shipped,
        Arrived,
        Completed,
        Returned,
        Cancelled
    }
}
