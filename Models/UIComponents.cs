using System;
using System.Collections.Generic;

namespace UIBuilderApp.Models
{
    public class ButtonComponent : UIComponentBase
    {
        public override string ComponentType => "Button";

        public ButtonComponent()
        {
            Properties["Text"] = "Button";
            Properties["CssClass"] = "btn btn-primary";
            Width = 120;
            Height = 40;
        }
    }

    public class TextBoxComponent : UIComponentBase
    {
        public override string ComponentType => "TextBox";

        public TextBoxComponent()
        {
            Properties["Placeholder"] = "Enter text...";
            Properties["Value"] = "";
            Width = 200;
            Height = 40;
        }
    }

    public class LabelComponent : UIComponentBase
    {
        public override string ComponentType => "Label";

        public LabelComponent()
        {
            Properties["Text"] = "Label";
            Properties["CssClass"] = "form-label";
            Width = 150;
            Height = 30;
        }
    }

    public class DropdownComponent : UIComponentBase
    {
        public override string ComponentType => "Dropdown";

        public DropdownComponent()
        {
            Properties["Options"] = "Option 1,Option 2,Option 3";
            Width = 200;
            Height = 40;
        }
    }

    public class CheckboxComponent : UIComponentBase
    {
        public override string ComponentType => "Checkbox";

        public CheckboxComponent()
        {
            Properties["Text"] = "Checkbox";
            Properties["Checked"] = "false";
            Width = 150;
            Height = 30;
        }
    }

    public class RadioButtonComponent : UIComponentBase
    {
        public override string ComponentType => "RadioButton";

        public RadioButtonComponent()
        {
            Properties["Text"] = "Radio Button";
            Properties["GroupName"] = "group1";
            Properties["Checked"] = "false";
            Width = 150;
            Height = 30;
        }
    }

    public class ImageComponent : UIComponentBase
    {
        public override string ComponentType => "Image";

        public ImageComponent()
        {
            Properties["Src"] = "https://via.placeholder.com/150";
            Properties["Alt"] = "Image";
            Width = 150;
            Height = 150;
        }
    }

    public class ContainerComponent : UIComponentBase
    {
        public override string ComponentType => "Container";
        
        public List<UIComponentBase> Children { get; set; } = new List<UIComponentBase>();

        public ContainerComponent()
        {
            Properties["Border"] = "1px solid #ccc";
            Properties["Background"] = "#f8f9fa";
            Width = 300;
            Height = 200;
        }
    }
} 