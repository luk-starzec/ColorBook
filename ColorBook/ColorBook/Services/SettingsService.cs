using ColorBook.Models;
using Microsoft.JSInterop;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ColorBook.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly Settings defaultSettings;
        private readonly IJSRuntime js;
        private Settings currentSettings = null;

        public SettingsService(IJSRuntime js)
        {
            this.js = js;

            defaultSettings = new Settings
            {
                LightBackgroundColor = "#FFFFFF",
                DarkBackgroundColor = "#000000",
                LightTextColor = "#FFFFFF",
                DarkTextColor = "#333333"
            };

            //var s = js.InvokeAsync<Settings[]>($"localDataStore.get", "settings", "current");

            //currentSettings = new Settings
            //{
            //    LightBackgroundColor = "#FFFFFF",
            //    DarkBackgroundColor = "#000000",
            //    LightTextColor = "#FFFFFF",
            //    DarkTextColor = "#333333"
            //};
            this.js = js;
        }

        public async Task<Settings> GetCurrentSettingsAsync()
        {
            if (currentSettings == null)
            {
                var aaa = await js.InvokeAsync<string>($"localDataStore.get", "settings", "current");
                Console.WriteLine(aaa);
                var bbb = await js.InvokeAsync<object>($"localDataStore.get", "settings", "current");
                Console.WriteLine(bbb);
                try
                {
                    var ccc = await js.InvokeAsync<Settings>($"localDataStore.get", "settings", "current");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
                //var options = new JsonSerializerSettings
                //{
                //    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                //};
                currentSettings = JsonSerializer.Deserialize<Settings>(aaa);
                //currentSettings = await js.InvokeAsync<Settings>($"localDataStore.get", "settings", "current");

                if (currentSettings == null)
                    currentSettings = new Settings
                    {
                        LightBackgroundColor = "#FFFFFF",
                        DarkBackgroundColor = "#000000",
                        LightTextColor = "#FFFFFF",
                        DarkTextColor = "#333333"
                    };
            }
            return currentSettings;
        }

        public Settings GetDefaultSettings() => defaultSettings;

        public void RestoreDefaultSettings()
        {
            currentSettings.LightBackgroundColor = defaultSettings.LightBackgroundColor;
            currentSettings.DarkBackgroundColor = defaultSettings.DarkBackgroundColor;
            currentSettings.LightTextColor = defaultSettings.LightTextColor;
            currentSettings.DarkTextColor = defaultSettings.DarkTextColor;
        }

        public async Task SaveSettingsAsync()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(currentSettings);
            //var options = new JsonSerializerSettings
            //{
            //    ContractResolver = new CamelCasePropertyNamesContractResolver(),
            //};
            //var json = JsonConvert.SerializeObject(currentSettings, options);
            //await js.InvokeVoidAsync($"localDataStore.test", json);
            //return PutAsync("metadata", DateTime.Now, "tst");   

            //await js.InvokeVoidAsync($"localDataStore.putFromJson", "settings", json);  
            await js.InvokeVoidAsync($"localDataStore.put", "settings", "current", json);
            //await js.InvokeVoidAsync($"localDataStore.put", "settings", "DarkBackgroundColor", currentSettings.DarkBackgroundColor);          
            //await js.InvokeVoidAsync($"localDataStore.put", "settings", "LightBackgroundColor", currentSettings.LightBackgroundColor);          
        }

        public async Task SaveSettingsAsync(Settings settings)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(settings);
            Console.WriteLine(json);
            //var options = new JsonSerializerSettings
            //{
            //    ContractResolver = new CamelCasePropertyNamesContractResolver(),
            //};
            //var json = JsonConvert.SerializeObject(settings, options);
            await js.InvokeVoidAsync($"localDataStore.put", "settings", "current", json);
            await js.InvokeVoidAsync($"localDataStore.test");
        }

        public ValueTask SaveTestResult(string result)
        {
            //return PutAsync("testsresults", "pierwszy", result);
            return PutAsync("metadata", DateTime.Now, result);
        }

        ValueTask PutAsync<T>(string storeName, object key, T value)
            => js.InvokeVoidAsync($"localDataStore.put", storeName, key, value);

        //ValueTask PutAllAsync<T>(string storeName, T value)
        //{
        //    var settings = new JsonSerializerSettings
        //    {
        //        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        //    };
        //    var json = JsonConvert.SerializeObject(value, settings);

        //    return js.InvokeVoidAsync($"localDataStore.putAllFromJson", storeName, json);
        //}

    }
}
