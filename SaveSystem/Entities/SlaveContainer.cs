using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace AssemblyCSharp.Assets.Logic.SaveSystem.Entities
{
    [Serializable]
    public class SlaveContainer
    {
        public List<SlaveSaveElement> Slaves;

        public SlaveContainer()
        {
            Slaves = new List<SlaveSaveElement>();
        }

        public SlaveSaveElement GetSlaveByFileName(string ItemFileName)
        {
            if (Slaves.Any(x => x.ItemFileName == ItemFileName))
                return Slaves.Where(x => x.ItemFileName == ItemFileName).FirstOrDefault();
            return new SlaveSaveElement("Asa_Slave");
        }
    }
}
