using System.Linq;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.HierarchyId.Util;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace NHibernate.HierarchyId.Projections;

public class GetReparentedValueProjection : SimpleProjection
{
    private readonly IProjection _projection;
    private readonly string _oldRoot, _newRoot;

    public GetReparentedValueProjection(IProjection projection, string oldRoot, string newRoot)
    {
        _projection = projection;
        _oldRoot = oldRoot;
        _newRoot = newRoot;
    }

    public override SqlString ToSqlString(ICriteria criteria, int position, ICriteriaQuery criteriaQuery)
    {
        var loc = position * GetHashCode();
        var val = _projection.ToSqlString(criteria, loc, criteriaQuery);
        val = StringHelper.RemoveAsAliasesFromSql(val);

        var lhs = new SqlStringBuilder();

        lhs.Add(val);
        lhs.Add(".GetReparentedValue(");
        lhs.Add(criteriaQuery.NewQueryParameter(new TypedValue(NHibernateUtil.String, _oldRoot, false)).Single());
        lhs.Add(" , ");
        lhs.Add(criteriaQuery.NewQueryParameter(new TypedValue(NHibernateUtil.String, _newRoot, false)).Single());
        lhs.Add(") as ");
        lhs.Add(GetColumnAlias(position));

        var ret = lhs.ToSqlString();

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
