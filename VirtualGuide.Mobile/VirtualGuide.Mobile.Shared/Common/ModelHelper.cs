using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualGuide.Mobile.Common
{
    public class ModelHelper
    {
        public static List<TViewModel> ObjectToViewModel<TViewModel, TModel>(IEnumerable<TModel> list)

        {
            var result = new List<TViewModel>();

            foreach (var item in list)
            {
                result.Add((TViewModel)Activator.CreateInstance(typeof(TViewModel), item));
            }

            return result;
        }
    }
}
