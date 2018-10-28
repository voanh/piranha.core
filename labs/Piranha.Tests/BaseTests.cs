using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Xunit;


namespace Piranha.Tests
{
    public abstract class BaseTests : IDisposable
    {
        private static Lazy<IDocumentStore> _store = new Lazy<IDocumentStore>(CreateStore);
        protected static IDocumentStore Store => _store.Value;

        private static IDocumentStore CreateStore()
        {
            IDocumentStore store = new DocumentStore()
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "piranha"
            }.Initialize();

            return store;
        }

        public virtual void Dispose()
        {
            //Store.Dispose();
        }
    }
}