using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Lost
{
    public class UIPlane:MonoBehaviour
    {
        private object _dataContext;
        public object DataContext {
            get { return _dataContext; }
            private set{
                INotifyPropertyChanged propertyChanged = null;
                INotifyPropertyChanging propertyChanging = null;
                if (_dataContext!= value)
                {
                    propertyChanged = (INotifyPropertyChanged)_dataContext;
                    if (propertyChanged != null) propertyChanged.PropertyChanged -= OnPropertyChanged;
                    propertyChanging = (INotifyPropertyChanging)_dataContext;
                    if (propertyChanging != null) propertyChanging.PropertyChanging -= OnPropertyChanging;
                }
                _dataContext = value;
                propertyChanged = (INotifyPropertyChanged)_dataContext;
                if(propertyChanged!= null) propertyChanged.PropertyChanged += OnPropertyChanged;
                propertyChanging = (INotifyPropertyChanging) _dataContext;
                if (propertyChanging!= null) propertyChanging.PropertyChanging += OnPropertyChanging;
            }
        }

        protected virtual void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }
    }
    
}