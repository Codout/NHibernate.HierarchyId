using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.HierarchyId.Util;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace NHibernate.HierarchyId.Projections;

public class ToHierarchyIdProjection : SimpleProjection
{
    private readonly IProjection _projection;


    public ToHierarchyIdProjection(IProjection projection)
    {
        _projection = projection;
    }

    public override SqlString ToSqlString(ICriteria criteria, int position, ICriteriaQuery criteriaQuery)
    {
        var loc = position * GetHashCode();
        var val = _projection.ToSqlString(criteria, loc, criteriaQuery);
        val = StringHelper.RemoveAsAliasesFromSql(val);

        var ret = new SqlStringBuilder()
            .Add("hierarchyid::Parse( ")
            .Add(val)
            .Add(" )")
            .Add(" as ")
            .Add(GetColumnAlias(position))
            .ToSqlString();

        return ret;
    }

    public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
        return new IType[] { NHibernateUtil.String };
    }

    public override bool IsGrouped => _projection.IsGrouped;

    public override bool IsAggregate => false;

    public override SqlString ToGroupSqlString(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
        return _projection.ToGroupSqlString(criteria, criteriaQuery);
    }
}
