using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UIBuilderApp.Models
{
    /// <summary>
    /// Represents the entire form layout including all components
    /// </summary>
    public class FormLayout
    {
        /// <summary>
        /// Unique identifier for the form layout
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Name of the form
        /// </summary>
        public string Name { get; set; } = "Untitled Form";

        /// <summary>
        /// Description of the form
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// Date when the form was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Date when the form was last modified
        /// </summary>
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// The width of the canvas
        /// </summary>
        public int CanvasWidth { get; set; } = 1024;

        /// <summary>
        /// The height of the canvas
        /// </summary>
        public int CanvasHeight { get; set; } = 768;

        /// <summary>
        /// List of components in the form
        /// </summary>
        public List<UIComponentBase> Components { get; set; } = new List<UIComponentBase>();

        /// <summary>
        /// Additional form metadata
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Custom form settings
        /// </summary>
        public Dictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Create a deep copy of the current form layout
        /// </summary>
        /// <returns>New FormLayout instance with same data</returns>
        public FormLayout? Clone()
        {
            // Using JSON serialization to perform a deep copy
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions 
            { 
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve 
            });
            return JsonSerializer.Deserialize<FormLayout>(json, new JsonSerializerOptions 
            { 
                ReferenceHandler = ReferenceHandler.Preserve 
            });
        }
    }
} 