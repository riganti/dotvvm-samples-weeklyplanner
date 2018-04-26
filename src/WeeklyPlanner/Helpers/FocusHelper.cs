using DotVVM.Framework.Binding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;

namespace WeeklyPlanner.Helpers
{
    [ContainsDotvvmProperties]
    public class FocusHelper
    {

        [MarkupOptions(AllowHardCodedValue = false)]
        [AttachedProperty(typeof(object))]
        public static readonly ActiveDotvvmProperty HasFocusProperty =
            DelegateActionProperty<object>.Register<FocusHelper>("HasFocus", AddHasFocusBinding);


        private static void AddHasFocusBinding(IHtmlWriter writer, IDotvvmRequestContext context, DotvvmProperty property, DotvvmControl control)
        {
            writer.AddKnockoutDataBind("hasFocus", control, property);
        }
    }
}
