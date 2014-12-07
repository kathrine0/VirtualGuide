using System;
using System.Collections.Generic;

namespace VirtualGuide.Mobile.Helper
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
