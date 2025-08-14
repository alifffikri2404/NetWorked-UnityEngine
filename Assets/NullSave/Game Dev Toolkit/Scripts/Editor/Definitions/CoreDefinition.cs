using System.Collections.Generic;

namespace NullSave.GDTK
{
    public class CoreDefinition : ToolDefinition
    {
        public override List<string> GetSymbols
        {
            get
            {
                return new List<string>() { "GDTK" };
            }
        }
    }
}