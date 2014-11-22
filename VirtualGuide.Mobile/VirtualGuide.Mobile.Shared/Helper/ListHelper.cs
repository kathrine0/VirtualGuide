using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;

namespace VirtualGuide.Mobile.Helper
{
    public static class ListHelper
    {
        /// <summary>
        /// Groups and sorts into a list of group lists based on a selector.
        /// </summary>
        /// <typeparam name="TSource">Type of the items in the list.</typeparam>
        /// <typeparam name="TSort">Type of value returned by sortSelector.</typeparam>
        /// <typeparam name="TGroup">Type of value returned by groupSelector.</typeparam>
        /// <param name="source">List to be grouped and sorted</param>
        /// <param name="sortSelector">A selector that provides the value that items will be sorted by.</param>
        /// <param name="groupSelector">A selector that provides the value that items will be grouped by.</param>
        /// <param name="isSortDescending">Value indicating to sort groups in reverse. Items in group will still sort ascending.</param>
        /// <returns>A list of JumpListGroups.</returns>
        public static List<ListGroup<TSource>> ToGroups<TSource, TSort, TGroup>(
            this IEnumerable<TSource> source, Func<TSource, TSort> sortSelector,
            Func<TSource, TGroup> groupSelector, bool isSortDescending = false)
        {
            var groups = new List<ListGroup<TSource>>();

            // Group and sort items based on values returned from the selectors
            var query = from item in source
                        orderby groupSelector(item), sortSelector(item)
                        group item by groupSelector(item) into g
                        select new { GroupName = g.Key, Items = g };

            // For each group generated from the query, create a ListGroup
            // and fill it with its items
            foreach (var g in query)
            {
                ListGroup<TSource> group = new ListGroup<TSource>();
                group.Key = g.GroupName;
                foreach (var item in g.Items)
                    group.Add(item);

                if (isSortDescending)
                    groups.Insert(0, group);
                else
                    groups.Add(group);
            }

            return groups;
        }

        public static ObservableCollection<ListGroup<TSource>> ToObservableGroups<TSource, TSort, TGroup>(
            this IEnumerable<TSource> source, Func<TSource, TSort> sortSelector,
            Func<TSource, TGroup> groupSelector, bool isSortDescending = false)
        {
            var groups = new ObservableCollection<ListGroup<TSource>>();

            // Group and sort items based on values returned from the selectors
            var query = from item in source
                        orderby groupSelector(item), sortSelector(item)
                        group item by groupSelector(item) into g
                        select new { GroupName = g.Key, Items = g };

            // For each group generated from the query, create a ListGroup
            // and fill it with its items
            foreach (var g in query)
            {
                ListGroup<TSource> group = new ListGroup<TSource>();
                group.Key = g.GroupName;
                foreach (var item in g.Items)
                    group.Add(item);

                if (isSortDescending)
                    groups.Insert(0, group);
                else
                    groups.Add(group);
            }

            return groups;
        }

    }

    public class ListGroup<T> : List<object>
    {
        /// <summary>
        /// Key that represents the group of objects and used as group header.
        /// </summary>
        public object Key { get; set; }

        public new IEnumerator<object> GetEnumerator()
        {
            return (System.Collections.Generic.IEnumerator<object>)base.GetEnumerator();
        }
    }
}
