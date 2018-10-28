using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Xunit;
using Piranha.Models;

namespace Piranha.Tests
{
    public class ParamTests : BaseTests
    {
        private const string PARAM_1 = "MyFirstParam";
        private const string PARAM_2 = "MySecondParam";
        private const string PARAM_3 = "MyThirdParam";
        private const string PARAM_4 = "MyFourthParam";
        private const string PARAM_5 = "MyFifthParam";
        private string PARAM_1_ID = Guid.NewGuid().ToString();
        private string PARAM_1_VALUE = "My first value";

        public ParamTests()
        {
            using (var session = Store.OpenAsyncSession())
            {
                session.Advanced.WaitForIndexesAfterSaveChanges();

                var repo = new Piranha.Raven.Repositories.ParamRepository(session);

                Task.Run(async () => 
                {
                    await repo.Save(new Param() {
                        Id = PARAM_1_ID,
                        Key = PARAM_1,
                        Value = PARAM_1_VALUE
                    });

                    await repo.Save(new Param() {
                        Id = Guid.NewGuid().ToString(),
                        Key = PARAM_4,
                    });

                    await repo.Save(new Param() {
                        Id = Guid.NewGuid().ToString(),
                        Key = PARAM_5,
                    });
                }).Wait();
            }
        }

        [Fact]
        public async Task Add() {
            using (var session = Store.OpenAsyncSession())
            {
                session.Advanced.WaitForIndexesAfterSaveChanges();

                var repo = new Piranha.Raven.Repositories.ParamRepository(session);

                await repo.Save(new Param() {
                    Id = Guid.NewGuid().ToString(),
                    Key = PARAM_2,
                    Value = "My second value"
                });
            }
        }

        [Fact]
        public async Task GetNoneById() {
            using (var session = Store.OpenAsyncSession())
            {
                session.Advanced.WaitForIndexesAfterSaveChanges();

                var repo = new Piranha.Raven.Repositories.ParamRepository(session);
                
                var none = await repo.GetById(Guid.NewGuid().ToString());

                Assert.Null(none);
            }
        }

        [Fact]
        public async Task GetNoneByKey() {
            using (var session = Store.OpenAsyncSession())
            {
                session.Advanced.WaitForIndexesAfterSaveChanges();

                var repo = new Piranha.Raven.Repositories.ParamRepository(session);

                var none = await repo.GetByKey("none-existing-key");

                Assert.Null(none);
            }
        }


        [Fact]
        public async Task GetAll() {
            using (var session = Store.OpenAsyncSession())
            {
                session.Advanced.WaitForIndexesAfterSaveChanges();

                var repo = new Piranha.Raven.Repositories.ParamRepository(session);

                var models = await repo.GetAll();

                Assert.NotNull(models);
                Assert.NotEmpty(models);
            }
        }

        [Fact]
        public async Task GetById() {
            using (var session = Store.OpenAsyncSession())
            {
                session.Advanced.WaitForIndexesAfterSaveChanges();

                var repo = new Piranha.Raven.Repositories.ParamRepository(session);

                var model = await repo.GetById(PARAM_1_ID);

                Assert.NotNull(model);
                Assert.Equal(PARAM_1, model.Key);
            }
        }

        [Fact]
        public async Task GetByKey() {
            using (var session = Store.OpenAsyncSession())
            {
                session.Advanced.WaitForIndexesAfterSaveChanges();

                var repo = new Piranha.Raven.Repositories.ParamRepository(session);

                var model = await repo.GetByKey(PARAM_1);

                Assert.NotNull(model);
                Assert.Equal(PARAM_1, model.Key);
            }
        }

        [Fact]
        public async Task Update() {
            using (var session = Store.OpenAsyncSession())
            {
                session.Advanced.WaitForIndexesAfterSaveChanges();

                var repo = new Piranha.Raven.Repositories.ParamRepository(session);

                var model = await repo.GetById(PARAM_1_ID);

                Assert.Equal(PARAM_1_VALUE, model.Value);

                model.Value = "Updated";

                await repo.Save(model);
            }
        }

        [Fact]
        public async Task Delete() {
            using (var session = Store.OpenAsyncSession())
            {
                session.Advanced.WaitForIndexesAfterSaveChanges();

                var repo = new Piranha.Raven.Repositories.ParamRepository(session);

                var model = await repo.GetByKey(PARAM_4);

                Assert.NotNull(model);

                await repo.Delete(model.Id);
            }
        }

        public override void Dispose()
        {
            using (var session = Store.OpenAsyncSession())
            {
                session.Advanced.WaitForIndexesAfterSaveChanges();

                var repo = new Piranha.Raven.Repositories.ParamRepository(session);

                Task.Run(async () => 
                {
                    var models = await repo.GetAll();

                    foreach (var model in models)
                    {
                        await repo.Delete(model.Id);
                    }
                }).Wait();
            }

            base.Dispose();
        }
    }
}