using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace Borigran.OneData.Platform.NHibernate.Repository
{
    public class AliasDefinition
    {
        public AliasDefinition(string path, string alias, JoinType joinType = JoinType.InnerJoin)
        {
            Path = path;
            Alias = alias;
            JoinType = joinType;
        }

        public string Path { get; set; }
        public string Alias { get; set; }
        public JoinType JoinType { get; set; }
    }
}
