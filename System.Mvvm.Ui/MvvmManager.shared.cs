﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace System.Mvvm.Ui
{
    public partial class MvvmManager
    {
        /// <summary>
        /// Common initialization
        /// </summary>
        private static void CommonInit()
        {
            PlatformInit();

            //Centralised management of errors notifications through the ViewModel
            ViewModel.OnErrorOccured += async (object sen, Exception e2) =>
            {
                if (e2 is TitledException)
                {
                    var ex = e2 as TitledException;

                    await UI.ShowAlertAsync(ex.Title, e2.Message);
                }
                else
                {

                    await UI.ShowAlertAsync("System Error", e2.Message);
                }

            };

        }

        /// <summary>
        /// Initializes the MvvmManager
        /// </summary>
        public static void Init()
        {
            CommonInit();

            UI.Init<PlatformUIProvider>(Assembly.GetCallingAssembly(), typeof(MvvmManager).Assembly);
        }

        /// <summary>
        /// Initializes the MvvmManager with the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to register</param>
        public static void Init(params Assembly[] assemblies)
        {
            CommonInit();

            var newAssms = new List<Assembly>()
            {
                typeof(MvvmManager).Assembly,
            };

            if (!assemblies.Contains(Assembly.GetCallingAssembly()))
                newAssms.Add(Assembly.GetCallingAssembly());

            newAssms.AddRange(assemblies);

            UI.Init<PlatformUIProvider>(newAssms.ToArray());

            Services.Init(newAssms.ToArray());
        }

        /// <summary>
        /// Initializes the MvvmManager with the specified types
        /// </summary>
        /// <param name="types">Types from each assembly to register</param>
        public static void Init(params Type[] types)
        {
            CommonInit();

            var typesList = new List<Type>()
            {
                typeof(MvvmManager),
            };

            if (Assembly.GetCallingAssembly().GetTypes().Any())
            {
                var eTypes = Assembly.GetCallingAssembly().GetTypes();

                foreach (var aType in eTypes)
                {
                    if (!typesList.Contains(aType) && !types.Contains(aType))
                    {
                        typesList.Add(aType);
                        break;
                    }
                }
                
            }
                
            typesList.AddRange(types);

            UI.Init<PlatformUIProvider>(typesList.ToArray());
            Services.Init(typesList.ToArray());
        }

    }
}
