using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualGuide.Services
{
    public class ServicesHelper
    {
        public static List<TViewModel> CreateViewModelListFromModel<TViewModel, TModel>(IList<TModel> list)
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
