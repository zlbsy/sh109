using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.ViewModel;

namespace App.View{
	public interface IView
	{
		VMBase BindingContext { get; set; }
	}
	public class VBase : MonoBehaviour, IView {
		public readonly VMProperty<VMBase> vmProperty = new VMProperty<VMBase>();
        private App.Controller.Common.CBase _controller;
		public VMBase BindingContext
		{
			get { return vmProperty.Value; }
			set { vmProperty.Value = value; }
		}
		protected virtual void OnBindingContextChanged(VMBase oldViewModel, VMBase newViewModel)
		{
		}
		public VBase()
		{
			this.vmProperty.OnValueChanged += OnBindingContextChanged;
		}
        public App.Controller.Common.CBase Controller{
            get{ 
                if (_controller == null)
                {
                    _controller = this.GetComponentInParent<App.Controller.Common.CBase>();
                }
                return _controller;
            }
        }
        public void ClearChild()
        {
            Util.Global.ClearChild(this.gameObject);
        }
        public virtual void UpdateView(){
        
        }
        public virtual void UpdateView(App.Model.MBase model){

        }
        public virtual void OnDestroy(){
            BindingContext = null;
        }
	}
}