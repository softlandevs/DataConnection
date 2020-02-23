using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelTest
{
    internal class TestModelContainer : Model.ModelContainerBase
    {
        public TestModelContainer()
        {
            Users = new Dictionary<string, Model.Personal.User>();
        }

        public Dictionary<string,Model.Personal.User> Users{ get; }
    }
}
