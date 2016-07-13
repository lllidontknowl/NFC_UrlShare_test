using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Nfc;
using System.Collections.Generic;


namespace App4
{
    [Activity(Label = "App4", MainLauncher = true, Icon = "@drawable/icon")]
    [IntentFilter(new[] { Intent.ActionSend }, Categories = new[] {
    Intent.CategoryDefault,
    Intent.CategoryBrowsable
    }, DataMimeType = "text/plain")]
    public class MainActivity : Activity
    {
        string share;

        protected override void OnCreate(Bundle bundle)
        {
            string share;
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            NfcAdapter NA = NfcAdapter.GetDefaultAdapter(this);
            if(NA!=null && NA.IsEnabled)
            {
                Toast.MakeText(this, "fatboybonbon", ToastLength.Long).Show();
            }else
            {
                Toast.MakeText(this, "fatboybon2", ToastLength.Long).Show();

            }
            // Get our button from the layout resource,
            // and attach an event to it
            EditText ET = FindViewById<EditText>(Resource.Id.edit_text);
            TextView testTV = FindViewById<TextView>(Resource.Id.text_view);
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            Intent intent = new Intent();
            share = intent.GetStringExtra(Intent.ExtraText);
            button.Click += delegate
            {
                testTV.Text = share;
            };
            testTV.Text = share;


        }

        protected override void OnPause()
        {
            base.OnPause();

            NfcManager manager = (NfcManager)GetSystemService(NfcService);
            NfcAdapter adapter = manager.DefaultAdapter;
            adapter.DisableForegroundNdefPush(this);


        }

        protected override void OnResume()
        {
            base.OnResume();
            //  Array array = new byte[NdefRecord.RtdUri.Count];
            // NdefRecord.RtdUri.CopyTo(array, 0);
            var result = new byte[NdefRecord.RtdUri.Count];
            NdefRecord.RtdUri.CopyTo(result, 0);   

            NfcManager manager = (NfcManager)GetSystemService(NfcService);
            NdefRecord record = new NdefRecord(NdefRecord.TnfAbsoluteUri,result,new byte[0], System.Text.Encoding.Default.GetBytes(share));
            manager.DefaultAdapter.EnableForegroundNdefPush(this,new NdefMessage(record));
        }
    }


}

//