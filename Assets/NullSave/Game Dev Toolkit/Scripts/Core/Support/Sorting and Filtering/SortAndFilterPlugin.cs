using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    public class SortAndFilterPlugin : UniversalPlugin
    {

        #region Fields

        [HideInInspector] public SimpleEvent requiresUpdate;

        #endregion

        #region Properties

        public virtual string[] compatibleListTypes { get; }

        public virtual object filterBy { get; set; }

        public virtual bool isEnabled { get; }

        public virtual object sortBy { get; set; }

        #endregion

        #region Public Methods

        public virtual void SortAndFilter<T>(List<T> list) { }

        #endregion

    }
}