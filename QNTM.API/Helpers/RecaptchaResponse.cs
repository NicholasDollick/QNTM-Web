using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QNTM.API.Helpers
{
    public class RecaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        
        [JsonProperty("error-codes")]
        public IEnumerable<string> ErrorCodes { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTime ChallengeTs { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        public static async Task<bool> Validate(string encodedResponse)
        {
            if (string.IsNullOrEmpty(encodedResponse))
                return false;
            
            var secret = "";

            if (string.IsNullOrEmpty(secret))
                return false;
            
            using (var client = new System.Net.WebClient())
            {
                var reply = await client.DownloadStringTaskAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={encodedResponse}");
                return await Task.Run( () => JsonConvert.DeserializeObject<RecaptchaResponse>(reply).Success);
            }

        }
    }
}