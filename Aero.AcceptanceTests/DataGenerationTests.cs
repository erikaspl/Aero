using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aero.EF;
using Xunit;

namespace Aero.AcceptanceTests
{
    public class DataGenerationTests
    {
        [UseDatabase]
        [Fact]
        public void GenerateTestDataTest()
        {
            TestData.GenerateTestData();

            using (AeroContainer aero = new AeroContainer())
            {
                Assert.Equal(aero.Contacts.Count(), 50);
                Assert.Equal(aero.Vendors.Count(), 10);
                Assert.Equal(aero.Parts.Count(), 300);
            }
        }
    }
}
