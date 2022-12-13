using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// USG SDK
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class InitializeUGS : MonoBehaviour
{
    // Setup Environtment Here
    private string _environment = "development";

    async void Awake()
    {
        try
        {

            var options = new InitializationOptions()
                .SetEnvironmentName(_environment);

            await UnityServices.InitializeAsync(options);
            //await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("UGS State: " + UnityServices.State);

            SignInAnonymouslyAsync();
            SetupEvents();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.Log("UGS State: " + UnityServices.State);
        }
    }

    void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () => {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

        };

        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () => {
            Debug.Log("Player signed out.");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired.");
        };
    }

    async void SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            PlayerPrefs.SetString("playerID", AuthenticationService.Instance.PlayerId);
            PlayerPrefs.SetString("playerName", "Guest");

        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }


    // Next Incase need to implement cached user login
    async Task SignInCachedUserAsync()
    {
        Debug.Log("CCHHHAEECCCEEDDDD");
        // Check if a cached user already exists by checking if the session token exists
        if (!AuthenticationService.Instance.SessionTokenExists)
        {
            // if not, then do nothing
            return;
        }

        // Sign in Anonymously
        // This call will sign in the cached user.
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException exception)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(exception);
        }
    }
}
