using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelTest
{
    [TestClass]
    public class ModelContainer
    {
        [TestMethod]
        public void ModelContainerBase_FindsManagedClasses_NotNull()
        {
            var modelContainer = new TestModelContainer();
            var result = modelContainer.GetAllManagedClasses();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ModelContainerBase_FindsManagedClasses_Any()
        {
            var modelContainer = new TestModelContainer();
            var result = modelContainer.GetAllManagedClasses();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);
        }

        [TestMethod]
        public void ModelContainerBase_FindsManagedClasses_User()
        {
            var modelContainer = new TestModelContainer();
            var result = modelContainer.GetAllManagedClasses();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);

            Assert.AreEqual(result.First().Value, typeof(Model.Personal.User));
        }

        [TestMethod]
        public void ModelContainerBase_FindsManagedClassTypes_User()
        {
            var modelContainer = new TestModelContainer();
            var result = modelContainer.GetAllManagedClassTypes();

            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.Count);

            Assert.AreEqual(result.First(), typeof(Model.Personal.User));
        }
    }

}
