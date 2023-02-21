﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mgr.Core.Extensions;
public static class DataTableExtensions
{
    /// <summary>
    /// Map a DataTable to Models
    /// </summary>
    /// <typeparam name="T">Type of Model</typeparam>
    /// <param name="dataTable"></param>
    /// <returns>List of Models</returns>
    public static List<T> ToObjects<T>(this DataTable dataTable) where T : new()
    {
        var result = new List<T>();
        var setProps = typeof(T).GetProperties().Where(p => p.CanWrite).ToList();

        foreach (DataRow row in dataTable.Rows)
        {
            var oRow = new T();
            foreach (var setProp in setProps)
            {
                var colVal = row[setProp.Name];
                if (colVal != DBNull.Value)
                    setProp.SetValue(oRow, colVal);
            }
            result.Add(oRow);
        }
        return result;
    }
}
