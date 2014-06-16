﻿using System;
using System.Linq;
using System.Reflection;
using Tweetinvi.Core.Enum;

namespace Tweetinvi.Core.Extensions
{
    public static class LanguageExtension
    {
        public static string GetDescriptionAttribute(this Language language)
        {
            var field = language.GetType().GetField(language.ToString());
            var descriptionAttribute = (LanguageAttribute)Attribute.GetCustomAttribute(field, typeof(LanguageAttribute));
            return descriptionAttribute != null ? descriptionAttribute.Language : language.ToString();
        }

        public static Language GetLangFromDescription(string descriptionValue)
        {
            try
            {
                if (!String.IsNullOrEmpty(descriptionValue))
                {
                    descriptionValue = descriptionValue.Substring(0, 2);
                }

                var language = typeof(Language).GetFields().First(field => IsValidDescriptionField(descriptionValue, field));
                return (Language)language.GetValue(null);
            }
            catch (Exception)
            {
                return Language.Undefined;
            }
            
        }

        private static bool IsValidDescriptionField(string descriptionValue, FieldInfo field)
        {
            var descriptionAttribute = Attribute.GetCustomAttribute(field, typeof(LanguageAttribute));

            if (descriptionAttribute == null)
            {
                return false;
            }

            return ((LanguageAttribute) descriptionAttribute).Language == descriptionValue;
        }
    }
}