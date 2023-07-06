using NHibernate.SqlCommand;

namespace NHibernate.HierarchyId.Util;

public static class StringHelper
{
    public static SqlString RemoveAsAliasesFromSql(SqlString sql)
    {
        return sql.Substring(0, sql.LastIndexOfCaseInsensitive(" as "));
    }
}