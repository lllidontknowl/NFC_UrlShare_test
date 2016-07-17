using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Nfc;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App4
{
    [Activity(Label = "App4", MainLauncher = false, Icon = "@drawable/icon",LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
    [IntentFilter(new[] { Intent.ActionSend , NfcAdapter.ActionNdefDiscovered }, Categories = new[] {
    Intent.CategoryDefault,
    Intent.CategoryBrowsable
    }, DataMimeType = "text/plain")]
    public class MainActivity : Activity
    {
        string share;
        PendingIntent mPendingIntent;
        IntentFilter ndefDetected;
        IntentFilter[] intentF;
        TextView testTV;
        protected override void OnCreate(Bundle bundle)
        {
            
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);  

            Intent Myintent = new Intent(this, GetType());
            Myintent.SetFlags(ActivityFlags.SingleTop);
            mPendingIntent = PendingIntent.GetActivity(this, 0, Myintent, 0);
            ndefDetected = new IntentFilter(NfcAdapter.ActionNdefDiscovered);
            try
            {
                ndefDetected.AddDataType("text/plain");
                ndefDetected.AddCategory(Intent.CategoryDefault);
            }
            catch { };

            intentF = new IntentFilter[] { ndefDetected };
            NfcAdapter NA = NfcAdapter.GetDefaultAdapter(this);   
          
            if (NA!=null && NA.IsEnabled)
            {
                Toast.MakeText(this, "Nfc Found", ToastLength.Long).Show();
            }else
            {
                Toast.MakeText(this, "Nfc Not Found", ToastLength.Long).Show();
            }
            testTV = FindViewById<TextView>(Resource.Id.text_view);
            share = Intent.GetStringExtra(Intent.ExtraText);  
            testTV.Text = share;   
    }
        
        protected override void OnPause()
        {
            base.OnPause();

            NfcManager manager = (NfcManager)GetSystemService(NfcService);
            NfcAdapter adapter = manager.DefaultAdapter;
            adapter.DisableForegroundNdefPush(this);
            adapter.DisableForegroundDispatch(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
       

            var result2 = new byte[NdefRecord.RtdText.Count];
            NdefRecord.RtdUri.CopyTo(result2, 0);

            NfcManager manager = (NfcManager)GetSystemService(NfcService);
            NdefRecord record = new NdefRecord(NdefRecord.TnfAbsoluteUri,new byte[0], new byte[0], System.Text.Encoding.Default.GetBytes(share));
            manager.DefaultAdapter.EnableForegroundNdefPush(this, new NdefMessage(record));
            manager.DefaultAdapter.EnableForegroundDispatch(this, mPendingIntent, intentF, null);
        }

        protected void onNewIntent(Intent intent)
        {
   
            base.OnNewIntent(intent);
            testTV.Text = "onNewIntent";
            /*
            String url=null;


            IParcelable[] rawMsgs = Intent.GetParcelableArrayExtra(NfcAdapter.ExtraNdefMessages);
            if (rawMsgs != null)
            {
                NdefMessage[] msgs = new NdefMessage[rawMsgs.Length];
                for (int i = 0; i < rawMsgs.Length; i++)
                {
                    msgs[i] = (NdefMessage)rawMsgs[i];
                    url = new String(Encoding.Unicode.GetChars(msgs[i].GetRecords()[0].GetPayload()));
                    testTV.Text = url;
                }
            }

            if (url != null)
            {
                Uri myUri = new Uri(url, UriKind.Absolute);
                Device.OpenUri(myUri);
            }*/
        }

    }
}

//