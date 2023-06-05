﻿using NUnit.Framework;
using SparkChange.Domain;

namespace SparkChange.Tests.Helpers
{
    public abstract class AbstractDbTests
    {
        protected DatabaseContext databaseContext;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            databaseContext = DbHelper.CreateDatabaseContext();

            databaseContext.Products.AddRange(UnitTestHelpers.GetProducts());
            databaseContext.Baskets.AddRange(UnitTestHelpers.GetBaskets());
            databaseContext.BasketItems.AddRange(UnitTestHelpers.GetBasketsItems());
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            databaseContext.Database.EnsureDeleted();
            databaseContext.Dispose();
        }
    }
}