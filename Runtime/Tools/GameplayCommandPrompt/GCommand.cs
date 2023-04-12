using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laio.Tools
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class GCommand : Attribute
    {

        public string Command;
        public object[] Params;

        public GCommand(string name, params object[] Args)
        {
            this.Command = name;
            this.Params = Args;
        }

    }
}
