﻿using Frontends.Models.Hyperspin;
using System.Threading.Tasks;

namespace Horsesoft.Frontends.Helper.Settings
{
    public interface ISettings
    {
        Task<bool> DeserializeSettingsAsync(string systemName = null);

        void SerializeSettings();

        SystemSetting Settings { get; set; }
    }
}
