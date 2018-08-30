﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Core.Logic.IcyWind
{
    internal static class HelperFunctions
    {
        internal static List<T> GetInstances<T>()
        {
            return (from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.BaseType == (typeof(T)) && t.GetConstructor(Type.EmptyTypes) != null
                select (T)Activator.CreateInstance(t)).ToList();
        }
    }
}
