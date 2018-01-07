using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using App.View;
using App.Model;

namespace App.ViewModel{
	public class VMProperty<T> {
		public delegate void ValueChangedHandler(T oldValue, T newValue);
		public ValueChangedHandler OnValueChanged;
        protected T _value;
        public virtual T Value
		{
			get
			{
				return _value;
			}
			set
			{
                if (!object.Equals(_value, value))
				{
					T old = _value;
					_value = value;
					ValueChanged(old, _value);
				}
			}
		}

        protected void ValueChanged(T oldValue, T newValue)
        {
			if (OnValueChanged != null)
			{
				OnValueChanged(oldValue, newValue);
			}
		}

		public override string ToString()
		{
			return (Value != null ? Value.ToString() : "null");
		}
	}
}