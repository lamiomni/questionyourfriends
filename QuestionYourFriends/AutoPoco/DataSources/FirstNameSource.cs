using System;
using AutoPoco.Engine;

namespace AutoPoco.DataSources
{
    public class FirstNameSource : DatasourceBase<String>
    {
        private static readonly string[] FirstNames = new[]
                                                          {
                                                              "Jack",
                                                              "Thomas",
                                                              "Oliver",
                                                              "Joshua",
                                                              "Harry",
                                                              "Charlie",
                                                              "Daniel",
                                                              "William",
                                                              "James",
                                                              "Alfie",
                                                              "Samuel",
                                                              "George",
                                                              "Joseph",
                                                              "Benjamin",
                                                              "Ethan",
                                                              "Lewis",
                                                              "Mohammed",
                                                              "Jake",
                                                              "Dylan",
                                                              "Jacob",
                                                              "Ruby",
                                                              "Olivia",
                                                              "Grace",
                                                              "Emily",
                                                              "Jessica",
                                                              "Chloe",
                                                              "Lily",
                                                              "Mia",
                                                              "Lucy",
                                                              "Amelia",
                                                              "Evie",
                                                              "Ella",
                                                              "Katie",
                                                              "Ellie",
                                                              "Charlotte",
                                                              "Summer",
                                                              "Mohammed",
                                                              "Megan",
                                                              "Hannah",
                                                              "Ava"
                                                          };

        private readonly Random mRandom = new Random(1337);

        public override string Next(IGenerationContext context)
        {
            return FirstNames[mRandom.Next(0, FirstNames.Length)];
        }
    }
}