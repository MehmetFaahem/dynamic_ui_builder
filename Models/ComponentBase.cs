using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UIBuilderApp.Models
{
    /// <summary>
    /// Base class for all UI components
    /// </summary>
    public abstract class UIComponentBase
    {
        /// <summary>
        /// Unique identifier for the component
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Component type name
        /// </summary>
        public abstract string ComponentType { get; }

        /// <summary>
        /// Component properties (style, attributes, etc.)
        /// </summary>
        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// The X position of the component on the canvas
        /// </summary>
        public int X { get; set; } = 0;

        /// <summary>
        /// The Y position of the component on the canvas
        /// </summary>
        public int Y { get; set; } = 0;

        /// <summary>
        /// The width of the component
        /// </summary>
        public int Width { get; set; } = 100;

        /// <summary>
        /// The height of the component
        /// </summary>
        public int Height { get; set; } = 40;

        /// <summary>
        /// The Z-index for layering
        /// </summary>
        public int ZIndex { get; set; } = 0;

        /// <summary>
        /// Constructor with optional id parameter
        /// </summary>
        public UIComponentBase(string id = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Id = id;
            }
        }
    }
} 