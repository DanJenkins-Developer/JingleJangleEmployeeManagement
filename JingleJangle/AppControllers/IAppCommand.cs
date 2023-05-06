using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// •	Implement a IAppCommand interface that requires Initialize() and Execute() methods

namespace JingleJangle.AppControllers
{
    internal interface IAppCommand
    {
        void Execute();
        void Initialize();
    }
}
