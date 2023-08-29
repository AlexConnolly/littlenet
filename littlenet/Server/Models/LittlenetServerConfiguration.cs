using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace littlenet.Server.Models
{
    public enum MalformedMessageStrategy
    {
        Continue = 1,
        Disconnect = 2
    }

    public record LittlenetServerConfiguration
    {
        /// <summary>
        /// The strategy to use when a malformed message is presented to the system
        /// </summary>
        public MalformedMessageStrategy MalformedMessageStrategy { get; set; } = MalformedMessageStrategy.Continue;
    }
}
