using System;
using System.Collections.Generic;
using System.Linq;
using UIBuilderApp.Models;

namespace UIBuilderApp.Services
{
    /// <summary>
    /// Service for design-time UI operations
    /// </summary>
    public class DesignService
    {
        // Currently active form being edited
        public FormLayout CurrentForm { get; private set; }

        // Currently selected component
        public ComponentBase SelectedComponent { get; private set; }

        // Event raised when component selection changes
        public event Action<ComponentBase> OnComponentSelected;

        // Event raised when the form is modified
        public event Action OnFormModified;

        // Event raised when components need to be redrawn
        public event Action OnComponentsChanged;

        // Constructor
        public DesignService()
        {
            CreateNewForm();
        }

        /// <summary>
        /// Create a new empty form
        /// </summary>
        public void CreateNewForm()
        {
            CurrentForm = new FormLayout();
            SelectedComponent = null;
            OnComponentsChanged?.Invoke();
            OnFormModified?.Invoke();
        }

        /// <summary>
        /// Load an existing form
        /// </summary>
        public void LoadForm(FormLayout form)
        {
            CurrentForm = form ?? new FormLayout();
            SelectedComponent = null;
            OnComponentsChanged?.Invoke();
            OnFormModified?.Invoke();
        }

        /// <summary>
        /// Select a component
        /// </summary>
        public void SelectComponent(string componentId)
        {
            if (string.IsNullOrEmpty(componentId))
            {
                SelectedComponent = null;
            }
            else
            {
                SelectedComponent = CurrentForm.Components.FirstOrDefault(c => c.Id == componentId);
            }
            
            OnComponentSelected?.Invoke(SelectedComponent);
        }

        /// <summary>
        /// Clear component selection
        /// </summary>
        public void ClearSelection()
        {
            SelectedComponent = null;
            OnComponentSelected?.Invoke(null);
        }

        /// <summary>
        /// Add a new component to the form
        /// </summary>
        public void AddComponent(ComponentBase component)
        {
            CurrentForm.Components.Add(component);
            
            // Set the Z-index to be on top
            component.ZIndex = CurrentForm.Components.Count;
            
            OnComponentsChanged?.Invoke();
            OnFormModified?.Invoke();
            
            // Select the newly added component
            SelectComponent(component.Id);
        }

        /// <summary>
        /// Remove a component from the form
        /// </summary>
        public void RemoveComponent(string componentId)
        {
            var component = CurrentForm.Components.FirstOrDefault(c => c.Id == componentId);
            if (component != null)
            {
                CurrentForm.Components.Remove(component);
                
                // If we removed the selected component, clear the selection
                if (SelectedComponent?.Id == componentId)
                {
                    ClearSelection();
                }
                
                OnComponentsChanged?.Invoke();
                OnFormModified?.Invoke();
            }
        }

        /// <summary>
        /// Update component position
        /// </summary>
        public void UpdateComponentPosition(string componentId, int x, int y)
        {
            var component = CurrentForm.Components.FirstOrDefault(c => c.Id == componentId);
            if (component != null)
            {
                component.X = x;
                component.Y = y;
                OnComponentsChanged?.Invoke();
                OnFormModified?.Invoke();
            }
        }

        /// <summary>
        /// Update component size
        /// </summary>
        public void UpdateComponentSize(string componentId, int width, int height)
        {
            var component = CurrentForm.Components.FirstOrDefault(c => c.Id == componentId);
            if (component != null)
            {
                component.Width = Math.Max(10, width);  // Ensure minimum size
                component.Height = Math.Max(10, height);
                OnComponentsChanged?.Invoke();
                OnFormModified?.Invoke();
            }
        }

        /// <summary>
        /// Update component property
        /// </summary>
        public void UpdateComponentProperty(string componentId, string propertyName, object value)
        {
            var component = CurrentForm.Components.FirstOrDefault(c => c.Id == componentId);
            if (component != null)
            {
                if (component.Properties.ContainsKey(propertyName))
                {
                    component.Properties[propertyName] = value;
                }
                else
                {
                    component.Properties.Add(propertyName, value);
                }
                
                OnComponentsChanged?.Invoke();
                OnFormModified?.Invoke();
            }
        }

        /// <summary>
        /// Bring component to front (highest z-index)
        /// </summary>
        public void BringToFront(string componentId)
        {
            var component = CurrentForm.Components.FirstOrDefault(c => c.Id == componentId);
            if (component != null)
            {
                // Get the highest Z-index and set this component's Z-index one higher
                int highestZIndex = CurrentForm.Components.Max(c => c.ZIndex);
                component.ZIndex = highestZIndex + 1;
                
                OnComponentsChanged?.Invoke();
                OnFormModified?.Invoke();
            }
        }

        /// <summary>
        /// Send component to back (lowest z-index)
        /// </summary>
        public void SendToBack(string componentId)
        {
            var component = CurrentForm.Components.FirstOrDefault(c => c.Id == componentId);
            if (component != null)
            {
                // Get the lowest Z-index and set this component's Z-index one lower
                int lowestZIndex = CurrentForm.Components.Min(c => c.ZIndex);
                component.ZIndex = lowestZIndex - 1;
                
                OnComponentsChanged?.Invoke();
                OnFormModified?.Invoke();
            }
        }

        /// <summary>
        /// Creates a new component instance of the specified type
        /// </summary>
        public ComponentBase CreateComponent(string componentType)
        {
            return componentType switch
            {
                "Button" => new ButtonComponent(),
                "TextBox" => new TextBoxComponent(),
                "Label" => new LabelComponent(),
                "Dropdown" => new DropdownComponent(),
                "CheckBox" => new CheckBoxComponent(),
                "RadioButton" => new RadioButtonComponent(),
                "Image" => new ImageComponent(),
                "Container" => new ContainerComponent(),
                _ => throw new ArgumentException($"Unknown component type: {componentType}")
            };
        }

        /// <summary>
        /// Get a list of available component types
        /// </summary>
        public List<string> GetAvailableComponentTypes()
        {
            return new List<string>
            {
                "Button",
                "TextBox",
                "Label",
                "Dropdown",
                "CheckBox",
                "RadioButton",
                "Image",
                "Container"
            };
        }
    }
} 