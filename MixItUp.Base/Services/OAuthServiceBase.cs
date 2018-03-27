﻿using Mixer.Base;
using Mixer.Base.Model.OAuth;
using Mixer.Base.Services;
using Mixer.Base.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace MixItUp.Base.Services
{
    public abstract class OAuthServiceBase : RestServiceBase
    {
        protected OAuthTokenModel token;

        protected string baseAddress;

        protected OAuthServiceBase(string baseAddress) { this.baseAddress = baseAddress; }

        protected OAuthServiceBase(string baseAddress, OAuthTokenModel token) : this(baseAddress) { this.token = token; }

        public OAuthTokenModel GetOAuthTokenCopy()
        {
            if (this.token != null)
            {
                return new OAuthTokenModel()
                {
                    clientID = this.token.clientID,
                    authorizationCode = this.token.authorizationCode,
                    refreshToken = this.token.refreshToken,
                    accessToken = this.token.accessToken,
                    expiresIn = this.token.expiresIn
                };
            }
            return null;
        }

        protected async Task<string> ConnectViaOAuthRedirect(string oauthPageURL)
        {
            OAuthHttpListenerServer oauthServer = new OAuthHttpListenerServer(MixerConnection.DEFAULT_OAUTH_LOCALHOST_URL, loginSuccessHtmlPageFilePath: "LoginRedirectPage.html");
            oauthServer.Start();

            ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = oauthPageURL, UseShellExecute = true };
            Process.Start(startInfo);

            string authorizationCode = await oauthServer.WaitForAuthorizationCode();
            oauthServer.End();

            return authorizationCode;
        }

        protected override string GetBaseAddress() { return this.baseAddress; }

        protected override async Task<OAuthTokenModel> GetOAuthToken(bool autoRefreshToken = true)
        {
            if (autoRefreshToken && this.token != null && this.token.ExpirationDateTime < DateTimeOffset.Now)
            {
                await this.RefreshOAuthToken();
            }
            return this.token;
        }

        protected async Task<OAuthTokenModel> GetWWWFormUrlEncodedOAuthToken(string endpoint, string clientID, string clientSecret, List<KeyValuePair<string, string>> bodyContent)
        {
            string authorizationValue = string.Format("{0}:{1}", clientID, clientSecret);
            byte[] authorizationBytes = System.Text.Encoding.UTF8.GetBytes(authorizationValue);
            authorizationValue = Convert.ToBase64String(authorizationBytes);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + authorizationValue);
                using (var content = new FormUrlEncodedContent(bodyContent))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await client.PostAsync(endpoint, content);
                    return await this.ProcessResponse<OAuthTokenModel>(response);
                }
            }
        }

        protected abstract Task RefreshOAuthToken();
    }
}
