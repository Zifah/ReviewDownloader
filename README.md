# The Review Downloader
Is a simple website which allows users download reviews from Amazon marketplace, Google Play store and Apple app store, simply by entering the link to the review item (product or application)

**ReviewCurator** sub-directory holds a .NET class-library which contains all the logic for pulling reviews from Amazon marketplace, Google Play store and Apple app store

**ReviewDownloader** sub-directory holds a .NET web application which employs the **ReviewCurator** library and essentially exposes the power of the library via a simple UI. 

Hey, you can find a <a href="http://reviewdownloader.somee.com" target="_blank">live demo of the project here.</a>
