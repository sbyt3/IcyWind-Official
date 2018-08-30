using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using IcyWind.Core.Logic.Riot.Lobby;
using IcyWind.Core.Pages;
using IcyWind.Core.Pages.IcyWindPages;
using IcyWind.Core.Pages.IcyWindPages.PlayPage;

namespace IcyWind.Core.Logic
{
    public static class UserInterfaceCore
    {
        /// <summary>
        ///     The main view. This is how the entire thing works
        /// </summary>
        public static HolderPage HolderPage;

        /// <summary>
        ///     The main view. This is how the entire thing works
        /// </summary>
        public static MainPage MainPage;


        public static Type SelectedMainPage;

        /// <summary>
        ///     All of the loaded usercontrols
        /// </summary>
        internal static readonly Dictionary<Type, UserControl> TypeControls = new Dictionary<Type, UserControl>();

        /// <summary>
        ///     The language resource dictionary
        /// </summary>
        public static ResourceDictionary MainLanguage = new ResourceDictionary();

        public static ChampionSelectPage ChampPage;

        internal static Action Focus;
        internal static Action Flash;

        /// <summary>
        ///     Changes the view
        /// </summary>
        /// <param name="control">The type of the control
        ///     <example>LoginControl</example>
        /// </param>
        /// <param name="args">Any args required to change to that control</param>
        public static void ChangeView(Type control, params object[] args)
        {
            if (!TypeControls.ContainsKey(control))
            {
                TypeControls.Add(control, (UserControl)Activator.CreateInstance(control, args));
            }

            if (control == typeof(MainPage))
            {
                var page = (MainPage) TypeControls[control];
                page.Load();
                HolderPage.Main.Content = page;
                return;
            }
            HolderPage.Main.Content = TypeControls[control];
        }

        /// <summary>
        ///     Changes the view
        /// </summary>
        /// <param name="control">The type of the control
        ///     <example>LoginControl</example>
        /// </param>
        /// <param name="args">Any args required to change to that control</param>
        public static void ChangeMainPageView<T>(params object[] args)
        {
            if (typeof(T) == typeof(ChampionSelectPage))
            {
                if (ChampPage == null)
                {
                    ChampPage = new ChampionSelectPage((PartyPhaseMessage)args[0]);
                }
                MainPage.ContentContainer.Content = ChampPage;
                return;
            }

            if (!TypeControls.ContainsKey(typeof(T)))
            {
                if (typeof(T) == typeof(LobbyPage))
                {
                    TypeControls.Add(typeof(T), (UserControl)Activator.CreateInstance(typeof(T)));
                    (TypeControls[typeof(T)] as LobbyPage)?.Load((int)args[0]);
                    MainPage.ContentContainer.Content = TypeControls[typeof(T)];
                    SelectedMainPage = typeof(T);
                    return;
                }
                TypeControls.Add(typeof(T), (UserControl)Activator.CreateInstance(typeof(T), args));
            }

            MainPage.ContentContainer.Content = TypeControls[typeof(T)];
            SelectedMainPage = typeof(T);

            if (typeof(T) == typeof(LobbyPage))
            {
                (TypeControls[typeof(T)] as LobbyPage)?.Load((int)args[0]);
            }
        }

        /// <summary>
        ///     Sets the application language
        /// </summary>
        /// <param name="language">The language you want set</param>
        public static void SetResource(string language)
        {
            MainLanguage.Source = new Uri($"pack://application:,,,/IcyWind.Languages;component/{language}.xaml");
        }

        /// <summary>
        ///     Turns a short name into the full text
        /// </summary>
        /// <param name="shortName">The short name from the resource dictionary</param>
        /// <returns>The text in the correct language</returns>
        public static string ShortNameToString(string shortName)
        {
            return (string) MainLanguage[shortName];
        }
    }
}