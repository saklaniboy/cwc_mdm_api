﻿using System.Data;
using System.Reflection;

namespace cwcerp.MDM_Models.Uitlity
{
    public class ListtoDataTableConverter

    {

        public DataTable ToDataTable<T>(List<T> items)
        {

            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)

            {
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)

            {

                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }

                dataTable.Rows.Add(values);

            }
            return dataTable;

        }

    }

}
