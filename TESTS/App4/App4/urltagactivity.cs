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
using Android.Nfc;
using Xamarin.Forms;

namespace App4
{
    [Activity(Label = "urltagactivity")]
    [IntentFilter(new[] {"android.nfc.action.NDEF_DISCOVERED" }, 
    Categories = new[] {"android.intent.category.DEFAULT"}, 
    DataScheme = "http")
    ]
    public class urltagactivity : Activity
    {

        String url;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            TextView testTV = FindViewById<TextView>(Resource.Id.text_view);
            

            
            IParcelable[] rawMsgs = Intent.GetParcelableArrayExtra(NfcAdapter.ExtraNdefMessages);
            if (rawMsgs != null)
            {
                NdefMessage[] msgs = new NdefMessage[rawMsgs.Length];
                for (int i = 0; i < rawMsgs.Length; i++)
                {        
                    msgs[i] = (NdefMessage)rawMsgs[i];
                    url = new String(Encoding.Unicode.GetChars(msgs[i].GetRecords()[0].GetPayload()));
                    testTV.Text = url;
                    break;

                }                               
            }

            if (url != null)
            {
                Uri myUri = new Uri(url, UriKind.Absolute);

                Device.OpenUri(myUri);
            }

            // Create your application here
        }
    }
}