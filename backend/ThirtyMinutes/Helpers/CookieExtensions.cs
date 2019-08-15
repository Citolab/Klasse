using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ThirtyMinutes.Helpers
{
    public static class CookieExtensions
    {
        /// <summary>
        /// Get, decrypt (if <see cref="DataProtectionProvider"/> is not null) and deserialize cookie.
        /// </summary>
        /// <param name="cookies"></param>
        /// <param name="key"></param>
        /// <param name="dataProtectionProvider">Optional <see cref="DataProtectionProvider"/> to use.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>(this IRequestCookieCollection cookies, string key,
            IDataProtectionProvider dataProtectionProvider = null
        )
        {
            string resultText;
            if (!cookies.TryGetValue(key, out var value))
            {
                return default(T);
            }

            if (dataProtectionProvider != null)
            {
                var protector = dataProtectionProvider.CreateProtector(Strings.SessionDataPurpose);
                resultText = protector.Unprotect(value);
            }
            else
            {
                resultText = value;
            }

            return JsonConvert.DeserializeObject<T>(resultText);
        }

        /// <summary>
        /// Encrypt (if <see cref="DataProtectionProvider"/> is not null), serialize and set cookie.
        /// </summary>
        /// <param name="cookies"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dataProtectionProvider">Optional <see cref="DataProtectionProvider"/> to use.</param>
        /// <param name="expireTimeInMinutes">Optional expire time for the cookie. Defaults to 60 minutes.</param>
        /// <typeparam name="T"></typeparam>
        public static void Set<T>(this IResponseCookies cookies, string key, T value,
            IDataProtectionProvider dataProtectionProvider = null, int expireTimeInMinutes = 60)
        {
            var options = new CookieOptions
                {Expires = DateTimeOffset.Now.AddMinutes(expireTimeInMinutes), HttpOnly = true};

            string cookieValue;
            if (dataProtectionProvider != null)
            {
                var protector = dataProtectionProvider.CreateProtector(Strings.SessionDataPurpose);
                var valueJson = JsonConvert.SerializeObject(value);
                cookieValue = protector.Protect(valueJson);
            }
            else
            {
                cookieValue = JsonConvert.SerializeObject(value);
            }

            cookies.Append(key, cookieValue, options);
        }
    }
}