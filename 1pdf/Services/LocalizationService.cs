using System;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace _1pdf.Services
{
    /// <summary>
    /// Manages application culture and localized string retrieval.
    /// Wraps ResourceManager so forms never touch resources directly.
    /// </summary>
    public class LocalizationService
    {
        private readonly ResourceManager _resourceManager;

        public LocalizationService()
        {
            _resourceManager = new ResourceManager(
                "_1pdf.Properties.Resources",
                Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// The culture currently applied to the UI thread.
        /// </summary>
        public CultureInfo CurrentCulture => Thread.CurrentThread.CurrentUICulture;

        /// <summary>
        /// True when the current UI culture is Spanish.
        /// </summary>
        public bool IsSpanish =>
            CurrentCulture.Name.StartsWith("es", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Applies the given culture to the current thread.
        /// </summary>
        /// <param name="cultureName">Culture name, e.g. "en-US" or "es-ES".</param>
        public void SetCulture(string cultureName)
        {
            var culture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        /// <summary>
        /// Gets a localized string for the current culture.
        /// </summary>
        public string GetString(string key)
        {
            return _resourceManager.GetString(key, CurrentCulture) ?? key;
        }

        /// <summary>
        /// Gets an image resource by key, or null when not found.
        /// </summary>
        public Image GetImage(string key)
        {
            return _resourceManager.GetObject(key, CurrentCulture) as Image;
        }

        /// <summary>
        /// Gets a localized string formatted with the provided arguments.
        /// </summary>
        public string GetString(string key, params object[] args)
        {
            var template = GetString(key);
            return string.Format(template, args);
        }
    }
}
