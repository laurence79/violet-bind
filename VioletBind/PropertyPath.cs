using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VioletBind
{
    /// <summary>
    /// Property path.
    /// </summary>
    public class PropertyPath : IReadOnlyList<PropertyInfo>
    {
        private readonly List<PropertyInfo> _propertyInfos;

        /// <summary>
        /// Initializes a new instance of the <see cref="VioletBind.PropertyPath"/> class.
        /// </summary>
        /// <param name="properties">Properties.</param>
        public PropertyPath(params PropertyInfo[] properties)
        {
            _propertyInfos = new List<PropertyInfo>(properties);
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count => _propertyInfos.Count;

        /// <summary>
        /// Gets the <see cref="VioletBind.PropertyPath"/> at the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        public PropertyInfo this[int index] => _propertyInfos[index];

        /// <summary>
        /// Reduce the specified paths.
        /// </summary>
        /// <returns>The reduce.</returns>
        /// <param name="paths">Paths.</param>
        public static IReadOnlyList<PropertyPath> Reduce(IEnumerable<PropertyPath> paths)
        {
            var inputList = paths
                .ToList();

            int index = 0;
            while (index < inputList.Count)
            {
                var currentItem = inputList[index];
                var currentPath = currentItem.ToString();

                if (inputList.Any(
                    item => !object.ReferenceEquals(currentItem, item)
                        && item.ToString().StartsWith(currentPath, StringComparison.InvariantCulture)))
                {
                    inputList.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }

            return inputList;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<PropertyInfo> GetEnumerator()
        {
            return ((IReadOnlyList<PropertyInfo>)_propertyInfos).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IReadOnlyList<PropertyInfo>)_propertyInfos).GetEnumerator();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="VioletBind.PropertyPath"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="VioletBind.PropertyPath"/>.</returns>
        public override string ToString()
        {
            return string.Join(".", this.Select(p => p.Name));
        }
    }
}
