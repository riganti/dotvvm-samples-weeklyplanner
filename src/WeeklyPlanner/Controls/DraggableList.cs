using DotVVM.Framework.Binding;
using System;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;

namespace WeeklyPlanner.Controls
{
    /// <summary>
    /// A list of items with drag &amp; drop support.
    /// </summary>
    [ControlMarkupOptions(AllowContent = false, DefaultContentProperty = nameof(ItemTemplate))]
    public class DraggableList : ItemsControl
    {

        /// <summary>
        /// Gets or sets the command triggered when the item is dropped.
        /// </summary>
        [ControlPropertyBindingDataContextChange(nameof(DataSource))]
        [CollectionElementDataContextChange(1)]
        public ICommandBinding ItemDropped
        {
            get { return (ICommandBinding)GetValue(ItemDroppedProperty); }
            set { SetValue(ItemDroppedProperty, value); }
        }
        public static readonly DotvvmProperty ItemDroppedProperty
            = DotvvmProperty.Register<ICommandBinding, DraggableList>(c => c.ItemDropped, null);

        /// <summary>
        /// Gets or sets the maximum number of items that can be added in the collection.
        /// </summary>
        public int MaxItemsCount
        {
            get { return (int)GetValue(MaxItemsCountProperty); }
            set { SetValue(MaxItemsCountProperty, value); }
        }
        public static readonly DotvvmProperty MaxItemsCountProperty
            = DotvvmProperty.Register<int, DraggableList>(c => c.MaxItemsCount, 0);

        /// <summary>
        /// Gets or sets the name of the group of the draggable items. If you have multiple DraggableList controls with the same GroupName, 
        /// the items can be moved between these lists.
        /// </summary>
        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }
        public static readonly DotvvmProperty GroupNameProperty
            = DotvvmProperty.Register<string, DraggableList>(c => c.GroupName, "");


        /// <summary>
        /// Gets or sets the template for the items in the collection.
        /// </summary>
        [MarkupOptions(AllowBinding = false, MappingMode = MappingMode.InnerElement, Required = true)]
        [ControlPropertyBindingDataContextChange(nameof(DataSource))]
        [CollectionElementDataContextChange(1)]
        public ITemplate ItemTemplate
        {
            get { return (ITemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        public static readonly DotvvmProperty ItemTemplateProperty
            = DotvvmProperty.Register<ITemplate, DraggableList>(c => c.ItemTemplate, null);


        public DraggableList() : base("div")
        {
        }

        protected override void OnPreRender(IDotvvmRequestContext context)
        {
            context.ResourceManager.AddRequiredResource("draggableList");
            base.OnPreRender(context);
        }

        protected override void AddAttributesToRender(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            writer.AddKnockoutDataBind("draggable-list", BuildControlKnockoutBinding());
            writer.AddKnockoutForeachDataBind($"DraggableList.getDataSourceFromExpression({GetDataSourceBinding().GetKnockoutBindingExpression(this)})");

            writer.AddAttribute("class", "draggable-list", true);
            
            base.AddAttributesToRender(writer, context);
        }

        protected KnockoutBindingGroup BuildControlKnockoutBinding()
        {
            var binding = new KnockoutBindingGroup();

            binding.Add("maxItemsCount", this, MaxItemsCountProperty, () =>
            {
                binding.Add("maxItemsCount", MaxItemsCount.ToString());
            });

            binding.Add("groupName", this, GroupNameProperty, () =>
            {
                binding.Add("groupName", KnockoutHelper.MakeStringLiteral(GroupName));
            });

            if (HasBinding(ItemDroppedProperty))
            {
                binding.Add("onItemDropped", KnockoutHelper.GenerateClientPostBackScript(nameof(ItemDropped), GetCommandBinding(ItemDroppedProperty), this));
            }

            return binding;
        }


        protected override void RenderContents(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            writer.AddAttribute("class", "draggable-list-item", true);
            writer.RenderBeginTag("div");

            var placeholder = new DataItemContainer() { DataContext = null };
            placeholder.SetValue(Internal.PathFragmentProperty, GetPathFragmentExpression() + "/[$index]");
            placeholder.SetValue(Internal.ClientIDFragmentProperty, "$index()");
            ItemTemplate.BuildContent(context, placeholder);
            Children.Add(placeholder);
            placeholder.Render(writer, context);

            writer.RenderEndTag();
        }

    }
}
