﻿using System;

namespace Angela.Core
{
    //TODO: Consider merging this partial class.  Not much here anymore
    public partial class Angie
    {
        /// <summary>
        /// Resets Angie to default state for next generation
        /// and allows access to fluent interface.
        /// </summary>
        /// <returns></returns>
        public static AngieConfigurator Configure()
        {
            Reset();
            return new AngieConfigurator(_angie, _fillerManager);
        }

        /// <summary>
        /// Reset and configure how the specified type is populated by Angie
        /// NOTE: Overwrites all previous configuration
        /// </summary>
        /// <typeparam name="T">The target object type</typeparam>
        /// <returns>A configurator for the specified object type</returns>
        public static AngieConfigurator<T> Configure<T>() where T : new()
        {
            Reset<T>();
            return new AngieConfigurator<T>(_angie, _fillerManager);
        }

        public static AngieConfigurator Set()
        {
            return new AngieConfigurator(_angie, _fillerManager);
        }

        /// <summary>
        /// Configure how the specified type is populated by Angie.
        /// NOTE: Maintains previous configuration for the specified type.
        /// </summary>
        /// <typeparam name="T">The target object type</typeparam>
        /// <returns>A configurator for the specified object type</returns>
        public static AngieConfigurator<T> Set<T>() where T : new()
        {
            return new AngieConfigurator<T>(_angie, _fillerManager);
        }

        /// <summary>
        /// Set global defaults for Angie
        /// </summary>
        /// <returns>The Angie Defaulturator</returns>
        public static AngieDefaulturator Default()
        {
            return new AngieDefaulturator(_angie, _fillerManager);
        }

        public static void Reset()
        {
            _fillerManager.ResetFillers();
            
            _fillerManager.SetMinInt(Angie.Defaults.MIN_INT);
            _fillerManager.SetMaxInt(Angie.Defaults.MAX_INT);

            _fillerManager.SetMinShort(Angie.Defaults.MIN_SHORT);
            _fillerManager.SetMaxShort(Angie.Defaults.MAX_SHORT);
            
            _fillerManager.SetMinDecimal(Angie.Defaults.MIN_DECIMAL);
            _fillerManager.SetMaxDecimal(Angie.Defaults.MAX_DECIMAL);

            _listCount = Angie.Defaults.LIST_COUNT;

            _fillerManager.SetMinDateTime(Angie.Defaults.MIN_DATETIME);
            _fillerManager.SetMaxDateTime(Angie.Defaults.MAX_DATETIME);

            ResourceLoader.PropertyFillers.Clear();
        }

        public static void Reset<T>()
        {
            _fillerManager.ResetFillers<T>();

        }

        public void ListCount(int count)
        {
            _listCount = count;
        }
    }
}