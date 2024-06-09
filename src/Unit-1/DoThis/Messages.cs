using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.Buffers.Text;

namespace WinTail
{
    class Messages
    {
        #region Neutral/system messages
        /// <summary>
        /// Marker class to continue processing.
        /// </summary>
        public class ContinueProcessing { }
        #endregion


        #region Success messages

        /// <summary>
        /// Base class for signalling that user input was valid.
        /// </summary>
        public class InputSuccess
        {
            public InputSuccess(string reason)
            {
                Reason = reason; 
            }
            public string Reason { get; private set; }
        }
        #endregion


        #region Error messages

        /// <summary>
        /// Base class for signalling that user input is invalid
        /// </summary>
        public class InputError
        {
            public InputError(string reason)
            {
                Reason = reason; 
            }

            public string Reason { get; private set; }
        }

        /// <summary>
        /// User provided blank input
        /// </summary>
        public class NullInputError : InputError
        {
            public NullInputError(string reason) : base(reason)
            {

            }
        }

        /// <summary>
        /// User privided invalid input (currently, input w/ odd # chars)
        /// </summary>
        public class ValidationError : InputError
        {
            public ValidationError(string reason) : base(reason)
            {

            }
        }

        #endregion

    }

}