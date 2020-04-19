using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.Firestore.Storage.Options
{
    public class CollectionConfiguration
    {
        public string Parent { get; }
        public string Name { get; }

        public CollectionConfiguration(string name)
        {
            Name = name;
        }

        public CollectionConfiguration(string name, string parent)
        {
            Name = name;
            Parent = parent;
        }
    }
}
