using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StringFormatter.Core;

public class Cache
{

    private readonly ConcurrentDictionary<string, Func<object, string>> _cacheTable = new();

    public string TryHitCache(object formatterObject, string processedArgument)
    {
        string tableKey = $"{formatterObject.GetType()}.{processedArgument}";
        if (_cacheTable.TryGetValue(tableKey, out var funcResultDelegate))
            return funcResultDelegate(formatterObject);
        else
        {
            var exprTreeLambda = Build(formatterObject.GetType(), processedArgument);
            funcResultDelegate = exprTreeLambda.Compile();
            _cacheTable.TryAdd(tableKey, funcResultDelegate);
            return funcResultDelegate(formatterObject);
        }
    }

    public Expression<Func<object, string>> Build(Type formatterObjectType, string processedArgument)
    {
        char[] separator = { '[', ']' };
        string[] fieldWithIndex = processedArgument.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        bool doesContainIndex = fieldWithIndex.Length == 2;
        string field = fieldWithIndex[0];
        string index = doesContainIndex ? fieldWithIndex[1] : String.Empty;

        PropertyInfo[] properties = formatterObjectType.GetProperties();
        FieldInfo[] fields = formatterObjectType.GetFields();

        if (properties.Where(p => p.Name == field).Any() || fields.Where(f => f.Name == field).Any())
        {
            var paramNode = Expression.Parameter(typeof(object), "target");
            var targetExpr = Expression.TypeAs(paramNode, formatterObjectType);
            Expression instanceExpression;

            if (doesContainIndex)
            {
                MemberExpression member = Expression.PropertyOrField(targetExpr, field);
                instanceExpression = Expression.ArrayAccess(member,
                    Expression.Constant(int.Parse(index), typeof(int)));
            }
            else
                instanceExpression = Expression.PropertyOrField(targetExpr, field);

            var instanceToString = Expression.Call(instanceExpression, "ToString", null, null);
            return Expression.Lambda<Func<object, string>>(instanceToString, paramNode);
        }
        else
            throw new ArgumentException($"No field/property \"{processedArgument}\" found in \"{formatterObjectType}\"");
    }

}