﻿using NUnit.Framework;
using Xamarin.UITest.iOS;
using static Toggl.Daneel.Tests.UI.Extensions.StartTimeEntryExtensions;

namespace Toggl.Daneel.Tests.UI
{
    [TestFixture]
    public sealed class StartTimeEntryTests
    {
        private const string validEmail = "susancalvin@psychohistorian.museum";

        private iOSApp app;

        [SetUp]
        public void BeforeEachTest()
        {
            app = Configuration.GetApp();

            app.WaitForStartTimeEntryScreen();
        }

        [Test]
        public void TappingTheDoneButtonCreatesANewTimeEntry()
        {
            app.Tap(StartTimeEntry.DoneButton);

            app.WaitForElement(Main.StopTimeEntryButton);
        }

        [Test]
        public void TappingTheDoneButtonCreatesANewTimeEntryWhoseDescriptionMatchesWhatWasTypedInTheDescriptionField()
        {
            var description = "UI testing the Toggl App";

            app.EnterText(description);
            app.Tap(StartTimeEntry.DoneButton);

            app.WaitForElement(query => query.Marked(description));
        }
    }
}
