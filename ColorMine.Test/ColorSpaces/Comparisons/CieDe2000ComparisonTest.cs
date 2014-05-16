using System;
using System.Collections.Generic;
using ColorMine.ColorSpaces;
using ColorMine.ColorSpaces.Comparisons;
using ColorMine.ColorSpaces.Conversions.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ColorMine.Test.ColorSpaces.Comparisons
{
    public class CieDe2000ComparisonTest
    {
        [TestClass]
        public class Compare
        {
            private IDictionary<KeyValuePair<ILab, ILab>, double> TestData { get; set; }

            [TestInitialize]
            public void Initialize()
            {
                TestData = new Dictionary<KeyValuePair<ILab, ILab>, double>();
                var reader =
                    new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory +
                                               @"/TestData/CieDe2000TestData.dat");
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    if (line.StartsWith("//")) continue;

                    var parts = line.Split('\t');
                    var a = new Lab
                        {
                            L = Double.Parse(parts[0]),
                            A = Double.Parse(parts[1]),
                            B = Double.Parse(parts[2])
                        };
                    var b = new Lab
                        {
                            L = Double.Parse(parts[3]),
                            A = Double.Parse(parts[4]),
                            B = Double.Parse(parts[5])
                        };
                    var input = new KeyValuePair<ILab, ILab>(a, b);
                    var expected = Double.Parse(parts[6]);
                    TestData[input] = expected;
                }
            }

            private void ReturnsExpectedValueForKnownInput(double expectedValue, IColorSpace a, IColorSpace b)
            {
                var target = new CieDe2000Comparison();
                var actualValue = a.Compare(b, target);
                Assert.IsTrue(expectedValue.BasicallyEqualTo(actualValue), expectedValue + " != " + actualValue);
            }

            [TestMethod]
            public void ReturnsExpectedValueForKnownInputs()
            {
                foreach (var test in TestData)
                {
                    ReturnsExpectedValueForKnownInput(test.Value, test.Key.Key, test.Key.Value);
                }
            }
        }
    }
}
