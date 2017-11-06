# Environment Variables :microscope:

While all Toggl apps are open source, not all things can be made public. This includes, but is not limited to, analytics services' keys and testing credentials. When in doubt, read the company policy before making something public.

Below is a list of all the environment variables you need to configure before building and the purposes they serve: 

- iOS analytics' services :bar_chart:
`TOGGL_AD_UNIT_ID_FOR_BANNER_TEST`
`TOGGL_AD_UNIT_ID_FOR_INTERSTITIAL_TEST`
`TOGGL_CLIENT_ID`
`TOGGL_REVERSED_CLIENT_ID`
`TOGGL_API_KEY`
`TOGGL_GCM_SENDER_ID`
`TOGGL_PROJECT_ID`
`TOGGL_STORAGE_BUCKET`
`TOGGL_GOOGLE_APP_ID`

- Running the UI tests past login :key:
`TOGGL_UI_TEST_USERNAME`
`TOGGL_UI_TEST_PASSWORD`

# new 'puter who dis? :computer:

To configure this on a macOS:

```
$ touch ~/.bash_profile
$ open ~/.bash_profile
```

Then add the following lines to `.bash_profile`:
```
export TOGGL_UI_TEST_USERNAME=""
export TOGGL_UI_TEST_PASSWORD=""
export TOGGL_AD_UNIT_ID_FOR_BANNER_TEST=""
export TOGGL_AD_UNIT_ID_FOR_INTERSTITIAL_TEST=""
export TOGGL_CLIENT_ID=""
export TOGGL_REVERSED_CLIENT_ID=""
export TOGGL_API_KEY=""
export TOGGL_GCM_SENDER_ID=""
export TOGGL_PROJECT_ID=""
export TOGGL_STORAGE_BUCKET=""
export TOGGL_GOOGLE_APP_ID=""
export TOGGL_DATABASE_URL=""
```

Finish it off with:

```
$ source ~/.bash_profile
```
