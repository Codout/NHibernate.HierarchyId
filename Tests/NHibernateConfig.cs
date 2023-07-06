﻿using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Driver;
using NHibernate.HierarchyId;

namespace Tests
{
    public class NHibernateConfig
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly Configuration _config;

        public NHibernateConfig(string connectionString)
        {
            _config = Fluently.Configure()
                        .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString).Driver<MicrosoftDataSqlClientDriver>)
                        .Mappings(m => m.FluentMappings.AddFromAssembly(typeof(NHibernateConfig).Assembly))                        
                        .Diagnostics(d => d.OutputToConsole().Enable())                        
                        .BuildConfiguration();                        

            // register HierarchyId extensions for NH
            HierarchyIdExtensions.RegisterHierarchySupport(_config);

            _sessionFactory = _config.BuildSessionFactory();
        }

        public ISessionFactory SessionFactory => _sessionFactory;

        public ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public IStatelessSession OpenStatelessSession()
        {
            return SessionFactory.OpenStatelessSession();
        }

        public void UpdateDatabase(bool update)
        {
            using var s = OpenStatelessSession();
            var schemaExport = new NHibernate.Tool.hbm2ddl.SchemaExport(_config);
            schemaExport.Execute(false, update, false, s.Connection, Console.Out);
        }
    }
}
