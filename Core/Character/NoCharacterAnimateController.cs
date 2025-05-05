using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lost.Character
{
    public class NoCharacterAnimateController : AbstractCharacterAnimateController
    {
        public override AnimateRunData PlayAnimateByName(string animateName, Action complete = null, Action interrupt = null)
        {
            //throw new NotImplementedException();
            return null;
        }

        public override AnimateRunData PlayAnimateByTrigger(string trigger, Action complete = null, Action interrupt = null)
        {
            //throw new NotImplementedException();
            return null;
        }

        public override void Trigger(string name)
        {
            //throw new NotImplementedException();
            return;
        }
    }
}
