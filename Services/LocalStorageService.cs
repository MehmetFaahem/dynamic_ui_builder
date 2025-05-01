using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using UIBuilderApp.Models;
using System.Text.Json.Serialization;

namespace UIBuilderApp.Services
{
    /// <summary>
    /// Service for managing forms in local storage
    /// </summary>
    public class LocalStorageService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly JsonSerializerOptions _jsonOptions;
        private const string FORM_LIST_KEY = "form_list";
        private const string FORM_PREFIX = "form_";

        public LocalStorageService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };
            _jsonOptions.Converters.Add(new ComponentConverter());
        }

        /// <summary>
        /// Get a list of all saved forms
        /// </summary>
        public async Task<List<FormMetadata>> GetFormListAsync()
        {
            return await GetAllFormMetadataAsync();
        }

        /// <summary>
        /// Get all form metadata (without full component details)
        /// </summary>
        public async Task<List<FormMetadata>> GetAllFormMetadataAsync()
        {
            try
            {
                var result = await _localStorage.GetItemAsync<List<FormMetadata>>(FORM_LIST_KEY);
                return result ?? new List<FormMetadata>();
            }
            catch
            {
                return new List<FormMetadata>();
            }
        }

        /// <summary>
        /// Save form metadata list
        /// </summary>
        public async Task SaveFormMetadataListAsync(List<FormMetadata> formMetadataList)
        {
            await _localStorage.SetItemAsync(FORM_LIST_KEY, formMetadataList);
        }

        /// <summary>
        /// Load a form by id
        /// </summary>
        public async Task<FormLayout> LoadFormAsync(string formId)
        {
            try
            {
                return await _localStorage.GetItemAsync<FormLayout>($"{FORM_PREFIX}{formId}");
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Save a form
        /// </summary>
        public async Task SaveFormAsync(FormLayout form)
        {
            form.ModifiedDate = System.DateTime.Now;
            await _localStorage.SetItemAsync($"{FORM_PREFIX}{form.Id}", form, _jsonOptions);

            // Update the metadata list
            var metadataList = await GetAllFormMetadataAsync();
            
            var existingMetadata = metadataList.Find(m => m.Id == form.Id);
            if (existingMetadata != null)
            {
                existingMetadata.Name = form.Name;
                existingMetadata.Description = form.Description;
                existingMetadata.ModifiedDate = form.ModifiedDate;
            }
            else
            {
                metadataList.Add(new FormMetadata
                {
                    Id = form.Id,
                    Name = form.Name,
                    Description = form.Description,
                    CreatedDate = form.CreatedDate,
                    ModifiedDate = form.ModifiedDate
                });
            }

            await SaveFormMetadataListAsync(metadataList);
        }

        /// <summary>
        /// Delete a form
        /// </summary>
        public async Task DeleteFormAsync(string formId)
        {
            await _localStorage.RemoveItemAsync($"{FORM_PREFIX}{formId}");

            // Update the metadata list
            var metadataList = await GetAllFormMetadataAsync();
            metadataList.RemoveAll(m => m.Id == formId);
            await SaveFormMetadataListAsync(metadataList);
        }

        /// <summary>
        /// Export a form as JSON
        /// </summary>
        public string ExportFormAsJson(FormLayout form)
        {
            return JsonSerializer.Serialize(form, _jsonOptions);
        }

        /// <summary>
        /// Import a form from JSON
        /// </summary>
        public FormLayout ImportFormFromJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<FormLayout>(json, _jsonOptions);
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Lightweight metadata about a form, without component details
    /// </summary>
    public class FormMetadata
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    }
} 