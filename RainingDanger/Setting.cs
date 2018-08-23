using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZeroLibrary;

#if ANDROID
using Android.Content;
#endif

namespace RainingDanger
{
    public class Setting
    {
        private static Setting _instance;

        public static Setting Create()
        {
            if (_instance == null)
            {
                _instance = new Setting();
            }

            return _instance;
        }

#if ANDROID
        ISharedPreferences _preferences;
#endif

        private Setting()
        {
#if ANDROID
             _preferences = Game.Activity.Cast<Context>().GetSharedPreferences("RainingDanger", FileCreationMode.Private);
#endif
        }

        //todo fix this
        public TPrimitive Get<TPrimitive>(string name) where TPrimitive : struct
        {
            TPrimitive value = default(TPrimitive);
            Type type = typeof(TPrimitive);

#if ANDROID
            if(type == typeof(int))
            {
                value = _preferences.GetInt(name, 0).Cast<TPrimitive>();
            }
            else if(type == typeof(bool))
            {
                value = _preferences.GetBoolean(name, true).Cast<TPrimitive>();
            }
            else if(type == typeof(float))
            {
                value = _preferences.GetFloat(name, 0).Cast<TPrimitive>(); 
            }
            else if(type == typeof(long))
            {
                value = _preferences.GetLong(name, 0).Cast<TPrimitive>();
            }
            else
            {
                
                throw new NotImplementedException("");
            }
#elif iOS
            if(type == typeof(int))
            {
                value = MonoTouch.Foundation.NSUserDefaults.StandardUserDefaults.IntForKey(name).Cast<TPrimitive>();
            }
            else if(type == typeof(bool))
            {
                value = MonoTouch.Foundation.NSUserDefaults.StandardUserDefaults.BoolForKey(name).Cast<TPrimitive>();
            }
            else if (type == typeof(float))
            {
                value = MonoTouch.Foundation.NSUserDefaults.StandardUserDefaults.FloatForKey(name).Cast<TPrimitive>();
            }
            else if (type == typeof(long))
            {
                value = MonoTouch.Foundation.NSUserDefaults.StandardUserDefaults.StringForKey(name).ToLong().Cast<TPrimitive>();
            }
            else
            {

                throw new NotImplementedException("");
            }

#endif

            return value;
        }

        public void Set(string key, object value, Type type)
        {
#if ANDROID
            ISharedPreferencesEditor editor = _preferences.Edit();
            
            if (type == typeof(int))
            {
                editor.PutInt(key, value.ToInt());
            }
            else if (type == typeof(bool))
            {
                editor.PutBoolean(key, value.ToBoolean());
            }
            else if (type == typeof(float))
            {
                editor.PutFloat(key, value.ToFloat());
            }
            else if(type == typeof(long))
            {
                editor.PutLong(key, value.ToLong());
            }
            else
            {
                throw new NotImplementedException("");
            }
            editor.Apply();
#elif iOS

            if (type == typeof(int))
            {
                MonoTouch.Foundation.NSUserDefaults.StandardUserDefaults.SetInt(value.ToInt(), key);
            }
            else if (type == typeof(bool))
            {
                MonoTouch.Foundation.NSUserDefaults.StandardUserDefaults.SetBool(value.ToBoolean(), key);
            }
            else if (type == typeof(float))
            {
                MonoTouch.Foundation.NSUserDefaults.StandardUserDefaults.SetFloat(value.ToFloat(), key);
            }
            else if (type == typeof(long))
            {
                MonoTouch.Foundation.NSUserDefaults.StandardUserDefaults.SetString(value.ToString(), key);
            }
            else
            {

                throw new NotImplementedException("");
            }


#endif
        }
    }
}
