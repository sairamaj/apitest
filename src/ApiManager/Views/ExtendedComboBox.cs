﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ApiManager.Views
{
	class ExtendedComboBox : ComboBox
	{
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Key == Key.Escape)
			{
				MessageBox.Show("escape done");
			}
		}
	}
}
