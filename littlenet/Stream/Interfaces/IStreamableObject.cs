﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.Stream.Interfaces
{
    public interface IStreamableObject
    {
        public void WriteToStream(IDataStream stream);
        public void ReadFromStream(IDataStream stream);
    }
}
