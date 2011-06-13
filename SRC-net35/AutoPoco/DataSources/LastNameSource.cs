using System;
using AutoPoco.Engine;

namespace AutoPoco.DataSources
{
    public class LastNameSource : DatasourceBase<String>
    {
        private static readonly string[] SecondNames = new[]
                                                           {
                                                               "White",
                                                               "Black",
                                                               "Red",
                                                               "Gray",
                                                               "Magenta",
                                                               "Myrtle",
                                                               "Gold",
                                                               "Silver",
                                                               "Green",
                                                               "Puce",
                                                               "Carmine",
                                                               "Purple",
                                                               "Yellow",
                                                               "Indigo"
                                                           };

        private readonly Random mRandom = new Random(1337);

        public override string Next(IGenerationContext context)
        {
            return SecondNames[mRandom.Next(0, SecondNames.Length)];
        }
    }
}