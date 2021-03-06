﻿using System;
using System.Collections;
using System.Threading.Tasks;

namespace SQLite
{
    public partial class SQLiteConnection : IDisposable
    {
        public int InsertOrReplaceAll(System.Collections.IEnumerable objects)
        {
            var c = 0;
            RunInTransaction(() =>
            {
                foreach (var r in objects)
                {
                    c += Insert(r, "OR REPLACE", r.GetType());
                }
            });
            return c;
        }

        public int InsertOrIgnoreAll(System.Collections.IEnumerable objects)
        {
            var c = 0;
            RunInTransaction(() =>
            {
                foreach (var r in objects)
                {
                    c += Insert(r, "OR IGNORE", r.GetType());
                }
            });
            return c;
        }
    }

    public partial class SQLiteAsyncConnection
    {
        public Task<int> InsertOrReplaceAllAsync(IEnumerable items)
        {
            return Task.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    return conn.InsertOrReplaceAll(items);
                }
            });
        }

        public Task<int> InsertOrReplaceAsync(object item)
        {
            return Task.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    return conn.InsertOrReplace(item);
                }
            });
        }

        public Task<int> InsertOrIgnoreAllAsync(IEnumerable items)
        {
            return Task.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    return conn.InsertOrIgnoreAll(items);
                }
            });
        }
    }
}
