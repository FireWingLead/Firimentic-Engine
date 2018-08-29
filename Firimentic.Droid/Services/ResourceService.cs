using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using FirimenticEngine.Services;
using Java.IO;
using Android.Content.Res;
using System.IO;

namespace Firimentic.Droid.Services
{
    class ResourceService : IResourceService
    {
        AssetManager assets;


        internal ResourceService(AssetManager assets) {
            this.assets = assets;
        }

        public void Dispose() {
            //assets.Dispose(); //This is done by the context that originally gave us the AssetManager.
            assets = null;
        }


        public string GetTextFileResource(string resourceName) {
            string content;
            using (StreamReader reader = new StreamReader(assets.Open(resourceName))) {
                content = reader.ReadToEnd();
            }
            return content;
        }
    }
}