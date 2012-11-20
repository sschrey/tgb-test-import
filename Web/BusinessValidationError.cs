using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Web
{
    ///<summary>
    /// An implementation of IValidator to enable server code messages to be added to a ValidationSummary
    ///</summary>
    public class BusinessValidationError : IValidator
    {
        ///<summary>
        ///Initializes a new instance of the <see cref="T:BusinessValidationError"/> class.
        ///</summary>
        ///<param name="errorMessage">The error message.</param>
        public BusinessValidationError(string errorMessage)
        {
            _ErrorMessage = errorMessage;
            _IsValid = false;
        }

        #region IValidator
        private string _ErrorMessage;
        private bool _IsValid;

        ///<summary>
        /// Gets or sets the error message.
        ///</summary>
        ///<value>The error message.</value>
        string IValidator.ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        ///<summary>
        /// When implemented by a class, gets or sets a value indicating whether the user-entered content in the specified control passes validation.
        ///</summary>
        ///<value></value>
        ///<returns>true if the content is valid; otherwise, false.</returns>
        bool IValidator.IsValid
        {
            get { return _IsValid; }
            set { _IsValid = value; }
        }

        ///<summary>
        /// When implemented by a class, evaluates the condition it checks and updates the <see cref="P:System.Web.UI.IValidator.IsValid"></see> property.
        ///</summary>
        void IValidator.Validate()
        {
        }//Validate
        #endregion
    }
}
