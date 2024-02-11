using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Systems.Inventory {
    public static class UQueryBuilderExtensions {
        public static IEnumerable<T> OrderBy<T, TKey>(this UQueryBuilder<T> query, Func<T, TKey> keySelector, Comparer<TKey> @default) where T : VisualElement {
            return query.ToList().OrderBy(keySelector, @default);
        }

        public static IEnumerable<T> OrderByDistance<T>(this UQueryBuilder<T> query, Func<T, float> keySelector) where T : VisualElement {
            return query.OrderBy(keySelector, Comparer<float>.Default);
        }
        
        public static T FirstOrDefault<T>(this UQueryBuilder<T> query) where T : VisualElement {
            return query.ToList().FirstOrDefault();
        }
    }
}