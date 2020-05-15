﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GKit {
	public static class ReflectionUtility {
		public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class, IComparable{
			List<T> objects = new List<T>();
			foreach (Type type in
				Assembly.GetAssembly(typeof(T)).GetTypes()
				.Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))) {
				objects.Add((T)Activator.CreateInstance(type, constructorArgs));
			}
			objects.Sort();
			return objects;
		}
	}
}
