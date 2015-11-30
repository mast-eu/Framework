﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Data.Sql.Columns
{
    public class ForeignKeyColumn<T> : IColumn<T>, IReadableColumn<T>, IInsertableColumn<T>, IUpdateableColumn<T>
    {
        public IColumnPath Path { get; private set; }

        public ForeignKeyColumn(IColumnPath path)
        {
            Ensure.NotNull(path, "path");
            Path = path;
        }
    }
}