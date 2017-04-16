# The Review Downloader
Is a simple website which allows users download reviews from Amazon marketplace, Google Play store and Apple app store, simply by entering the link to the review item (product or application)

The ReviewCurator sub-directory holds a .NET class-library which contains all the logic for pulling reviews from Amazon marketplace, Google Play store and Apple app store

The ReviewDownloader sub-directory holds a .NET web application which employs the ReviewCurator library and essentially exposes the power of the ReviewCurator via a very simple UI. 
