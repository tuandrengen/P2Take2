﻿using NUnit.Framework;
using System.Collections.Generic;
using P2SeriousGame;
using System.Windows.Forms;
using System.Drawing;

namespace Unittest
{
    [TestFixture]
    public class PersonTests
    {
        [TestCase("Player1")]
        public void Person_ValueGotRightFormat_ConstructedRight(string name)
        {
            Persons person = new Persons(name);
            Assert.AreEqual(name, person.Name);
        }
    }
}
