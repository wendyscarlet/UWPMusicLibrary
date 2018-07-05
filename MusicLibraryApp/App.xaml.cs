using MusicLibraryApp.VoiceCommandObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MusicLibraryApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();

                // Cortana voice commands.
                // Install Voice Command Definition (VCD) file.
                try
                {
                    // Install the main VCD on launch to ensure 
                    // most recent version is installed.
                    StorageFile vcdStorageFile =
                        await Package.Current.InstalledLocation.GetFileAsync(
                            @"VoiceCommandObjects\VoiceCommandDefinitions.xml");

                    await VoiceCommandDefinitionManager
                        .InstallCommandDefinitionsFromStorageFileAsync(vcdStorageFile);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(
                        "Installing Voice Commands Failed: " + ex.ToString());
                }
            }
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            Type navigationToPageType = null;
            CortanaCommands navCommand = null;

            if (args.Kind == ActivationKind.VoiceCommand)
            {
                VoiceCommandActivatedEventArgs cmd = args as VoiceCommandActivatedEventArgs;
                SpeechRecognitionResult speechRecognitionResult = cmd.Result;
                string commandName = speechRecognitionResult.RulePath[0];
                string textSpoken = speechRecognitionResult.Text;

                // commandMode indicates whether the command was entered using speech or text.
                // Apps should respect text mode by providing silent (text) feedback.
                string commandMode = this.SemanticInterpretation("commandMode", speechRecognitionResult);
                navCommand = new CortanaCommands
                {
                    CommandMode = commandMode,
                    VoiceCommandName = commandName,
                    TextSpoken = textSpoken
                };
                navigationToPageType = typeof(MainPage);
            }
            // Protocol activation occurs when a user is clicked within Cortana (using a background task).
            else if (args.Kind == ActivationKind.Protocol)
            {
                // No background service at this time.
                navigationToPageType = typeof(MainPage);
            }
            else
            {
                // If we were launched via any other mechanism, fall back to the main page view.
                // Otherwise, we'll hang at a splash screen.
                navigationToPageType = typeof(MainPage);
            }

            // Repeat the same basic initialization as OnLaunched() above, 
            // taking into account whether or not the app is already active.
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                if (args.PreviousExecutionState != ApplicationExecutionState.Running &&
                    args.PreviousExecutionState != ApplicationExecutionState.Suspended)
                {
                    //TODO: Load state from previously suspended application
                }
                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }
            // Since we're expecting to always show the home page, navigate even if 
            // a content frame is in place (unlike OnLaunched).
            //navigationToPageType = typeof(MainPage);
            rootFrame.Navigate(navigationToPageType, navCommand);

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Returns the semantic interpretation of a speech result. 
        /// Returns null if there is no interpretation for that key.
        /// </summary>
        /// <param name="interpretationKey">The interpretation key.</param>
        /// <param name="speechRecognitionResult">The speech recognition result to get the semantic interpretation from.</param>
        /// <returns></returns>
        private string SemanticInterpretation(string interpretationKey, SpeechRecognitionResult speechRecognitionResult)
        {
            return speechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
