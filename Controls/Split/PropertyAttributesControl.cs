﻿/*
    Reflexil .NET assembly editor.
    Copyright (C) 2007-2010 Sebastien LEBRETON

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

#region " Imports "
using Mono.Cecil;
using System.ComponentModel;
using System;
#endregion

namespace Reflexil.Editors
{
    /// <summary>
    /// Property attributes editor (all object readable/writeable non indexed properties)
    /// </summary>
    public partial class PropertyAttributesControl : BasePropertyAttributesControl
    {
       
        #region " Methods "
        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyAttributesControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Bind a property definition to this control
        /// </summary>
        /// <param name="mdef">Property definition to bind</param>
        public override void Bind(PropertyDefinition pdef)
        {
            base.Bind(pdef);
            if (pdef != null)
            {
                PropertyType.SelectedTypeReference = pdef.PropertyType;
                Constant.ReadStateFrom(pdef);
            }
            else
            {
                PropertyType.SelectedTypeReference = null;
                Constant.Reset();
            }
        }
        #endregion

        #region " Events "
        /// <summary>
        /// Handle text box validation
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arguments</param>
        private void PropertyType_Validating(object sender, CancelEventArgs e)
        {
            bool validated;
            if (PropertyType.SelectedTypeReference is TypeSpecification)
            {
                TypeSpecification tspec = PropertyType.SelectedTypeReference as TypeSpecification;
                validated = tspec.ElementType != null;
            }
            else
            {
                validated = PropertyType.SelectedTypeReference != null;
            }

            if (!validated)
            {
                ErrorProvider.SetError(PropertyType, "Type is mandatory");
                e.Cancel = true;
            }
            else
            {
                ErrorProvider.SetError(PropertyType, string.Empty);
                if (Item != null)
                {
                    Item.PropertyType = PropertyType.SelectedTypeReference;
                }
            }
        }

        private void Constant_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                ErrorProvider.SetError(Constant, string.Empty);
                Constant.CopyStateTo(Item);
            }
            catch (Exception)
            {
                ErrorProvider.SetError(Constant, "Unable to convert input");
                e.Cancel = true;
            }
        }
        #endregion

    }

    #region " VS Designer generic support "
    public class BasePropertyAttributesControl : SplitAttributesControl<PropertyDefinition>
    {
    }
    #endregion
}