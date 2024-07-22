using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.DungeonEnvironment.Enums
{
    public enum EnvironmentPass : int
    {
        Previous = 0,
        Main = 1,
        Past = 2,
        AI = 3,
        Uncollideble = 4,
        RenderLoad = 5,
    }
}
