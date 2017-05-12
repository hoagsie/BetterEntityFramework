using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace BetterEntityFramework.DataReaders
{
    internal class QueryableDataReader<T> : DbDataReader
    {
        private readonly IQueryable<T> _queryable;
        private readonly IEnumerator<T> _enumerator;
        private readonly PropertyInfo[] _properties;
        private T _current;
        private int _recordCount;

        public QueryableDataReader(IQueryable<T> queryable)
        {
            _queryable = queryable;
            _enumerator = _queryable.GetEnumerator();
            _properties = _queryable.ElementType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            _recordCount = 0;
        }

        public override string GetName(int ordinal)
        {
            return _properties[ordinal].Name;
        }

        public override int GetValues(object[] values)
        {
            values = _properties.Select(info => info.GetValue(_current)).ToArray();
            return values.Length;
        }

        public override bool IsDBNull(int ordinal)
        {
            return DBNull.Value == _properties[ordinal].GetValue(_current);
        }

        public override int FieldCount => _properties.Length;

        public override object this[int ordinal] => _properties[ordinal].GetValue(_current);

        public override object this[string name]
        {
            get
            {
                var property = _properties.Single(info => info.Name == name);
                return property.GetValue(_current);
            }
        }

        public override bool HasRows => _recordCount > 0 || !_current.Equals(default(T)) || NextResult();

        public override bool IsClosed => false;

        public override int RecordsAffected => _recordCount;

        public override bool NextResult()
        {
            var next = _enumerator.MoveNext();

            if (next)
            {
                _current = _enumerator.Current;
                _recordCount++;
            }

            return next;
        }

        public override bool Read()
        {
            var next = _enumerator.MoveNext();

            if (next)
            {
                _current = _enumerator.Current;
                _recordCount++;
            }

            return next;
        }

        public override int Depth => 1;

        public override int GetOrdinal(string name)
        {
            for (var index = 0; index < _properties.Length; index++)
            {
                if (_properties[index].Name == name)
                {
                    return index;
                }
            }

            return -1;
        }

        public override bool GetBoolean(int ordinal)
        {
            return Convert.ToBoolean(this[ordinal]);
        }

        public override byte GetByte(int ordinal)
        {
            return Convert.ToByte(this[ordinal]);
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            var bufferObject = buffer.OfType<object>().ToArray();
            var copied = PushToBuffer(ordinal, dataOffset, bufferObject, bufferOffset, length);
            buffer = bufferObject.OfType<byte>().ToArray();
            return copied;
        }

        public override char GetChar(int ordinal)
        {
            return Convert.ToChar(this[ordinal]);
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            var bufferObject = buffer.OfType<object>().ToArray();
            var copied = PushToBuffer(ordinal, dataOffset, bufferObject, bufferOffset, length);
            buffer = bufferObject.OfType<char>().ToArray();
            return copied;
        }

        private long PushToBuffer(int ordinal, long dataOffset, object[] buffer, int bufferOffset, int length)
        {
            var formatter = new BinaryFormatter();
            byte[] ordinalBytes;
            using (var memory = new MemoryStream())
            {
                formatter.Serialize(memory, this[ordinal]);
                ordinalBytes = memory.ToArray();
            }

            Array.Copy(ordinalBytes, dataOffset, buffer, bufferOffset, length);

            var ordinalTotal = ordinalBytes.LongLength - dataOffset;
            return ordinalTotal > length ? length : ordinalTotal;
        }

        public override Guid GetGuid(int ordinal)
        {
            return Guid.Parse(this[ordinal].ToString());
        }

        public override short GetInt16(int ordinal)
        {
            return Convert.ToInt16(this[ordinal]);
        }

        public override int GetInt32(int ordinal)
        {
            return Convert.ToInt32(this[ordinal]);
        }

        public override long GetInt64(int ordinal)
        {
            return Convert.ToInt64(this[ordinal]);
        }

        public override DateTime GetDateTime(int ordinal)
        {
            return DateTime.Parse(this[ordinal].ToString());
        }

        public override string GetString(int ordinal)
        {
            return Convert.ToString(this[ordinal]);
        }

        public override decimal GetDecimal(int ordinal)
        {
            return Convert.ToDecimal(this[ordinal]);
        }

        public override double GetDouble(int ordinal)
        {
            return Convert.ToDouble(this[ordinal]);
        }

        public override float GetFloat(int ordinal)
        {
            return float.Parse(this[ordinal].ToString());
        }

        public override string GetDataTypeName(int ordinal)
        {
            return _properties[ordinal].PropertyType.FullName;
        }

        public override Type GetFieldType(int ordinal)
        {
            return _properties[ordinal].PropertyType;
        }

        public override object GetValue(int ordinal)
        {
            return this[ordinal];
        }

        public override IEnumerator GetEnumerator()
        {
            return _enumerator;
        }
    }
}
