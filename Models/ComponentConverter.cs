using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UIBuilderApp.Models
{
    /// <summary>
    /// JSON converter for handling polymorphic serialization/deserialization of ComponentBase types
    /// </summary>
    public class ComponentConverter : JsonConverter<ComponentBase>
    {
        /// <summary>
        /// Read the JSON and convert it to the appropriate component type
        /// </summary>
        public override ComponentBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // We'll keep the original reader state and then clone a new reader
            // to read ahead and determine the component type
            var readerClone = reader;

            // We need to read the JSON as we look for the ComponentType property
            while (readerClone.Read())
            {
                if (readerClone.TokenType == JsonTokenType.PropertyName && 
                    readerClone.GetString() == "ComponentType")
                {
                    // Move to the value
                    readerClone.Read();
                    var componentType = readerClone.GetString();

                    // Create the appropriate component type based on ComponentType value
                    ComponentBase component = componentType switch
                    {
                        "Button" => new ButtonComponent(),
                        "TextBox" => new TextBoxComponent(),
                        "Label" => new LabelComponent(),
                        "Dropdown" => new DropdownComponent(),
                        "CheckBox" => new CheckBoxComponent(),
                        "RadioButton" => new RadioButtonComponent(),
                        "Image" => new ImageComponent(),
                        "Container" => new ContainerComponent(),
                        _ => throw new JsonException($"Unknown component type: {componentType}")
                    };

                    // Now deserialize using the original reader into our created object
                    return JsonSerializer.Deserialize<JsonElement>(ref reader, options).Deserialize(component.GetType(), options) as ComponentBase;
                }
            }

            throw new JsonException("Could not find ComponentType property in JSON");
        }

        /// <summary>
        /// Write the component to JSON with its specific type
        /// </summary>
        public override void Write(Utf8JsonWriter writer, ComponentBase value, JsonSerializerOptions options)
        {
            // Simply serialize the actual component type
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
} 