using System.Collections.Generic;
using Tests.NhibernateMappings;

namespace Tests;

public abstract class TestsBase
{
    protected NHibernateConfig Config { get; private set; }

    protected Dictionary<string, IList<string>> Items { get; } = new();


    protected TestsBase()
    {
        Initialize();
    }

    private void Initialize()
    {
        // Chonfigure NH
        Config = new NHibernateConfig("Server=(localdb)\\MSSQLLocalDB;Integrated Security=true");

        // Create database structure
        Config.UpdateDatabase(true);

        // Create test entries
        PopulateData();
    }

    private void PopulateData()
    {
        using var s = Config.SessionFactory.OpenSession();

        using var t = s.BeginTransaction();

        for (var i = 0; i < 2; i++)
        {
            var p = new HierarchyModel
            {
                Hid = $"/{i + 1}/",
                Name = $"Parent {i + 1}"
            };

            s.Save(p);

            var childs = new List<string>();

            for (var j = 0; j < 10; j++)
            {
                var c = new HierarchyModel
                {
                    Hid = $"/{i + 1}/{j + 1}/",
                    Name = $"Child {i + 1}/{j + 1}"
                };

                childs.Add(c.Hid);
                s.Save(c);
            }

            Items.Add(p.Hid, childs);
        }

        t.Commit();
    }
}
