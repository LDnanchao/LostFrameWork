using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lost
{
    public class UIManager
    {
        //ViewModel IOC
        private Dictionary<Type, object> _viewModelDict = new Dictionary<Type, object>();
        public void RegisterViewModel(Type viewModelType, object viewModelInstance)
        {
            _viewModelDict[viewModelType] = viewModelInstance;
        }
        public void RegisterViewModel<TViewModel>(TViewModel viewModelInstance) where TViewModel : class
        {
            _viewModelDict[typeof(TViewModel)] = viewModelInstance;
        }
        public TVViewModel GetViewModel<TVViewModel>() where TVViewModel : class
        {
            return (TVViewModel)_viewModelDict[typeof(TVViewModel)];
        }
        public void TryGetViewModel<TVViewModel>(out TVViewModel viewModel) where TVViewModel : class
        {
            _viewModelDict.TryGetValue(typeof(TVViewModel), out  object obj);
            viewModel = obj as TVViewModel;
        }
        //打开，关闭，卸载
    }
}
