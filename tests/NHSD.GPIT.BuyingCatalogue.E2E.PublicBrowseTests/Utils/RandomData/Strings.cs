﻿using Bogus;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Utils.RandomData
{
    internal static class Strings
    {
        internal static string RandomString(int numChars)
        {
            var faker = new Faker("en_GB");
            return string.Join(string.Empty, faker.Random.AlphaNumeric(numChars));
        }

        internal static string RandomUrl(int numChars)
        {
            var faker = new Faker("en_GB");
            var url = faker.Internet.Url();
            return string.Join(string.Empty, url, "/", faker.Random.AlphaNumeric(numChars - url.Length - 1));
        }
    }
}
