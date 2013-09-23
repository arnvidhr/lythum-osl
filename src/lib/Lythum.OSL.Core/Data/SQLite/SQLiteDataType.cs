using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lythum.OSL.Core.Data.SQLite
{
    /// <summary>
    /// According to http://sqlite.org/datatype3.html
    /// </summary>
    public enum SQLiteDataType
    {
        INT,
        INTEGER,
        TINYINT,
        SMALLINT,
        MEDIUMINT,
        BIGINT,
        UNSIGNED_BIG_INT,
        INT2,
        INT8,

        TEXT,

        BLOB,

        REAL,
        DOUBLE,
        DOUBLE_PRECISION,
        FLOAT,

        NUMERIC,
        DECIMAL_10_5,
        DECIMAL_10_4,   // Accounting
        DECIMAL_10_3,   // Half precision
        DECIMAL_10_2,   // Money
        BOOLEAN,
        DATE,
        DATETIME,
    }
}
